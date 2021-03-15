using AutoMapper;
using JWA.Core.DTOs;
using JWA.Core.Entities;
using System.Collections.Generic;
using System.Linq;

namespace JWA.Infrastructure.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            //Facilities
            CreateMap<AddFacilityDto, Address>()
                .ForMember(destination => destination.Description, options => options.MapFrom(source => source.Address));
            CreateMap<AddFacilityDto, Facility>();
            CreateMap<AddFacilityDto, List<Invite>>();
            CreateMap<AddFacilityDto, List<Unit>>();
            CreateMap<AddFacilityDto, Address>()
                .ForMember(destination => destination.Description, options => options.MapFrom(source => source.Address));
            CreateMap<FacilityInfoDto, Facility>().ReverseMap();
            CreateMap<Facility, FacilityProfileDto>()
                .ForMember(destination => destination.UnitsNumber, options => options.MapFrom(source => source.Units.Count))
                .ForMember(destination => destination.UsersNumber, options => options.MapFrom(source => source.Supervisors.Count));
            CreateMap<Facility, FacilitiesListDto>();

            //Invites
            CreateMap<InviteDto, Invite>()
                .ForMember(source => source.Facility, options => options.Ignore())
                .ForMember(destination => destination.FacilityId, options => options.Condition(source => (source.FacilityId > 0)));//.ReverseMap();
            CreateMap<Invite, UserInfoDto>().ReverseMap();

            //Organizations
            CreateMap<SetupOrganizationDto, Address>()
                .ForMember(destination => destination.Description, options => options.MapFrom(source => source.Address));
            CreateMap<SetupOrganizationDto, Organization>();
            CreateMap<SetupOrganizationDto, List<Facility>>();
            CreateMap<SetupOrganizationDto, List<Invite>>();
            CreateMap<SetupOrganizationDto, List<Unit>>();
            CreateMap<SetupOrganizationDto, Address>()
                .ForMember(destination => destination.Description, options => options.MapFrom(source => source.Address));
            CreateMap<OrganizationInfoDto, Organization>().ReverseMap();
            CreateMap<AddressInfoDto, Address>()
                .ForMember(destination => destination.Description, options => options.MapFrom(source => source.Address)).ReverseMap();
            CreateMap<Organization, OrganizationProfileDto>()
                .ForMember(destination => destination.FacilitiesNumber, options => options.MapFrom(source => source.Facilities.Count))
                .ForMember(destination => destination.UsersNumber, options => options.MapFrom(source => (source.Supervisors.Count + source.Facilities.SelectMany(sourcefac => sourcefac.Supervisors).Count())));
            CreateMap<Organization, OrganizationsListDto>();

            //Supervisors
            CreateMap<Supervisor, RelocateDto>().ReverseMap();

            //Units
            CreateMap<AssignUnitDto, Unit>()
                .ForMember(x => x.Facility, opt => opt.Ignore())
                .ForMember(destination => destination.Suin, options => options.MapFrom(source => source.SuinNumber));
            CreateMap<EditUnitDto, Unit>();
            CreateMap<RegisterUnitDto, Unit>();
            CreateMap<Unit, UnassignedUnitsDto>();
            CreateMap<Unit, UnitsHubDto>();

            //Users
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<User, ProfileDto>().ReverseMap();
            CreateMap<User, UpdateRoleDto>();
            CreateMap<User, UpdatePasswordDto>();
            CreateMap<ConfirmAccountRequest, User>();
            CreateMap<ConfirmAccountRequest, ConfirmAccountResponse>();
        }
    }
}
