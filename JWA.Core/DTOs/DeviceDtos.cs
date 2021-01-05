namespace JWA.Core.DTOs
{
    /// <summary>
    /// User data sent over the network.
    /// </summary>
    public class RegisterDto
    {
        public string device_id { get; set; }
    }

    public class SystemStatusDto
    {
        public int unix_timestamp { get; set; }
        public string battery { get; set; }
        public string case_temperature { get; set; }
    }

    public class FlushDto
    {
        public int unix_timestamp { get; set; }
        public filters_pressure filters_pressure { get; set; }
    }

    public class filters_pressure
    {
        public string f1 { get; set; }
        public string f2 { get; set; }
        public string f3 { get; set; }
        public string f4 { get; set; }
    }

}
