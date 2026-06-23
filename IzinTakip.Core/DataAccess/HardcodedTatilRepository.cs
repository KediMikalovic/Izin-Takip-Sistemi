using IzinTakip.Core.Abstractions;

namespace IzinTakip.Core.DataAccess
{
    /// <summary>
    /// Mevcut hardcoded tatil listesini <see cref="IResmiTatilRepository"/> arayüzüne saran sınıf.
    /// NagerPlusHijriTatilRepository kullanılamadığında yedek (fallback) olarak devreye girer.
    /// </summary>
    public class HardcodedTatilRepository : IResmiTatilRepository
    {
        public Task<IEnumerable<DateTime>> TatillerGetirAsync(int yil)
        {
            return Task.FromResult(TatillerGetir(yil));
        }

        public IEnumerable<DateTime> TatillerGetir(int yil)
        {
            var tatiller = new HashSet<DateTime>
            {
                // Sabit tarihli resmi tatiller
                new DateTime(yil, 1,  1),   // Yılbaşı
                new DateTime(yil, 4, 23),   // Ulusal Egemenlik ve Çocuk Bayramı
                new DateTime(yil, 5,  1),   // Emek ve Dayanışma Günü
                new DateTime(yil, 5, 19),   // Atatürk'ü Anma, Gençlik ve Spor Bayramı
                new DateTime(yil, 7, 15),   // Demokrasi ve Millî Birlik Günü
                new DateTime(yil, 8, 30),   // Zafer Bayramı
                new DateTime(yil, 10, 28),  // Cumhuriyet Bayramı arifesi
                new DateTime(yil, 10, 29),  // Cumhuriyet Bayramı
            };

            // Hareketli dini bayramlar — 2024–2026 arası hardcoded
            var diniTatiller = new Dictionary<int, DateTime[]>
            {
                [2024] = new[]
                {
                    new DateTime(2024, 4, 10), new DateTime(2024, 4, 11), new DateTime(2024, 4, 12), // Ramazan
                    new DateTime(2024, 6, 16), new DateTime(2024, 6, 17), new DateTime(2024, 6, 18), new DateTime(2024, 6, 19), // Kurban
                },
                [2025] = new[]
                {
                    new DateTime(2025, 3, 30), new DateTime(2025, 3, 31), new DateTime(2025, 4,  1), // Ramazan
                    new DateTime(2025, 6,  6), new DateTime(2025, 6,  7), new DateTime(2025, 6,  8), new DateTime(2025, 6,  9), // Kurban
                },
                [2026] = new[]
                {
                    new DateTime(2026, 3, 20), new DateTime(2026, 3, 21), new DateTime(2026, 3, 22), // Ramazan
                    new DateTime(2026, 5, 27), new DateTime(2026, 5, 28), new DateTime(2026, 5, 29), new DateTime(2026, 5, 30), // Kurban
                },
            };

            if (diniTatiller.TryGetValue(yil, out var dini))
            {
                foreach (var d in dini) tatiller.Add(d);
            }

            return tatiller;
        }
    }
}
