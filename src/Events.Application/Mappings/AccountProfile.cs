using AutoMapper;
using Events.Application.Models.Account;
using Events.Domain.Entities;

namespace Events.Application.Mappings;

public class AccountProfile : Profile
{
    public AccountProfile()
    {
        CreateMap<AccountUpdateRequestModel, Account>()
            .ForMember(
                dest => dest.Username,
                opt =>
                {
                    opt.PreCondition(src => !string.IsNullOrWhiteSpace(src.Username));
                    opt.MapFrom(src => src.Username!.Trim());
                }
            )
            .ForMember(dest => dest.Password, opt => opt.Ignore());

        CreateMap<Account, AccountResponseModel>();
    }
}
