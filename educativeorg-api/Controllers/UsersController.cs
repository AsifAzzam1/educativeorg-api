using educativeorg_models.ViewModels;
using educativeorg_models.ViewModels.Accounts;
using educativeorg_services.Services.UserServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace educativeorg_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<ResponseViewModel<GetUserViewModel>> CreateUser(SignUpViewModel input)
        {
            return await _userService.CreateUser(input);
        }

        [HttpGet("{id}")]
        public async Task<ResponseViewModel<GetUserViewModel>> GetById(Guid id) 
        {
            return await _userService.GetById(id);
        }

        [HttpPut]
        public async Task<ResponseViewModel<GetUserViewModel>> Update(GetUserViewModel input)
        {
            return await _userService.Update(input);
        }

        [HttpGet("[action]/{id}")]
        public async Task<ResponseViewModel<GetUserViewModel>> ChangeStatus(Guid id)
        {
            return await _userService.ToggleStatus(id);
        }

        [HttpGet]
        public async Task<ResponseViewModel<PaginateResponseModel<GetUserViewModel>>> ChangeStatus([FromQuery]FilterViewModel filter)
        {
            return await _userService.GetAll(filter);
        }

    }
}
