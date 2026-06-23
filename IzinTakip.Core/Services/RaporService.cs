using System.Data;
using Microsoft.Data.SqlClient;
using IzinTakip.Core.Abstractions;
using IzinTakip.Core.Helpers;

namespace IzinTakip.Core.Services
{
    /// <summary>
    /// Rapor ve istatistik sorguları.
    /// RaporForm + LogForm'daki tüm SQL burada merkezileştirildi.
    /// </summary>
    public class RaporService : IRaporService
    {
        private readonly IDbHelper _db;

        public RaporService(IDbHelper db)
        {
            _db = db;
        }

        // ── 1. Departman Bazlı Toplam İzin Günleri ─────────────────
        public async Task<DataTable> DepartmanIstatistikleriniGetirAsync(DateTime? bas = null, DateTime? bit = null)
        {
            string query = @"
                SELECT
                    d.DepartmentName AS Departman,
                    ISNULL(SUM(l.DayCount), 0) AS ToplamIzinGunu
                FROM Departments d
                LEFT JOIN Employees e ON d.DepartmentID = e.DepartmentID
                LEFT JOIN Leaves l ON e.EmployeeID = l.EmployeeID AND l.Status = 'Onaylandi'
                    AND (@Bas IS NULL OR l.StartDate >= @Bas)
                    AND (@Bit IS NULL OR l.EndDate   <= @Bit)
                GROUP BY d.DepartmentName
                ORDER BY ToplamIzinGunu DESC";

            var prms = new[]
            {
                new SqlParameter("@Bas", bas.HasValue ? (object)bas.Value.Date : DBNull.Value),
                new SqlParameter("@Bit", bit.HasValue ? (object)bit.Value.Date : DBNull.Value),
            };
            return await _db.ExecuteQueryAsync(query, prms);
        }

        // ── 2. Aylık İzin Yoğunluğu (İçinde Bulunduğumuz Yıl) ─────
        public async Task<DataTable> AylikYogunluguGetirAsync(DateTime? bas = null, DateTime? bit = null)
        {
            string query = @"
                SELECT
                    MONTH(l.StartDate) AS AyNumarasi,
                    SUM(l.DayCount) AS ToplamGun
                FROM Leaves l
                WHERE l.Status = 'Onaylandi'
                  AND (
                    (@Bas IS NULL AND @Bit IS NULL AND YEAR(l.StartDate) = YEAR(GETDATE()))
                    OR
                    ((@Bas IS NOT NULL OR @Bit IS NOT NULL)
                     AND (@Bas IS NULL OR l.StartDate >= @Bas)
                     AND (@Bit IS NULL OR l.EndDate   <= @Bit))
                  )
                GROUP BY MONTH(l.StartDate)
                ORDER BY AyNumarasi";

            var prms = new[]
            {
                new SqlParameter("@Bas", bas.HasValue ? (object)bas.Value.Date : DBNull.Value),
                new SqlParameter("@Bit", bit.HasValue ? (object)bit.Value.Date : DBNull.Value),
            };
            DataTable dt = await _db.ExecuteQueryAsync(query, prms);

            // Ay numaralarını ay isimlerine çevir
            dt.Columns.Add("Ay", typeof(string));
            foreach (DataRow row in dt.Rows)
            {
                int ayNo = Convert.ToInt32(row["AyNumarasi"]);
                row["Ay"] = new DateTime(2000, ayNo, 1).ToString("MMMM");
            }

            return dt;
        }

        // ── 3. En Çok İzin Alan 5 Çalışan ──────────────────────────
        public async Task<DataTable> EnCokIzinAlanlariGetirAsync(DateTime? bas = null, DateTime? bit = null)
        {
            string query = @"
                SELECT TOP 5
                    e.FullName AS Personel,
                    d.DepartmentName AS Departman,
                    SUM(l.DayCount) AS KullanilanGun
                FROM Leaves l
                INNER JOIN Employees e ON l.EmployeeID = e.EmployeeID
                INNER JOIN Departments d ON e.DepartmentID = d.DepartmentID
                WHERE l.Status = 'Onaylandi'
                  AND (@Bas IS NULL OR l.StartDate >= @Bas)
                  AND (@Bit IS NULL OR l.EndDate   <= @Bit)
                GROUP BY e.FullName, d.DepartmentName
                ORDER BY KullanilanGun DESC";

            var prms = new[]
            {
                new SqlParameter("@Bas", bas.HasValue ? (object)bas.Value.Date : DBNull.Value),
                new SqlParameter("@Bit", bit.HasValue ? (object)bit.Value.Date : DBNull.Value),
            };
            return await _db.ExecuteQueryAsync(query, prms);
        }

