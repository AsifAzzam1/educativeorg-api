using educativeorg_data.Data;
using educativeorg_models.Models;
using educativeorg_models.ViewModels.Accounts;
using educativeorg_models.ViewModels;
using educativeorg_models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using educativeorg_data.Helpers;

namespace educativeorg_services.Services.PermissionServices
{
    public class PermissionService : IPermissionService
    {
        private readonly EducativeOrgDbContext _context;

        public PermissionService(EducativeOrgDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseViewModel<PermissionViewModel>> GetPermissionsForRole(Guid roleId)
        {
            try
            {
                ApplicationRole role = await _context.Roles.Where(_ => _.Id == roleId).Include(_ => _.Permissions).FirstAsync("Role not found");

                List<string> allPermissions = _context.Permissions.Select(_ => _.Name).ToList();
                //List<string> deniedPermissions = allPermissions.Except(role.Permissions.Select(_=>_.Name)).ToList();
                List<string> grantedPermissions = allPermissions.Union(role.Permissions.Select(_ => _.Name)).ToList();


                PermissionViewModel result = new PermissionViewModel
                {
                    RoleId = roleId,
                    ModulePermissions = EducativeOrg_Constants.Permissions
                                                              .Select(permission => new ModulePermissionViewModel
                                                              {
                                                                  Module = permission.Key.ToString(),
                                                                  Permissions = permission.Value.Select(_ => new ModulePermissions
                                                                  {
                                                                      PermissionId = (int)_,
                                                                      PermissionName = _.ToString(),
                                                                      Granted = grantedPermissions.Contains(_.ToString())
                                                                  }).ToList()
                                                              }).ToList()
                };
                


                return new ResponseViewModel<PermissionViewModel>
                {
                    Message = "Data Found",
                    Data = result
                };
            }
            catch (Exception)
            {

                throw;
            }
        }


        public async Task<ResponseViewModel<object]>> SetPermissionsForRole(PermissionViewModel input)
        {
            try
            {
                var strategy = _context.Database.CreateExecutionStrategy();
                return await strategy.Execute(async () => {

                    var oldPermissions = _context.RolePermissions.Where(_=>_.RoleId == input.RoleId).ToList();
                    if(oldPermissions.Count > 0)
                        _context.RolePermissions.RemoveRange(oldPermissions);

                    var permissions = input.ModulePermissions.SelectMany(_ => _.Permissions)
                                                             .Where(_=>_.Granted)
                                                             .Select(_=> new RolePermissions 
                                                             {
                                                                PermissionId = _.PermissionId,
                                                                RoleId = input.RoleId,
                                                             }).ToList();

                    if (permissions.Count > 0)
                        _context.RolePermissions.AddRange(permissions);

                    _context.SaveChanges();

                    return new ResponseViewModel<object>
                    {
                        Message = "Permissions Granted",
                    };
                });



                
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
