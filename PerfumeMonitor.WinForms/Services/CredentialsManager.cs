using Newtonsoft.Json;
using PerfumeMonitor.Shared.Models;

namespace PerfumeMonitor.Services
{
    public class CredentialsManager
    {
        private readonly string _credentialsPath;

        public CredentialsManager()
        {
            // Buscar no diretório raiz do projeto
            // Se estamos em bin/Debug/net6.0-windows/, precisamos subir 4 níveis para chegar na raiz
            var currentDir = Directory.GetCurrentDirectory();
            var projectRoot = currentDir;
            
            // Subir até encontrar a raiz do projeto (onde está o arquivo .sln)
            while (!string.IsNullOrEmpty(projectRoot) && !File.Exists(Path.Combine(projectRoot, "PerfumeMonitor.sln")))
            {
                var parent = Path.GetDirectoryName(projectRoot);
                if (parent == projectRoot) break; // Chegou na raiz do sistema
                projectRoot = parent ?? "";
            }
            
            _credentialsPath = Path.Combine(projectRoot, "twilio-credentials.json");
            
            System.Diagnostics.Debug.WriteLine($"CredentialsManager usando arquivo: {_credentialsPath}");
            System.Diagnostics.Debug.WriteLine($"CurrentDirectory: {currentDir}");
            System.Diagnostics.Debug.WriteLine($"ProjectRoot: {projectRoot}");
            System.Diagnostics.Debug.WriteLine($"Arquivo existe: {File.Exists(_credentialsPath)}");
        }

        public TwilioCredentials LoadCredentials()
        {
            try
            {
                if (File.Exists(_credentialsPath))
                {
                    string json = File.ReadAllText(_credentialsPath);
                    var credentials = JsonConvert.DeserializeObject<TwilioCredentials>(json);
                    System.Diagnostics.Debug.WriteLine($"Credenciais carregadas: AccountSid={credentials?.AccountSid}, From={credentials?.FromPhoneNumber}, To={credentials?.ToPhoneNumber}");
                    return credentials ?? new TwilioCredentials();
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"Arquivo de credenciais não encontrado: {_credentialsPath}");
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
                File.WriteAllText(_credentialsPath, json);
                System.Diagnostics.Debug.WriteLine($"Credenciais salvas em: {_credentialsPath}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro ao salvar credenciais: {ex.Message}");
                throw;
            }
        }

        public bool CredentialsExist()
        {
            return File.Exists(_credentialsPath);
        }
    }
} 