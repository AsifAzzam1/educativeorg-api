using educativeorg_models.ViewModels;
using educativeorg_services.Services.SeederServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace educativeorg_api.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("api/[controller]")]
    [ApiController]
    public class SeederController : ControllerBase
    {
        private readonly ISeedService _seedService;

        public SeederController(ISeedService seedService)
        {
            _seedService = seedService;
        }

        [HttpGet("[action]")]
        public async Task<ResponseViewModel<object>> SeedPermissions()
        {
            return await _seedService.SeedPermissions();
        }
    }
}
