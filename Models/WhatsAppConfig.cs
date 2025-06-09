namespace PerfumeMonitor.Models
{
    public class WhatsAppConfig
    {
        public bool IsEnabled { get; set; } = false;
        public int MaxDailyNotifications { get; set; } = 10;
        public int CooldownMinutes { get; set; } = 30;
        public bool ReduceNightTimeChecks { get; set; } = true;
    }
} 