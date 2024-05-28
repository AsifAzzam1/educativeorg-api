using educativeorg_models.ViewModels;
using educativeorg_models.ViewModels.Accounts;
using educativeorg_services.Services.AccountServices;
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
    }
}
