using System.Data;

namespace IzinTakip.Core.Abstractions
{
    /// <summary>
    /// Kimlik dogrulama servisi.
    /// LoginForm bu interface'e bagimlidir, SQL bilmez.
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Sicil no ve sifre ile giris yapar.
        /// Basariliysa kullanici bilgilerini DataRow olarak doner; degilse null.
        /// </summary>
        Task<DataRow?> GirisYapAsync(string sicilNo, string password);
    }
}
