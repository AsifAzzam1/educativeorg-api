using educativeorg_data.Data;
using educativeorg_models;
using educativeorg_models.Models;
using educativeorg_models.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace educativeorg_services.Services.SeederServices
{
    public class SeedService:ISeedService
    {
        private readonly EducativeOrgDbContext _context;

        public SeedService(EducativeOrgDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseViewModel<object>> SeedPermissions() 
        {
            var oldPermissions = await _context.Permissions.ToListAsync();
            


            var permissions = Enum.GetValues<PermissionsSet>()
                                  .Select(_ => new Permissions
                                  {
                                      Id = (int)_,
                                      Name = _.ToString(),
                                  }).ToList();


            var newPermissions = permissions.Where(_ => !oldPermissions.Any(x => x.Name.ToLower() == _.Name.ToLower())).ToList();

            var removedPermissions = oldPermissions.Where(_ => !permissions.Any(x => x.Name.ToLower() == _.Name.ToLower())).ToList();

            var rolePermissions = _context.RolePermissions.ToList();

            if (newPermissions.Count > 0)
                _context.Permissions.AddRange(newPermissions);

            List<RolePermissions> RolePermissionTOdelete = new();

            if (removedPermissions.Count > 0) 
            {
                foreach (var item in removedPermissions)
                {
                    var rolePermisson = rolePermissions.Where(_ => _.PermissionId == item.Id).ToList();
                    if (rolePermissions.Count > 0)
                        RolePermissionTOdelete.AddRange(rolePermisson);
                }
                if(RolePermissionTOdelete.Count > 0)
                    _context.RolePermissions.RemoveRange(RolePermissionTOdelete);
                _context.Permissions.RemoveRange(removedPermissions);
            }
                

            _context.SaveChanges();
            //_context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Permissions ON;");
            //_context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Permissions OFF");
            return new ResponseViewModel<object> { Message = "Permissions Seeded"};
        }
    }
}
