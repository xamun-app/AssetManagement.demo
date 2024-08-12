using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AssetManagementProject.web.Entity
{
    /// <summary>
    /// Entities use for Worflow
    /// </summary>
    public class WorkflowTe : BaseEntity
    {
        [Required]
        [StringLength(150)]
        public string Name { get; set; }  
        [Required]
        public string WorkflowCoreJson { get; set; }  
        public string WorkflowObjectParamJson { get; set; }  
        [Required]
        public string ModuleName { get; set; }  
        public bool IsDeleted { get; set; }  
        public int? ApplicationWorkflowId { get; set; }  
    }


}
