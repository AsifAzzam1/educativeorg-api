using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace educativeorg_models.Helper
{
    public class HasPermissionAttribute:AuthorizeAttribute
    {
        public HasPermissionAttribute(PermissionsSet permission)
            :base(policy:permission.ToString())
        {
            
        }
    }
}
