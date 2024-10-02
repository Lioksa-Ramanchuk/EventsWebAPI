using AutoMapper;
using Events.Application.Models.Notification;
using Events.Domain.Entities;

namespace Events.Application.Mappings;

public class NotificationProfile : Profile
{
    public NotificationProfile()
    {
        CreateMap<NotificationSendRequestModel, Notification>()
            .ForMember(dest => dest.IsRead, opt => opt.MapFrom(src => false))
            .ForMember(dest => dest.SentAt, opt => opt.MapFrom(src => DateTime.UtcNow));

        CreateMap<Notification, NotificationResponseModel>();
    }
}
