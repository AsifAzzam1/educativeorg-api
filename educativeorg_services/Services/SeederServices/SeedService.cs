using educativeorg_data.Data;
using educativeorg_data.Helpers;
using educativeorg_models;
using educativeorg_models.Models;
using educativeorg_models.ViewModels;
using educativeorg_models.ViewModels.Accounts;
using educativeorg_services.Services.CompanyServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static educativeorg_models.ViewModels.ExceptionResposne;

namespace educativeorg_services.Services.SeederServices
{
    public class SeedService:ISeedService
    {
        private readonly EducativeOrgDbContext _context;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly ICompanyService _companyService;
        private readonly UserManager<ApplicationUser> _userManager;

        public SeedService(EducativeOrgDbContext context, RoleManager<ApplicationRole> roleManager, ICompanyService companyService, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _roleManager = roleManager;
            _companyService = companyService;
            _userManager = userManager;
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

        public async Task<ResponseViewModel<object>> SeedBaseData() 
        {
            try
            {
                try
                {
                    var seePermissionRes = await SeedPermissions();

                   
                    ApplicationRole? superAdminRole = _context.Roles.FirstOrDefault(_ => _.Name == "super_admin");
                    if (superAdminRole == null)
                    {
                        superAdminRole = new ApplicationRole
                        {
                            Name = "super_admin",
                            Active = true
                        };
                        var roleRes = await _roleManager.CreateAsync(superAdminRole);
                        if (!roleRes.Succeeded)
                            throw new HttpStatusException(System.Net.HttpStatusCode.BadRequest, roleRes.Errors.First().Description);
                    }

                    var allPermission = _context.Permissions.ToList();
                    var oldPermissions = _context.RolePermissions.Where(_ => _.RoleId == superAdminRole.Id).ToList();
                    if (oldPermissions.Count > 0)
                        _context.RolePermissions.RemoveRange(oldPermissions);

                    List<RolePermissions> newPer = new();
                    foreach (var item in allPermission)
                    {
                        newPer.Add(new RolePermissions
                        {
                            PermissionId = item.Id,
                            RoleId = superAdminRole.Id,
                        });
                    }

                    if(newPer.Count > 0)
                        _context.RolePermissions.AddRange(newPer);


                    var company = _context.Companies.FirstOrDefault(_ => _.Title == "Educative Org Admin BU");
                    if(company == null)
                    {
                        var companyRes = await _companyService.AddCompany(new CompanyViewModel
                        {
                            Title = "Educative Org Admin BU",
                            Address = "Lahore Pakistan",
                            Phone = "+923315728416"
                        });
                        company = _context.Companies.FirstOrDefault(_ => _.Title == "Educative Org Admin BU");
                    }
                    var newUser = _context.Users.FirstOrDefault(_ => _.UserName == "edadmin@educativeorg.com");

                    if(newUser == null) 
                    {
                        newUser = new ApplicationUser
                        {
                            FirstName = "Ed",
                            LastName = "Admin",
                            Email = "admin@eduorg.com",
                            UserName = "admin@eduorg.com",
                            Active = true,
                            EmailConfirmed = true,
                            CompanyId = company!.Id,
                        };

                        var userRes = await _userManager.CreateAsync(newUser,"Eduorg1122!");
                        if (!userRes.Succeeded)
                            throw new HttpStatusException(System.Net.HttpStatusCode.BadRequest,
                                userRes.Errors.First().Description);
                    }
                    if (!await _userManager.IsInRoleAsync(newUser, superAdminRole!.Name!))
                    {
                        var addroleRes = await _userManager.AddToRoleAsync(newUser, superAdminRole!.Name!);
                        if(!addroleRes.Succeeded)
                            throw new HttpStatusException(System.Net.HttpStatusCode.BadRequest, addroleRes.Errors.First().Description);
                    }

                    company.InitiazlizeBaseColumns(newUser.Id);
                    _context.SaveChanges();
                    return new ResponseViewModel<object> { };
                }
                catch (Exception)
                {
                    throw;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
