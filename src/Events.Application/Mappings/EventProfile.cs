using AutoMapper;
using Events.Application.Models.Event;
using Events.Domain.Entities;

namespace Events.Application.Mappings;

public partial class EventProfile : Profile
{
    public EventProfile()
    {
        CreateMap<EventAddRequestModel, Event>()
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title.Trim()))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description.Trim()))
            .ForMember(dest => dest.EventDate, opt => opt.MapFrom(src => src.EventDate))
            .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.Location.Trim()))
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.Trim()))
            .ForMember(
                dest => dest.MaxParticipantsCount,
                opt => opt.MapFrom(src => src.MaxParticipantsCount)
            );

        CreateMap<EventUpdateRequestModel, Event>()
            .ForMember(
                dest => dest.Title,
                opt =>
                {
                    opt.PreCondition(src => !string.IsNullOrWhiteSpace(src.Title));
                    opt.MapFrom(src => src.Title!.Trim());
                }
            )
            .ForMember(
                dest => dest.Description,
                opt =>
                {
                    opt.PreCondition(src => !string.IsNullOrWhiteSpace(src.Description));
                    opt.MapFrom(src => src.Description!.Trim());
                }
            )
            .ForMember(
                dest => dest.EventDate,
                opt =>
                {
                    opt.PreCondition(src => src.EventDate is not null);
                    opt.MapFrom(src => src.EventDate);
                }
            )
            .ForMember(
                dest => dest.Location,
                opt =>
                {
                    opt.PreCondition(src => !string.IsNullOrWhiteSpace(src.Location));
                    opt.MapFrom(src => src.Location!.Trim());
                }
            )
            .ForMember(
                dest => dest.Category,
                opt =>
                {
                    opt.PreCondition(src => !string.IsNullOrWhiteSpace(src.Category));
                    opt.MapFrom(src => src.Category!.Trim());
                }
            )
            .ForMember(
                dest => dest.MaxParticipantsCount,
                opt =>
                {
                    opt.PreCondition(src => src.MaxParticipantsCount is not null);
                    opt.MapFrom(src => src.MaxParticipantsCount);
                }
            );

        CreateMap<Event, EventResponseModel>();
    }
}
