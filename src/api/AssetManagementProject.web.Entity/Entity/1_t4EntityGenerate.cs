

 
 
 

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using AssetManagementProject.web.Entity;

namespace AssetManagementProject.web.Entity
{

        public partial class Asset : BaseEntity
    {
        [StringLength(150)]
        public string Name { get; set; } 
        [StringLength(250)]
        public string Description { get; set; } 
        public bool IsActive { get; set; } 
        [ForeignKey("Category")]
        public int CategoryId { get; set; } 
        [ForeignKey("Class")]
        public int ClassId { get; set; } 
        public virtual Category Category { get; set; }
        public virtual AssetClass AssetClass { get; set; }
    }
    public partial class Category : BaseEntity
    {
        [StringLength(150)]
        public string Name { get; set; } 
        [StringLength(250)]
        public string Description { get; set; } 
    }
    public partial class AssetClass : BaseEntity
    {
        [StringLength(150)]
        public string Name { get; set; } 
        [StringLength(250)]
        public string Description { get; set; } 
    }
    public partial class AssetMaintenance : BaseEntity
    {
        [ForeignKey("Asset")]
        public int AssetId { get; set; } 
        [StringLength(150)]
        public string Location { get; set; } 
        public decimal Cost { get; set; } 
        [StringLength(150)]
        public string Status { get; set; } 
        public DateTime LastMaintenanceDate { get; set; } 
        public DateTime NextMaintenanceDate { get; set; } 
        public virtual Asset Asset { get; set; }
    }
}

