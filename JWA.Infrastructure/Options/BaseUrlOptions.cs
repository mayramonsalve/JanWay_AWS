namespace JWA.Infrastructure.Options
{
    public class BaseUrlOptions
    {
        public string Scheme { get; set; }
        public string Host { get; set; }
        public string Port { get; set; }
        public string GetBaseUrl()
        { 
            return $"{this.Scheme}://{this.Host}:{this.Port}";
        }
        private string baseUrl;
        public string BaseUrl
        {
            get
            {
                return baseUrl;
            }
            set
            {
                baseUrl = Scheme + "://" + Host + ":" + Port;
            }
        }
    }
}
