using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ClosedXML.Excel;
using IzinTakip.Core.Abstractions;

namespace IzinTakipSistemi
{
    public partial class RaporForm : Form
    {
        private readonly IRaporService _raporService;
        private DataTable? _personelTamListe; // filtre için master kopya

        public RaporForm(IRaporService raporService)
        {
            _raporService = raporService;
            InitializeComponent();
            this.Load += RaporForm_Load;
        }

        private async void RaporForm_Load(object? sender, EventArgs e)
        {
            this.Text = "Yönetici Raporları ve İstatistikler (Dashboard)";

            // Logo ve ikon
            try
            {
                string iconPath = Path.Combine(AppContext.BaseDirectory, "BossaIconFormat.ico");
                if (File.Exists(iconPath))
                    this.Icon = new Icon(iconPath);
            }
            catch { /* İkon yüklenemezse varsayılan kullan */ }

            picLogoAlan.Image = Properties.Resources.görsel1;
            picLogoAlan.BackColor = Color.White;

            // Departman ve aylık veriler hem grid hem grafik tarafından kullanılır;
            // tek seferinde çekilip her ikisine aktarılır
            var departmanTask = _raporService.DepartmanIstatistikleriniGetirAsync();
            var aylikTask     = _raporService.AylikYogunluguGetirAsync();

            // Tüm veri sorgularını paralel çalıştır — UI donmadan yüklenir
            await Task.WhenAll(
                departmanTask,
                aylikTask,
                EnCokIzinAlanlariGetirAsync(),
                AktifIzinlileriGetirAsync(),
                PersonelDetaylariniGetirAsync()
            );

            // Grid bağlama
            DataTable dtDepartman = departmanTask.Result;
            dgvDepartman.DataSource = dtDepartman;
            AyarlaGrid(dgvDepartman);

            DataTable dtAylik = aylikTask.Result;
            dgvAylik.DataSource = dtAylik;
            if (dgvAylik.Columns.Contains("AyNumarasi")) dgvAylik.Columns["AyNumarasi"].Visible = false;
            dgvAylik.Columns["Ay"].DisplayIndex = 0;
            AyarlaGrid(dgvAylik);

            // Grafikler önceden çekilen veriyle çizilir — ekstra servis çağrısı yok
            CizDepartmanGrafigi(dtDepartman);
            CizAylikGrafigi(dtAylik);
        }

