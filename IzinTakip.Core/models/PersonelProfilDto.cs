using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IzinTakip.Core.Models
{
    public record PersonelProfilDto(
        int EmployeeId,
        string FullName,
        string SicilNo,
        string Departman,
        string Role,
        int KidemYil,
        int ToplamHak,
        int KullanilanIzin,
        int KalanIzin,
        DateTime IseBaslamaTarihi
    );
}