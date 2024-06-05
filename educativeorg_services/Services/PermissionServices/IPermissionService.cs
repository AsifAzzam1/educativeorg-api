using educativeorg_models.ViewModels;
using educativeorg_models.ViewModels.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace educativeorg_services.Services.PermissionServices
{
    public interface IPermissionService
    {
        Task<ResponseViewModel<PermissionViewModel>> GetPermissionsForRole(Guid roleId)
Task<ResponseViewModel<object>> SetPermissionsForRole(PermissionViewModel input);
    }
}
