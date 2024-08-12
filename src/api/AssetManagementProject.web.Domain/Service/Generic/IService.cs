using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;


namespace AssetManagementProject.web.Domain.Service
{
    public interface IService<Tv, Te> 
        where Tv: class 
        where Te : class

    {
        PageResult<Tv> GetPage(int pageIndex = 0, 
                                int pageSize = 10,
                                string sortColumn = null, 
                                string sortOrder = null, 
                                string filterColumn = null, 
                                string filterQuery = null);

        IEnumerable<Tv> GetAll();
        int Add(Tv obj);
        int Update(Tv obj);
        int Remove(int id); 
        Tv GetOne(int id);
        IEnumerable<Tv> Get(Expression<Func<Te, bool>> predicate);
    }
}
