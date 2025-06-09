namespace PerfumeMonitor.Forms
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;
        private ListView listViewPerfumes;
        private Label labelStatus;
        private Button buttonConfig;
        private Button buttonCheckNow;



        private void InitializeComponent()
        {
            this.listViewPerfumes = new ListView();
            this.labelStatus = new Label();
            this.buttonConfig = new Button();
            this.buttonCheckNow = new Button();
            this.SuspendLayout();
            
            this.listViewPerfumes.Dock = DockStyle.Fill;
            this.listViewPerfumes.View = View.Details;
            this.listViewPerfumes.FullRowSelect = true;
            this.listViewPerfumes.GridLines = true;
            this.listViewPerfumes.Columns.Add("Perfume", 250);
            this.listViewPerfumes.Columns.Add("Status", 100);
            this.listViewPerfumes.Columns.Add("Última Verificação", 150);
            this.listViewPerfumes.Columns.Add("Detalhes", 200);
            
            this.labelStatus.Text = "Monitorando perfumes...";
            this.labelStatus.Dock = DockStyle.Bottom;
            this.labelStatus.Height = 30;
            this.labelStatus.TextAlign = ContentAlignment.MiddleLeft;
            this.labelStatus.Padding = new Padding(10, 0, 0, 0);
            
            this.buttonConfig.Text = "Configurações";
            this.buttonConfig.Size = new Size(100, 30);
            this.buttonConfig.Location = new Point(10, 10);
            this.buttonConfig.Click += ShowConfig;
            
            this.buttonCheckNow.Text = "Verificar Agora";
            this.buttonCheckNow.Size = new Size(100, 30);
            this.buttonCheckNow.Location = new Point(120, 10);
            this.buttonCheckNow.Click += async (s, e) => await CheckPerfumes();

            this.Controls.Add(this.listViewPerfumes);
            this.Controls.Add(this.labelStatus);
            this.Controls.Add(this.buttonConfig);
            this.Controls.Add(this.buttonCheckNow);
            
            this.Text = "Monitor de Perfumes";
            this.Size = new Size(800, 400);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormClosing += MainForm_FormClosing;
            
            this.ResumeLayout(false);
        }
    }
} 