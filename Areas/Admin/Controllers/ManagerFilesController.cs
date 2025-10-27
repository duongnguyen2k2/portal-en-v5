using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using API.Models.ManagerFiles;
using Microsoft.AspNetCore.Mvc;

namespace API.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ManagerFilesController : Controller
    {
        [HttpGet]
        public async Task<dynamic> DownloadFile([FromQuery]string url, string folder = "temp")
        {
            try
            {
                if (folder == "" || folder == null) { folder = "temp"; }
                string TenFile = ManagerFilesService.ValidateFileName(url);
                folder = ManagerFilesService.ValidateFileName(folder);
                if (url == null) { return Content("filename not present"); };
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folder + "/" + url);
                var memory = new MemoryStream();
                using (var stream = new FileStream(path, FileMode.Open))
                {
                    await stream.CopyToAsync(memory);
                }
                memory.Position = 0;
                var contentType = "application/octet-stream";
                return File(memory, contentType, Path.GetFileName(path));
            }
            catch (Exception e)
            {
                return new API.Models.MsgError() { Msg = e.Message };
            }
        }
        [HttpGet]
        public async Task<dynamic> DownloadFileURL([FromQuery]string url)
        {
            try
            {
                var path = url;
                var memory = new MemoryStream();
                using (var stream = new FileStream(path, FileMode.Open))
                {
                    await stream.CopyToAsync(memory);
                }
                memory.Position = 0;
                var contentType = "application/octet-stream";
                return File(memory, contentType, Path.GetFileName(path));
            }
            catch (Exception e)
            {
                return new API.Models.MsgError() { Msg = e.Message };
            }
        }
    }
}