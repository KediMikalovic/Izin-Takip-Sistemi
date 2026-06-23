using IzinTakip.Api.Helpers;
using IzinTakip.Core.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace IzinTakip.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly JwtHelper _jwtHelper;

        public AuthController(IAuthService authService, JwtHelper jwtHelper)
        {
            _authService = authService;
            _jwtHelper = jwtHelper;
        }

        /// <summary>
        /// Sicil no ve şifre ile giriş yapar, JWT token döner.
        /// </summary>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.SicilNo) ||
                string.IsNullOrWhiteSpace(request.Password))
                return BadRequest("Sicil no ve şifre boş olamaz.");

            var row = await _authService.GirisYapAsync(request.SicilNo, request.Password);

            if (row == null)
                return Unauthorized("Sicil no veya şifre hatalı.");

            // DataRow'dan bilgileri çek
            int employeeId = Convert.ToInt32(row["EmployeeID"]);
            string fullName = row["FullName"].ToString()!;
            string sicilNo = row["SicilNo"].ToString()!;
            int departmentId = Convert.ToInt32(row["DepartmentID"]);
            string role = row["Role"].ToString()!;

            // JwtHelper'ın Employee beklemesi yerine direkt claim üretelim
            // TODO: İleride JwtHelper DataRow yerine bir LoginResponseDto alacak şekilde refactor edilebilir
            var token = _jwtHelper.TokenUretFromRow(employeeId, fullName, sicilNo, departmentId, role);

            return Ok(new
            {
                token,
                employeeId,
                fullName,
                role,
                departmentId
            });
        }
    }

    public record LoginRequest(string SicilNo, string Password);
}