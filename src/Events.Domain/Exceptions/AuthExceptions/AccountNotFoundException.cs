using Events.Domain.Exceptions.StatusCodeExceptionInterfaces;
using Events.Domain.Resources;
using SmartFormat;

namespace Events.Domain.Exceptions.AuthExceptions;

public class AccountNotFoundException(Guid accountId)
    : AuthException(Smart.Format(ExceptionMessages.AccountWithIdNotFound, new { accountId })),
        INotFoundException { }
