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
        private readonly ISeedService _seedService;

        public AccountController(IAccountService accountService, ISeedService seedService)
        {
            _accountService = accountService;
            _seedService = seedService;
        }
        [HasPermission(educativeorg_models.PermissionsSet.AddUsers)]
        [HttpGet]
        public async Task<string> GetResponse()
        {
            return "api working";
        }

        [HttpPost("[action]")]
        public async Task<ResponseViewModel<GetUserViewModel>> SignUp(SignUpViewModel input) 
        {
            return await _accountService.SignUp(input);
        }


        [HttpPost("[action]")]
        public async Task<ResponseViewModel<LoginResponseViewModel>> SignIn(SignInViewModel input)
        {
            return await _accountService.SignIn(input);
        }

        [HttpGet("[action]")]
        public async Task<ResponseViewModel<object>> SeedPermissions()
        {
            return await _seedService.SeedPermissions();
        }
    }
}
