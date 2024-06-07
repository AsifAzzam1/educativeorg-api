using educativeorg_models;
using educativeorg_models.Helper;
using educativeorg_models.ViewModels;
using educativeorg_models.ViewModels.Accounts;
using educativeorg_services.Services.PermissionServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security;

namespace educativeorg_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionController : ControllerBase
    {
        private readonly IPermissionService _permissionService;

        public PermissionController(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        [HasPermission(PermissionsSet.GetPermissiosForRole)]
        [HttpGet("[action]/{RoleId}")]
        public async Task<ResponseViewModel<PermissionViewModel>> GetRolePermissions(Guid RoleId)
        {
            return await _permissionService.GetPermissionsForRole(RoleId);
        }
        //[HasPermission(PermissionsSet.SetPermissionsForRole)]
        [HttpPost("[action]")]
        public async Task<ResponseViewModel<object>> GetRolePermissions(PermissionViewModel input)
        {
            return await _permissionService.SetPermissionsForRole(input);
        }
    }
}
