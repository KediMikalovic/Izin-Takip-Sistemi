using IzinTakip.Core.Abstractions;
using IzinTakip.Core.DataAccess;
using IzinTakip.Core.Helpers;
using IzinTakip.Core.Services;

namespace IzinTakipSistemi
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            // ── Manual Dependency Injection (Composition Root) ────
            // Tüm bağımlılıklar burada oluşturulup formlara enjekte edilir.
            // WinForms'ta DI container gerekmez — bu kadarı yeterli.

            // Tatil sağlayıcısını ayarla: Nager.Date (sabit tatiller) + HijriCalendar (dini bayramlar)
            // Sorun olursa HardcodedTatilRepository otomatik devreye girer (DateHelper içinde).
            DateHelper.TatilSaglayiciyiAyarla(new NagerPlusHijriTatilRepository());

            IDbHelper     dbHelper     = new SqlHelper();
            IAuthService  authService  = new AuthService(dbHelper);
            IIzinService  izinService  = new IzinService(dbHelper);
            IRaporService raporService = new RaporService(dbHelper);

            Application.Run(new LoginForm(authService, izinService, raporService));
        }
    }
}