using System.Security.Claims;
using Events.Application.Authorization.Requirements;
using Events.Application.Configuration.Settings;
using Events.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;

namespace Events.Application.Authorization.Handlers;

public class ParticipantAccountOwnerOrAdministratorAuthorizationHandler(
    IHttpContextAccessor httpContextAccessor,
    IOptions<AppSettings> appSettings
) : AuthorizationHandler<ParticipantAccountOwnerOrAdministratorRequirement>
{
    private readonly AuthorizationSettings _authorizationSettings = appSettings
        .Value
        .AuthorizationSettings;

    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        ParticipantAccountOwnerOrAdministratorRequirement requirement
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
                    .Values.TryGetValue(RouteKeys.ParticipantId, out var routeParticipantIdValue)
                && Guid.TryParse(routeParticipantIdValue?.ToString(), out var routeParticipantId)
                && routeParticipantId.ToString() == userId
            )
            {
                context.Succeed(requirement);
            }
        }

        return Task.CompletedTask;
    }
}
