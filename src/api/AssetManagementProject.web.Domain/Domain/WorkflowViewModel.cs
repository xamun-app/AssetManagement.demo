using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Newtonsoft.Json;

namespace AssetManagementProject.web.Domain
{
    
	public partial class WorkflowTeViewModel : BaseDomain
    {
	  		public string Name { get; set; } 
			public string WorkflowCoreJson { get; set; } 
			public string WorkflowObjectParamJson { get; set; } 
			public string ModuleName { get; set; } 
			public bool IsDeleted { get; set; } 
			public int ApplicationWorkflowId { get; set; } 
	} 

	public partial class WorkflowVersionViewModel : BaseDomain
    {
	  		public int Version { get; set; } 
	} 
	
	public partial class ProcessWorkflowViewModel
    {
        public string WorkflowDefinitionJson { get; set; }
        public string WorkflowObjectParamJson { get; set; }
    }
}
