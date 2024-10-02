using AutoMapper;
using Events.Application.Models.Account;
using Events.Application.Models.Participant;
using Events.Domain.Entities;

namespace Events.Application.Mappings;

public class ParticipantProfile : Profile
{
    public ParticipantProfile()
    {
        CreateMap<ParticipantSignUpRequestModel, Participant>()
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username.Trim()))
            .ForMember(dest => dest.Password, opt => opt.Ignore())
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName.Trim()))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName.Trim()))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.Trim()));

        CreateMap<ParticipantUpdateRequestModel, Participant>()
            .IncludeBase<AccountUpdateRequestModel, Account>()
            .ForMember(
                dest => dest.FirstName,
                opt =>
                {
                    opt.PreCondition(src => !string.IsNullOrWhiteSpace(src.FirstName));
                    opt.MapFrom(src => src.FirstName!.Trim());
                }
            )
            .ForMember(
                dest => dest.LastName,
                opt =>
                {
                    opt.PreCondition(src => !string.IsNullOrWhiteSpace(src.LastName));
                    opt.MapFrom(src => src.LastName!.Trim());
                }
            )
            .ForMember(
                dest => dest.BirthDate,
                opt =>
                {
                    opt.PreCondition(src => src.BirthDate is not null);
                    opt.MapFrom(src => src.BirthDate);
                }
            )
            .ForMember(
                dest => dest.Email,
                opt =>
                {
                    opt.PreCondition(src => !string.IsNullOrWhiteSpace(src.Email));
                    opt.MapFrom(src => src.Email!.Trim());
                }
            );

        CreateMap<Participant, ParticipantResponseModel>();
    }
}
