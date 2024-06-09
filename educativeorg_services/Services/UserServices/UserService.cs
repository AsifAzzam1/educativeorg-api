using AutoMapper;
using educativeorg_data.Data;
using educativeorg_data.Helpers;
using educativeorg_models.Models;
using educativeorg_models.ViewModels;
using educativeorg_models.ViewModels.Accounts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static educativeorg_models.ViewModels.ExceptionResposne;

namespace educativeorg_services.Services.UserServices
{
    public class UserService:IUserService
    {
        private readonly EducativeOrgDbContext _context;
        private readonly UserManager<ApplicationUser> _userMananger;
        private readonly IMapper _mapper;

        public UserService(EducativeOrgDbContext context, IMapper mapper, UserManager<ApplicationUser> userMananger)
        {
            _context = context;
            _mapper = mapper;
            _userMananger = userMananger;
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
                            if (item.Active && !(await _userMananger.IsInRoleAsync(appUser, item.Value)))
                            {
                                var Addres = await _userMananger.AddToRoleAsync(appUser, item.Value);
                                if (!Addres.Succeeded)
                                    throw new HttpStatusException(System.Net.HttpStatusCode.BadRequest, Addres.Errors.First().Description);
                            }
                            else if (!item.Active && (await _userMananger.IsInRoleAsync(appUser, item.Value)))
                            {
                                var removeRes = await _userMananger.RemoveFromRoleAsync(appUser, item.Value);
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

        public async Task<ResponseViewModel<object>> ToggleStatus(Guid userId) 
        {
            try
            {
                var user = await _context.Users.FirstAsync("user not found", _ => _.Id == userId);
                user.Active = false;
                _context.Entry(user).Property(_=>_.Active).IsModified = true;
                _context.SaveChanges();

                return new ResponseViewModel<object>
                {
                    Message = "User Deleted"
                };
            }
            catch (Exception)
            {

                throw;
            }
        }



    }
}
