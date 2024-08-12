using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagementProject.web.Domain.Interface
{
    public interface IDapperServiceAsync
    {
        Task<T> Get<T>(string sp, string cn, DynamicParameters parms, CommandType commandType);
        Task<IEnumerable<T>> GetAll<T>(string sp, string cn, DynamicParameters parms, CommandType commandType);
        Task Insert<T>(string sp, string cn, DynamicParameters parms, CommandType commandType);
        Task Update<T>(string sp, string cn, DynamicParameters parms, CommandType commandType);
        Task Delete<T>(string sp, string cn, DynamicParameters parms, CommandType commandType);
    }
}
