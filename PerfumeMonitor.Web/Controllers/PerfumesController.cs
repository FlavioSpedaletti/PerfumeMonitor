using Microsoft.AspNetCore.Mvc;
using PerfumeMonitor.Shared.Interfaces;
using PerfumeMonitor.Shared.Models;
using PerfumeMonitor.Web.Services;

namespace PerfumeMonitor.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PerfumesController : ControllerBase
    {
        private readonly IConfigManager _configManager;
        private readonly IPerfumeChecker _perfumeChecker;
        private readonly ILogger<PerfumesController> _logger;
        private readonly IMonitoringStatusService _statusService;

        public PerfumesController(IConfigManager configManager, IPerfumeChecker perfumeChecker, ILogger<PerfumesController> logger, IMonitoringStatusService statusService)
        {
            _configManager = configManager;
            _perfumeChecker = perfumeChecker;
            _logger = logger;
            _statusService = statusService;
        }

        [HttpGet]
        public async Task<ActionResult<List<PerfumeUrl>>> GetPerfumes()
        {
            var config = await _configManager.LoadConfigAsync();
            return Ok(config.PerfumeUrls);
        }

        [HttpPost]
        public async Task<ActionResult<PerfumeUrl>> AddPerfume([FromBody] PerfumeUrl perfume)
        {
            if (string.IsNullOrWhiteSpace(perfume.Name) || string.IsNullOrWhiteSpace(perfume.Url))
            {
                return BadRequest("Nome e URL são obrigatórios");
            }

            var config = await _configManager.LoadConfigAsync();
            
            if (config.PerfumeUrls.Any(p => p.Url.Equals(perfume.Url, StringComparison.OrdinalIgnoreCase)))
            {
                return BadRequest("Perfume com esta URL já existe");
            }

            config.PerfumeUrls.Add(perfume);
            await _configManager.SaveConfigAsync(config);

            _logger.LogInformation($"Perfume adicionado: {perfume.Name}");
            return CreatedAtAction(nameof(GetPerfumes), perfume);
        }

        [HttpDelete("{index}")]
        public async Task<ActionResult> RemovePerfume(int index)
        {
            var config = await _configManager.LoadConfigAsync();
            
            if (index < 0 || index >= config.PerfumeUrls.Count)
            {
                return NotFound("Perfume não encontrado");
            }

            var perfume = config.PerfumeUrls[index];
            config.PerfumeUrls.RemoveAt(index);
            await _configManager.SaveConfigAsync(config);

            _logger.LogInformation($"Perfume removido: {perfume.Name}");
            return NoContent();
        }

        [HttpPost("{index}/check")]
        public async Task<ActionResult<PerfumeUrl>> CheckPerfume(int index)
        {
            var config = await _configManager.LoadConfigAsync();
            
            if (index < 0 || index >= config.PerfumeUrls.Count)
            {
                return NotFound("Perfume não encontrado");
            }

            var perfume = config.PerfumeUrls[index];
            
            try
            {
                var isAvailable = await _perfumeChecker.CheckAvailabilityAsync(perfume);
                perfume.IsAvailable = isAvailable;
                
                await _configManager.SaveConfigAsync(config);
                
                _logger.LogInformation($"Verificação manual: {perfume.Name} - {perfume.LastStatus}");
                return Ok(perfume);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao verificar perfume: {perfume.Name}");
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        [HttpPost("check-all")]
        public async Task<ActionResult> CheckAllPerfumes()
        {
            var config = await _configManager.LoadConfigAsync();
            
            if (!config.PerfumeUrls.Any())
            {
                return BadRequest("Nenhum perfume configurado para verificação");
            }

            _logger.LogInformation($"Verificação manual de todos os perfumes iniciada - {config.PerfumeUrls.Count} perfumes");

            // Executar verificação em background para não bloquear a resposta
            _ = Task.Run(async () =>
            {
                _statusService.StartCheck(config.PerfumeUrls.Count);
                _logger.LogInformation("🚀 Verificação manual iniciada via interface web");

                try
                {
                    for (int i = 0; i < config.PerfumeUrls.Count; i++)
                    {
                        var perfume = config.PerfumeUrls[i];
                        _statusService.UpdateCurrentPerfume(i + 1, perfume.Name);

                        try
                        {
                            _logger.LogInformation($"🔍 Verificação manual {i + 1}/{config.PerfumeUrls.Count}: {perfume.Name}");
                            var isAvailable = await _perfumeChecker.CheckAvailabilityAsync(perfume);
                            perfume.IsAvailable = isAvailable;
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, $"Erro na verificação manual de {perfume.Name}");
                        }
                    }
                    
                    await _configManager.SaveConfigAsync(config);
                    _logger.LogInformation("✅ Verificação manual de todos os perfumes concluída");
                }
                finally
                {
                    _statusService.CompleteCheck();
                }
            });

            return Ok(new { message = "Verificação iniciada", perfumeCount = config.PerfumeUrls.Count });
        }
    }
} 