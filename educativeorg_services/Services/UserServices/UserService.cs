using AutoMapper;
using educativeorg_data.Data;
using educativeorg_data.Helpers;
using educativeorg_models.Helper;
using educativeorg_models.Models;
using educativeorg_models.ViewModels;
using educativeorg_models.ViewModels.Accounts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static educativeorg_models.ViewModels.ExceptionResposne;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace educativeorg_services.Services.UserServices
{
    public class UserService:IUserService
    {
        private readonly EducativeOrgDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpcontextaccessor;

        public UserService(EducativeOrgDbContext context, IMapper mapper, UserManager<ApplicationUser> userMananger, IHttpContextAccessor httpcontextaccessor)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userMananger;
            _httpcontextaccessor = httpcontextaccessor;
        }

        public async Task<ResponseViewModel<GetUserViewModel>> CreateUser(SignUpViewModel input)
        {
            try
            {
                using (var transaction = _context.Database.BeginTransaction()) 
                {
                    try
                    {
                        var userexist = await _context.Users.FirstOrDefaultAsync(_ => _.UserName == input.Email);
                        if (userexist != null)
                            throw new HttpStatusException(HttpStatusCode.BadRequest, $"Username with email {input.Email} already exist");

                        if (input.CompanyInfo == null || input.CompanyInfo.Id == null || input.CompanyInfo.Id == Guid.Empty)
                            throw new HttpStatusException(HttpStatusCode.BadRequest, "Company Information not provided");



                        ApplicationUser userToadd = new ApplicationUser
                        {
                            UserName = input.Email,
                            Email = input.Email,
                            Active = true,
                            FirstName = input.FirstName,
                            LastName = input.LastName,
                            CompanyId = input.CompanyInfo.Id!.Value
                        };

                        var userRes = await _userManager.CreateAsync(userToadd, input.Password);
                        if (!userRes.Succeeded)
                            throw new HttpStatusException(HttpStatusCode.BadGateway, userRes.Errors.First().Description);

                        var role = _context.Roles.First("Role not found", _ => _.Id == input.RoleId);
                        var rolesRes = await _userManager.AddToRoleAsync(userToadd, role.Name);
                        if (!rolesRes.Succeeded)
                            throw new HttpStatusException(HttpStatusCode.BadRequest, rolesRes.Errors.First().Description);

                        transaction.Commit();
                        return new ResponseViewModel<GetUserViewModel>
                        {
                            Data = _mapper.Map<GetUserViewModel>(userToadd),
                            Message = "User Created"
                        };
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<ResponseViewModel<GetUserViewModel>> GetById(Guid Id) 
        {
            try
            {
                var User = await _context.Users.Where(_=>_.Id == Id).Include(_=>_.Roles).FirstAsync("User not found");
                
                return new ResponseViewModel<GetUserViewModel>
                {
                    Message = "User found",
                    Data = _mapper.Map<GetUserViewModel>(User)
                };
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<ResponseViewModel<GetUserViewModel>> Update(GetUserViewModel user) 
        {
            try
            {
                using var transaction = _context.Database.BeginTransaction();
                try
                {
                    var appUser = await _context.Users.FirstAsync("User not found", _ => _.Id == user.Id);
                    appUser.FirstName = user.FirstName;
                    appUser.LastName = user.LastName;
                    appUser.Email = user.Email;
                    _context.Users.Update(appUser);

                    if (user.UserRole != null)
                    {
                        foreach (var item in user.UserRole)
                        {
                            if (item.Active && !(await _userManager.IsInRoleAsync(appUser, item.Value)))
                            {
                                var Addres = await _userManager.AddToRoleAsync(appUser, item.Value);
                                if (!Addres.Succeeded)
                                    throw new HttpStatusException(System.Net.HttpStatusCode.BadRequest, Addres.Errors.First().Description);
                            }
                            else if (!item.Active && (await _userManager.IsInRoleAsync(appUser, item.Value)))
                            {
                                var removeRes = await _userManager.RemoveFromRoleAsync(appUser, item.Value);
                                if (!removeRes.Succeeded)
                                    throw new HttpStatusException(System.Net.HttpStatusCode.BadRequest, removeRes.Errors.First().Description);
                            }
                        }
                    }
                    _context.SaveChanges();
                    transaction.Commit();
                    return new ResponseViewModel<GetUserViewModel>
                    {
                        Data = _mapper.Map<GetUserViewModel>(appUser),
                        Message = "User Updated"
                    };
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<ResponseViewModel<GetUserViewModel>> ToggleStatus(Guid userId) 
        {
            try
            {
                var user = await _context.Users.FirstAsync("user not found", _ => _.Id == userId);
                user.Active = false;
                _context.Entry(user).Property(_=>_.Active).IsModified = true;
                _context.SaveChanges();

                return new ResponseViewModel<GetUserViewModel>
                {
                    Message = "User Deleted",
                    Data = _mapper.Map<GetUserViewModel>(user)
                };
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<ResponseViewModel<PaginateResponseModel<GetUserViewModel>>> GetAll(FilterViewModel filter) 
        {
            try
            {
                var companyId = _httpcontextaccessor.GetCompanyId()!.Value;
                var query = _context.Users.Where(_=>_.Active && _.CompanyId == companyId);

                var res = new PaginateResponseModel<GetUserViewModel>
                {
                    TotalRecords = query.Count()
                };

                if (filter.Status.HasValue)
                    query = query.Where(_ => _.Active == filter.Status.Value);

                if (!string.IsNullOrWhiteSpace(filter.Query))
                {
                    query = query.Where(_=>_.FirstName.Contains(filter.Query) ||
                                            _.LastName.Contains(filter.Query) ||
                                            _.Email!.Contains(filter.Query));
                }

                if (!string.IsNullOrWhiteSpace(filter.SortBy))
                    query = filter.SortDesc != null && filter.SortDesc!.Value ? query.OrderByDescending(_=> filter.SortBy) : query = query.OrderBy(_=> filter.SortBy);
               
                if (!string.IsNullOrWhiteSpace(filter.SortBy))
                {
                    if(!filter.SortDesc ?? false) 
                    {
                        query = filter.SortBy switch
                        {
                            "FirstName" => query.OrderBy(_ => _.FirstName),
                            "LastName" => query.OrderBy(_ => _.LastName),
                            "Email" => query.OrderBy(_ => _.Email),
                            _ => query.OrderBy(_ => _.FirstName),
                        };
                    }
                    else
                    {
                        query = filter.SortBy switch
                        {
                            "FirstName" => query.OrderByDescending(_ => _.FirstName),
                            "LastName" => query.OrderByDescending(_ => _.LastName),
                            "Email" => query.OrderByDescending(_ => _.Email),
                            _ => query.OrderByDescending(_ => _.FirstName),
                        };
                    }
                }

                if (filter.PageSize == -1)
                    res.Data = _mapper.Map<List<GetUserViewModel>>(await query.ToListAsync());
                else
                    res.Data = _mapper.Map<List<GetUserViewModel>>(await query.Skip((filter.PageNo - 1) * filter.PageNo).Take(filter.PageSize).ToListAsync());
                
                res.RecordsAfterFilter = res.Data.Count;

                return new ResponseViewModel<PaginateResponseModel<GetUserViewModel>> 
                {
                    Message = "Data found",
                    Data = res
                };
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
