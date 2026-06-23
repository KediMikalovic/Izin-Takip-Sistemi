using System.Data;
using Microsoft.Data.SqlClient;
using IzinTakip.Core.Abstractions;
using IzinTakip.Core.Security;

namespace IzinTakip.Core.Services
{
    public class AuthService : IAuthService
    {
        private readonly IDbHelper _db;

        public AuthService(IDbHelper db)
        {
            _db = db;
        }

        public async Task<DataRow?> GirisYapAsync(string sicilNo, string password)
        {
            string query = @"
                SELECT e.EmployeeID, e.FullName, e.DepartmentID, e.Role, e.SicilNo, e.Password,
                       d.DepartmentName
                FROM Employees e
                INNER JOIN Departments d ON e.DepartmentID = d.DepartmentID
                WHERE e.SicilNo = @sicil AND e.IsActive = 1";

            SqlParameter[] parameters = { new SqlParameter("@sicil", sicilNo) };

            DataTable dt = await _db.ExecuteQueryAsync(query, parameters);

            if (dt.Rows.Count > 0 && SecurityHelper.VerifyPassword(password, dt.Rows[0]["Password"].ToString()!))
                return dt.Rows[0];

            return null;
        }
    }
}
