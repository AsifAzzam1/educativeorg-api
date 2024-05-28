using educativeorg_models.ViewModels;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static educativeorg_models.ViewModels.ExceptionResposne;

namespace educativeorg_api.Controllers
{
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ExceptionController : ControllerBase
    {
        [Route("exceptionhandler")]
        public ExceptionResposne Error()
        {
            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
            var exception = context?.Error; // Your exception
            var code = 500; // Internal Server Error by default

            if (exception is HttpStatusException httpException)
            {
                code = (int)httpException.Status;
            }

            Response.StatusCode = code;

            return new ExceptionResposne(exception);
        }
    }
}
