using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ClosedXML.Excel;
using IzinTakip.Core.Abstractions;
using IzinTakip.Core.Helpers;
using IzinTakip.Core.Models;
using Microsoft.VisualBasic;

namespace IzinTakipSistemi
{
    public partial class AmirForm : Form
    {
        private readonly IIzinService _izinService;
        private readonly IRaporService _raporService;
        private bool _isLoggingOut = false;
        private DataTable? _personelTable;   // arama filtresi için master kopya

        public AmirForm(IIzinService izinService, IRaporService raporService)
        {
            _izinService = izinService;
            _raporService = raporService;
            InitializeComponent();
            this.FormClosed += AmirForm_FormClosed;
            dgvPersoneller.CellFormatting += dgvPersoneller_CellFormatting;
            dgvIzinler.CellFormatting += dgvIzinler_CellFormatting;
            tabMain.SelectedIndexChanged += tabMain_SelectedIndexChanged;
            dgvAmirOnay.SelectionChanged += dgvAmirOnay_SelectionChanged;
        }

        private async void AmirForm_Load(object? sender, EventArgs e)
        {
            this.Text = $"Amir Paneli - {Session.CurrentFullName} | {Session.CurrentDepartmentName}";

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

            await PersonelleriGetirAsync();
        }

        // X butonu ile kapatılırsa uygulamayı sonlandır
        private void AmirForm_FormClosed(object? sender, FormClosedEventArgs e)
        {
            if (!_isLoggingOut)
                Application.Exit();
        }

