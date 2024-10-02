using Microsoft.AspNetCore.Http;

namespace Events.Domain.Exceptions.StatusCodeExceptionInterfaces;

public interface IInternalServerErrorException : IStatusCodeException
{
    int IStatusCodeException.StatusCode => StatusCodes.Status500InternalServerError;
}
