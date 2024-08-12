#pragma warning disable xUnit1013 // Public method should be marked as test
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using AssetManagementProject.web.Api;
using AssetManagementProject.web.Api.Middlewares;
using AssetManagementProject.web.Domain;
using IdentityModel.Client;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;
using static JWT.Controllers.TokenController;

/// <summary>
/// Designed by BlastAsia, Inc.(c) 2019-2021
/// https://www.blastasia.com/
/// 
/// NOTE: Tests are not working with InMemory database.
///       Must update database connection in appsettings.json - "AssetManagementProject.webDB".  
///       Initial database and tables will be created and seeded once during tests startup
///
///
/// AUTHENTICATION:
/// This test drives which authentication/authorization mechanism is used.
/// Update appsettings.json to switch between 
/// "UseIdentityServer4": false = uses embeded JWT authentication 
/// "UseIdentityServer4": true  =  uses IdentityServer 4
/// IMPORTANT: Before run IS4 test must build the solution and run once solution with IdentityServer as startup project
///            After you get the start page for IdentityServer4 you can stop run and run unit tests 
/// </summary>

namespace AssetManagementProject.web.Test
{
    public class BaseTest
    {
        public static bool RemoteService = false;  //true to use service deployed to remote server
        public static string UserName = "my@email.com";
        public static string Password = "mysecretpassword123"; //encoded in User table
    }

    //https://xunit.net/docs/shared-context#collection-fixture
    /// <summary>
    /// Initialize Http client for testing for all test classes
    /// </summary>
    public class HttpClientFixture : IDisposable
    {
        public HttpClient Client { get; private set; }
        public HttpClientFixture()
        {
            var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile("appsettings.json").Build();
            //overwrite if azure db test
            if (BaseTest.RemoteService)
            {
                configuration = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                           .AddJsonFile("appsettings-remote.json").Build();
            }

            IWebHostBuilder whb = new WebHostBuilder().UseStartup<Startup>();
            whb.UseConfiguration(configuration);
            var server = new TestServer(whb);
            Client = server.CreateClient();

        }
        public void Dispose()
        {
            Client.Dispose();
        }
    }

