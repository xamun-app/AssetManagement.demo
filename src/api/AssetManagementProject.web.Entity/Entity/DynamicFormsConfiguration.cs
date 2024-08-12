using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagementProject.web.Entity
{
    public class DynamicFormsConfiguration : BaseEntity
    {
        [Required]
        [StringLength(50)]
        public string  Name { get; set; }
        [Required]
        public string Value { get; set; }
        [Required]
        public double Version { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}
