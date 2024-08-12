using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace AssetManagementProject.web.Domain.Service
{
    public interface IServiceAsync<Tv, Te>
        where Tv : class
        where Te : class

    {
        Task<PageResult<Tv>> GetPage(int pageIndex = 0, 
                                          int pageSize = 10,
                                          string sortColumn = null, 
                                          string sortOrder = null, 
                                          string filterColumn = null, 
                                          string filterQuery = null);

        Task<IEnumerable<Tv>> GetAll();
        Task<int> Add(Tv obj);
        Task<int> Update(Tv obj);
        Task<int> Remove(int id);
        Task<Tv> GetOne(int id);
        Task<IEnumerable<Tv>> Get(Expression<Func<Te, bool>> predicate);
    }

}
