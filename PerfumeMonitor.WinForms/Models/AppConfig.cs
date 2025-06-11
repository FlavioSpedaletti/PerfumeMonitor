namespace PerfumeMonitor.Models
{
    public class AppConfig
    {
        public List<PerfumeUrl> PerfumeUrls { get; set; } = new List<PerfumeUrl>();
        public int CheckIntervalMinutes { get; set; } = 1;
        public WhatsAppConfig WhatsApp { get; set; } = new WhatsAppConfig();

        public AppConfig()
        {
            PerfumeUrls = new List<PerfumeUrl>();
            CheckIntervalMinutes = 1;
            WhatsApp = new WhatsAppConfig();
        }
    }
} 