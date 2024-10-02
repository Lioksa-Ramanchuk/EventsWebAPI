using AutoMapper;
using Events.Application.Models.Role;
using Events.Domain.Entities;

namespace Events.Application.Mappings;

public class RoleProfile : Profile
{
    public RoleProfile()
    {
        CreateMap<Role, RoleResponseModel>();
    }
}
