using Newtonsoft.Json;
using PerfumeMonitor.Models;

namespace PerfumeMonitor.Services
{
    public class ConfigManager
    {
        private readonly string _configPath = "config.json";

        public AppConfig LoadConfig()
        {
            try
            {
                if (!File.Exists(_configPath))
                {
                    return GetDefaultConfig();
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