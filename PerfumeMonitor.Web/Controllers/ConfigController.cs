using Microsoft.AspNetCore.Mvc;
using PerfumeMonitor.Shared.Interfaces;
using PerfumeMonitor.Shared.Models;
using PerfumeMonitor.Web.Services;

namespace PerfumeMonitor.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConfigController : ControllerBase
    {
        private readonly IConfigManager _configManager;
        private readonly ILogger<ConfigController> _logger;
        private readonly IMonitoringStatusService _statusService;

        public ConfigController(IConfigManager configManager, ILogger<ConfigController> logger, IMonitoringStatusService statusService)
        {
            _configManager = configManager;
            _logger = logger;
            _statusService = statusService;
        }

        [HttpGet]
        public async Task<ActionResult<AppConfig>> GetConfig()
        {
            var config = await _configManager.LoadConfigAsync();
            return Ok(config);
        }

        [HttpPut("interval")]
        public async Task<ActionResult> UpdateCheckInterval([FromBody] int intervalMinutes)
        {
            if (intervalMinutes < 1)
            {
                return BadRequest("Intervalo deve ser maior que 0");
            }

            var config = await _configManager.LoadConfigAsync();
            config.CheckIntervalMinutes = intervalMinutes;
            await _configManager.SaveConfigAsync(config);

            _logger.LogInformation($"Intervalo de verificação atualizado para {intervalMinutes} minutos");
            return Ok();
        }

        [HttpPut("whatsapp")]
        public async Task<ActionResult> UpdateWhatsAppConfig([FromBody] WhatsAppConfig whatsAppConfig)
        {
            var config = await _configManager.LoadConfigAsync();
            config.WhatsApp = whatsAppConfig;
            await _configManager.SaveConfigAsync(config);

            _logger.LogInformation("Configurações WhatsApp atualizadas");
            return Ok();
        }

        [HttpGet("status")]
        public async Task<ActionResult> GetStatus()
        {
            var config = await _configManager.LoadConfigAsync();
            var credentials = await _configManager.LoadTwilioCredentialsAsync();
            
            var status = new
            {
                PerfumeCount = config.PerfumeUrls.Count,
                CheckIntervalMinutes = config.CheckIntervalMinutes,
                WhatsAppEnabled = config.WhatsApp.IsEnabled,
                TwilioConfigured = credentials != null,
                LastUpdate = DateTime.Now,
                IsCheckingNow = _statusService.IsCheckingNow,
                LastCheckStarted = _statusService.LastCheckStarted,
                LastCheckCompleted = _statusService.LastCheckCompleted,
                CurrentPerfumeIndex = _statusService.CurrentPerfumeIndex,
                TotalPerfumes = _statusService.TotalPerfumes,
                CurrentPerfumeName = _statusService.CurrentPerfumeName
            };

            return Ok(status);
        }
    }
} 