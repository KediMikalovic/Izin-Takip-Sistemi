using System.Data;
using IzinTakip.Core.Models;

namespace IzinTakip.Core.Abstractions
{
    /// <summary>
    /// Izin islemleri servisi — tum izin is mantigi burada.
    /// AmirForm ve YoneticiForm bu interface'e bagimlidir.
    /// </summary>
    public interface IIzinService
    {
        // ── Sorgular ─────────────────────────────────────────────

        /// <summary>Departmandaki personelleri hak edis bilgileriyle getirir (AmirForm).
        /// Opsiyonel aralikBas/aralikBit verilirse yalnizca o aralikta ONAYLI izni olan personeller listelenir.</summary>
        Task<DataTable> PersonelListesiGetirAsync(int departmanId, DateTime? aralikBas = null, DateTime? aralikBit = null);

        /// <summary>Bir personelin izin gecmisini getirir.</summary>
        Task<DataTable> PersonelIzinleriniGetirAsync(int employeeId);

        /// <summary>Tarih araliginda izinli personelleri getirir (Amir — kendi departmani).</summary>
        Task<DataTable> AralikIzinlileriAmirAsync(DateTime baslangic, DateTime bitis, int departmanId);

        /// <summary>Tarih araliginda izinli personelleri getirir (Yonetici — tum departmanlar).</summary>
        Task<DataTable> AralikIzinlileriYoneticiAsync(DateTime baslangic, DateTime bitis);

        /// <summary>Filtreye gore izin taleplerini getirir (YoneticiForm).
        /// Opsiyonel bas/bit verilirse yalnizca o aralikta baslayan talepler listelenir.</summary>
        Task<DataTable> TalepleriGetirAsync(string durumFiltresi, int departmanId, string personelAdi, DateTime? bas = null, DateTime? bit = null);

        // ── Izin islemleri ───────────────────────────────────────

        /// <summary>Yeni izin talebi olusturur. Bakiye, cakisma ve kota kontrolleri serviste yapilir.</summary>
        Task<IzinIslemSonucu> IzinEkleAsync(IzinEkleIstek istek);

        /// <summary>
        /// Avans izin talebi olusturur. Kalan izin sifir veya altinda olan personel icin gelecek yilin hakkindan avans kullanilir.
        /// Validasyon: mevcut acik + talepGun &lt;= gelecekYilHak
        /// </summary>
        Task<IzinIslemSonucu> AvansIzinEkleAsync(IzinEkleIstek istek);

        /// <summary>Beklemedeki izni iptal eder (Soft Delete).</summary>
        Task<IzinIslemSonucu> IzinIptalEtAmirAsync(int leaveId, int amirId, string personelAd);

        /// <summary>Beklemedeki iznin tarihlerini gunceller (Amir).</summary>
        Task<IzinIslemSonucu> IzinGuncelleAmirAsync(int leaveId, IzinGuncelleIstek istek);

        /// <summary>Yonetici yetkisiyle izni gunceller (tum durumlar icin).</summary>
        Task<IzinIslemSonucu> IzinGuncelleYoneticiAsync(int leaveId, IzinGuncelleIstek istek);

        /// <summary>Izin talebini onaylar (Yonetici).</summary>
        Task<IzinIslemSonucu> OnaylaAsync(int leaveId, int yoneticiId, string personelAd, int gunSayisi);

        /// <summary>Izin talebini reddeder (Yonetici).</summary>
        Task<IzinIslemSonucu> ReddetAsync(int leaveId, int yoneticiId, string redSebebi, string personelAd);

        /// <summary>Yonetici yetkisiyle izni iptal eder.</summary>
        Task<IzinIslemSonucu> IzinIptalEtYoneticiAsync(int leaveId, int yoneticiId, string personelAd);


        // ── Personel izin akışı (yeni) ───────────────────────────

        /// <summary>
        /// Personel izin talebi oluşturur. Statü "Amir Onayinda" olarak kaydedilir.
        /// Amir veya Yönetici açarsa mevcut IzinEkleAsync kullanılmalı.
        /// </summary>
        Task<IzinIslemSonucu> PersonelIzinEkleAsync(IzinEkleIstek istek);

        /// <summary>
        /// Amir, "Amir Onayinda" statüsündeki talebi onaylar.
        /// Statü "Beklemede"ye geçer, Yönetici kuyruğuna düşer.
        /// </summary>
        Task<IzinIslemSonucu> AmirOnaylaAsync(int leaveId, int amirId, string personelAd);

        /// <summary>
        /// Amir, "Amir Onayinda" statüsündeki talebi reddeder.
        /// </summary>
        Task<IzinIslemSonucu> AmirReddetAsync(int leaveId, int amirId, string redSebebi, string personelAd);

        /// <summary>
        /// Amir onayında bekleyen talepleri getirir (kendi departmanı).
        /// </summary>
        Task<DataTable> AmirOnayindakileriGetirAsync(int departmanId);


        /// <summary>Personelin profil ve izin özeti bilgilerini getirir.</summary>
        Task<PersonelProfilDto> PersonelProfilGetirAsync(int employeeId);

    }
}
