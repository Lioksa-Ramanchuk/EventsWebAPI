using System.Security.Claims;
using Events.Application.Authorization.Requirements;
using Events.Application.Configuration.Settings;
using Events.Domain.Constants;
using Events.Domain.Interfaces.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Events.Application.Authorization.Handlers;

public class NotificationOwnerOrAdministratorAuthorizationHandler(
    IHttpContextAccessor httpContextAccessor,
    IServiceProvider serviceProvider,
    IOptions<AppSettings> appSettings
) : AuthorizationHandler<NotificationOwnerOrAdministratorRequirement>
{
    private readonly AuthorizationSettings _authorizationSettings = appSettings
        .Value
        .AuthorizationSettings;

    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        NotificationOwnerOrAdministratorRequirement requirement
    )
    {
        if (context.User.IsInRole(_authorizationSettings.RoleTitles.Administrator))
        {
            context.Succeed(requirement);
            return;
        }

        if (httpContextAccessor.HttpContext is DefaultHttpContext httpContext)
        {
            var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (
                httpContext.Request.Query.TryGetValue(
                    RouteKeys.AccountId,
                    out var queryAccountIdValue
                )
                && Guid.TryParse(queryAccountIdValue, out var queryAccountId)
                && queryAccountId.ToString() == userId
            )
            {
                context.Succeed(requirement);
                return;
            }

            if (
                httpContext
                    .GetRouteData()
                    .Values.TryGetValue(RouteKeys.NotificationId, out var routeNotificationIdValue)
                && Guid.TryParse(routeNotificationIdValue?.ToString(), out var routeNotificationId)
            )
            {
                using var scope = serviceProvider.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<IDbUnitOfWork>();

                var notification = await db.Notifications.GetByIdAsync(routeNotificationId);
                var notificationAccountId = notification?.AccountId;

                if (notificationAccountId is not null && notificationAccountId.ToString() == userId)
                {
                    context.Succeed(requirement);
                }
            }
        }
    }
}
