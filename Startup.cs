using Microsoft.Extensions.Configuration;

namespace API
{
    public class Startup
    {
        public static IConfiguration _config { get; set; }
        public static IConfiguration Configuration { get; set; }
        public static string ConnectionString { get; set; }
        public static string? ConnectionStringSTP { get; set; }
        public static string? ConnectionStringMysql { get; set; }
        public static string? reCAPTCHASiteKey { get; set; }
        public static string? reCAPTCHASecretKey { get; set; }
    }
}