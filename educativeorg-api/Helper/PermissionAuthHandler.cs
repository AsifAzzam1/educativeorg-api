using educativeorg_data.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using static educativeorg_models.ViewModels.ExceptionResposne;

namespace educativeorg_api.Helper
{
    public class PermissionAuthHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly EducativeOrgDbContext _context;

        public PermissionAuthHandler(EducativeOrgDbContext context)
        {
            _context = context;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            var userid = context.User.Claims.FirstOrDefault(_ => _.Type == ClaimTypes.NameIdentifier)?.Value;
            
            if (string.IsNullOrWhiteSpace(userid))
                throw new HttpStatusException(System.Net.HttpStatusCode.Unauthorized, "Invalid Session");
           
            var IsUserIdParsed = Guid.TryParse(userid, out Guid UserId);
            if (IsUserIdParsed) 
            {
                var userPermissions = _context.Users.Where(_ => _.Id == UserId)
                                                    .Include(_ => _.Roles).ThenInclude(_ => _.Permissions)
                                                    .SelectMany(x=>x.Roles).SelectMany(_=>_.Permissions)
                                                    .Select(_=>_.Name).ToHashSet<string>();
                return;
            }

        }
    }
}