        // 5. Personel Listesi — işe giriş, hak ediş, kalan izin bilgileriyle
        private async Task PersonelDetaylariniGetirAsync()
        {
            try
            {
                DataTable dt = await _raporService.PersonelDetaylariniGetirAsync();

                // Master listeyi sakla — filtreler bu kopya üzerinde çalışır
                _personelTamListe = dt;

                // Departman ComboBox'ını doldur (benzersiz, sıralı)
                var departmanlar = dt.AsEnumerable()
                    .Select(r => r["Departman"].ToString()!)
                    .Distinct()
                    .OrderBy(d => d)
                    .ToList();
                departmanlar.Insert(0, "Tüm Departmanlar");

                cmbPersonelDepartman.Items.Clear();
                foreach (var dep in departmanlar)
                    cmbPersonelDepartman.Items.Add(dep);
                cmbPersonelDepartman.SelectedIndex = 0;

                dgvPersonelDetay.DataSource = dt;
                AyarlaGrid(dgvPersonelDetay);

                // İlk yükleme filtresi (Durum=Aktif varsayılan)
                PersonelFiltreUygula();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Personel listesi yüklenirken hata: " + ex.Message,
                    "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 3. En Çok İzin Alan 5 Çalışan
        private async Task EnCokIzinAlanlariGetirAsync()
        {
            dgvEnCokAlanlar.DataSource = await _raporService.EnCokIzinAlanlariGetirAsync();
            AyarlaGrid(dgvEnCokAlanlar);
        }

        // 4. Bugün İzinli Olanlar
        private async Task AktifIzinlileriGetirAsync()
        {
            dgvAktifIzinliler.DataSource = await _raporService.AktifIzinlileriGetirAsync();
            AyarlaGrid(dgvAktifIzinliler);
        }

        // ── Personel Listesi Filtreleme ────────────────────────────

        private void txtPersonelAra_TextChanged(object? sender, EventArgs e)        => PersonelFiltreUygula();
        private void cmbPersonelDepartman_SelectedIndexChanged(object? sender, EventArgs e) => PersonelFiltreUygula();
        private void cmbPersonelDurum_SelectedIndexChanged(object? sender, EventArgs e)    => PersonelFiltreUygula();
        private void chkSadeceDusukIzin_CheckedChanged(object? sender, EventArgs e)       => PersonelFiltreUygula();

        // DataView.RowFilter için özel karakterleri güvenli hale getirir
        private static string RowFilterKacir(string deger) =>
            deger.Replace("'", "''")
                 .Replace("[", "[[]")
                 .Replace("]", "[]]")
                 .Replace("%", "[%]")
                 .Replace("*", "[*]");

        private void PersonelFiltreUygula()
        {
            if (_personelTamListe == null) return;

            var kosullar = new List<string>();

            // 1. İsim / Sicil No araması
            string aranan = RowFilterKacir(txtPersonelAra.Text.Trim());
            if (!string.IsNullOrEmpty(aranan))
                kosullar.Add($"([Ad Soyad] LIKE '%{aranan}%' OR [Sicil No] LIKE '%{aranan}%')");

            // 2. Departman filtresi
            string seciliDep = cmbPersonelDepartman.SelectedItem?.ToString() ?? "Tüm Departmanlar";
            if (seciliDep != "Tüm Departmanlar")
                kosullar.Add($"Departman = '{RowFilterKacir(seciliDep)}'");

            // 3. Durum filtresi (Aktif / Pasif / Tümü)
            string seciliDurum = cmbPersonelDurum.SelectedItem?.ToString() ?? "Tümü";
            if (seciliDurum != "Tümü")
                kosullar.Add($"Durum = '{RowFilterKacir(seciliDurum)}'");

            // 4. Kritik bakiye filtresi (kalan izin ≤ 5)
            if (chkSadeceDusukIzin.Checked)
                kosullar.Add("[Kalan İzin] <= 5");

            _personelTamListe.DefaultView.RowFilter = kosullar.Count > 0
                ? string.Join(" AND ", kosullar)
                : "";

            int gorunen = _personelTamListe.DefaultView.Count;
            int toplam  = _personelTamListe.Rows.Count;
            lblPersonelSonuc.Text = gorunen == toplam
                ? $"{toplam} personel listeleniyor"
                : $"{gorunen} / {toplam} personel gösteriliyor";
        }

        // ── Tarih Filtresi ─────────────────────────────────────────
        private async void btnRaporFiltrele_Click(object? sender, EventArgs e)
        {
            DateTime? bas = dtpRaporBas.Checked ? dtpRaporBas.Value : (DateTime?)null;
            DateTime? bit = dtpRaporBit.Checked ? dtpRaporBit.Value : (DateTime?)null;

            try
            {
                var departmanTask = _raporService.DepartmanIstatistikleriniGetirAsync(bas, bit);
                var aylikTask     = _raporService.AylikYogunluguGetirAsync(bas, bit);
                var enCokTask     = _raporService.EnCokIzinAlanlariGetirAsync(bas, bit);
                await Task.WhenAll(departmanTask, aylikTask, enCokTask);

                DataTable dtDep = departmanTask.Result;
                dgvDepartman.DataSource = dtDep;
                AyarlaGrid(dgvDepartman);
                CizDepartmanGrafigi(dtDep);

                DataTable dtAy = aylikTask.Result;
                dgvAylik.DataSource = dtAy;
                if (dgvAylik.Columns.Contains("AyNumarasi")) dgvAylik.Columns["AyNumarasi"].Visible = false;
                if (dgvAylik.Columns.Contains("Ay")) dgvAylik.Columns["Ay"].DisplayIndex = 0;
                AyarlaGrid(dgvAylik);
                CizAylikGrafigi(dtAy);

                dgvEnCokAlanlar.DataSource = enCokTask.Result;
                AyarlaGrid(dgvEnCokAlanlar);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Filtreleme sırasında hata oluştu: " + ex.Message,
                    "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ── Excel Aktarım ──────────────────────────────────────────
        private void btnRaporExcel_Click(object? sender, EventArgs e)
        {
            // Hangi sekme aktif? O sekmedeki DGV'yi bul
            TabPage seciliSayfa = tabControl1.SelectedTab;
            DataGridView? dgv = seciliSayfa?.Controls.OfType<DataGridView>().FirstOrDefault();
            if (dgv == null || dgv.Rows.Count == 0)
            {
                MessageBox.Show("Aktarılacak veri bulunamadı.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter   = "Excel Dosyası (*.xlsx)|*.xlsx";
            sfd.FileName = $"Rapor_{seciliSayfa!.Text}_{DateTime.Now:yyyyMMdd_HHmm}.xlsx";
            sfd.Title    = "Excel Dosyasını Kaydet";
            if (sfd.ShowDialog() != DialogResult.OK) return;

            try
            {
                using XLWorkbook wb = new XLWorkbook();
                IXLWorksheet ws = wb.Worksheets.Add(seciliSayfa.Text.Length > 30
                    ? seciliSayfa.Text.Substring(0, 30) : seciliSayfa.Text);

                // Başlık satırı — excelBaslik sayacı gizli sütunları atlayarak hizalamayı korur
                int excelBaslik = 1;
                for (int c = 0; c < dgv.Columns.Count; c++)
                {
                    if (!dgv.Columns[c].Visible) continue;
                    var cell = ws.Cell(1, excelBaslik++);
                    cell.Value = dgv.Columns[c].HeaderText;
                    cell.Style.Font.Bold = true;
                    cell.Style.Fill.BackgroundColor = XLColor.FromHtml("#2E75B6");
                    cell.Style.Font.FontColor = XLColor.White;
                    cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                }

                // Veri satırları — RowFilter aktifse yalnızca görünen satırlar aktarılır
                int excelSatir = 2;
                DataTable? dt = dgv.DataSource as DataTable;
                if (dt != null)
                {
                    // DefaultView üzerinden iterasyon: aktif RowFilter'ı dikkate alır
                    var gorunenSatirlar = dt.DefaultView.Cast<DataRowView>().Select(v => v.Row).ToList();
                    for (int r = 0; r < gorunenSatirlar.Count; r++)
                    {
                        DataRow row = gorunenSatirlar[r];
                        int excelSutun = 1;
                        foreach (DataGridViewColumn col in dgv.Columns)
                        {
                            if (!col.Visible) continue;
                            object? val = row[col.DataPropertyName ?? col.Name];
                            var cell = ws.Cell(excelSatir, excelSutun);
                            if (val != null && val != DBNull.Value)
                            {
                                if (val is DateTime dt2)
                                {
                                    cell.Value = dt2;
                                    cell.Style.DateFormat.Format = "dd.MM.yyyy";
                                }
                                else
                                    cell.Value = val.ToString();
                            }
                            if (r % 2 == 0)
                                cell.Style.Fill.BackgroundColor = XLColor.FromHtml("#EBF3FB");
                            excelSutun++;
                        }
                        excelSatir++;
                    }
                }

                ws.Columns().AdjustToContents();
                int toplamSatir = excelSatir - 1;
                int toplamSutun = dgv.Columns.Cast<DataGridViewColumn>().Count(c => c.Visible);
                if (toplamSatir >= 1 && toplamSutun >= 1)
                {
                    ws.Range(1, 1, toplamSatir, toplamSutun).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    ws.Range(1, 1, toplamSatir, toplamSutun).Style.Border.InsideBorder  = XLBorderStyleValues.Hair;
                }

                wb.SaveAs(sfd.FileName);

                var ac = MessageBox.Show(
                    $"Rapor başarıyla aktarıldı!\n{sfd.FileName}\n\nDosyayı şimdi açmak istiyor musunuz?",
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

        // Tüm Grid'leri aynı temiz görünüme kavuşturmak için yardımcı metot (DRY)
        private void AyarlaGrid(DataGridView dgv)
        {
            dgv.ReadOnly = true;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.MultiSelect = false;
            dgv.AllowUserToAddRows = false;
            dgv.RowHeadersVisible = false;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        // ─────────────────────────────────────────────────────────
        //  GRAFİK 1 — Departman Bazlı İzin Günleri (Sütun Grafik)
        // ─────────────────────────────────────────────────────────
        private void CizDepartmanGrafigi(DataTable dt)
        {
            try
            {
                if (dt.Rows.Count == 0) return;

                int w = picGrafikDepartman.Width;
                int h = picGrafikDepartman.Height;
                Bitmap? bmp = null;
                try
                {
                    bmp = new Bitmap(w, h);
                    using Graphics g = Graphics.FromImage(bmp);
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    g.Clear(Color.White);

                    int sol = 55, sag = 15, ust = 20, alt = 60;
                    int grafikW = w - sol - sag;
                    int grafikH = h - ust - alt;

                    int maxVal = 1;
                    foreach (DataRow row in dt.Rows)
                        maxVal = Math.Max(maxVal, Convert.ToInt32(row["ToplamIzinGunu"]));

                    int adim = 5;
                    using Pen gridPen = new Pen(Color.LightGray, 1f) { DashStyle = DashStyle.Dash };
                    using Font kucukFont = new Font("Segoe UI", 7f);
                    using Brush textBrush = new SolidBrush(Color.FromArgb(60, 60, 60));
                    for (int i = 0; i <= adim; i++)
                    {
                        int deger = (int)Math.Round((double)maxVal * i / adim);
                        int yPos = ust + grafikH - (int)((double)grafikH * i / adim);
                        g.DrawLine(gridPen, sol, yPos, sol + grafikW, yPos);
                        g.DrawString(deger.ToString(), kucukFont, textBrush, 2, yPos - 7);
                    }

                    Color[] renkler = {
                        Color.FromArgb(46, 117, 182),
                        Color.FromArgb(237, 125, 49),
                        Color.FromArgb(112, 173, 71),
                        Color.FromArgb(255, 192, 0),
                        Color.FromArgb(192, 0, 0),
                        Color.FromArgb(0, 176, 240)
                    };

                    int n = dt.Rows.Count;
                    float cubukGenislik = (float)grafikW / (n * 1.6f);
                    float bosluk = cubukGenislik * 0.6f;
                    float baslangicX = sol + bosluk / 2f;

                    for (int i = 0; i < n; i++)
                    {
                        int deger = Convert.ToInt32(dt.Rows[i]["ToplamIzinGunu"]);
                        string etiket = dt.Rows[i]["Departman"].ToString() ?? "";
                        Color renk = renkler[i % renkler.Length];

                        float cubukX = baslangicX + i * (cubukGenislik + bosluk);
                        float cubukH = (float)grafikH * deger / maxVal;
                        if (cubukH < 1f) cubukH = 1f;
                        float cubukY = ust + grafikH - cubukH;

                        using SolidBrush golge = new SolidBrush(Color.FromArgb(30, 0, 0, 0));
                        g.FillRectangle(golge, cubukX + 3, cubukY + 3, cubukGenislik, cubukH);

                        var gradRect = new RectangleF(cubukX, cubukY, Math.Max(cubukGenislik, 1f), Math.Max(cubukH, 1f));
                        using LinearGradientBrush gradBrush = new LinearGradientBrush(
                            gradRect,
                            renk, Color.FromArgb(renk.R / 2, renk.G / 2, renk.B / 2),
                            LinearGradientMode.Vertical);
                        g.FillRectangle(gradBrush, cubukX, cubukY, cubukGenislik, cubukH);

                        using Font degerFont = new Font("Segoe UI", 7.5f, FontStyle.Bold);
                        string degerStr = deger.ToString();
                        SizeF degerBoyut = g.MeasureString(degerStr, degerFont);
                        g.DrawString(degerStr, degerFont, Brushes.Black,
                            cubukX + cubukGenislik / 2f - degerBoyut.Width / 2f, cubukY - degerBoyut.Height - 1);

                        if (etiket.Length > 8) etiket = etiket.Substring(0, 7) + "..";
                        SizeF etiketBoyut = g.MeasureString(etiket, kucukFont);
                        float etiketX = cubukX + cubukGenislik / 2f - etiketBoyut.Width / 2f;
                        g.DrawString(etiket, kucukFont, textBrush, etiketX, ust + grafikH + 5);
                    }

                    using Pen eksenPen = new Pen(Color.FromArgb(80, 80, 80), 1.5f);
                    g.DrawLine(eksenPen, sol, ust, sol, ust + grafikH);
                    g.DrawLine(eksenPen, sol, ust + grafikH, sol + grafikW, ust + grafikH);

                    picGrafikDepartman.Image?.Dispose();
                    picGrafikDepartman.Image = bmp;
                    bmp = null; // sahiplik PictureBox'a geçti
                }
                finally
                {
                    bmp?.Dispose(); // yalnızca hata durumunda dispose edilir
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Departman grafiği çizilemedi: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // ─────────────────────────────────────────────────────────
        //  GRAFİK 2 — Aylık İzin Yoğunluğu (Çizgi Grafik)
        // ─────────────────────────────────────────────────────────
        private void CizAylikGrafigi(DataTable dt)
        {
            try
            {
                // 12 aylık tam dizi oluştur (veri yoksa 0)
                int[] aylikVeri = new int[12];
                foreach (DataRow row in dt.Rows)
                {
                    int ayNo = Convert.ToInt32(row["AyNumarasi"]);
                    if (ayNo >= 1 && ayNo <= 12)
                        aylikVeri[ayNo - 1] = Convert.ToInt32(row["ToplamGun"]);
                }

                string[] ayIsimleri = { "Oca", "Şub", "Mar", "Nis", "May", "Haz",
                                        "Tem", "Ağu", "Eyl", "Eki", "Kas", "Ara" };

                int w = picGrafikAylik.Width;
                int h = picGrafikAylik.Height;
                Bitmap? bmp = null;
                try
                {
                    bmp = new Bitmap(w, h);
                    using Graphics g = Graphics.FromImage(bmp);
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    g.Clear(Color.White);

                    int sol = 50, sag = 15, ust = 20, alt = 50;
                    int grafikW = w - sol - sag;
                    int grafikH = h - ust - alt;

                    int maxVal = 1;
                    foreach (int v in aylikVeri) maxVal = Math.Max(maxVal, v);
                    if (maxVal == 0) maxVal = 1;

                    using Pen gridPen = new Pen(Color.LightGray, 1f) { DashStyle = DashStyle.Dash };
                    using Font kucukFont = new Font("Segoe UI", 7f);
                    using Brush textBrush = new SolidBrush(Color.FromArgb(60, 60, 60));

                    int adim = 5;
                    for (int i = 0; i <= adim; i++)
                    {
                        int deger = (int)Math.Round((double)maxVal * i / adim);
                        int yPos = ust + grafikH - (int)((double)grafikH * i / adim);
                        g.DrawLine(gridPen, sol, yPos, sol + grafikW, yPos);
                        g.DrawString(deger.ToString(), kucukFont, textBrush, 2, yPos - 7);
                    }

                    float xAdim = (float)grafikW / 11f;
                    PointF[] noktalar = new PointF[12];
                    for (int i = 0; i < 12; i++)
                    {
                        float x = sol + i * xAdim;
                        float y = ust + grafikH - (float)grafikH * aylikVeri[i] / maxVal;
                        noktalar[i] = new PointF(x, y);
                    }

                    PointF[] dolguNoktalar = new PointF[14];
                    dolguNoktalar[0] = new PointF(sol, ust + grafikH);
                    for (int i = 0; i < 12; i++) dolguNoktalar[i + 1] = noktalar[i];
                    dolguNoktalar[13] = new PointF(sol + grafikW, ust + grafikH);
                    using SolidBrush dolguFirca = new SolidBrush(Color.FromArgb(50, 46, 117, 182));
                    g.FillPolygon(dolguFirca, dolguNoktalar);

                    using Pen cizgiPen = new Pen(Color.FromArgb(46, 117, 182), 2.5f);
                    g.DrawLines(cizgiPen, noktalar);

                    for (int i = 0; i < 12; i++)
                    {
                        using SolidBrush noktaFirca = new SolidBrush(Color.FromArgb(46, 117, 182));
                        g.FillEllipse(noktaFirca, noktalar[i].X - 4, noktalar[i].Y - 4, 8, 8);
                        g.DrawEllipse(Pens.White, noktalar[i].X - 3, noktalar[i].Y - 3, 6, 6);

                        if (aylikVeri[i] > 0)
                        {
                            using Font degerFont = new Font("Segoe UI", 7f, FontStyle.Bold);
                            string degerStr = aylikVeri[i].ToString();
                            SizeF boyut = g.MeasureString(degerStr, degerFont);
                            g.DrawString(degerStr, degerFont, Brushes.DarkBlue,
                                noktalar[i].X - boyut.Width / 2f, noktalar[i].Y - boyut.Height - 3);
                        }

                        SizeF ayBoyut = g.MeasureString(ayIsimleri[i], kucukFont);
                        g.DrawString(ayIsimleri[i], kucukFont, textBrush,
                            noktalar[i].X - ayBoyut.Width / 2f, ust + grafikH + 5);
                    }

                    using Pen eksenPen = new Pen(Color.FromArgb(80, 80, 80), 1.5f);
                    g.DrawLine(eksenPen, sol, ust, sol, ust + grafikH);
                    g.DrawLine(eksenPen, sol, ust + grafikH, sol + grafikW, ust + grafikH);

                    using Font baslikFont = new Font("Segoe UI", 8f, FontStyle.Italic);
                    g.DrawString($"{DateTime.Now.Year} Yılı", baslikFont, textBrush, sol + grafikW - 50, ust);

                    picGrafikAylik.Image?.Dispose();
                    picGrafikAylik.Image = bmp;
                    bmp = null; // sahiplik PictureBox'a geçti
                }
                finally
                {
                    bmp?.Dispose(); // yalnızca hata durumunda dispose edilir
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Aylık grafik çizilemedi: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
