using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Api.Attributes;

public class ClaimsAuthorizeAttribute : TypeFilterAttribute
{
    public ClaimsAuthorizeAttribute(string claimType, string claimValue) : base(typeof(ClaimRequirementFilter)) =>
        Arguments = [new Claim(claimType, claimValue)];

    public class ClaimRequirementFilter(Claim claim) : IAuthorizationFilter
    {
        readonly Claim _claim = claim;

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // var isAdmin = user.IsInRole("Admin");

            if (context.HttpContext.User is not ClaimsPrincipal user
                || user.Identity!.IsAuthenticated)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            if (!user.HasClaim(_claim.Type, _claim.Value))
                context.Result = new ForbidResult();
        }
    }
}