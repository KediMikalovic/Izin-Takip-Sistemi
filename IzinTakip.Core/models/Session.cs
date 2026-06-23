namespace IzinTakip.Core.Models
{
    public static class Session
    {
        public static int CurrentEmployeeID { get; set; }
        public static string? CurrentFullName { get; set; }
        public static string? CurrentSicilNo { get; set; }
        public static int CurrentDepartmentID { get; set; }
        public static string? CurrentDepartmentName { get; set; }
        public static string? CurrentRole { get; set; }

        /// <summary>
        /// Logout sırasında tüm oturum bilgilerini temizler.
        /// </summary>
        public static void Clear()
        {
            CurrentEmployeeID   = 0;
            CurrentFullName     = null;
            CurrentSicilNo      = null;
            CurrentDepartmentID = 0;
            CurrentDepartmentName = null;
            CurrentRole         = null;
        }
    }
}