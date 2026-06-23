namespace IzinTakipSistemi
{
    partial class RaporForm
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
            pnlRaporFiltre = new Panel();
            lblRaporBas = new Label();
            dtpRaporBas = new DateTimePicker();
            lblRaporBit = new Label();
            dtpRaporBit = new DateTimePicker();
            btnRaporFiltrele = new Button();
            btnRaporExcel = new Button();
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            dgvDepartman = new DataGridView();
            tabPage2 = new TabPage();
            dgvAylik = new DataGridView();
            tabPage3 = new TabPage();
            dgvEnCokAlanlar = new DataGridView();
            tabPage4 = new TabPage();
            dgvAktifIzinliler = new DataGridView();
            tabPage5 = new TabPage();
            lblGrafikDepartmanBaslik = new Label();
            picGrafikDepartman = new PictureBox();
            lblGrafikAylikBaslik = new Label();
            picGrafikAylik = new PictureBox();
            tabPage6 = new TabPage();
            dgvPersonelDetay = new DataGridView();
            pnlPersonelFiltre = new Panel();
            lblPersonelAra = new Label();
            txtPersonelAra = new TextBox();
            lblPersonelDepartman = new Label();
            cmbPersonelDepartman = new ComboBox();
            lblPersonelDurum = new Label();
            cmbPersonelDurum = new ComboBox();
            chkSadeceDusukIzin = new CheckBox();
            lblPersonelSonuc = new Label();
            pnlHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picLogoAlan).BeginInit();
            pnlRaporFiltre.SuspendLayout();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvDepartman).BeginInit();
            tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvAylik).BeginInit();
            tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvEnCokAlanlar).BeginInit();
            tabPage4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvAktifIzinliler).BeginInit();
            tabPage5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picGrafikDepartman).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picGrafikAylik).BeginInit();
            tabPage6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvPersonelDetay).BeginInit();
            pnlPersonelFiltre.SuspendLayout();
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
            pnlHeader.Size = new Size(1280, 58);
            pnlHeader.TabIndex = 10;
            // 
            // picLogoAlan
            // 
            picLogoAlan.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            picLogoAlan.BackColor = Color.FromArgb(50, 255, 255, 255);
            picLogoAlan.BorderStyle = BorderStyle.FixedSingle;
            picLogoAlan.Location = new Point(1103, 6);
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
            lblPanelBaslik.Size = new Size(600, 34);
            lblPanelBaslik.TabIndex = 0;
            lblPanelBaslik.Text = "İzin Takip Sistemi  —  Raporlar & İstatistikler";
            // 
            // pnlRaporFiltre
            //
            pnlRaporFiltre.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            pnlRaporFiltre.BackColor = Color.FromArgb(240, 244, 250);
            pnlRaporFiltre.BorderStyle = BorderStyle.FixedSingle;
            pnlRaporFiltre.Controls.Add(lblRaporBas);
            pnlRaporFiltre.Controls.Add(dtpRaporBas);
            pnlRaporFiltre.Controls.Add(lblRaporBit);
            pnlRaporFiltre.Controls.Add(dtpRaporBit);
            pnlRaporFiltre.Controls.Add(btnRaporFiltrele);
            pnlRaporFiltre.Controls.Add(btnRaporExcel);
            pnlRaporFiltre.Location = new Point(0, 58);
            pnlRaporFiltre.Name = "pnlRaporFiltre";
            pnlRaporFiltre.Size = new Size(1280, 46);
            pnlRaporFiltre.TabIndex = 11;
            //
            // lblRaporBas
            //
            lblRaporBas.AutoSize = true;
            lblRaporBas.Font = new Font("Segoe UI", 9F);
            lblRaporBas.Location = new Point(10, 14);
            lblRaporBas.Name = "lblRaporBas";
            lblRaporBas.Text = "Başlangıç:";
            //
            // dtpRaporBas
            //
            dtpRaporBas.Format = DateTimePickerFormat.Short;
            dtpRaporBas.Location = new Point(90, 10);
            dtpRaporBas.Name = "dtpRaporBas";
            dtpRaporBas.ShowCheckBox = true;
            dtpRaporBas.Checked = false;
            dtpRaporBas.Size = new Size(155, 27);
            dtpRaporBas.TabIndex = 0;
            //
            // lblRaporBit
            //
            lblRaporBit.AutoSize = true;
            lblRaporBit.Font = new Font("Segoe UI", 9F);
            lblRaporBit.Location = new Point(260, 14);
            lblRaporBit.Name = "lblRaporBit";
            lblRaporBit.Text = "Bitiş:";
            //
            // dtpRaporBit
            //
            dtpRaporBit.Format = DateTimePickerFormat.Short;
            dtpRaporBit.Location = new Point(305, 10);
            dtpRaporBit.Name = "dtpRaporBit";
            dtpRaporBit.ShowCheckBox = true;
            dtpRaporBit.Checked = false;
            dtpRaporBit.Size = new Size(155, 27);
            dtpRaporBit.TabIndex = 1;
            //
            // btnRaporFiltrele
            //
            btnRaporFiltrele.BackColor = Color.FromArgb(46, 117, 182);
            btnRaporFiltrele.FlatStyle = FlatStyle.Flat;
            btnRaporFiltrele.ForeColor = Color.White;
            btnRaporFiltrele.Location = new Point(475, 9);
            btnRaporFiltrele.Name = "btnRaporFiltrele";
            btnRaporFiltrele.Size = new Size(100, 28);
            btnRaporFiltrele.TabIndex = 2;
            btnRaporFiltrele.Text = "Filtrele";
            btnRaporFiltrele.UseVisualStyleBackColor = false;
            btnRaporFiltrele.Click += btnRaporFiltrele_Click;
            //
            // btnRaporExcel
            //
            btnRaporExcel.BackColor = Color.FromArgb(33, 115, 70);
            btnRaporExcel.FlatStyle = FlatStyle.Flat;
            btnRaporExcel.ForeColor = Color.White;
            btnRaporExcel.Location = new Point(585, 9);
            btnRaporExcel.Name = "btnRaporExcel";
            btnRaporExcel.Size = new Size(130, 28);
            btnRaporExcel.TabIndex = 3;
            btnRaporExcel.Text = "Excel'e Aktar";
            btnRaporExcel.UseVisualStyleBackColor = false;
            btnRaporExcel.Click += btnRaporExcel_Click;
            //
            // tabControl1
            //
            tabControl1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Controls.Add(tabPage3);
            tabControl1.Controls.Add(tabPage4);
            tabControl1.Controls.Add(tabPage5);
            tabControl1.Controls.Add(tabPage6);
            tabControl1.Location = new Point(0, 104);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(1280, 694);
            tabControl1.TabIndex = 6;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(dgvDepartman);
            tabPage1.Location = new Point(4, 29);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(1272, 707);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Departman İstatistikleri";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // dgvDepartman
            // 
            dgvDepartman.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvDepartman.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvDepartman.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvDepartman.Location = new Point(16, 16);
            dgvDepartman.Name = "dgvDepartman";
            dgvDepartman.RowHeadersWidth = 51;
            dgvDepartman.Size = new Size(1240, 675);
            dgvDepartman.TabIndex = 0;
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(dgvAylik);
            tabPage2.Location = new Point(4, 29);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(1272, 707);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "Aylık Yoğunluk";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // dgvAylik
            // 
            dgvAylik.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvAylik.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvAylik.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvAylik.Location = new Point(16, 16);
            dgvAylik.Name = "dgvAylik";
            dgvAylik.RowHeadersWidth = 51;
            dgvAylik.Size = new Size(1240, 675);
            dgvAylik.TabIndex = 1;
            // 
            // tabPage3
            // 
            tabPage3.Controls.Add(dgvEnCokAlanlar);
            tabPage3.Location = new Point(4, 29);
            tabPage3.Name = "tabPage3";
            tabPage3.Padding = new Padding(3);
            tabPage3.Size = new Size(1272, 707);
            tabPage3.TabIndex = 2;
            tabPage3.Text = "En Çok İzin Alanlar";
            tabPage3.UseVisualStyleBackColor = true;
            // 
            // dgvEnCokAlanlar
            // 
            dgvEnCokAlanlar.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvEnCokAlanlar.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvEnCokAlanlar.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvEnCokAlanlar.Location = new Point(16, 16);
            dgvEnCokAlanlar.Name = "dgvEnCokAlanlar";
            dgvEnCokAlanlar.RowHeadersWidth = 51;
            dgvEnCokAlanlar.Size = new Size(1240, 675);
            dgvEnCokAlanlar.TabIndex = 2;
            // 
            // tabPage4
            // 
            tabPage4.Controls.Add(dgvAktifIzinliler);
            tabPage4.Location = new Point(4, 29);
            tabPage4.Name = "tabPage4";
            tabPage4.Padding = new Padding(3);
            tabPage4.Size = new Size(1272, 707);
            tabPage4.TabIndex = 3;
            tabPage4.Text = "Aktif İzinliler";
            tabPage4.UseVisualStyleBackColor = true;
            // 
            // dgvAktifIzinliler
            // 
            dgvAktifIzinliler.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvAktifIzinliler.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvAktifIzinliler.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvAktifIzinliler.Location = new Point(16, 16);
            dgvAktifIzinliler.Name = "dgvAktifIzinliler";
            dgvAktifIzinliler.RowHeadersWidth = 51;
            dgvAktifIzinliler.Size = new Size(1240, 675);
            dgvAktifIzinliler.TabIndex = 3;
            // 
            // tabPage5
            // 
            tabPage5.Controls.Add(lblGrafikDepartmanBaslik);
            tabPage5.Controls.Add(picGrafikDepartman);
            tabPage5.Controls.Add(lblGrafikAylikBaslik);
            tabPage5.Controls.Add(picGrafikAylik);
            tabPage5.Location = new Point(4, 29);
            tabPage5.Name = "tabPage5";
            tabPage5.Padding = new Padding(3);
            tabPage5.Size = new Size(1272, 707);
            tabPage5.TabIndex = 4;
            tabPage5.Text = "Grafikler";
            tabPage5.UseVisualStyleBackColor = true;
            // 
            // lblGrafikDepartmanBaslik
            // 
            lblGrafikDepartmanBaslik.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblGrafikDepartmanBaslik.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblGrafikDepartmanBaslik.Location = new Point(10, 8);
            lblGrafikDepartmanBaslik.Name = "lblGrafikDepartmanBaslik";
            lblGrafikDepartmanBaslik.Size = new Size(616, 22);
            lblGrafikDepartmanBaslik.TabIndex = 0;
            lblGrafikDepartmanBaslik.Text = "Departman Bazlı İzin Günleri (Sütun Grafik)";
            lblGrafikDepartmanBaslik.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // picGrafikDepartman
            // 
            picGrafikDepartman.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            picGrafikDepartman.BorderStyle = BorderStyle.FixedSingle;
            picGrafikDepartman.Location = new Point(10, 34);
            picGrafikDepartman.Name = "picGrafikDepartman";
            picGrafikDepartman.Size = new Size(616, 660);
            picGrafikDepartman.TabIndex = 0;
            picGrafikDepartman.TabStop = false;
            // 
            // lblGrafikAylikBaslik
            // 
            lblGrafikAylikBaslik.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblGrafikAylikBaslik.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblGrafikAylikBaslik.Location = new Point(638, 8);
            lblGrafikAylikBaslik.Name = "lblGrafikAylikBaslik";
            lblGrafikAylikBaslik.Size = new Size(620, 22);
            lblGrafikAylikBaslik.TabIndex = 1;
            lblGrafikAylikBaslik.Text = "Aylık İzin Yoğunluğu (Çizgi Grafik)";
            lblGrafikAylikBaslik.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // picGrafikAylik
            // 
            picGrafikAylik.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            picGrafikAylik.BorderStyle = BorderStyle.FixedSingle;
            picGrafikAylik.Location = new Point(638, 34);
            picGrafikAylik.Name = "picGrafikAylik";
            picGrafikAylik.Size = new Size(620, 660);
            picGrafikAylik.TabIndex = 1;
            picGrafikAylik.TabStop = false;
            // 
            // tabPage6
            // 
            tabPage6.Controls.Add(dgvPersonelDetay);
            tabPage6.Controls.Add(pnlPersonelFiltre);
            tabPage6.Location = new Point(4, 29);
            tabPage6.Name = "tabPage6";
            tabPage6.Padding = new Padding(3);
            tabPage6.Size = new Size(1272, 707);
            tabPage6.TabIndex = 5;
            tabPage6.Text = "Personel Listesi";
            tabPage6.UseVisualStyleBackColor = true;
            // 
            // dgvPersonelDetay
            // 
            dgvPersonelDetay.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvPersonelDetay.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvPersonelDetay.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvPersonelDetay.Location = new Point(16, 56);
            dgvPersonelDetay.Name = "dgvPersonelDetay";
            dgvPersonelDetay.RowHeadersWidth = 51;
            dgvPersonelDetay.Size = new Size(1240, 638);
            dgvPersonelDetay.TabIndex = 0;
            // 
            // pnlPersonelFiltre
            // 
            pnlPersonelFiltre.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            pnlPersonelFiltre.BackColor = Color.FromArgb(240, 244, 250);
            pnlPersonelFiltre.BorderStyle = BorderStyle.FixedSingle;
            pnlPersonelFiltre.Controls.Add(lblPersonelAra);
            pnlPersonelFiltre.Controls.Add(txtPersonelAra);
            pnlPersonelFiltre.Controls.Add(lblPersonelDepartman);
            pnlPersonelFiltre.Controls.Add(cmbPersonelDepartman);
            pnlPersonelFiltre.Controls.Add(lblPersonelDurum);
            pnlPersonelFiltre.Controls.Add(cmbPersonelDurum);
            pnlPersonelFiltre.Controls.Add(chkSadeceDusukIzin);
            pnlPersonelFiltre.Controls.Add(lblPersonelSonuc);
            pnlPersonelFiltre.Location = new Point(3, 3);
            pnlPersonelFiltre.Name = "pnlPersonelFiltre";
            pnlPersonelFiltre.Size = new Size(1266, 46);
            pnlPersonelFiltre.TabIndex = 1;
            // 
            // lblPersonelAra
            // 
            lblPersonelAra.AutoSize = true;
            lblPersonelAra.Font = new Font("Segoe UI", 9F);
            lblPersonelAra.Location = new Point(2, 15);
            lblPersonelAra.Name = "lblPersonelAra";
            lblPersonelAra.Size = new Size(97, 20);
            lblPersonelAra.TabIndex = 0;
            lblPersonelAra.Text = "🔍 Ad / Sicil:";
            // 
            // txtPersonelAra
            // 
            txtPersonelAra.Location = new Point(99, 11);
            txtPersonelAra.Name = "txtPersonelAra";
            txtPersonelAra.PlaceholderText = "İsim veya sicil no ile ara...";
            txtPersonelAra.Size = new Size(220, 27);
            txtPersonelAra.TabIndex = 0;
            txtPersonelAra.TextChanged += txtPersonelAra_TextChanged;
            // 
            // lblPersonelDepartman
            // 
            lblPersonelDepartman.AutoSize = true;
            lblPersonelDepartman.Font = new Font("Segoe UI", 9F);
            lblPersonelDepartman.Location = new Point(325, 15);
            lblPersonelDepartman.Name = "lblPersonelDepartman";
            lblPersonelDepartman.Size = new Size(87, 20);
            lblPersonelDepartman.TabIndex = 1;
            lblPersonelDepartman.Text = "Departman:";
            // 
            // cmbPersonelDepartman
            // 
            cmbPersonelDepartman.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbPersonelDepartman.Location = new Point(418, 10);
            cmbPersonelDepartman.Name = "cmbPersonelDepartman";
            cmbPersonelDepartman.Size = new Size(185, 28);
            cmbPersonelDepartman.TabIndex = 1;
            cmbPersonelDepartman.SelectedIndexChanged += cmbPersonelDepartman_SelectedIndexChanged;
            // 
            // lblPersonelDurum
            // 
            lblPersonelDurum.AutoSize = true;
            lblPersonelDurum.Font = new Font("Segoe UI", 9F);
            lblPersonelDurum.Location = new Point(617, 15);
            lblPersonelDurum.Name = "lblPersonelDurum";
            lblPersonelDurum.Size = new Size(57, 20);
            lblPersonelDurum.TabIndex = 2;
            lblPersonelDurum.Text = "Durum:";
            // 
            // cmbPersonelDurum
            // 
            cmbPersonelDurum.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbPersonelDurum.Items.AddRange(new object[] { "Tümü", "Aktif", "Pasif" });
            cmbPersonelDurum.Location = new Point(680, 10);
            cmbPersonelDurum.Name = "cmbPersonelDurum";
            cmbPersonelDurum.Size = new Size(110, 28);
            cmbPersonelDurum.TabIndex = 2;
            cmbPersonelDurum.SelectedIndexChanged += cmbPersonelDurum_SelectedIndexChanged;
            // 
            // chkSadeceDusukIzin
            // 
            chkSadeceDusukIzin.AutoSize = true;
            chkSadeceDusukIzin.Font = new Font("Segoe UI", 9F);
            chkSadeceDusukIzin.Location = new Point(807, 14);
            chkSadeceDusukIzin.Name = "chkSadeceDusukIzin";
            chkSadeceDusukIzin.Size = new Size(294, 24);
            chkSadeceDusukIzin.TabIndex = 3;
            chkSadeceDusukIzin.Text = "⚠ Yalnızca kritik izin bakiyesi (≤ 5 gün)";
            chkSadeceDusukIzin.CheckedChanged += chkSadeceDusukIzin_CheckedChanged;
            // 
            // lblPersonelSonuc
            // 
            lblPersonelSonuc.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblPersonelSonuc.Font = new Font("Segoe UI", 9F, FontStyle.Italic);
            lblPersonelSonuc.ForeColor = Color.FromArgb(60, 90, 130);
            lblPersonelSonuc.Location = new Point(1030, 15);
            lblPersonelSonuc.Name = "lblPersonelSonuc";
            lblPersonelSonuc.Size = new Size(230, 18);
            lblPersonelSonuc.TabIndex = 4;
            lblPersonelSonuc.TextAlign = ContentAlignment.MiddleRight;
            // 
            // RaporForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1280, 800);
            Controls.Add(tabControl1);
            Controls.Add(pnlRaporFiltre);
            Controls.Add(pnlHeader);
            MinimumSize = new Size(1100, 700);
            Name = "RaporForm";
            Text = "Raporlar ve İstatistikler";
            pnlHeader.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)picLogoAlan).EndInit();
            pnlRaporFiltre.ResumeLayout(false);
            pnlRaporFiltre.PerformLayout();
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvDepartman).EndInit();
            tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvAylik).EndInit();
            tabPage3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvEnCokAlanlar).EndInit();
            tabPage4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvAktifIzinliler).EndInit();
            tabPage5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)picGrafikDepartman).EndInit();
            ((System.ComponentModel.ISupportInitialize)picGrafikAylik).EndInit();
            tabPage6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvPersonelDetay).EndInit();
            pnlPersonelFiltre.ResumeLayout(false);
            pnlPersonelFiltre.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlHeader;
        private Label lblPanelBaslik;
        private PictureBox picLogoAlan;
        // Filtre paneli kontrolleri
        private Panel pnlRaporFiltre;
        private Label lblRaporBas;
        private DateTimePicker dtpRaporBas;
        private Label lblRaporBit;
        private DateTimePicker dtpRaporBit;
        private Button btnRaporFiltrele;
        private Button btnRaporExcel;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private TabPage tabPage3;
        private TabPage tabPage4;
        private TabPage tabPage5;
        private DataGridView dgvDepartman;
        private DataGridView dgvAylik;
        private DataGridView dgvEnCokAlanlar;
        private DataGridView dgvAktifIzinliler;
        private PictureBox picGrafikDepartman;
        private PictureBox picGrafikAylik;
        private Label lblGrafikDepartmanBaslik;
        private Label lblGrafikAylikBaslik;
        private TabPage tabPage6;
        private DataGridView dgvPersonelDetay;
        // Personel filtre kontrolleri
        private Panel pnlPersonelFiltre;
        private Label lblPersonelAra;
        private TextBox txtPersonelAra;
        private Label lblPersonelDepartman;
        private ComboBox cmbPersonelDepartman;
        private Label lblPersonelDurum;
        private ComboBox cmbPersonelDurum;
        private CheckBox chkSadeceDusukIzin;
        private Label lblPersonelSonuc;
    }
}
