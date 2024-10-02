using Events.Domain.Exceptions.StatusCodeExceptionInterfaces;

namespace Events.Domain.Exceptions.AuthExceptions
{
    public class ForbiddenException : AuthException, IForbiddenException
    {
        public ForbiddenException() { }
    }
}
