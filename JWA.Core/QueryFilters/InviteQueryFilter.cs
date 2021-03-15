namespace JWA.Core.QueryFilters
{
    public class InviteQueryFilter : BaseQueryFilter
    {
        public string Email { get; set; }
        public string Role { get; set; }
        public string Organization { get; set; }
        public string Facility { get; set; }

    }
}
