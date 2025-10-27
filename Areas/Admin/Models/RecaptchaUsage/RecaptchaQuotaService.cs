using API.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Linq;

namespace API.Areas.Admin.Models.Services
{
    public interface IRecaptchaQuotaService
    {
        bool IsQuotaExceeded(int idCoQuan = 1);
        void IncrementUsage(int idCoQuan = 1);
        void ResetMonthlyUsage();
    }

    public class RecaptchaQuotaService : IRecaptchaQuotaService
    {
        private readonly IConfiguration _config;
        private readonly string _connectionString;
        private readonly int _quotaLimit;
        private readonly int _freeQuota;
        private readonly int[] _validIdCoQuans;

        public RecaptchaQuotaService(IConfiguration config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _connectionString = _config.GetConnectionString("DefaultConnection")  // Hoặc key phù hợp, thay vì Startup.ConnectionString
                ?? throw new InvalidOperationException("ConnectionString 'DefaultConnection' not found.");
            _quotaLimit = _config.GetValue<int>("RecaptchaSettings:QuotaLimit");
            _freeQuota = _config.GetValue<int>("RecaptchaSettings:FreeQuota");

            // Đọc mảng ValidIdCoQuans từ config (fallback nếu không có hoặc empty)
            var validIdsSection = _config.GetSection("RecaptchaSettings:ValidIdCoQuans");
            _validIdCoQuans = validIdsSection.Get<int[]>() ?? new int[] { 0 };
        }

        public bool IsQuotaExceeded(int idCoQuan = 1)
        {
            ValidateIdCoQuan(idCoQuan);

            var now = DateTime.UtcNow;
            var sql = @"
                SELECT ISNULL(SUM(Count), 0) AS TotalCount 
                FROM RecaptchaUsage 
                WHERE Month = @Month AND Year = @Year AND IdCoQuan IN ({0})";  // Dynamic IN clause với valid IDs

            // Tạo IN clause động (ví dụ: 1,2,3,4)
            var inClause = string.Join(",", _validIdCoQuans);
            sql = string.Format(sql, inClause);

            var parameters = new[] { "@Month", "@Year" };
            var values = new object[] { now.Month, now.Year };

            var table = ConnectDb.ExecuteQuerySQLWithParamsTask(_connectionString, sql, parameters, values);

            if (table?.Rows.Count > 0)
            {
                var totalCount = Convert.ToInt64(table.Rows[0]["TotalCount"]);

                // Log nếu exceed free quota (không block, chỉ log - giả sử có ILogger, thêm nếu cần)
                if (totalCount > _freeQuota)
                {
                    // Ví dụ: _logger.LogWarning("Exceeded free quota: {TotalCount} > {FreeQuota}", totalCount, _freeQuota);
                    Console.WriteLine($"Exceeded free quota: {totalCount} > {_freeQuota}");  // Fallback console log
                }

                return totalCount >= _quotaLimit;
            }

            // Không có entry: Tạo entry cho IdCoQuan hiện tại và return false
            CreateUsageEntry(now.Month, now.Year, idCoQuan);
            return false;
        }

        public void IncrementUsage(int idCoQuan = 0)
        {
            ValidateIdCoQuan(idCoQuan);

            var now = DateTime.UtcNow;
            var sql = @"
                IF EXISTS (SELECT 1 FROM RecaptchaUsage WHERE Month = @Month AND Year = @Year AND IdCoQuan = @IdCoQuan)
                    UPDATE RecaptchaUsage SET Count = Count + 1 WHERE Month = @Month AND Year = @Year AND IdCoQuan = @IdCoQuan
                ELSE
                    INSERT INTO RecaptchaUsage (Month, Year, Count, IdCoQuan, CreatedAt) VALUES (@Month, @Year, 1, @IdCoQuan, @CreatedAt)";

            var parameters = new[] { "@Month", "@Year", "@IdCoQuan", "@CreatedAt" };
            var values = new object[] { now.Month, now.Year, idCoQuan, now };

            ConnectDb.ExecuteNonQueryWithParamsTask(_connectionString, sql, parameters, values);
        }

        public void ResetMonthlyUsage()
        {
            var now = DateTime.UtcNow;
            var sql = @"
                DELETE FROM RecaptchaUsage 
                WHERE (Month != @Month OR Year != @Year) AND IdCoQuan IN ({0})";  // Dynamic cho valid IDs

            var inClause = string.Join(",", _validIdCoQuans);
            sql = string.Format(sql, inClause);

            var parameters = new[] { "@Month", "@Year" };
            var values = new object[] { now.Month, now.Year };

            ConnectDb.ExecuteNonQueryWithParamsTask(_connectionString, sql, parameters, values);
        }

        private void CreateUsageEntry(int month, int year, int idCoQuan)
        {
            var now = DateTime.UtcNow;
            var sql = "INSERT INTO RecaptchaUsage (Month, Year, Count, IdCoQuan, CreatedAt) VALUES (@Month, @Year, 0, @IdCoQuan, @CreatedAt)";

            var parameters = new[] { "@Month", "@Year", "@IdCoQuan", "@CreatedAt" };
            var values = new object[] { month, year, idCoQuan, now };

            ConnectDb.ExecuteNonQueryWithParamsTask(_connectionString, sql, parameters, values);
        }

        private void ValidateIdCoQuan(int idCoQuan)
        {
            if (!_validIdCoQuans.Contains(idCoQuan))
            {
                throw new ArgumentException($"IdCoQuan phải là một trong [{string.Join(",", _validIdCoQuans)}]. Giá trị hiện tại: {idCoQuan}", nameof(idCoQuan));
            }
        }
    }
}