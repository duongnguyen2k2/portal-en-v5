using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
namespace API.Models.ManagerFiles
{
    public class TempFileCleanupService:BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    // Lấy đường dẫn thư mục tạm
                    string tempPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "temp");
                    
                    // Tìm file Excel và Word
                    var extensions = new[] { "*.xlsx", "*.xls", "*.docx", "*.doc", "*.png" };
                    var files = extensions
                        .SelectMany(ext => Directory.GetFiles(tempPath, ext, SearchOption.TopDirectoryOnly))
                        .Distinct();

                    if (files.Any())
                    {                    
                        // Thời gian hiện tại để so sánh
                        DateTime oneHourAgo = DateTime.Now.AddHours(-1);
                        foreach (string file in files)
                        {
                            try
                            {
                                FileInfo fileInfo = new FileInfo(file);
                                // Kiểm tra thời gian tạo file
                                if (fileInfo.CreationTime < oneHourAgo)
                                {
                                    File.Delete(file);
                                    Console.WriteLine($"[{DateTime.Now:HH:mm:ss dd/MM/yyyy}] xóa file thành công");
                                }                               
                            }
                            catch (UnauthorizedAccessException)
                            {
                                
                            }                            
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[{DateTime.Now:HH:mm:ss dd/MM/yyyy}] Lỗi: {ex.Message}");
                }
                // Chờ 30 phút
                await Task.Delay(TimeSpan.FromMinutes(50), stoppingToken);
            }
        }
    }
}
