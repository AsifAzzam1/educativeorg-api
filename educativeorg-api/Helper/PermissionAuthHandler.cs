using educativeorg_data.Data;
using educativeorg_models.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using static educativeorg_models.ViewModels.ExceptionResposne;

namespace educativeorg_api.Helper
{
    public class PermissionAuthHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public PermissionAuthHandler(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }
        
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            string? userid = context.User.Claims.FirstOrDefault(_ => _.Type == ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(userid))
            {
                context.Fail(new AuthorizationFailureReason(this, "Invalid Session"));
                return;
            }
                //throw new HttpStatusException(System.Net.HttpStatusCode.Unauthorized, "Invalid Session");
           
            using IServiceScope _scope = _scopeFactory.CreateScope();

            EducativeOrgDbContext _context = _scope.ServiceProvider.GetRequiredService<EducativeOrgDbContext>();

            bool IsUserIdParsed = Guid.TryParse(userid, out Guid UserId);
            if (IsUserIdParsed) 
            {
                List<ApplicationRole> userRoles = await _context.Users.Where(_ => _.Id == UserId)
                                                    .Include(_ => _.Roles).ThenInclude(_ => _.Permissions)
                                                    .SelectMany(x=>x.Roles).ToListAsync();

                HashSet<string> userPermissions = userRoles.SelectMany(_ => _.Permissions)
                                                    .Select(_ => _.Name).ToHashSet<string>();

                if (userPermissions.Contains(requirement.Permission))
                {
                    context.Succeed(requirement);
                }
                else 
                {
                    context.Fail(new AuthorizationFailureReason(this, "Invalid Session"));
                }

                
                return;

            }



        }
    }
}
