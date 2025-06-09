namespace PerfumeMonitor.Models
{
    public class PerfumeUrl
    {
        public string Name { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public bool IsAvailable { get; set; }
        public DateTime LastChecked { get; set; }
        public string LastStatus { get; set; } = "Nunca verificado";

        public PerfumeUrl() { }

        public PerfumeUrl(string name, string url)
        {
            Name = name;
            Url = url;
            IsAvailable = false;
            LastChecked = DateTime.MinValue;
        }
    }
} 