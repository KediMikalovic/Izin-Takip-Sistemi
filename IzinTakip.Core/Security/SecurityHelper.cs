using System;
using System.Security.Cryptography;
using System.Text;

namespace IzinTakip.Core.Security
{
    /// <summary>
    /// Şifre güvenliği için yardımcı sınıf.
    /// PBKDF2 + rastgele salt kullanarak rainbow table saldırılarına karşı koruma sağlar.
    /// Format: salt:hash (her kullanıcının salt'ı farklıdır)
    /// </summary>
    public static class SecurityHelper
    {
        private const int SaltSize = 32;       // 256 bit salt
        private const int HashSize = 32;       // 256 bit hash
        private const int Iterations = 100_000; // PBKDF2 iterasyon sayısı

        /// <summary>
        /// Yeni bir şifreyi tuzlayarak hashler.
        /// Dönen string "salt:hash" formatındadır ve veritabanına kaydedilir.
        /// </summary>
        public static string HashPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
                return string.Empty;

            // Rastgele salt üret
            byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);

            // PBKDF2 ile hash hesapla
            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(password),
                salt,
                Iterations,
                HashAlgorithmName.SHA256,
                HashSize);

            // "salt:hash" formatında döndür
            return $"{Convert.ToBase64String(salt)}:{Convert.ToBase64String(hash)}";
        }

        /// <summary>
        /// Girilen şifreyi veritabanındaki kayıtlı hash ile karşılaştırır.
        /// Hem yeni format (salt:hash) hem de eski format (düz SHA-256) desteklenir.
        /// </summary>
        public static bool VerifyPassword(string password, string storedHash)
        {
            if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(storedHash))
                return false;

            // Yeni format: "salt:hash"
            if (storedHash.Contains(':'))
            {
                string[] parts = storedHash.Split(':');
                if (parts.Length != 2) return false;

                byte[] salt = Convert.FromBase64String(parts[0]);
                byte[] expectedHash = Convert.FromBase64String(parts[1]);

                byte[] actualHash = Rfc2898DeriveBytes.Pbkdf2(
                    Encoding.UTF8.GetBytes(password),
                    salt,
                    Iterations,
                    HashAlgorithmName.SHA256,
                    HashSize);

                return CryptographicOperations.FixedTimeEquals(actualHash, expectedHash);
            }

            // Eski format: düz SHA-256 (geçiş dönemi uyumluluğu)
            using var sha256 = SHA256.Create();
            byte[] legacyHash = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            StringBuilder sb = new StringBuilder();
            foreach (byte b in legacyHash) sb.Append(b.ToString("x2"));
            return sb.ToString() == storedHash;
        }
    }
}
