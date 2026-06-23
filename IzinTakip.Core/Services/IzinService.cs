using System.Data;
using Microsoft.Data.SqlClient;
using IzinTakip.Core.Abstractions;
using IzinTakip.Core.Helpers;
using IzinTakip.Core.Models;

namespace IzinTakip.Core.Services
{
    public class IzinService : IIzinService
    {
        private readonly IDbHelper _db;

        public IzinService(IDbHelper db)
        {
            _db = db;
        }

        // ══════════════════════════════════════════════════════════
        //  SORGULAR
        // ══════════════════════════════════════════════════════════

        public async Task<DataTable> PersonelListesiGetirAsync(int departmanId, DateTime? aralikBas = null, DateTime? aralikBit = null)
        {
            string query = @"
                SELECT
                    e.EmployeeID, e.FullName, e.SicilNo, e.StartDateOfWork,
                    ISNULL(lSum.KullanilanIzin, 0) AS KullanilanIzin
                FROM Employees e
                LEFT JOIN (
                    SELECT EmployeeID, SUM(DayCount) AS KullanilanIzin
                    FROM Leaves WHERE Status = 'Onaylandi' GROUP BY EmployeeID
                ) AS lSum ON lSum.EmployeeID = e.EmployeeID
                WHERE e.DepartmentID = @DepID AND e.Role = 'Personel' AND e.IsActive = 1
                  AND (@AralikBas IS NULL OR EXISTS (
                        SELECT 1 FROM Leaves lf
                        WHERE lf.EmployeeID = e.EmployeeID
                          AND lf.Status = 'Onaylandi'
                          AND lf.StartDate <= @AralikBit
                          AND lf.EndDate   >= @AralikBas
                  ))
                ORDER BY e.FullName";

            var dt = await _db.ExecuteQueryAsync(query, new[] {
                new SqlParameter("@DepID",     departmanId),
                new SqlParameter("@AralikBas", (object?)aralikBas ?? DBNull.Value),
                new SqlParameter("@AralikBit", (object?)aralikBit ?? DBNull.Value)
            });

            // Hak edis hesaplari — IzinHakEdisHelper'dan (tek kaynak)
            dt.Columns.Add("KidemYil",  typeof(int));
            dt.Columns.Add("ToplamHak", typeof(int));
            dt.Columns.Add("KalanIzin", typeof(int));

            foreach (DataRow row in dt.Rows)
            {
                DateTime startDate  = Convert.ToDateTime(row["StartDateOfWork"]);
                int kullanilan      = Convert.ToInt32(row["KullanilanIzin"]);
                int kidemYil        = IzinHakEdisHelper.KidemYilHesapla(startDate);
                int toplamHak       = IzinHakEdisHelper.ToplamHakHesapla(kidemYil);

                row["KidemYil"]  = kidemYil;
                row["ToplamHak"] = toplamHak;
                row["KalanIzin"] = toplamHak - kullanilan;
            }

            return dt;
        }

        public async Task<DataTable> PersonelIzinleriniGetirAsync(int employeeId)
        {
            string query = @"
                SELECT LeaveID, StartDate, EndDate, DayCount, Status, ManagerNote, CreatedDate, LeaveType
                FROM Leaves
                WHERE EmployeeID = @EmpID
                ORDER BY StartDate DESC";

            return await _db.ExecuteQueryAsync(query, new[] { new SqlParameter("@EmpID", employeeId) });
        }

        public async Task<DataTable> AralikIzinlileriAmirAsync(DateTime baslangic, DateTime bitis, int departmanId)
        {
            string query = @"
                SELECT
                    e.FullName AS [Ad Soyad], l.StartDate AS [Baslangic],
                    l.EndDate AS [Bitis], l.DayCount AS [Gun]
                FROM Leaves l
                INNER JOIN Employees e ON l.EmployeeID = e.EmployeeID
                WHERE e.DepartmentID = @DepID AND l.Status = 'Onaylandi'
                  AND l.StartDate <= @Bitis AND l.EndDate >= @Baslangic
                ORDER BY l.StartDate, e.FullName";

            return await _db.ExecuteQueryAsync(query, new[] {
                new SqlParameter("@DepID", departmanId),
                new SqlParameter("@Baslangic", baslangic),
                new SqlParameter("@Bitis", bitis)
            });
        }

