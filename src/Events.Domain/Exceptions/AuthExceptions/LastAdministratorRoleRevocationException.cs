using Events.Domain.Exceptions.StatusCodeExceptionInterfaces;
using Events.Domain.Resources;
using SmartFormat;

namespace Events.Domain.Exceptions.AuthExceptions;

public class LastAdministratorRoleRevocationException(Guid accountId)
    : AuthException(
        Smart.Format(ExceptionMessages.LastAdministratorRoleWithKeyRevocation, new { accountId })
    ),
        IBadRequestException { }
