using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace educativeorg_models.ViewModels.Accounts
{
    public class PermissionViewModel
    {
        public Guid RoleId { get; set; }
        public List<ModulePermissionViewModel> ModulePermissions { get; set; }

    }

    public class ModulePermissionViewModel 
    {
        public string Module { get; set; }
        public List<ModulePermissions> Permissions { get; set; }
    }


    public class ModulePermissions 
    {
        public int PermissionId { get; set; }
        public string PermissionName { get; set; }
        public bool Granted { get; set; }
    }
}
