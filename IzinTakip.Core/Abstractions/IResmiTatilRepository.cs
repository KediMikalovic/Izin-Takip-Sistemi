namespace IzinTakip.Core.Abstractions
{
    /// <summary>
    /// Türkiye resmi tatillerini sağlayan repository arayüzü.
    /// Farklı kaynaklar (hardcoded, Nager.Date, API) bu arayüzü uygulayabilir.
    /// </summary>
    public interface IResmiTatilRepository
    {
        /// <summary>
        /// Belirtilen yıla ait tüm resmi tatil tarihlerini döndürür (async).
        /// </summary>
        Task<IEnumerable<DateTime>> TatillerGetirAsync(int yil);

        /// <summary>
        /// Belirtilen yıla ait tüm resmi tatil tarihlerini döndürür (sync).
        /// İş günü hesabı gibi senkron bağlamlarda kullanılır.
        /// </summary>
        IEnumerable<DateTime> TatillerGetir(int yil);
    }
}
