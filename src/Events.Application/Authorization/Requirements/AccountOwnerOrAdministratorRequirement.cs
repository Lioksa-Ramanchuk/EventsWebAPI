using Microsoft.AspNetCore.Authorization;

namespace Events.Application.Authorization.Requirements;

public class AccountOwnerOrAdministratorRequirement : IAuthorizationRequirement { }
