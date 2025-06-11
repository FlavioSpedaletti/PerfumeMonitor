namespace PerfumeMonitor.Web.Services
{
    public interface IMonitoringStatusService
    {
        bool IsCheckingNow { get; }
        DateTime? LastCheckStarted { get; }
        DateTime? LastCheckCompleted { get; }
        int CurrentPerfumeIndex { get; }
        int TotalPerfumes { get; }
        string CurrentPerfumeName { get; }
        void StartCheck(int totalPerfumes);
        void UpdateCurrentPerfume(int index, string name);
        void CompleteCheck();
    }

    public class MonitoringStatusService : IMonitoringStatusService
    {
        private readonly object _lock = new object();
        
        public bool IsCheckingNow { get; private set; }
        public DateTime? LastCheckStarted { get; private set; }
        public DateTime? LastCheckCompleted { get; private set; }
        public int CurrentPerfumeIndex { get; private set; }
        public int TotalPerfumes { get; private set; }
        public string CurrentPerfumeName { get; private set; } = string.Empty;

        public void StartCheck(int totalPerfumes)
        {
            lock (_lock)
            {
                IsCheckingNow = true;
                LastCheckStarted = DateTime.Now;
                TotalPerfumes = totalPerfumes;
                CurrentPerfumeIndex = 0;
                CurrentPerfumeName = string.Empty;
            }
        }

        public void UpdateCurrentPerfume(int index, string name)
        {
            lock (_lock)
            {
                CurrentPerfumeIndex = index;
                CurrentPerfumeName = name;
            }
        }

        public void CompleteCheck()
        {
            lock (_lock)
            {
                IsCheckingNow = false;
                LastCheckCompleted = DateTime.Now;
                CurrentPerfumeIndex = 0;
                TotalPerfumes = 0;
                CurrentPerfumeName = string.Empty;
            }
        }
    }
} 