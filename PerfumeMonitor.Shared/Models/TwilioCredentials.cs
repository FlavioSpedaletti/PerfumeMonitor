namespace PerfumeMonitor.Shared.Models
{
    public class TwilioCredentials
    {
        public string AccountSid { get; set; } = string.Empty;
        public string AuthToken { get; set; } = string.Empty;
        public string FromPhoneNumber { get; set; } = string.Empty;
        public string ToPhoneNumber { get; set; } = string.Empty;
    }
} 