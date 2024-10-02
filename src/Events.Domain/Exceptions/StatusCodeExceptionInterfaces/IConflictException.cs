using Microsoft.AspNetCore.Http;

namespace Events.Domain.Exceptions.StatusCodeExceptionInterfaces;

public interface IConflictException : IStatusCodeException
{
    int IStatusCodeException.StatusCode => StatusCodes.Status409Conflict;
}
