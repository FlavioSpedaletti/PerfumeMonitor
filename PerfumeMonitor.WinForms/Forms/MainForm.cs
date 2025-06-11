using PerfumeMonitor.Shared.Models;
using PerfumeMonitor.Services;

namespace PerfumeMonitor.Forms
{
    public partial class MainForm : Form
    {
        private NotifyIcon _notifyIcon;
        private System.Windows.Forms.Timer _monitorTimer;
        private ConfigManager _configManager;
        private CredentialsManager _credentialsManager;
        private PerfumeChecker _perfumeChecker;
        private WhatsAppNotifier _whatsAppNotifier;
        private List<PerfumeUrl> _perfumeUrls;
        private bool _isMonitoring = true;

        public MainForm()
        {
            InitializeComponent();
            InitializeServices();
            InitializeTrayIcon();
            InitializeTimer();
            LoadConfiguration();
            PerformInitialCheck();
            
            this.WindowState = FormWindowState.Minimized;
            this.ShowInTaskbar = false;
        }

        private void InitializeServices()
        {
            _configManager = new ConfigManager();
            _credentialsManager = new CredentialsManager();
            _perfumeChecker = new PerfumeChecker();
            
            var config = _configManager.LoadConfig();
            var credentials = _credentialsManager.LoadCredentials();
            _whatsAppNotifier = new WhatsAppNotifier(config.WhatsApp, credentials);
        }

        private void InitializeTrayIcon()
        {
            _notifyIcon = new NotifyIcon();
            _notifyIcon.Icon = CreatePerfumeIcon();
            _notifyIcon.Text = "Monitor de Perfumes";
            _notifyIcon.Visible = true;

            var contextMenu = new ContextMenuStrip();
            contextMenu.Items.Add("Mostrar", null, ShowForm);
            contextMenu.Items.Add("Configurações", null, ShowConfig);
            contextMenu.Items.Add("WhatsApp", null, ShowWhatsAppConfig);
            contextMenu.Items.Add("-");
            contextMenu.Items.Add(_isMonitoring ? "Pausar" : "Retomar", null, ToggleMonitoring);
            contextMenu.Items.Add("-");
            contextMenu.Items.Add("Sair", null, ExitApplication);

            _notifyIcon.ContextMenuStrip = contextMenu;
            _notifyIcon.DoubleClick += ShowForm;
        }

        private void InitializeTimer()
        {
            _monitorTimer = new System.Windows.Forms.Timer();
            UpdateTimerInterval();
            _monitorTimer.Tick += async (s, e) => await CheckPerfumes();
            _monitorTimer.Start();
        }

        private void UpdateTimerInterval()
        {
            var config = _configManager.LoadConfig();
            _monitorTimer.Interval = config.CheckIntervalMinutes * 60 * 1000;
            UpdateStatusLabel();
        }

        private void UpdateStatusLabel()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(() => UpdateStatusLabel());
                return;
            }