        // ── 4. Bugün İzinli Olan Aktif Çalışanlar ──────────────────
        public async Task<DataTable> AktifIzinlileriGetirAsync()
        {
            string query = @"
                SELECT
                    e.FullName AS Personel,
                    d.DepartmentName AS Departman,
                    l.EndDate AS BitisTarihi
                FROM Leaves l
                INNER JOIN Employees e ON l.EmployeeID = e.EmployeeID
                INNER JOIN Departments d ON e.DepartmentID = d.DepartmentID
                WHERE l.Status = 'Onaylandi'
                  AND CAST(GETDATE() AS DATE) BETWEEN l.StartDate AND l.EndDate
                ORDER BY l.EndDate ASC";

            return await _db.ExecuteQueryAsync(query);
        }

        // ── 5. Personel Detayları — hak ediş hesaplamasıyla ────────
        public async Task<DataTable> PersonelDetaylariniGetirAsync()
        {
            string query = @"
                SELECT
                    e.SicilNo                       AS [Sicil No],
                    e.FullName                      AS [Ad Soyad],
                    d.DepartmentName                AS [Departman],
                    e.Role                          AS [Rol],
                    e.StartDateOfWork               AS [İşe Giriş],
                    DATEDIFF(YEAR, e.StartDateOfWork, GETDATE())
                      - CASE WHEN DATEADD(YEAR,
                            DATEDIFF(YEAR, e.StartDateOfWork, GETDATE()),
                            e.StartDateOfWork) > GETDATE() THEN 1 ELSE 0
                        END                         AS [Çalışma Yılı],
                    ISNULL(lSum.KullanilanIzin, 0)  AS [Kullanılan İzin],
                    CASE e.IsActive WHEN 1 THEN 'Aktif' ELSE 'Pasif' END AS [Durum]
                FROM Employees e
                INNER JOIN Departments d ON e.DepartmentID = d.DepartmentID
                LEFT JOIN (
                    SELECT EmployeeID, SUM(DayCount) AS KullanilanIzin
                    FROM Leaves
                    WHERE Status = 'Onaylandi'
                    GROUP BY EmployeeID
                ) AS lSum ON lSum.EmployeeID = e.EmployeeID
                ORDER BY d.DepartmentName, e.FullName";

            DataTable dt = await _db.ExecuteQueryAsync(query);

            // Hak ediş hesabı — IzinHakEdisHelper ile (DRY)
            dt.Columns.Add("Toplam Hak", typeof(int));
            dt.Columns.Add("Kalan İzin", typeof(int));

            foreach (DataRow row in dt.Rows)
            {
                int calisilanYil = Convert.ToInt32(row["Çalışma Yılı"]);
                int toplamHak    = IzinHakEdisHelper.ToplamHakHesapla(calisilanYil);
                int kullanilan   = Convert.ToInt32(row["Kullanılan İzin"]);

                row["Toplam Hak"] = toplamHak;
                row["Kalan İzin"] = toplamHak - kullanilan;
            }

            return dt;
        }

        // ── 6. Departman Listesi (ComboBox doldurmak için) ─────────
        public async Task<DataTable> DepartmanlariGetirAsync()
        {
            string query = @"
                SELECT DepartmentID, DepartmentName
                FROM Departments
                ORDER BY DepartmentName";

            return await _db.ExecuteQueryAsync(query);
        }

        // ── 7. Audit Log Kayıtları (Türkçe İşlem Tipi ile) ────────
        public async Task<DataTable> LoglariGetirAsync()
        {
            string query = @"
                SELECT
                    al.LogID,
                    CASE
                        WHEN al.OperationType = 'Insert'                              THEN 'Oluşturuldu'
                        WHEN al.OperationType = 'Update' AND al.Status = 'Onaylandi'  THEN 'Onaylandı'
                        WHEN al.OperationType = 'Update' AND al.Status = 'Reddedildi' THEN 'Reddedildi'
                        WHEN al.OperationType = 'Update'                              THEN 'Güncellendi'
                        WHEN al.OperationType = 'Delete'                              THEN 'Silindi'
                        ELSE al.OperationType
                    END AS IslemTipi,
                    e.FullName AS IslemYapan,
                    al.TargetTable,
                    al.Details,
                    al.LogDate
                FROM AuditLogs al
                INNER JOIN Employees e ON al.PerformedBy = e.EmployeeID
                ORDER BY al.LogDate DESC";

            return await _db.ExecuteQueryAsync(query);
        }

