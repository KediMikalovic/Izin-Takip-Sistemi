using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ClosedXML.Excel;
using IzinTakip.Core.Abstractions;
using IzinTakip.Core.Helpers;
using IzinTakip.Core.Models;

namespace IzinTakipSistemi
{
    public partial class YoneticiForm : Form
    {
        private readonly IIzinService _izinService;
        private readonly IRaporService _raporService;
        private bool _isLoggingOut = false;

        // Designer'da tanımlı sabit (yerleşik) red şablonlarının sayısı.
        // Bu indeksin altındakiler kalıcıdır, üzerindekiler kullanıcı tarafından
        // eklenmiş özel şablonlardır ve sağ tıkla silinebilir.
        private int _yerlesikSablonSayisi;
        private ContextMenuStrip? _redSablonMenu;
        private ToolStripMenuItem? _miSablonSil;

        public YoneticiForm(IIzinService izinService, IRaporService raporService)
        {
            _izinService = izinService;
            _raporService = raporService;
            InitializeComponent();
            this.FormClosed += YoneticiForm_FormClosed;
            dgvBekleyenIzinler.CellFormatting += dgvBekleyenIzinler_CellFormatting;
            tabMain.SelectedIndexChanged += tabMain_SelectedIndexChanged;
        }

        // X butonu ile kapatılırsa uygulamayı sonlandır
        private void YoneticiForm_FormClosed(object? sender, FormClosedEventArgs e)
        {
            if (!_isLoggingOut)
                Application.Exit();
        }

        // ── İzin listesi renk kodlaması (durum ve tip bazlı) ──────
        private void dgvBekleyenIzinler_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var row = dgvBekleyenIzinler.Rows[e.RowIndex];
            if (!dgvBekleyenIzinler.Columns.Contains("Status") || row.Cells["Status"].Value == null) return;

            // Avans izin — mor renk önceliklidir
            if (dgvBekleyenIzinler.Columns.Contains("LeaveType") &&
                row.Cells["LeaveType"].Value?.ToString() == "Avans")
            {
                row.DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(225, 190, 231);
                row.DefaultCellStyle.ForeColor = System.Drawing.Color.FromArgb(106, 27, 154);
                return;
            }

            string durum = row.Cells["Status"].Value.ToString() ?? "";
            (System.Drawing.Color bg, System.Drawing.Color fg) = durum switch
            {
                "Beklemede"    => (System.Drawing.Color.FromArgb(255, 249, 196), System.Drawing.Color.FromArgb(130, 100, 0)),
                "Onaylandi"    => (System.Drawing.Color.FromArgb(200, 230, 201), System.Drawing.Color.FromArgb(27, 94, 32)),
                "Reddedildi"   => (System.Drawing.Color.FromArgb(255, 205, 210), System.Drawing.Color.FromArgb(183, 28, 28)),
                "Iptal Edildi" => (System.Drawing.Color.FromArgb(236, 239, 241), System.Drawing.Color.FromArgb(96, 125, 139)),
                _              => (System.Drawing.Color.White, System.Drawing.Color.Black)
            };
            row.DefaultCellStyle.BackColor = bg;
            row.DefaultCellStyle.ForeColor = fg;
        }

        // Satır seçiminde buton durumlarını ve bilgi panelini güncelle
        private void dgvBekleyenIzinler_SelectionChanged(object? sender, EventArgs e)
        {
            ButonDurumlariniGuncelle();
            SecilenIzinPaneliniGuncelle(
                dgvBekleyenIzinler.SelectedRows.Count > 0 ? dgvBekleyenIzinler.SelectedRows[0] : null);
        }

        // ── Seçilen izin talebi bilgi panelini güncelle ───────────
        private void SecilenIzinPaneliniGuncelle(DataGridViewRow? row)
        {
            if (row == null)
            {
                pnlSecilenIzin.BackColor = Color.FromArgb(230, 230, 230);
                lblSecilenIzinBilgi.ForeColor = Color.FromArgb(120, 120, 120);
                lblSecilenIzinBilgi.Font = new Font("Segoe UI", 9.5F);
                lblSecilenIzinBilgi.Text = "  Listeden bir izin talebi seçin...";
                return;
            }

            string ad       = row.Cells["PersonelAd"].Value?.ToString()  ?? "-";
            string dept     = row.Cells["Departman"].Value?.ToString()   ?? "-";
            string baslangic = dgvBekleyenIzinler.Columns.Contains("StartDate") && row.Cells["StartDate"].Value != null
                ? Convert.ToDateTime(row.Cells["StartDate"].Value).ToString("dd.MM.yyyy") : "-";
            string bitis    = dgvBekleyenIzinler.Columns.Contains("EndDate") && row.Cells["EndDate"].Value != null
                ? Convert.ToDateTime(row.Cells["EndDate"].Value).ToString("dd.MM.yyyy") : "-";
            string gun      = dgvBekleyenIzinler.Columns.Contains("DayCount") ? row.Cells["DayCount"].Value?.ToString() ?? "-" : "-";
            string durum    = dgvBekleyenIzinler.Columns.Contains("Status")   ? row.Cells["Status"].Value?.ToString()   ?? "-" : "-";

            (Color panelRenk, Color yaziRenk) = durum switch
            {
                "Beklemede"    => (Color.FromArgb(255, 248, 225), Color.FromArgb(130, 100,  0)),
                "Onaylandi"    => (Color.FromArgb(220, 242, 220), Color.FromArgb( 27,  94, 32)),
                "Reddedildi"   => (Color.FromArgb(255, 235, 238), Color.FromArgb(183,  28, 28)),
                "Iptal Edildi" => (Color.FromArgb(236, 239, 241), Color.FromArgb( 96, 125,139)),
                _              => (Color.FromArgb(213, 232, 252), Color.FromArgb( 31,  73,125))
            };

            pnlSecilenIzin.BackColor = panelRenk;
            lblSecilenIzinBilgi.ForeColor = yaziRenk;
            lblSecilenIzinBilgi.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lblSecilenIzinBilgi.Text =
                $"  📋  {ad}   |   {dept}   |   {baslangic} – {bitis}   |   {gun} iş günü   |   {durum}";
        }