        public async Task<DataTable> AralikIzinlileriYoneticiAsync(DateTime baslangic, DateTime bitis)
        {
            string query = @"
                SELECT
                    e.FullName AS [Ad Soyad], d.DepartmentName AS [Departman],
                    l.StartDate AS [Baslangic], l.EndDate AS [Bitis],
                    l.DayCount AS [Gun], l.Status AS [Durum]
                FROM Leaves l
                INNER JOIN Employees e ON l.EmployeeID = e.EmployeeID
                INNER JOIN Departments d ON e.DepartmentID = d.DepartmentID
                WHERE l.Status = 'Onaylandi'
                  AND l.StartDate <= @Bitis AND l.EndDate >= @Baslangic
                ORDER BY d.DepartmentName, l.StartDate, e.FullName";

            return await _db.ExecuteQueryAsync(query, new[] {
                new SqlParameter("@Baslangic", baslangic),
                new SqlParameter("@Bitis", bitis)
            });
        }

        public async Task<DataTable> TalepleriGetirAsync(string durumFiltresi, int departmanId, string personelAdi, DateTime? bas = null, DateTime? bit = null)
        {
            string query = @"
                SELECT
                    l.LeaveID, l.EmployeeID, e.FullName AS PersonelAd,
                    d.DepartmentName AS Departman,
                    l.StartDate, l.EndDate, l.DayCount, l.Status, l.CreatedDate, l.LeaveType
                FROM Leaves l
                INNER JOIN Employees e ON l.EmployeeID = e.EmployeeID
                INNER JOIN Departments d ON e.DepartmentID = d.DepartmentID
                WHERE (@Durum = 'Tumu' OR l.Status = @Durum)
                  AND (@DepID = 0 OR e.DepartmentID = @DepID)
                  AND (@PersonelAdi = '' OR e.FullName LIKE '%' + @PersonelAdi + '%')
                  AND (@TalepBas IS NULL OR l.StartDate >= @TalepBas)
                  AND (@TalepBit IS NULL OR l.StartDate <= @TalepBit)
                ORDER BY l.CreatedDate DESC";

            return await _db.ExecuteQueryAsync(query, new[] {
                new SqlParameter("@Durum",     durumFiltresi.Trim()),
                new SqlParameter("@DepID",     departmanId),
                new SqlParameter("@PersonelAdi", personelAdi.Trim()),
                new SqlParameter("@TalepBas",  (object?)bas ?? DBNull.Value),
                new SqlParameter("@TalepBit",  (object?)bit ?? DBNull.Value)
            });
        }

        // ══════════════════════════════════════════════════════════
        //  IS ISLEMLERI (Transaction + Loglama)
        // ══════════════════════════════════════════════════════════

