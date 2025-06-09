namespace PerfumeMonitor.Forms
{
    partial class ConfigForm
    {
        private System.ComponentModel.IContainer components = null;
        private GroupBox groupBoxAddEdit;
        private Label labelName;
        private Label labelUrl;
        private TextBox textBoxName;
        private TextBox textBoxUrl;
        private Button buttonAdd;
        private Button buttonClear;
        private GroupBox groupBoxPerfumes;
        private ListBox listBoxUrls;
        private Button buttonEdit;
        private Button buttonRemove;
        private GroupBox groupBoxSettings;
        private Label labelInterval;
        private NumericUpDown numericUpDownInterval;
        private Button buttonSave;
        private Button buttonCancel;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.groupBoxAddEdit = new GroupBox();
            this.labelName = new Label();
            this.labelUrl = new Label();
            this.textBoxName = new TextBox();
            this.textBoxUrl = new TextBox();
            this.buttonAdd = new Button();
            this.buttonClear = new Button();
            this.groupBoxPerfumes = new GroupBox();
            this.listBoxUrls = new ListBox();
            this.buttonEdit = new Button();
            this.buttonRemove = new Button();
            this.groupBoxSettings = new GroupBox();
            this.labelInterval = new Label();
            this.numericUpDownInterval = new NumericUpDown();
            this.buttonSave = new Button();
            this.buttonCancel = new Button();
            this.SuspendLayout();

            this.groupBoxAddEdit.Text = "Adicionar/Editar Perfume";
            this.groupBoxAddEdit.Location = new Point(12, 12);
            this.groupBoxAddEdit.Size = new Size(580, 90);

            this.labelName.Text = "Nome:";
            this.labelName.Location = new Point(15, 25);
            this.labelName.Size = new Size(50, 23);

            this.textBoxName.Location = new Point(70, 22);
            this.textBoxName.Size = new Size(280, 23);

            this.buttonAdd.Text = "Adicionar";
            this.buttonAdd.Location = new Point(360, 21);
            this.buttonAdd.Size = new Size(80, 25);
            this.buttonAdd.Click += this.buttonAdd_Click;

            this.buttonClear.Text = "Limpar";
            this.buttonClear.Location = new Point(450, 21);
            this.buttonClear.Size = new Size(80, 25);
            this.buttonClear.Click += this.buttonClear_Click;

            this.labelUrl.Text = "URL:";
            this.labelUrl.Location = new Point(15, 55);
            this.labelUrl.Size = new Size(50, 23);

            this.textBoxUrl.Location = new Point(70, 52);
            this.textBoxUrl.Size = new Size(460, 23);

            this.groupBoxPerfumes.Text = "Perfumes Cadastrados";
            this.groupBoxPerfumes.Location = new Point(12, 115);
            this.groupBoxPerfumes.Size = new Size(580, 180);

            this.listBoxUrls.Location = new Point(15, 25);
            this.listBoxUrls.Size = new Size(460, 140);

            this.buttonEdit.Text = "Editar";
            this.buttonEdit.Location = new Point(485, 25);
            this.buttonEdit.Size = new Size(80, 30);
            this.buttonEdit.Click += this.buttonEdit_Click;

            this.buttonRemove.Text = "Remover";
            this.buttonRemove.Location = new Point(485, 65);
            this.buttonRemove.Size = new Size(80, 30);
            this.buttonRemove.Click += this.buttonRemove_Click;

            this.groupBoxSettings.Text = "Configurações Gerais";
            this.groupBoxSettings.Location = new Point(12, 305);
            this.groupBoxSettings.Size = new Size(580, 60);

            this.labelInterval.Text = "Intervalo de Verificação (minutos):";
            this.labelInterval.Location = new Point(15, 25);
            this.labelInterval.Size = new Size(200, 23);

            this.numericUpDownInterval.Location = new Point(220, 23);
            this.numericUpDownInterval.Size = new Size(60, 23);
            this.numericUpDownInterval.Minimum = 1;
            this.numericUpDownInterval.Maximum = 60;
            this.numericUpDownInterval.Value = 1;

            this.buttonSave.Text = "Salvar Configurações";
            this.buttonSave.Location = new Point(380, 380);
            this.buttonSave.Size = new Size(130, 35);
            this.buttonSave.Click += this.buttonSave_Click;

            this.buttonCancel.Text = "Cancelar";
            this.buttonCancel.Location = new Point(520, 380);
            this.buttonCancel.Size = new Size(80, 35);
            this.buttonCancel.Click += this.buttonCancel_Click;

            this.groupBoxAddEdit.Controls.Add(this.labelName);
            this.groupBoxAddEdit.Controls.Add(this.textBoxName);
            this.groupBoxAddEdit.Controls.Add(this.labelUrl);
            this.groupBoxAddEdit.Controls.Add(this.textBoxUrl);
            this.groupBoxAddEdit.Controls.Add(this.buttonAdd);
            this.groupBoxAddEdit.Controls.Add(this.buttonClear);

            this.groupBoxPerfumes.Controls.Add(this.listBoxUrls);
            this.groupBoxPerfumes.Controls.Add(this.buttonEdit);
            this.groupBoxPerfumes.Controls.Add(this.buttonRemove);

            this.groupBoxSettings.Controls.Add(this.labelInterval);
            this.groupBoxSettings.Controls.Add(this.numericUpDownInterval);

            this.Controls.Add(this.groupBoxAddEdit);
            this.Controls.Add(this.groupBoxPerfumes);
            this.Controls.Add(this.groupBoxSettings);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.buttonCancel);

            this.Text = "Configuração de Perfumes";
            this.Size = new Size(620, 460);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
} 