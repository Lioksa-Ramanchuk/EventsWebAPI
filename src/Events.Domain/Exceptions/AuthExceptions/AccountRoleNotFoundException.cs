using Events.Domain.Exceptions.StatusCodeExceptionInterfaces;
using Events.Domain.Resources;
using SmartFormat;

namespace Events.Domain.Exceptions.AuthExceptions;

public class AccountRoleNotFoundException(Guid accountId, Guid roleId)
    : AuthException(
        Smart.Format(ExceptionMessages.AccountRoleWithKeyNotFound, new { accountId, roleId })
    ),
        INotFoundException { }