        public async Task<IzinIslemSonucu> IzinEkleAsync(IzinEkleIstek istek)
        {
            // 1. Tarih validasyonu
            if (istek.Baslangic < DateTime.Today)
                return IzinIslemSonucu.Hata("Gecmis bir tarihe izin giremezsiniz!");
            if (istek.Bitis < istek.Baslangic)
                return IzinIslemSonucu.Hata("Bitis tarihi baslangictan once olamaz!");

            int talepGun = DateHelper.IsGunuHesapla(istek.Baslangic, istek.Bitis);
            if (talepGun == 0)
                return IzinIslemSonucu.Hata("Secilen tarih araligi sadece hafta sonuna denk geliyor.");
            if (talepGun > istek.KalanIzin)
                return IzinIslemSonucu.Hata($"Kalan izin ({istek.KalanIzin} gun) talep edilen izni ({talepGun} gun) karsilamiyor!");

            // 2. Cakisma kontrolu
            var cakisma = await CakismaKontrolAsync(istek.EmployeeId, istek.Baslangic, istek.Bitis);
            if (cakisma != null) return cakisma;

            // 3. Departman kotasi
            var kota = await DepartmanKotaKontrolAsync(istek.DepartmanId, istek.EmployeeId, istek.Baslangic, istek.Bitis);
            if (kota != null) return kota;

            // 4. Kayit (Transaction)
            using var conn = new SqlConnection(_db.ConnectionString);
            await conn.OpenAsync();
            using var tx = conn.BeginTransaction();
            try
            {
                string insertLeave = @"INSERT INTO Leaves (EmployeeID, StartDate, EndDate, DayCount, Status, AmirID)
                                       VALUES (@EmpID, @Start, @End, @Days, 'Beklemede', @AmirID)";
                using (var cmd = new SqlCommand(insertLeave, conn, tx))
                {
                    cmd.Parameters.AddWithValue("@EmpID",  istek.EmployeeId);
                    cmd.Parameters.AddWithValue("@Start",  istek.Baslangic);
                    cmd.Parameters.AddWithValue("@End",    istek.Bitis);
                    cmd.Parameters.AddWithValue("@Days",   talepGun);
                    cmd.Parameters.AddWithValue("@AmirID", istek.AmirId);
                    await cmd.ExecuteNonQueryAsync();
                }

                await LogEkleAsync(conn, tx, "Insert", istek.AmirId, "Leaves",
                    $"{istek.PersonelAd} icin {talepGun} gunluk yeni izin talebi (Beklemede) olusturuldu.");

                tx.Commit();
                return IzinIslemSonucu.Basari("Izin talebi basariyla olusturuldu ve Yonetici onayina sunuldu.");
            }
            catch (Exception ex)
            {
                tx.Rollback();
                return IzinIslemSonucu.Hata("Kayit sirasinda hata: " + ex.Message);
            }
        }

        public async Task<IzinIslemSonucu> AvansIzinEkleAsync(IzinEkleIstek istek)
        {
            // 1. Tarih validasyonu
            if (istek.Baslangic < DateTime.Today)
                return IzinIslemSonucu.Hata("Gecmis bir tarihe izin giremezsiniz!");
            if (istek.Bitis < istek.Baslangic)
                return IzinIslemSonucu.Hata("Bitis tarihi baslangictan once olamaz!");

            int talepGun = DateHelper.IsGunuHesapla(istek.Baslangic, istek.Bitis);
            if (talepGun == 0)
                return IzinIslemSonucu.Hata("Secilen tarih araligi sadece hafta sonuna denk geliyor.");

            // 2. Kıdem ve avans limiti kontrolü — DB'den anlık veri
            var empDt = await _db.ExecuteQueryAsync(@"
                SELECT e.StartDateOfWork,
                       ISNULL((SELECT SUM(DayCount) FROM Leaves
                               WHERE EmployeeID = e.EmployeeID AND Status = 'Onaylandi'), 0) AS KullanilanIzin
                FROM Employees e WHERE e.EmployeeID = @EmpID",
                new[] { new SqlParameter("@EmpID", istek.EmployeeId) });

            if (empDt.Rows.Count == 0)
                return IzinIslemSonucu.Hata("Personel bulunamadi!");

            DateTime startDate     = Convert.ToDateTime(empDt.Rows[0]["StartDateOfWork"]);
            int guncelKullanilan   = Convert.ToInt32(empDt.Rows[0]["KullanilanIzin"]);
            int kidemYil           = IzinHakEdisHelper.KidemYilHesapla(startDate);
            int toplamHak          = IzinHakEdisHelper.ToplamHakHesapla(kidemYil);
            int guncelKalan        = toplamHak - guncelKullanilan;  // UI'dan bağımsız, anlık değer
            int gelecekYilHak      = IzinHakEdisHelper.YillikHakHesapla(kidemYil + 1);

            if (gelecekYilHak == 0)
                return IzinIslemSonucu.Hata("Gelecek yil icin henuz izin hakki hesaplanamadi!");

            // Mevcut acik: anlık kalan negatifse zaten avans kullanilmis
            int mevcutAcik = Math.Max(-guncelKalan, 0);
            if (mevcutAcik + talepGun > gelecekYilHak)
                return IzinIslemSonucu.Hata(
                    $"Avans limit asiliyor! Mevcut acik: {mevcutAcik} gun, Talep: {talepGun} gun, " +
                    $"Gelecek yil hakki: {gelecekYilHak} gun.");

            // 3. Cakisma kontrolu
            var cakisma = await CakismaKontrolAsync(istek.EmployeeId, istek.Baslangic, istek.Bitis);
            if (cakisma != null) return cakisma;

            // 4. Departman kotasi
            var kota = await DepartmanKotaKontrolAsync(istek.DepartmanId, istek.EmployeeId, istek.Baslangic, istek.Bitis);
            if (kota != null) return kota;

            // 5. Kayit (Transaction)
            using var conn = new SqlConnection(_db.ConnectionString);
            await conn.OpenAsync();
            using var tx = conn.BeginTransaction();
            try
            {
                string insertLeave = @"
                    INSERT INTO Leaves (EmployeeID, StartDate, EndDate, DayCount, Status, AmirID, LeaveType)
                    VALUES (@EmpID, @Start, @End, @Days, 'Beklemede', @AmirID, 'Avans')";
                using (var cmd = new SqlCommand(insertLeave, conn, tx))
                {
                    cmd.Parameters.AddWithValue("@EmpID",  istek.EmployeeId);
                    cmd.Parameters.AddWithValue("@Start",  istek.Baslangic);
                    cmd.Parameters.AddWithValue("@End",    istek.Bitis);
                    cmd.Parameters.AddWithValue("@Days",   talepGun);
                    cmd.Parameters.AddWithValue("@AmirID", istek.AmirId);
                    await cmd.ExecuteNonQueryAsync();
                }

                await LogEkleAsync(conn, tx, "Insert", istek.AmirId, "Leaves",
                    $"{istek.PersonelAd} icin {talepGun} gunluk AVANS izin talebi olusturuldu (Beklemede).");

                tx.Commit();
                return IzinIslemSonucu.Basari(
                    $"Avans izin talebi basariyla olusturuldu ({talepGun} gun) ve Yonetici onayina sunuldu.");
            }
            catch (Exception ex)
            {
                tx.Rollback();
                return IzinIslemSonucu.Hata("Kayit sirasinda hata: " + ex.Message);
            }
        }

