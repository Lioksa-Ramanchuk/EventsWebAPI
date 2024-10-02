using Microsoft.AspNetCore.Authorization;

namespace Events.Application.Authorization.Requirements;

public class NotificationOwnerOrAdministratorRequirement : IAuthorizationRequirement { }
