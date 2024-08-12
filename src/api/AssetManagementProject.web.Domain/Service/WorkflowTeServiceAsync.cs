using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AssetManagementProject.web.Entity;
using AssetManagementProject.web.Entity.UnitofWork;

namespace AssetManagementProject.web.Domain.Service
{
   /// <summary>
    /// A WorkflowTe service
    /// </summary>
    public class WorkflowTeServiceAsync<Tv, Te> : GenericServiceAsync<Tv, Te>
                                        where Tv : WorkflowTeViewModel
                                        where Te : WorkflowTe
    {
        //DI must be implemented in specific service as well beside GenericService constructor
        public WorkflowTeServiceAsync(IUnitOfWork unitOfWork, IMapper mapper)
        {
            if (_unitOfWork == null)
                _unitOfWork = unitOfWork;
            if (_mapper == null)
                _mapper = mapper;
        }
        //add here any custom service method or override generic service method
        public async Task<int> DeleteAll()
        {
            string sql = "DELETE FROM [dbo].[WorkflowTes]";

            int records = await _unitOfWork.GetRepositoryAsync<WorkflowTe>().ExecuteSql(sql);
            return records;
        }
        public async Task<int> DeleteAllByAppWorkflowId(int appWorkflowId)
        {
            string sql = $"DELETE FROM [dbo].[WorkflowTes] WHERE ApplicationWorkflowId={appWorkflowId}";

            int records = await _unitOfWork.GetRepositoryAsync<WorkflowTe>().ExecuteSql(sql);
            return records;
        }

    }
 }
