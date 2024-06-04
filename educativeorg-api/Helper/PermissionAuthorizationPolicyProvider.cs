using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace educativeorg_api.Helper
{
    public class PermissionAuthorizationPolicyProvider:DefaultAuthorizationPolicyProvider
    {
        public PermissionAuthorizationPolicyProvider(IOptions<AuthorizationOptions> opts)
            :base(opts)
        {
            
        }

        public override async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
        {
            AuthorizationPolicy? policy = await base.GetPolicyAsync(policyName);
            if (policy != null)
                return policy;

            return new AuthorizationPolicyBuilder().AddRequirements(new PermissionRequirement(policyName)).Build();


        }
    }
}
