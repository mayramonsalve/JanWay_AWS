using AutoMapper;
using JWA.Core.DTOs;
using JWA.Core.Entities;

namespace JWA.Infrastructure.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Invite, InviteDtos>().ReverseMap();
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<User, ProfileDto>().ReverseMap();
            CreateMap<User, UpdateRoleDto>();
            CreateMap<User, UpdatePasswordDto>();
            CreateMap<ConfirmAccountRequest, User>();
            CreateMap<ConfirmAccountRequest, ConfirmAccountResponse>();
            CreateMap<Supervisor, RelocateDto>().ReverseMap();
        }
    }
}
