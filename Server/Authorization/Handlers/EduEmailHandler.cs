using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Server.Authorization.Policies;
 
namespace Server.Authorization.Handlers
{
    public class EduEmailHandler : AuthorizationHandler<EduEmailRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, EduEmailRequirement requirement)
        {
            var email = context.User.FindFirst(ClaimTypes.Email)?.Value;
 
            if (!string.IsNullOrEmpty(email) && email.EndsWith(".edu", StringComparison.OrdinalIgnoreCase))
            {
                context.Succeed(requirement);
            }
 
            return Task.CompletedTask;
        }
    }
}