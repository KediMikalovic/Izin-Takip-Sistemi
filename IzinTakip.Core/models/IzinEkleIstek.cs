namespace IzinTakip.Core.Models
{
    /// <summary>
    /// Yeni izin talebi olusturmak icin form'dan servise gonderilen veri nesnesi.
    /// </summary>
    public record IzinEkleIstek(
        int EmployeeId,
        int DepartmanId,
        int AmirId,
        string PersonelAd,
        DateTime Baslangic,
        DateTime Bitis,
        int KalanIzin
    );

    /// <summary>
    /// Mevcut izni guncellemek icin gonderilen veri nesnesi.
    /// </summary>
    public record IzinGuncelleIstek(
        int EmployeeId,
        int DepartmanId,
        int YapanId,
        string PersonelAd,
        DateTime YeniBaslangic,
        DateTime YeniBitis,
        int KalanIzin
    );
}
