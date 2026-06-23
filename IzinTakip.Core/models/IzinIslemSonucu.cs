namespace IzinTakip.Core.Models
{
    /// <summary>
    /// Servis → Form iletisim nesnesi.
    /// Form'lar sadece Basarili/HataMesaji kontrol eder, is mantigi bilmez.
    /// </summary>
    public record IzinIslemSonucu(
        bool Basarili,
        string Mesaj,
        string? HataMesaji = null
    )
    {
        public static IzinIslemSonucu Basari(string mesaj) => new(true, mesaj);
        public static IzinIslemSonucu Hata(string hataMesaji) => new(false, string.Empty, hataMesaji);
    }
}
