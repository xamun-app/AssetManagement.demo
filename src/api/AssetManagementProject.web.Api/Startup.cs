using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AssetManagementProject.web.Api;
using AssetManagementProject.web.Api.Configuration;
using AssetManagementProject.web.Api.Middlewares;
using AssetManagementProject.web.Api.Utilities;
using AssetManagementProject.web.Domain.Interface;
using AssetManagementProject.web.Domain.Mapping;
using AssetManagementProject.web.Domain.Service;
using AssetManagementProject.web.Entity.Context;
using AssetManagementProject.web.Entity.Repository;
using AssetManagementProject.web.Entity.UnitofWork;
using IdentityModel;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using Serilog;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

/// <summary>
/// Designed by BlastAsia, Inc. 2019
/// https://www.blastasia.com 
///
/// NOTE:
/// Must update database connection in appsettings.json - "AssetManagementProject.web.ApiDB"
/// </summary>

namespace AssetManagementProject.web.Api
{
    public partial class Startup
    {

        public static IConfiguration Configuration { get; set; }
        public IWebHostEnvironment HostingEnvironment { get; private set; }

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            HostingEnvironment = env;
        }

        public void ConfigureServices(IServiceCollection services)
        {

            Log.Information("Startup::ConfigureServices");

            try
            {
                services.AddControllers(
                opt =>
                {
                    //Custom filters can be added here 
                    //opt.Filters.Add(typeof(CustomFilterAttribute));
                    //opt.Filters.Add(new ProducesAttribute("application/json"));
                }
                ).SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

                services.Configure<ForwardedHeadersOptions>(options =>
                {
                    options.ForwardedHeaders =
                        ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
                });

                #region "API versioning"
                //API versioning service
                services.AddApiVersioning(
                    o =>
                    {
                        //o.Conventions.Controller<UserController>().HasApiVersion(1, 0);
                        o.AssumeDefaultVersionWhenUnspecified = true;
                        o.ReportApiVersions = true;
                        o.DefaultApiVersion = new ApiVersion(1, 0);
                        o.ApiVersionReader = new UrlSegmentApiVersionReader();
                    }
                    );

                // format code as "'v'major[.minor][-status]"
                services.AddVersionedApiExplorer(
                options =>
                {
                    options.GroupNameFormat = "'v'VVV";
                    //versioning by url segment
                    options.SubstituteApiVersionInUrl = true;
                });
                #endregion

                //db service
                if (Configuration["ConnectionStrings:UseInMemoryDatabase"] == "True")
                    services.AddDbContext<DefaultDbContext>(opt => opt.UseInMemoryDatabase("TestDB-" + Guid.NewGuid().ToString()));
                else
                    services.AddDbContext<DefaultDbContext>(options => options.UseSqlServer(Configuration["ConnectionStrings:AssetManagementProject.webDB"]));

                #region "Authentication"
                if (Configuration["Authentication:UseIdentityServer4"] == "False")
                {
                    //JWT API authentication service
                    services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,
                            ValidIssuer = Configuration["Jwt:Issuer"],
                            ValidAudience = Configuration["Jwt:Issuer"],
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                        };
                    }
                    );
                }
                else
                {
                    //Identity Server 4 API authentication service
                    services.AddAuthorization(options =>
                    {
                        options.AddPolicy(AuthorizationRolesConstants.Administrator,
                            policy =>
                                policy.RequireAssertion(context => context.User.HasClaim(c =>
                                        ((c.Type == JwtClaimTypes.Role && c.Value == Configuration["Authentication:AdministrationRole"]) ||
                                        (c.Type == $"client_{JwtClaimTypes.Role}" && c.Value == Configuration["Authentication:AdministrationRole"]))
                                    ) && context.User.HasClaim(c => c.Type == JwtClaimTypes.Scope && c.Value == Configuration["Authentication:OidcApiName"])
                                ));
                        options.AddPolicy(AuthorizationRolesConstants.ApplicationUser,
                            policy =>
                                policy.RequireAssertion(context => context.User.HasClaim(c =>
                                        ((c.Type == JwtClaimTypes.Role && c.Value == Configuration["Authentication:ApplicationUserRole"]) ||
                                        (c.Type == $"client_{JwtClaimTypes.Role}" && c.Value == Configuration["Authentication:ApplicationUserRole"]))
                                    ) && context.User.HasClaim(c => c.Type == JwtClaimTypes.Scope && c.Value == Configuration["Authentication:OidcApiName"])
                                ));
                        options.AddPolicy(AuthorizationRolesConstants.IDAScript,
                            policy =>
                                policy.RequireAssertion(context => context.User.HasClaim(c =>
                                        c.Type == "client_id" && c.Value == "ida_script") &&
                                        context.User.HasClaim(c => c.Type == $"client_user" && c.Value == "admin") &&
                                        context.User.HasClaim(c => c.Type == JwtClaimTypes.Scope && c.Value == Configuration["Authentication:OidcApiName"])
                                ));
                    });
                    //.AddJsonFormatters();
                    services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                    .AddIdentityServerAuthentication(option =>
                    {
                        option.Authority = Configuration["Authentication:IdentityServer4IP"];
                        option.RequireHttpsMetadata = true;
                        //option.ApiSecret = "secret";
                        option.ApiName = Configuration["Authentication:ApiName"];  //This is the resourceAPI that we defined in the Config.cs in the AuthServ project (apiresouces.json and clients.json). They have to be named equal.
                        //option.Audience = Configuration["Authentication:ApiName"];
                        //option.TokenValidationParameters.ValidIssuers = new[]
                        //{
                        //    Configuration["Authentication:IdentityServer4IP"],
                        //    Configuration["Authentication:IdentityServer4MobileIP"],
                        //};
                    });

                }
                #endregion

                #region "CORS"
                // include support for CORS
                // More often than not, we will want to specify that our API accepts requests coming from other origins (other domains). When issuing AJAX requests, browsers make preflights to check if a server accepts requests from the domain hosting the web app. If the response for these preflights don't contain at least the Access-Control-Allow-Origin header specifying that accepts requests from the original domain, browsers won't proceed with the real requests (to improve security).
                services.AddCors(options =>
                    {
                        options.AddPolicy("CorsPolicy-public",
                            builder => builder.AllowAnyOrigin()   //WithOrigins and define a specific origin to be allowed (e.g. https://mydomain.com)
                                .AllowAnyMethod()
                                .AllowAnyHeader()
                        //.AllowCredentials()
                        .Build());
                    });
                #endregion

                if (Configuration["DDosAttackProtection:Enabled"] == "True")
                    services.AddSingleton(typeof(DDosAttackMonitoringService));

                #region "MVC and JSON options"
                //mvc service (set to ignore ReferenceLoopHandling in json serialization like Users[0].Account.Users)
                //in case you need to serialize entity children use commented out option instead
                services.AddMvc(option => option.EnableEndpointRouting = false)
            .AddNewtonsoftJson(options => { options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore; });  //NO entity classes' children serialization
                                                                                                                                                  //.AddNewtonsoftJson(ops =>
                                                                                                                                                  //{
                                                                                                                                                  //    ops.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Serialize;
                                                                                                                                                  //    ops.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                                                                                                                                                  //}); //WITH entity classes' children serialization
                #endregion

                #region "DI code"
                //general unitofwork injections
                services.AddTransient<IUnitOfWork, UnitOfWork>();

                //services injections
                services.AddTransient(typeof(AccountService<,>), typeof(AccountService<,>));
                services.AddTransient(typeof(UserService<,>), typeof(UserService<,>));
                services.AddTransient(typeof(AccountServiceAsync<,>), typeof(AccountServiceAsync<,>));
                services.AddTransient(typeof(UserServiceAsync<,>), typeof(UserServiceAsync<,>));
                services.AddTransient(typeof(DynamicFormsConfigurationService<,>), typeof(DynamicFormsConfigurationService<,>));
                services.AddTransient(typeof(DynamicFormsConfigurationServiceAsync<,>), typeof(DynamicFormsConfigurationServiceAsync<,>));
                services.AddTransient(typeof(WorkflowTeService<,>), typeof(WorkflowTeService<,>));
                services.AddTransient(typeof(WorkflowTeServiceAsync<,>), typeof(WorkflowTeServiceAsync<,>));
                services.AddTransient(typeof(WorkflowVersionServiceAsync<,>), typeof(WorkflowVersionServiceAsync<,>));
                services.AddTransient(typeof(WorkflowVersionService<,>), typeof(WorkflowVersionService<,>));


                services.AddWorkflow();
                services.AddWorkflowDSL();

                services.AddXamunWorkflow();

                //
                SetAdditionalDIServices(services);
                //...add other services
                //
                services.AddTransient(typeof(IService<,>), typeof(GenericService<,>));
                services.AddTransient(typeof(IServiceAsync<,>), typeof(GenericServiceAsync<,>));
                #endregion

                //data mapper services configuration
                services.AddAutoMapper(typeof(MappingProfile));

                #region Services
                services.AddScoped<IDapperServiceAsync, DapperServiceAsync>();
                #endregion

                #region "Swagger API"
                //Swagger API documentation
                services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo { Title = "AssetManagementProject.web API", Version = "v1" });
                    c.SwaggerDoc("v2", new OpenApiInfo { Title = "AssetManagementProject.web API", Version = "v2" });

                    // Add server
                    c.AddServer(new OpenApiServer()
                    {
                        Url = Configuration["ApiBaseUrl"]
                    });

                    //In Test project find attached swagger.auth.pdf file with instructions how to run Swagger authentication 
                    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                    {
                        Description = "Authorization header using the Bearer scheme",
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.ApiKey
                    });


                    c.AddSecurityRequirement(new OpenApiSecurityRequirement{
                        {
                            new OpenApiSecurityScheme{
                                Reference = new OpenApiReference{
                                    Id = "Bearer", //The name of the previously defined security scheme.
                                    Type = ReferenceType.SecurityScheme
                                }
                            },new List<string>()
                        }
                    });

                    c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                    {
                        Type = SecuritySchemeType.OAuth2,
                        Flows = new OpenApiOAuthFlows
                        {
                          AuthorizationCode = new OpenApiOAuthFlow
                          {
                              AuthorizationUrl = new Uri($"{Configuration["Authentication:IdentityServer4IP"]}/connect/authorize"),
                              TokenUrl = new Uri($"{Configuration["Authentication:IdentityServer4IP"]}/connect/token"),
                              Scopes = new Dictionary<string, string> {
                                { Configuration["Authentication:ApiScopeId"], Configuration["Authentication:ApiScopeName"] }
                              }
                          }
                        }
                    });
                    c.AddSecurityRequirement(new OpenApiSecurityRequirement{
                        {
                            new OpenApiSecurityScheme{
                                Reference = new OpenApiReference{
                                    Id = "oauth2", //The name of the previously defined security scheme.
                                    Type = ReferenceType.SecurityScheme
                                }
                            },new[]{Configuration["Authentication:OidcSwaggerUIClientId"] }
                        }
                    });

                    //c.DocumentFilter<api.infrastructure.filters.SwaggerSecurityRequirementsDocumentFilter>();
                });
                #endregion
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }
        }


        //call scaffolded class method to add DIs
        partial void SetAdditionalDIServices(IServiceCollection services);


        // This method gets called by the runtime
        // This method can be used to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UsePathBase(Configuration["BasePath"]);

            Log.Information("Startup::Configure");

            try
            {

                //Forwarded Headers Middleware should run before other middleware. This ordering ensures that the middleware relying on forwarded headers information can consume the header values for processing.
                app.UseForwardedHeaders();

                if (env.EnvironmentName == "Development")
                    app.UseDeveloperExceptionPage();
                else
                {
                    app.UseMiddleware<ExceptionHandler>();
                    //app.UseHsts();
                }

                //app.UseHttpsRedirection();
                //app.UseStaticFiles();
                //app.UseCookiePolicy();
                //app.UseRouting();
                //app.UseRequestLocalization();

                app.UseCors("CorsPolicy-public");  //apply to every request
                app.UseAuthentication(); //needs to be up in the pipeline, before MVC
                app.UseAuthorization();

                // app.UseSession();
                // app.UseResponseCompression();
                // app.UseResponseCaching()

                //Swagger API documentation
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint($"{Configuration["BasePath"]}/swagger/v1/swagger.json", "AssetManagementProject.web API V1");
                    c.SwaggerEndpoint($"{Configuration["BasePath"]}/swagger/v2/swagger.json", "AssetManagementProject.web API V2");
                    c.OAuthClientId(Configuration["Authentication:OidcSwaggerUIClientId"]);
                    c.OAuthAppName(Configuration["Authentication:OidcApiName"]);
                    c.OAuthUsePkce();
                    c.DisplayOperationId();
                    c.DisplayRequestDuration();
                });

                //Middlewares (orders of all middlewares(including custom) is very important)
                //1
                //NOTE:  this pipeline (1) is only used when integration tests run to populate empty
                //       requests RemoteIp address required for DDoS attacks prevention test
                if (Configuration["IntegrationTests"] == "True")
                    app.UseFakeRemoteIpAddressMiddlewareExtensions();
                //2
                if (Configuration["DDosAttackProtection:Enabled"] == "True")
                    app.UseDDosAttackStopMiddlewareExtensions();


                app.UseMvc();


                //migrations and seeds from json files
                using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
                {
                    if (Configuration["ConnectionStrings:UseInMemoryDatabase"] == "False" && !serviceScope.ServiceProvider.GetService<DefaultDbContext>().AllMigrationsApplied())
                    {
                        if (Configuration["ConnectionStrings:UseMigrationService"] == "True")
                            serviceScope.ServiceProvider.GetService<DefaultDbContext>().Database.Migrate();
                    }
                    //it will seed tables on aservice run from json files if tables empty
                    if (Configuration["ConnectionStrings:UseSeedService"] == "True")
                        serviceScope.ServiceProvider.GetService<DefaultDbContext>().EnsureSeeded();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }
        }
    }
}


namespace api.infrastructure.filters
{
    public class SwaggerSecurityRequirementsDocumentFilter : IDocumentFilter
    {
        public void Apply(OpenApiDocument document, DocumentFilterContext context)
        {
            document.SecurityRequirements = new List<OpenApiSecurityRequirement>
            {
                new OpenApiSecurityRequirement{
                    {
                        new OpenApiSecurityScheme{
                            Reference = new OpenApiReference{
                                Id = "Bearer", //The name of the previously defined security scheme.
                                Type = ReferenceType.SecurityScheme
                            }
                        },new List<string>()
                    }
                },
                new OpenApiSecurityRequirement{
                    {
                        new OpenApiSecurityScheme{
                            Reference = new OpenApiReference{
                                Id = "Basic", //The name of the previously defined security scheme.
                                Type = ReferenceType.SecurityScheme
                            }
                        },new List<string>()
                    }
                }
             };

        }
    }
}







