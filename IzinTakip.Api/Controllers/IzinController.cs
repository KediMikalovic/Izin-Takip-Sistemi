using IzinTakip.Core.Abstractions;
using IzinTakip.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims; 

namespace IzinTakip.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class IzinController : ControllerBase
    {
        private readonly IIzinService _izinService;

        public IzinController(IIzinService izinService)
        {
            _izinService = izinService;
        }

        // ── Yardımcı: Token'dan bilgi çek ───────────────────────

        private int GetEmployeeId() =>
            int.Parse(User.FindFirstValue("employeeId")!);

        private int GetDepartmentId() =>
            int.Parse(User.FindFirstValue("departmentId")!);

        private string GetFullName() =>
            User.FindFirstValue("fullName")!;

        private string GetRole() =>
            User.FindFirstValue(ClaimTypes.Role)!;

        // ── Personel endpoint'leri ───────────────────────────────

        /// <summary>Giriş yapan personelin kendi izin geçmişini getirir.</summary>
        [HttpGet("gecmis")]
        public async Task<IActionResult> IzinGecmisi()
        {
            var dt = await _izinService.PersonelIzinleriniGetirAsync(GetEmployeeId());
            return Ok(DataTableToList(dt));
        }

        /// <summary>Personel yeni izin talebi oluşturur.</summary>
        [HttpPost("talep")]
        public async Task<IActionResult> IzinTalebi([FromBody] IzinTalepRequest request)
        {
            var istek = new IzinEkleIstek(
                EmployeeId: GetEmployeeId(),
                DepartmanId: GetDepartmentId(),
                AmirId: request.AmirId,
                PersonelAd: GetFullName(),
                Baslangic: request.Baslangic,
                Bitis: request.Bitis,
                KalanIzin: request.KalanIzin
            );

            var sonuc = await _izinService.IzinEkleAsync(istek);

            if (!sonuc.Basarili)
                return BadRequest(sonuc.HataMesaji);

            return Ok(sonuc.Mesaj);
        }

        /// <summary>Giriş yapan personelin profil ve izin özeti bilgilerini getirir.</summary>
        [HttpGet("profil")]
        public async Task<IActionResult> ProfilGetir()
        {
            try
            {
                var profil = await _izinService.PersonelProfilGetirAsync(GetEmployeeId());
                return Ok(profil);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }


        /// <summary>Personel avans izin talebi oluşturur.</summary>
        [HttpPost("avans-talep")]
        public async Task<IActionResult> AvansIzinTalebi([FromBody] IzinTalepRequest request)
        {
            var istek = new IzinEkleIstek(
                EmployeeId: GetEmployeeId(),
                DepartmanId: GetDepartmentId(),
                AmirId: request.AmirId,
                PersonelAd: GetFullName(),
                Baslangic: request.Baslangic,
                Bitis: request.Bitis,
                KalanIzin: request.KalanIzin
            );

            var sonuc = await _izinService.AvansIzinEkleAsync(istek);

            if (!sonuc.Basarili)
                return BadRequest(sonuc.HataMesaji);

            return Ok(sonuc.Mesaj);
        }

        // ── Amir endpoint'leri ───────────────────────────────────

        /// <summary>Amir kendi departmanının personel listesini getirir.</summary>
        [HttpGet("departman/personeller")]
        [Authorize(Policy = "AmirVeyaYonetici")]
        public async Task<IActionResult> DepartmanPersonelleri()
        {
            var dt = await _izinService.PersonelListesiGetirAsync(GetDepartmentId());
            return Ok(DataTableToList(dt));
        }

        /// <summary>Belirli tarih aralığında izinli personelleri getirir.</summary>
        [HttpGet("departman/aralik")]
        [Authorize(Policy = "AmirVeyaYonetici")]
        public async Task<IActionResult> AralikIzinliler(
            [FromQuery] DateTime baslangic,
            [FromQuery] DateTime bitis)
        {
            var dt = await _izinService.AralikIzinlileriAmirAsync(
                baslangic, bitis, GetDepartmentId());
            return Ok(DataTableToList(dt));
        }

        // ── Yönetici endpoint'leri ───────────────────────────────

        /// <summary>Yönetici tüm izin taleplerini filtreli getirir.</summary>
        [HttpGet("talepler")]
        [Authorize(Policy = "YoneticiOnly")]
        public async Task<IActionResult> TalepleriGetir(
            [FromQuery] string durum = "Tümü",
            [FromQuery] int departmanId = 0,
            [FromQuery] string personelAdi = "",
            [FromQuery] DateTime? bas = null,
            [FromQuery] DateTime? bit = null)
        {
            var dt = await _izinService.TalepleriGetirAsync(
                durum, departmanId, personelAdi, bas, bit);
            return Ok(DataTableToList(dt));
        }

        /// <summary>Yönetici izni onaylar.</summary>
        [HttpPost("{leaveId}/onayla")]
        [Authorize(Policy = "YoneticiOnly")]
        public async Task<IActionResult> Onayla(int leaveId, [FromQuery] int gunSayisi)
        {
            var sonuc = await _izinService.OnaylaAsync(
                leaveId, GetEmployeeId(), GetFullName(), gunSayisi);

            if (!sonuc.Basarili)
                return BadRequest(sonuc.HataMesaji);

            return Ok(sonuc.Mesaj);
        }

        /// <summary>Yönetici izni reddeder.</summary>
        [HttpPost("{leaveId}/reddet")]
        [Authorize(Policy = "YoneticiOnly")]
        public async Task<IActionResult> Reddet(int leaveId, [FromBody] RedRequest request)
        {
            var sonuc = await _izinService.ReddetAsync(
                leaveId, GetEmployeeId(), request.RedSebebi, GetFullName());

            if (!sonuc.Basarili)
                return BadRequest(sonuc.HataMesaji);

            return Ok(sonuc.Mesaj);
        }

        /// <summary>Yönetici izni iptal eder.</summary>
        [HttpPost("{leaveId}/iptal")]
        [Authorize(Policy = "YoneticiOnly")]
        public async Task<IActionResult> IptalEt(int leaveId)
        {
            var sonuc = await _izinService.IzinIptalEtYoneticiAsync(
                leaveId, GetEmployeeId(), GetFullName());

            if (!sonuc.Basarili)
                return BadRequest(sonuc.HataMesaji);

            return Ok(sonuc.Mesaj);
        }


        // ── Personel izin talebi (yeni akış) ────────────────────────

        /// <summary>Personel izin talebi oluşturur → Amir Onayında statüsüyle kaydedilir.</summary>

        [HttpPost("personel-talep")]
        public async Task<IActionResult> PersonelIzinTalebi([FromBody] IzinTalepRequest request)
        {
            if (GetRole() != "Personel")
                return Forbid();

            var istek = new IzinEkleIstek(
                EmployeeId: GetEmployeeId(),
                DepartmanId: GetDepartmentId(),
                AmirId: request.AmirId,
                PersonelAd: GetFullName(),
                Baslangic: request.Baslangic,
                Bitis: request.Bitis,
                KalanIzin: request.KalanIzin
            );

            var sonuc = await _izinService.PersonelIzinEkleAsync(istek);

            if (!sonuc.Basarili)
                return BadRequest(sonuc.HataMesaji);

            return Ok(sonuc.Mesaj);
        }

        //[HttpPost("personel-talep")]
        //public async Task<IActionResult> PersonelIzinTalebi([FromBody] IzinTalepRequest request)
        //{
        //    var role = GetRole();
        //    return Ok($"Role: {role}"); // geçici test
        //}

        // ── Amir endpoint'leri (yeni akış) ──────────────────────────

        /// <summary>Amir onayında bekleyen talepleri getirir.</summary>
        [HttpGet("amir-bekleyenler")]
        [Authorize(Policy = "AmirVeyaYonetici")]
        public async Task<IActionResult> AmirBekleyenler()
        {
            var dt = await _izinService.AmirOnayindakileriGetirAsync(GetDepartmentId());
            return Ok(DataTableToList(dt));
        }

        /// <summary>Amir talebi onaylar → Beklemede'ye geçer, Yönetici görür.</summary>
        [HttpPost("{leaveId}/amir-onayla")]
        [Authorize(Policy = "AmirVeyaYonetici")]
        public async Task<IActionResult> AmirOnayla(int leaveId, [FromQuery] string personelAd)
        {
            if (GetRole() != "Amir")
                return Forbid();

            var sonuc = await _izinService.AmirOnaylaAsync(leaveId, GetEmployeeId(), personelAd);

            if (!sonuc.Basarili)
                return BadRequest(sonuc.HataMesaji);

            return Ok(sonuc.Mesaj);
        }

        /// <summary>Amir talebi reddeder.</summary>
        [HttpPost("{leaveId}/amir-reddet")]
        [Authorize(Policy = "AmirVeyaYonetici")]
        public async Task<IActionResult> AmirReddet(int leaveId, [FromBody] RedRequest request)
        {
            if (GetRole() != "Amir")
                return Forbid();

            var sonuc = await _izinService.AmirReddetAsync(
                leaveId, GetEmployeeId(), request.RedSebebi, request.PersonelAd ?? "");

            if (!sonuc.Basarili)
                return BadRequest(sonuc.HataMesaji);

            return Ok(sonuc.Mesaj);
        }

        // DataTable'ı JSON serialize edilebilir formata çevirir
        private static List<Dictionary<string, object?>> DataTableToList(DataTable dt)
        {
            var liste = new List<Dictionary<string, object?>>();
            foreach (DataRow row in dt.Rows)
            {
                var dict = new Dictionary<string, object?>();
                foreach (DataColumn col in dt.Columns)
                    dict[col.ColumnName] = row[col] == DBNull.Value ? null : row[col];
                liste.Add(dict);
            }
            return liste;
        }
    }

    // ── Request DTO'ları ─────────────────────────────────────────
    public record IzinTalepRequest(
        int AmirId,
        DateTime Baslangic,
        DateTime Bitis,
        int KalanIzin);

    public record RedRequest(string RedSebebi, string? PersonelAd = null);



}


