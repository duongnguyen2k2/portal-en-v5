using API.Areas.Admin.Models.ManagerFile;
using API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class ManagerFileController : Controller
    {
        private string controllerName = "ManagerFileController";
        private string controllerSecret;
        public ManagerFileController(IConfiguration config)
        {
            controllerSecret = config["Security:SecretId"] + controllerName;
        }
        private string UrlRoot = "/uploads/";

        public IActionResult Index()
        {
            return Json(new API.Models.MsgSuccess() { Msg = "Thư mục không tồn tại" });
        }

        public async Task<IActionResult> DownloadFileMobile([FromQuery] string Ids, int Id, string PathFile)
        {
           
            int IdDC = Id;
            if (IdDC == Id)
            {
                var localFilePath = Path.Combine(Directory.GetCurrentDirectory(), "TaiLieuHop/" + PathFile);

                if (!System.IO.File.Exists(localFilePath))
                {
                    return Json(new API.Models.MsgError() { Msg = "File Không tồn tại trên server" });
                }
                else
                {
                    var memory = new MemoryStream();
                    using (var stream = new FileStream(localFilePath, FileMode.Open))
                    {
                        await stream.CopyToAsync(memory);
                    }
                    memory.Position = 0;
                    return File(memory, GetContentType(localFilePath), PathFile);
                }
            }
            else
            {
                return Json(new API.Models.MsgError() { Msg = "File Không tồn tại" });
            }


        }

       

        private string GetContentType(string path)
        {
            var provider = new FileExtensionContentTypeProvider();
            string contentType;
            if (!provider.TryGetContentType(path, out contentType))
            {
                contentType = "application/octet-stream";
            }
            return contentType;
        }
        
    }
}
