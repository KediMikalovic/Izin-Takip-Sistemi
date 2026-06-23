using System.Globalization;
using IzinTakip.Core.Abstractions;
using Nager.Date;

namespace IzinTakip.Core.DataAccess
{
    /// <summary>
    /// Nager.Date (sabit Türk tatilleri) + HijriCalendar (hareketli İslami bayramlar)
    /// kombinasyonunu kullanan tatil repository'si.
    /// İnternet bağlantısı gerektirmez — tamamen offline çalışır.
    /// </summary>
    public class NagerPlusHijriTatilRepository : IResmiTatilRepository
    {
        // Hesaplanan yıllar önbelleğe alınır; aynı yıl için tekrar hesaplanmaz.
        private readonly Dictionary<int, HashSet<DateTime>> _onbellek = new();

        public Task<IEnumerable<DateTime>> TatillerGetirAsync(int yil)
        {
            return Task.FromResult(TatillerGetir(yil));
        }

        public IEnumerable<DateTime> TatillerGetir(int yil)
        {
            if (_onbellek.TryGetValue(yil, out var onbellekten))
                return onbellekten;

            var tatiller = new HashSet<DateTime>();

            // ── 1. Nager.Date ile sabit Türk resmi tatilleri ────────────────
            // HolidaySystem (Nager.Date 2.x); Yılbaşı, 23 Nisan, 1 Mayıs, 19 Mayıs,
            // 15 Temmuz, 30 Ağustos ve 29 Ekim'i offline olarak sağlar.
            try
            {
                var nagerTatiller = HolidaySystem.GetHolidays(yil, CountryCode.TR);
                foreach (var tatil in nagerTatiller)
                {
                    tatiller.Add(tatil.Date);
                }
            }
            catch
            {
                // Nager.Date beklenmedik bir hata fırlatırsa sabit listeye dön
                EkleSabitTatiller(tatiller, yil);
            }

            // 28 Ekim (Cumhuriyet Bayramı arifesi) Nager.Date'de yarım gün olarak işaretlenir;
            // uygulamamız tam gün saydığından eksikse manuel ekle.
            tatiller.Add(new DateTime(yil, 10, 28));

            // ── 2. HijriCalendar ile hareketli İslami bayramlar ─────────────
            // Ramazan Bayramı : Şevval 1, 2, 3  (3 gün)
            // Kurban Bayramı  : Zilhicce 10, 11, 12, 13  (4 gün)
            EkleHijriTatiller(tatiller, yil);

            _onbellek[yil] = tatiller;
            return tatiller;
        }

        // ────────────────────────────────────────────────────────────────────
        // Yardımcı: sabit tatilleri elle ekle (Nager.Date fallback)
        // ────────────────────────────────────────────────────────────────────
        private static void EkleSabitTatiller(HashSet<DateTime> tatiller, int yil)
        {
            tatiller.Add(new DateTime(yil,  1,  1));  // Yılbaşı
            tatiller.Add(new DateTime(yil,  4, 23));  // Ulusal Egemenlik ve Çocuk Bayramı
            tatiller.Add(new DateTime(yil,  5,  1));  // Emek ve Dayanışma Günü
            tatiller.Add(new DateTime(yil,  5, 19));  // Gençlik ve Spor Bayramı
            tatiller.Add(new DateTime(yil,  7, 15));  // Demokrasi ve Millî Birlik Günü
            tatiller.Add(new DateTime(yil,  8, 30));  // Zafer Bayramı
            tatiller.Add(new DateTime(yil, 10, 28));  // Cumhuriyet Bayramı arifesi
            tatiller.Add(new DateTime(yil, 10, 29));  // Cumhuriyet Bayramı
        }

        // ────────────────────────────────────────────────────────────────────
        // Yardımcı: Hicri takvimden Ramazan ve Kurban Bayramı günlerini hesapla
        // ────────────────────────────────────────────────────────────────────
        private static void EkleHijriTatiller(HashSet<DateTime> tatiller, int miladi_yil)
        {
            var hicri = new HijriCalendar();

            // Verilen miladi yılın başlangıcını Hicri takvime çevir
            // ve o yıl içindeki (~12–13 ay) tüm Hicri ayları tara
            var baslangic = new DateTime(miladi_yil, 1, 1);
            var bitis     = new DateTime(miladi_yil, 12, 31);

            // Hicri yıl miladi yıldan ~11 gün kısa; bir miladi yıl içinde
            // genellikle 2 farklı Hicri yıla denk gelebilir.
            int hicriYilBaslangic = hicri.GetYear(baslangic);
            int hicriYilBitis     = hicri.GetYear(bitis);

            for (int hYil = hicriYilBaslangic; hYil <= hicriYilBitis; hYil++)
            {
                // Ramazan Bayramı: Şevval ayı (10. ay) 1–3
                EkleHijriGunler(tatiller, hicri, hYil, 10, 1, 3, miladi_yil);

                // Kurban Bayramı: Zilhicce ayı (12. ay) 10–13
                EkleHijriGunler(tatiller, hicri, hYil, 12, 10, 13, miladi_yil);
            }
        }

        private static void EkleHijriGunler(
            HashSet<DateTime> tatiller,
            HijriCalendar hicri,
            int hicriYil, int hicriAy,
            int baslangicGun, int bitisGun,
            int hedefMiladiYil)
        {
            for (int gun = baslangicGun; gun <= bitisGun; gun++)
            {
                try
                {
                    // HijriCalendar.ToDateTime: Hicri→Miladi dönüşüm
                    DateTime miladi = hicri.ToDateTime(hicriYil, hicriAy, gun, 0, 0, 0, 0);

                    // Yalnızca hedef miladi yıla ait günleri ekle
                    if (miladi.Year == hedefMiladiYil)
                    {
                        tatiller.Add(miladi.Date);
                    }
                }
                catch (ArgumentOutOfRangeException)
                {
                    // Geçersiz Hicri tarih (ay sonu taşması vb.) — atla
                }
            }
        }
    }
}
