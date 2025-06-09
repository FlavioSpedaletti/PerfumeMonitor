namespace PerfumeMonitor.Models
{
    public class AppConfig
    {
        public List<PerfumeUrl> PerfumeUrls { get; set; } = new List<PerfumeUrl>();
        public int CheckIntervalMinutes { get; set; } = 1;

        public AppConfig()
        {
            PerfumeUrls = new List<PerfumeUrl>();
            CheckIntervalMinutes = 1;
        }
    }
} 