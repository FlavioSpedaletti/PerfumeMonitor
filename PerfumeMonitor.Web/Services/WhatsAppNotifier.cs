using PerfumeMonitor.Shared.Interfaces;
using PerfumeMonitor.Shared.Models;
using PerfumeMonitor.Shared.Services;

namespace PerfumeMonitor.Web.Services
{
    public class WhatsAppNotifier : WhatsAppNotifierBase
    {
        private readonly IConfigManager _configManager;
        private readonly ILogger<WhatsAppNotifier> _logger;
        private TwilioCredentials? _credentials;

        public WhatsAppNotifier(IConfigManager configManager, ILogger<WhatsAppNotifier> logger)
        {
            _configManager = configManager;
            _logger = logger;
        }

        protected override async Task<TwilioCredentials?> GetCredentialsAsync()
        {
            _credentials ??= await _configManager.LoadTwilioCredentialsAsync();
            return _credentials;
        }

        protected override void LogInformation(string message)
        {
            _logger.LogInformation(message);
        }

        protected override void LogWarning(string message)
        {
            _logger.LogWarning(message);
        }

        protected override void LogError(Exception ex, string message)
        {
            _logger.LogError(ex, message);
        }
    }
} 