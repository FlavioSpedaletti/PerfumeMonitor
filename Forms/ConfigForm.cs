using PerfumeMonitor.Models;
using PerfumeMonitor.Services;

namespace PerfumeMonitor.Forms
{
    public partial class ConfigForm : Form
    {
        private List<PerfumeUrl> _perfumeUrls;
        private ConfigManager _configManager;
        private AppConfig _config;

        public ConfigForm(List<PerfumeUrl> perfumeUrls, ConfigManager configManager)
        {
            InitializeComponent();
            _perfumeUrls = new List<PerfumeUrl>(perfumeUrls);
            _configManager = configManager;
            _config = _configManager.LoadConfig();
            LoadUrls();
            LoadConfig();
        }

        private void LoadUrls()
        {
            listBoxUrls.Items.Clear();
            foreach (var perfume in _perfumeUrls)
            {
                listBoxUrls.Items.Add($"{perfume.Name} - {perfume.Url}");
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            var name = textBoxName.Text.Trim();
            var url = textBoxUrl.Text.Trim();

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(url))
            {
                MessageBox.Show("Preencha o nome e a URL do perfume.", "Dados incompletos", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
            {
                MessageBox.Show("URL inválida.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _perfumeUrls.Add(new PerfumeUrl(name, url));
            LoadUrls();
            ClearInputs();
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            if (listBoxUrls.SelectedIndex >= 0)
            {
                _perfumeUrls.RemoveAt(listBoxUrls.SelectedIndex);
                LoadUrls();
            }
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            if (listBoxUrls.SelectedIndex >= 0)
            {
                var perfume = _perfumeUrls[listBoxUrls.SelectedIndex];
                textBoxName.Text = perfume.Name;
                textBoxUrl.Text = perfume.Url;
                
                _perfumeUrls.RemoveAt(listBoxUrls.SelectedIndex);
                LoadUrls();
            }
        }

        private void LoadConfig()
        {
            numericUpDownInterval.Value = _config.CheckIntervalMinutes;
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            _config.PerfumeUrls = _perfumeUrls;
            _config.CheckIntervalMinutes = (int)numericUpDownInterval.Value;
            _configManager.SaveConfig(_config);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void ClearInputs()
        {
            textBoxName.Clear();
            textBoxUrl.Clear();
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            ClearInputs();
        }
    }
} 