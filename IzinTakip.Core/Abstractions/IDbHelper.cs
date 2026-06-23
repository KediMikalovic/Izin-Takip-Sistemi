using System.Data;
using Microsoft.Data.SqlClient;

namespace IzinTakip.Core.Abstractions
{
    /// <summary>
    /// Veritabani erisim katmani soyutlamasi.
    /// Concrete sinif: DataAccess/SqlHelper
    /// </summary>
    public interface IDbHelper
    {
        // Transaction gerektiren servisler icin
        string ConnectionString { get; }

        // SELECT
        DataTable ExecuteQuery(string query, SqlParameter[]? parameters = null);
        Task<DataTable> ExecuteQueryAsync(string query, SqlParameter[]? parameters = null);

        // INSERT / UPDATE / DELETE
        int ExecuteNonQuery(string query, SqlParameter[]? parameters = null);
        Task<int> ExecuteNonQueryAsync(string query, SqlParameter[]? parameters = null);
    }
}