        private void btnCikis_Click(object? sender, EventArgs e)
        {
            var onay = MessageBox.Show(
                "Oturumu kapatıp giriş ekranına dönmek istiyor musunuz?",
                "Çıkış", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (onay != DialogResult.Yes) return;

            _isLoggingOut = true;
            Session.Clear();

            foreach (Form f in Application.OpenForms)
            {
                if (f is LoginForm loginForm)
                {
                    loginForm.Show();
                    loginForm.BringToFront();
                    break;
                }
            }

            this.Close();
        }

        private async void YoneticiForm_Load(object? sender, EventArgs e)
        {
            this.Text = $"Yönetici Paneli - {Session.CurrentFullName} | Sicil No: {Session.CurrentSicilNo}";

            // Logo ve ikon
            try
            {
                string iconPath = Path.Combine(AppContext.BaseDirectory, "BossaIconFormat.ico");
                if (File.Exists(iconPath))
                    this.Icon = new System.Drawing.Icon(iconPath);
            }
            catch { /* İkon yüklenemezse varsayılan kullan */ }

            picLogoAlan.Image = Properties.Resources.görsel1;
            picLogoAlan.BackColor = System.Drawing.Color.White;

            // 1. Durum filtresi
            cmbFiltre.Items.Clear();
            cmbFiltre.Items.AddRange(new string[] { "Beklemede", "Onaylandi", "Reddedildi", "Iptal Edildi", "Tumu" });
            this.cmbFiltre.SelectedIndexChanged -= cmbFiltre_SelectedIndexChanged;
            this.cmbFiltre.SelectedIndexChanged += cmbFiltre_SelectedIndexChanged;

            // 2. Departman filtresi — servis üzerinden yükle
            await DepartmanlariYukleAsync();

            // 3. Varsayılan seçimler ve buton durumları
            cmbFiltre.SelectedIndex = 0;

            // Red sebebi şablonları: önce yerleşik sayısını kaydet, sonra
            // diskten yüklenen özel şablonları ekle ve sağ tık menüsünü kur.
            _yerlesikSablonSayisi = cmbRedSebebi.Items.Count;
            OzelRedSablonlariniYukle();
            RedSablonContextMenuKur();

            if (cmbRedSebebi.Items.Count > 0)
                cmbRedSebebi.SelectedIndex = 0;
            ButonDurumlariniGuncelle();

            // 4. İlk yükleme — beklemede olanları getir
            await FiltreyiUygulaAsync();
        }

        // ── Departman ComboBox (servis katmanından) ───────────────
        private async Task DepartmanlariYukleAsync()
        {
            try
            {
                DataTable dt = await _raporService.DepartmanlariGetirAsync();

                // "Tüm Departmanlar" satırını başa ekle
                DataRow tumRow = dt.NewRow();
                tumRow["DepartmentID"] = 0;
                tumRow["DepartmentName"] = "Tüm Departmanlar";
                dt.Rows.InsertAt(tumRow, 0);

                // DataTable'ı ComboBox'a bağla
                cmbDepartman.DataSource = dt;
                cmbDepartman.DisplayMember = "DepartmentName";
                cmbDepartman.ValueMember = "DepartmentID";
                cmbDepartman.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Departmanlar yüklenemedi: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void cmbDepartman_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (cmbFiltre.SelectedItem != null)
                await FiltreyiUygulaAsync();
        }

        private async void btnAra_Click(object? sender, EventArgs e)
        {
            await FiltreyiUygulaAsync();
        }

        private async Task FiltreyiUygulaAsync()
        {
            string durum = cmbFiltre.SelectedItem?.ToString() ?? "Tumu";
            int departmanId = cmbDepartman.SelectedValue != null ? Convert.ToInt32(cmbDepartman.SelectedValue) : 0;
            string personelAdi = txtPersonelAra.Text.Trim();
            DateTime? talepBas = dtpTalepBas.Checked ? dtpTalepBas.Value.Date : (DateTime?)null;
            DateTime? talepBit = dtpTalepBit.Checked ? dtpTalepBit.Value.Date : (DateTime?)null;
            await TalepleriGetirAsync(durum, departmanId, personelAdi, talepBas, talepBit);
        }

        // Durum filtresi değiştiğinde
        private async void cmbFiltre_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (cmbFiltre.SelectedItem != null)
            {
                await FiltreyiUygulaAsync();
                ButonDurumlariniGuncelle();
            }
        }

        private void ButonDurumlariniGuncelle()
        {
            // Buton durumu filtre seçimine değil, seçili satırın gerçek statüsüne göre belirlenir
            string statu = dgvBekleyenIzinler.SelectedRows.Count > 0
                ? dgvBekleyenIzinler.SelectedRows[0].Cells["Status"].Value?.ToString() ?? ""
                : "";

            bool seciliVar     = !string.IsNullOrEmpty(statu);
            bool beklemedeMi   = statu == "Beklemede";
            bool iptalEdildiMi = statu == "Iptal Edildi";
            bool reddedildiMi  = statu == "Reddedildi";

            btnOnayla.Enabled        = beklemedeMi;
            btnReddet.Enabled        = beklemedeMi;
            txtRedAciklamasi.Enabled = beklemedeMi;
            btnIzinDuzenle.Enabled   = seciliVar && !iptalEdildiMi && !reddedildiMi;
            btnIzinSil.Enabled       = seciliVar && !iptalEdildiMi;
        }

        // ── Talepler Listesi (servis katmanından) ─────────────────
        private async Task TalepleriGetirAsync(string durumFiltresi, int departmanId = 0, string personelAdi = "", DateTime? talepBas = null, DateTime? talepBit = null)
        {
            try
            {
                DataTable dt = await _izinService.TalepleriGetirAsync(durumFiltresi.Trim(), departmanId, personelAdi.Trim(), talepBas, talepBit);
                dgvBekleyenIzinler.DataSource = dt;

                // --- Grid Görünüm Ayarları ---
                if (dgvBekleyenIzinler.Columns.Contains("LeaveID")) dgvBekleyenIzinler.Columns["LeaveID"].Visible = false;
                if (dgvBekleyenIzinler.Columns.Contains("EmployeeID")) dgvBekleyenIzinler.Columns["EmployeeID"].Visible = false;

                dgvBekleyenIzinler.Columns["PersonelAd"].HeaderText  = "Personel";
                dgvBekleyenIzinler.Columns["PersonelAd"].Width        = 160;

                dgvBekleyenIzinler.Columns["Departman"].HeaderText    = "Departman";
                dgvBekleyenIzinler.Columns["Departman"].Width         = 130;

                dgvBekleyenIzinler.Columns["StartDate"].HeaderText    = "Başlangıç";
                dgvBekleyenIzinler.Columns["StartDate"].DefaultCellStyle.Format = "dd.MM.yyyy";
                dgvBekleyenIzinler.Columns["StartDate"].Width         = 95;

                dgvBekleyenIzinler.Columns["EndDate"].HeaderText      = "Bitiş";
                dgvBekleyenIzinler.Columns["EndDate"].DefaultCellStyle.Format = "dd.MM.yyyy";
                dgvBekleyenIzinler.Columns["EndDate"].Width           = 95;

                dgvBekleyenIzinler.Columns["DayCount"].HeaderText     = "Gün";
                dgvBekleyenIzinler.Columns["DayCount"].Width          = 50;
                dgvBekleyenIzinler.Columns["DayCount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                dgvBekleyenIzinler.Columns["Status"].HeaderText       = "Durum";
                dgvBekleyenIzinler.Columns["Status"].Width            = 110;

                dgvBekleyenIzinler.Columns["CreatedDate"].HeaderText  = "Talep Tarihi";
                dgvBekleyenIzinler.Columns["CreatedDate"].DefaultCellStyle.Format = "dd.MM.yyyy";
                dgvBekleyenIzinler.Columns["CreatedDate"].Width       = 95;

                if (dgvBekleyenIzinler.Columns.Contains("LeaveType"))
                {
                    dgvBekleyenIzinler.Columns["LeaveType"].HeaderText = "Tür";
                    dgvBekleyenIzinler.Columns["LeaveType"].Width      = 70;
                }

                dgvBekleyenIzinler.ReadOnly = true;
                dgvBekleyenIzinler.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgvBekleyenIzinler.MultiSelect = false;
                dgvBekleyenIzinler.AllowUserToAddRows = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Talepler yüklenirken hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Satır seçili mi ve statüsü uygun mu kontrolü
        private bool SeciliIslemGecerliMi(out int seciliIzinID, out string personelAd, out string statu)
        {
            seciliIzinID = 0;
            personelAd = string.Empty;
            statu = string.Empty;

            if (dgvBekleyenIzinler.SelectedRows.Count == 0)
            {
                MessageBox.Show("Lütfen işlem yapmak için bir talep seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            seciliIzinID = Convert.ToInt32(dgvBekleyenIzinler.SelectedRows[0].Cells["LeaveID"].Value);
            personelAd = dgvBekleyenIzinler.SelectedRows[0].Cells["PersonelAd"].Value?.ToString() ?? "";
            statu      = dgvBekleyenIzinler.SelectedRows[0].Cells["Status"].Value?.ToString()     ?? "";

            return true;
        }

        // ── Onayla (servis katmanına delege) ──────────────────────
        private async void btnOnayla_Click(object? sender, EventArgs e)
        {
            if (!SeciliIslemGecerliMi(out int seciliIzinID, out string personelAd, out string statu)) return;

            if (statu != "Beklemede")
            {
                MessageBox.Show("Sadece 'Beklemede' statüsündeki izin taleplerini onaylayabilirsiniz!", "Yetki Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int gunSayisi = Convert.ToInt32(dgvBekleyenIzinler.SelectedRows[0].Cells["DayCount"].Value ?? 0);
            DialogResult onay = MessageBox.Show($"{personelAd} adlı personelin {gunSayisi} günlük izin talebini onaylıyor musunuz?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (onay != DialogResult.Yes) return;

            try
            {
                IzinIslemSonucu sonuc = await _izinService.OnaylaAsync(seciliIzinID, Session.CurrentEmployeeID, personelAd, gunSayisi);

                if (sonuc.Basarili)
                {
                    MessageBox.Show(sonuc.Mesaj, "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    await FiltreyiUygulaAsync();
                }
                else
                {
                    MessageBox.Show(sonuc.HataMesaji!, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Onaylama sırasında hata: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ── Reddet (servis katmanına delege) ──────────────────────
        private async void btnReddet_Click(object? sender, EventArgs e)
        {
            if (!SeciliIslemGecerliMi(out int seciliIzinID, out string personelAd, out string statu)) return;

            if (statu != "Beklemede")
            {
                MessageBox.Show("Sadece 'Beklemede' statüsündeki izin taleplerini reddedebilirsiniz!", "Yetki Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string redSebebi = txtRedAciklamasi.Text.Trim();

            if (string.IsNullOrEmpty(redSebebi) || redSebebi.Length < 5)
            {
                MessageBox.Show("Lütfen geçerli bir reddetme gerekçesi giriniz (En az 5 karakter).", "Gerekçe Eksik", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtRedAciklamasi.Focus();
                return;
            }

            DialogResult onay = MessageBox.Show($"{personelAd} adlı personelin izin talebini REDDETMEK istediğinize emin misiniz?", "Red Onayı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (onay != DialogResult.Yes) return;

            try
            {
                IzinIslemSonucu sonuc = await _izinService.ReddetAsync(seciliIzinID, Session.CurrentEmployeeID, redSebebi, personelAd);

                if (sonuc.Basarili)
                {
                    MessageBox.Show(sonuc.Mesaj, "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtRedAciklamasi.Clear();
                    if (cmbRedSebebi.Items.Count > 0)
                        cmbRedSebebi.SelectedIndex = 0;
                    await FiltreyiUygulaAsync();
                }
                else
                {
                    MessageBox.Show(sonuc.HataMesaji!, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Reddetme işlemi sırasında hata: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ── Red sebebi şablonu seçildiğinde txtRedAciklamasi'ne kopyala ──
        private void cmbRedSebebi_SelectedIndexChanged(object? sender, EventArgs e)
        {
            // İlk öğe placeholder ("— Şablon Seç —"); seçilirse hiçbir şey yapma
            if (cmbRedSebebi.SelectedIndex <= 0) return;

            string sablon = cmbRedSebebi.SelectedItem?.ToString() ?? "";
            if (!string.IsNullOrEmpty(sablon))
            {
                txtRedAciklamasi.Text = sablon;
                // İmleci sona al — kullanıcı düzenleyebilsin
                txtRedAciklamasi.SelectionStart = txtRedAciklamasi.Text.Length;
                txtRedAciklamasi.SelectionLength = 0;
                txtRedAciklamasi.Focus();
            }
        }

        // ── Diskten kayıtlı özel şablonları yükle ──
        private void OzelRedSablonlariniYukle()
        {
            try
            {
                var ozeller = RedSablonStore.Yukle();
                foreach (var s in ozeller)
                {
                    if (string.IsNullOrWhiteSpace(s)) continue;
                    cmbRedSebebi.Items.Add(s);
                }
            }
            catch (Exception ex)
            {
                // Sessiz başarısızlık — özel şablonlar olmadan devam et
                System.Diagnostics.Debug.WriteLine("Özel şablonlar yüklenemedi: " + ex.Message);
            }
        }

        // ── Mevcut Items'tan kullanıcı eklemelerini ayıkla (yerleşik sonrası) ──
        private List<string> OzelSablonlariTopla()
        {
            var liste = new List<string>();
            for (int i = _yerlesikSablonSayisi; i < cmbRedSebebi.Items.Count; i++)
            {
                string? s = cmbRedSebebi.Items[i]?.ToString();
                if (!string.IsNullOrWhiteSpace(s)) liste.Add(s);
            }
            return liste;
        }

        // ── ➕ Şablona Ekle ──
        private void btnSablonEkle_Click(object? sender, EventArgs e)
        {
            string yeni = txtRedAciklamasi.Text?.Trim() ?? "";

            if (yeni.Length < 5)
            {
                MessageBox.Show("Şablona eklemek için açıklama en az 5 karakter olmalıdır.",
                    "Geçersiz Şablon", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtRedAciklamasi.Focus();
                return;
            }

            // Yinelenenleri (case-insensitive) engelle
            for (int i = 0; i < cmbRedSebebi.Items.Count; i++)
            {
                string mevcut = cmbRedSebebi.Items[i]?.ToString() ?? "";
                if (string.Equals(mevcut, yeni, StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show("Bu şablon zaten listede mevcut.",
                        "Yinelenen Şablon", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cmbRedSebebi.SelectedIndex = i;
                    return;
                }
            }

            try
            {
                int yeniIndex = cmbRedSebebi.Items.Add(yeni);
                RedSablonStore.Kaydet(OzelSablonlariTopla());
                cmbRedSebebi.SelectedIndex = yeniIndex;
                MessageBox.Show("Şablon başarıyla eklendi.\n\nSilmek için ComboBox'a sağ tıklayabilirsiniz.",
                    "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Şablon kaydedilirken hata oluştu: " + ex.Message,
                    "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ── Sağ tık menüsü kurulumu (sadece özel şablonlar silinebilir) ──
        private void RedSablonContextMenuKur()
        {
            _redSablonMenu = new ContextMenuStrip();
            _miSablonSil  = new ToolStripMenuItem("🗑  Bu şablonu sil");
            _miSablonSil.Click += MiSablonSil_Click;
            _redSablonMenu.Items.Add(_miSablonSil);

            // Menü açılmadan önce: sadece kullanıcı eklediği bir şablon seçiliyse aktif et
            _redSablonMenu.Opening += (s, e) =>
            {
                int idx = cmbRedSebebi.SelectedIndex;
                bool ozelMi = idx >= _yerlesikSablonSayisi && idx < cmbRedSebebi.Items.Count;
                _miSablonSil!.Enabled = ozelMi;
                if (!ozelMi)
                    _miSablonSil.Text = "🗑  (Yerleşik şablon — silinemez)";
                else
                    _miSablonSil.Text = "🗑  Bu şablonu sil";
            };

            cmbRedSebebi.ContextMenuStrip = _redSablonMenu;
        }

        private void MiSablonSil_Click(object? sender, EventArgs e)
        {
            int idx = cmbRedSebebi.SelectedIndex;
            if (idx < _yerlesikSablonSayisi || idx >= cmbRedSebebi.Items.Count) return;

            string sablon = cmbRedSebebi.Items[idx]?.ToString() ?? "";
            string kisaltma = sablon.Length > 60 ? sablon.Substring(0, 60) + "…" : sablon;

            DialogResult onay = MessageBox.Show(
                $"Bu özel şablonu silmek istediğinize emin misiniz?\n\n\"{kisaltma}\"",
                "Şablon Sil", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (onay != DialogResult.Yes) return;

            try
            {
                cmbRedSebebi.Items.RemoveAt(idx);
                RedSablonStore.Kaydet(OzelSablonlariTopla());
                cmbRedSebebi.SelectedIndex = 0; // Placeholder'a dön
            }
            catch (Exception ex)
            {
                MessageBox.Show("Şablon silinirken hata oluştu: " + ex.Message,
                    "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ── İzin Düzenle (servis katmanına delege) ────────────────
        private async void btnIzinDuzenle_Click(object? sender, EventArgs e)
        {
            if (!SeciliIslemGecerliMi(out int seciliIzinID, out string personelAd, out string statu)) return;

            if (statu == "Iptal Edildi" || statu == "Reddedildi")
            {
                MessageBox.Show("İptal edilmiş veya reddedilmiş izinler üzerinde düzenleme yapılamaz!", "Kural Hatası", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int seciliPersonelID = Convert.ToInt32(dgvBekleyenIzinler.SelectedRows[0].Cells["EmployeeID"].Value);
            DateTime yeniBaslangic = dtpBaslangic.Value.Date;
            DateTime yeniBitis = dtpBitis.Value.Date;

            if (yeniBaslangic < DateTime.Today || yeniBitis < yeniBaslangic)
            {
                MessageBox.Show("Tarih kurallarına uymayan bir seçim yaptınız!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int talepEdilenGun = DateHelper.IsGunuHesapla(yeniBaslangic, yeniBitis);

            if (talepEdilenGun == 0)
            {
                MessageBox.Show("Seçtiğiniz tarihler sadece hafta sonu!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult onay = MessageBox.Show(
                $"Yönetici yetkisiyle {personelAd} adlı personelin iznini {yeniBaslangic.ToShortDateString()} - {yeniBitis.ToShortDateString()} olarak DEĞİŞTİRMEK istiyor musunuz?",
                "Yönetici Düzenleme Onayı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (onay != DialogResult.Yes) return;

            try
            {
                var istek = new IzinGuncelleIstek(
                    EmployeeId: seciliPersonelID,
                    DepartmanId: 0, // Yönetici tüm departmanlarda işlem yapabilir
                    YapanId: Session.CurrentEmployeeID,
                    PersonelAd: personelAd,
                    YeniBaslangic: yeniBaslangic,
                    YeniBitis: yeniBitis,
                    KalanIzin: int.MaxValue // Yönetici bakiye kontrolünü bypass eder
                );

                IzinIslemSonucu sonuc = await _izinService.IzinGuncelleYoneticiAsync(seciliIzinID, istek);

                if (sonuc.Basarili)
                {
                    MessageBox.Show(sonuc.Mesaj, "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    await FiltreyiUygulaAsync();
                }
                else
                {
                    MessageBox.Show(sonuc.HataMesaji!, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Sistem hatası: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ── İzin Sil / İptal Et (servis katmanına delege) ────────
        private async void btnIzinSil_Click(object? sender, EventArgs e)
        {
            if (dgvBekleyenIzinler.SelectedRows.Count == 0)
            {
                MessageBox.Show("Lütfen iptal etmek (silmek) istediğiniz izni seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int seciliIzinID = Convert.ToInt32(dgvBekleyenIzinler.SelectedRows[0].Cells["LeaveID"].Value);
            string personelAd = dgvBekleyenIzinler.SelectedRows[0].Cells["PersonelAd"].Value?.ToString() ?? "";
            string statu      = dgvBekleyenIzinler.SelectedRows[0].Cells["Status"].Value?.ToString()     ?? "";

            if (statu == "Iptal Edildi")
            {
                MessageBox.Show("Bu izin zaten iptal edilmiş durumda.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult onay = MessageBox.Show(
                $"Yönetici müdahalesi ile {personelAd} adlı personelin iznini SİLMEK (İptal Etmek) istediğinize emin misiniz?",
                "Kritik İşlem Onayı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (onay != DialogResult.Yes) return;

            try
            {
                IzinIslemSonucu sonuc = await _izinService.IzinIptalEtYoneticiAsync(seciliIzinID, Session.CurrentEmployeeID, personelAd);

                if (sonuc.Basarili)
                {
                    MessageBox.Show(sonuc.Mesaj, "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    await FiltreyiUygulaAsync();
                }
                else
                {
                    MessageBox.Show(sonuc.HataMesaji!, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Silme işlemi sırasında hata: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ── İzin Talepleri — Excel'e Aktar (filtrelenmiş veri) ────
        private void btnTalepExcel_Click(object? sender, EventArgs e)
        {
            var dt = dgvBekleyenIzinler.DataSource as DataTable;
            if (dt == null || dt.Rows.Count == 0)
            {
                MessageBox.Show("Aktarılacak izin talebi bulunamadı.",
                    "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // DefaultView üzerinden — aktif RowFilter'a saygı gösterir
            var gorunenSatirlar = dt.DefaultView.Cast<DataRowView>().Select(v => v.Row).ToList();
            if (gorunenSatirlar.Count == 0)
            {
                MessageBox.Show("Filtrelenmiş listede kayıt bulunamadı.",
                    "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Aktif filtre özetini hazırla
            string durumStr     = cmbFiltre.SelectedItem?.ToString() ?? "Tümü";
            string departmanStr = cmbDepartman.SelectedItem is DataRowView drv
                                    ? drv["DepartmentName"]?.ToString() ?? "Tümü"
                                    : (cmbDepartman.Text ?? "Tümü");
            string personelStr  = string.IsNullOrWhiteSpace(txtPersonelAra.Text) ? "—" : txtPersonelAra.Text.Trim();
            DateTime? talepBas  = dtpTalepBas.Checked ? dtpTalepBas.Value.Date : (DateTime?)null;
            DateTime? talepBit  = dtpTalepBit.Checked ? dtpTalepBit.Value.Date : (DateTime?)null;

            using var sfd = new SaveFileDialog
            {
                Title      = "Excel Dosyasını Kaydet",
                Filter     = "Excel Dosyası (*.xlsx)|*.xlsx",
                FileName   = $"IzinTalepleri_{DateTime.Now:yyyyMMdd_HHmm}.xlsx",
                DefaultExt = "xlsx"
            };
            if (sfd.ShowDialog() != DialogResult.OK) return;

            try
            {
                // Görünen kolonları DisplayIndex sırasına göre topla (gizliler hariç)
                var gorunenKolonlar = dgvBekleyenIzinler.Columns
                    .Cast<DataGridViewColumn>()
                    .Where(c => c.Visible && dt.Columns.Contains(c.DataPropertyName))
                    .OrderBy(c => c.DisplayIndex)
                    .ToList();

                using var wb = new XLWorkbook();
                var ws = wb.Worksheets.Add("İzin Talepleri");

                int colCount = gorunenKolonlar.Count;

                // Başlık (1. satır)
                ws.Cell(1, 1).Value = "İzin Talepleri Raporu";
                ws.Cell(1, 1).Style.Font.Bold = true;
                ws.Cell(1, 1).Style.Font.FontSize = 13;
                ws.Range(1, 1, 1, colCount).Merge();

                // Filtre bilgisi (2. satır)
                string aralikStr = (talepBas.HasValue || talepBit.HasValue)
                    ? $"{(talepBas.HasValue ? talepBas.Value.ToString("dd.MM.yyyy") : "…")} – {(talepBit.HasValue ? talepBit.Value.ToString("dd.MM.yyyy") : "…")}"
                    : "—";
                string filtreBilgi = $"Durum: {durumStr}   |   Departman: {departmanStr}   |   Personel: {personelStr}   |   Tarih Aralığı: {aralikStr}   |   Rapor: {DateTime.Now:dd.MM.yyyy HH:mm}";
                ws.Cell(2, 1).Value = filtreBilgi;
                ws.Cell(2, 1).Style.Font.Italic = true;
                ws.Range(2, 1, 2, colCount).Merge();

                // Sütun başlıkları (3. satır)
                for (int c = 0; c < gorunenKolonlar.Count; c++)
                {
                    var cell = ws.Cell(3, c + 1);
                    cell.Value = gorunenKolonlar[c].HeaderText;
                    cell.Style.Font.Bold = true;
                    cell.Style.Fill.BackgroundColor = XLColor.FromArgb(31, 73, 125);
                    cell.Style.Font.FontColor = XLColor.White;
                    cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                }

                // Veri satırları
                for (int r = 0; r < gorunenSatirlar.Count; r++)
                {
                    DataRow satir = gorunenSatirlar[r];

                    // Satır rengi: Avans → mor, yoksa Status'e göre
                    XLColor? satirBg = null;
                    XLColor? satirFg = null;

                    bool avansMi = dt.Columns.Contains("LeaveType") &&
                                   satir["LeaveType"]?.ToString() == "Avans";
                    if (avansMi)
                    {
                        satirBg = XLColor.FromArgb(225, 190, 231);
                        satirFg = XLColor.FromArgb(106, 27, 154);
                    }
                    else if (dt.Columns.Contains("Status"))
                    {
                        string st = satir["Status"]?.ToString() ?? "";
                        switch (st)
                        {
                            case "Beklemede":    satirBg = XLColor.FromArgb(255, 249, 196); satirFg = XLColor.FromArgb(130, 100, 0);   break;
                            case "Onaylandi":    satirBg = XLColor.FromArgb(200, 230, 201); satirFg = XLColor.FromArgb(27, 94, 32);   break;
                            case "Reddedildi":   satirBg = XLColor.FromArgb(255, 205, 210); satirFg = XLColor.FromArgb(183, 28, 28);  break;
                            case "Iptal Edildi": satirBg = XLColor.FromArgb(236, 239, 241); satirFg = XLColor.FromArgb(96, 125, 139); break;
                        }
                    }
                    // Status'e göre renk yoksa zebra
                    if (satirBg == null && r % 2 == 1)
                        satirBg = XLColor.FromArgb(242, 242, 242);

                    for (int c = 0; c < gorunenKolonlar.Count; c++)
                    {
                        string colName = gorunenKolonlar[c].DataPropertyName;
                        var val = satir[colName];
                        var cell = ws.Cell(r + 4, c + 1);

                        if (val == DBNull.Value || val == null)
                        {
                            cell.Value = "";
                        }
                        else if (val is DateTime dtVal)
                        {
                            cell.Value = dtVal;
                            cell.Style.DateFormat.Format = "dd.MM.yyyy";
                        }
                        else if (val is int || val is long || val is short)
                        {
                            cell.Value = Convert.ToDouble(val);
                            cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        }
                        else
                        {
                            cell.Value = val.ToString() ?? "";
                        }

                        if (satirBg != null) cell.Style.Fill.BackgroundColor = satirBg;
                        if (satirFg != null) cell.Style.Font.FontColor = satirFg;
                    }
                }

                ws.Columns().AdjustToContents();

                int sonSatir = gorunenSatirlar.Count + 3;
                ws.Range(3, 1, sonSatir, colCount).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Range(3, 1, sonSatir, colCount).Style.Border.InsideBorder  = XLBorderStyleValues.Hair;

                wb.SaveAs(sfd.FileName);

                var ac = MessageBox.Show(
                    $"İzin talepleri başarıyla aktarıldı.\n\n{sfd.FileName}\n\nDosyayı şimdi açmak ister misiniz?",
                    "Excel Aktarımı", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (ac == DialogResult.Yes)
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(sfd.FileName) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Excel aktarımı sırasında hata oluştu: " + ex.Message,
                    "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ── Alt Formlar (servisleri enjekte ederek aç) ────────────
        private void btnLoglariGor_Click(object? sender, EventArgs e)
        {
            LogForm logEkrani = new LogForm(_raporService);
            logEkrani.ShowDialog();
        }

        private void btnRaporlar_Click(object? sender, EventArgs e)
        {
            RaporForm raporForm = new RaporForm(_raporService);
            raporForm.ShowDialog();
        }

        // ══════════════════════════════════════════════════════════
        // TOPLU ONAY SEKMESİ
        // ══════════════════════════════════════════════════════════

        // Toplu Onay sekmesine geçildiğinde otomatik yükle
        private async void tabMain_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (tabMain.SelectedTab == tabPageTopluOnay)
                await TopluOnayTabiniYukleAsync();
        }

        private async void btnTopluYenile_Click(object? sender, EventArgs e)
        {
            await TopluOnayTabiniYukleAsync();
        }

        private async Task TopluOnayTabiniYukleAsync()
        {
            try
            {
                DataTable dt = await _izinService.TalepleriGetirAsync("Beklemede", 0, "");

                // Checkbox kolonu en başa ekle
                if (!dt.Columns.Contains("Secili"))
                    dt.Columns.Add("Secili", typeof(bool));
                foreach (DataRow row in dt.Rows) row["Secili"] = false;

                dgvTopluOnay.DataSource = dt;

                // Gizli kolonlar
                if (dgvTopluOnay.Columns.Contains("LeaveID"))    dgvTopluOnay.Columns["LeaveID"].Visible    = false;
                if (dgvTopluOnay.Columns.Contains("EmployeeID")) dgvTopluOnay.Columns["EmployeeID"].Visible = false;
                if (dgvTopluOnay.Columns.Contains("Status"))     dgvTopluOnay.Columns["Status"].Visible     = false;

                // Checkbox kolonu
                if (dgvTopluOnay.Columns.Contains("Secili"))
                {
                    dgvTopluOnay.Columns["Secili"].HeaderText    = "✔";
                    dgvTopluOnay.Columns["Secili"].Width         = 40;
                    dgvTopluOnay.Columns["Secili"].DisplayIndex  = 0;
                    dgvTopluOnay.Columns["Secili"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }

                if (dgvTopluOnay.Columns.Contains("PersonelAd"))
                {
                    dgvTopluOnay.Columns["PersonelAd"].HeaderText = "Personel";
                    dgvTopluOnay.Columns["PersonelAd"].Width      = 160;
                }
                if (dgvTopluOnay.Columns.Contains("Departman"))
                {
                    dgvTopluOnay.Columns["Departman"].HeaderText = "Departman";
                    dgvTopluOnay.Columns["Departman"].Width      = 130;
                }
                if (dgvTopluOnay.Columns.Contains("StartDate"))
                {
                    dgvTopluOnay.Columns["StartDate"].HeaderText = "Başlangıç";
                    dgvTopluOnay.Columns["StartDate"].DefaultCellStyle.Format = "dd.MM.yyyy";
                    dgvTopluOnay.Columns["StartDate"].Width      = 95;
                }
                if (dgvTopluOnay.Columns.Contains("EndDate"))
                {
                    dgvTopluOnay.Columns["EndDate"].HeaderText = "Bitiş";
                    dgvTopluOnay.Columns["EndDate"].DefaultCellStyle.Format = "dd.MM.yyyy";
                    dgvTopluOnay.Columns["EndDate"].Width      = 95;
                }
                if (dgvTopluOnay.Columns.Contains("DayCount"))
                {
                    dgvTopluOnay.Columns["DayCount"].HeaderText = "Gün";
                    dgvTopluOnay.Columns["DayCount"].Width      = 50;
                    dgvTopluOnay.Columns["DayCount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
                if (dgvTopluOnay.Columns.Contains("CreatedDate"))
                {
                    dgvTopluOnay.Columns["CreatedDate"].HeaderText = "Talep Tarihi";
                    dgvTopluOnay.Columns["CreatedDate"].DefaultCellStyle.Format = "dd.MM.yyyy";
                    dgvTopluOnay.Columns["CreatedDate"].Width      = 95;
                }

                dgvTopluOnay.SelectionMode      = DataGridViewSelectionMode.FullRowSelect;
                dgvTopluOnay.MultiSelect         = false;
                dgvTopluOnay.AllowUserToAddRows  = false;

                // Sağ panel sıfırla
                TopluOnaySayaciniGuncelle();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Talepler yüklenirken hata: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Checkbox tek tıkla commit edilsin
        private void dgvTopluOnay_CurrentCellDirtyStateChanged(object? sender, EventArgs e)
        {
            if (dgvTopluOnay.IsCurrentCellDirty &&
                dgvTopluOnay.CurrentCell is DataGridViewCheckBoxCell)
            {
                dgvTopluOnay.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        // Checkbox değiştiğinde sağ paneli güncelle
        private void dgvTopluOnay_CellValueChanged(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (dgvTopluOnay.Columns.Count > e.ColumnIndex &&
                dgvTopluOnay.Columns[e.ColumnIndex].Name == "Secili")
            {
                TopluOnaySayaciniGuncelle();
            }
        }

        // Seçili listesini ve sayacı güncelle
        private void TopluOnaySayaciniGuncelle()
        {
            // Grid henüz yüklenmemişse çık
            if (!dgvTopluOnay.Columns.Contains("Secili")) return;

            var secilenDt = new DataTable();
            secilenDt.Columns.Add("Personel",   typeof(string));
            secilenDt.Columns.Add("Başlangıç",  typeof(string));
            secilenDt.Columns.Add("Bitiş",      typeof(string));
            secilenDt.Columns.Add("Gün",        typeof(int));

            int seciliSayisi = 0;

            foreach (DataGridViewRow row in dgvTopluOnay.Rows)
            {
                if (row.Cells["Secili"].Value is true)
                {
                    seciliSayisi++;
                    string ad  = row.Cells["PersonelAd"].Value?.ToString() ?? "-";
                    string bas = row.Cells["StartDate"].Value is DateTime bd ? bd.ToString("dd.MM.yyyy") : "-";
                    string bit = row.Cells["EndDate"].Value   is DateTime ed ? ed.ToString("dd.MM.yyyy") : "-";
                    int gun    = row.Cells["DayCount"].Value  is int g       ? g : 0;
                    secilenDt.Rows.Add(ad, bas, bit, gun);
                }
            }

            lblSeciliSayisi.Text = seciliSayisi == 0
                ? "0 talep seçildi"
                : $"{seciliSayisi} talep seçildi";

            dgvSecilen.DataSource        = secilenDt;
            dgvSecilen.ReadOnly          = true;
            dgvSecilen.AllowUserToAddRows = false;
            if (dgvSecilen.Columns.Count > 0)
                dgvSecilen.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        // Toplu onaylama
        private async void btnTopluOnayla_Click(object? sender, EventArgs e)
        {
            var seciliIzinler = new List<(int LeaveID, string PersonelAd, int DayCount)>();

            foreach (DataGridViewRow row in dgvTopluOnay.Rows)
            {
                if (row.Cells["Secili"].Value is true)
                {
                    int leaveId  = Convert.ToInt32(row.Cells["LeaveID"].Value);
                    string ad    = row.Cells["PersonelAd"].Value?.ToString() ?? "";
                    int gun      = row.Cells["DayCount"].Value is int g ? g : 0;
                    seciliIzinler.Add((leaveId, ad, gun));
                }
            }

            if (seciliIzinler.Count == 0)
            {
                MessageBox.Show("Onaylanacak talep seçilmedi.\nCheckbox sütunundan talepleri işaretleyin.",
                    "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult onay = MessageBox.Show(
                $"{seciliIzinler.Count} adet izin talebini ONAYLAMAK istediğinize emin misiniz?",
                "Toplu Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (onay != DialogResult.Yes) return;

            int basarili = 0, basarisiz = 0;

            foreach (var (leaveId, personelAd, gunSayisi) in seciliIzinler)
            {
                try
                {
                    IzinIslemSonucu sonuc = await _izinService.OnaylaAsync(
                        leaveId, Session.CurrentEmployeeID, personelAd, gunSayisi);
                    if (sonuc.Basarili) basarili++;
                    else basarisiz++;
                }
                catch
                {
                    basarisiz++;
                }
            }

            string mesaj = $"Toplu onay tamamlandı.\n✔ Onaylanan: {basarili}\n✘ Başarısız: {basarisiz}";
            MessageBox.Show(mesaj, "Toplu Onay Sonucu", MessageBoxButtons.OK,
                basarisiz == 0 ? MessageBoxIcon.Information : MessageBoxIcon.Warning);

            // Her iki sekmedeki listeyi yenile
            await TopluOnayTabiniYukleAsync();
            await FiltreyiUygulaAsync();
        }
    }
}
