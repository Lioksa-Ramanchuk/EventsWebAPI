using Microsoft.AspNetCore.Http;

namespace Events.Domain.Exceptions.StatusCodeExceptionInterfaces;

public interface IUnauthorizedException : IStatusCodeException
{
    int IStatusCodeException.StatusCode => StatusCodes.Status401Unauthorized;
}
