using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using IzinTakip.Core.Abstractions;

namespace IzinTakip.Core.DataAccess
{
    public class SqlHelper : IDbHelper
    {
        // ── Static cache: appsettings.json yalnızca bir kez okunur ──────────
        private static readonly string _connectionString;

        static SqlHelper()
        {
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                .Build();

            _connectionString = config.GetConnectionString("IzinTakipDB")
                ?? throw new InvalidOperationException("'IzinTakipDB' connection string appsettings.json içinde bulunamadı.");
        }

        // IDbHelper interface uyeligi — servis katmani transaction icin kullanir
        public string ConnectionString => _connectionString;

        private const int DefaultCommandTimeout = 60; // saniye

        // ── Senkron: SELECT sorguları ────────────────────────────────────────
        public DataTable ExecuteQuery(string query, SqlParameter[]? parameters = null)
        {
            using SqlConnection conn = new(_connectionString);
            using SqlCommand cmd = new(query, conn) { CommandTimeout = DefaultCommandTimeout };

            if (parameters != null) cmd.Parameters.AddRange(parameters);

            try
            {
                conn.Open();
                DataTable dt = new();
                using SqlDataReader reader = cmd.ExecuteReader();
                dt.Load(reader);
                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception("Veritabanı Okuma Hatası: " + ex.Message, ex);
            }
        }

        // ── Asenkron: SELECT sorguları (UI donmadan) ─────────────────────────
        public async Task<DataTable> ExecuteQueryAsync(string query, SqlParameter[]? parameters = null)
        {
            using SqlConnection conn = new(_connectionString);
            using SqlCommand cmd = new(query, conn) { CommandTimeout = DefaultCommandTimeout };

            if (parameters != null) cmd.Parameters.AddRange(parameters);

            try
            {
                await conn.OpenAsync();
                DataTable dt = new();
                using SqlDataReader reader = await cmd.ExecuteReaderAsync();
                dt.Load(reader);
                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception("Veritabanı Okuma Hatası: " + ex.Message, ex);
            }
        }

        // ── Senkron: INSERT / UPDATE / DELETE ────────────────────────────────
        public int ExecuteNonQuery(string query, SqlParameter[]? parameters = null)
        {
            using SqlConnection conn = new(_connectionString);
            using SqlCommand cmd = new(query, conn) { CommandTimeout = DefaultCommandTimeout };

            if (parameters != null) cmd.Parameters.AddRange(parameters);

            try
            {
                conn.Open();
                return cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Veritabanı İşlem Hatası: " + ex.Message, ex);
            }
        }

        // ── Asenkron: INSERT / UPDATE / DELETE ───────────────────────────────
        public async Task<int> ExecuteNonQueryAsync(string query, SqlParameter[]? parameters = null)
        {
            using SqlConnection conn = new(_connectionString);
            using SqlCommand cmd = new(query, conn) { CommandTimeout = DefaultCommandTimeout };

            if (parameters != null) cmd.Parameters.AddRange(parameters);

            try
            {
                await conn.OpenAsync();
                return await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Veritabanı İşlem Hatası: " + ex.Message, ex);
            }
        }
    }
}
