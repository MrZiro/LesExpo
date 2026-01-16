namespace LesExpo.web.Models.Configuration
{
    public class ExternalApiConfig
    {
        public const string SectionName = "ExternalApi";
        
        public string BaseUrl { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public int Timeout { get; set; } = 30;
        public int RetryCount { get; set; } = 3;
        public int RetryDelaySeconds { get; set; } = 2;
    }
} 