        private void btnCikis_Click(object? sender, EventArgs e)
        {
            var onay = MessageBox.Show(
                "Oturumu kapatıp giriş ekranına dönmek istiyor musunuz?",
                "Çıkış", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (onay != DialogResult.Yes) return;

            _isLoggingOut = true;
            Session.Clear();

            // Gizlenmiş LoginForm'u bul ve tekrar göster
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

        // ── Personel Listesi (servis katmanından) ─────────────────
        private async Task PersonelleriGetirAsync(DateTime? aralikBas = null, DateTime? aralikBit = null)
        {
            try
            {
                DataTable dt = await _izinService.PersonelListesiGetirAsync(Session.CurrentDepartmentID, aralikBas, aralikBit);

                _personelTable = dt;           // arama için sakla
                dgvPersoneller.DataSource = dt;

                // Gizli kolonlar
                if (dgvPersoneller.Columns.Contains("EmployeeID"))
                    dgvPersoneller.Columns["EmployeeID"].Visible = false;

                // Görünür kolonların başlık ve sıralaması
                dgvPersoneller.Columns["FullName"].HeaderText = "Ad Soyad";
                dgvPersoneller.Columns["FullName"].Width = 160;
                dgvPersoneller.Columns["FullName"].DisplayIndex = 0;

                dgvPersoneller.Columns["SicilNo"].HeaderText = "Sicil No";
                dgvPersoneller.Columns["SicilNo"].Width = 80;
                dgvPersoneller.Columns["SicilNo"].DisplayIndex = 1;

                dgvPersoneller.Columns["StartDateOfWork"].HeaderText = "İşe Giriş";
                dgvPersoneller.Columns["StartDateOfWork"].Width = 95;
                dgvPersoneller.Columns["StartDateOfWork"].DisplayIndex = 2;
                dgvPersoneller.Columns["StartDateOfWork"].DefaultCellStyle.Format = "dd.MM.yyyy";

                dgvPersoneller.Columns["KidemYil"].HeaderText = "Kıdem (Yıl)";
                dgvPersoneller.Columns["KidemYil"].Width = 90;
                dgvPersoneller.Columns["KidemYil"].DisplayIndex = 3;
                dgvPersoneller.Columns["KidemYil"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                dgvPersoneller.Columns["KullanilanIzin"].HeaderText = "Kullanılan";
                dgvPersoneller.Columns["KullanilanIzin"].Width = 85;
                dgvPersoneller.Columns["KullanilanIzin"].DisplayIndex = 4;
                dgvPersoneller.Columns["KullanilanIzin"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                dgvPersoneller.Columns["ToplamHak"].HeaderText = "Toplam Hak";
                dgvPersoneller.Columns["ToplamHak"].Width = 90;
                dgvPersoneller.Columns["ToplamHak"].DisplayIndex = 5;
                dgvPersoneller.Columns["ToplamHak"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                dgvPersoneller.Columns["KalanIzin"].HeaderText = "Kalan İzin";
                dgvPersoneller.Columns["KalanIzin"].Width = 85;
                dgvPersoneller.Columns["KalanIzin"].DisplayIndex = 6;
                dgvPersoneller.Columns["KalanIzin"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                dgvPersoneller.ReadOnly = true;
                dgvPersoneller.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgvPersoneller.MultiSelect = false;
                dgvPersoneller.AllowUserToAddRows = false;

                // Filtre aktifse sonuç sayısını göster
                if (aralikBas.HasValue || aralikBit.HasValue)
                    lblPersSonuc.Text = $"{dt.Rows.Count} personel izinli";
                else
                    lblPersSonuc.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Personeller yüklenirken hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // DataView.RowFilter için özel karakterleri güvenli hale getirir
        private static string RowFilterKacir(string deger) =>
            deger.Replace("'", "''")
                 .Replace("[", "[[]")
                 .Replace("]", "[]]")
                 .Replace("%", "[%]")
                 .Replace("*", "[*]");

        // ── Anlık arama ───────────────────────────────────────────
        private void txtArama_TextChanged(object? sender, EventArgs e)
        {
            if (_personelTable == null) return;
            string aranan = RowFilterKacir(txtArama.Text.Trim());
            _personelTable.DefaultView.RowFilter = string.IsNullOrEmpty(aranan)
                ? ""
                : $"FullName LIKE '%{aranan}%' OR SicilNo LIKE '%{aranan}%'";
        }

        // ── Personel listesi renk kodlaması (kalan izin uyarısı) ──
        private void dgvPersoneller_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var row = dgvPersoneller.Rows[e.RowIndex];
            if (!dgvPersoneller.Columns.Contains("KalanIzin") || row.Cells["KalanIzin"].Value == null) return;

            int kalan = Convert.ToInt32(row.Cells["KalanIzin"].Value);
            if (kalan <= 0)
            {
                row.DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(255, 205, 210); // kırmızı
                row.DefaultCellStyle.ForeColor = System.Drawing.Color.FromArgb(183, 28, 28);
            }
            else if (kalan <= 5)
            {
                row.DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(255, 236, 179); // turuncu
                row.DefaultCellStyle.ForeColor = System.Drawing.Color.FromArgb(230, 81, 0);
            }
            else
            {
                row.DefaultCellStyle.BackColor = System.Drawing.Color.White;
                row.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            }
        }

        // ── İzin listesi renk kodlaması (durum ve tip bazlı) ──────
        private void dgvIzinler_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var row = dgvIzinler.Rows[e.RowIndex];
            if (!dgvIzinler.Columns.Contains("Status") || row.Cells["Status"].Value == null) return;

            // Avans izin — mor renk önceliklidir
            if (dgvIzinler.Columns.Contains("LeaveType") &&
                row.Cells["LeaveType"].Value?.ToString() == "Avans")
            {
                row.DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(225, 190, 231);
                row.DefaultCellStyle.ForeColor = System.Drawing.Color.FromArgb(106, 27, 154);
                return;
            }

            string durum = row.Cells["Status"].Value.ToString() ?? "";
            (System.Drawing.Color bg, System.Drawing.Color fg) = durum switch
            {
                "Beklemede" => (System.Drawing.Color.FromArgb(255, 249, 196), System.Drawing.Color.FromArgb(130, 100, 0)),
                "Onaylandi" => (System.Drawing.Color.FromArgb(200, 230, 201), System.Drawing.Color.FromArgb(27, 94, 32)),
                "Reddedildi" => (System.Drawing.Color.FromArgb(255, 205, 210), System.Drawing.Color.FromArgb(183, 28, 28)),
                "Iptal Edildi" => (System.Drawing.Color.FromArgb(236, 239, 241), System.Drawing.Color.FromArgb(96, 125, 139)),
                _ => (System.Drawing.Color.White, System.Drawing.Color.Black)
            };
            row.DefaultCellStyle.BackColor = bg;
            row.DefaultCellStyle.ForeColor = fg;
        }

        // ── Personel seçilince izin geçmişini getir ───────────────
        private async void dgvPersoneller_SelectionChanged(object? sender, EventArgs e)
        {
            if (dgvPersoneller.SelectedRows.Count > 0)
            {
                var row = dgvPersoneller.SelectedRows[0];
                SecilenPersonelPaneliniGuncelle(row);
                int seciliPersonelID = Convert.ToInt32(row.Cells["EmployeeID"].Value);
                await PersonelIzinleriniGetirAsync(seciliPersonelID);
            }
            else
            {
                SecilenPersonelPaneliniGuncelle(null);
            }
        }

        // ── Seçilen personel bilgi panelini güncelle ───────────────
        private void SecilenPersonelPaneliniGuncelle(DataGridViewRow? row)
        {
            if (row == null)
            {
                pnlSecilenPersonel.BackColor = Color.FromArgb(230, 230, 230);
                lblSecilenPersonelBilgi.ForeColor = Color.FromArgb(120, 120, 120);
                lblSecilenPersonelBilgi.Font = new Font("Segoe UI", 9.5F);
                lblSecilenPersonelBilgi.Text = "  Listeden bir personel seçin...";
                return;
            }

            string ad = row.Cells["FullName"].Value?.ToString() ?? "-";
            string sicil = row.Cells["SicilNo"].Value?.ToString() ?? "-";
            string kidem = row.Cells["KidemYil"].Value?.ToString() ?? "-";
            string toplam = row.Cells["ToplamHak"].Value?.ToString() ?? "-";
            string kalan = row.Cells["KalanIzin"].Value?.ToString() ?? "-";
            int kalanInt = int.TryParse(kalan, out int k) ? k : 999;

            // Kalan izne göre panel rengi
            (Color panelRenk, Color yaziRenk) = kalanInt <= 0
                ? (Color.FromArgb(255, 235, 238), Color.FromArgb(183, 28, 28))
                : kalanInt <= 5
                    ? (Color.FromArgb(255, 248, 225), Color.FromArgb(180, 90, 0))
                    : (Color.FromArgb(213, 232, 252), Color.FromArgb(31, 73, 125));

            pnlSecilenPersonel.BackColor = panelRenk;
            lblSecilenPersonelBilgi.ForeColor = yaziRenk;
            lblSecilenPersonelBilgi.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lblSecilenPersonelBilgi.Text =
                $"  👤  {ad}   |   Sicil: {sicil}   |   Kıdem: {kidem} yıl   |   Toplam Hak: {toplam} gün   |   Kalan İzin: {kalan} gün";

            // Avans butonu yalnızca kalan izin sıfır veya altında olduğunda göster
            btnAvansIzinEkle.Visible = kalanInt <= 0;
        }

        // İzin Geçmişi'nde satır seçilince buton durumlarını güncelle
        private void dgvIzinler_SelectionChanged(object? sender, EventArgs e)
        {
            bool seciliVar = dgvIzinler.SelectedRows.Count > 0;
            string durum = seciliVar
                ? dgvIzinler.SelectedRows[0].Cells["Status"].Value?.ToString() ?? ""
                : "";

            btnIzinSil.Enabled = seciliVar && durum == "Beklemede";
            btnIzinDuzenle.Enabled = seciliVar && durum == "Beklemede";
        }

        // ── İzin Geçmişi (servis katmanından) ────────────────────
        private async Task PersonelIzinleriniGetirAsync(int employeeId)
        {
            try
            {
                DataTable dt = await _izinService.PersonelIzinleriniGetirAsync(employeeId);
                dgvIzinler.DataSource = dt;

                if (dgvIzinler.Columns.Contains("LeaveID")) dgvIzinler.Columns["LeaveID"].Visible = false;

                dgvIzinler.Columns["StartDate"].HeaderText = "Başlangıç";
                dgvIzinler.Columns["StartDate"].DefaultCellStyle.Format = "dd.MM.yyyy";
                dgvIzinler.Columns["StartDate"].Width = 95;

                dgvIzinler.Columns["EndDate"].HeaderText = "Bitiş";
                dgvIzinler.Columns["EndDate"].DefaultCellStyle.Format = "dd.MM.yyyy";
                dgvIzinler.Columns["EndDate"].Width = 95;

                dgvIzinler.Columns["DayCount"].HeaderText = "Gün";
                dgvIzinler.Columns["DayCount"].Width = 50;
                dgvIzinler.Columns["DayCount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                dgvIzinler.Columns["Status"].HeaderText = "Durum";
                dgvIzinler.Columns["Status"].Width = 110;

                dgvIzinler.Columns["ManagerNote"].HeaderText = "Yönetici Notu";
                dgvIzinler.Columns["ManagerNote"].Width = 200;

                dgvIzinler.Columns["CreatedDate"].HeaderText = "Kayıt Tarihi";
                dgvIzinler.Columns["CreatedDate"].DefaultCellStyle.Format = "dd.MM.yyyy";
                dgvIzinler.Columns["CreatedDate"].Width = 95;

                if (dgvIzinler.Columns.Contains("LeaveType"))
                {
                    dgvIzinler.Columns["LeaveType"].HeaderText = "Tür";
                    dgvIzinler.Columns["LeaveType"].Width = 70;
                }

                dgvIzinler.ReadOnly = true;
                dgvIzinler.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgvIzinler.MultiSelect = false;
                dgvIzinler.AllowUserToAddRows = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("İzinler yüklenirken hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ── İzin Ekle (servis katmanına delege) ───────────────────
        private async void btnIzinEkle_Click(object? sender, EventArgs e)
        {
            if (dgvPersoneller.SelectedRows.Count == 0)
            {
                MessageBox.Show("Lütfen izin eklemek için sol taraftan bir personel seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int seciliPersonelID = Convert.ToInt32(dgvPersoneller.SelectedRows[0].Cells["EmployeeID"].Value);
            string personelAd = dgvPersoneller.SelectedRows[0].Cells["FullName"].Value.ToString();
            int kalanIzin = Convert.ToInt32(dgvPersoneller.SelectedRows[0].Cells["KalanIzin"].Value);

            DateTime baslangic = dtpBaslangic.Value.Date;
            DateTime bitis = dtpBitis.Value.Date;

            // Temel validasyonlar form katmanında kalır (UI sorumluluğu)
            if (baslangic < DateTime.Today)
            {
                MessageBox.Show("Geçmiş bir tarihe izin giremezsiniz!", "Kural Hatası", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (bitis < baslangic)
            {
                MessageBox.Show("İzin bitiş tarihi, başlangıç tarihinden önce olamaz!", "Kural Hatası", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int talepEdilenGun = DateHelper.IsGunuHesapla(baslangic, bitis);

            if (talepEdilenGun == 0)
            {
                MessageBox.Show("Seçtiğiniz tarih aralığı sadece hafta sonuna denk geliyor. İzin yazılamaz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (talepEdilenGun > kalanIzin)
            {
                MessageBox.Show($"{personelAd} adlı personelin kalan izni ({kalanIzin} gün), talep edilen izni ({talepEdilenGun} gün) karşılamıyor!", "Bakiye Yetersiz", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult onay = MessageBox.Show(
                $"{personelAd} için {baslangic.ToShortDateString()} - {bitis.ToShortDateString()} tarihleri arasına {talepEdilenGun} günlük izin talebi oluşturulacaktır. Onaylıyor musunuz?",
                "İzin Talebi Onayı", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (onay != DialogResult.Yes) return;

            try
            {
                var istek = new IzinEkleIstek(
                    EmployeeId: seciliPersonelID,
                    DepartmanId: Session.CurrentDepartmentID,
                    AmirId: Session.CurrentEmployeeID,
                    PersonelAd: personelAd,
                    Baslangic: baslangic,
                    Bitis: bitis,
                    KalanIzin: kalanIzin
                );

                IzinIslemSonucu sonuc = await _izinService.IzinEkleAsync(istek);

                if (sonuc.Basarili)
                {
                    MessageBox.Show(sonuc.Mesaj, "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    await PersonelleriGetirAsync();
                    await PersonelIzinleriniGetirAsync(seciliPersonelID);
                }
                else
                {
                    MessageBox.Show(sonuc.HataMesaji!, "İşlem Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("İşlem sırasında hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ── İzin İptal (Soft Delete — servis katmanına delege) ────
        private async void btnIzinSil_Click(object? sender, EventArgs e)
        {
            if (dgvIzinler.SelectedRows.Count == 0)
            {
                MessageBox.Show("Lütfen iptal etmek istediğiniz izni sağ taraftaki İzin Geçmişi listesinden seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (dgvPersoneller.SelectedRows.Count == 0)
            {
                MessageBox.Show("Lütfen önce sol listeden bir personel seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int seciliIzinID = Convert.ToInt32(dgvIzinler.SelectedRows[0].Cells["LeaveID"].Value);
            string durum = dgvIzinler.SelectedRows[0].Cells["Status"].Value?.ToString() ?? "";
            string baslangic = Convert.ToDateTime(dgvIzinler.SelectedRows[0].Cells["StartDate"].Value).ToShortDateString();
            string personelAd = dgvPersoneller.SelectedRows[0].Cells["FullName"].Value?.ToString() ?? "";
            int seciliPersonelID = Convert.ToInt32(dgvPersoneller.SelectedRows[0].Cells["EmployeeID"].Value);

            if (durum != "Beklemede")
            {
                MessageBox.Show("Sadece 'Beklemede' statüsündeki izin taleplerini silebilirsiniz. Onaylanmış veya reddedilmiş izinlere müdahale edilemez.", "Yetki Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DialogResult onay = MessageBox.Show(
                $"{personelAd} adlı personelin {baslangic} başlangıç tarihli BEKLEMEDE olan izin talebini iptal etmek istediğinize emin misiniz?",
                "İptal Onayı", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (onay != DialogResult.Yes) return;

            try
            {
                IzinIslemSonucu sonuc = await _izinService.IzinIptalEtAmirAsync(seciliIzinID, Session.CurrentEmployeeID, personelAd);

                if (sonuc.Basarili)
                {
                    MessageBox.Show(sonuc.Mesaj, "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    await PersonelleriGetirAsync();
                    await PersonelIzinleriniGetirAsync(seciliPersonelID);
                }
                else
                {
                    MessageBox.Show(sonuc.HataMesaji!, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("İşlem sırasında hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ── Personel Listesi — Tarih Aralığı Filtresi ────────────
        private async void btnPersFiltrele_Click(object? sender, EventArgs e)
        {
            DateTime? bas = dtpPersFiltreBas.Checked ? dtpPersFiltreBas.Value.Date : (DateTime?)null;
            DateTime? bit = dtpPersFiltreBit.Checked ? dtpPersFiltreBit.Value.Date : (DateTime?)null;

            if (bas.HasValue && bit.HasValue && bit.Value < bas.Value)
            {
                MessageBox.Show("Bitiş tarihi başlangıç tarihinden önce olamaz!", "Tarih Hatası",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            await PersonelleriGetirAsync(bas, bit);
        }

        // ── Personel Listesi — Excel'e Aktar (filtrelenmiş veri) ──
        private void btnPersExcel_Click(object? sender, EventArgs e)
        {
            var dt = dgvPersoneller.DataSource as DataTable;
            if (dt == null || dt.Rows.Count == 0)
            {
                MessageBox.Show("Aktarılacak personel kaydı bulunamadı.",
                    "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // DefaultView üzerinden — txtArama RowFilter'ı ve aralık filtresi dikkate alınır
            var gorunenSatirlar = dt.DefaultView.Cast<DataRowView>().Select(v => v.Row).ToList();
            if (gorunenSatirlar.Count == 0)
            {
                MessageBox.Show("Filtrelenmiş listede personel bulunamadı.",
                    "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DateTime? aralikBas = dtpPersFiltreBas.Checked ? dtpPersFiltreBas.Value.Date : (DateTime?)null;
            DateTime? aralikBit = dtpPersFiltreBit.Checked ? dtpPersFiltreBit.Value.Date : (DateTime?)null;
            string aramaMetni = txtArama.Text?.Trim() ?? "";

            using var sfd = new SaveFileDialog
            {
                Title = "Excel Dosyasını Kaydet",
                Filter = "Excel Dosyası (*.xlsx)|*.xlsx",
                FileName = $"Personeller_{Session.CurrentDepartmentName}_{DateTime.Now:yyyyMMdd_HHmm}.xlsx",
                DefaultExt = "xlsx"
            };
            if (sfd.ShowDialog() != DialogResult.OK) return;

            try
            {
                // Görünen kolonları (EmployeeID hariç) DisplayIndex sırasına göre topla
                var gorunenKolonlar = dgvPersoneller.Columns
                    .Cast<DataGridViewColumn>()
                    .Where(c => c.Visible && dt.Columns.Contains(c.DataPropertyName))
                    .OrderBy(c => c.DisplayIndex)
                    .ToList();

                using var wb = new XLWorkbook();
                var ws = wb.Worksheets.Add("Personeller");

                int colCount = gorunenKolonlar.Count;

                // Başlık (1. satır)
                ws.Cell(1, 1).Value = $"Departman: {Session.CurrentDepartmentName}";
                ws.Cell(1, 1).Style.Font.Bold = true;
                ws.Cell(1, 1).Style.Font.FontSize = 13;
                ws.Range(1, 1, 1, colCount).Merge();

                // Filtre bilgisi (2. satır)
                string filtreBilgi = "Rapor Tarihi: " + DateTime.Now.ToString("dd.MM.yyyy HH:mm");
                if (aralikBas.HasValue || aralikBit.HasValue)
                {
                    string basStr = aralikBas.HasValue ? aralikBas.Value.ToString("dd.MM.yyyy") : "…";
                    string bitStr = aralikBit.HasValue ? aralikBit.Value.ToString("dd.MM.yyyy") : "…";
                    filtreBilgi += $"   |   İzin Aralığı: {basStr} – {bitStr}";
                }
                if (!string.IsNullOrEmpty(aramaMetni))
                    filtreBilgi += $"   |   Arama: \"{aramaMetni}\"";

                ws.Cell(2, 1).Value = filtreBilgi;
                ws.Cell(2, 1).Style.Font.Italic = true;
                ws.Range(2, 1, 2, colCount).Merge();

                // Sütun başlıkları (3. satır) — Türkçe HeaderText
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

                        // Zebra renklendirme
                        if (r % 2 == 1)
                            cell.Style.Fill.BackgroundColor = XLColor.FromArgb(242, 242, 242);
                    }

                    // Kalan izin uyarı renkleri
                    int kalanColIdx = gorunenKolonlar.FindIndex(k => k.DataPropertyName == "KalanIzin");
                    if (kalanColIdx >= 0 && satir["KalanIzin"] != DBNull.Value)
                    {
                        int kalan = Convert.ToInt32(satir["KalanIzin"]);
                        if (kalan <= 0)
                        {
                            for (int c = 0; c < colCount; c++)
                            {
                                ws.Cell(r + 4, c + 1).Style.Fill.BackgroundColor = XLColor.FromArgb(255, 205, 210);
                                ws.Cell(r + 4, c + 1).Style.Font.FontColor = XLColor.FromArgb(183, 28, 28);
                            }
                        }
                        else if (kalan <= 5)
                        {
                            for (int c = 0; c < colCount; c++)
                            {
                                ws.Cell(r + 4, c + 1).Style.Fill.BackgroundColor = XLColor.FromArgb(255, 236, 179);
                                ws.Cell(r + 4, c + 1).Style.Font.FontColor = XLColor.FromArgb(230, 81, 0);
                            }
                        }
                    }
                }

                ws.Columns().AdjustToContents();

                int sonSatir = gorunenSatirlar.Count + 3;
                ws.Range(3, 1, sonSatir, colCount).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Range(3, 1, sonSatir, colCount).Style.Border.InsideBorder = XLBorderStyleValues.Hair;

                wb.SaveAs(sfd.FileName);

                var ac = MessageBox.Show(
                    $"Personel listesi başarıyla aktarıldı.\n\n{sfd.FileName}\n\nDosyayı şimdi açmak ister misiniz?",
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

        // ══════════════════════════════════════════════════════════
        // DEPARTMAN RAPORU VE İŞLEM GEÇMİŞİ SEKMELERİ
        // ══════════════════════════════════════════════════════════

        // Tab değiştiğinde ilgili sekmeni yükle
        private async void tabMain_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (tabMain.SelectedTab == tabPageDepRapor)
                await DepartmanRaporuYukleAsync();
            else if (tabMain.SelectedTab == tabPageDepLog)
                await AmirLoglariniYukleAsync();
            else if (tabMain.SelectedTab == tabPageAmirOnay)
                await AmirOnayBekleyenleriYukleAsync();
        }

        private async void btnDepRaporYenile_Click(object? sender, EventArgs e)
        {
            await DepartmanRaporuYukleAsync();
        }

        private async void btnExcelAktar_Click(object? sender, EventArgs e)
        {
            // Veri yoksa veya dgv boşsa yükle
            if (dgvDepRapor.DataSource == null)
                await DepartmanRaporuYukleAsync();

            var dt = dgvDepRapor.DataSource as DataTable;
            if (dt == null || dt.Rows.Count == 0)
            {
                MessageBox.Show("Aktarılacak veri bulunamadı. Bugün departmanda izinli personel yok.",
                    "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using var saveDialog = new SaveFileDialog
            {
                Title = "Excel Dosyasını Kaydet",
                Filter = "Excel Dosyası (*.xlsx)|*.xlsx",
                FileName = $"DepartmanRaporu_{Session.CurrentDepartmentName}_{DateTime.Today:yyyyMMdd}.xlsx",
                DefaultExt = "xlsx"
            };

            if (saveDialog.ShowDialog() != DialogResult.OK) return;

            try
            {
                using var workbook = new XLWorkbook();
                var ws = workbook.Worksheets.Add("Departman Raporu");

                // Başlık satırı
                ws.Cell(1, 1).Value = $"Departman: {Session.CurrentDepartmentName}";
                ws.Cell(1, 1).Style.Font.Bold = true;
                ws.Cell(1, 1).Style.Font.FontSize = 13;
                ws.Range(1, 1, 1, dt.Columns.Count).Merge();

                ws.Cell(2, 1).Value = $"Rapor Tarihi: {DateTime.Today:dd.MM.yyyy}";
                ws.Cell(2, 1).Style.Font.Italic = true;
                ws.Range(2, 1, 2, dt.Columns.Count).Merge();

                // Sütun başlıkları (3. satır)
                for (int c = 0; c < dt.Columns.Count; c++)
                {
                    var cell = ws.Cell(3, c + 1);
                    cell.Value = dt.Columns[c].ColumnName;
                    cell.Style.Font.Bold = true;
                    cell.Style.Fill.BackgroundColor = XLColor.FromArgb(31, 73, 125);
                    cell.Style.Font.FontColor = XLColor.White;
                    cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                }

                // Veri satırları — DefaultView üzerinden (aktif RowFilter'a saygı gösterir)
                var gorunenSatirlar = dt.DefaultView.Cast<DataRowView>().Select(v => v.Row).ToList();
                for (int r = 0; r < gorunenSatirlar.Count; r++)
                {
                    DataRow satir = gorunenSatirlar[r];
                    for (int c = 0; c < dt.Columns.Count; c++)
                    {
                        var val = satir[c];
                        var cell = ws.Cell(r + 4, c + 1);
                        if (val is DateTime dt2)
                        {
                            cell.Value = dt2;
                            cell.Style.DateFormat.Format = "dd.MM.yyyy";
                        }
                        else
                        {
                            cell.Value = val?.ToString() ?? "";
                        }
                        // Satır rengi (zebra)
                        if (r % 2 == 1)
                            cell.Style.Fill.BackgroundColor = XLColor.FromArgb(242, 242, 242);
                    }
                }

                // Tüm sütunları otomatik genişlet
                ws.Columns().AdjustToContents();

                // Kenarlık
                var dataRange = ws.Range(3, 1, gorunenSatirlar.Count + 3, dt.Columns.Count);
                dataRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                dataRange.Style.Border.InsideBorder = XLBorderStyleValues.Hair;

                workbook.SaveAs(saveDialog.FileName);

                var sor = MessageBox.Show(
                    $"Excel dosyası başarıyla kaydedildi.\n\n{saveDialog.FileName}\n\nDosyayı şimdi açmak ister misiniz?",
                    "Kayıt Başarılı", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                if (sor == DialogResult.Yes)
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(saveDialog.FileName) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Excel dosyası oluşturulurken hata: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnDepLogYenile_Click(object? sender, EventArgs e)
        {
            await AmirLoglariniYukleAsync();
        }

        private async void btnLogFiltrele_Click(object? sender, EventArgs e)
        {
            await AmirLoglariniYukleAsync();
        }

        private async void btnLogExcel_Click(object? sender, EventArgs e)
        {
            var dt = dgvDepLog.DataSource as DataTable;
            if (dt == null || dt.Rows.Count == 0)
            {
                MessageBox.Show("Aktarılacak log kaydı bulunamadı.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DateTime? logBas = dtpLogBas.Checked ? dtpLogBas.Value : (DateTime?)null;
            DateTime? logBit = dtpLogBit.Checked ? dtpLogBit.Value : (DateTime?)null;

            string tarihAralik = (logBas.HasValue || logBit.HasValue)
                ? $"_{(logBas.HasValue ? logBas.Value.ToString("yyyyMMdd") : "")}-{(logBit.HasValue ? logBit.Value.ToString("yyyyMMdd") : "")}"
                : "";

            using var sfd = new SaveFileDialog
            {
                Title = "Excel Dosyasını Kaydet",
                Filter = "Excel Dosyası (*.xlsx)|*.xlsx",
                FileName = $"IslemGecmisi_{Session.CurrentDepartmentName}{tarihAralik}_{DateTime.Now:yyyyMMdd_HHmm}.xlsx",
                DefaultExt = "xlsx"
            };
            if (sfd.ShowDialog() != DialogResult.OK) return;

            try
            {
                using var wb = new XLWorkbook();
                var ws = wb.Worksheets.Add("İşlem Geçmişi");

                ws.Cell(1, 1).Value = $"Departman: {Session.CurrentDepartmentName}";
                ws.Cell(1, 1).Style.Font.Bold = true;
                ws.Cell(1, 1).Style.Font.FontSize = 13;
                ws.Range(1, 1, 1, 5).Merge();

                string aralikMetni = logBas.HasValue || logBit.HasValue
                    ? $"{(logBas.HasValue ? logBas.Value.ToString("dd.MM.yyyy") : "…")} – {(logBit.HasValue ? logBit.Value.ToString("dd.MM.yyyy") : "…")}"
                    : "Tüm kayıtlar";
                ws.Cell(2, 1).Value = $"Tarih Aralığı: {aralikMetni}";
                ws.Cell(2, 1).Style.Font.Italic = true;
                ws.Range(2, 1, 2, 5).Merge();

                string[] headers = { "İşlem Tipi", "İşlemi Yapan", "Etkilenen Tablo", "İşlem Detayı", "Tarih / Saat" };
                for (int i = 0; i < headers.Length; i++)
                {
                    var cell = ws.Cell(3, i + 1);
                    cell.Value = headers[i];
                    cell.Style.Font.Bold = true;
                    cell.Style.Fill.BackgroundColor = XLColor.FromHtml("#2E75B6");
                    cell.Style.Font.FontColor = XLColor.White;
                    cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                }

                // DefaultView üzerinden (aktif RowFilter'a saygı gösterir)
                var logSatirlar = dt.DefaultView.Cast<DataRowView>().Select(v => v.Row).ToList();
                for (int r = 0; r < logSatirlar.Count; r++)
                {
                    DataRow row = logSatirlar[r];
                    ws.Cell(r + 4, 1).Value = row["IslemTipi"]?.ToString() ?? "";
                    ws.Cell(r + 4, 2).Value = row["IslemYapan"]?.ToString() ?? "";
                    ws.Cell(r + 4, 3).Value = row["TargetTable"]?.ToString() ?? "";
                    ws.Cell(r + 4, 4).Value = row["Details"]?.ToString() ?? "";
                    if (row["LogDate"] != DBNull.Value && DateTime.TryParse(row["LogDate"].ToString(), out DateTime logDate))
                    {
                        ws.Cell(r + 4, 5).Value = logDate;
                        ws.Cell(r + 4, 5).Style.DateFormat.Format = "dd.MM.yyyy HH:mm:ss";
                    }
                    if (r % 2 == 0)
                        ws.Row(r + 4).Style.Fill.BackgroundColor = XLColor.FromHtml("#EBF3FB");
                }

                ws.Columns().AdjustToContents();
                if (ws.Column(4).Width > 60) ws.Column(4).Width = 60;
                int sonSatir = logSatirlar.Count + 3;
                ws.Range(3, 1, sonSatir, 5).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Range(3, 1, sonSatir, 5).Style.Border.InsideBorder = XLBorderStyleValues.Hair;

                wb.SaveAs(sfd.FileName);
                var ac = MessageBox.Show(
                    $"Log kayıtları başarıyla aktarıldı!\n{sfd.FileName}\n\nDosyayı şimdi açmak istiyor musunuz?",
                    "Excel Aktarımı", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (ac == DialogResult.Yes)
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(sfd.FileName) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Excel aktarımı sırasında hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Departman izin listesi — mevcut filtre değerleriyle
        private async Task DepartmanRaporuYukleAsync()
        {
            try
            {
                // Filtre değerlerini oku
                string? personelAd = string.IsNullOrWhiteSpace(txtPersonelFiltre.Text) ? null : txtPersonelFiltre.Text.Trim();
                string? durum = cmbDurumFiltre.SelectedIndex <= 0 ? null : DurumSqlKarsiligiAl(cmbDurumFiltre.SelectedItem?.ToString());
                DateTime? baslangic = dtpFiltreBas.Checked ? dtpFiltreBas.Value.Date : (DateTime?)null;
                DateTime? bitis = dtpFiltreEnd.Checked ? dtpFiltreEnd.Value.Date : (DateTime?)null;

                DataTable dt = await _raporService.DepartmanTumIzinleriGetirAsync(
                    Session.CurrentDepartmentID, personelAd, baslangic, bitis, durum);

                dgvDepRapor.DataSource = dt;

                if (dgvDepRapor.Columns.Contains("BaslangicTarihi"))
                {
                    dgvDepRapor.Columns["BaslangicTarihi"].HeaderText = "Başlangıç";
                    dgvDepRapor.Columns["BaslangicTarihi"].DefaultCellStyle.Format = "dd.MM.yyyy";
                    dgvDepRapor.Columns["BaslangicTarihi"].Width = 100;
                }
                if (dgvDepRapor.Columns.Contains("BitisTarihi"))
                {
                    dgvDepRapor.Columns["BitisTarihi"].HeaderText = "Bitiş";
                    dgvDepRapor.Columns["BitisTarihi"].DefaultCellStyle.Format = "dd.MM.yyyy";
                    dgvDepRapor.Columns["BitisTarihi"].Width = 100;
                }
                if (dgvDepRapor.Columns.Contains("GunSayisi"))
                {
                    dgvDepRapor.Columns["GunSayisi"].HeaderText = "Gün";
                    dgvDepRapor.Columns["GunSayisi"].Width = 55;
                    dgvDepRapor.Columns["GunSayisi"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
                if (dgvDepRapor.Columns.Contains("Durum"))
                {
                    dgvDepRapor.Columns["Durum"].Width = 100;
                }
                if (dgvDepRapor.Columns.Contains("TalepTarihi"))
                {
                    dgvDepRapor.Columns["TalepTarihi"].HeaderText = "Talep Tarihi";
                    dgvDepRapor.Columns["TalepTarihi"].DefaultCellStyle.Format = "dd.MM.yyyy";
                    dgvDepRapor.Columns["TalepTarihi"].Width = 100;
                }
                if (dgvDepRapor.Columns.Contains("YoneticiNotu"))
                {
                    dgvDepRapor.Columns["YoneticiNotu"].HeaderText = "Yönetici Notu";
                }

                dgvDepRapor.ReadOnly = true;
                dgvDepRapor.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgvDepRapor.MultiSelect = false;
                dgvDepRapor.AllowUserToAddRows = false;
                dgvDepRapor.CellFormatting -= dgvDepRapor_CellFormatting;
                dgvDepRapor.CellFormatting += dgvDepRapor_CellFormatting;

                int sayi = dt.Rows.Count;
                lblDepRaporBaslik.Text = sayi == 0
                    ? "Departman İzin Listesi  —  Kayıt bulunamadı"
                    : $"Departman İzin Listesi  —  {sayi} kayıt";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Departman raporu yüklenirken hata: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Combo'daki Türkçe görünen değeri DB'deki Status değerine çevir
        private static string? DurumSqlKarsiligiAl(string? gorunenDurum) => gorunenDurum switch
        {
            "Beklemede" => "Beklemede",
            "Onaylandi" => "Onaylandi",
            "Reddedildi" => "Reddedildi",
            "Iptal Edildi" => "Iptal Edildi",
            _ => null
        };

        // Durum bazlı satır renklendirmesi
        private void dgvDepRapor_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || !dgvDepRapor.Columns.Contains("Durum")) return;
            string durum = dgvDepRapor.Rows[e.RowIndex].Cells["Durum"].Value?.ToString() ?? "";
            (Color bg, Color fg) = durum switch
            {
                "Beklemede" => (Color.FromArgb(255, 249, 196), Color.FromArgb(130, 100, 0)),
                "Onaylandı" => (Color.FromArgb(200, 230, 201), Color.FromArgb(27, 94, 32)),
                "Reddedildi" => (Color.FromArgb(255, 205, 210), Color.FromArgb(183, 28, 28)),
                "İptal Edildi" => (Color.FromArgb(236, 239, 241), Color.FromArgb(96, 125, 139)),
                _ => (Color.White, Color.Black)
            };
            dgvDepRapor.Rows[e.RowIndex].DefaultCellStyle.BackColor = bg;
            dgvDepRapor.Rows[e.RowIndex].DefaultCellStyle.ForeColor = fg;
        }

        // Filtrele butonu
        private async void btnFiltrele_Click(object? sender, EventArgs e)
        {
            await DepartmanRaporuYukleAsync();
        }

        // Temizle butonu — filtreleri sıfırla ve tüm kayıtları göster
        private async void btnTemizle_Click(object? sender, EventArgs e)
        {
            txtPersonelFiltre.Text = "";
            cmbDurumFiltre.SelectedIndex = 0;
            dtpFiltreBas.Checked = false;
            dtpFiltreEnd.Checked = false;
            await DepartmanRaporuYukleAsync();
        }

        // Departman işlem geçmişi (audit log) — DTP filtreli
        private async Task AmirLoglariniYukleAsync()
        {
            try
            {
                DateTime? logBas = dtpLogBas.Checked ? dtpLogBas.Value : (DateTime?)null;
                DateTime? logBit = dtpLogBit.Checked ? dtpLogBit.Value : (DateTime?)null;
                DataTable dt = await _raporService.LoglariGetirAmirAsync(Session.CurrentDepartmentID, logBas, logBit);
                dgvDepLog.DataSource = dt;

                if (dgvDepLog.Columns.Contains("LogID"))
                    dgvDepLog.Columns["LogID"].Visible = false;

                if (dgvDepLog.Columns.Contains("IslemTipi"))
                {
                    dgvDepLog.Columns["IslemTipi"].HeaderText = "İşlem";
                    dgvDepLog.Columns["IslemTipi"].Width = 100;
                }
                if (dgvDepLog.Columns.Contains("IslemYapan"))
                {
                    dgvDepLog.Columns["IslemYapan"].HeaderText = "İşlemi Yapan";
                    dgvDepLog.Columns["IslemYapan"].Width = 160;
                }
                if (dgvDepLog.Columns.Contains("TargetTable"))
                {
                    dgvDepLog.Columns["TargetTable"].HeaderText = "Tablo";
                    dgvDepLog.Columns["TargetTable"].Width = 90;
                }
                if (dgvDepLog.Columns.Contains("Details"))
                {
                    dgvDepLog.Columns["Details"].HeaderText = "Detay";
                    dgvDepLog.Columns["Details"].Width = 400;
                }
                if (dgvDepLog.Columns.Contains("LogDate"))
                {
                    dgvDepLog.Columns["LogDate"].HeaderText = "Tarih / Saat";
                    dgvDepLog.Columns["LogDate"].DefaultCellStyle.Format = "dd.MM.yyyy HH:mm";
                    dgvDepLog.Columns["LogDate"].Width = 130;
                }

                dgvDepLog.ReadOnly = true;
                dgvDepLog.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgvDepLog.MultiSelect = false;
                dgvDepLog.AllowUserToAddRows = false;

                int sayi = dt.Rows.Count;
                lblDepLogBaslik.Text = sayi == 0
                    ? "Departman İşlem Geçmişi  —  Kayıt bulunamadı"
                    : $"Departman İşlem Geçmişi  —  {sayi} kayıt";
            }
            catch (Exception ex)
            {
                MessageBox.Show("İşlem geçmişi yüklenirken hata: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ── İzin Düzenle (servis katmanına delege) ────────────────
        private async void btnIzinDuzenle_Click(object? sender, EventArgs e)
        {
            // 1. Seçim Kontrolleri
            if (dgvIzinler.SelectedRows.Count == 0)
            {
                MessageBox.Show("Lütfen düzenlemek istediğiniz izni sağ taraftaki İzin Geçmişi listesinden seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (dgvPersoneller.SelectedRows.Count == 0)
            {
                MessageBox.Show("Lütfen önce sol listeden bir personel seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int seciliIzinID = Convert.ToInt32(dgvIzinler.SelectedRows[0].Cells["LeaveID"].Value);
            string durum = dgvIzinler.SelectedRows[0].Cells["Status"].Value?.ToString() ?? "";

            if (durum != "Beklemede")
            {
                MessageBox.Show("Sadece 'Beklemede' statüsündeki izin taleplerini düzenleyebilirsiniz!", "Yetki Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int seciliPersonelID = Convert.ToInt32(dgvPersoneller.SelectedRows[0].Cells["EmployeeID"].Value);
            string personelAd = dgvPersoneller.SelectedRows[0].Cells["FullName"].Value?.ToString() ?? "";
            int kalanIzin = Convert.ToInt32(dgvPersoneller.SelectedRows[0].Cells["KalanIzin"].Value ?? 0);

            DateTime yeniBaslangic = dtpBaslangic.Value.Date;
            DateTime yeniBitis = dtpBitis.Value.Date;

            // 2. Tarih ve Bakiye Kontrolleri (UI validasyonu)
            if (yeniBaslangic < DateTime.Today)
            {
                MessageBox.Show("Geçmiş bir tarihe izin güncelleyemezsiniz!", "Kural Hatası", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (yeniBitis < yeniBaslangic)
            {
                MessageBox.Show("İzin bitiş tarihi, başlangıç tarihinden önce olamaz!", "Kural Hatası", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int talepEdilenGun = DateHelper.IsGunuHesapla(yeniBaslangic, yeniBitis);

            if (talepEdilenGun == 0)
            {
                MessageBox.Show("Seçtiğiniz tarih aralığı sadece hafta sonuna denk geliyor.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (talepEdilenGun > kalanIzin)
            {
                MessageBox.Show($"Kalan izin ({kalanIzin} gün), talep edilen izni ({talepEdilenGun} gün) karşılamıyor!", "Bakiye Yetersiz", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult onay = MessageBox.Show(
                $"İzni {yeniBaslangic.ToShortDateString()} - {yeniBitis.ToShortDateString()} ({talepEdilenGun} gün) olarak güncellemek istiyor musunuz?",
                "Güncelleme Onayı", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (onay != DialogResult.Yes) return;

            try
            {
                var istek = new IzinGuncelleIstek(
                    EmployeeId: seciliPersonelID,
                    DepartmanId: Session.CurrentDepartmentID,
                    YapanId: Session.CurrentEmployeeID,
                    PersonelAd: personelAd,
                    YeniBaslangic: yeniBaslangic,
                    YeniBitis: yeniBitis,
                    KalanIzin: kalanIzin
                );

                IzinIslemSonucu sonuc = await _izinService.IzinGuncelleAmirAsync(seciliIzinID, istek);

                if (sonuc.Basarili)
                {
                    MessageBox.Show(sonuc.Mesaj, "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    await PersonelIzinleriniGetirAsync(seciliPersonelID);
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

        // ── Avans İzin Ekle ───────────────────────────────────────
        private async void btnAvansIzinEkle_Click(object? sender, EventArgs e)
        {
            if (dgvPersoneller.SelectedRows.Count == 0)
            {
                MessageBox.Show("Lütfen avans izin eklemek için sol taraftan bir personel seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int seciliPersonelID = Convert.ToInt32(dgvPersoneller.SelectedRows[0].Cells["EmployeeID"].Value);
            string personelAd = dgvPersoneller.SelectedRows[0].Cells["FullName"].Value.ToString();
            int kalanIzin = Convert.ToInt32(dgvPersoneller.SelectedRows[0].Cells["KalanIzin"].Value);

            DateTime baslangic = dtpBaslangic.Value.Date;
            DateTime bitis = dtpBitis.Value.Date;

            if (baslangic < DateTime.Today)
            {
                MessageBox.Show("Geçmiş bir tarihe izin giremezsiniz!", "Kural Hatası", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (bitis < baslangic)
            {
                MessageBox.Show("İzin bitiş tarihi, başlangıç tarihinden önce olamaz!", "Kural Hatası", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int talepEdilenGun = DateHelper.IsGunuHesapla(baslangic, bitis);
            if (talepEdilenGun == 0)
            {
                MessageBox.Show("Seçtiğiniz tarih aralığı sadece hafta sonuna denk geliyor. İzin yazılamaz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult onay = MessageBox.Show(
                $"{personelAd} için {baslangic.ToShortDateString()} - {bitis.ToShortDateString()} tarihleri arasına {talepEdilenGun} günlük AVANS izin talebi oluşturulacaktır.\n\nMevcut kalan izin: {kalanIzin} gün\n\nOnaylıyor musunuz?",
                "Avans İzin Talebi Onayı", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (onay != DialogResult.Yes) return;

            try
            {
                var istek = new IzinEkleIstek(
                    EmployeeId: seciliPersonelID,
                    DepartmanId: Session.CurrentDepartmentID,
                    AmirId: Session.CurrentEmployeeID,
                    PersonelAd: personelAd,
                    Baslangic: baslangic,
                    Bitis: bitis,
                    KalanIzin: kalanIzin
                );

                IzinIslemSonucu sonuc = await _izinService.AvansIzinEkleAsync(istek);

                if (sonuc.Basarili)
                {
                    MessageBox.Show(sonuc.Mesaj, "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    await PersonelleriGetirAsync();
                    await PersonelIzinleriniGetirAsync(seciliPersonelID);
                }
                else
                {
                    MessageBox.Show(sonuc.HataMesaji!, "İşlem Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("İşlem sırasında hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ── Amir Onayı Bekleyen Talepler ─────────────────────────

        private async Task AmirOnayBekleyenleriYukleAsync()
        {
            try
            {
                DataTable dt = await _izinService.AmirOnayindakileriGetirAsync(
                    Session.CurrentDepartmentID);

                dgvAmirOnay.DataSource = dt;

                if (dgvAmirOnay.Columns.Contains("LeaveID"))
                    dgvAmirOnay.Columns["LeaveID"].Visible = false;

                if (dgvAmirOnay.Columns.Contains("FullName"))
                {
                    dgvAmirOnay.Columns["FullName"].HeaderText = "Ad Soyad";
                    dgvAmirOnay.Columns["FullName"].Width = 160;
                }
                if (dgvAmirOnay.Columns.Contains("SicilNo"))
                {
                    dgvAmirOnay.Columns["SicilNo"].HeaderText = "Sicil No";
                    dgvAmirOnay.Columns["SicilNo"].Width = 80;
                }
                if (dgvAmirOnay.Columns.Contains("StartDate"))
                {
                    dgvAmirOnay.Columns["StartDate"].HeaderText = "Başlangıç";
                    dgvAmirOnay.Columns["StartDate"].DefaultCellStyle.Format = "dd.MM.yyyy";
                    dgvAmirOnay.Columns["StartDate"].Width = 95;
                }
                if (dgvAmirOnay.Columns.Contains("EndDate"))
                {
                    dgvAmirOnay.Columns["EndDate"].HeaderText = "Bitiş";
                    dgvAmirOnay.Columns["EndDate"].DefaultCellStyle.Format = "dd.MM.yyyy";
                    dgvAmirOnay.Columns["EndDate"].Width = 95;
                }
                if (dgvAmirOnay.Columns.Contains("DayCount"))
                {
                    dgvAmirOnay.Columns["DayCount"].HeaderText = "Gün";
                    dgvAmirOnay.Columns["DayCount"].Width = 50;
                    dgvAmirOnay.Columns["DayCount"].DefaultCellStyle.Alignment =
                        DataGridViewContentAlignment.MiddleCenter;
                }
                if (dgvAmirOnay.Columns.Contains("Status"))
                    dgvAmirOnay.Columns["Status"].Visible = false;

                if (dgvAmirOnay.Columns.Contains("CreatedDate"))
                {
                    dgvAmirOnay.Columns["CreatedDate"].HeaderText = "Talep Tarihi";
                    dgvAmirOnay.Columns["CreatedDate"].DefaultCellStyle.Format = "dd.MM.yyyy";
                    dgvAmirOnay.Columns["CreatedDate"].Width = 95;
                }
                if (dgvAmirOnay.Columns.Contains("LeaveType"))
                {
                    dgvAmirOnay.Columns["LeaveType"].HeaderText = "Tür";
                    dgvAmirOnay.Columns["LeaveType"].Width = 70;
                }

                dgvAmirOnay.ReadOnly = true;
                dgvAmirOnay.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgvAmirOnay.MultiSelect = false;
                dgvAmirOnay.AllowUserToAddRows = false;

                int sayi = dt.Rows.Count;
                lblAmirOnayBaslik.Text = sayi == 0
                    ? "Amir Onayı Bekleyen Talepler  —  Kayıt bulunamadı"
                    : $"Amir Onayı Bekleyen Talepler  —  {sayi} talep";

                btnAmirOnayla.Enabled = false;
                btnAmirReddet.Enabled = false;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Talepler yüklenirken hata: " + ex.Message,
                    "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvAmirOnay_SelectionChanged(object? sender, EventArgs e)
        {
            bool seciliVar = dgvAmirOnay.SelectedRows.Count > 0;
            btnAmirOnayla.Enabled = seciliVar;
            btnAmirReddet.Enabled = seciliVar;
        }

        private async void btnAmirOnayla_Click(object? sender, EventArgs e)
        {
            if (dgvAmirOnay.SelectedRows.Count == 0)
            {
                MessageBox.Show("Lütfen onaylamak istediğiniz talebi seçin.",
                    "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int leaveId = Convert.ToInt32(dgvAmirOnay.SelectedRows[0].Cells["LeaveID"].Value);
            string personelAd = dgvAmirOnay.SelectedRows[0].Cells["FullName"].Value?.ToString() ?? "";
            string baslangic = Convert.ToDateTime(dgvAmirOnay.SelectedRows[0].Cells["StartDate"].Value).ToShortDateString();
            string bitis = Convert.ToDateTime(dgvAmirOnay.SelectedRows[0].Cells["EndDate"].Value).ToShortDateString();
            int gun = Convert.ToInt32(dgvAmirOnay.SelectedRows[0].Cells["DayCount"].Value);

            var onay = MessageBox.Show(
                $"{personelAd} adlı personelin {baslangic} - {bitis} tarihleri arası {gun} günlük izin talebini onaylamak istiyor musunuz?\n\nOnaylandıktan sonra talep Yönetici onayına sunulacaktır.",
                "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (onay != DialogResult.Yes) return;

            try
            {
                var sonuc = await _izinService.AmirOnaylaAsync(
                    leaveId, Session.CurrentEmployeeID, personelAd);

                if (sonuc.Basarili)
                {
                    MessageBox.Show(sonuc.Mesaj, "Başarılı",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    await AmirOnayBekleyenleriYukleAsync();
                }
                else
                {
                    MessageBox.Show(sonuc.HataMesaji!, "Hata",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message, "Hata",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnAmirReddet_Click(object? sender, EventArgs e)
        {
            if (dgvAmirOnay.SelectedRows.Count == 0)
            {
                MessageBox.Show("Lütfen reddetmek istediğiniz talebi seçin.",
                    "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int leaveId = Convert.ToInt32(dgvAmirOnay.SelectedRows[0].Cells["LeaveID"].Value);
            string personelAd = dgvAmirOnay.SelectedRows[0].Cells["FullName"].Value?.ToString() ?? "";

            string redSebebi = Microsoft.VisualBasic.Interaction.InputBox(
                "Red sebebini giriniz:",
                "Red Gerekçesi",
                "");

            if (string.IsNullOrWhiteSpace(redSebebi))
            {
                MessageBox.Show("Red sebebi boş olamaz.", "Uyarı",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var sonuc = await _izinService.AmirReddetAsync(
                    leaveId, Session.CurrentEmployeeID, redSebebi, personelAd);

                if (sonuc.Basarili)
                {
                    MessageBox.Show(sonuc.Mesaj, "Başarılı",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    await AmirOnayBekleyenleriYukleAsync();
                }
                else
                {
                    MessageBox.Show(sonuc.HataMesaji!, "Hata",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message, "Hata",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnAmirOnayla_Click_1(object? sender, EventArgs e)
        {
            if (dgvAmirOnay.SelectedRows.Count == 0)
            {
                MessageBox.Show("Lütfen onaylamak istediğiniz talebi seçin.",
                    "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int leaveId = Convert.ToInt32(dgvAmirOnay.SelectedRows[0].Cells["LeaveID"].Value);
            string personelAd = dgvAmirOnay.SelectedRows[0].Cells["FullName"].Value?.ToString() ?? "";
            string baslangic = Convert.ToDateTime(dgvAmirOnay.SelectedRows[0].Cells["StartDate"].Value).ToShortDateString();
            string bitis = Convert.ToDateTime(dgvAmirOnay.SelectedRows[0].Cells["EndDate"].Value).ToShortDateString();
            int gun = Convert.ToInt32(dgvAmirOnay.SelectedRows[0].Cells["DayCount"].Value);

            var onay = MessageBox.Show(
                $"{personelAd} adlı personelin {baslangic} - {bitis} tarihleri arası {gun} günlük izin talebini onaylamak istiyor musunuz?\n\nOnaylandıktan sonra talep Yönetici onayına sunulacaktır.",
                "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (onay != DialogResult.Yes) return;

            try
            {
                var sonuc = await _izinService.AmirOnaylaAsync(
                    leaveId, Session.CurrentEmployeeID, personelAd);

                if (sonuc.Basarili)
                {
                    MessageBox.Show(sonuc.Mesaj, "Başarılı",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    await AmirOnayBekleyenleriYukleAsync();
                }
                else
                {
                    MessageBox.Show(sonuc.HataMesaji!, "Hata",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message, "Hata",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnAmirReddet_Click_1(object? sender, EventArgs e)
        {
            if (dgvAmirOnay.SelectedRows.Count == 0)
            {
                MessageBox.Show("Lütfen reddetmek istediğiniz talebi seçin.",
                    "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int leaveId = Convert.ToInt32(dgvAmirOnay.SelectedRows[0].Cells["LeaveID"].Value);
            string personelAd = dgvAmirOnay.SelectedRows[0].Cells["FullName"].Value?.ToString() ?? "";

            string redSebebi = Microsoft.VisualBasic.Interaction.InputBox(
                "Red sebebini giriniz:",
                "Red Gerekçesi",
                "");

            if (string.IsNullOrWhiteSpace(redSebebi))
            {
                MessageBox.Show("Red sebebi boş olamaz.", "Uyarı",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var sonuc = await _izinService.AmirReddetAsync(
                    leaveId, Session.CurrentEmployeeID, redSebebi, personelAd);

                if (sonuc.Basarili)
                {
                    MessageBox.Show(sonuc.Mesaj, "Başarılı",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    await AmirOnayBekleyenleriYukleAsync();
                }
                else
                {
                    MessageBox.Show(sonuc.HataMesaji!, "Hata",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message, "Hata",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