        public async Task<IzinIslemSonucu> IzinIptalEtAmirAsync(int leaveId, int amirId, string personelAd)
        {
            using var conn = new SqlConnection(_db.ConnectionString);
            await conn.OpenAsync();
            using var tx = conn.BeginTransaction();
            try
            {
                string update = "UPDATE Leaves SET Status = 'Iptal Edildi' WHERE LeaveID = @LeaveID";
                using (var cmd = new SqlCommand(update, conn, tx))
                {
                    cmd.Parameters.AddWithValue("@LeaveID", leaveId);
                    await cmd.ExecuteNonQueryAsync();
                }

                await LogEkleAsync(conn, tx, "Delete", amirId, "Leaves",
                    $"{personelAd} adli personelin 'Beklemede' olan izin talebi amir tarafindan iptal edildi (Soft Delete).");

                tx.Commit();
                return IzinIslemSonucu.Basari("Izin talebi basariyla iptal edildi.");
            }
            catch (Exception ex)
            {
                tx.Rollback();
                return IzinIslemSonucu.Hata("Iptal sirasinda hata: " + ex.Message);
            }
        }

        public async Task<IzinIslemSonucu> IzinGuncelleAmirAsync(int leaveId, IzinGuncelleIstek istek)
        {
            return await IzinGuncelleOrtakAsync(leaveId, istek, "Amir");
        }

        public async Task<IzinIslemSonucu> IzinGuncelleYoneticiAsync(int leaveId, IzinGuncelleIstek istek)
        {
            return await IzinGuncelleOrtakAsync(leaveId, istek, "Yonetici");
        }

        public async Task<IzinIslemSonucu> OnaylaAsync(int leaveId, int yoneticiId, string personelAd, int gunSayisi)
        {
            using var conn = new SqlConnection(_db.ConnectionString);
            await conn.OpenAsync();
            using var tx = conn.BeginTransaction();
            try
            {
                using (var cmd = new SqlCommand("UPDATE Leaves SET Status = 'Onaylandi' WHERE LeaveID = @LeaveID", conn, tx))
                {
                    cmd.Parameters.AddWithValue("@LeaveID", leaveId);
                    await cmd.ExecuteNonQueryAsync();
                }

                await LogEkleAsync(conn, tx, "Update", yoneticiId, "Leaves",
                    $"Yonetici, {personelAd} adli personelin izin talebini onayladi.", "Onaylandi");

                tx.Commit();
                return IzinIslemSonucu.Basari("Talep basariyla ONAYLANDI.");
            }
            catch (Exception ex)
            {
                tx.Rollback();
                return IzinIslemSonucu.Hata("Onaylama sirasinda hata: " + ex.Message);
            }
        }

