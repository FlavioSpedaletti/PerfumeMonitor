using PerfumeMonitor.Shared.Interfaces;
using PerfumeMonitor.Shared.Models;
using PerfumeMonitor.Shared.Services;

namespace PerfumeMonitor.Services
{
    public class WhatsAppNotifier : WhatsAppNotifierBase
    {
        private readonly TwilioCredentials _credentials;
        private readonly WhatsAppConfig _config;

        public WhatsAppNotifier(WhatsAppConfig config, TwilioCredentials credentials)
        {
            _config = config;
            _credentials = credentials;
        }

        protected override Task<TwilioCredentials?> GetCredentialsAsync()
        {
            return Task.FromResult<TwilioCredentials?>(_credentials);
        }

        protected override void LogInformation(string message)
        {
            System.Diagnostics.Debug.WriteLine(message);
        }

        protected override void LogWarning(string message)
        {
            System.Diagnostics.Debug.WriteLine(message);
        }

        protected override void LogError(Exception ex, string message)
        {
            System.Diagnostics.Debug.WriteLine($"{message}: {ex.Message}");
        }

        // Método adicional para compatibilidade com código existente
        public async Task<bool> SendNotificationAsync(PerfumeUrl perfume)
        {
            return await SendNotificationAsync(perfume, _config);
        }
    }
} 