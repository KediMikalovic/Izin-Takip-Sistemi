using System;
using System.Data;
using System.IO;
using System.Windows.Forms;
using ClosedXML.Excel;
using IzinTakip.Core.Abstractions;

namespace IzinTakipSistemi
{
    public partial class LogForm : Form
    {
        private readonly IRaporService _raporService;

        public LogForm(IRaporService raporService)
        {
            _raporService = raporService;
            InitializeComponent();

            this.Load += LogForm_Load;
            btnYenile.Click += btnYenile_Click;
        }

        private async void LogForm_Load(object? sender, EventArgs e)
        {
            this.Text = "Sistem Hareketleri (Audit Logs)";

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

            await LoglariGetirAsync();
        }

        private async void btnYenile_Click(object? sender, EventArgs e)
        {
            await LoglariGetirAsync();
        }

        private void btnExcelAktar_Click(object? sender, EventArgs e)
        {
            if (dgvLogs.Rows.Count == 0)
            {
                MessageBox.Show("Aktarılacak log kaydı bulunamadı.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Excel Dosyası (*.xlsx)|*.xlsx";
            sfd.FileName = $"SistemLoglari_{DateTime.Now:yyyyMMdd_HHmm}.xlsx";
            sfd.Title = "Excel Dosyasını Kaydet";

            if (sfd.ShowDialog() != DialogResult.OK) return;

            try
            {
                using XLWorkbook wb = new XLWorkbook();
                IXLWorksheet ws = wb.Worksheets.Add("Sistem Logları");

                // --- Başlık Satırı ---
                string[] headers = { "İşlem Tipi", "İşlemi Yapan", "Etkilenen Tablo", "İşlem Detayı", "Tarih / Saat" };
                for (int i = 0; i < headers.Length; i++)
                {
                    var cell = ws.Cell(1, i + 1);
                    cell.Value = headers[i];
                    cell.Style.Font.Bold = true;
                    cell.Style.Fill.BackgroundColor = XLColor.FromHtml("#2E75B6");
                    cell.Style.Font.FontColor = XLColor.White;
                    cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                }

                // --- Veri Satırları ---
                DataTable? dt = dgvLogs.DataSource as DataTable;
                if (dt != null)
                {
                    for (int r = 0; r < dt.Rows.Count; r++)
                    {
                        DataRow row = dt.Rows[r];
                        ws.Cell(r + 2, 1).Value = row["IslemTipi"]?.ToString() ?? "";
                        ws.Cell(r + 2, 2).Value = row["IslemYapan"]?.ToString() ?? "";
                        ws.Cell(r + 2, 3).Value = row["TargetTable"]?.ToString() ?? "";
                        ws.Cell(r + 2, 4).Value = row["Details"]?.ToString() ?? "";

                        if (row["LogDate"] != DBNull.Value && DateTime.TryParse(row["LogDate"].ToString(), out DateTime logDate))
                        {
                            ws.Cell(r + 2, 5).Value = logDate;
                            ws.Cell(r + 2, 5).Style.DateFormat.Format = "dd.MM.yyyy HH:mm:ss";
                        }

                        if (r % 2 == 0)
                        {
                            ws.Row(r + 2).Style.Fill.BackgroundColor = XLColor.FromHtml("#EBF3FB");
                        }
                    }
                }

                ws.Columns().AdjustToContents();
                if (ws.Column(4).Width > 60) ws.Column(4).Width = 60;

                int toplamSatir = (dt?.Rows.Count ?? 0) + 1;
                ws.Range(1, 1, toplamSatir, 5).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Range(1, 1, toplamSatir, 5).Style.Border.InsideBorder = XLBorderStyleValues.Hair;

                wb.SaveAs(sfd.FileName);
                MessageBox.Show($"Loglar başarıyla aktarıldı!\n{sfd.FileName}", "Excel Aktarımı", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Excel aktarımı sırasında hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task LoglariGetirAsync()
        {
            try
            {
                DataTable dt = await _raporService.LoglariGetirAsync();
                dgvLogs.DataSource = dt;

                // --- Grid Görünüm Ayarları ---
                if (dgvLogs.Columns.Contains("LogID")) dgvLogs.Columns["LogID"].Visible = false;

                dgvLogs.Columns["IslemTipi"].HeaderText = "İşlem Tipi";
                dgvLogs.Columns["IslemYapan"].HeaderText = "İşlemi Yapan";
                dgvLogs.Columns["TargetTable"].HeaderText = "Etkilenen Tablo";
                dgvLogs.Columns["Details"].HeaderText = "İşlem Detayı";
                dgvLogs.Columns["LogDate"].HeaderText = "Tarih / Saat";

                dgvLogs.Columns["Details"].Width = 350;

                dgvLogs.ReadOnly = true;
                dgvLogs.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgvLogs.MultiSelect = false;
                dgvLogs.AllowUserToAddRows = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Loglar yüklenirken bir hata oluştu: " + ex.Message, "Veri Tabanı Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
