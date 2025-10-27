using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Linq;

namespace API.Controllers
{
    public class PostgresImportController : Controller
    {
        // Import Categories từ CSV
        public async Task<IActionResult> ImportCategories()
        {
            var filePath = @"C:\temp\categories.csv";
            var categories = new List<CategoryCsv>();

            try
            {
                using var reader = new StreamReader(filePath, Encoding.UTF8);
                await reader.ReadLineAsync(); // Skip header

                while (!reader.EndOfStream)
                {
                    var line = await reader.ReadLineAsync();
                    var values = ParseCsvLine(line);

                    categories.Add(new CategoryCsv
                    {
                        CategoryId = long.Parse(values[0]),
                        Title = values[1],
                        Description = values[2],
                        ParentId = string.IsNullOrEmpty(values[3]) ? (long?)null : long.Parse(values[3]),
                        DisplayOrder = string.IsNullOrEmpty(values[4]) ? 0 : int.Parse(values[4]),
                        IsActive = values[5] == "1" || values[5].ToLower() == "true"
                    });
                }

                // Insert vào SQL Server
                using var connection = new SqlConnection(Startup.ConnectionString);
                await connection.OpenAsync();

                foreach (var cat in categories)
                {
                    using var command = connection.CreateCommand();
                    command.CommandText = @"
                        INSERT INTO Categories_wordpress 
                        (CategoryId, Title, Description, ParentId, DisplayOrder, IsActive)
                        VALUES (@CategoryId, @Title, @Description, @ParentId, @DisplayOrder, @IsActive)";

                    command.Parameters.AddWithValue("@CategoryId", cat.CategoryId);
                    command.Parameters.AddWithValue("@Title", cat.Title ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Description", cat.Description ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@ParentId", cat.ParentId ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@DisplayOrder", cat.DisplayOrder);
                    command.Parameters.AddWithValue("@IsActive", cat.IsActive);

                    await command.ExecuteNonQueryAsync();
                }

                return Json(new { success = true, message = $"Đã import {categories.Count} categories", data = categories });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // Import Articles từ CSV
        public async Task<IActionResult> ImportArticles()
        {
            var filePath = @"C:\temp\articles.csv";
            var articles = new List<ArticleCsv>();
            var errors = new List<string>();

            try
            {
                using var reader = new StreamReader(filePath, Encoding.UTF8);
                await reader.ReadLineAsync(); // Skip header

                int lineNumber = 1;
                while (!reader.EndOfStream)
                {
                    lineNumber++;
                    var line = await reader.ReadLineAsync();

                    if (string.IsNullOrWhiteSpace(line)) continue; // Bỏ qua dòng trống

                    var values = ParseCsvLine(line);

                    // Kiểm tra số cột
                    if (values.Length < 14)
                    {
                        errors.Add($"Dòng {lineNumber}: chỉ có {values.Length} cột");
                        continue;
                    }

                    try
                    {
                        articles.Add(new ArticleCsv
                        {
                            ArticleId = long.Parse(values[0]),
                            Title = values[1],
                            Summary = values[2],
                            Content = values[3],
                            Image = values[4],
                            Author = values[5],
                            ViewCount = string.IsNullOrEmpty(values[6]) ? 0 : long.Parse(values[6]),
                            Status = string.IsNullOrEmpty(values[7]) ? 0 : int.Parse(values[7]),
                            CreatedDate = string.IsNullOrEmpty(values[8]) ? (DateTime?)null : DateTime.Parse(values[8]),
                            PublishedDate = string.IsNullOrEmpty(values[9]) ? (DateTime?)null : DateTime.Parse(values[9]),
                            FriendlyUrl = values[10],
                            IsHomepage = values[11] == "t" || values[11] == "1" || values[11].ToLower() == "true",
                            IsFeatured = values[12] == "t" || values[12] == "1" || values[12].ToLower() == "true",
                            Tags = values.Length > 13 ? values[13] : ""
                        });
                    }
                    catch (Exception ex)
                    {
                        errors.Add($"Dòng {lineNumber}: {ex.Message}");
                    }
                }

                // Insert vào SQL Server
                using var connection = new SqlConnection(Startup.ConnectionString);
                await connection.OpenAsync();

                int successCount = 0;
                foreach (var article in articles)
                {
                    try
                    {
                        using var command = connection.CreateCommand();
                        command.CommandText = @"
                    INSERT INTO Articles_wordpress 
                    (ArticleId, Title, Summary, Content, Image, Author, ViewCount, Status, 
                     CreatedDate, PublishedDate, FriendlyUrl, IsHomepage, IsFeatured, Tags)
                    VALUES (@ArticleId, @Title, @Summary, @Content, @Image, @Author, @ViewCount, @Status,
                            @CreatedDate, @PublishedDate, @FriendlyUrl, @IsHomepage, @IsFeatured, @Tags)";

                        command.Parameters.AddWithValue("@ArticleId", article.ArticleId);
                        command.Parameters.AddWithValue("@Title", article.Title ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Summary", article.Summary ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Content", article.Content ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Image", article.Image ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Author", article.Author ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@ViewCount", article.ViewCount);
                        command.Parameters.AddWithValue("@Status", article.Status);
                        command.Parameters.AddWithValue("@CreatedDate", article.CreatedDate ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@PublishedDate", article.PublishedDate ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@FriendlyUrl", article.FriendlyUrl ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@IsHomepage", article.IsHomepage);
                        command.Parameters.AddWithValue("@IsFeatured", article.IsFeatured);
                        command.Parameters.AddWithValue("@Tags", article.Tags ?? (object)DBNull.Value);

                        await command.ExecuteNonQueryAsync();
                        successCount++;
                    }
                    catch (Exception ex)
                    {
                        errors.Add($"ArticleId {article.ArticleId}: {ex.Message}");
                    }
                }

                return Json(new
                {
                    success = true,
                    message = $"Import thành công {successCount}/{articles.Count} articles",
                    errors = errors.Take(10) // Chỉ lấy 10 lỗi đầu
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message, errors = errors });
            }
        }

        // Import Images từ CSV
        public async Task<IActionResult> ImportImages()
        {
            var filePath = @"C:\temp\images.csv";
            var images = new List<ImageCsv>();

            try
            {
                using var reader = new StreamReader(filePath, Encoding.UTF8);
                await reader.ReadLineAsync(); // Skip header

                while (!reader.EndOfStream)
                {
                    var line = await reader.ReadLineAsync();
                    var values = ParseCsvLine(line);

                    images.Add(new ImageCsv
                    {
                        ImageId = long.Parse(values[0]),
                        FileVersionId = string.IsNullOrEmpty(values[1]) ? (long?)null : long.Parse(values[1]),
                        MimeType = values[2],
                        Height = string.IsNullOrEmpty(values[3]) ? 0 : int.Parse(values[3]),
                        Width = string.IsNullOrEmpty(values[4]) ? 0 : int.Parse(values[4]),
                        FileSize = string.IsNullOrEmpty(values[5]) ? 0 : long.Parse(values[5]),
                        CreateDate = string.IsNullOrEmpty(values[6]) ? (DateTime?)null : DateTime.Parse(values[6])
                    });
                }

                // Insert vào SQL Server
                using var connection = new SqlConnection(Startup.ConnectionString);
                await connection.OpenAsync();

                foreach (var img in images)
                {
                    using var command = connection.CreateCommand();
                    command.CommandText = @"
                        INSERT INTO Images_wordpress 
                        (ImageId, FileVersionId, MimeType, Height, Width, FileSize, CreateDate)
                        VALUES (@ImageId, @FileVersionId, @MimeType, @Height, @Width, @FileSize, @CreateDate)";

                    command.Parameters.AddWithValue("@ImageId", img.ImageId);
                    command.Parameters.AddWithValue("@FileVersionId", img.FileVersionId ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@MimeType", img.MimeType ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Height", img.Height);
                    command.Parameters.AddWithValue("@Width", img.Width);
                    command.Parameters.AddWithValue("@FileSize", img.FileSize);
                    command.Parameters.AddWithValue("@CreateDate", img.CreateDate ?? (object)DBNull.Value);

                    await command.ExecuteNonQueryAsync();
                }

                return Json(new { success = true, message = $"Đã import {images.Count} images", data = images });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // Import ArticleCategories từ CSV
        public async Task<IActionResult> ImportArticleCategories()
        {
            var filePath = @"C:\temp\article_categories.csv";
            var relations = new List<ArticleCategoryCsv>();
            var errors = new List<string>();

            try
            {
                // Đọc file CSV
                using (var reader = new StreamReader(filePath, Encoding.UTF8))
                {
                    await reader.ReadLineAsync(); // Skip header

                    while (!reader.EndOfStream)
                    {
                        var line = await reader.ReadLineAsync();
                        if (string.IsNullOrWhiteSpace(line)) continue;

                        var values = ParseCsvLine(line);
                        if (values.Length < 2) continue;

                        relations.Add(new ArticleCategoryCsv
                        {
                            ArticleId = long.Parse(values[0]),
                            CategoryId = long.Parse(values[1])
                        });
                    }
                }

                // Lấy danh sách ArticleId và CategoryId tồn tại trong DB
                using var connection = new SqlConnection(Startup.ConnectionString);
                await connection.OpenAsync();

                // Lấy tất cả ArticleId có trong Articles_wordpress
                var existingArticleIds = new HashSet<long>();
                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = "SELECT ArticleId FROM Articles_wordpress";
                    using var reader = await cmd.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        existingArticleIds.Add(reader.GetInt64(0));
                    }
                }

                // Lấy tất cả CategoryId có trong Categories_wordpress
                var existingCategoryIds = new HashSet<long>();
                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = "SELECT CategoryId FROM Categories_wordpress";
                    using var reader = await cmd.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        existingCategoryIds.Add(reader.GetInt64(0));
                    }
                }

                // Insert chỉ những relation hợp lệ
                int successCount = 0;
                int skippedCount = 0;

                foreach (var rel in relations)
                {
                    // Kiểm tra ArticleId và CategoryId có tồn tại không
                    if (!existingArticleIds.Contains(rel.ArticleId))
                    {
                        errors.Add($"ArticleId {rel.ArticleId} không tồn tại");
                        skippedCount++;
                        continue;
                    }

                    if (!existingCategoryIds.Contains(rel.CategoryId))
                    {
                        errors.Add($"CategoryId {rel.CategoryId} không tồn tại");
                        skippedCount++;
                        continue;
                    }

                    try
                    {
                        using var command = connection.CreateCommand();
                        command.CommandText = @"
                    INSERT INTO ArticleCategories_wordpress (ArticleId, CategoryId)
                    VALUES (@ArticleId, @CategoryId)";

                        command.Parameters.AddWithValue("@ArticleId", rel.ArticleId);
                        command.Parameters.AddWithValue("@CategoryId", rel.CategoryId);

                        await command.ExecuteNonQueryAsync();
                        successCount++;
                    }
                    catch (Exception ex)
                    {
                        errors.Add($"ArticleId {rel.ArticleId}, CategoryId {rel.CategoryId}: {ex.Message}");
                        skippedCount++;
                    }
                }

                return Json(new
                {
                    success = true,
                    message = $"Import thành công {successCount}/{relations.Count} relations. Bỏ qua {skippedCount} relations.",
                    totalRelations = relations.Count,
                    successCount = successCount,
                    skippedCount = skippedCount,
                    errors = errors.Take(20)
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message, errors = errors });
            }
        }

        // Import tất cả (chạy tuần tự)
        public async Task<IActionResult> ImportAll()
        {
            var results = new List<string>();

            try
            {
                // 1. Import Categories trước
                var catResult = await ImportCategories() as JsonResult;
                results.Add($"Categories: {((dynamic)catResult.Value).message}");

                // 2. Import Articles
                var artResult = await ImportArticles() as JsonResult;
                results.Add($"Articles: {((dynamic)artResult.Value).message}");

                // 3. Import Images
                var imgResult = await ImportImages() as JsonResult;
                results.Add($"Images: {((dynamic)imgResult.Value).message}");

                // 4. Import ArticleCategories cuối cùng (có foreign key)
                var relResult = await ImportArticleCategories() as JsonResult;
                results.Add($"ArticleCategories: {((dynamic)relResult.Value).message}");

                return Json(new { success = true, message = "Import tất cả thành công!", results = results });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message, results = results });
            }
        }

        // Helper: Parse CSV line xử lý dấu ngoặc kép và dấu phẩy
        private string[] ParseCsvLine(string line)
        {
            var result = new List<string>();
            bool inQuotes = false;
            var currentValue = new StringBuilder();

            for (int i = 0; i < line.Length; i++)
            {
                char c = line[i];

                if (c == '"')
                {
                    inQuotes = !inQuotes;
                }
                else if (c == ',' && !inQuotes)
                {
                    result.Add(currentValue.ToString());
                    currentValue.Clear();
                }
                else
                {
                    currentValue.Append(c);
                }
            }

            result.Add(currentValue.ToString());
            return result.ToArray();
        }
    }

    // Models
    public class CategoryCsv
    {
        public long CategoryId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public long? ParentId { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }
    }

    public class ArticleCsv
    {
        public long ArticleId { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Content { get; set; }
        public string Image { get; set; }
        public string Author { get; set; }
        public long ViewCount { get; set; }
        public int Status { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? PublishedDate { get; set; }
        public string FriendlyUrl { get; set; }
        public bool IsHomepage { get; set; }
        public bool IsFeatured { get; set; }
        public string Tags { get; set; }
    }

    public class ImageCsv
    {
        public long ImageId { get; set; }
        public long? FileVersionId { get; set; }
        public string MimeType { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public long FileSize { get; set; }
        public DateTime? CreateDate { get; set; }
    }

    public class ArticleCategoryCsv
    {
        public long ArticleId { get; set; }
        public long CategoryId { get; set; }
    }
}