using PerfumeMonitor.Shared.Models;
using PerfumeMonitor.Services;

namespace PerfumeMonitor.Forms
{
    public partial class WhatsAppConfigForm : Form
    {
        private ConfigManager _configManager;
        private CredentialsManager _credentialsManager;
        private WhatsAppConfig _config;
        private TwilioCredentials _credentials;

        public WhatsAppConfigForm()
        {
            InitializeComponent();
            _configManager = new ConfigManager();
            _credentialsManager = new CredentialsManager();
            LoadConfiguration();
        }

        private void LoadConfiguration()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("=== CARREGANDO CONFIGURAÇÕES WHATSAPP ===");
                
                var appConfig = _configManager.LoadConfig();
                _config = appConfig.WhatsApp;
                System.Diagnostics.Debug.WriteLine($"AppConfig carregado. WhatsApp config é null: {_config == null}");
                if (_config != null)
                {
                    System.Diagnostics.Debug.WriteLine($"WhatsApp IsEnabled: {_config.IsEnabled}");
                    System.Diagnostics.Debug.WriteLine($"WhatsApp MaxDaily: {_config.MaxDailyNotifications}");
                    System.Diagnostics.Debug.WriteLine($"WhatsApp Cooldown: {_config.CooldownMinutes}");
                }

                _credentials = _credentialsManager.LoadCredentials();
                System.Diagnostics.Debug.WriteLine($"Credentials carregadas. É null: {_credentials == null}");
                if (_credentials != null)
                {
                    System.Diagnostics.Debug.WriteLine($"AccountSid: '{_credentials.AccountSid}'");
                    System.Diagnostics.Debug.WriteLine($"AuthToken: '{_credentials.AuthToken}'");
                    System.Diagnostics.Debug.WriteLine($"FromPhoneNumber: '{_credentials.FromPhoneNumber}'");
                    System.Diagnostics.Debug.WriteLine($"ToPhoneNumber: '{_credentials.ToPhoneNumber}'");
                }

                // Verificar se _config é null
                if (_config == null)
                {
                    System.Diagnostics.Debug.WriteLine("ERRO: _config é null! Criando config padrão.");
                    _config = new WhatsAppConfig
                    {
                        IsEnabled = true,
                        MaxDailyNotifications = 10,
                        CooldownMinutes = 30,
                        ReduceNightTimeChecks = true
                    };
                }

                // Verificar se _credentials é null
                if (_credentials == null)
                {
                    System.Diagnostics.Debug.WriteLine("ERRO: _credentials é null! Criando credentials vazias.");
                    _credentials = new TwilioCredentials();
                }

                // Aplicar valores aos controles
                checkBoxEnabled.Checked = _config.IsEnabled;
                textBoxAccountSid.Text = _credentials.AccountSid ?? "";
                textBoxAuthToken.Text = _credentials.AuthToken ?? "";
                textBoxFromNumber.Text = _credentials.FromPhoneNumber ?? "";
                textBoxToNumber.Text = _credentials.ToPhoneNumber ?? "";
                numericMaxDaily.Value = _config.MaxDailyNotifications;
                numericCooldown.Value = _config.CooldownMinutes;
                checkBoxReduceNight.Checked = _config.ReduceNightTimeChecks;

                System.Diagnostics.Debug.WriteLine("=== CONFIGURAÇÕES APLICADAS AOS CONTROLES ===");
                System.Diagnostics.Debug.WriteLine($"checkBoxEnabled.Checked: {checkBoxEnabled.Checked}");
                System.Diagnostics.Debug.WriteLine($"textBoxAccountSid.Text: '{textBoxAccountSid.Text}'");
                System.Diagnostics.Debug.WriteLine($"textBoxFromNumber.Text: '{textBoxFromNumber.Text}'");
                System.Diagnostics.Debug.WriteLine($"textBoxToNumber.Text: '{textBoxToNumber.Text}'");
                System.Diagnostics.Debug.WriteLine($"numericMaxDaily.Value: {numericMaxDaily.Value}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ERRO ao carregar configurações: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"StackTrace: {ex.StackTrace}");
                MessageBox.Show($"Erro ao carregar configurações do WhatsApp: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            _config.IsEnabled = checkBoxEnabled.Checked;
            _config.MaxDailyNotifications = (int)numericMaxDaily.Value;
            _config.CooldownMinutes = (int)numericCooldown.Value;
            _config.ReduceNightTimeChecks = checkBoxReduceNight.Checked;

            _credentials.AccountSid = textBoxAccountSid.Text.Trim();
            _credentials.AuthToken = textBoxAuthToken.Text.Trim();
            _credentials.FromPhoneNumber = textBoxFromNumber.Text.Trim();
            _credentials.ToPhoneNumber = textBoxToNumber.Text.Trim();

            var appConfig = _configManager.LoadConfig();
            appConfig.WhatsApp = _config;
            _configManager.SaveConfig(appConfig);
            _credentialsManager.SaveCredentials(_credentials);

            MessageBox.Show("Configurações do WhatsApp salvas com sucesso!\n\n" +
                           $"Máximo diário: {_config.MaxDailyNotifications} notificações\n" +
                           $"Cooldown: {_config.CooldownMinutes} minutos\n" +
                           $"Redução noturna: {(_config.ReduceNightTimeChecks ? "Sim" : "Não")}\n\n" +
                           "Credenciais salvas em arquivo separado.", 
                           "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            
            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void linkLabelHelp_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = "https://console.twilio.com/",
                UseShellExecute = true
            });
        }
    }
} 