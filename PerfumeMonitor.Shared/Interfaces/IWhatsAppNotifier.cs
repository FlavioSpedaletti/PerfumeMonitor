using PerfumeMonitor.Shared.Models;

namespace PerfumeMonitor.Shared.Interfaces
{
    public interface IWhatsAppNotifier
    {
        Task<bool> SendNotificationAsync(PerfumeUrl perfume, WhatsAppConfig config);
        bool CanSendNotification(PerfumeUrl perfume, WhatsAppConfig config);
    }
} 