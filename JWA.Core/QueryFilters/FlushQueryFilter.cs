using System;

namespace JWA.Core.QueryFilters
{
    public class FlushQueryFilter : BaseQueryFilter
    {
        public int UnitId { get; set; }
        public DateTime Date { get; set; }
        public int? SelenoidTemperature { get; set; }
        public string PressureOnFilters { get; set; }
        public int? Health { get; set; }

    }
}
