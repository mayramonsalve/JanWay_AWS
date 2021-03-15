namespace JWA.Core.QueryFilters
{
    public class FacilityQueryFilter : BaseQueryFilter
    {
        public string Name { get; set; }
        public int? OrganizationId { get; set; }
        public string City { get; set; }
        public int? StateId { get; set; }
        public bool Status { get; set; }

    }
}
