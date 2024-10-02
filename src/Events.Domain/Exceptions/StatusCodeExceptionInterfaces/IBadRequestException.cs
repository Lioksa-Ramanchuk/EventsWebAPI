using Microsoft.AspNetCore.Http;

namespace Events.Domain.Exceptions.StatusCodeExceptionInterfaces;

public interface IBadRequestException : IStatusCodeException
{
    int IStatusCodeException.StatusCode => StatusCodes.Status400BadRequest;
}
