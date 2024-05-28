using AutoMapper;
using educativeorg_data.Data;
using educativeorg_models.Models;
using educativeorg_models.ViewModels;
using educativeorg_models.ViewModels.Accounts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static educativeorg_models.ViewModels.ExceptionResposne;

namespace educativeorg_services.Services.AccountServices
{
    public class AccountService: IAccountService
    {
        private readonly EducativeOrgDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public AccountService(EducativeOrgDbContext context, UserManager<ApplicationUser> userManager, IMapper mapper, RoleManager<ApplicationRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
            _roleManager = roleManager;
        }

        public async Task<ResponseViewModel<GetUserViewModel>> SignUp(SignUpViewModel input) 
        {
            try
            {
                var userexist = await _context.Users.FirstOrDefaultAsync(_=>_.UserName == input.Email);
                if (userexist != null)
                    throw new HttpStatusException(HttpStatusCode.BadRequest, $"Username with email {input.Email} already exist");

                ApplicationUser userToadd = new ApplicationUser 
                {
                    UserName = input.Email,
                    Email = input.Email,
                    Active = true,
                    FirstName = input.FirstName, 
                    LastName = input.LastName,
                };

                var userRes = await _userManager.CreateAsync(userToadd,input.Password);
                if (!userRes.Succeeded)
                    throw new HttpStatusException(HttpStatusCode.BadGateway,userRes.Errors.First().Description);

                var rolesRes = await _userManager.AddToRoleAsync(userToadd, input.RoleName);
                if (!rolesRes.Succeeded)
                    throw new HttpStatusException(HttpStatusCode.BadRequest, rolesRes.Errors.First().Description);



                return new ResponseViewModel<GetUserViewModel>
                {
                    Data = _mapper.Map<GetUserViewModel>(userToadd),
                    Message = "User Created"
                };
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<ResponseViewModel<object>> SignIn(SignInViewModel input) 
        {
            return new ResponseViewModel<object> { };
        }
    }
}
