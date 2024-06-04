using Microsoft.AspNetCore.Authorization;

namespace educativeorg_api.Helper
{
    public class PermissionRequirement:IAuthorizationRequirement
    {
        public PermissionRequirement(string permission)
        {
            this.Permission = permission;
        }
        public string Permission { get; set; }

    }
}