        public async Task<IzinIslemSonucu> ReddetAsync(int leaveId, int yoneticiId, string redSebebi, string personelAd)
        {
            using var conn = new SqlConnection(_db.ConnectionString);
            await conn.OpenAsync();
            using var tx = conn.BeginTransaction();
            try
            {
                using (var cmd = new SqlCommand("UPDATE Leaves SET Status = 'Reddedildi', ManagerNote = @Note WHERE LeaveID = @LeaveID", conn, tx))
                {
                    cmd.Parameters.AddWithValue("@LeaveID", leaveId);
                    cmd.Parameters.AddWithValue("@Note", redSebebi);
                    await cmd.ExecuteNonQueryAsync();
                }

                await LogEkleAsync(conn, tx, "Update", yoneticiId, "Leaves",
                    $"Yonetici, {personelAd} adli personelin iznini reddetti. Gerekce: {redSebebi}", "Reddedildi");

                tx.Commit();
                return IzinIslemSonucu.Basari("Talep basariyla REDDEDILDI.");
            }
            catch (Exception ex)
            {
                tx.Rollback();
                return IzinIslemSonucu.Hata("Reddetme sirasinda hata: " + ex.Message);
            }
        }

        public async Task<IzinIslemSonucu> IzinIptalEtYoneticiAsync(int leaveId, int yoneticiId, string personelAd)
        {
            using var conn = new SqlConnection(_db.ConnectionString);
            await conn.OpenAsync();
            using var tx = conn.BeginTransaction();
            try
            {
                using (var cmd = new SqlCommand("UPDATE Leaves SET Status = 'Iptal Edildi' WHERE LeaveID = @LeaveID", conn, tx))
                {
                    cmd.Parameters.AddWithValue("@LeaveID", leaveId);
                    await cmd.ExecuteNonQueryAsync();
                }

                await LogEkleAsync(conn, tx, "Delete", yoneticiId, "Leaves",
                    $"Yonetici mudahalesi: {personelAd} adli personelin izni sistemden silindi (Soft Delete).", "Iptal Edildi");

                tx.Commit();
                return IzinIslemSonucu.Basari("Izin kaydi basariyla silindi (Iptal edildi).");
            }
            catch (Exception ex)
            {
                tx.Rollback();
                return IzinIslemSonucu.Hata("Silme sirasinda hata: " + ex.Message);
            }
        }

        // ══════════════════════════════════════════════════════════
        //  PRIVATE YARDIMCI METOTLAR
        // ══════════════════════════════════════════════════════════

        private async Task<IzinIslemSonucu?> CakismaKontrolAsync(int employeeId, DateTime baslangic, DateTime bitis, int? haricLeaveId = null)
        {
            string query = @"SELECT COUNT(*) FROM Leaves
                             WHERE EmployeeID = @EmpID AND Status IN ('Onaylandi', 'Beklemede')
                             AND ((StartDate <= @End) AND (EndDate >= @Start))"
                + (haricLeaveId.HasValue ? " AND LeaveID != @ExcludeID" : "");

            var pars = new List<SqlParameter> {
                new("@EmpID", employeeId),
                new("@Start", baslangic),
                new("@End", bitis)
            };
            if (haricLeaveId.HasValue) pars.Add(new("@ExcludeID", haricLeaveId.Value));

            int sayi = Convert.ToInt32((await _db.ExecuteQueryAsync(query, pars.ToArray())).Rows[0][0]);
            return sayi > 0 ? IzinIslemSonucu.Hata("Bu tarih araliginda cakisan baska bir izin var!") : null;
        }

