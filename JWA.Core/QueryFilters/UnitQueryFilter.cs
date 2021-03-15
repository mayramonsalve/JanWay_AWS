namespace JWA.Core.QueryFilters
{
    public class UnitQueryFilter : BaseQueryFilter
    {
        public string Name { get; set; }
        public int? OrganizationId { get; set; }
        public int? FacilityId { get; set; }
        public int? SuinNumber { get; set; }
        public int? Health { get; set; }

    }
}
