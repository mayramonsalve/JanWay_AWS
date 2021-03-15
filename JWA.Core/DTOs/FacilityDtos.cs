using System;
using System.Collections.Generic;

namespace JWA.Core.DTOs
{
    /// <summary>
    /// Facility data sent over the network.
    /// </summary>
    public class AddFacilityDto
    {
        public FacilityInfoDto Facility { get; set; }
        public AddressInfoDto Address { get; set; }
        public List<InviteDto> Users { get; set; }
        public List<AssignUnitDto> Units { get; set; }
    }

    public class FacilityInfoDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public int OrganizationId { get; set; }

    }

    public class FacilitiesListDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int UnitsNumber { get; set; }
        public int UsersNumber { get; set; }
        public string Status { get; set; }
    }

    public class FacilityProfileDto
    {
        public FacilityInfoDto Facility { get; set; }
        public AddressInfoDto Address { get; set; }
        public int UnitsNumber { get; set; }
        public int UsersNumber { get; set; }
    }

    public class EditFacilityDto
    {
        public FacilityInfoDto Facility { get; set; }
        public AddressInfoDto Address { get; set; }
    }
    public class DeleteFacilityDto
    {
        public int Id { get; set; }
    }
    public class ActivateFacilityDto
    {
        public int Id { get; set; }
    }
    public class DeactivateFacilityDto
    {
        public int Id { get; set; }
    }

}
