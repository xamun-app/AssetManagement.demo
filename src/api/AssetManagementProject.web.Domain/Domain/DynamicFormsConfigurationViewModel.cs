using AssetManagementProject.web.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagementProject.web.Domain
{
    public class DynamicFormsConfigurationViewModel: BaseDomain
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public double Version { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}
