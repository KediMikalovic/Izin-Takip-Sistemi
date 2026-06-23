namespace IzinTakipSistemi
{
    partial class YoneticiForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            pnlHeader = new Panel();
            picLogoAlan = new PictureBox();
            btnCikis = new Button();
            lblPanelBaslik = new Label();
            tabMain = new TabControl();
            tabPageIzinTalepleri = new TabPage();
            lblFiltre = new Label();
            cmbFiltre = new ComboBox();
            lblDepartman = new Label();
            cmbDepartman = new ComboBox();
            lblPersonelAra = new Label();
            txtPersonelAra = new TextBox();
            btnAra = new Button();
            pnlSecilenIzin = new Panel();
            lblSecilenIzinBilgi = new Label();
            dgvBekleyenIzinler = new DataGridView();
            label1 = new Label();
            cmbRedSebebi = new ComboBox();
            btnSablonEkle = new Button();
            txtRedAciklamasi = new TextBox();
            btnReddet = new Button();
            btnOnayla = new Button();
            lblDuzenlemeBaslik = new Label();
            dtpBaslangic = new DateTimePicker();
            dtpBitis = new DateTimePicker();
            btnIzinDuzenle = new Button();
            btnIzinSil = new Button();
            lblTalepBas = new Label();
            dtpTalepBas = new DateTimePicker();
            lblTalepAyrac = new Label();
            dtpTalepBit = new DateTimePicker();
            btnTalepExcel = new Button();
            btnLoglariGor = new Button();
            btnRaporlar = new Button();
            tabPageTopluOnay = new TabPage();
            btnTopluYenile = new Button();
            dgvTopluOnay = new DataGridView();
            lblSecilenBaslik = new Label();
            dgvSecilen = new DataGridView();
            lblSeciliSayisi = new Label();
            btnTopluOnayla = new Button();
            pnlHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picLogoAlan).BeginInit();
            tabMain.SuspendLayout();
            tabPageIzinTalepleri.SuspendLayout();
            pnlSecilenIzin.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvBekleyenIzinler).BeginInit();
            tabPageTopluOnay.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvTopluOnay).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvSecilen).BeginInit();
            SuspendLayout();
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.FromArgb(31, 73, 125);
            pnlHeader.Controls.Add(picLogoAlan);
            pnlHeader.Controls.Add(btnCikis);
            pnlHeader.Controls.Add(lblPanelBaslik);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(1280, 58);
            pnlHeader.TabIndex = 30;
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
            // btnCikis
            // 
            btnCikis.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnCikis.BackColor = Color.FromArgb(192, 0, 0);
            btnCikis.FlatAppearance.BorderColor = Color.FromArgb(220, 50, 50);
            btnCikis.FlatStyle = FlatStyle.Flat;
            btnCikis.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnCikis.ForeColor = Color.White;
            btnCikis.Location = new Point(960, 13);
            btnCikis.Name = "btnCikis";
            btnCikis.Size = new Size(130, 32);
            btnCikis.TabIndex = 2;
            btnCikis.Text = "⟵  Çıkış Yap";
            btnCikis.UseVisualStyleBackColor = false;
            btnCikis.Click += btnCikis_Click;
            // 
            // lblPanelBaslik
            // 
            lblPanelBaslik.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            lblPanelBaslik.ForeColor = Color.White;
            lblPanelBaslik.Location = new Point(14, 12);
            lblPanelBaslik.Name = "lblPanelBaslik";
            lblPanelBaslik.Size = new Size(550, 34);
            lblPanelBaslik.TabIndex = 0;
            lblPanelBaslik.Text = "İzin Takip Sistemi  —  Yönetici Paneli";
            // 
            // tabMain
            // 
            tabMain.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tabMain.Controls.Add(tabPageIzinTalepleri);
            tabMain.Controls.Add(tabPageTopluOnay);
            tabMain.Location = new Point(0, 58);
            tabMain.Name = "tabMain";
            tabMain.SelectedIndex = 0;
            tabMain.Size = new Size(1280, 782);
            tabMain.TabIndex = 40;
            // 
            // tabPageIzinTalepleri
            // 
            tabPageIzinTalepleri.Controls.Add(lblFiltre);
            tabPageIzinTalepleri.Controls.Add(cmbFiltre);
            tabPageIzinTalepleri.Controls.Add(lblDepartman);
            tabPageIzinTalepleri.Controls.Add(cmbDepartman);
            tabPageIzinTalepleri.Controls.Add(lblPersonelAra);
            tabPageIzinTalepleri.Controls.Add(txtPersonelAra);
            tabPageIzinTalepleri.Controls.Add(btnAra);
            tabPageIzinTalepleri.Controls.Add(pnlSecilenIzin);
            tabPageIzinTalepleri.Controls.Add(dgvBekleyenIzinler);
            tabPageIzinTalepleri.Controls.Add(label1);
            tabPageIzinTalepleri.Controls.Add(cmbRedSebebi);
            tabPageIzinTalepleri.Controls.Add(btnSablonEkle);
            tabPageIzinTalepleri.Controls.Add(txtRedAciklamasi);
            tabPageIzinTalepleri.Controls.Add(btnReddet);
            tabPageIzinTalepleri.Controls.Add(btnOnayla);
            tabPageIzinTalepleri.Controls.Add(lblDuzenlemeBaslik);
            tabPageIzinTalepleri.Controls.Add(dtpBaslangic);
            tabPageIzinTalepleri.Controls.Add(dtpBitis);
            tabPageIzinTalepleri.Controls.Add(btnIzinDuzenle);
            tabPageIzinTalepleri.Controls.Add(btnIzinSil);
            tabPageIzinTalepleri.Controls.Add(lblTalepBas);
            tabPageIzinTalepleri.Controls.Add(dtpTalepBas);
            tabPageIzinTalepleri.Controls.Add(lblTalepAyrac);
            tabPageIzinTalepleri.Controls.Add(dtpTalepBit);
            tabPageIzinTalepleri.Controls.Add(btnTalepExcel);
            tabPageIzinTalepleri.Controls.Add(btnLoglariGor);
            tabPageIzinTalepleri.Controls.Add(btnRaporlar);
            tabPageIzinTalepleri.Location = new Point(4, 29);
            tabPageIzinTalepleri.Name = "tabPageIzinTalepleri";
            tabPageIzinTalepleri.Padding = new Padding(3);
            tabPageIzinTalepleri.Size = new Size(1272, 749);
            tabPageIzinTalepleri.TabIndex = 0;
            tabPageIzinTalepleri.Text = "  İzin Talepleri  ";
            tabPageIzinTalepleri.UseVisualStyleBackColor = true;
            // 
            // lblFiltre
            // 
            lblFiltre.AutoSize = true;
            lblFiltre.Location = new Point(12, 12);
            lblFiltre.Name = "lblFiltre";
            lblFiltre.Size = new Size(104, 20);
            lblFiltre.TabIndex = 6;
            lblFiltre.Text = "Durum Filtresi:";
            // 
            // cmbFiltre
            // 
            cmbFiltre.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbFiltre.FormattingEnabled = true;
            cmbFiltre.Location = new Point(12, 34);
            cmbFiltre.Name = "cmbFiltre";
            cmbFiltre.Size = new Size(165, 28);
            cmbFiltre.TabIndex = 5;
            cmbFiltre.SelectedIndexChanged += cmbFiltre_SelectedIndexChanged;
            // 
            // lblDepartman
            // 
            lblDepartman.AutoSize = true;
            lblDepartman.Location = new Point(193, 12);
            lblDepartman.Name = "lblDepartman";
            lblDepartman.Size = new Size(87, 20);
            lblDepartman.TabIndex = 13;
            lblDepartman.Text = "Departman:";
            // 
            // cmbDepartman
            // 
            cmbDepartman.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbDepartman.FormattingEnabled = true;
            cmbDepartman.Location = new Point(193, 34);
            cmbDepartman.Name = "cmbDepartman";
            cmbDepartman.Size = new Size(190, 28);
            cmbDepartman.TabIndex = 14;
            cmbDepartman.SelectedIndexChanged += cmbDepartman_SelectedIndexChanged;
            // 
            // lblPersonelAra
            // 
            lblPersonelAra.AutoSize = true;
            lblPersonelAra.Location = new Point(400, 12);
            lblPersonelAra.Name = "lblPersonelAra";
            lblPersonelAra.Size = new Size(94, 20);
            lblPersonelAra.TabIndex = 15;
            lblPersonelAra.Text = "Personel Adı:";
            // 
            // txtPersonelAra
            // 
            txtPersonelAra.Location = new Point(400, 34);
            txtPersonelAra.Name = "txtPersonelAra";
            txtPersonelAra.Size = new Size(190, 27);
            txtPersonelAra.TabIndex = 16;
            // 
            // btnAra
            // 
            btnAra.Location = new Point(600, 32);
            btnAra.Name = "btnAra";
            btnAra.Size = new Size(70, 30);
            btnAra.TabIndex = 17;
            btnAra.Text = "Ara";
            btnAra.UseVisualStyleBackColor = true;
            btnAra.Click += btnAra_Click;
            // 
            // pnlSecilenIzin
            // 
            pnlSecilenIzin.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            pnlSecilenIzin.BackColor = Color.FromArgb(230, 230, 230);
            pnlSecilenIzin.BorderStyle = BorderStyle.FixedSingle;
            pnlSecilenIzin.Controls.Add(lblSecilenIzinBilgi);
            pnlSecilenIzin.Location = new Point(12, 70);
            pnlSecilenIzin.Name = "pnlSecilenIzin";
            pnlSecilenIzin.Size = new Size(838, 42);
            pnlSecilenIzin.TabIndex = 31;
            // 
            // lblSecilenIzinBilgi
            // 
            lblSecilenIzinBilgi.Dock = DockStyle.Fill;
            lblSecilenIzinBilgi.Font = new Font("Segoe UI", 9.5F);
            lblSecilenIzinBilgi.ForeColor = Color.FromArgb(120, 120, 120);
            lblSecilenIzinBilgi.Location = new Point(0, 0);
            lblSecilenIzinBilgi.Name = "lblSecilenIzinBilgi";
            lblSecilenIzinBilgi.Size = new Size(836, 40);
            lblSecilenIzinBilgi.TabIndex = 0;
            lblSecilenIzinBilgi.Text = "  Listeden bir izin talebi seçin...";
            lblSecilenIzinBilgi.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // dgvBekleyenIzinler
            // 
            dgvBekleyenIzinler.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvBekleyenIzinler.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvBekleyenIzinler.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvBekleyenIzinler.Location = new Point(12, 120);
            dgvBekleyenIzinler.Name = "dgvBekleyenIzinler";
            dgvBekleyenIzinler.RowHeadersWidth = 51;
            dgvBekleyenIzinler.Size = new Size(838, 610);
            dgvBekleyenIzinler.TabIndex = 0;
            dgvBekleyenIzinler.SelectionChanged += dgvBekleyenIzinler_SelectionChanged;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label1.AutoSize = true;
            label1.Location = new Point(870, 10);
            label1.Name = "label1";
            label1.Size = new Size(88, 20);
            label1.TabIndex = 2;
            label1.Text = "Red Sebebi:";
            // 
            // cmbRedSebebi
            // 
            cmbRedSebebi.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            cmbRedSebebi.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbRedSebebi.FormattingEnabled = true;
            cmbRedSebebi.Items.AddRange(new object[] { "— Şablon Seç —", "Departmanda yeterli personel bulunmuyor.", "Belirtilen tarihler proje teslim dönemine denk geliyor.", "Bu dönem kritik iş yoğunluğu nedeniyle izin uygun değildir.", "Yıllık izin hakkı yetersiz olduğu için talep onaylanamamıştır.", "Belirtilen tarihlerde başka bir izinle çakışma mevcuttur.", "Talep edilen sürede asgari personel sayısı altına düşülmektedir.", "Belge/onay eksikliği nedeniyle talep değerlendirmeye alınamamıştır." });
            cmbRedSebebi.Location = new Point(866, 94);
            cmbRedSebebi.Name = "cmbRedSebebi";
            cmbRedSebebi.Size = new Size(358, 28);
            cmbRedSebebi.TabIndex = 33;
            cmbRedSebebi.SelectedIndexChanged += cmbRedSebebi_SelectedIndexChanged;
            //
            // btnSablonEkle
            //
            btnSablonEkle.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSablonEkle.BackColor = Color.FromArgb(46, 117, 182);
            btnSablonEkle.FlatAppearance.BorderColor = Color.FromArgb(70, 140, 200);
            btnSablonEkle.FlatStyle = FlatStyle.Flat;
            btnSablonEkle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnSablonEkle.ForeColor = Color.White;
            btnSablonEkle.Location = new Point(1228, 94);
            btnSablonEkle.Name = "btnSablonEkle";
            btnSablonEkle.Size = new Size(36, 28);
            btnSablonEkle.TabIndex = 34;
            btnSablonEkle.Text = "➕";
            btnSablonEkle.UseVisualStyleBackColor = false;
            btnSablonEkle.Click += btnSablonEkle_Click;
            //
            // txtRedAciklamasi
            // 
            txtRedAciklamasi.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            txtRedAciklamasi.Location = new Point(870, 128);
            txtRedAciklamasi.Multiline = true;
            txtRedAciklamasi.Name = "txtRedAciklamasi";
            txtRedAciklamasi.PlaceholderText = "Şablon seçin veya kendi açıklamanızı yazın (zorunlu)...";
            txtRedAciklamasi.Size = new Size(398, 112);
            txtRedAciklamasi.TabIndex = 1;
            // 
            // btnReddet
            // 
            btnReddet.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnReddet.Location = new Point(870, 246);
            btnReddet.Name = "btnReddet";
            btnReddet.Size = new Size(190, 36);
            btnReddet.TabIndex = 4;
            btnReddet.Text = "Reddet";
            btnReddet.UseVisualStyleBackColor = true;
            btnReddet.Click += btnReddet_Click;
            // 
            // btnOnayla
            // 
            btnOnayla.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnOnayla.Location = new Point(1066, 246);
            btnOnayla.Name = "btnOnayla";
            btnOnayla.Size = new Size(192, 36);
            btnOnayla.TabIndex = 3;
            btnOnayla.Text = "Onayla";
            btnOnayla.UseVisualStyleBackColor = true;
            btnOnayla.Click += btnOnayla_Click;
            // 
            // lblDuzenlemeBaslik
            // 
            lblDuzenlemeBaslik.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblDuzenlemeBaslik.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblDuzenlemeBaslik.ForeColor = Color.FromArgb(31, 73, 125);
            lblDuzenlemeBaslik.Location = new Point(866, 299);
            lblDuzenlemeBaslik.Name = "lblDuzenlemeBaslik";
            lblDuzenlemeBaslik.Size = new Size(398, 22);
            lblDuzenlemeBaslik.TabIndex = 18;
            lblDuzenlemeBaslik.Text = "── İzin Düzenleme ──";
            lblDuzenlemeBaslik.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // dtpBaslangic
            // 
            dtpBaslangic.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            dtpBaslangic.Location = new Point(866, 335);
            dtpBaslangic.Name = "dtpBaslangic";
            dtpBaslangic.Size = new Size(398, 27);
            dtpBaslangic.TabIndex = 8;
            // 
            // dtpBitis
            // 
            dtpBitis.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            dtpBitis.Location = new Point(866, 368);
            dtpBitis.Name = "dtpBitis";
            dtpBitis.Size = new Size(398, 27);
            dtpBitis.TabIndex = 9;
            // 
            // btnIzinDuzenle
            // 
            btnIzinDuzenle.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnIzinDuzenle.Location = new Point(866, 401);
            btnIzinDuzenle.Name = "btnIzinDuzenle";
            btnIzinDuzenle.Size = new Size(398, 34);
            btnIzinDuzenle.TabIndex = 10;
            btnIzinDuzenle.Text = "Düzenle";
            btnIzinDuzenle.UseVisualStyleBackColor = true;
            btnIzinDuzenle.Click += btnIzinDuzenle_Click;
            // 
            // btnIzinSil
            // 
            btnIzinSil.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnIzinSil.Location = new Point(866, 450);
            btnIzinSil.Name = "btnIzinSil";
            btnIzinSil.Size = new Size(398, 34);
            btnIzinSil.TabIndex = 12;
            btnIzinSil.Text = "İzin Sil";
            btnIzinSil.UseVisualStyleBackColor = true;
            btnIzinSil.Click += btnIzinSil_Click;
            // 
            // lblTalepBas
            // 
            lblTalepBas.AutoSize = true;
            lblTalepBas.Font = new Font("Segoe UI", 9F);
            lblTalepBas.Location = new Point(688, 12);
            lblTalepBas.Name = "lblTalepBas";
            lblTalepBas.Size = new Size(75, 20);
            lblTalepBas.TabIndex = 27;
            lblTalepBas.Text = "Başlangıç:";
            // 
            // dtpTalepBas
            // 
            dtpTalepBas.Checked = false;
            dtpTalepBas.Format = DateTimePickerFormat.Short;
            dtpTalepBas.Location = new Point(688, 33);
            dtpTalepBas.Name = "dtpTalepBas";
            dtpTalepBas.ShowCheckBox = true;
            dtpTalepBas.Size = new Size(155, 27);
            dtpTalepBas.TabIndex = 28;
            // 
            // lblTalepAyrac
            // 
            lblTalepAyrac.AutoSize = true;
            lblTalepAyrac.Location = new Point(849, 37);
            lblTalepAyrac.Name = "lblTalepAyrac";
            lblTalepAyrac.Size = new Size(17, 20);
            lblTalepAyrac.TabIndex = 29;
            lblTalepAyrac.Text = "–";
            // 
            // dtpTalepBit
            // 
            dtpTalepBit.Checked = false;
            dtpTalepBit.Format = DateTimePickerFormat.Short;
            dtpTalepBit.Location = new Point(870, 33);
            dtpTalepBit.Name = "dtpTalepBit";
            dtpTalepBit.ShowCheckBox = true;
            dtpTalepBit.Size = new Size(155, 27);
            dtpTalepBit.TabIndex = 30;
            // 
            // btnTalepExcel
            // 
            btnTalepExcel.BackColor = Color.FromArgb(33, 115, 70);
            btnTalepExcel.FlatAppearance.BorderColor = Color.FromArgb(60, 140, 90);
            btnTalepExcel.FlatStyle = FlatStyle.Flat;
            btnTalepExcel.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            btnTalepExcel.ForeColor = Color.White;
            btnTalepExcel.Location = new Point(1035, 32);
            btnTalepExcel.Name = "btnTalepExcel";
            btnTalepExcel.Size = new Size(140, 30);
            btnTalepExcel.TabIndex = 32;
            btnTalepExcel.Text = "📊  Excel'e Aktar";
            btnTalepExcel.UseVisualStyleBackColor = false;
            btnTalepExcel.Click += btnTalepExcel_Click;
            // 
            // btnLoglariGor
            // 
            btnLoglariGor.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnLoglariGor.Location = new Point(870, 700);
            btnLoglariGor.Name = "btnLoglariGor";
            btnLoglariGor.Size = new Size(190, 36);
            btnLoglariGor.TabIndex = 7;
            btnLoglariGor.Text = "Log'ları Gör";
            btnLoglariGor.UseVisualStyleBackColor = true;
            btnLoglariGor.Click += btnLoglariGor_Click;
            // 
            // btnRaporlar
            // 
            btnRaporlar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnRaporlar.Location = new Point(1078, 700);
            btnRaporlar.Name = "btnRaporlar";
            btnRaporlar.Size = new Size(190, 36);
            btnRaporlar.TabIndex = 11;
            btnRaporlar.Text = "Raporları Gör";
            btnRaporlar.UseVisualStyleBackColor = true;
            btnRaporlar.Click += btnRaporlar_Click;
            // 
            // tabPageTopluOnay
            // 
            tabPageTopluOnay.Controls.Add(btnTopluYenile);
            tabPageTopluOnay.Controls.Add(dgvTopluOnay);
            tabPageTopluOnay.Controls.Add(lblSecilenBaslik);
            tabPageTopluOnay.Controls.Add(dgvSecilen);
            tabPageTopluOnay.Controls.Add(lblSeciliSayisi);
            tabPageTopluOnay.Controls.Add(btnTopluOnayla);
            tabPageTopluOnay.Location = new Point(4, 29);
            tabPageTopluOnay.Name = "tabPageTopluOnay";
            tabPageTopluOnay.Padding = new Padding(3);
            tabPageTopluOnay.Size = new Size(1272, 749);
            tabPageTopluOnay.TabIndex = 1;
            tabPageTopluOnay.Text = "  Toplu Onay  ";
            tabPageTopluOnay.UseVisualStyleBackColor = true;
            // 
            // btnTopluYenile
            // 
            btnTopluYenile.Location = new Point(12, 12);
            btnTopluYenile.Name = "btnTopluYenile";
            btnTopluYenile.Size = new Size(130, 30);
            btnTopluYenile.TabIndex = 0;
            btnTopluYenile.Text = "↺  Yenile";
            btnTopluYenile.UseVisualStyleBackColor = true;
            btnTopluYenile.Click += btnTopluYenile_Click;
            // 
            // dgvTopluOnay
            // 
            dgvTopluOnay.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvTopluOnay.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvTopluOnay.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvTopluOnay.Location = new Point(12, 52);
            dgvTopluOnay.Name = "dgvTopluOnay";
            dgvTopluOnay.RowHeadersWidth = 30;
            dgvTopluOnay.Size = new Size(838, 680);
            dgvTopluOnay.TabIndex = 1;
            dgvTopluOnay.CellValueChanged += dgvTopluOnay_CellValueChanged;
            dgvTopluOnay.CurrentCellDirtyStateChanged += dgvTopluOnay_CurrentCellDirtyStateChanged;
            // 
            // lblSecilenBaslik
            // 
            lblSecilenBaslik.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblSecilenBaslik.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblSecilenBaslik.ForeColor = Color.FromArgb(31, 73, 125);
            lblSecilenBaslik.Location = new Point(870, 12);
            lblSecilenBaslik.Name = "lblSecilenBaslik";
            lblSecilenBaslik.Size = new Size(398, 22);
            lblSecilenBaslik.TabIndex = 2;
            lblSecilenBaslik.Text = "── Seçili Talepler ──";
            lblSecilenBaslik.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // dgvSecilen
            // 
            dgvSecilen.AllowUserToAddRows = false;
            dgvSecilen.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            dgvSecilen.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvSecilen.Location = new Point(870, 40);
            dgvSecilen.Name = "dgvSecilen";
            dgvSecilen.ReadOnly = true;
            dgvSecilen.RowHeadersWidth = 30;
            dgvSecilen.Size = new Size(398, 590);
            dgvSecilen.TabIndex = 3;
            // 
            // lblSeciliSayisi
            // 
            lblSeciliSayisi.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            lblSeciliSayisi.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lblSeciliSayisi.ForeColor = Color.DarkBlue;
            lblSeciliSayisi.Location = new Point(870, 642);
            lblSeciliSayisi.Name = "lblSeciliSayisi";
            lblSeciliSayisi.Size = new Size(398, 26);
            lblSeciliSayisi.TabIndex = 4;
            lblSeciliSayisi.Text = "0 talep seçildi";
            lblSeciliSayisi.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnTopluOnayla
            // 
            btnTopluOnayla.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnTopluOnayla.BackColor = Color.FromArgb(0, 128, 0);
            btnTopluOnayla.FlatAppearance.BorderColor = Color.FromArgb(0, 160, 0);
            btnTopluOnayla.FlatStyle = FlatStyle.Flat;
            btnTopluOnayla.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnTopluOnayla.ForeColor = Color.White;
            btnTopluOnayla.Location = new Point(870, 678);
            btnTopluOnayla.Name = "btnTopluOnayla";
            btnTopluOnayla.Size = new Size(398, 46);
            btnTopluOnayla.TabIndex = 5;
            btnTopluOnayla.Text = "✔  Seçili İzinleri Onayla";
            btnTopluOnayla.UseVisualStyleBackColor = false;
            btnTopluOnayla.Click += btnTopluOnayla_Click;
            // 
            // YoneticiForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1280, 840);
            Controls.Add(tabMain);
            Controls.Add(pnlHeader);
            MinimumSize = new Size(1100, 750);
            Name = "YoneticiForm";
            Text = "Yönetici Paneli";
            Load += YoneticiForm_Load;
            pnlHeader.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)picLogoAlan).EndInit();
            tabMain.ResumeLayout(false);
            tabPageIzinTalepleri.ResumeLayout(false);
            tabPageIzinTalepleri.PerformLayout();
            pnlSecilenIzin.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvBekleyenIzinler).EndInit();
            tabPageTopluOnay.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvTopluOnay).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvSecilen).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlHeader;
        private Label lblPanelBaslik;
        private PictureBox picLogoAlan;
        private Button btnCikis;
        private TabControl tabMain;
        private TabPage tabPageIzinTalepleri;
        private TabPage tabPageTopluOnay;
        // Tab1 — mevcut kontroller
        private Panel pnlSecilenIzin;
        private Label lblSecilenIzinBilgi;
        private DataGridView dgvBekleyenIzinler;
        private TextBox txtRedAciklamasi;
        private ComboBox cmbRedSebebi;
        private Button btnSablonEkle;
        private Label label1;
        private Button btnOnayla;
        private Button btnReddet;
        private ComboBox cmbFiltre;
        private Label lblFiltre;
        private Button btnLoglariGor;
        private DateTimePicker dtpBaslangic;
        private DateTimePicker dtpBitis;
        private Button btnIzinDuzenle;
        private Button btnRaporlar;
        private Button btnIzinSil;
        private Label lblDepartman;
        private ComboBox cmbDepartman;
        private Label lblPersonelAra;
        private TextBox txtPersonelAra;
        private Button btnAra;
        private Label lblDuzenlemeBaslik;
        private Label lblTalepBas;
        private DateTimePicker dtpTalepBas;
        private Label lblTalepAyrac;
        private DateTimePicker dtpTalepBit;
        private Button btnTalepExcel;
        // Tab2 — toplu onay
        private Button btnTopluYenile;
        private DataGridView dgvTopluOnay;
        private Label lblSecilenBaslik;
        private DataGridView dgvSecilen;
        private Label lblSeciliSayisi;
        private Button btnTopluOnayla;
    }
}
