namespace IzinTakipSistemi
{
    partial class LogForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            pnlHeader = new Panel();
            picLogoAlan = new PictureBox();
            lblPanelBaslik = new Label();
            dgvLogs = new DataGridView();
            btnYenile = new Button();
            btnExcelAktar = new Button();
            pnlHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picLogoAlan).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvLogs).BeginInit();
            SuspendLayout();
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.FromArgb(31, 73, 125);
            pnlHeader.Controls.Add(picLogoAlan);
            pnlHeader.Controls.Add(lblPanelBaslik);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(1200, 58);
            pnlHeader.TabIndex = 10;
            // 
            // picLogoAlan
            // 
            picLogoAlan.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            picLogoAlan.BackColor = Color.FromArgb(50, 255, 255, 255);
            picLogoAlan.BorderStyle = BorderStyle.FixedSingle;
            picLogoAlan.Location = new Point(1023, 6);
            picLogoAlan.Name = "picLogoAlan";
            picLogoAlan.Size = new Size(165, 46);
            picLogoAlan.SizeMode = PictureBoxSizeMode.Zoom;
            picLogoAlan.TabIndex = 1;
            picLogoAlan.TabStop = false;
            // 
            // lblPanelBaslik
            // 
            lblPanelBaslik.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            lblPanelBaslik.ForeColor = Color.White;
            lblPanelBaslik.Location = new Point(14, 12);
            lblPanelBaslik.Name = "lblPanelBaslik";
            lblPanelBaslik.Size = new Size(500, 34);
            lblPanelBaslik.TabIndex = 0;
            lblPanelBaslik.Text = "İzin Takip Sistemi  —  Sistem Logları";
            // 
            // dgvLogs
            // 
            dgvLogs.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvLogs.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvLogs.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvLogs.Location = new Point(12, 70);
            dgvLogs.Name = "dgvLogs";
            dgvLogs.RowHeadersWidth = 51;
            dgvLogs.Size = new Size(1176, 580);
            dgvLogs.TabIndex = 0;
            // 
            // btnYenile
            // 
            btnYenile.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnYenile.Location = new Point(1094, 662);
            btnYenile.Name = "btnYenile";
            btnYenile.Size = new Size(94, 34);
            btnYenile.TabIndex = 1;
            btnYenile.Text = "Yenile";
            btnYenile.UseVisualStyleBackColor = true;
            // 
            // btnExcelAktar
            // 
            btnExcelAktar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnExcelAktar.Location = new Point(964, 662);
            btnExcelAktar.Name = "btnExcelAktar";
            btnExcelAktar.Size = new Size(120, 34);
            btnExcelAktar.TabIndex = 2;
            btnExcelAktar.Text = "Excel'e Aktar";
            btnExcelAktar.UseVisualStyleBackColor = true;
            btnExcelAktar.Click += btnExcelAktar_Click;
            // 
            // LogForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1200, 710);
            Controls.Add(btnExcelAktar);
            Controls.Add(btnYenile);
            Controls.Add(dgvLogs);
            Controls.Add(pnlHeader);
            MinimumSize = new Size(900, 600);
            Name = "LogForm";
            Text = "Sistem Logları";
            pnlHeader.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)picLogoAlan).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvLogs).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlHeader;
        private Label lblPanelBaslik;
        private PictureBox picLogoAlan;
        private DataGridView dgvLogs;
        private Button btnYenile;
        private Button btnExcelAktar;
    }
}
