using Microsoft.AspNetCore.Mvc;
using PerfumeMonitor.Shared.Interfaces;
using PerfumeMonitor.Shared.Models;

namespace PerfumeMonitor.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        private readonly IConfigManager _configManager;
        private readonly IWhatsAppNotifier _whatsAppNotifier;
        private readonly ILogger<TestController> _logger;

        public TestController(IConfigManager configManager, IWhatsAppNotifier whatsAppNotifier, ILogger<TestController> logger)
        {
            _configManager = configManager;
            _whatsAppNotifier = whatsAppNotifier;
            _logger = logger;
        }

        [HttpPost("whatsapp")]
        public async Task<ActionResult> TestWhatsApp()
        {
            try
            {
                var config = await _configManager.LoadConfigAsync();
                var credentials = await _configManager.LoadTwilioCredentialsAsync();

                if (config?.WhatsApp == null)
                {
                    return StatusCode(500, new { Error = "Configuração WhatsApp não encontrada" });
                }

                var testPerfume = new PerfumeUrl
                {
                    Name = "Teste de Notificação",
                    Url = "https://exemplo.com/teste",
                    IsAvailable = true,
                    LastChecked = DateTime.Now,
                    LastStatus = "Teste - Disponível",
                    LastWhatsAppNotification = DateTime.MinValue // Força sem cooldown
                };

                var canSend = _whatsAppNotifier.CanSendNotification(testPerfume, config.WhatsApp);
                var credentialsValid = !string.IsNullOrEmpty(credentials?.AccountSid) && credentials.AccountSid.StartsWith("AC");

                var result = new
                {
                    WhatsAppEnabled = config.WhatsApp.IsEnabled,
                    TwilioConfigured = credentials != null,
                    CredentialsValid = credentialsValid,
                    CanSend = canSend,
                    CurrentTime = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"),
                    CooldownMinutes = config.WhatsApp.CooldownMinutes,
                    ReduceNightTime = config.WhatsApp.ReduceNightTimeChecks,
                    CurrentHour = DateTime.Now.Hour,
                    IsNightTime = DateTime.Now.Hour >= 1 && DateTime.Now.Hour <= 6,
                    AccountSid = credentials?.AccountSid?.Substring(0, Math.Min(8, credentials.AccountSid.Length)) + "...",
                    FromNumber = credentials?.FromPhoneNumber ?? "Não configurado",
                    ToNumber = credentials?.ToPhoneNumber ?? "Não configurado"
                };

                if (canSend && config.WhatsApp.IsEnabled && credentials != null && credentialsValid)
                {
                    _logger.LogInformation("🧪 Tentando enviar notificação de teste...");
                    var sent = await _whatsAppNotifier.SendNotificationAsync(testPerfume, config.WhatsApp);
                    
                    _logger.LogInformation($"✅ Resultado do teste: {(sent ? "Sucesso" : "Falha")}");
                    
                    return Ok(new { 
                        result, 
                        TestSent = sent,
                        Message = sent ? "✅ Notificação de teste enviada com sucesso!" : "❌ Falha ao enviar notificação de teste"
                    });
                }

                var reason = "Motivos: ";
                if (!config.WhatsApp.IsEnabled) reason += "WhatsApp desabilitado; ";
                if (credentials == null) reason += "Credenciais não encontradas; ";
                if (!credentialsValid) reason += "Credenciais inválidas; ";
                if (!canSend) reason += "Bloqueado por horário noturno ou cooldown; ";

                return Ok(new { 
                    result, 
                    TestSent = false,
                    Message = $"❌ Condições não atendidas para envio. {reason.TrimEnd(';', ' ')}"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro no teste de WhatsApp: {Message}", ex.Message);
                return StatusCode(500, new { 
                    Error = ex.Message,
                    Details = "Verifique os logs da aplicação para mais detalhes"
                });
            }
        }

        [HttpGet("whatsapp-status")]
        public async Task<ActionResult> GetWhatsAppStatus()
        {
            try
            {
                var config = await _configManager.LoadConfigAsync();
                var credentials = await _configManager.LoadTwilioCredentialsAsync();

                // Verificar se config é válido
                if (config?.WhatsApp == null)
                {
                    return StatusCode(500, new { Error = "Configuração WhatsApp não encontrada" });
                }

                // Tratar AccountSid com segurança
                string accountSidDisplay = "Não configurado";
                if (!string.IsNullOrEmpty(credentials?.AccountSid))
                {
                    accountSidDisplay = credentials.AccountSid.Length > 8 ? 
                        credentials.AccountSid.Substring(0, 8) + "..." : 
                        credentials.AccountSid;
                }

                // Calcular notificações com segurança
                var lastNotifications = new List<object>();
                if (config.PerfumeUrls?.Any() == true)
                {
                    lastNotifications = config.PerfumeUrls
                        .Where(p => p?.LastWhatsAppNotification > DateTime.MinValue)
                        .Select(p => new { 
                            Name = p.Name ?? "Sem nome", 
                            LastNotification = p.LastWhatsAppNotification.ToString("dd/MM/yyyy HH:mm"),
                            MinutesSince = Math.Round((DateTime.Now - p.LastWhatsAppNotification).TotalMinutes, 1)
                        })
                        .ToList<object>();
                }

                var status = new
                {
                    WhatsAppEnabled = config.WhatsApp.IsEnabled,
                    TwilioConfigured = credentials != null,
                    AccountSid = accountSidDisplay,
                    FromNumber = credentials?.FromPhoneNumber ?? "Não configurado",
                    ToNumber = credentials?.ToPhoneNumber ?? "Não configurado",
                    MaxDailyNotifications = config.WhatsApp.MaxDailyNotifications,
                    CooldownMinutes = config.WhatsApp.CooldownMinutes,
                    ReduceNightTimeChecks = config.WhatsApp.ReduceNightTimeChecks,
                    CurrentTime = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"),
                    CurrentHour = DateTime.Now.Hour,
                    IsNightTime = DateTime.Now.Hour >= 1 && DateTime.Now.Hour <= 6,
                    LastNotifications = lastNotifications,
                    TotalPerfumes = config.PerfumeUrls?.Count ?? 0
                };

                return Ok(status);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter status WhatsApp: {Message}", ex.Message);
                return StatusCode(500, new { 
                    Error = ex.Message,
                    StackTrace = ex.StackTrace?.Split('\n').Take(5).ToArray() // Primeiras 5 linhas apenas
                });
            }
        }
    }
} 