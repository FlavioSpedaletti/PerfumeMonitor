using PerfumeMonitor.Shared.Models;

namespace PerfumeMonitor.Shared.Interfaces
{
    public interface IConfigManager
    {
        Task<AppConfig> LoadConfigAsync();
        Task SaveConfigAsync(AppConfig config);
        Task<TwilioCredentials?> LoadTwilioCredentialsAsync();
    }
} 