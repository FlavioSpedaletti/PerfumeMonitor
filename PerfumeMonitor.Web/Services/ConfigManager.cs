using System.Text.Json;
using PerfumeMonitor.Shared.Interfaces;
using PerfumeMonitor.Shared.Models;

namespace PerfumeMonitor.Web.Services
{
    public class ConfigManager : IConfigManager
    {
        private readonly string _configPath;
        private readonly string _credentialsPath;
        private readonly ILogger<ConfigManager> _logger;

        public ConfigManager(ILogger<ConfigManager> logger)
        {
            _logger = logger;
            
            // Tentar múltiplos caminhos para encontrar os arquivos de configuração
            _configPath = GetConfigFilePath("config.json");
            _credentialsPath = GetConfigFilePath("twilio-credentials.json");
            
            _logger.LogInformation("ConfigManager usando config.json em: {ConfigPath}", _configPath);
            _logger.LogInformation("ConfigManager usando twilio-credentials.json em: {CredentialsPath}", _credentialsPath);
        }

        private string GetConfigFilePath(string fileName)
        {
            // Lista de caminhos para tentar (em ordem de prioridade)
            var paths = new[]
            {
                // 1. Caminho compartilhado (desenvolvimento local - diretório pai)
                Path.Combine(Path.GetDirectoryName(Directory.GetCurrentDirectory()) ?? "", fileName),
                
                // 2. Diretório da aplicação (publicação/Azure)
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName),
                
                // 3. Diretório de trabalho atual
                Path.Combine(Directory.GetCurrentDirectory(), fileName)
            };

            foreach (var path in paths)
            {
                if (!string.IsNullOrEmpty(path) && File.Exists(path))
                {
                    var fullPath = Path.GetFullPath(path);
                    _logger.LogInformation("Arquivo {FileName} encontrado em: {Path}", fileName, fullPath);
                    return fullPath;
                }
            }

            // Se não encontrou em nenhum lugar, usar o caminho local como fallback
            var fallbackPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
            _logger.LogWarning("Arquivo {FileName} não encontrado nos caminhos conhecidos. Usando fallback: {Path}", fileName, fallbackPath);
            return fallbackPath;
        }

        public async Task<AppConfig> LoadConfigAsync()
        {
            try
            {
                if (!File.Exists(_configPath))
                {
                    var defaultConfig = new AppConfig();
                    await SaveConfigAsync(defaultConfig);
                    return defaultConfig;
                }

                var json = await File.ReadAllTextAsync(_configPath);
                return JsonSerializer.Deserialize<AppConfig>(json) ?? new AppConfig();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao carregar configurações");
                return new AppConfig();
            }
        }

        public async Task SaveConfigAsync(AppConfig config)
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true
                };
                var json = JsonSerializer.Serialize(config, options);
                await File.WriteAllTextAsync(_configPath, json);
                _logger.LogInformation("Configurações salvas com sucesso");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao salvar configurações");
            }
        }

        public async Task<TwilioCredentials?> LoadTwilioCredentialsAsync()
        {
            try
            {
                // Primeiro tenta carregar do arquivo
                if (File.Exists(_credentialsPath))
                {
                    var json = await File.ReadAllTextAsync(_credentialsPath);
                    var credentials = JsonSerializer.Deserialize<TwilioCredentials>(json);
                    if (credentials != null)
                    {
                        _logger.LogInformation("Credenciais do Twilio carregadas do arquivo");
                        return credentials;
                    }
                }

                // Se não conseguiu carregar do arquivo, tenta das variáveis de ambiente
                var envCredentials = LoadTwilioCredentialsFromEnvironment();
                if (envCredentials != null)
                {
                    _logger.LogInformation("Credenciais do Twilio carregadas das variáveis de ambiente");
                    return envCredentials;
                }

                _logger.LogWarning("Credenciais do Twilio não encontradas nem em arquivo nem em variáveis de ambiente");
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao carregar credenciais do Twilio");
                return null;
            }
        }

        private TwilioCredentials? LoadTwilioCredentialsFromEnvironment()
        {
            var accountSid = Environment.GetEnvironmentVariable("TWILIO_ACCOUNT_SID");
            var authToken = Environment.GetEnvironmentVariable("TWILIO_AUTH_TOKEN");
            var fromNumber = Environment.GetEnvironmentVariable("TWILIO_FROM_NUMBER");
            var toNumber = Environment.GetEnvironmentVariable("TWILIO_TO_NUMBER");

            if (string.IsNullOrEmpty(accountSid) || string.IsNullOrEmpty(authToken) || 
                string.IsNullOrEmpty(fromNumber) || string.IsNullOrEmpty(toNumber))
            {
                return null;
            }

            return new TwilioCredentials
            {
                AccountSid = accountSid,
                AuthToken = authToken,
                FromPhoneNumber = fromNumber,
                ToPhoneNumber = toNumber
            };
        }
    }
}