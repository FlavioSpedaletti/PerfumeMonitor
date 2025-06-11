namespace PerfumeMonitor.Models
{
    public class PerfumeUrl
    {
        public string Name { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public bool IsAvailable { get; set; } = false;
        public DateTime LastChecked { get; set; } = DateTime.Now;
        public string LastStatus { get; set; } = "Não verificado";
        public DateTime LastWhatsAppNotification { get; set; } = DateTime.MinValue;

        public PerfumeUrl() { }

        public PerfumeUrl(string name, string url)
        {
            Name = name;
            Url = url;
            LastChecked = DateTime.Now;
            LastStatus = "Não verificado";
            LastWhatsAppNotification = DateTime.MinValue;
        }
    }
} 