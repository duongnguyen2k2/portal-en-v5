using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using API.Areas.Admin.Models.Liferay;
using API.Areas.Image.Models;

namespace API.Areas.Image.Controllers
{
    [Area("Image")]
    public class JournalController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

       

        [HttpGet]
        public IActionResult Article([FromQuery] int img_id)
        {
            //select * from image where imageid=323518  // 
            // string ImgRoot = "/document_library/0/0/";
            //string PathImg = ImgRoot+img_id.ToString() +  "/";
            string filename = img_id.ToString();
            string type = "image/png";
            Z_Image item = LiferayService.GetItemZImage(img_id);
            if (item != null && item.imageid > 0) {
                filename = item.imageid.ToString() + "." + item.type_;
                type = "image/" + item.type_;
            }
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "document_library") + "/0/0/"+ filename+ "/1.0";
            if (!System.IO.File.Exists(filePath))
            {
                filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot") + "/images/no-image.png";
            }

            Byte[] b = System.IO.File.ReadAllBytes(filePath);   // You can use your own method over here.         
            return File(b, type);

            //return Json(new API.Models.MsgSuccess() { Data = filePath });
        }
    }
}
