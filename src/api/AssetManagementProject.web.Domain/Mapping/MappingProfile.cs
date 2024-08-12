using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using AssetManagementProject.web.Entity;

namespace AssetManagementProject.web.Domain.Mapping
{
    public partial class MappingProfile : Profile
    {
        /// <summary>
        /// Create automap mapping profiles
        /// </summary>
        public MappingProfile()
        {
            CreateMap<AccountViewModel, Account>();
            CreateMap<Account, AccountViewModel>();
            CreateMap<UserViewModel, User>()
                .ForMember(dest => dest.DecryptedPassword, opts => opts.MapFrom(src => src.Password))
                .ForMember(dest => dest.Roles, opts => opts.MapFrom(src => string.Join(";", src.Roles)));
            CreateMap<User, UserViewModel>()
                .ForMember(dest => dest.Password, opts => opts.MapFrom(src => src.DecryptedPassword))
                .ForMember(dest => dest.Roles, opts => opts.MapFrom(src => src.Roles.Split(";", StringSplitOptions.RemoveEmptyEntries)));
            CreateMap<DynamicFormsConfigurationViewModel, DynamicFormsConfiguration>();
            CreateMap<DynamicFormsConfiguration, DynamicFormsConfigurationViewModel>();

            CreateMap<WorkflowTeViewModel, WorkflowTe>();
            CreateMap<WorkflowTe, WorkflowTeViewModel>();
            CreateMap<WorkflowVersionViewModel, WorkflowVersion>();
            CreateMap<WorkflowVersion, WorkflowVersionViewModel>();

            //call code in partial scaffolded function
            SetAddedMappingProfile();
        }

        //to call scaffolded method
        partial void SetAddedMappingProfile();

    }





}
