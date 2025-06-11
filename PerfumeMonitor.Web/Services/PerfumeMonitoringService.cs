using PerfumeMonitor.Shared.Interfaces;
using PerfumeMonitor.Shared.Models;

namespace PerfumeMonitor.Web.Services
{
    public class PerfumeMonitoringService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<PerfumeMonitoringService> _logger;
        private readonly IMonitoringStatusService _statusService;

        public PerfumeMonitoringService(IServiceProvider serviceProvider, ILogger<PerfumeMonitoringService> logger, IMonitoringStatusService statusService)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _statusService = statusService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Serviço de monitoramento de perfumes iniciado");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await CheckAllPerfumesAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro durante verificação de perfumes");
                }

                using var scope = _serviceProvider.CreateScope();
                var configManager = scope.ServiceProvider.GetRequiredService<IConfigManager>();
                var config = await configManager.LoadConfigAsync();
                
                var delayMinutes = config.CheckIntervalMinutes;
                if (ShouldReduceNightTimeChecks(config))
                {
                    delayMinutes = Math.Max(delayMinutes, 30);
                }

                _logger.LogInformation($"Próxima verificação em {delayMinutes} minutos");
                await Task.Delay(TimeSpan.FromMinutes(delayMinutes), stoppingToken);
            }

            _logger.LogInformation("Serviço de monitoramento de perfumes parado");
        }

        private async Task CheckAllPerfumesAsync()
        {
            using var scope = _serviceProvider.CreateScope();
            var configManager = scope.ServiceProvider.GetRequiredService<IConfigManager>();
            var perfumeChecker = scope.ServiceProvider.GetRequiredService<IPerfumeChecker>();
            var whatsAppNotifier = scope.ServiceProvider.GetRequiredService<IWhatsAppNotifier>();

            var config = await configManager.LoadConfigAsync();

            if (!config.PerfumeUrls.Any())
            {
                _logger.LogInformation("Nenhum perfume configurado para monitoramento");
                return;
            }

            _logger.LogInformation($"Verificando {config.PerfumeUrls.Count} perfumes...");
            _statusService.StartCheck(config.PerfumeUrls.Count);

            try
            {
                for (int i = 0; i < config.PerfumeUrls.Count; i++)
                {
                    var perfume = config.PerfumeUrls[i];
                    _statusService.UpdateCurrentPerfume(i + 1, perfume.Name);
                    _logger.LogInformation($"🔍 Verificando perfume {i + 1}/{config.PerfumeUrls.Count}: {perfume.Name}");

                    try
                    {
                        var wasAvailable = perfume.IsAvailable;
                        var isAvailable = await perfumeChecker.CheckAvailabilityAsync(perfume);
                        perfume.IsAvailable = isAvailable;

                        if (isAvailable && !wasAvailable)
                        {
                            _logger.LogInformation($"🌟 {perfume.Name} ficou disponível!");
                            
                            if (whatsAppNotifier.CanSendNotification(perfume, config.WhatsApp))
                            {
                                await whatsAppNotifier.SendNotificationAsync(perfume, config.WhatsApp);
                            }
                        }
                        else if (isAvailable)
                        {
                            _logger.LogInformation($"✅ {perfume.Name} continua disponível");
                        }
                        else
                        {
                            _logger.LogInformation($"❌ {perfume.Name} está esgotado");
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"Erro ao verificar {perfume.Name}");
                    }
                }

                await configManager.SaveConfigAsync(config);
            }
            finally
            {
                _statusService.CompleteCheck();
            }
        }

        private bool ShouldReduceNightTimeChecks(AppConfig config)
        {
            if (!config.WhatsApp.ReduceNightTimeChecks)
                return false;

            var currentHour = DateTime.Now.Hour;
            return currentHour >= 22 || currentHour <= 6;
        }
    }
} 