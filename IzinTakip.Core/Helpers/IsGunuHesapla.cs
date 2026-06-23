using IzinTakip.Core.Abstractions;
using IzinTakip.Core.DataAccess;

namespace IzinTakip.Core.Helpers
{
    /// <summary>
    /// İş günü ve resmi tatil hesaplamalarını yöneten yardımcı sınıf.
    /// Tatil verileri <see cref="IResmiTatilRepository"/> üzerinden alınır;
    /// repository atanmamışsa <see cref="HardcodedTatilRepository"/> devreye girer.
    /// </summary>
    public static class DateHelper
    {
        // Program.cs'de TatilSaglayiciyiAyarla() çağrılarak atanır.
        // null ise HardcodedTatilRepository yedek olarak kullanılır.
        private static IResmiTatilRepository? _tatilRepo;

        /// <summary>
        /// Kullanılacak tatil repository'sini ayarlar.
        /// Program.cs (composition root) tarafından uygulama başlangıcında çağrılmalıdır.
        /// </summary>
        public static void TatilSaglayiciyiAyarla(IResmiTatilRepository repo)
        {
            _tatilRepo = repo;
        }

        // ── Aktif repository'yi döndürür (null ise hardcoded yedek) ─────────
        private static IResmiTatilRepository AktifRepo =>
            _tatilRepo ?? new HardcodedTatilRepository();

        // ── Önbellek: aynı yılın tatilleri tekrar hesaplanmaz ───────────────
        private static readonly Dictionary<int, HashSet<DateTime>> _onbellek = new();

        /// <summary>
        /// Başlangıç ve bitiş tarihleri arasındaki iş günü sayısını hesaplar.
        /// Hafta sonları ve Türkiye resmi tatilleri hariç tutulur.
        /// </summary>
        public static int IsGunuHesapla(DateTime baslangic, DateTime bitis)
        {
            int isGunu = 0;
            for (DateTime tarih = baslangic.Date; tarih <= bitis.Date; tarih = tarih.AddDays(1))
            {
                if (tarih.DayOfWeek != DayOfWeek.Saturday &&
                    tarih.DayOfWeek != DayOfWeek.Sunday &&
                    !ResmiTatilMi(tarih))
                {
                    isGunu++;
                }
            }
            return isGunu;
        }

        /// <summary>
        /// Verilen tarihin Türkiye resmi tatili olup olmadığını döndürür.
        /// </summary>
        public static bool ResmiTatilMi(DateTime tarih)
        {
            var tatiller = ResmiTatilleriGetir(tarih.Year);
            return tatiller.Contains(tarih.Date);
        }

        /// <summary>
        /// Verilen yıl için Türkiye resmi tatil listesini döndürür.
        /// Sonuçlar önbelleklenir.
        /// </summary>
        public static HashSet<DateTime> ResmiTatilleriGetir(int yil)
        {
            if (_onbellek.TryGetValue(yil, out var onbellekten))
                return onbellekten;

            var tatiller = new HashSet<DateTime>(AktifRepo.TatillerGetir(yil));
            _onbellek[yil] = tatiller;
            return tatiller;
        }
    }
}
