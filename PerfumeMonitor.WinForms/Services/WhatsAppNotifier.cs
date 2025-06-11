using Twilio;
using Twilio.Rest.Api.V2010.Account;
using PerfumeMonitor.Shared.Interfaces;
using PerfumeMonitor.Shared.Models;

namespace PerfumeMonitor.Services
{
    public class WhatsAppNotifier : IWhatsAppNotifier
    {
        private readonly string _accountSid;
        private readonly string _authToken;
        private readonly string _fromNumber;
        private readonly string _toNumber;
        private readonly bool _isEnabled;
        private readonly int _maxDailyNotifications;
        private readonly int _cooldownMinutes;
        private readonly bool _reduceNightTimeChecks;
        private static int _todayNotificationCount = 0;
        private static DateTime _lastCountReset = DateTime.Today;

        public WhatsAppNotifier(WhatsAppConfig config, TwilioCredentials credentials)
        {
            _accountSid = credentials.AccountSid;
            _authToken = credentials.AuthToken;
            _fromNumber = credentials.FromPhoneNumber;
            _toNumber = credentials.ToPhoneNumber;
            _isEnabled = config.IsEnabled;
            _maxDailyNotifications = config.MaxDailyNotifications;
            _cooldownMinutes = config.CooldownMinutes;
            _reduceNightTimeChecks = config.ReduceNightTimeChecks;

            if (_isEnabled && !string.IsNullOrEmpty(_accountSid) && !string.IsNullOrEmpty(_authToken))
            {
                TwilioClient.Init(_accountSid, _authToken);
            }

            ResetCounterIfNewDay();
        }

        public async Task<bool> SendNotificationAsync(PerfumeUrl perfume, WhatsAppConfig config)
        {
            if (!_isEnabled || string.IsNullOrEmpty(_toNumber) || string.IsNullOrEmpty(_fromNumber))
            {
                return false;
            }

            ResetCounterIfNewDay();

            if (!CanSendNotification(perfume, config))
            {
                System.Diagnostics.Debug.WriteLine($"Notificação WhatsApp bloqueada para {perfume.Name} - limite/cooldown");
                return false;
            }

            try
            {
                string message = $"🎉 *PERFUME DISPONÍVEL!* 🎉\n\n" +
                               $"*{perfume.Name}* está disponível para compra!\n\n" +
                               $"🔗 Link: {perfume.Url}\n\n" +
                               $"⏰ Verificado em: {perfume.LastChecked:dd/MM/yyyy HH:mm}\n\n" +
                               $"Aproveite antes que esgote! 🏃‍♂️💨";

                var messageResource = await MessageResource.CreateAsync(
                    body: message,
                    from: new Twilio.Types.PhoneNumber($"whatsapp:{_fromNumber}"),
                    to: new Twilio.Types.PhoneNumber($"whatsapp:{_toNumber}")
                );

                if (messageResource.Status != MessageResource.StatusEnum.Failed)
                {
                    perfume.LastWhatsAppNotification = DateTime.Now;
                    _todayNotificationCount++;
                    System.Diagnostics.Debug.WriteLine($"WhatsApp enviado para {perfume.Name}. Contador diário: {_todayNotificationCount}/{_maxDailyNotifications}");
                }

                return messageResource.Status != MessageResource.StatusEnum.Failed;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro ao enviar WhatsApp: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> SendNotificationAsync(PerfumeUrl perfume)
        {
            var config = new WhatsAppConfig
            {
                IsEnabled = _isEnabled,
                MaxDailyNotifications = _maxDailyNotifications,
                CooldownMinutes = _cooldownMinutes,
                ReduceNightTimeChecks = _reduceNightTimeChecks
            };
            return await SendNotificationAsync(perfume, config);
        }

        public bool CanSendNotification(PerfumeUrl perfume, WhatsAppConfig config)
        {
            if (_todayNotificationCount >= config.MaxDailyNotifications)
            {
                return false;
            }

            var timeSinceLastNotification = DateTime.Now - perfume.LastWhatsAppNotification;
            if (timeSinceLastNotification.TotalMinutes < config.CooldownMinutes)
            {
                return false;
            }

            if (config.ReduceNightTimeChecks)
            {
                var currentHour = DateTime.Now.Hour;
                if (currentHour >= 1 && currentHour <= 6)
                {
                    return false;
                }
            }

            return true;
        }

        private bool ShouldSendNotification(PerfumeUrl perfume)
        {
            var config = new WhatsAppConfig
            {
                IsEnabled = _isEnabled,
                MaxDailyNotifications = _maxDailyNotifications,
                CooldownMinutes = _cooldownMinutes,
                ReduceNightTimeChecks = _reduceNightTimeChecks
            };
            return CanSendNotification(perfume, config);
        }

        private void ResetCounterIfNewDay()
        {
            if (DateTime.Today > _lastCountReset)
            {
                _todayNotificationCount = 0;
                _lastCountReset = DateTime.Today;
            }
        }

        public int GetRemainingNotifications()
        {
            ResetCounterIfNewDay();
            return Math.Max(0, _maxDailyNotifications - _todayNotificationCount);
        }
    }
} 