using educativeorg_models.ViewModels;
using educativeorg_models.ViewModels.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace educativeorg_services.Services.RoleServices
{
    public interface IRoleService
    {
        Task<ResponseViewModel<object>> CreateRole(string RoleName);
        Task<ResponseViewModel<object>> DeleteRole(Guid roleId);
        Task<ResponseViewModel<List<RoleOutputViewModel>>> GetAllRoles();
        Task<ResponseViewModel<RoleOutputViewModel>> GetRoleById(Guid roleId);
        Task<ResponseViewModel<object>> Update(RoleOutputViewModel Role);
    }
}
