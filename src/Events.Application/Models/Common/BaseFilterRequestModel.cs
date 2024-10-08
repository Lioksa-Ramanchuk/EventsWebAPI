namespace Events.Application.Models.Common;

public abstract class BaseFilterRequestModel
{
    public int? Offset { get; set; }
    public int? Limit { get; set; }
}
