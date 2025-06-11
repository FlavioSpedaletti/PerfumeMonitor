using PerfumeMonitor.Shared.Interfaces;
using PerfumeMonitor.Shared.Models;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace PerfumeMonitor.Web.Services
{
    public class WhatsAppNotifier : IWhatsAppNotifier
    {
        private readonly IConfigManager _configManager;
        private readonly ILogger<WhatsAppNotifier> _logger;
        private TwilioCredentials? _credentials;

        public WhatsAppNotifier(IConfigManager configManager, ILogger<WhatsAppNotifier> logger)
        {
            _configManager = configManager;
            _logger = logger;
        }

        public async Task<bool> SendNotificationAsync(PerfumeUrl perfume, WhatsAppConfig config)
        {
            try
            {
                if (!config.IsEnabled)
                {
                    _logger.LogInformation("Notificações WhatsApp desabilitadas");
                    return false;
                }

                _credentials ??= await _configManager.LoadTwilioCredentialsAsync();
                
                if (_credentials == null)
                {
                    _logger.LogWarning("Credenciais do Twilio não configuradas");
                    return false;
                }

                TwilioClient.Init(_credentials.AccountSid, _credentials.AuthToken);

                var message = $"🌟 Perfume Disponível!\n\n" +
                            $"Nome: {perfume.Name}\n" +
                            $"Status: {perfume.LastStatus}\n" +
                            $"Link: {perfume.Url}\n\n" +
                            $"Verificado em: {perfume.LastChecked:dd/MM/yyyy HH:mm}";

                var twilioMessage = await MessageResource.CreateAsync(
                    body: message,
                    from: new Twilio.Types.PhoneNumber($"whatsapp:{_credentials.FromPhoneNumber}"),
                    to: new Twilio.Types.PhoneNumber($"whatsapp:{_credentials.ToPhoneNumber}")
                );

                perfume.LastWhatsAppNotification = DateTime.Now;
                _logger.LogInformation($"Notificação WhatsApp enviada para {perfume.Name}. SID: {twilioMessage.Sid}");
                
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao enviar notificação WhatsApp para {perfume.Name}");
                return false;
            }
        }

        public bool CanSendNotification(PerfumeUrl perfume, WhatsAppConfig config)
        {
            if (!config.IsEnabled)
                return false;

            var timeSinceLastNotification = DateTime.Now - perfume.LastWhatsAppNotification;
            var cooldownExpired = timeSinceLastNotification.TotalMinutes >= config.CooldownMinutes;

            if (config.ReduceNightTimeChecks)
            {
                var currentHour = DateTime.Now.Hour;
                if (currentHour >= 1 && currentHour <= 6)
                {
                    return false; // Bloqueia completamente no horário noturno (igual Windows Forms)
                }
            }

            return cooldownExpired;
        }
    }
} 