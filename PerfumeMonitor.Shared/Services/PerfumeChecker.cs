using HtmlAgilityPack;
using PerfumeMonitor.Shared.Interfaces;
using PerfumeMonitor.Shared.Models;

namespace PerfumeMonitor.Shared.Services
{
    public class PerfumeChecker : IPerfumeChecker
    {
        private static readonly HttpClient _httpClient = new HttpClient();

        static PerfumeChecker()
        {
            _httpClient.DefaultRequestHeaders.Add("User-Agent", 
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");
            _httpClient.Timeout = TimeSpan.FromSeconds(30);
        }

        public async Task<bool> CheckAvailabilityAsync(PerfumeUrl perfume)
        {
            try
            {
                var html = await _httpClient.GetStringAsync(perfume.Url);
                var doc = new HtmlDocument();
                doc.LoadHtml(html);

                bool isAvailable = IsProductAvailable(doc);
                
                perfume.LastChecked = DateTime.Now;
                perfume.LastStatus = isAvailable ? "Disponível" : "Esgotado";
                
                return isAvailable;
            }
            catch (Exception ex)
            {
                perfume.LastChecked = DateTime.Now;
                perfume.LastStatus = $"Erro: {ex.Message}";
                return false;
            }
        }

        private bool IsProductAvailable(HtmlDocument doc)
        {
            var buyButton = doc.DocumentNode.SelectSingleNode("//input[@type='submit' and contains(@class, 'product-buy-btn')]");
            
            if (buyButton != null)
            {
                string buttonClass = buyButton.GetAttributeValue("class", "").ToLower();
                string buttonValue = buyButton.GetAttributeValue("value", "").ToLower();
                bool isDisabled = buyButton.GetAttributeValue("disabled", null) != null;
                
                if (buttonValue.Contains("esgotado") || isDisabled)
                {
                    return false;
                }
                
                return true;
            }
            
            string pageText = doc.DocumentNode.InnerText.ToLower();
            return !pageText.Contains("esgotado");
        }
    }
} 