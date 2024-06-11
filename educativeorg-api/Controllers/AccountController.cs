using educativeorg_models;
using educativeorg_models.Helper;
using educativeorg_models.ViewModels;
using educativeorg_models.ViewModels.Accounts;
using educativeorg_services.Services.AccountServices;
using educativeorg_services.Services.SeederServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace educativeorg_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }
        [HasPermission(PermissionsSet.AddUsers)]
        [HttpGet]
        public async Task<string> GetResponse()
        {
            return "api working";
        }

        [HttpPost("[action]")]
        public async Task<ResponseViewModel<GetUserViewModel>> SignUp(SignUpViewModel input) 
        {
            return await _accountService.RegisterCompany(input);
        }


        [HttpPost("[action]")]
        public async Task<ResponseViewModel<LoginResponseViewModel>> SignIn(SignInViewModel input)
        {
            return await _accountService.SignIn(input);
        }

        


    }
}
