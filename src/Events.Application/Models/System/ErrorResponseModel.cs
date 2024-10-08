namespace Events.Application.Models.System;

public class ErrorResponseModel
{
    public string ErrorMessage { get; set; } = null!;
    public DateTime Timestamp { get; set; }
}
