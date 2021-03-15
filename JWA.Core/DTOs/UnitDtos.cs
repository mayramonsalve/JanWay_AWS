using System;
using System.Collections.Generic;

namespace JWA.Core.DTOs
{
    /// <summary>
    /// Unit data sent over the network.
    /// </summary>
    public class AssignUnitDto
    {
        public string Name { get; set; }
        public int SuinNumber { get; set; }
        public string Facility { get; set; }
        public int? FacilityId { get; set; }
    }

    public class UnitsHubDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string IpAddress { get; set; }
        public string Status { get; set; }
    }

    public class UnitsProfileDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string IpAddress { get; set; }
        public int FlushesNumber { get; set; }
        public int BatteryLevel { get; set; }
        public string PressureOnFilters { get; set; }
        public FlushesHistoryDto LastFlush { get; set; }
        //public FiltersPSIDto Filters { get; set; }
        public List<FlushesHistoryDto> FlushesHistory { get; set; }
    }

    public class FiltersPSIDto
    {
        public int Filter1 { get; set; }
        public int Filter2 { get; set; }
        public int Filter3 { get; set; }
        public int Filter4 { get; set; }
    }

    public class FlushesHistoryDto
    {
        public DateTime Date { get; set; }
        public double SelenoidTemperature { get; set; }
        public int Health { get; set; }
        public string Status { get; set; }
        public FiltersPSIDto Filters { get; set; }
    }
    public class EditUnitDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int FacilityId { get; set; }
    }
    public class DetachUnitDto
    {
        public int Id { get; set; }
    }
    public class RebootUnitDto
    {
        public int Id { get; set; }
    }
    public class RegisterUnitDto
    {
        public string MacAddress { get; set; }
    }
    public class UnassignedUnitsDto
    {
        public int Id { get; set; }
        public string MacAddress { get; set; }
    }
}
