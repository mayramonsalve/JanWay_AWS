using System;
using System.Collections.Generic;

namespace JWA.Core.DTOs
{
    /// <summary>
    /// Organization data sent over the network.
    /// </summary>
    public class SetupOrganizationDto
    {
        public OrganizationInfoDto Organization { get; set; }
        public AddressInfoDto Address { get; set; }
        public List<FacilityInfoDto> Facilities { get; set; }
        public List<InviteDto> Users { get; set; }
        public List<AssignUnitDto> Units { get; set; }
    }

    public class OrganizationInfoDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }

    }

    public class OrganizationsListDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int UnitsNumber { get; set; }
        public int UsersNumber { get; set; }
        public string Status { get; set; }
    }

    public class OrganizationProfileDto
    {
        public OrganizationInfoDto Organization { get; set; }
        public AddressInfoDto Address { get; set; }
        public int FacilitiesNumber { get; set; }
        public int UsersNumber { get; set; }
        //public List<UnitsHubDto> UnitsHub { get; set; }
        //public List<UsersListDto> UsersList { get; set; }
    }

    public class EditOrganizationDto
    {
        public OrganizationInfoDto Organization { get; set; }
        public AddressInfoDto Address { get; set; }
    }
    public class DeleteOrganizationDto
    {
        public int Id { get; set; }
    }
    public class ActivateOrganizationDto
    {
        public int Id { get; set; }
    }
    public class DeactivateOrganizationDto
    {
        public int Id { get; set; }
    }

}
