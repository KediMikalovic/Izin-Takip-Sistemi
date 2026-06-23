using System.Data;

namespace IzinTakip.Core.Abstractions
{
    /// <summary>
    /// Rapor ve istatistik servisi.
    /// RaporForm ve LogForm bu interface'e bagimlidir.
    /// </summary>
    public interface IRaporService
    {
        Task<DataTable> DepartmanIstatistikleriniGetirAsync(DateTime? bas = null, DateTime? bit = null);
        Task<DataTable> AylikYogunluguGetirAsync(DateTime? bas = null, DateTime? bit = null);
        Task<DataTable> EnCokIzinAlanlariGetirAsync(DateTime? bas = null, DateTime? bit = null);
        Task<DataTable> AktifIzinlileriGetirAsync();

        /// <summary>Tum personel listesi — hak edis, kalan izin hesaplamalariyla birlikte.</summary>
        Task<DataTable> PersonelDetaylariniGetirAsync();

        /// <summary>Departman listesini getirir (ComboBox icin).</summary>
        Task<DataTable> DepartmanlariGetirAsync();

        /// <summary>Audit log kayitlarini getirir (Turkce islem tipi ile).</summary>
        Task<DataTable> LoglariGetirAsync();

        /// <summary>Belirli bir departmana ait audit loglarini getirir (Amir paneli icin).</summary>
        Task<DataTable> LoglariGetirAmirAsync(int departmanId, DateTime? baslangic = null, DateTime? bitis = null);

        /// <summary>Bugun izinli olan aktif calisanlari departman bazli filtreler.</summary>
        Task<DataTable> DepartmanAktifIzinlileriGetirAsync(int departmanId);

        /// <summary>
        /// Departmandaki tüm izinleri getirir; isteğe bağlı kişi adı, tarih aralığı ve durum filtresiyle.
        /// null parametre = o filtre uygulanmaz.
        /// </summary>
        Task<DataTable> DepartmanTumIzinleriGetirAsync(
            int departmanId,
            string? personelAd,
            DateTime? baslangicFiltre,
            DateTime? bitisFiltre,
            string? durum);
    }
}