        private async Task<IzinIslemSonucu?> DepartmanKotaKontrolAsync(int departmanId, int employeeId, DateTime baslangic, DateTime bitis)
        {
            string query = @"
                SELECT COUNT(DISTINCT l.EmployeeID)
                FROM Leaves l INNER JOIN Employees e ON l.EmployeeID = e.EmployeeID
                WHERE e.DepartmentID = @DepID AND l.EmployeeID != @EmpID
                  AND l.Status IN ('Onaylandi', 'Beklemede')
                  AND ((l.StartDate <= @End) AND (l.EndDate >= @Start))";

            int sayi = Convert.ToInt32((await _db.ExecuteQueryAsync(query, new[] {
                new SqlParameter("@DepID", departmanId),
                new SqlParameter("@EmpID", employeeId),
                new SqlParameter("@Start", baslangic),
                new SqlParameter("@End", bitis)
            })).Rows[0][0]);

            return sayi >= 2 ? IzinIslemSonucu.Hata($"Bu tarihlerde departmandan zaten {sayi} kisi izinli! Limit dolu.") : null;
        }

        private async Task<IzinIslemSonucu> IzinGuncelleOrtakAsync(int leaveId, IzinGuncelleIstek istek, string rol)
        {
            if (istek.YeniBaslangic < DateTime.Today)
                return IzinIslemSonucu.Hata("Gecmis tarihe izin guncellenemez!");
            if (istek.YeniBitis < istek.YeniBaslangic)
                return IzinIslemSonucu.Hata("Bitis tarihi baslangictan once olamaz!");

            int talepGun = DateHelper.IsGunuHesapla(istek.YeniBaslangic, istek.YeniBitis);
            if (talepGun == 0)
                return IzinIslemSonucu.Hata("Secilen tarih araligi sadece hafta sonuna denk geliyor.");

            // Amir icin bakiye kontrolu (Yonetici sinirli degildir)
            if (rol == "Amir" && talepGun > istek.KalanIzin)
                return IzinIslemSonucu.Hata($"Kalan izin ({istek.KalanIzin} gun), talep ({talepGun} gun) karsilamiyor!");

            var cakisma = await CakismaKontrolAsync(istek.EmployeeId, istek.YeniBaslangic, istek.YeniBitis, leaveId);
            if (cakisma != null) return cakisma;

            var kota = await DepartmanKotaKontrolAsync(istek.DepartmanId, istek.EmployeeId, istek.YeniBaslangic, istek.YeniBitis);
            if (kota != null) return kota;

            using var conn = new SqlConnection(_db.ConnectionString);
            await conn.OpenAsync();
            using var tx = conn.BeginTransaction();
            try
            {
                using (var cmd = new SqlCommand("UPDATE Leaves SET StartDate=@Start, EndDate=@End, DayCount=@Days WHERE LeaveID=@LeaveID", conn, tx))
                {
                    cmd.Parameters.AddWithValue("@Start",   istek.YeniBaslangic);
                    cmd.Parameters.AddWithValue("@End",     istek.YeniBitis);
                    cmd.Parameters.AddWithValue("@Days",    talepGun);
                    cmd.Parameters.AddWithValue("@LeaveID", leaveId);
                    await cmd.ExecuteNonQueryAsync();
                }

                string detay = rol == "Yonetici"
                    ? $"Yonetici mudahalesi: {istek.PersonelAd} izni {talepGun} gun olarak guncellendi."
                    : $"Amir, {istek.PersonelAd} adli personelin beklemedeki iznini {istek.YeniBaslangic:dd.MM.yyyy}-{istek.YeniBitis:dd.MM.yyyy} olarak guncelledi.";

                await LogEkleAsync(conn, tx, "Update", istek.YapanId, "Leaves", detay);

                tx.Commit();
                return IzinIslemSonucu.Basari("Izin basariyla guncellendi.");
            }
            catch (Exception ex)
            {
                tx.Rollback();
                return IzinIslemSonucu.Hata("Guncelleme hatasi: " + ex.Message);
            }
        }

