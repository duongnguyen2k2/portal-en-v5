using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Drawing;
using System.IO;
using System.Text;

namespace API.Controllers
{
    public class CaptchaController : Controller
    {
        

        // Background color 
        private readonly Color bgColor = Color.FromArgb(0xe9, 0xec, 0xef);
        // Code color 
        private readonly Color codeColor = Color.FromArgb(0x00, 0x69, 0xd9);
        // Obstruction color 
        private readonly Color obsColor = Color.FromArgb(0x28, 0xa7, 0x45);
        public IActionResult Index([FromQuery] string u="abc") // u=controler or action 
        {
            string dirPathCaptcha = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot") + "/temp";

            DirectoryInfo dirInfo = new DirectoryInfo(dirPathCaptcha);
            if (Directory.Exists(dirPathCaptcha))
            {
                FileInfo[] childFiles = dirInfo.GetFiles();
                foreach (FileInfo childFile in childFiles)
                {
                    if(childFile.Name.Substring(0, 2) == "c_")
                    {
                        System.IO.File.Delete(childFile.FullName);
                    }
                }
            }
            // Setup output format
            var contentType = "image/png";
            // Image width
            const int imageWidth = 150;
            // Image height
            const int imageHeight = 50;
            // Captcha code length
            const int captchaCodeLength = 4;
            // Captcha code string, all the possible chars that can appear in the image.
            const string captchaCodeString = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string captchaCode = "abc";

            Random random = new Random();
            // Generate random characters
            StringBuilder s = new StringBuilder();
            using var ms = new MemoryStream();

            // Create the image
            using Bitmap bitmap = new Bitmap(imageWidth, imageHeight);
            // Create the graphics 
            using Graphics graphics = Graphics.FromImage(bitmap);
            // Write bg color
            graphics.FillRectangle(new SolidBrush(bgColor), 0, 0, imageWidth, imageHeight);

            // Add obstructions
            using (Pen pen = new Pen(new SolidBrush(obsColor), 2))
            {
                for (int i = 0; i < 10; i++)
                {
                    graphics.DrawLine(pen, new Point(random.Next(0, imageWidth - 1), random.Next(0, imageHeight - 1)), new Point(random.Next(0, imageWidth - 1), random.Next(0, imageHeight - 1)));
                }
            }
            for (int i = 0; i < 100; i++)
            {
                bitmap.SetPixel(random.Next(imageWidth), random.Next(imageHeight), Color.FromArgb(random.Next()));
            }

            // Font
            using (Font font = new Font(FontFamily.GenericMonospace, 32, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Pixel))
            {
                for (int i = 0; i < captchaCodeLength; i++)
                {                    
                    s.Append(captchaCodeString.Substring(random.Next(0, captchaCodeString.Length - 1), 1));
                    // Write char to the graphic 
                    graphics.DrawString(s[s.Length - 1].ToString(), font, new SolidBrush(codeColor), i * 32, random.Next(0, 24));
                }
            }
            u = "CaptChaCode_" + u;
            HttpContext.Session.SetString(u, s.ToString());
            // Save image, image format type is consistent with response content type.
            string CaptchaPath = "/temp/c_" + DateTime.Now.ToString("yyyyMMddHHmmssffff") + ".png";
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot") + CaptchaPath;
            bitmap.Save(filePath, System.Drawing.Imaging.ImageFormat.Png);
            //return (ms.ToArray(), contentType);

            return Json(new API.Models.MsgSuccess() { Data = CaptchaPath });
        }

        public IActionResult GetCaptChaCode([FromQuery] string u="abc")
        {
            u = "CaptChaCode_" + u;
            return Json(new API.Models.MsgSuccess() { Data = HttpContext.Session.GetString(u) });
        }
    }
}
