using Microsoft.AspNetCore.Http;

namespace Events.Domain.Exceptions.StatusCodeExceptionInterfaces;

public interface INoContentException : IStatusCodeException
{
    int IStatusCodeException.StatusCode => StatusCodes.Status204NoContent;
}