        private static async Task LogEkleAsync(SqlConnection conn, SqlTransaction tx,
            string operationType, int performedBy, string targetTable, string details, string? status = null)
        {
            string sql = status != null
                ? @"INSERT INTO AuditLogs (OperationType, PerformedBy, TargetTable, Details, Status)
                    VALUES (@Op, @By, @Tbl, @Det, @St)"
                : @"INSERT INTO AuditLogs (OperationType, PerformedBy, TargetTable, Details)
                    VALUES (@Op, @By, @Tbl, @Det)";

            using var cmd = new SqlCommand(sql, conn, tx);
            cmd.Parameters.AddWithValue("@Op",  operationType);
            cmd.Parameters.AddWithValue("@By",  performedBy);
            cmd.Parameters.AddWithValue("@Tbl", targetTable);
            cmd.Parameters.AddWithValue("@Det", details);
            if (status != null) cmd.Parameters.AddWithValue("@St", status);
            await cmd.ExecuteNonQueryAsync();
        }

        // ── Personel izin akışı (yeni) ───────────────────────────

        public async Task<IzinIslemSonucu> PersonelIzinEkleAsync(IzinEkleIstek istek)
        {
            if (istek.Baslangic.Date < DateTime.Today)
                return IzinIslemSonucu.Hata("Gecmis bir tarihe izin giremezsiniz!");
            if (istek.Bitis < istek.Baslangic)
                return IzinIslemSonucu.Hata("Bitis tarihi baslangictan once olamaz!");

            int gunSayisi = DateHelper.IsGunuHesapla(istek.Baslangic, istek.Bitis);
            if (gunSayisi == 0)
                return IzinIslemSonucu.Hata("Secilen tarih araligi sadece hafta sonuna denk geliyor.");
            if (gunSayisi > istek.KalanIzin)
                return IzinIslemSonucu.Hata($"Kalan izin ({istek.KalanIzin} gun) talep edilen izni ({gunSayisi} gun) karsilamiyor!");

            var cakisma = await CakismaKontrolAsync(istek.EmployeeId, istek.Baslangic, istek.Bitis);
            if (cakisma != null) return cakisma;

            using var conn = new SqlConnection(_db.ConnectionString);
            await conn.OpenAsync();
            using var tx = conn.BeginTransaction();
            try
            {
                using (var cmd = new SqlCommand(@"
            INSERT INTO Leaves (EmployeeID, StartDate, EndDate, DayCount, Status, AmirID, LeaveType)
            VALUES (@EmpID, @Start, @End, @Days, 'Amir Onayinda', @AmirID, 'Normal')", conn, tx))
                {
                    cmd.Parameters.AddWithValue("@EmpID", istek.EmployeeId);
                    cmd.Parameters.AddWithValue("@Start", istek.Baslangic);
                    cmd.Parameters.AddWithValue("@End", istek.Bitis);
                    cmd.Parameters.AddWithValue("@Days", gunSayisi);
                    cmd.Parameters.AddWithValue("@AmirID", istek.AmirId);
                    await cmd.ExecuteNonQueryAsync();
                }

                await LogEkleAsync(conn, tx, "Insert", istek.EmployeeId, "Leaves",
                    $"{istek.PersonelAd} icin {gunSayisi} gunluk izin talebi olusturuldu (Amir Onayinda).");

                tx.Commit();
                return IzinIslemSonucu.Basari("Izin talebiniz amire iletildi.");
            }
            catch (Exception ex)
            {
                tx.Rollback();
                return IzinIslemSonucu.Hata("Kayit sirasinda hata: " + ex.Message);
            }
        }

        public async Task<IzinIslemSonucu> AmirOnaylaAsync(int leaveId, int amirId, string personelAd)
        {
            using var conn = new SqlConnection(_db.ConnectionString);
            await conn.OpenAsync();
            using var tx = conn.BeginTransaction();
            try
            {
                using (var cmd = new SqlCommand(@"
            UPDATE Leaves SET Status = 'Beklemede', AmirID = @AmirID
            WHERE LeaveID = @LeaveID AND Status = 'Amir Onayinda'", conn, tx))
                {
                    cmd.Parameters.AddWithValue("@AmirID", amirId);
                    cmd.Parameters.AddWithValue("@LeaveID", leaveId);
                    await cmd.ExecuteNonQueryAsync();
                }

                await LogEkleAsync(conn, tx, "Update", amirId, "Leaves",
                    $"{personelAd} adli personelin izin talebi amir tarafindan onaylandi, yoneticiye sunuldu.", "Beklemede");

                tx.Commit();
                return IzinIslemSonucu.Basari("Izin talebi onaylandi, yoneticiye iletildi.");
            }
            catch (Exception ex)
            {
                tx.Rollback();
                return IzinIslemSonucu.Hata("Onaylama sirasinda hata: " + ex.Message);
            }
        }

        public async Task<IzinIslemSonucu> AmirReddetAsync(int leaveId, int amirId, string redSebebi, string personelAd)
        {
            using var conn = new SqlConnection(_db.ConnectionString);
            await conn.OpenAsync();
            using var tx = conn.BeginTransaction();
            try
            {
                using (var cmd = new SqlCommand(@"
            UPDATE Leaves SET Status = 'Reddedildi', AmirID = @AmirID, ManagerNote = @Note
            WHERE LeaveID = @LeaveID AND Status = 'Amir Onayinda'", conn, tx))
                {
                    cmd.Parameters.AddWithValue("@AmirID", amirId);
                    cmd.Parameters.AddWithValue("@Note", redSebebi);
                    cmd.Parameters.AddWithValue("@LeaveID", leaveId);
                    await cmd.ExecuteNonQueryAsync();
                }

                await LogEkleAsync(conn, tx, "Update", amirId, "Leaves",
                    $"{personelAd} adli personelin izin talebi amir tarafindan reddedildi. Sebep: {redSebebi}", "Reddedildi");

                tx.Commit();
                return IzinIslemSonucu.Basari("Izin talebi reddedildi.");
            }
            catch (Exception ex)
            {
                tx.Rollback();
                return IzinIslemSonucu.Hata("Reddetme sirasinda hata: " + ex.Message);
            }
        }

        public async Task<DataTable> AmirOnayindakileriGetirAsync(int departmanId)
        {
            var sql = @"
        SELECT 
            l.LeaveID, e.FullName, e.SicilNo,
            l.StartDate, l.EndDate, l.DayCount,
            l.Status, l.CreatedDate, l.LeaveType
        FROM Leaves l
        INNER JOIN Employees e ON l.EmployeeID = e.EmployeeID
        WHERE e.DepartmentID = @deptId
          AND l.Status = 'Amir Onayinda'
        ORDER BY l.CreatedDate ASC";

            return await _db.ExecuteQueryAsync(sql,
                [new SqlParameter("@deptId", departmanId)]);
        }

        public async Task<PersonelProfilDto> PersonelProfilGetirAsync(int employeeId)
        {
            var dt = await _db.ExecuteQueryAsync(@"
        SELECT 
            e.EmployeeID, e.FullName, e.SicilNo, e.Role,
            e.StartDateOfWork, d.DepartmentName,
            ISNULL((SELECT SUM(DayCount) FROM Leaves
                    WHERE EmployeeID = e.EmployeeID 
                    AND Status = 'Onaylandi'), 0) AS KullanilanIzin
        FROM Employees e
        INNER JOIN Departments d ON e.DepartmentID = d.DepartmentID
        WHERE e.EmployeeID = @EmpID",
                new[] { new SqlParameter("@EmpID", employeeId) });

            if (dt.Rows.Count == 0)
                throw new Exception("Personel bulunamadı.");

            var row = dt.Rows[0];
            DateTime startDate = Convert.ToDateTime(row["StartDateOfWork"]);
            int kullanilan = Convert.ToInt32(row["KullanilanIzin"]);
            int kidemYil = IzinHakEdisHelper.KidemYilHesapla(startDate);
            int toplamHak = IzinHakEdisHelper.ToplamHakHesapla(kidemYil);

            return new PersonelProfilDto(
                EmployeeId: Convert.ToInt32(row["EmployeeID"]),
                FullName: row["FullName"].ToString()!,
                SicilNo: row["SicilNo"].ToString()!,
                Departman: row["DepartmentName"].ToString()!,
                Role: row["Role"].ToString()!,
                KidemYil: kidemYil,
                ToplamHak: toplamHak,
                KullanilanIzin: kullanilan,
                KalanIzin: toplamHak - kullanilan,
                IseBaslamaTarihi: startDate
            );
        }
    }
}
