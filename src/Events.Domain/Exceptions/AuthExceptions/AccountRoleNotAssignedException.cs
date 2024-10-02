using Events.Domain.Exceptions.StatusCodeExceptionInterfaces;
using Events.Domain.Resources;
using SmartFormat;

namespace Events.Domain.Exceptions.AuthExceptions;

public class AccountRoleNotAssignedException(Guid roleId, Guid accountId)
    : AuthException(
        Smart.Format(ExceptionMessages.AccountRoleWithKeyNotAssigned, new { roleId, accountId })
    ),
        IBadRequestException { }
