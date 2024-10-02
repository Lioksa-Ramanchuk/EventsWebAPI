namespace Events.Application.Models.Common;

public class PagedResponseModel<T>
{
    public int TotalCount { get; set; }
    public int Offset { get; set; }
    public int Limit { get; set; }
    public bool HasMore => Offset + Data.Count < TotalCount;
    public ICollection<T> Data { get; set; } = [];
}
