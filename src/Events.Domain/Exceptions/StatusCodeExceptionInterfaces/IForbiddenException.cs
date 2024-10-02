using Microsoft.AspNetCore.Http;

namespace Events.Domain.Exceptions.StatusCodeExceptionInterfaces;

public interface IForbiddenException : IStatusCodeException
{
    int IStatusCodeException.StatusCode => StatusCodes.Status403Forbidden;
}
