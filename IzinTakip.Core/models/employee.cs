using System;

namespace IzinTakip.Core.Models
{
    public class Employee
    {
        public int EmployeeID { get; set; }
        public string FullName { get; set; }
        public string SicilNo { get; set; } // Username yerine geldi
        public string Password { get; set; }
        public int DepartmentID { get; set; }
        public string Role { get; set; }
        public DateTime StartDateOfWork { get; set; } // Yeni eklendi
        public bool IsActive { get; set; }
    }
}