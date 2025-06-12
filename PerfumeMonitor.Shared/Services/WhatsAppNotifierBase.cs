using PerfumeMonitor.Shared.Interfaces;
using PerfumeMonitor.Shared.Models;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace PerfumeMonitor.Shared.Services
{
    public abstract class WhatsAppNotifierBase : IWhatsAppNotifier
    {
        private static int _todayNotificationCount = 0;
        private static DateTime _lastCountReset = DateTime.Today;

        protected WhatsAppNotifierBase()
        {
            ResetCounterIfNewDay();
        }

        public async Task<bool> SendNotificationAsync(PerfumeUrl perfume, WhatsAppConfig config)
        {
            try
            {
                if (!config.IsEnabled)
                {
                    LogInformation("Notificações WhatsApp desabilitadas");
                    return false;
                }

                ResetCounterIfNewDay();

                if (!CanSendNotification(perfume, config))
                {
                    LogInformation($"Notificação WhatsApp bloqueada para {perfume.Name} - limite/cooldown");
                    return false;
                }

                var credentials = await GetCredentialsAsync();
                
                if (credentials == null)
                {
                    LogWarning("Credenciais do Twilio não configuradas");
                    return false;
                }

                TwilioClient.Init(credentials.AccountSid, credentials.AuthToken);

                var message = $"🌟 Perfume Disponível!\n\n" +
                            $"Nome: {perfume.Name}\n" +
                            $"Status: {perfume.LastStatus}\n" +
                            $"Link: {perfume.Url}\n\n" +
                            $"Verificado em: {perfume.LastChecked:dd/MM/yyyy HH:mm}";

                var twilioMessage = await MessageResource.CreateAsync(
                    body: message,
                    from: new Twilio.Types.PhoneNumber($"whatsapp:{credentials.FromPhoneNumber}"),
                    to: new Twilio.Types.PhoneNumber($"whatsapp:{credentials.ToPhoneNumber}")
                );

                if (twilioMessage.Status != MessageResource.StatusEnum.Failed)
                {
                    perfume.LastWhatsAppNotification = DateTime.Now;
                    _todayNotificationCount++;
                    LogInformation($"Notificação WhatsApp enviada com sucesso para {perfume.Name}. SID: {twilioMessage.Sid}, Status: {twilioMessage.Status}. Contador diário: {_todayNotificationCount}/{config.MaxDailyNotifications}");
                    return true;
                }
                else
                {
                    LogWarning($"Falha ao enviar notificação WhatsApp para {perfume.Name}. SID: {twilioMessage.Sid}, Status: {twilioMessage.Status}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                LogError(ex, $"Erro ao enviar notificação WhatsApp para {perfume.Name}");
                return false;
            }
        }

        public bool CanSendNotification(PerfumeUrl perfume, WhatsAppConfig config)
        {
            if (!config.IsEnabled)
                return false;

            ResetCounterIfNewDay();

            if (_todayNotificationCount >= config.MaxDailyNotifications)
            {
                return false;
            }

            var timeSinceLastNotification = DateTime.Now - perfume.LastWhatsAppNotification;
            var cooldownExpired = timeSinceLastNotification.TotalMinutes >= config.CooldownMinutes;

            if (config.ReduceNightTimeChecks)
            {
                var currentHour = DateTime.Now.Hour;
                if (currentHour >= 1 && currentHour <= 6)
                {
                    return false;
                }
            }

            return cooldownExpired;
        }

        public int GetRemainingNotifications(int maxDailyNotifications)
        {
            ResetCounterIfNewDay();
            return Math.Max(0, maxDailyNotifications - _todayNotificationCount);
        }

        private void ResetCounterIfNewDay()
        {
            if (DateTime.Today > _lastCountReset)
            {
                _todayNotificationCount = 0;
                _lastCountReset = DateTime.Today;
            }
        }

        // Métodos abstratos que cada implementação deve fornecer
        protected abstract Task<TwilioCredentials?> GetCredentialsAsync();
        protected abstract void LogInformation(string message);
        protected abstract void LogWarning(string message);
        protected abstract void LogError(Exception ex, string message);
    }
} 