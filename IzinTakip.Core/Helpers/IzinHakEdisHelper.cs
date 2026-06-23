namespace IzinTakip.Core.Helpers
{
    /// <summary>
    /// Turk Is Kanunu'na gore yillik izin hak edisi hesaplama.
    ///   1–4  yil calisan → yilda 14 gun
    ///   5–14 yil calisan → yilda 20 gun
    ///   15+  yil calisan → yilda 26 gun
    ///
    /// Onceki durum: Bu hesaplama AmirForm.cs VE RaporForm.cs'te birebir kopyalanmisti.
    /// Simdi tek kaynak (SRP + DRY).
    /// </summary>
    public static class IzinHakEdisHelper
    {
        /// <summary>
        /// Ise giris tarihinden bugunun tarihine kadar gecen tam yil sayisini hesaplar.
        /// </summary>
        public static int KidemYilHesapla(DateTime iseGirisTarihi)
        {
            int yil = DateTime.Today.Year - iseGirisTarihi.Year;
            if (iseGirisTarihi.AddYears(yil) > DateTime.Today)
                yil--;
            return Math.Max(yil, 0);
        }

        /// <summary>
        /// Calisilan yil sayisina gore toplam izin hakki gununu hesaplar (kumulatif).
        /// </summary>
        public static int ToplamHakHesapla(int calisilanYil)
        {
            int toplamHak = 0;
            for (int yil = 1; yil <= calisilanYil; yil++)
            {
                if (yil < 5)        toplamHak += 14;
                else if (yil < 15)  toplamHak += 20;
                else                toplamHak += 26;
            }
            return toplamHak;
        }

        /// <summary>
        /// Ise giris tarihinden toplam hakki dogrudan hesaplar (kolaylik metodu).
        /// </summary>
        public static int ToplamHakHesapla(DateTime iseGirisTarihi)
            => ToplamHakHesapla(KidemYilHesapla(iseGirisTarihi));

        /// <summary>
        /// Kalan izin = Toplam hak - kullanilan gun.
        /// </summary>
        public static int KalanIzinHesapla(DateTime iseGirisTarihi, int kullanilanGun)
            => ToplamHakHesapla(iseGirisTarihi) - kullanilanGun;

        /// <summary>
        /// Belirli bir kidem yilindaki yillik (o yila ait, kumulatif degil) izin hakki gununu dondurur.
        /// Avans izin validasyonunda "gelecek yil hak edisi" icin kullanilir.
        ///   0 yil → 0 (henuz tam yil calismadi)
        ///   1–4  yil → 14 gun/yil
        ///   5–14 yil → 20 gun/yil
        ///   15+  yil → 26 gun/yil
        /// </summary>
        public static int YillikHakHesapla(int kidemYil) => kidemYil switch
        {
            <= 0 => 0,
            < 5  => 14,
            < 15 => 20,
            _    => 26
        };
    }
}
