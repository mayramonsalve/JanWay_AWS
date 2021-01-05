namespace JWA.Core.QueryFilters
{
    public class InviteQueryFilter
    {
        public string Email { get; set; }
        public string Role { get; set; }
        public string Organization { get; set; }
        public string Facility { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }

    }
}
