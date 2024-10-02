using Events.Domain.Exceptions.StatusCodeExceptionInterfaces;
using Events.Domain.Resources;
using SmartFormat;

namespace Events.Domain.Exceptions.AuthExceptions;

public class LastAdministratorRemovalException(Guid accountId)
    : AuthException(
        Smart.Format(ExceptionMessages.LastAdministratorWithIdRemoval, new { accountId })
    ),
        IBadRequestException { }
