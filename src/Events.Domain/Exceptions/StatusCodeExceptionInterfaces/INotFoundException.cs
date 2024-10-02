using Microsoft.AspNetCore.Http;

namespace Events.Domain.Exceptions.StatusCodeExceptionInterfaces;

public interface INotFoundException : IStatusCodeException
{
    int IStatusCodeException.StatusCode => StatusCodes.Status404NotFound;
}
