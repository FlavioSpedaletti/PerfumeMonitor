using Newtonsoft.Json;
using PerfumeMonitor.Shared.Interfaces;
using PerfumeMonitor.Shared.Models;

namespace PerfumeMonitor.Services
{
    public class ConfigManager : IConfigManager
    {
        private readonly string _configPath;
        private readonly string _credentialsPath;

        public ConfigManager()
        {
            // Buscar no diretório raiz do projeto
            var currentDir = Directory.GetCurrentDirectory();
            var projectRoot = currentDir;
            
            // Subir até encontrar a raiz do projeto (onde está o arquivo .sln)
            while (!string.IsNullOrEmpty(projectRoot) && !File.Exists(Path.Combine(projectRoot, "PerfumeMonitor.sln")))
            {
                var parent = Path.GetDirectoryName(projectRoot);
                if (parent == projectRoot) break; // Chegou na raiz do sistema
                projectRoot = parent ?? "";
            }
            
            _configPath = Path.Combine(projectRoot, "config.json");
            _credentialsPath = Path.Combine(projectRoot, "twilio-credentials.json");
            
            System.Diagnostics.Debug.WriteLine($"ConfigManager usando config: {_configPath}");
            System.Diagnostics.Debug.WriteLine($"ConfigManager usando credentials: {_credentialsPath}");
        }

        public async Task<AppConfig> LoadConfigAsync()
        {
            return await Task.Run(() => LoadConfig());
        }

        public async Task SaveConfigAsync(AppConfig config)
        {
            await Task.Run(() => SaveConfig(config));
        }

        public async Task<TwilioCredentials?> LoadTwilioCredentialsAsync()
        {
            return await Task.Run(() => LoadTwilioCredentials());
        }

        public AppConfig LoadConfig()
        {
            try
            {
                if (!File.Exists(_configPath))
                {
                    var defaultConfig = GetDefaultConfig();
                    SaveConfig(defaultConfig);
                    return defaultConfig;
                }

                var json = File.ReadAllText(_configPath);
                var config = JsonConvert.DeserializeObject<AppConfig>(json);
                return config ?? GetDefaultConfig();
            }
            catch
            {
                return GetDefaultConfig();
            }
        }

        public void SaveConfig(AppConfig config)
        {
            try
            {
                var json = JsonConvert.SerializeObject(config, Formatting.Indented);
                File.WriteAllText(_configPath, json);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao salvar configuração: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public TwilioCredentials? LoadTwilioCredentials()
        {
            try
            {
                if (!File.Exists(_credentialsPath))
                {
                    return null;
                }

                var json = File.ReadAllText(_credentialsPath);
                return JsonConvert.DeserializeObject<TwilioCredentials>(json);
            }
            catch
            {
                return null;
            }
        }

        public List<PerfumeUrl> LoadUrls()
        {
            return LoadConfig().PerfumeUrls;
        }

        public void SaveUrls(List<PerfumeUrl> urls)
        {
            var config = LoadConfig();
            config.PerfumeUrls = urls;
            SaveConfig(config);
        }

        private AppConfig GetDefaultConfig()
        {
            return new AppConfig
            {
                CheckIntervalMinutes = 1,
                PerfumeUrls = new List<PerfumeUrl>
                {
                    new PerfumeUrl("Stella Dustin - Silver Alloy", "https://www.thekingofparfums.com.br/produtos/stella-dustin-silver-alloy/"),
                    new PerfumeUrl("Stella Dustin - Marine Alloy", "https://www.thekingofparfums.com.br/produtos/stella-dustin-marine-alloy/")
                }
            };
        }
    }
} 