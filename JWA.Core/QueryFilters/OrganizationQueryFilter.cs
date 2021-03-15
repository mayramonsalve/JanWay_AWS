namespace JWA.Core.QueryFilters
{
    public class OrganizationQueryFilter : BaseQueryFilter
    {
        public string Name { get; set; }
        public string City { get; set; }
        public int? StateId { get; set; }
        public bool Status { get; set; }

    }
}
