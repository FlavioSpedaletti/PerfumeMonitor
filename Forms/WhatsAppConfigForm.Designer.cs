namespace PerfumeMonitor.Forms
{
    partial class WhatsAppConfigForm
    {
        private System.ComponentModel.IContainer components = null;
        private CheckBox checkBoxEnabled;
        private TextBox textBoxAccountSid;
        private TextBox textBoxAuthToken;
        private TextBox textBoxFromNumber;
        private TextBox textBoxToNumber;
        private NumericUpDown numericMaxDaily;
        private NumericUpDown numericCooldown;
        private CheckBox checkBoxReduceNight;
        private Button buttonSave;
        private Button buttonCancel;
        private LinkLabel linkLabelHelp;

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
            this.checkBoxEnabled = new CheckBox();
            this.textBoxAccountSid = new TextBox();
            this.textBoxAuthToken = new TextBox();
            this.textBoxFromNumber = new TextBox();
            this.textBoxToNumber = new TextBox();
            this.numericMaxDaily = new NumericUpDown();
            this.numericCooldown = new NumericUpDown();
            this.checkBoxReduceNight = new CheckBox();
            this.buttonSave = new Button();
            this.buttonCancel = new Button();
            this.linkLabelHelp = new LinkLabel();
            this.SuspendLayout();

            this.checkBoxEnabled.AutoSize = true;
            this.checkBoxEnabled.Location = new Point(20, 20);
            this.checkBoxEnabled.Name = "checkBoxEnabled";
            this.checkBoxEnabled.Size = new Size(182, 19);
            this.checkBoxEnabled.Text = "Habilitar notificações WhatsApp";
            this.checkBoxEnabled.UseVisualStyleBackColor = true;

            var labelAccountSid = new Label();
            labelAccountSid.AutoSize = true;
            labelAccountSid.Location = new Point(20, 60);
            labelAccountSid.Text = "Account SID:";

            this.textBoxAccountSid.Location = new Point(20, 80);
            this.textBoxAccountSid.Name = "textBoxAccountSid";
            this.textBoxAccountSid.Size = new Size(350, 23);
            this.textBoxAccountSid.UseSystemPasswordChar = true;

            var labelAuthToken = new Label();
            labelAuthToken.AutoSize = true;
            labelAuthToken.Location = new Point(20, 120);
            labelAuthToken.Text = "Auth Token:";

            this.textBoxAuthToken.Location = new Point(20, 140);
            this.textBoxAuthToken.Name = "textBoxAuthToken";
            this.textBoxAuthToken.Size = new Size(350, 23);
            this.textBoxAuthToken.UseSystemPasswordChar = true;

            var labelFromNumber = new Label();
            labelFromNumber.AutoSize = true;
            labelFromNumber.Location = new Point(20, 180);
            labelFromNumber.Text = "Número do Twilio (ex: +14155238886):";

            this.textBoxFromNumber.Location = new Point(20, 200);
            this.textBoxFromNumber.Name = "textBoxFromNumber";
            this.textBoxFromNumber.Size = new Size(350, 23);

            var labelToNumber = new Label();
            labelToNumber.AutoSize = true;
            labelToNumber.Location = new Point(20, 240);
            labelToNumber.Text = "Seu número WhatsApp (ex: +5511999999999):";

            this.textBoxToNumber.Location = new Point(20, 260);
            this.textBoxToNumber.Name = "textBoxToNumber";
            this.textBoxToNumber.Size = new Size(350, 23);

            var labelMaxDaily = new Label();
            labelMaxDaily.AutoSize = true;
            labelMaxDaily.Location = new Point(20, 300);
            labelMaxDaily.Text = "Máximo de notificações WhatsApp por dia:";

            this.numericMaxDaily.Location = new Point(20, 320);
            this.numericMaxDaily.Name = "numericMaxDaily";
            this.numericMaxDaily.Size = new Size(100, 23);
            this.numericMaxDaily.Minimum = 1;
            this.numericMaxDaily.Maximum = 100;
            this.numericMaxDaily.Value = 10;

            var labelCooldown = new Label();
            labelCooldown.AutoSize = true;
            labelCooldown.Location = new Point(150, 300);
            labelCooldown.Text = "Cooldown entre notificações (min):";

            this.numericCooldown.Location = new Point(150, 320);
            this.numericCooldown.Name = "numericCooldown";
            this.numericCooldown.Size = new Size(100, 23);
            this.numericCooldown.Minimum = 5;
            this.numericCooldown.Maximum = 1440;
            this.numericCooldown.Value = 30;

            this.checkBoxReduceNight.AutoSize = true;
            this.checkBoxReduceNight.Location = new Point(20, 360);
            this.checkBoxReduceNight.Name = "checkBoxReduceNight";
            this.checkBoxReduceNight.Size = new Size(250, 19);
            this.checkBoxReduceNight.Text = "Reduzir verificações durante madrugada (1h-6h)";
            this.checkBoxReduceNight.UseVisualStyleBackColor = true;
            this.checkBoxReduceNight.Checked = true;

            this.linkLabelHelp.AutoSize = true;
            this.linkLabelHelp.Location = new Point(20, 390);
            this.linkLabelHelp.Name = "linkLabelHelp";
            this.linkLabelHelp.Size = new Size(250, 15);
            this.linkLabelHelp.Text = "Clique aqui para obter credenciais no Twilio";
            this.linkLabelHelp.LinkClicked += new LinkLabelLinkClickedEventHandler(this.linkLabelHelp_LinkClicked);

            this.buttonSave.Location = new Point(210, 430);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new Size(75, 23);
            this.buttonSave.Text = "Salvar";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new EventHandler(this.buttonSave_Click);

            this.buttonCancel.Location = new Point(295, 430);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new Size(75, 23);
            this.buttonCancel.Text = "Cancelar";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new EventHandler(this.buttonCancel_Click);

            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(400, 480);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.linkLabelHelp);
            this.Controls.Add(this.checkBoxReduceNight);
            this.Controls.Add(this.numericCooldown);
            this.Controls.Add(labelCooldown);
            this.Controls.Add(this.numericMaxDaily);
            this.Controls.Add(labelMaxDaily);
            this.Controls.Add(this.textBoxToNumber);
            this.Controls.Add(labelToNumber);
            this.Controls.Add(this.textBoxFromNumber);
            this.Controls.Add(labelFromNumber);
            this.Controls.Add(this.textBoxAuthToken);
            this.Controls.Add(labelAuthToken);
            this.Controls.Add(this.textBoxAccountSid);
            this.Controls.Add(labelAccountSid);
            this.Controls.Add(this.checkBoxEnabled);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "WhatsAppConfigForm";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Configuração WhatsApp";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
} 