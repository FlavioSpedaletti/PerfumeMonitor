using PerfumeMonitor.Shared.Models;

namespace PerfumeMonitor.Shared.Interfaces
{
    public interface IPerfumeChecker
    {
        Task<bool> CheckAvailabilityAsync(PerfumeUrl perfume);
    }
} 