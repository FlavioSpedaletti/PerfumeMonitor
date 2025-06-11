namespace PerfumeMonitor.Models
{
    public class TwilioCredentials
    {
        public string AccountSid { get; set; } = string.Empty;
        public string AuthToken { get; set; } = string.Empty;
        public string FromNumber { get; set; } = string.Empty;
        public string ToNumber { get; set; } = string.Empty;
    }
} 