using AutoMapper;
using Events.Application.Models.AccountRole.AccountRole;
using Events.Domain.Entities;

namespace Events.Application.Mappings;

public class AccountRoleProfile : Profile
{
    public AccountRoleProfile()
    {
        CreateMap<Role, AccountRoleResponseModel>()
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src));

        CreateMap<AccountRole, AccountRoleResponseModel>()
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role));
    }
}
