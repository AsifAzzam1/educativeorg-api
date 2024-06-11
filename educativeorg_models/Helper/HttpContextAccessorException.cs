using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace educativeorg_models.Helper
{
    public static class HttpContextAccessorException
    {
        public static string GetBaseUrl(this IHttpContextAccessor httpContextAccessor)
        {
            var request = httpContextAccessor.HttpContext.Request;
            var baseurl = $"{request.Scheme}://{request.Host}{request.PathBase}";
            return baseurl;
        }

        public static Guid? GetUserId(this IHttpContextAccessor httpContextAccessor)
        {
            var data = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(data))
                return null;

            var userid = new Guid(data);
            return userid;
        }

        public static Guid? GetCompanyId(this IHttpContextAccessor httpContextAccessor)
        {
            var data = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.System);
            if (string.IsNullOrWhiteSpace(data))
                return null;

            var userid = new Guid(data);
            return userid;
        }
    }
}
