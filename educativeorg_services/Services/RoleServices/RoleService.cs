using AutoMapper;
using educativeorg_data.Data;
using educativeorg_data.Helpers;
using educativeorg_models.Models;
using educativeorg_models.ViewModels;
using educativeorg_models.ViewModels.Accounts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static educativeorg_models.ViewModels.ExceptionResposne;

namespace educativeorg_services.Services.RoleServices
{
    public class RoleService : IRoleService
    {
        private readonly EducativeOrgDbContext _context;
        private readonly IMapper _mapper;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public RoleService(EducativeOrgDbContext context, IMapper mapper, RoleManager<ApplicationRole> roleManager)
        {
            _context = context;
            _mapper = mapper;
            _roleManager = roleManager;
        }

        public async Task<ResponseViewModel<List<RoleOutputViewModel>>> GetAllRoles()
        {
            try
            {
                List<ApplicationRole> roles = await _context.Roles.ToListAsync();

                return new ResponseViewModel<List<RoleOutputViewModel>>
                {
                    Message = "Role Found",
                    Data = _mapper.Map<List<RoleOutputViewModel>>(roles)

                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ResponseViewModel<object>> CreateRole(string RoleName)
        {
            try
            {
                var roleRes = await _roleManager.CreateAsync(new ApplicationRole { Name = RoleName, Active = true });
                if (!roleRes.Succeeded)
                    throw new HttpStatusException(System.Net.HttpStatusCode.InternalServerError, roleRes.Errors.First().Description);

                return new ResponseViewModel<object>
                {
                    Message = "Role Found",

                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ResponseViewModel<RoleOutputViewModel>> GetRoleById(Guid roleId) 
        {
            try
            {
                var role = await _context.Roles.IgnoreQueryFilters().FirstAsync("Role not found",_ => _.Id == roleId);
                return new ResponseViewModel<RoleOutputViewModel>
                {
                    Message = "Role Found",
                    Data = _mapper.Map<RoleOutputViewModel>(role)

                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ResponseViewModel<object>> DeleteRole(Guid roleId)
        {
            try
            {
                var strategy = _context.Database.CreateExecutionStrategy();
                return await strategy.Execute(async () =>
                {
                    var role = _context.Roles.First("Role not found",_=>_.Id == roleId);

                    var rolePermissions = await _context.RolePermissions.Where(_ => _.RoleId == roleId).ToListAsync();
                    if (rolePermissions.Count > 0)
                        _context.RolePermissions.RemoveRange(rolePermissions);

                    var roleres = await _roleManager.DeleteAsync(role);
                    if (roleres.Succeeded)
                        throw new HttpStatusException(System.Net.HttpStatusCode.InternalServerError,roleres.Errors.First().Description);

                    return new ResponseViewModel<object>
                    {
                        Message = "Role Delted",
                    };
                });
                    
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ResponseViewModel<object>> Update(RoleOutputViewModel Role)
        {
            try
            {
                var role = _context.Roles.First("Role not found", _ => _.Id == Role.Id);
                role.Name = Role.Name;

                var updateRes = await _roleManager.UpdateAsync(role);
                if (!updateRes.Succeeded)
                    throw new HttpStatusException(System.Net.HttpStatusCode.InternalServerError, updateRes.Errors.First().Description);

                return new ResponseViewModel<object>
                {
                    Message = "Role Updated",

                };
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
