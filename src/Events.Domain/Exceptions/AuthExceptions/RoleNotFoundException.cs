using Events.Domain.Exceptions.StatusCodeExceptionInterfaces;
using Events.Domain.Resources;
using SmartFormat;

namespace Events.Domain.Exceptions.AuthExceptions;

public class RoleNotFoundException : AuthException, INotFoundException
{
    public RoleNotFoundException(string message)
        : base(message) { }

    public RoleNotFoundException(Guid roleId)
        : base(Smart.Format(ExceptionMessages.RoleWithIdNotFound, new { roleId })) { }
}
