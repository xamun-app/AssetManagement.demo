using AssetManagementProject.web.Domain.Interface;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagementProject.web.Domain.Service
{
    public class DapperServiceAsync : IDapperServiceAsync
    {
        private readonly IConfiguration _config;

        public DapperServiceAsync(IConfiguration config)
        {
            _config = config;
        }

        public async Task<T> Get<T>(string sp, string cn, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            using var db = new SqlConnection(_config.GetConnectionString(cn));
            var result = await db.QueryAsync<T>(sp, parms, commandType: commandType);
            return result.FirstOrDefault();
        }

        public async Task<IEnumerable<T>> GetAll<T>(string sp, string cn, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            using var db = new SqlConnection(_config.GetConnectionString(cn));
            var result = await db.QueryAsync<T>(sp, parms, commandTimeout: 600, commandType: commandType);
            return result.ToList();
        }

        public async Task Insert<T>(string sp, string cn, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            using var db = new SqlConnection(_config.GetConnectionString(cn));
            var result = await db.ExecuteAsync(sp, parms, commandType: commandType);
        }

        public async Task Update<T>(string sp, string cn, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            using var db = new SqlConnection(_config.GetConnectionString(cn));
            var result = await db.ExecuteAsync(sp, parms, commandType: commandType);
        }

        public async Task Delete<T>(string sp, string cn, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            using var db = new SqlConnection(_config.GetConnectionString(cn));
            var result = await db.ExecuteAsync(sp, parms, commandType: commandType);
        }
    }
}
