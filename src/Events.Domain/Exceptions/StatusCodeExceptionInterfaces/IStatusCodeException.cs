namespace Events.Domain.Exceptions.StatusCodeExceptionInterfaces;

public interface IStatusCodeException
{
    public int StatusCode { get; }
}
