namespace IzinTakipSistemi
{
    partial class AmirForm
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
            tabPagePersonel = new TabPage();
            pnlSecilenPersonel = new Panel();
            lblSecilenPersonelBilgi = new Label();
            label1 = new Label();
            txtArama = new TextBox();
            dgvPersoneller = new DataGridView();
            label2 = new Label();
            dgvIzinler = new DataGridView();
            label3 = new Label();
            dtpBaslangic = new DateTimePicker();
            label4 = new Label();
            dtpBitis = new DateTimePicker();
            btnIzinSil = new Button();
            btnIzinEkle = new Button();
            btnIzinDuzenle = new Button();
            btnAvansIzinEkle = new Button();
            lblPersAralik = new Label();
            dtpPersFiltreBas = new DateTimePicker();
            lblPersAralikAyrac = new Label();
            dtpPersFiltreBit = new DateTimePicker();
            btnPersFiltrele = new Button();
            btnPersExcel = new Button();
            lblPersSonuc = new Label();
            tabPageDepRapor = new TabPage();
            lblDepRaporBaslik = new Label();
            btnDepRaporYenile = new Button();
            btnExcelAktar = new Button();
            lblPersonelFiltre = new Label();
            txtPersonelFiltre = new TextBox();
            lblDurumFiltre = new Label();
            cmbDurumFiltre = new ComboBox();
            lblFiltreBas = new Label();
            dtpFiltreBas = new DateTimePicker();
            lblFiltreAyrac = new Label();
            dtpFiltreEnd = new DateTimePicker();
            btnFiltrele = new Button();
            btnTemizle = new Button();
            dgvDepRapor = new DataGridView();
            tabPageDepLog = new TabPage();
            lblDepLogBaslik = new Label();
            btnDepLogYenile = new Button();
            lblLogBas = new Label();
            dtpLogBas = new DateTimePicker();
            lblLogBit = new Label();
            dtpLogBit = new DateTimePicker();
            btnLogFiltrele = new Button();
            btnLogExcel = new Button();
            dgvDepLog = new DataGridView();
            tabPageAmirOnay = new TabPage();
            lblAmirOnayBaslik = new Label();
            btnAmirReddet = new Button();
            btnAmirOnayla = new Button();
            dgvAmirOnay = new DataGridView();
            pnlHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picLogoAlan).BeginInit();
            tabMain.SuspendLayout();
            tabPagePersonel.SuspendLayout();
            pnlSecilenPersonel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvPersoneller).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvIzinler).BeginInit();
            tabPageDepRapor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvDepRapor).BeginInit();
            tabPageDepLog.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvDepLog).BeginInit();
            tabPageAmirOnay.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvAmirOnay).BeginInit();
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
            pnlHeader.TabIndex = 20;
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
            lblPanelBaslik.Size = new Size(500, 34);
            lblPanelBaslik.TabIndex = 0;
            lblPanelBaslik.Text = "İzin Takip Sistemi  —  Amir Paneli";
            // 
            // tabMain
            // 
            tabMain.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tabMain.Controls.Add(tabPagePersonel);
            tabMain.Controls.Add(tabPageDepRapor);
            tabMain.Controls.Add(tabPageDepLog);
            tabMain.Controls.Add(tabPageAmirOnay);
            tabMain.Location = new Point(0, 58);
            tabMain.Name = "tabMain";
            tabMain.SelectedIndex = 0;
            tabMain.Size = new Size(1280, 742);
            tabMain.TabIndex = 40;
            // 
            // tabPagePersonel
            // 
            tabPagePersonel.Controls.Add(pnlSecilenPersonel);
            tabPagePersonel.Controls.Add(label1);
            tabPagePersonel.Controls.Add(txtArama);
            tabPagePersonel.Controls.Add(dgvPersoneller);
            tabPagePersonel.Controls.Add(label2);
            tabPagePersonel.Controls.Add(dgvIzinler);
            tabPagePersonel.Controls.Add(label3);
            tabPagePersonel.Controls.Add(dtpBaslangic);
            tabPagePersonel.Controls.Add(label4);
            tabPagePersonel.Controls.Add(dtpBitis);
            tabPagePersonel.Controls.Add(btnIzinSil);
            tabPagePersonel.Controls.Add(btnIzinEkle);
            tabPagePersonel.Controls.Add(btnIzinDuzenle);
            tabPagePersonel.Controls.Add(btnAvansIzinEkle);
            tabPagePersonel.Controls.Add(lblPersAralik);
            tabPagePersonel.Controls.Add(dtpPersFiltreBas);
            tabPagePersonel.Controls.Add(lblPersAralikAyrac);
            tabPagePersonel.Controls.Add(dtpPersFiltreBit);
            tabPagePersonel.Controls.Add(btnPersFiltrele);
            tabPagePersonel.Controls.Add(btnPersExcel);
            tabPagePersonel.Controls.Add(lblPersSonuc);
            tabPagePersonel.Location = new Point(4, 29);
            tabPagePersonel.Name = "tabPagePersonel";
            tabPagePersonel.Padding = new Padding(3);
            tabPagePersonel.Size = new Size(1272, 709);
            tabPagePersonel.TabIndex = 0;
            tabPagePersonel.Text = "  Personel  ";
            tabPagePersonel.UseVisualStyleBackColor = true;
            // 
            // pnlSecilenPersonel
            // 
            pnlSecilenPersonel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            pnlSecilenPersonel.BackColor = Color.FromArgb(230, 230, 230);
            pnlSecilenPersonel.BorderStyle = BorderStyle.FixedSingle;
            pnlSecilenPersonel.Controls.Add(lblSecilenPersonelBilgi);
            pnlSecilenPersonel.Location = new Point(12, 8);
            pnlSecilenPersonel.Name = "pnlSecilenPersonel";
            pnlSecilenPersonel.Size = new Size(934, 42);
            pnlSecilenPersonel.TabIndex = 31;
            // 
            // lblSecilenPersonelBilgi
            // 
            lblSecilenPersonelBilgi.Dock = DockStyle.Fill;
            lblSecilenPersonelBilgi.Font = new Font("Segoe UI", 9.5F);
            lblSecilenPersonelBilgi.ForeColor = Color.FromArgb(120, 120, 120);
            lblSecilenPersonelBilgi.Location = new Point(0, 0);
            lblSecilenPersonelBilgi.Name = "lblSecilenPersonelBilgi";
            lblSecilenPersonelBilgi.Size = new Size(932, 40);
            lblSecilenPersonelBilgi.TabIndex = 0;
            lblSecilenPersonelBilgi.Text = "  Listeden bir personel seçin...";
            lblSecilenPersonelBilgi.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label1.Location = new Point(12, 58);
            label1.Name = "label1";
            label1.Size = new Size(121, 20);
            label1.TabIndex = 6;
            label1.Text = "Personel Listesi:";
            // 
            // txtArama
            // 
            txtArama.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtArama.Font = new Font("Segoe UI", 9.5F);
            txtArama.Location = new Point(12, 78);
            txtArama.Name = "txtArama";
            txtArama.PlaceholderText = "🔍  Ad soyad veya sicil no ile arayın...";
            txtArama.Size = new Size(934, 29);
            txtArama.TabIndex = 30;
            txtArama.TextChanged += txtArama_TextChanged;
            // 
            // dgvPersoneller
            // 
            dgvPersoneller.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvPersoneller.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvPersoneller.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvPersoneller.Location = new Point(12, 150);
            dgvPersoneller.Name = "dgvPersoneller";
            dgvPersoneller.ReadOnly = true;
            dgvPersoneller.RowHeadersWidth = 51;
            dgvPersoneller.Size = new Size(934, 268);
            dgvPersoneller.TabIndex = 0;
            dgvPersoneller.SelectionChanged += dgvPersoneller_SelectionChanged;
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label2.Location = new Point(12, 430);
            label2.Name = "label2";
            label2.Size = new Size(97, 20);
            label2.TabIndex = 7;
            label2.Text = "İzin Geçmişi:";
            // 
            // dgvIzinler
            // 
            dgvIzinler.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvIzinler.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvIzinler.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvIzinler.Location = new Point(12, 452);
            dgvIzinler.Name = "dgvIzinler";
            dgvIzinler.RowHeadersWidth = 51;
            dgvIzinler.Size = new Size(934, 254);
            dgvIzinler.TabIndex = 1;
            dgvIzinler.SelectionChanged += dgvIzinler_SelectionChanged;
            // 
            // label3
            // 
            label3.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label3.AutoSize = true;
            label3.Location = new Point(963, 10);
            label3.Name = "label3";
            label3.Size = new Size(141, 20);
            label3.TabIndex = 8;
            label3.Text = "İzin Başlangıç Tarihi:";
            // 
            // dtpBaslangic
            // 
            dtpBaslangic.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            dtpBaslangic.Location = new Point(963, 32);
            dtpBaslangic.Name = "dtpBaslangic";
            dtpBaslangic.Size = new Size(305, 27);
            dtpBaslangic.TabIndex = 2;
            // 
            // label4
            // 
            label4.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label4.AutoSize = true;
            label4.Location = new Point(963, 74);
            label4.Name = "label4";
            label4.Size = new Size(106, 20);
            label4.TabIndex = 9;
            label4.Text = "İzin Bitiş Tarihi:";
            // 
            // dtpBitis
            // 
            dtpBitis.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            dtpBitis.Location = new Point(963, 96);
            dtpBitis.Name = "dtpBitis";
            dtpBitis.Size = new Size(305, 27);
            dtpBitis.TabIndex = 3;
            // 
            // btnIzinSil
            // 
            btnIzinSil.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnIzinSil.Location = new Point(963, 142);
            btnIzinSil.Name = "btnIzinSil";
            btnIzinSil.Size = new Size(145, 34);
            btnIzinSil.TabIndex = 5;
            btnIzinSil.Text = "İzin SİL";
            btnIzinSil.UseVisualStyleBackColor = true;
            btnIzinSil.Click += btnIzinSil_Click;
            // 
            // btnIzinEkle
            // 
            btnIzinEkle.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnIzinEkle.Location = new Point(1122, 142);
            btnIzinEkle.Name = "btnIzinEkle";
            btnIzinEkle.Size = new Size(146, 34);
            btnIzinEkle.TabIndex = 4;
            btnIzinEkle.Text = "İzin EKLE";
            btnIzinEkle.UseVisualStyleBackColor = true;
            btnIzinEkle.Click += btnIzinEkle_Click;
            // 
            // btnIzinDuzenle
            // 
            btnIzinDuzenle.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnIzinDuzenle.Location = new Point(1010, 192);
            btnIzinDuzenle.Name = "btnIzinDuzenle";
            btnIzinDuzenle.Size = new Size(160, 34);
            btnIzinDuzenle.TabIndex = 10;
            btnIzinDuzenle.Text = "İzin Düzenle";
            btnIzinDuzenle.UseVisualStyleBackColor = true;
            btnIzinDuzenle.Click += btnIzinDuzenle_Click;
            // 
            // btnAvansIzinEkle
            // 
            btnAvansIzinEkle.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnAvansIzinEkle.BackColor = Color.Purple;
            btnAvansIzinEkle.FlatStyle = FlatStyle.Flat;
            btnAvansIzinEkle.ForeColor = Color.White;
            btnAvansIzinEkle.Location = new Point(963, 236);
            btnAvansIzinEkle.Name = "btnAvansIzinEkle";
            btnAvansIzinEkle.Size = new Size(305, 34);
            btnAvansIzinEkle.TabIndex = 11;
            btnAvansIzinEkle.Text = "Avans İzin Ekle";
            btnAvansIzinEkle.UseVisualStyleBackColor = false;
            btnAvansIzinEkle.Visible = false;
            btnAvansIzinEkle.Click += btnAvansIzinEkle_Click;
            // 
            // lblPersAralik
            // 
            lblPersAralik.AutoSize = true;
            lblPersAralik.Font = new Font("Segoe UI", 9F);
            lblPersAralik.Location = new Point(19, 117);
            lblPersAralik.Name = "lblPersAralik";
            lblPersAralik.Size = new Size(50, 20);
            lblPersAralik.TabIndex = 36;
            lblPersAralik.Text = "Aralık:";
            // 
            // dtpPersFiltreBas
            // 
            dtpPersFiltreBas.Checked = false;
            dtpPersFiltreBas.Format = DateTimePickerFormat.Short;
            dtpPersFiltreBas.Location = new Point(75, 112);
            dtpPersFiltreBas.Name = "dtpPersFiltreBas";
            dtpPersFiltreBas.ShowCheckBox = true;
            dtpPersFiltreBas.Size = new Size(155, 27);
            dtpPersFiltreBas.TabIndex = 32;
            // 
            // lblPersAralikAyrac
            // 
            lblPersAralikAyrac.AutoSize = true;
            lblPersAralikAyrac.Location = new Point(235, 117);
            lblPersAralikAyrac.Name = "lblPersAralikAyrac";
            lblPersAralikAyrac.Size = new Size(17, 20);
            lblPersAralikAyrac.TabIndex = 37;
            lblPersAralikAyrac.Text = "–";
            // 
            // dtpPersFiltreBit
            // 
            dtpPersFiltreBit.Checked = false;
            dtpPersFiltreBit.Format = DateTimePickerFormat.Short;
            dtpPersFiltreBit.Location = new Point(258, 112);
            dtpPersFiltreBit.Name = "dtpPersFiltreBit";
            dtpPersFiltreBit.ShowCheckBox = true;
            dtpPersFiltreBit.Size = new Size(155, 27);
            dtpPersFiltreBit.TabIndex = 33;
            // 
            // btnPersFiltrele
            // 
            btnPersFiltrele.BackColor = Color.FromArgb(31, 73, 125);
            btnPersFiltrele.FlatAppearance.BorderColor = Color.FromArgb(50, 100, 160);
            btnPersFiltrele.FlatStyle = FlatStyle.Flat;
            btnPersFiltrele.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            btnPersFiltrele.ForeColor = Color.White;
            btnPersFiltrele.Location = new Point(438, 112);
            btnPersFiltrele.Name = "btnPersFiltrele";
            btnPersFiltrele.Size = new Size(110, 29);
            btnPersFiltrele.TabIndex = 34;
            btnPersFiltrele.Text = "🔍  Filtrele";
            btnPersFiltrele.UseVisualStyleBackColor = false;
            btnPersFiltrele.Click += btnPersFiltrele_Click;
            // 
            // btnPersExcel
            // 
            btnPersExcel.BackColor = Color.FromArgb(33, 115, 70);
            btnPersExcel.FlatAppearance.BorderColor = Color.FromArgb(60, 140, 90);
            btnPersExcel.FlatStyle = FlatStyle.Flat;
            btnPersExcel.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            btnPersExcel.ForeColor = Color.White;
            btnPersExcel.Location = new Point(558, 112);
            btnPersExcel.Name = "btnPersExcel";
            btnPersExcel.Size = new Size(120, 29);
            btnPersExcel.TabIndex = 38;
            btnPersExcel.Text = "📊  Excel'e Aktar";
            btnPersExcel.UseVisualStyleBackColor = false;
            btnPersExcel.Click += btnPersExcel_Click;
            // 
            // lblPersSonuc
            // 
            lblPersSonuc.AutoSize = true;
            lblPersSonuc.Font = new Font("Segoe UI", 8.5F, FontStyle.Italic);
            lblPersSonuc.ForeColor = Color.FromArgb(80, 80, 80);
            lblPersSonuc.Location = new Point(688, 117);
            lblPersSonuc.Name = "lblPersSonuc";
            lblPersSonuc.Size = new Size(0, 20);
            lblPersSonuc.TabIndex = 35;
            // 
            // tabPageDepRapor
            // 
            tabPageDepRapor.Controls.Add(lblDepRaporBaslik);
            tabPageDepRapor.Controls.Add(btnDepRaporYenile);
            tabPageDepRapor.Controls.Add(btnExcelAktar);
            tabPageDepRapor.Controls.Add(lblPersonelFiltre);
            tabPageDepRapor.Controls.Add(txtPersonelFiltre);
            tabPageDepRapor.Controls.Add(lblDurumFiltre);
            tabPageDepRapor.Controls.Add(cmbDurumFiltre);
            tabPageDepRapor.Controls.Add(lblFiltreBas);
            tabPageDepRapor.Controls.Add(dtpFiltreBas);
            tabPageDepRapor.Controls.Add(lblFiltreAyrac);
            tabPageDepRapor.Controls.Add(dtpFiltreEnd);
            tabPageDepRapor.Controls.Add(btnFiltrele);
            tabPageDepRapor.Controls.Add(btnTemizle);
            tabPageDepRapor.Controls.Add(dgvDepRapor);
            tabPageDepRapor.Location = new Point(4, 29);
            tabPageDepRapor.Name = "tabPageDepRapor";
            tabPageDepRapor.Padding = new Padding(3);
            tabPageDepRapor.Size = new Size(1272, 709);
            tabPageDepRapor.TabIndex = 1;
            tabPageDepRapor.Text = "  Departman Raporu  ";
            tabPageDepRapor.UseVisualStyleBackColor = true;
            // 
            // lblDepRaporBaslik
            // 
            lblDepRaporBaslik.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblDepRaporBaslik.ForeColor = Color.FromArgb(31, 73, 125);
            lblDepRaporBaslik.Location = new Point(12, 14);
            lblDepRaporBaslik.Name = "lblDepRaporBaslik";
            lblDepRaporBaslik.Size = new Size(600, 26);
            lblDepRaporBaslik.TabIndex = 0;
            lblDepRaporBaslik.Text = "Bugün Departmanda İzinli Olanlar";
            // 
            // btnDepRaporYenile
            // 
            btnDepRaporYenile.Location = new Point(630, 12);
            btnDepRaporYenile.Name = "btnDepRaporYenile";
            btnDepRaporYenile.Size = new Size(120, 30);
            btnDepRaporYenile.TabIndex = 1;
            btnDepRaporYenile.Text = "↺  Yenile";
            btnDepRaporYenile.UseVisualStyleBackColor = true;
            btnDepRaporYenile.Click += btnDepRaporYenile_Click;
            // 
            // btnExcelAktar
            // 
            btnExcelAktar.Location = new Point(762, 12);
            btnExcelAktar.Name = "btnExcelAktar";
            btnExcelAktar.Size = new Size(150, 30);
            btnExcelAktar.TabIndex = 3;
            btnExcelAktar.Text = "📊  Excel'e Aktar";
            btnExcelAktar.UseVisualStyleBackColor = true;
            btnExcelAktar.Click += btnExcelAktar_Click;
            // 
            // lblPersonelFiltre
            // 
            lblPersonelFiltre.AutoSize = true;
            lblPersonelFiltre.Location = new Point(12, 52);
            lblPersonelFiltre.Name = "lblPersonelFiltre";
            lblPersonelFiltre.Size = new Size(67, 20);
            lblPersonelFiltre.TabIndex = 10;
            lblPersonelFiltre.Text = "Personel:";
            // 
            // txtPersonelFiltre
            // 
            txtPersonelFiltre.Location = new Point(81, 48);
            txtPersonelFiltre.Name = "txtPersonelFiltre";
            txtPersonelFiltre.PlaceholderText = "Ad soyad...";
            txtPersonelFiltre.Size = new Size(155, 27);
            txtPersonelFiltre.TabIndex = 11;
            // 
            // lblDurumFiltre
            // 
            lblDurumFiltre.AutoSize = true;
            lblDurumFiltre.Location = new Point(242, 52);
            lblDurumFiltre.Name = "lblDurumFiltre";
            lblDurumFiltre.Size = new Size(57, 20);
            lblDurumFiltre.TabIndex = 12;
            lblDurumFiltre.Text = "Durum:";
            // 
            // cmbDurumFiltre
            // 
            cmbDurumFiltre.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbDurumFiltre.FormattingEnabled = true;
            cmbDurumFiltre.Items.AddRange(new object[] { "Tümü", "Beklemede", "Onaylandi", "Reddedildi", "Iptal Edildi" });
            cmbDurumFiltre.Location = new Point(305, 48);
            cmbDurumFiltre.Name = "cmbDurumFiltre";
            cmbDurumFiltre.Size = new Size(135, 28);
            cmbDurumFiltre.TabIndex = 13;
            // 
            // lblFiltreBas
            // 
            lblFiltreBas.AutoSize = true;
            lblFiltreBas.Location = new Point(460, 55);
            lblFiltreBas.Name = "lblFiltreBas";
            lblFiltreBas.Size = new Size(75, 20);
            lblFiltreBas.TabIndex = 14;
            lblFiltreBas.Text = "Başlangıç:";
            // 
            // dtpFiltreBas
            // 
            dtpFiltreBas.Checked = false;
            dtpFiltreBas.Format = DateTimePickerFormat.Short;
            dtpFiltreBas.Location = new Point(541, 50);
            dtpFiltreBas.Name = "dtpFiltreBas";
            dtpFiltreBas.ShowCheckBox = true;
            dtpFiltreBas.Size = new Size(140, 27);
            dtpFiltreBas.TabIndex = 15;
            // 
            // lblFiltreAyrac
            // 
            lblFiltreAyrac.AutoSize = true;
            lblFiltreAyrac.Location = new Point(687, 56);
            lblFiltreAyrac.Name = "lblFiltreAyrac";
            lblFiltreAyrac.Size = new Size(17, 20);
            lblFiltreAyrac.TabIndex = 16;
            lblFiltreAyrac.Text = "–";
            // 
            // dtpFiltreEnd
            // 
            dtpFiltreEnd.Checked = false;
            dtpFiltreEnd.Format = DateTimePickerFormat.Short;
            dtpFiltreEnd.Location = new Point(710, 50);
            dtpFiltreEnd.Name = "dtpFiltreEnd";
            dtpFiltreEnd.ShowCheckBox = true;
            dtpFiltreEnd.Size = new Size(140, 27);
            dtpFiltreEnd.TabIndex = 17;
            // 
            // btnFiltrele
            // 
            btnFiltrele.BackColor = Color.FromArgb(31, 73, 125);
            btnFiltrele.FlatAppearance.BorderColor = Color.FromArgb(50, 100, 160);
            btnFiltrele.FlatStyle = FlatStyle.Flat;
            btnFiltrele.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            btnFiltrele.ForeColor = Color.White;
            btnFiltrele.Location = new Point(856, 46);
            btnFiltrele.Name = "btnFiltrele";
            btnFiltrele.Size = new Size(85, 30);
            btnFiltrele.TabIndex = 18;
            btnFiltrele.Text = "🔍  Filtrele";
            btnFiltrele.UseVisualStyleBackColor = false;
            btnFiltrele.Click += btnFiltrele_Click;
            // 
            // btnTemizle
            // 
            btnTemizle.Location = new Point(956, 45);
            btnTemizle.Name = "btnTemizle";
            btnTemizle.Size = new Size(82, 30);
            btnTemizle.TabIndex = 19;
            btnTemizle.Text = "✖  Temizle";
            btnTemizle.UseVisualStyleBackColor = true;
            btnTemizle.Click += btnTemizle_Click;
            // 
            // dgvDepRapor
            // 
            dgvDepRapor.AllowUserToAddRows = false;
            dgvDepRapor.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvDepRapor.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvDepRapor.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvDepRapor.Location = new Point(12, 88);
            dgvDepRapor.Name = "dgvDepRapor";
            dgvDepRapor.ReadOnly = true;
            dgvDepRapor.RowHeadersWidth = 30;
            dgvDepRapor.Size = new Size(1254, 589);
            dgvDepRapor.TabIndex = 2;
            // 
            // tabPageDepLog
            // 
            tabPageDepLog.Controls.Add(lblDepLogBaslik);
            tabPageDepLog.Controls.Add(btnDepLogYenile);
            tabPageDepLog.Controls.Add(lblLogBas);
            tabPageDepLog.Controls.Add(dtpLogBas);
            tabPageDepLog.Controls.Add(lblLogBit);
            tabPageDepLog.Controls.Add(dtpLogBit);
            tabPageDepLog.Controls.Add(btnLogFiltrele);
            tabPageDepLog.Controls.Add(btnLogExcel);
            tabPageDepLog.Controls.Add(dgvDepLog);
            tabPageDepLog.Location = new Point(4, 29);
            tabPageDepLog.Name = "tabPageDepLog";
            tabPageDepLog.Padding = new Padding(3);
            tabPageDepLog.Size = new Size(1272, 709);
            tabPageDepLog.TabIndex = 2;
            tabPageDepLog.Text = "  İşlem Geçmişi  ";
            tabPageDepLog.UseVisualStyleBackColor = true;
            // 
            // lblDepLogBaslik
            // 
            lblDepLogBaslik.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblDepLogBaslik.ForeColor = Color.FromArgb(31, 73, 125);
            lblDepLogBaslik.Location = new Point(12, 14);
            lblDepLogBaslik.Name = "lblDepLogBaslik";
            lblDepLogBaslik.Size = new Size(600, 26);
            lblDepLogBaslik.TabIndex = 0;
            lblDepLogBaslik.Text = "Departman İşlem Geçmişi";
            // 
            // btnDepLogYenile
            // 
            btnDepLogYenile.Location = new Point(630, 12);
            btnDepLogYenile.Name = "btnDepLogYenile";
            btnDepLogYenile.Size = new Size(120, 30);
            btnDepLogYenile.TabIndex = 1;
            btnDepLogYenile.Text = "↺  Yenile";
            btnDepLogYenile.UseVisualStyleBackColor = true;
            btnDepLogYenile.Click += btnDepLogYenile_Click;
            // 
            // lblLogBas
            // 
            lblLogBas.AutoSize = true;
            lblLogBas.Font = new Font("Segoe UI", 9F);
            lblLogBas.Location = new Point(12, 55);
            lblLogBas.Name = "lblLogBas";
            lblLogBas.Size = new Size(75, 20);
            lblLogBas.TabIndex = 2;
            lblLogBas.Text = "Başlangıç:";
            // 
            // dtpLogBas
            // 
            dtpLogBas.Checked = false;
            dtpLogBas.Format = DateTimePickerFormat.Short;
            dtpLogBas.Location = new Point(90, 51);
            dtpLogBas.Name = "dtpLogBas";
            dtpLogBas.ShowCheckBox = true;
            dtpLogBas.Size = new Size(155, 27);
            dtpLogBas.TabIndex = 3;
            // 
            // lblLogBit
            // 
            lblLogBit.AutoSize = true;
            lblLogBit.Font = new Font("Segoe UI", 9F);
            lblLogBit.Location = new Point(260, 55);
            lblLogBit.Name = "lblLogBit";
            lblLogBit.Size = new Size(40, 20);
            lblLogBit.TabIndex = 4;
            lblLogBit.Text = "Bitiş:";
            // 
            // dtpLogBit
            // 
            dtpLogBit.Checked = false;
            dtpLogBit.Format = DateTimePickerFormat.Short;
            dtpLogBit.Location = new Point(305, 51);
            dtpLogBit.Name = "dtpLogBit";
            dtpLogBit.ShowCheckBox = true;
            dtpLogBit.Size = new Size(155, 27);
            dtpLogBit.TabIndex = 4;
            // 
            // btnLogFiltrele
            // 
            btnLogFiltrele.BackColor = Color.FromArgb(46, 117, 182);
            btnLogFiltrele.FlatStyle = FlatStyle.Flat;
            btnLogFiltrele.ForeColor = Color.White;
            btnLogFiltrele.Location = new Point(475, 50);
            btnLogFiltrele.Name = "btnLogFiltrele";
            btnLogFiltrele.Size = new Size(100, 28);
            btnLogFiltrele.TabIndex = 5;
            btnLogFiltrele.Text = "Filtrele";
            btnLogFiltrele.UseVisualStyleBackColor = false;
            btnLogFiltrele.Click += btnLogFiltrele_Click;
            // 
            // btnLogExcel
            // 
            btnLogExcel.BackColor = Color.FromArgb(33, 115, 70);
            btnLogExcel.FlatStyle = FlatStyle.Flat;
            btnLogExcel.ForeColor = Color.White;
            btnLogExcel.Location = new Point(585, 50);
            btnLogExcel.Name = "btnLogExcel";
            btnLogExcel.Size = new Size(130, 28);
            btnLogExcel.TabIndex = 6;
            btnLogExcel.Text = "Excel'e Aktar";
            btnLogExcel.UseVisualStyleBackColor = false;
            btnLogExcel.Click += btnLogExcel_Click;
            // 
            // dgvDepLog
            // 
            dgvDepLog.AllowUserToAddRows = false;
            dgvDepLog.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvDepLog.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvDepLog.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvDepLog.Location = new Point(12, 90);
            dgvDepLog.Name = "dgvDepLog";
            dgvDepLog.ReadOnly = true;
            dgvDepLog.RowHeadersWidth = 30;
            dgvDepLog.Size = new Size(1254, 612);
            dgvDepLog.TabIndex = 2;
            // 
            // tabPageAmirOnay
            // 
            tabPageAmirOnay.Controls.Add(lblAmirOnayBaslik);
            tabPageAmirOnay.Controls.Add(btnAmirReddet);
            tabPageAmirOnay.Controls.Add(btnAmirOnayla);
            tabPageAmirOnay.Controls.Add(dgvAmirOnay);
            tabPageAmirOnay.Location = new Point(4, 29);
            tabPageAmirOnay.Name = "tabPageAmirOnay";
            tabPageAmirOnay.Padding = new Padding(3);
            tabPageAmirOnay.Size = new Size(1272, 709);
            tabPageAmirOnay.TabIndex = 3;
            tabPageAmirOnay.Text = "Amir Onayı";
            tabPageAmirOnay.UseVisualStyleBackColor = true;
            // 
            // lblAmirOnayBaslik
            // 
            lblAmirOnayBaslik.AutoSize = true;
            lblAmirOnayBaslik.Location = new Point(222, 26);
            lblAmirOnayBaslik.Name = "lblAmirOnayBaslik";
            lblAmirOnayBaslik.Size = new Size(190, 20);
            lblAmirOnayBaslik.TabIndex = 3;
            lblAmirOnayBaslik.Text = "AmirOnayıBekleyenTalepler";
            // 
            // btnAmirReddet
            // 
            btnAmirReddet.Location = new Point(748, 142);
            btnAmirReddet.Name = "btnAmirReddet";
            btnAmirReddet.Size = new Size(94, 29);
            btnAmirReddet.TabIndex = 2;
            btnAmirReddet.Text = "Reddet";
            btnAmirReddet.UseVisualStyleBackColor = true;
            btnAmirReddet.Click += btnAmirReddet_Click_1;
            // 
            // btnAmirOnayla
            // 
            btnAmirOnayla.Location = new Point(748, 91);
            btnAmirOnayla.Name = "btnAmirOnayla";
            btnAmirOnayla.Size = new Size(94, 29);
            btnAmirOnayla.TabIndex = 1;
            btnAmirOnayla.Text = "Onayla";
            btnAmirOnayla.UseVisualStyleBackColor = true;
            btnAmirOnayla.Click += btnAmirOnayla_Click_1;
            // 
            // dgvAmirOnay
            // 
            dgvAmirOnay.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvAmirOnay.Location = new Point(10, 69);
            dgvAmirOnay.Name = "dgvAmirOnay";
            dgvAmirOnay.RowHeadersWidth = 51;
            dgvAmirOnay.Size = new Size(687, 632);
            dgvAmirOnay.TabIndex = 0;
            // 
            // AmirForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1280, 800);
            Controls.Add(tabMain);
            Controls.Add(pnlHeader);
            MinimumSize = new Size(1100, 700);
            Name = "AmirForm";
            Text = "Amir Paneli";
            Load += AmirForm_Load;
            pnlHeader.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)picLogoAlan).EndInit();
            tabMain.ResumeLayout(false);
            tabPagePersonel.ResumeLayout(false);
            tabPagePersonel.PerformLayout();
            pnlSecilenPersonel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvPersoneller).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvIzinler).EndInit();
            tabPageDepRapor.ResumeLayout(false);
            tabPageDepRapor.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvDepRapor).EndInit();
            tabPageDepLog.ResumeLayout(false);
            tabPageDepLog.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvDepLog).EndInit();
            tabPageAmirOnay.ResumeLayout(false);
            tabPageAmirOnay.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvAmirOnay).EndInit();
            ResumeLayout(false);
        }

        #endregion

        // Header
        private Panel pnlHeader;
        private Label lblPanelBaslik;
        private PictureBox picLogoAlan;
        private Button btnCikis;
        // Tabs
        private TabControl tabMain;
        private TabPage tabPagePersonel;
        private TabPage tabPageDepRapor;
        private TabPage tabPageDepLog;
        // Tab1 — personel
        private Panel pnlSecilenPersonel;
        private Label lblSecilenPersonelBilgi;
        private TextBox txtArama;
        private DataGridView dgvPersoneller;
        private DataGridView dgvIzinler;
        private DateTimePicker dtpBaslangic;
        private DateTimePicker dtpBitis;
        private Button btnIzinEkle;
        private Button btnIzinSil;
        private Button btnAvansIzinEkle;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Button btnIzinDuzenle;
        private Label lblPersAralik;
        private DateTimePicker dtpPersFiltreBas;
        private Label lblPersAralikAyrac;
        private DateTimePicker dtpPersFiltreBit;
        private Button btnPersFiltrele;
        private Button btnPersExcel;
        private Label lblPersSonuc;
        // Tab2 — departman raporu
        private Label lblDepRaporBaslik;
        private Button btnDepRaporYenile;
        private Button btnExcelAktar;
        private DataGridView dgvDepRapor;
        private Label lblPersonelFiltre;
        private TextBox txtPersonelFiltre;
        private Label lblDurumFiltre;
        private ComboBox cmbDurumFiltre;
        private Label lblFiltreBas;
        private DateTimePicker dtpFiltreBas;
        private Label lblFiltreAyrac;
        private DateTimePicker dtpFiltreEnd;
        private Button btnFiltrele;
        private Button btnTemizle;
        // Tab3 — işlem geçmişi
        private Label lblDepLogBaslik;
        private Button btnDepLogYenile;
        private Label lblLogBas;
        private DateTimePicker dtpLogBas;
        private Label lblLogBit;
        private DateTimePicker dtpLogBit;
        private Button btnLogFiltrele;
        private Button btnLogExcel;
        private DataGridView dgvDepLog;
        private TabPage tabPageAmirOnay;
        private Label lblAmirOnayBaslik;
        private Button btnAmirReddet;
        private Button btnAmirOnayla;
        private DataGridView dgvAmirOnay;
    }
}