            var config = _configManager.LoadConfig();
            string intervalText = config.CheckIntervalMinutes == 1 ? 
                "minuto" : $"{config.CheckIntervalMinutes} minutos";
            labelStatus.Text = $"Monitorando perfumes a cada {intervalText}...";
        }

        private void LoadConfiguration()
        {
            _perfumeUrls = _configManager.LoadUrls();
        }

        private void PerformInitialCheck()
        {
            if (_perfumeUrls == null || _perfumeUrls.Count == 0) return;
            
            Task.Run(async () =>
            {
                await CheckPerfumes();
            });
        }

        private async Task CheckPerfumes()
        {
            if (!_isMonitoring) return;

            SetTrayIconVerifying(true);

            try
            {
                foreach (var perfume in _perfumeUrls)
                {
                    try
                    {
                        bool wasAvailable = perfume.IsAvailable;
                        bool isNowAvailable = await _perfumeChecker.CheckAvailabilityAsync(perfume);
                        
                        perfume.IsAvailable = isNowAvailable;

                        if (!wasAvailable && isNowAvailable)
                        {
                            ShowAvailabilityNotification(perfume);
                        }
                    }
                    catch (Exception ex)
                    {
                        perfume.LastStatus = $"Erro: {ex.Message}";
                    }
                }

                _configManager.SaveUrls(_perfumeUrls);
                UpdateUI();
            }
            finally
            {
                SetTrayIconVerifying(false);
            }
        }

        private Icon CreatePerfumeIcon()
        {
            var bitmap = new Bitmap(16, 16);
            using (var graphics = Graphics.FromImage(bitmap))
            {
                graphics.Clear(Color.Transparent);
                using (var brush = new SolidBrush(Color.FromArgb(255, 138, 43, 226)))
                {
                    graphics.FillEllipse(brush, 6, 2, 8, 10);
                    graphics.FillRectangle(brush, 8, 1, 4, 3);
                    graphics.FillRectangle(brush, 9, 0, 2, 2);
                }
                using (var brush = new SolidBrush(Color.FromArgb(255, 75, 0, 130)))
                {
                    graphics.FillEllipse(brush, 2, 6, 6, 8);
                    graphics.FillRectangle(brush, 4, 5, 2, 3);
                }
            }
            return Icon.FromHandle(bitmap.GetHicon());
        }

        private void SetTrayIconVerifying(bool isVerifying)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(() => SetTrayIconVerifying(isVerifying));
                return;
            }

            if (isVerifying)
            {
                _notifyIcon.Icon = SystemIcons.Warning;
                _notifyIcon.Text = "Monitor de Perfumes - Verificando...";
            }
            else
            {
                _notifyIcon.Icon = CreatePerfumeIcon();
                _notifyIcon.Text = _isMonitoring ? "Monitor de Perfumes - Ativo" : "Monitor de Perfumes - Pausado";
            }
        }

        private async void ShowAvailabilityNotification(PerfumeUrl perfume)
        {
            _notifyIcon.ShowBalloonTip(5000, 
                "Perfume Disponível!", 
                $"{perfume.Name} está disponível para compra!",
                ToolTipIcon.Info);

            try
            {
                await _whatsAppNotifier.SendNotificationAsync(perfume);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro ao enviar WhatsApp: {ex.Message}");
            }

            if (this.InvokeRequired)
            {
                this.Invoke(() => ShowMessageBoxNotification(perfume));
            }
            else
            {
                ShowMessageBoxNotification(perfume);
            }
        }

        private void ShowMessageBoxNotification(PerfumeUrl perfume)
        {
            MessageBox.Show(
                $"🎉 ÓTIMA NOTÍCIA! 🎉\n\n" +
                $"O perfume '{perfume.Name}' está DISPONÍVEL para compra!\n\n" +
                $"URL: {perfume.Url}\n\n" +
                $"Verificado em: {perfume.LastChecked:dd/MM/yyyy HH:mm}\n\n" +
                $"Aproveite antes que esgote novamente! 🏃‍♂️💨",
                "🔔 Perfume Disponível!",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void ShowForm(object? sender, EventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.ShowInTaskbar = true;
            this.BringToFront();
        }

        private void ShowConfig(object? sender, EventArgs e)
        {
            var configForm = new ConfigForm(_perfumeUrls, _configManager);
            if (configForm.ShowDialog() == DialogResult.OK)
            {
                LoadConfiguration();
                UpdateTimerInterval();
                UpdateUI();
            }
        }

        private void ShowWhatsAppConfig(object? sender, EventArgs e)
        {
            using (var whatsAppForm = new WhatsAppConfigForm())
            {
                whatsAppForm.ShowDialog();
                
                var config = _configManager.LoadConfig();
                var credentials = _credentialsManager.LoadCredentials();
                _whatsAppNotifier = new WhatsAppNotifier(config.WhatsApp, credentials);
            }
        }

        private void ToggleMonitoring(object? sender, EventArgs e)
        {
            _isMonitoring = !_isMonitoring;
            var menuItem = sender as ToolStripMenuItem;
            if (menuItem != null)
            {
                menuItem.Text = _isMonitoring ? "Pausar" : "Retomar";
            }
            
            _notifyIcon.Text = _isMonitoring ? "Monitor de Perfumes - Ativo" : "Monitor de Perfumes - Pausado";
        }

        private void ExitApplication(object? sender, EventArgs e)
        {
            _notifyIcon.Visible = false;
            Application.Exit();
        }

        private void UpdateUI()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(UpdateUI));
                return;
            }

            listViewPerfumes.Items.Clear();
            foreach (var perfume in _perfumeUrls)
            {
                var item = new ListViewItem(perfume.Name);
                item.SubItems.Add(perfume.IsAvailable ? "Disponível" : "Esgotado");
                item.SubItems.Add(perfume.LastChecked.ToString("dd/MM/yyyy HH:mm"));
                item.SubItems.Add(perfume.LastStatus);
                item.Tag = perfume;
                
                if (perfume.IsAvailable)
                {
                    item.BackColor = Color.LightGreen;
                }
                
                listViewPerfumes.Items.Add(item);
            }
        }

        protected override void SetVisibleCore(bool value)
        {
            base.SetVisibleCore(value);
            if (value)
            {
                UpdateUI();
                UpdateStatusLabel();
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                this.Hide();
                this.ShowInTaskbar = false;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _notifyIcon?.Dispose();
                _monitorTimer?.Dispose();
                components?.Dispose();
            }
            base.Dispose(disposing);
        }


    }
} 