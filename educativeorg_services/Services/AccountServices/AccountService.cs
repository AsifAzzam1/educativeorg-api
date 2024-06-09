using AutoMapper;
using educativeorg_data.Data;
using educativeorg_data.Helpers;
using educativeorg_models;
using educativeorg_models.Models;
using educativeorg_models.ViewModels;
using educativeorg_models.ViewModels.Accounts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
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

                var role = _context.Roles.First("Role not found", _ => _.Id == input.RoleId);
                var rolesRes = await _userManager.AddToRoleAsync(userToadd, role.Name);
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

        public async Task<ResponseViewModel<LoginResponseViewModel>> SignIn(SignInViewModel input) 
        {
            var user = await _context.Users.FirstAsync("Invalid Credentials",_ =>_.UserName == input.UserName);

            var credentials_verified = await _userManager.CheckPasswordAsync(user, input.Password);
            if (!credentials_verified)
                throw new HttpStatusException(HttpStatusCode.BadRequest,"Invalid Credentials");

            var roles = _context.UserRoles.Where(_ => _.UserId == user.Id).ToList();

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim(ClaimTypes.Role,JsonConvert.SerializeObject(roles.Select(_=>_.RoleId).ToList())),
                new Claim(JwtRegisteredClaimNames.Iss,"https://localhost:7236"),
            };

            var Token = GenerateToken(claims, input.RememberMe);
            var test = (Token.ValidTo - Token.ValidFrom).TotalSeconds.ToString();
            return new ResponseViewModel<LoginResponseViewModel>
            {
                Message = "SignIn Successfull",
                Data = new LoginResponseViewModel
                {
                    UserId = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Expiry = (Token.ValidTo - Token.ValidFrom).TotalSeconds.ToString(),
                    Token = new JwtSecurityTokenHandler().WriteToken(Token)
        }
            };
        }

        private JwtSecurityToken GenerateToken(Claim[] claims,bool RememberMe) 
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(EducativeOrg_Constants.JWtkey));
            var token = new JwtSecurityToken(
                issuer: "",
                audience: "",
                claims: claims,
                notBefore: DateTime.Now,
                expires: RememberMe ? DateTime.Now.AddDays(15) : DateTime.Now.AddHours(1),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
                );
            return token;
        }
    }
}
