using AutoMapper;
using AssetManagementProject.web.Entity;
using AssetManagementProject.web.Entity.UnitofWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagementProject.web.Domain.Service
{
    /// <summary>
    /// A WorkflowTe service
    /// </summary>
    public class WorkflowTeService<Tv, Te> : GenericService<Tv, Te>
                                        where Tv : WorkflowTeViewModel
                                        where Te : WorkflowTe
    {
        //DI must be implemented in specific service as well beside GenericService constructor
        public WorkflowTeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            if (_unitOfWork == null)
                _unitOfWork = unitOfWork;
            if (_mapper == null)
                _mapper = mapper;
        }
        //add here any custom service method or override generic service method
    }
}