    [CollectionDefinition("HttpClient collection")]
    public class HttpClientCollection : ICollectionFixture<HttpClientFixture>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }

    public class Token
    {
        public string token;
    }

    [Collection("HttpClient collection")]
    public class TokenTest : BaseTest
    {
        static HttpClient Client;
        public HttpClientFixture fixture;

        public TokenTest(HttpClientFixture fixture)
        {
            this.fixture = fixture;
            Client = fixture.Client;
        }

        public static string TokenValue { get; set; }


        /// <summary>
        /// This test drives which authentication/authorization mechanism is used.
        /// Update appsettings.json to switch between 
        /// "UseIdentityServer4": false = uses embeded JWT authentication 
        /// "UseIdentityServer4": true  =  uses IdentityServer 4
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task token_test()
        {
            await token_get(null);
        }
        public static async Task token_get(HttpClient client)
        {
            if (client == null)
                client = Client;
            if (!String.IsNullOrEmpty(TokenValue)) return;

            //read from tests settings
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json").Build();

            //JWT or IS4 authentication
            if (configuration["Authentication:UseIdentityServer4"] == "False")
            { //JWT
                LoginModel login = new LoginModel { Username = UserName, Password = Password };
                var response = await client.PostAsync("/api/token", new StringContent(JsonConvert.SerializeObject(login), Encoding.UTF8, "application/json"));
                response.EnsureSuccessStatusCode();
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                var jsonString = await response.Content.ReadAsStringAsync();
                var token = JsonConvert.DeserializeObject<Token>(jsonString);
                TokenValue = token.token;
            }
            else
            { //IS4

                var is4ip = configuration["Authentication:IdentityServer4IP"];
                var is4token = is4ip + "/connect/token";

                //// request the token from the Auth server for type ClientCredentials
                //var tokenClient1 = new TokenClient(discoveryClient.TokenEndpoint, "clientCred", "secret");
                //var response1 = await tokenClient1.RequestClientCredentialsAsync("AssetManagementProject.web");
                //var resp1 = response1.Json;


                //BAD client test
                var response = LoginUsingIdentityServer(is4token, "AssetManagementProject.web-BAD", "secret", "read-write", "my@email.com", "mysecretpassword123");
                var response_json = response.AccessToken;
                if (response.IsError)
                {
                    Console.WriteLine(response.Error);
                    Console.WriteLine(response.ErrorDescription);
                }
                Assert.True(response.IsError);
                Assert.Equal("invalid_client", response.Error);
                Assert.Equal(HttpStatusCode.BadRequest, response.HttpStatusCode);

                //BAD grant test
                response = LoginUsingIdentityServer(is4token, "AssetManagementProject.webClient", "secret", "read-write", "my@email.com", "mysecretpassword123-BAD");
                response_json = response.AccessToken;
                if (response.IsError)
                {
                    Console.WriteLine(response.Error);
                    Console.WriteLine(response.ErrorDescription);
                }
                Assert.True(response.IsError);
                Assert.Equal("invalid_grant", response.Error);
                Assert.Equal(HttpStatusCode.BadRequest, response.HttpStatusCode);

                //GOOD TEST----------------
                //use your own user list (from database) to get a token for API user
                response = LoginUsingIdentityServer(is4token, "AssetManagementProject.webClient", "secret", "read-write", "my@email.com", "mysecretpassword123");
                response_json = response.AccessToken;
                if (response.IsError)
                {
                    Console.WriteLine(response.Error);
                    Console.WriteLine(response.ErrorDescription);
                }
                Assert.False(response.IsError);
                Assert.Equal(HttpStatusCode.OK, response.HttpStatusCode);
                var jsonString = response.AccessToken;
                var token = new Token(); token.token = jsonString;
                TokenValue = token.token;
            }
        }

        public static TokenResponse LoginUsingIdentityServer(string is4ip, string clientId, string clientSecret, string scope, string username, string password)
        {

            var client = new HttpClient();
            PasswordTokenRequest tokenRequest = new PasswordTokenRequest()
            {
                Address = is4ip,
                ClientId = clientId,
                ClientSecret = clientSecret,
                UserName = username,
                Password = password,
                Scope = scope
            };

            return client.RequestPasswordTokenAsync(tokenRequest).Result;
        }

    }


    #region Account tests
    /// <summary>
    /// Account API Integration tests
    /// </summary>
    [Collection("HttpClient collection")]
    public class AccountTest : BaseTest
    {
        public HttpClientFixture fixture;
        public AccountTest(HttpClientFixture fixture)
        {
            this.fixture = fixture;
            var client = fixture.Client;
        }

        #region Account tests

        [Fact]
        public async Task account_getall()
        {
            var client = fixture.Client;
            if (String.IsNullOrEmpty(TokenTest.TokenValue)) await TokenTest.token_get(client);
            //
            var util = new Utility();
            var accountid = await util.addAccount(client);
            //
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenTest.TokenValue);
            var response = await client.GetAsync("/api/account");
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var jsonString = await response.Content.ReadAsStringAsync();
            var accounts = (ICollection<AccountViewModel>)JsonConvert.DeserializeObject<IEnumerable<AccountViewModel>>(jsonString);
            Assert.True(accounts.Count > 0);
            //clean
            await util.removeAccount(client, accountid);
            //clean
            await util.removeAccount(client, accountid);
        }


        [Fact]
        public async Task account_add_update_delete()
        {
            var client = fixture.Client;
            if (String.IsNullOrEmpty(TokenTest.TokenValue)) await TokenTest.token_get(client);

            //insert
            AccountViewModel vmentity = new AccountViewModel
            {
                Name = "Account 1",
                Email = "apincore@blastasia.com",
                Description = "desc",
                IsTrial = false,
                IsActive = true,
                SetActive = DateTime.Now
            };
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenTest.TokenValue);
            var response = await client.PostAsync("/api/account", new StringContent(
                                                               JsonConvert.SerializeObject(vmentity), Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            var lastAddedId = await response.Content.ReadAsStringAsync();
            Assert.True(int.Parse(lastAddedId) > 1);
            int id = 0; int.TryParse(lastAddedId, out id);

            //get inserted
            var util = new Utility();
            vmentity = await util.GetAccount(client, id);

            //update test
            vmentity.Description = "desc updated";
            response = await client.PutAsync("/api/account/" + id.ToString(), new StringContent(JsonConvert.SerializeObject(vmentity), Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.Accepted, response.StatusCode);

            //confirm update
            response = await client.GetAsync("/api/account/" + id.ToString());
            response.EnsureSuccessStatusCode();
            var jsonString = await response.Content.ReadAsStringAsync();
            var oj = JObject.Parse(jsonString);
            var desc = oj["description"].ToString();
            Assert.Equal(desc, vmentity.Description);

            //another update with same account - concurrency
            vmentity.Description = "desc updated 2";
            response = await client.PutAsync("/api/account/" + id.ToString(), new StringContent(JsonConvert.SerializeObject(vmentity), Encoding.UTF8, "application/json"));
            Assert.Equal(HttpStatusCode.PreconditionFailed, response.StatusCode);

            //delete test 
            response = await client.DeleteAsync("/api/account/" + id.ToString());
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        }

        [Fact]
        public async Task account_getbyid()
        {
            var client = fixture.Client;
            if (String.IsNullOrEmpty(TokenTest.TokenValue)) await TokenTest.token_get(client);
            //
            var util = new Utility();
            var accountid = await util.addAccount(client);
            //
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenTest.TokenValue);
            var response = await client.GetAsync("/api/account/" + accountid.ToString());
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var jsonString = await response.Content.ReadAsStringAsync();
            var account = JsonConvert.DeserializeObject<AccountViewModel>(jsonString);
            Assert.True(account.Name == "Account");
            //clean
            await util.removeAccount(client, accountid);
        }

        [Fact]
        public async Task account_getactivebyname()
        {
            var client = fixture.Client;
            if (String.IsNullOrEmpty(TokenTest.TokenValue)) await TokenTest.token_get(client);
            //
            var util = new Utility();
            var accountid = await util.addAccount(client);
            //
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenTest.TokenValue);
            //get by id           
            var response = await client.GetAsync("/api/account/" + accountid.ToString());
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var jsonString = await response.Content.ReadAsStringAsync();
            var account = JsonConvert.DeserializeObject<AccountViewModel>(jsonString);
            //
            response = await client.GetAsync("/api/account/GetActiveByName/" + account.Name);
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            jsonString = await response.Content.ReadAsStringAsync();
            var accounts = (ICollection<AccountViewModel>)JsonConvert.DeserializeObject<IEnumerable<AccountViewModel>>(jsonString);
            Assert.True(accounts.Count > 0);
            //clean
            await util.removeAccount(client, accountid);
        }

        #endregion

        #region Account async tests

        [Fact]
        public async Task account_getallasync()
        {
            var client = fixture.Client;
            if (String.IsNullOrEmpty(TokenTest.TokenValue)) await TokenTest.token_get(client);
            //
            var util = new Utility();
            var accountid = await util.addAccount(client);
            //
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenTest.TokenValue);
            var response = await client.GetAsync("/api/accountasync");
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var jsonString = await response.Content.ReadAsStringAsync();
            var accounts = (ICollection<AccountViewModel>)JsonConvert.DeserializeObject<IEnumerable<AccountViewModel>>(jsonString);
            Assert.True(accounts.Count > 0);
            //clean
            await util.removeAccount(client, accountid);
        }


        [Fact]
        public async Task account_add_update_delete_async()
        {
            var client = fixture.Client;
            if (String.IsNullOrEmpty(TokenTest.TokenValue)) await TokenTest.token_get(client);

            //insert
            AccountViewModel vmentity = new AccountViewModel
            {
                Name = "Account 1",
                Email = "apincore@blastasia.com",
                Description = "desc",
                IsTrial = false,
                IsActive = true,
                SetActive = DateTime.Now
            };
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenTest.TokenValue);
            var response = await client.PostAsync("/api/accountasync", new StringContent(
                                                               JsonConvert.SerializeObject(vmentity), Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            var lastAddedId = await response.Content.ReadAsStringAsync();
            Assert.True(int.Parse(lastAddedId) > 1);
            int id = 0; int.TryParse(lastAddedId, out id);

            //get inserted
            var util = new Utility();
            vmentity = await util.GetAccount(client, id);

            //update test
            vmentity.Description = "desc updated";
            response = await client.PutAsync("/api/accountasync/" + id.ToString(), new StringContent(JsonConvert.SerializeObject(vmentity), Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.Accepted, response.StatusCode);

            //confirm update
            response = await client.GetAsync("/api/accountasync/" + id.ToString());
            response.EnsureSuccessStatusCode();
            var jsonString = await response.Content.ReadAsStringAsync();
            var oj = JObject.Parse(jsonString);
            var desc = oj["description"].ToString();
            Assert.Equal(desc, vmentity.Description);

            //another update with same account - concurrency
            vmentity.Description = "desc updated 2";
            response = await client.PutAsync("/api/accountasync/" + id.ToString(), new StringContent(JsonConvert.SerializeObject(vmentity), Encoding.UTF8, "application/json"));
            Assert.Equal(HttpStatusCode.PreconditionFailed, response.StatusCode);

            //delete test 
            response = await client.DeleteAsync("/api/accountasync/" + id.ToString());
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        }


        [Fact]
        public async Task account_getbyidasync()
        {
            var client = fixture.Client;
            if (String.IsNullOrEmpty(TokenTest.TokenValue)) await TokenTest.token_get(client);
            //
            var util = new Utility();
            var accountid = await util.addAccount(client);
            //
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenTest.TokenValue);
            var response = await client.GetAsync("/api/accountasync/" + accountid.ToString());
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var jsonString = await response.Content.ReadAsStringAsync();
            var account = JsonConvert.DeserializeObject<AccountViewModel>(jsonString);
            Assert.True(account.Name == "Account");
            //clean
            await util.removeAccount(client, accountid);
        }

        [Fact]
        public async Task account_getactivebynameasync()
        {
            var client = fixture.Client;
            if (String.IsNullOrEmpty(TokenTest.TokenValue)) await TokenTest.token_get(client);
            //
            var util = new Utility();
            var accountid = await util.addAccount(client);
            //
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenTest.TokenValue);
            //get by id           
            var response = await client.GetAsync("/api/accountasync/" + accountid.ToString());
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var jsonString = await response.Content.ReadAsStringAsync();
            var account = JsonConvert.DeserializeObject<AccountViewModel>(jsonString);
            //
            response = await client.GetAsync("/api/accountasync/GetActiveByName/" + account.Name);
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            jsonString = await response.Content.ReadAsStringAsync();
            var accounts = (ICollection<AccountViewModel>)JsonConvert.DeserializeObject<IEnumerable<AccountViewModel>>(jsonString);
            Assert.True(accounts.Count > 0);
            //clean
            await util.removeAccount(client, accountid);
        }

        #endregion

    }
    #endregion

    #region User tests
    [Collection("HttpClient collection")]
    public class UserTest : BaseTest
    {
        public HttpClientFixture fixture;
        public UserTest(HttpClientFixture fixture)
        {
            this.fixture = fixture;
            var client = fixture.Client;
        }

        public static string LastAddedUser { get; set; }

        #region User tests

        [Fact]
        public async Task user_getall()
        {
            var client = fixture.Client;
            if (String.IsNullOrEmpty(TokenTest.TokenValue)) await TokenTest.token_get(client);
            //
            var util = new Utility();
            var accountid = await util.addAccount(client);
            var userid = await util.addUser(client, accountid);
            //
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenTest.TokenValue);
            var response = await client.GetAsync("/api/user");
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var jsonString = await response.Content.ReadAsStringAsync();
            var users = (ICollection<UserViewModel>)JsonConvert.DeserializeObject<IEnumerable<UserViewModel>>(jsonString);
            Assert.True(users.Count > 0);
            //clean
            await util.removeUser(client, userid);
            await util.removeAccount(client, accountid);
        }


        [Fact]
        public async Task user_add_update_delete()
        {
            var client = fixture.Client;
            if (String.IsNullOrEmpty(TokenTest.TokenValue)) await TokenTest.token_get(client);
            //insert
            UserViewModel vmentity = new UserViewModel
            {
                FirstName = "User 1",
                LastName = "LastName",
                Email = "apincore@blastasia.com",
                Description = "desc",
                IsAdminRole = true,
                IsActive = true,
                Password = " ",
                AccountId = 1
            };

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenTest.TokenValue);
            var response = await client.PostAsync("/api/user", new StringContent(
                                                               JsonConvert.SerializeObject(vmentity), Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            var lastAddedId = await response.Content.ReadAsStringAsync();
            Assert.True(int.Parse(lastAddedId) > 1);
            int id = 0; int.TryParse(lastAddedId, out id);

            //get inserted
            var util = new Utility();
            vmentity = await util.GetUser(client, id);

            //update test
            vmentity.Description = "desc updated";
            response = await client.PutAsync("/api/user/" + id.ToString(), new StringContent(JsonConvert.SerializeObject(vmentity), Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.Accepted, response.StatusCode);

            //confirm update
            response = await client.GetAsync("/api/user/" + id.ToString());
            response.EnsureSuccessStatusCode();
            var jsonString = await response.Content.ReadAsStringAsync();
            var oj = JObject.Parse(jsonString);
            var desc = oj["description"].ToString();
            Assert.Equal(desc, vmentity.Description);

            //another update with same account - concurrency
            vmentity.Description = "desc updated 2";
            response = await client.PutAsync("/api/user/" + id.ToString(), new StringContent(JsonConvert.SerializeObject(vmentity), Encoding.UTF8, "application/json"));
            Assert.Equal(HttpStatusCode.PreconditionFailed, response.StatusCode);

            //delete test 
            response = await client.DeleteAsync("/api/user/" + id.ToString());
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        }

        [Fact]
        public async Task user_getbyid()
        {
            var client = fixture.Client;
            if (String.IsNullOrEmpty(TokenTest.TokenValue)) await TokenTest.token_get(client);
            //
            var util = new Utility();
            var accountid = await util.addAccount(client);
            var userid = await util.addUser(client, accountid);
            //
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenTest.TokenValue);
            var response = await client.GetAsync("/api/user/" + userid.ToString());
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var jsonString = await response.Content.ReadAsStringAsync();
            var user = JsonConvert.DeserializeObject<UserViewModel>(jsonString);
            Assert.True(user.FirstName == "FirstName");
            // lazy-loading test
            response = await client.GetAsync("/api/account/" + accountid.ToString());
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            jsonString = await response.Content.ReadAsStringAsync();
            var account = JsonConvert.DeserializeObject<AccountViewModel>(jsonString);
            Assert.True(account.Users.Count == 1);
            //clean
            await util.removeUser(client, userid);
            await util.removeAccount(client, accountid);
        }

        [Fact]
        public async Task user_getactivebyfirstname()
        {
            var client = fixture.Client;
            if (String.IsNullOrEmpty(TokenTest.TokenValue)) await TokenTest.token_get(client);
            //
            var util = new Utility();
            var accountid = await util.addAccount(client);
            var userid = await util.addUser(client, accountid);
            //
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenTest.TokenValue);
            //get by id           
            var response = await client.GetAsync("/api/user/" + userid.ToString());
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var jsonString = await response.Content.ReadAsStringAsync();
            var user = JsonConvert.DeserializeObject<UserViewModel>(jsonString);
            //
            response = await client.GetAsync("/api/user/GetActiveByFirstName/" + user.FirstName);
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            jsonString = await response.Content.ReadAsStringAsync();
            var users = (ICollection<UserViewModel>)JsonConvert.DeserializeObject<IEnumerable<UserViewModel>>(jsonString);
            Assert.True(users.Count > 0);
            //clean
            await util.removeUser(client, userid);
            await util.removeAccount(client, accountid);
        }

        [Fact]
        public async Task user_getbyname_sp()
        {
            var client = fixture.Client;
            if (String.IsNullOrEmpty(TokenTest.TokenValue)) await TokenTest.token_get(client);
            //
            var util = new Utility();
            var accountid = await util.addAccount(client);
            var userid = await util.addUser(client, accountid);
            //
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenTest.TokenValue);
            //get by id           
            var response = await client.GetAsync("/api/user/" + userid.ToString());
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var jsonString = await response.Content.ReadAsStringAsync();
            var user = JsonConvert.DeserializeObject<UserViewModel>(jsonString);
            //
            response = await client.GetAsync("/api/user/GetUsersByName/" + user.FirstName + "/" + user.LastName);
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            jsonString = await response.Content.ReadAsStringAsync();
            var users = (ICollection<UserViewModel>)JsonConvert.DeserializeObject<IEnumerable<UserViewModel>>(jsonString);
            Assert.True(users.Count > 0);
            //clean
            await util.removeUser(client, userid);
            await util.removeAccount(client, accountid);
        }

        [Fact]
        public async Task user_updateemailbyusername_sp()
        {
            var client = fixture.Client;
            if (String.IsNullOrEmpty(TokenTest.TokenValue)) await TokenTest.token_get(client);
            //
            var util = new Utility();
            var accountid = await util.addAccount(client);
            var userid = await util.addUser(client, accountid);
            //
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenTest.TokenValue);
            //get by id           
            var response = await client.GetAsync("/api/user/" + userid.ToString());
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var jsonString = await response.Content.ReadAsStringAsync();
            var user = JsonConvert.DeserializeObject<UserViewModel>(jsonString);
            //
            string updatedEmail = "newemail@com.com";
            response = await client.GetAsync("/api/user/UpdateEmailbyUsername/" + user.UserName + "/" + updatedEmail);
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var records = await response.Content.ReadAsStringAsync();
            Assert.True(records != "0");
            //get by id to confirm update           
            response = await client.GetAsync("/api/user/" + userid.ToString());
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            jsonString = await response.Content.ReadAsStringAsync();
            user = JsonConvert.DeserializeObject<UserViewModel>(jsonString);
            Assert.True(user.Email == updatedEmail);
            //clean
            await util.removeUser(client, userid);
            await util.removeAccount(client, accountid);
        }

        #endregion

        #region User async tests

        [Fact]
        public async Task user_getallasync()
        {
            var client = fixture.Client;
            if (String.IsNullOrEmpty(TokenTest.TokenValue)) await TokenTest.token_get(client);
            //
            var util = new Utility();
            var accountid = await util.addAccount(client);
            var userid = await util.addUser(client, accountid);
            //
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenTest.TokenValue);
            var response = await client.GetAsync("/api/userasync");
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var jsonString = await response.Content.ReadAsStringAsync();
            var users = (ICollection<UserViewModel>)JsonConvert.DeserializeObject<IEnumerable<UserViewModel>>(jsonString);
            Assert.True(users.Count > 0);
            // lazy-loading test
            response = await client.GetAsync("/api/accountasync/" + accountid.ToString());
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            jsonString = await response.Content.ReadAsStringAsync();
            var account = JsonConvert.DeserializeObject<AccountViewModel>(jsonString);
            Assert.True(account.Users.Count == 1);
            //clean
            await util.removeUser(client, userid);
            await util.removeAccount(client, accountid);
        }


        [Fact]
        public async Task user_add_update_delete_async()
        {
            var client = fixture.Client;
            if (String.IsNullOrEmpty(TokenTest.TokenValue)) await TokenTest.token_get(client);
            //insert
            UserViewModel vmentity = new UserViewModel
            {
                FirstName = "User 1",
                LastName = "LastName",
                Email = "apincore@blastasia.com",
                Description = "desc",
                IsAdminRole = true,
                IsActive = true,
                Password = " ",
                AccountId = 1
            };

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenTest.TokenValue);
            var response = await client.PostAsync("/api/userasync", new StringContent(
                                                               JsonConvert.SerializeObject(vmentity), Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            var lastAddedId = await response.Content.ReadAsStringAsync();
            Assert.True(int.Parse(lastAddedId) > 1);
            int id = 0; int.TryParse(lastAddedId, out id);

            //get inserted
            var util = new Utility();
            vmentity = await util.GetUser(client, id);

            //update test
            vmentity.Description = "desc updated";
            response = await client.PutAsync("/api/userasync/" + id.ToString(), new StringContent(JsonConvert.SerializeObject(vmentity), Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.Accepted, response.StatusCode);

            //confirm update
            response = await client.GetAsync("/api/userasync/" + id.ToString());
            response.EnsureSuccessStatusCode();
            var jsonString = await response.Content.ReadAsStringAsync();
            var oj = JObject.Parse(jsonString);
            var desc = oj["description"].ToString();
            Assert.Equal(desc, vmentity.Description);

            //another update with same account - concurrency
            vmentity.Description = "desc updated 2";
            response = await client.PutAsync("/api/userasync/" + id.ToString(), new StringContent(JsonConvert.SerializeObject(vmentity), Encoding.UTF8, "application/json"));
            Assert.Equal(HttpStatusCode.PreconditionFailed, response.StatusCode);

            //delete test 
            response = await client.DeleteAsync("/api/userasync/" + id.ToString());
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        }

        [Fact]
        public async Task user_getbyidasync()
        {
            var client = fixture.Client;
            if (String.IsNullOrEmpty(TokenTest.TokenValue)) await TokenTest.token_get(client);
            //
            var util = new Utility();
            var accountid = await util.addAccount(client);
            var userid = await util.addUser(client, accountid);
            //
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenTest.TokenValue);
            var response = await client.GetAsync("/api/userasync/" + userid.ToString());
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var jsonString = await response.Content.ReadAsStringAsync();
            var user = JsonConvert.DeserializeObject<UserViewModel>(jsonString);
            Assert.True(user.FirstName == "FirstName");
            //clean
            await util.removeUser(client, userid);
            await util.removeAccount(client, accountid);
        }

        [Fact]
        public async Task user_getactivebyfirstnameasync()
        {
            var client = fixture.Client;
            if (String.IsNullOrEmpty(TokenTest.TokenValue)) await TokenTest.token_get(client);
            //
            var util = new Utility();
            var accountid = await util.addAccount(client);
            var userid = await util.addUser(client, accountid);
            //
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenTest.TokenValue);
            //get by id           
            var response = await client.GetAsync("/api/userasync/" + userid.ToString());
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var jsonString = await response.Content.ReadAsStringAsync();
            var user = JsonConvert.DeserializeObject<UserViewModel>(jsonString);
            //
            response = await client.GetAsync("/api/userasync/GetActiveByFirstName/" + user.FirstName);
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            jsonString = await response.Content.ReadAsStringAsync();
            var users = (ICollection<UserViewModel>)JsonConvert.DeserializeObject<IEnumerable<UserViewModel>>(jsonString);
            Assert.True(users.Count > 0);
            //clean
            await util.removeUser(client, userid);
            await util.removeAccount(client, accountid);
        }

        [Fact]
        public async Task user_getbynameasync_sp()
        {
            var client = fixture.Client;
            if (String.IsNullOrEmpty(TokenTest.TokenValue)) await TokenTest.token_get(client);
            //
            var util = new Utility();
            var accountid = await util.addAccount(client);
            var userid = await util.addUser(client, accountid);
            //
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenTest.TokenValue);
            //get by id           
            var response = await client.GetAsync("/api/userasync/" + userid.ToString());
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var jsonString = await response.Content.ReadAsStringAsync();
            var user = JsonConvert.DeserializeObject<UserViewModel>(jsonString);
            //
            response = await client.GetAsync("/api/userasync/GetUsersByName/" + user.FirstName + "/" + user.LastName);
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            jsonString = await response.Content.ReadAsStringAsync();
            var users = (ICollection<UserViewModel>)JsonConvert.DeserializeObject<IEnumerable<UserViewModel>>(jsonString);
            Assert.True(users.Count > 0);
            //clean
            await util.removeUser(client, userid);
            await util.removeAccount(client, accountid);
        }

        [Fact]
        public async Task user_updateemailbyusernameasync_sp()
        {
            var client = fixture.Client;
            if (String.IsNullOrEmpty(TokenTest.TokenValue)) await TokenTest.token_get(client);
            //
            var util = new Utility();
            var accountid = await util.addAccount(client);
            var userid = await util.addUser(client, accountid);
            //
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenTest.TokenValue);
            //get by id           
            var response = await client.GetAsync("/api/userasync/" + userid.ToString());
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var jsonString = await response.Content.ReadAsStringAsync();
            var user = JsonConvert.DeserializeObject<UserViewModel>(jsonString);
            //
            string updatedEmail = "newemail@com.com";
            response = await client.GetAsync("/api/userasync/UpdateEmailbyUsername/" + user.UserName + "/" + updatedEmail);
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var records = await response.Content.ReadAsStringAsync();
            Assert.True(records != "0");
            //get by id to confirm update           
            response = await client.GetAsync("/api/userasync/" + userid.ToString());
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            jsonString = await response.Content.ReadAsStringAsync();
            user = JsonConvert.DeserializeObject<UserViewModel>(jsonString);
            Assert.True(user.Email == updatedEmail);
            //clean
            await util.removeUser(client, userid);
            await util.removeAccount(client, accountid);
        }


        #endregion

    }

    #endregion

    #region DynamicFormsConfiguration tests

    /// <summary>
    ///
    /// DynamicFormsConfiguration API Integration tests
    ///
    /// MANUAL UPDATES REQUIRED!
    ///
    /// NOTE: In order to run an pass these scaffolded tests they have to be manually adjusted 
    ///       according to new entity class properties - search for MANUAL UPDATES REQUIRED!
    ///
    /// </summary>
    [Collection("HttpClient collection")]
    public class DynamicFormsConfigurationTest : BaseTest
    {
        public HttpClientFixture fixture;
        public DynamicFormsConfigurationTest(HttpClientFixture fixture)
        {
            this.fixture = fixture;
            var client = fixture.Client;
        }

        public static string LastAddedDynamicFormsConfiguration { get; set; }

        #region DynamicFormsConfiguration tests

        [Fact]
        public async Task dynamicformsconfiguration_getall()
        {
            var httpclient = fixture.Client;
            if (String.IsNullOrEmpty(TokenTest.TokenValue)) await TokenTest.token_get(httpclient);
            //
            var util = new Utility();
            //MANUAL UPDATES REQUIRED!
            //todo - add if any parent of the entity
            //add entity
            
            var dynamicformsconfigurationid = await util.addDynamicFormsConfiguration(httpclient);
            //
            httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenTest.TokenValue);
            var response = await httpclient.GetAsync("/api/dynamicformsconfiguration");
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var jsonString = await response.Content.ReadAsStringAsync();
            var vmenititys = (ICollection<DynamicFormsConfigurationViewModel>)JsonConvert.DeserializeObject<IEnumerable<DynamicFormsConfigurationViewModel>>(jsonString);
            Assert.True(vmenititys.Count > 0);
            // lazy-loading test if entity has children
            response = await httpclient.GetAsync("/api/dynamicformsconfiguration/" + dynamicformsconfigurationid.ToString());
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            jsonString = await response.Content.ReadAsStringAsync();
            var vmenitity = JsonConvert.DeserializeObject<DynamicFormsConfigurationViewModel>(jsonString);
            //Assert.True(vmenitity.Kids.Count == 1);
            //clean
            await util.removeDynamicFormsConfiguration(httpclient, dynamicformsconfigurationid);
            //remove if any parent entity added 
        }


        [Fact]
        public async Task dynamicformsconfiguration_add_update_delete()
        {
            var httpclient = fixture.Client; ;
            if (String.IsNullOrEmpty(TokenTest.TokenValue)) await TokenTest.token_get(httpclient);
            //
            DynamicFormsConfigurationViewModel dynamicformsconfiguration = new DynamicFormsConfigurationViewModel
            {
                //MANUAL UPDATES REQUIRED!
                Name = "form-dynamic",
                Value = "[\r\n\t{\r\n      \"key\": \"firstname\",\r\n      \"type\": \"input\",\r\n      \"templateOptions\": {\r\n        \"label\": \"Firstname\",\r\n        \"placeholder\": \"Enter Firstname\",\r\n        \"required\": true,\r\n      }\r\n    },\r\n\t{\r\n      \"key\": \"lastname\",\r\n      \"type\": \"input\",\r\n      \"templateOptions\": {\r\n        \"label\": \"Lasttname\",\r\n        \"placeholder\": \"Enter Lastname\",\r\n        \"required\": true,\r\n      }\r\n    },\r\n    {\r\n      \"key\": \"email\",\r\n      \"type\": \"input\",\r\n      \"templateOptions: {\r\n        \"label\": \"Email address\",\r\n        \"placeholder\": \"Enter email\",\r\n        \"required\": true,\r\n      }\r\n    }\r\n  ]",
                Version = 1.0,
                IsActive = true,
                IsDeleted = false

            };

            httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenTest.TokenValue);
            var response = await httpclient.PostAsync("/api/dynamicformsconfiguration", new StringContent(
                                                               JsonConvert.SerializeObject(dynamicformsconfiguration), Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            var lastAddedId = await response.Content.ReadAsStringAsync();
            Assert.True(int.Parse(lastAddedId) > 1);
            int id = 0; int.TryParse(lastAddedId, out id);

            //get inserted
            var util = new Utility();
            var vmentity = await util.GetDynamicFormsConfiguration(httpclient, id);

            //update test

            response = await httpclient.PutAsync("/api/dynamicformsconfiguration/" + id.ToString(), new StringContent(JsonConvert.SerializeObject(vmentity), Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.Accepted, response.StatusCode);

            //confirm update
            response = await httpclient.GetAsync("/api/dynamicformsconfiguration/" + id.ToString());
            response.EnsureSuccessStatusCode();
            var jsonString = await response.Content.ReadAsStringAsync();
            var oj = JObject.Parse(jsonString);
            //var tt = oj["testText"].ToString();
            //Assert.Equal(tt, vmentity.TestText);
            //
            ////another update with same account - concurrency
            //vmentity.TestText = "tt updated 2";
            //response = await httpclient.PutAsync("/api/dynamicformsconfiguration/" + id.ToString(), new StringContent(JsonConvert.SerializeObject(vmentity), Encoding.UTF8, "application/json"));
            //Assert.Equal(HttpStatusCode.PreconditionFailed, response.StatusCode);

            //delete test 
            response = await httpclient.DeleteAsync("/api/dynamicformsconfiguration/" + id.ToString());
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task dynamicformsconfiguration_getbyid()
        {
            var httpclient = fixture.Client;
            if (String.IsNullOrEmpty(TokenTest.TokenValue)) await TokenTest.token_get(httpclient);
            //
            var util = new Utility();
            //MANUAL UPDATES REQUIRED!
            //todo - add parent of the entity if exist
            //add entity
            var dynamicformsconfigurationid = await util.addDynamicFormsConfiguration(httpclient);
            //
            httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenTest.TokenValue);
            var response = await httpclient.GetAsync("/api/dynamicformsconfiguration/" + dynamicformsconfigurationid.ToString());
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var jsonString = await response.Content.ReadAsStringAsync();
            var vmenitity = JsonConvert.DeserializeObject<DynamicFormsConfigurationViewModel>(jsonString);
            //Assert.True(vmenitity.TestText == "tt updated");

            //clean
            await util.removeDynamicFormsConfiguration(httpclient, dynamicformsconfigurationid);
            //remove if any parent entity added 
        }

        #endregion

        #region DynamicFormsConfiguration async tests

        [Fact]
        public async Task dynamicformsconfiguration_getallasync()
        {
            var httpclient = fixture.Client;
            if (String.IsNullOrEmpty(TokenTest.TokenValue)) await TokenTest.token_get(httpclient);
            //
            var util = new Utility();
            //MANUAL UPDATES REQUIRED!
            //todo - add parent of the entity if exist
            //add entity
            var dynamicformsconfigurationid = await util.addDynamicFormsConfiguration(httpclient);
            //
            httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenTest.TokenValue);
            var response = await httpclient.GetAsync("/api/dynamicformsconfigurationasync");
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var jsonString = await response.Content.ReadAsStringAsync();
            var vmenititys = (ICollection<UserViewModel>)JsonConvert.DeserializeObject<IEnumerable<UserViewModel>>(jsonString);
            Assert.True(vmenititys.Count > 0);
            // lazy-loading test if entity has children
            response = await httpclient.GetAsync("/api/dynamicformsconfigurationasync/" + dynamicformsconfigurationid.ToString());
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            jsonString = await response.Content.ReadAsStringAsync();
            var vmenitity = JsonConvert.DeserializeObject<DynamicFormsConfigurationViewModel>(jsonString);
            //Assert.True(vmenitity.Kids.Count == 1);
            //clean
            await util.removeDynamicFormsConfiguration(httpclient, dynamicformsconfigurationid);
            //remove if any parent entity added 
        }


        [Fact]
        public async Task dynamicformsconfiguration_add_update_delete_async()
        {
            var httpclient = fixture.Client; ;
            if (String.IsNullOrEmpty(TokenTest.TokenValue)) await TokenTest.token_get(httpclient);
            //
            DynamicFormsConfigurationViewModel dynamicformsconfiguration = new DynamicFormsConfigurationViewModel
            {
                //MANUAL UPDATES REQUIRED!
                //initiate viewmodel object
                //TestText = "tt updated"
                Name = "form-dynamic",
                Value = "[\r\n\t{\r\n      \"key\": \"firstname\",\r\n      \"type\": \"input\",\r\n      \"templateOptions\": {\r\n        \"label\": \"Firstname\",\r\n        \"placeholder\": \"Enter Firstname\",\r\n        \"required\": true,\r\n      }\r\n    },\r\n\t{\r\n      \"key\": \"lastname\",\r\n      \"type\": \"input\",\r\n      \"templateOptions\": {\r\n        \"label\": \"Lasttname\",\r\n        \"placeholder\": \"Enter Lastname\",\r\n        \"required\": true,\r\n      }\r\n    },\r\n    {\r\n      \"key\": \"email\",\r\n      \"type\": \"input\",\r\n      \"templateOptions: {\r\n        \"label\": \"Email address\",\r\n        \"placeholder\": \"Enter email\",\r\n        \"required\": true,\r\n      }\r\n    }\r\n  ]",
                Version = 1.0,
                IsActive = true,
                IsDeleted = false
            };

            httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenTest.TokenValue);
            var response = await httpclient.PostAsync("/api/dynamicformsconfigurationasync", new StringContent(
                                                               JsonConvert.SerializeObject(dynamicformsconfiguration), Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            var lastAddedId = await response.Content.ReadAsStringAsync();
            Assert.True(int.Parse(lastAddedId) > 1);
            int id = 0; int.TryParse(lastAddedId, out id);

            //get inserted
            var util = new Utility();
            var vmentity = await util.GetDynamicFormsConfiguration(httpclient, id);

            //update test
            //vmentity.TestText = "tt updated";
            response = await httpclient.PutAsync("/api/dynamicformsconfigurationasync/" + id.ToString(), new StringContent(JsonConvert.SerializeObject(vmentity), Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.Accepted, response.StatusCode);

            //confirm update
            response = await httpclient.GetAsync("/api/dynamicformsconfigurationasync/" + id.ToString());
            response.EnsureSuccessStatusCode();
            var jsonString = await response.Content.ReadAsStringAsync();
            var oj = JObject.Parse(jsonString);
            // var tt = oj["testText"].ToString();
            // Assert.Equal(tt, vmentity.TestText);

            //another update with same account - concurrency
            //vmentity.TestText = "tt updated 2";
            response = await httpclient.PutAsync("/api/dynamicformsconfigurationasync/" + id.ToString(), new StringContent(JsonConvert.SerializeObject(vmentity), Encoding.UTF8, "application/json"));
            Assert.Equal(HttpStatusCode.PreconditionFailed, response.StatusCode);

            //delete test 
            response = await httpclient.DeleteAsync("/api/dynamicformsconfigurationasync/" + id.ToString());
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        }

        [Fact]
        public async Task dynamicformsconfiguration_getbyidasync()
        {

            var httpclient = fixture.Client;
            if (String.IsNullOrEmpty(TokenTest.TokenValue)) await TokenTest.token_get(httpclient);
            //
            var util = new Utility();
            //MANUAL UPDATES REQUIRED!
            //todo - add if any parent of the entity
            //add entity
            var dynamicformsconfigurationid = await util.addDynamicFormsConfiguration(httpclient);
            //
            httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenTest.TokenValue);
            var response = await httpclient.GetAsync("/api/dynamicformsconfigurationasync/" + dynamicformsconfigurationid.ToString());
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var jsonString = await response.Content.ReadAsStringAsync();
            var vmenitity = JsonConvert.DeserializeObject<DynamicFormsConfigurationViewModel>(jsonString);
            // Assert.True(vmenitity.TestText == "tt updated");

            //clean
            await util.removeDynamicFormsConfiguration(httpclient, dynamicformsconfigurationid);
            //remove if any parent entity added 
        }

        #endregion
    }
    #endregion

    #region Shared test

    public class Utility
    {
        public async Task<int> addAccount(HttpClient client)
        {
            AccountViewModel account = new AccountViewModel
            {
                Name = "Account",
                Email = "apincore@blastasia.com",
                Description = "desc" + (new Random()).Next().ToString(),
                IsTrial = false,
                IsActive = true,
                SetActive = DateTime.Now
            };

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenTest.TokenValue);
            var response = await client.PostAsync("/api/accountasync", new StringContent(
                                                               JsonConvert.SerializeObject(account), Encoding.UTF8, "application/json"));
            var jsonString = await response.Content.ReadAsStringAsync();
            int lastAdded = 0;
            int.TryParse(jsonString, out lastAdded);
            return lastAdded;
        }
        public async Task<AccountViewModel> GetAccount(HttpClient client, int id)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenTest.TokenValue);
            var response = await client.GetAsync("/api/accountasync/" + id.ToString());
            response.EnsureSuccessStatusCode();
            var jsonString = await response.Content.ReadAsStringAsync();
            var account = JsonConvert.DeserializeObject<AccountViewModel>(jsonString);
            return account;
        }
        public async Task removeAccount(HttpClient client, int id)
        {
            await client.DeleteAsync("/api/account/" + id.ToString());
        }

        public async Task<int> addUser(HttpClient client, int accountId)
        {
            UserViewModel user = new UserViewModel
            {
                FirstName = "FirstName",
                LastName = "LastName",
                UserName = "username",
                Email = "email",
                Description = "desc" + (new Random()).Next().ToString(),
                IsAdminRole = true,
                IsActive = true,
                Password = " ",
                AccountId = accountId
            };

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenTest.TokenValue);
            var response = await client.PostAsync("/api/userasync", new StringContent(
                                                               JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json"));
            var jsonString = await response.Content.ReadAsStringAsync();
            int lastAdded = 0;
            int.TryParse(jsonString, out lastAdded);
            return lastAdded;
        }
        public async Task<UserViewModel> GetUser(HttpClient client, int id)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenTest.TokenValue);
            var response = await client.GetAsync("/api/userasync/" + id.ToString());
            response.EnsureSuccessStatusCode();
            var jsonString = await response.Content.ReadAsStringAsync();
            var user = JsonConvert.DeserializeObject<UserViewModel>(jsonString);
            return user;
        }
        public async Task removeUser(HttpClient client, int id)
        {
            await client.DeleteAsync("/api/user/" + id.ToString());
        }

        public async Task<int> addDynamicFormsConfiguration(HttpClient client)
        {

            DynamicFormsConfigurationViewModel vmentity = new DynamicFormsConfigurationViewModel
            {
                //MANUAL UPDATES REQUIRED!
                //initiate viewmodel object
                Name = "form-dynamic",
                Value = "[\r\n\t{\r\n      \"key\": \"firstname\",\r\n      \"type\": \"input\",\r\n      \"templateOptions\": {\r\n        \"label\": \"Firstname\",\r\n        \"placeholder\": \"Enter Firstname\",\r\n        \"required\": true,\r\n      }\r\n    },\r\n\t{\r\n      \"key\": \"lastname\",\r\n      \"type\": \"input\",\r\n      \"templateOptions\": {\r\n        \"label\": \"Lasttname\",\r\n        \"placeholder\": \"Enter Lastname\",\r\n        \"required\": true,\r\n      }\r\n    },\r\n    {\r\n      \"key\": \"email\",\r\n      \"type\": \"input\",\r\n      \"templateOptions: {\r\n        \"label\": \"Email address\",\r\n        \"placeholder\": \"Enter email\",\r\n        \"required\": true,\r\n      }\r\n    }\r\n  ]",
                Version = 1.0,
                IsActive = true,
                IsDeleted = false
            };

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenTest.TokenValue);
            var response = await client.PostAsync("/api/dynamicformsconfiguration", new StringContent(
                                                               JsonConvert.SerializeObject(vmentity), Encoding.UTF8, "application/json"));
            var jsonString = await response.Content.ReadAsStringAsync();
            int lastAdded = 0;
            int.TryParse(jsonString, out lastAdded);
            return lastAdded;
        }
        public async Task<DynamicFormsConfigurationViewModel> GetDynamicFormsConfiguration(HttpClient client, int id)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenTest.TokenValue);
            var response = await client.GetAsync("/api/dynamicformsconfigurationasync/" + id.ToString());
            response.EnsureSuccessStatusCode();
            var jsonString = await response.Content.ReadAsStringAsync();
            var vmentity = JsonConvert.DeserializeObject<DynamicFormsConfigurationViewModel>(jsonString);
            return vmentity;
        }
        public async Task removeDynamicFormsConfiguration(HttpClient client, int id)
        {
            await client.DeleteAsync("/api/dynamicformsconfiguration/" + id.ToString());
        }


    }
    #endregion


    #region async Load tests
    [Collection("HttpClient collection")]
    public class ZLoadTest : BaseTest
    {
        public HttpClientFixture fixture;
        public ZLoadTest(HttpClientFixture fixture)
        {
            this.fixture = fixture;
            var client = fixture.Client;
        }


        /// <summary>
        /// Load test
        /// --local service: BaseTest.RemoteService = false  
        /// --remote service: BaseTest.RemoteService = true   
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task LoadTest()
        {
            int loopmax = 10;
            var client = fixture.Client;
            if (String.IsNullOrEmpty(TokenTest.TokenValue)) await TokenTest.token_get(client);
            //
            var accountId = 0;
            var userId = 0;
            var util = new Utility();
            int i = 1;
            while (i < loopmax)
            {
                accountId = await util.addAccount(client);
                userId = await util.addUser(client, accountId);
                await util.GetAccount(client, accountId);
                await util.GetUser(client, userId);
                await util.removeUser(client, userId);
                await util.removeAccount(client, accountId);
                i++;
            }
            //
            Assert.True(i == loopmax);

        }


        /// <summary>
        ///  
        /// DDoSAttack prevention test 
        /// /api/info flagged for DDoS test with API.controller attribute  
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task DDoSAttack_Test()
        {
            int attackCount = 0;
            var client = fixture.Client;

            var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json").Build();


            if (configuration["IntegrationTests"] == "False" || configuration["DDosAttackProtection:Enabled"] == "False")  //skip
            {
                Assert.True(true, "Skip it when the configuration settings turn off DDoS tests");
                return;
            }

            ////read from tests settings
            //var configuration = new ConfigurationBuilder()
            //    .SetBasePath(Directory.GetCurrentDirectory())
            //    .AddJsonFile("appsettings.json").Build();
            //string maxHitsPerOrigin = configuration["DDosAttackProtection:MaxHitsPerOrigin"];
            //string maxHitsPerOriginIntervalMs = configuration["DDosAttackProtection:MaxHitsPerOriginIntervalMs"];
            //string releaseIntervalMs = configuration["DDosAttackProtection:ReleaseIntervalMs"];

            //SetAppSettingValue("DDosAttackProtection:MaxHitsPerOrigin", "50");
            //SetAppSettingValue("DDosAttackProtection:MaxHitsPerOriginIntervalMs", "1000");
            //SetAppSettingValue("DDosAttackProtection:ReleaseIntervalMs", "1000");

            //LoginModel login = new LoginModel { Username = UserName, Password = Password };
            //StringContent login_string = new StringContent(JsonConvert.SerializeObject(login), Encoding.UTF8, "application/json");

            HttpResponseMessage response = null;
            attackCount = 5000;
            int i = 1;
            while (i < attackCount)
            {
                //response = await client.PostAsync("/api/token", login_string);
                response = await client.GetAsync("/api/info");
                i++;
            }
            //forbidden
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);

            //blocked ip should be expired after 2s
            System.Threading.Thread.Sleep(2000);
            attackCount = 10;
            i = 1;
            while (i < attackCount)
            {
                //response = await client.PostAsync("/api/token", login_string);
                response = await client.GetAsync("/api/info");
                i++;
            }
            //forbidden
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            ////set back config settings
            //SetAppSettingValue("DDosAttackProtection:MaxHitsPerOrigin", maxHitsPerOrigin);
            //SetAppSettingValue("DDosAttackProtection:MaxHitsPerOriginIntervalMs", maxHitsPerOriginIntervalMs);
            //SetAppSettingValue("DDosAttackProtection:ReleaseIntervalMs", releaseIntervalMs);

        }


        //update json file
        public static void SetAppSettingValue(string key, string value, string appSettingsJsonFilePath = null)
        {
            if (appSettingsJsonFilePath == null)
            {
                appSettingsJsonFilePath = System.IO.Path.Combine(System.AppContext.BaseDirectory, "appsettings.json");
            }

            var json = System.IO.File.ReadAllText(appSettingsJsonFilePath);
            dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject<Newtonsoft.Json.Linq.JObject>(json);

            jsonObj[key] = value;

            string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);

            System.IO.File.WriteAllText(appSettingsJsonFilePath, output);
        }

    }
    #endregion


}
