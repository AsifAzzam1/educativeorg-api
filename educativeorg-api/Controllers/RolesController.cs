using educativeorg_models.ViewModels.Accounts;
using educativeorg_models.ViewModels;
using educativeorg_services.Services.RoleServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace educativeorg_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpPost]
        public async Task<ResponseViewModel<object>> CreateRole(string RoleName)
        {
            return await _roleService.CreateRole(RoleName);
        }

        [HttpPut]
        public async Task<ResponseViewModel<object>> Update(RoleOutputViewModel Role)
        {
            return await _roleService.Update(Role);
        }

        [HttpGet]
        public async Task<ResponseViewModel<List<RoleOutputViewModel>>> GetAllRoles()
        {
            return await _roleService.GetAllRoles();
        }

        [HttpGet("{RoleId}")]
        public async Task<ResponseViewModel<RoleOutputViewModel>> GetRoleById(Guid RoleId) 
        {
            return await _roleService.GetRoleById(RoleId);
        }

        [HttpDelete("{RoleId}")]
        public async Task<ResponseViewModel<object>> Delete(Guid RoleId)
        {
            return await _roleService.DeleteRole(RoleId);
        }

       
    }
}
