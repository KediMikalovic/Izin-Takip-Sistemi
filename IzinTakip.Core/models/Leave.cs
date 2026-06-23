namespace IzinTakip.Core.Models
{
    /// <summary>
    /// Veritabanindaki Leaves tablosunun C# karsiligi.
    /// </summary>
    public class Leave
    {
        public int LeaveID { get; set; }
        public int EmployeeID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int DayCount { get; set; }
        public LeaveStatus Status { get; set; }
        public int? AmirID { get; set; }
        public string? ManagerNote { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    /// <summary>
    /// Izin durum enum'i — veritabanindaki CHECK constraint ile uyumlu.
    /// Magic string'ler yerine bu enum kullanilir.
    /// </summary>
    public enum LeaveStatus
    {
        Beklemede,
        Onaylandi,
        Reddedildi,
        IptalEdildi
    }

    /// <summary>
    /// Enum ↔ DB string donusum extension'lari.
    /// DB'de ASCII-safe string'ler tutulur: 'Iptal Edildi' (bosluklu).
    /// </summary>
    public static class LeaveStatusExtensions
    {
        public static string ToDbString(this LeaveStatus status) => status switch
        {
            LeaveStatus.Beklemede    => "Beklemede",
            LeaveStatus.Onaylandi    => "Onaylandi",
            LeaveStatus.Reddedildi   => "Reddedildi",
            LeaveStatus.IptalEdildi  => "Iptal Edildi",
            _ => throw new ArgumentOutOfRangeException(nameof(status))
        };

        public static LeaveStatus ParseLeaveStatus(string dbValue) => dbValue switch
        {
            "Beklemede"     => LeaveStatus.Beklemede,
            "Onaylandi"     => LeaveStatus.Onaylandi,
            "Reddedildi"    => LeaveStatus.Reddedildi,
            "Iptal Edildi"  => LeaveStatus.IptalEdildi,
            _ => throw new ArgumentException($"Bilinmeyen izin durumu: {dbValue}", nameof(dbValue))
        };
    }
}