        // ── 8. Departman Bazlı Audit Logları (Amir paneli) ─────────
        public async Task<DataTable> LoglariGetirAmirAsync(int departmanId, DateTime? baslangic = null, DateTime? bitis = null)
        {
            string query = @"
                SELECT
                    al.LogID,
                    CASE
                        WHEN al.OperationType = 'Insert'                              THEN 'Oluşturuldu'
                        WHEN al.OperationType = 'Update' AND al.Status = 'Onaylandi'  THEN 'Onaylandı'
                        WHEN al.OperationType = 'Update' AND al.Status = 'Reddedildi' THEN 'Reddedildi'
                        WHEN al.OperationType = 'Update'                              THEN 'Güncellendi'
                        WHEN al.OperationType = 'Delete'                              THEN 'Silindi'
                        ELSE al.OperationType
                    END AS IslemTipi,
                    e.FullName AS IslemYapan,
                    al.TargetTable,
                    al.Details,
                    al.LogDate
                FROM AuditLogs al
                INNER JOIN Employees e ON al.PerformedBy = e.EmployeeID
                WHERE e.DepartmentID = @DepID
                  AND (@LogBas IS NULL OR al.LogDate >= @LogBas)
                  AND (@LogBit IS NULL OR al.LogDate < DATEADD(day, 1, @LogBit))
                ORDER BY al.LogDate DESC";

            var prms = new[]
            {
                new SqlParameter("@DepID",   departmanId),
                new SqlParameter("@LogBas",  baslangic.HasValue ? (object)baslangic.Value.Date : DBNull.Value),
                new SqlParameter("@LogBit",  bitis.HasValue     ? (object)bitis.Value.Date     : DBNull.Value),
            };
            return await _db.ExecuteQueryAsync(query, prms);
        }

        // ── 9. Departman Bazlı Aktif İzinliler (Amir paneli) ───────
        public async Task<DataTable> DepartmanAktifIzinlileriGetirAsync(int departmanId)
        {
            string query = @"
                SELECT
                    e.FullName AS Personel,
                    l.StartDate AS BaslangicTarihi,
                    l.EndDate AS BitisTarihi,
                    l.DayCount AS GunSayisi
                FROM Leaves l
                INNER JOIN Employees e ON l.EmployeeID = e.EmployeeID
                WHERE l.Status = 'Onaylandi'
                  AND e.DepartmentID = @DepID
                  AND CAST(GETDATE() AS DATE) BETWEEN l.StartDate AND l.EndDate
                ORDER BY l.EndDate ASC";

            return await _db.ExecuteQueryAsync(query, new[] { new SqlParameter("@DepID", departmanId) });
        }

        // ── 10. Departman Tüm İzinleri — filtreli (Amir Rapor tab) ─
        public async Task<DataTable> DepartmanTumIzinleriGetirAsync(
            int departmanId,
            string? personelAd,
            DateTime? baslangicFiltre,
            DateTime? bitisFiltre,
            string? durum)
        {
            string query = @"
                SELECT
                    e.FullName       AS Personel,
                    l.StartDate      AS BaslangicTarihi,
                    l.EndDate        AS BitisTarihi,
                    l.DayCount       AS GunSayisi,
                    CASE l.Status
                        WHEN 'Beklemede'    THEN 'Beklemede'
                        WHEN 'Onaylandi'    THEN 'Onaylandı'
                        WHEN 'Reddedildi'   THEN 'Reddedildi'
                        WHEN 'Iptal Edildi' THEN 'İptal Edildi'
                        ELSE l.Status
                    END              AS Durum,
                    l.CreatedDate    AS TalepTarihi,
                    ISNULL(l.ManagerNote, '') AS YoneticiNotu
                FROM Leaves l
                INNER JOIN Employees e ON l.EmployeeID = e.EmployeeID
                WHERE e.DepartmentID = @DepID
                  AND (@PersonelAd      IS NULL OR e.FullName   LIKE '%' + @PersonelAd + '%')
                  AND (@Durum           IS NULL OR l.Status      = @Durum)
                  AND (@BaslangicFiltre IS NULL OR l.StartDate  >= @BaslangicFiltre)
                  AND (@BitisFiltre     IS NULL OR l.EndDate    <= @BitisFiltre)
                ORDER BY l.StartDate DESC";

            var parameters = new[]
            {
                new SqlParameter("@DepID",           departmanId),
                new SqlParameter("@PersonelAd",      string.IsNullOrWhiteSpace(personelAd) ? (object)DBNull.Value : personelAd.Trim()),
                new SqlParameter("@Durum",           string.IsNullOrWhiteSpace(durum)      ? (object)DBNull.Value : durum),
                new SqlParameter("@BaslangicFiltre", baslangicFiltre.HasValue ? (object)baslangicFiltre.Value.Date : DBNull.Value),
                new SqlParameter("@BitisFiltre",     bitisFiltre.HasValue     ? (object)bitisFiltre.Value.Date     : DBNull.Value),
            };

            return await _db.ExecuteQueryAsync(query, parameters);
        }
    }
}
