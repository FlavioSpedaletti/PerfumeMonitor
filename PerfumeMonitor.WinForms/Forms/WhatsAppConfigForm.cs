using PerfumeMonitor.Models;
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
            var appConfig = _configManager.LoadConfig();
            _config = appConfig.WhatsApp;
            _credentials = _credentialsManager.LoadCredentials();

            checkBoxEnabled.Checked = _config.IsEnabled;
            textBoxAccountSid.Text = _credentials.AccountSid;
            textBoxAuthToken.Text = _credentials.AuthToken;
            textBoxFromNumber.Text = _credentials.FromNumber;
            textBoxToNumber.Text = _credentials.ToNumber;
            numericMaxDaily.Value = _config.MaxDailyNotifications;
            numericCooldown.Value = _config.CooldownMinutes;
            checkBoxReduceNight.Checked = _config.ReduceNightTimeChecks;
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            _config.IsEnabled = checkBoxEnabled.Checked;
            _config.MaxDailyNotifications = (int)numericMaxDaily.Value;
            _config.CooldownMinutes = (int)numericCooldown.Value;
            _config.ReduceNightTimeChecks = checkBoxReduceNight.Checked;

            _credentials.AccountSid = textBoxAccountSid.Text.Trim();
            _credentials.AuthToken = textBoxAuthToken.Text.Trim();
            _credentials.FromNumber = textBoxFromNumber.Text.Trim();
            _credentials.ToNumber = textBoxToNumber.Text.Trim();

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