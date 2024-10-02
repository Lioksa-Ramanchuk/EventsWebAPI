namespace Events.Application.Models.Common;

public class BaseResponseModel
{
    public BaseResponseModel() { }

    public BaseResponseModel(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; set; }
}
