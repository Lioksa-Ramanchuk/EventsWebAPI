using Events.Domain.Exceptions.StatusCodeExceptionInterfaces;
using Events.Domain.Resources;
using SmartFormat;

namespace Events.Domain.Exceptions.AuthExceptions;

public class AccountAlreadyHasRoleException(Guid accountId, Guid roleId)
    : AuthException(
        Smart.Format(ExceptionMessages.AccountRoleWithKeyAlreadyAssigned, new { accountId, roleId })
    ),
        IConflictException { }
