using System.Security.Claims;
using Events.Application.Authorization.Requirements;
using Events.Application.Configuration.Settings;
using Events.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;

namespace Events.Application.Authorization.Handlers;

public class AccountOwnerOrAdministratorAuthorizationHandler(
    IHttpContextAccessor httpContextAccessor,
    IOptions<AppSettings> appSettings
) : AuthorizationHandler<AccountOwnerOrAdministratorRequirement>
{
    private readonly AuthorizationSettings _authorizationSettings = appSettings
        .Value
        .AuthorizationSettings;

    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        AccountOwnerOrAdministratorRequirement requirement
    )
    {
        if (context.User.IsInRole(_authorizationSettings.RoleTitles.Administrator))
        {
            context.Succeed(requirement);
            return Task.CompletedTask;
        }

        if (httpContextAccessor.HttpContext is DefaultHttpContext httpContext)
        {
            var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (
                httpContext
                    .GetRouteData()
                    .Values.TryGetValue(RouteKeys.AccountId, out var routeAccountIdValue)
                && Guid.TryParse(routeAccountIdValue?.ToString(), out var routeAccountId)
                && routeAccountId.ToString() == userId
            )
            {
                context.Succeed(requirement);
            }
        }

        return Task.CompletedTask;
    }
}
