using Newtonsoft.Json;
using PerfumeMonitor.Models;

namespace PerfumeMonitor.Services
{
    public class CredentialsManager
    {
        private const string CredentialsFileName = "twilio-credentials.json";

        public TwilioCredentials LoadCredentials()
        {
            try
            {
                if (File.Exists(CredentialsFileName))
                {
                    string json = File.ReadAllText(CredentialsFileName);
                    var credentials = JsonConvert.DeserializeObject<TwilioCredentials>(json);
                    return credentials ?? new TwilioCredentials();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro ao carregar credenciais: {ex.Message}");
            }

            return new TwilioCredentials();
        }

        public void SaveCredentials(TwilioCredentials credentials)
        {
            try
            {
                string json = JsonConvert.SerializeObject(credentials, Formatting.Indented);
                File.WriteAllText(CredentialsFileName, json);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro ao salvar credenciais: {ex.Message}");
                throw;
            }
        }

        public bool CredentialsExist()
        {
            return File.Exists(CredentialsFileName);
        }
    }
} 