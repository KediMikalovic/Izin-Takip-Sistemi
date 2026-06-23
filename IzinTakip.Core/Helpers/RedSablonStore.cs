using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace IzinTakip.Core.Helpers
{
    /// <summary>
    /// Kullanıcı tarafından eklenen özel red sebebi şablonlarını
    /// %LOCALAPPDATA%\IzinTakipSistemi\red_sablonlari.json dosyasında saklar.
    ///
    /// Designer'da hazır bulunan sabit şablonlardan ayrıdır; sadece runtime'da
    /// kullanıcının "+ Şablona Ekle" ile eklediği gerekçeleri kalıcı tutar.
    /// </summary>
    public static class RedSablonStore
    {
        private const string KlasorAdi = "IzinTakipSistemi";
        private const string DosyaAdi  = "red_sablonlari.json";

        private static string DosyaYolu()
        {
            string baseDir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string klasor = Path.Combine(baseDir, KlasorAdi);
            Directory.CreateDirectory(klasor);
            return Path.Combine(klasor, DosyaAdi);
        }

        /// <summary>Kayıtlı özel şablonları yükler. Dosya yoksa boş liste döner.</summary>
        public static List<string> Yukle()
        {
            try
            {
                string yol = DosyaYolu();
                if (!File.Exists(yol)) return new List<string>();

                string json = File.ReadAllText(yol);
                if (string.IsNullOrWhiteSpace(json)) return new List<string>();

                var liste = JsonSerializer.Deserialize<List<string>>(json);
                return liste ?? new List<string>();
            }
            catch
            {
                // Dosya bozuksa veya okunamazsa sessizce boş liste dön
                return new List<string>();
            }
        }

        /// <summary>Verilen şablon listesini JSON olarak diske yazar.</summary>
        public static void Kaydet(IEnumerable<string> sablonlar)
        {
            string yol = DosyaYolu();
            var liste = sablonlar?.Where(s => !string.IsNullOrWhiteSpace(s)).ToList()
                        ?? new List<string>();
            string json = JsonSerializer.Serialize(liste, new JsonSerializerOptions
            {
                WriteIndented = true
            });
            File.WriteAllText(yol, json);
        }
    }
}
