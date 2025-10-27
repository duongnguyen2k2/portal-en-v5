using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;

namespace API.Areas.Image.Controllers
{
    [Area("Image")]
    public class DocumentController : Controller
    {
        [HttpGet]
        [Route("Image/Document/{folder}/{fileId}")]
        public IActionResult GetDocument(string folder, string fileId)
        {
            // Đường dẫn gốc: wwwroot/document_library/20101/43080/
            string basePath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                "document_library",
                folder,
                fileId
            );

            string filePath = null;

            if (Directory.Exists(basePath))
            {
                // Tìm tất cả file ảnh trong thư mục và các thư mục con
                var imageExtensions = new[] { "*.jpg", "*.jpeg", "*.png", "*.gif", "*.webp", "*.bmp" };

                foreach (var ext in imageExtensions)
                {
                    var files = Directory.GetFiles(basePath, ext, SearchOption.AllDirectories);
                    if (files.Length > 0)
                    {
                        filePath = files[0]; // Lấy file đầu tiên
                        break;
                    }
                }

                // Nếu không tìm thấy file có extension, tìm tất cả file
                if (string.IsNullOrEmpty(filePath))
                {
                    var allFiles = Directory.GetFiles(basePath, "*", SearchOption.AllDirectories);

                    // Ưu tiên file có tên "1.0" hoặc file đầu tiên
                    filePath = allFiles.FirstOrDefault(f => Path.GetFileName(f) == "1.0")
                               ?? allFiles.FirstOrDefault();
                }
            }

            // Nếu vẫn không tìm thấy
            if (string.IsNullOrEmpty(filePath) || !System.IO.File.Exists(filePath))
            {
                // Trả về ảnh mặc định
                string noImagePath = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot",
                    "images",
                    "no-image.png"
                );

                if (!System.IO.File.Exists(noImagePath))
                {
                    return NotFound($"Image not found: {folder}/{fileId}");
                }

                return File(System.IO.File.ReadAllBytes(noImagePath), "image/png");
            }

            // Đọc file và xác định content type
            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            string contentType = GetContentType(fileBytes);

            // Cache trong 30 ngày
            Response.Headers.Add("Cache-Control", "public, max-age=2592000");

            return File(fileBytes, contentType);
        }

        private string GetContentType(byte[] fileBytes)
        {
            if (fileBytes == null || fileBytes.Length < 4)
                return "image/jpeg";

            // JPEG: FF D8 FF
            if (fileBytes[0] == 0xFF && fileBytes[1] == 0xD8 && fileBytes[2] == 0xFF)
                return "image/jpeg";

            // PNG: 89 50 4E 47
            if (fileBytes[0] == 0x89 && fileBytes[1] == 0x50 &&
                fileBytes[2] == 0x4E && fileBytes[3] == 0x47)
                return "image/png";

            // GIF: 47 49 46 38
            if (fileBytes[0] == 0x47 && fileBytes[1] == 0x49 &&
                fileBytes[2] == 0x46 && fileBytes[3] == 0x38)
                return "image/gif";

            // WebP: 52 49 46 46
            if (fileBytes[0] == 0x52 && fileBytes[1] == 0x49 &&
                fileBytes[2] == 0x46 && fileBytes[3] == 0x46)
                return "image/webp";

            // BMP: 42 4D
            if (fileBytes.Length >= 2 && fileBytes[0] == 0x42 && fileBytes[1] == 0x4D)
                return "image/bmp";

            return "image/jpeg";
        }
    }
}