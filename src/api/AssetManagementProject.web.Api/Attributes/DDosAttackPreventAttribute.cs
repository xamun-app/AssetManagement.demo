using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssetManagementProject.web.Api.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class DDosAttackProtectedAttribute : Attribute
    {
    }
}
