using AutoMapper;
using Events.Application.Models.EventParticipant.EventParticipant;
using Events.Application.Models.EventParticipant.ParticipantEvent;
using Events.Domain.Entities;

namespace Events.Application.Mappings;

public class EventParticipantProfile : Profile
{
    public EventParticipantProfile()
    {
        CreateMap<EventParticipant, EventParticipantResponseModel>()
            .ForMember(dest => dest.Participant, opt => opt.MapFrom(src => src.Participant));

        CreateMap<EventParticipant, ParticipantEventResponseModel>()
            .ForMember(dest => dest.Event, opt => opt.MapFrom(src => src.Event));
    }
}
