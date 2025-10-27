using API.Areas.Admin.Models.DMCoQuan;
using Microsoft.AspNetCore.Components.Forms;
using QRCoder;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;


namespace API.Models.MyQRCode
{ 
    public class MyQRCodeService
    {
        public string? CreateQRDomain(DMCoQuan ItemCoQuan, string Domain, Boolean flagDelete)
        {
            string FileName = Domain;            
            if (!string.IsNullOrEmpty(Domain))
            {
                FileName = FileName.Replace("https://", "").Trim();
                FileName = FileName.Replace("http://", "").Trim();
                FileName = FileName.Replace("/", "").Trim();                
                FileName = FileName + ".png";                
            }
            else
            {
                return "Loi -"+ ItemCoQuan.FolderUpload;
            }
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            string Folder = "/uploads/"+ ItemCoQuan.FolderUpload + "/banners/";
            string PathFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot") + Folder;
            if (!Directory.Exists(PathFolder))
            {
                Directory.CreateDirectory(PathFolder);                
            }

            // Load your logo
            string logoPath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")+Folder + "logo.png";

            if (!System.IO.File.Exists(logoPath))
            {
                logoPath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "wwwroot") + "/images/logo.png";
            }
            Bitmap logoImage = new Bitmap(logoPath);

            string Link = Folder + FileName;

            QRCodeData qrCodeData = qrGenerator.CreateQrCode(Domain, QRCodeGenerator.ECCLevel.Q);

            PngByteQRCode qrCode = new PngByteQRCode(qrCodeData);

            string destFile = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")+"/"+Link;

            
            byte[] qrCodeBytes = qrCode.GetGraphic(20, Color.Black, Color.White, false);
            Bitmap qrCodeImage;
            
            using (MemoryStream ms = new MemoryStream(qrCodeBytes))
            {
                Bitmap qrWithoutBorder;
                using (Bitmap tempBitmap = new Bitmap(ms))
                {
                    qrWithoutBorder = new Bitmap(tempBitmap.Width, tempBitmap.Height, PixelFormat.Format32bppArgb);
                    using (Graphics g = Graphics.FromImage(qrWithoutBorder))
                    {
                        g.DrawImage(tempBitmap, 0, 0);
                    }
                }

                using (Graphics qrGraphics = Graphics.FromImage(qrWithoutBorder))
                {
                    int logoSize = Math.Min(qrWithoutBorder.Width, qrWithoutBorder.Height) * 15 / 100;
                    int logoX = (qrWithoutBorder.Width - logoSize) / 2;
                    int logoY = (qrWithoutBorder.Height - logoSize) / 2;
                    
                    qrGraphics.DrawImage(logoImage, logoX, logoY, logoSize, logoSize);
                }

                int borderSize = 5;
                qrCodeImage = new Bitmap(
                    qrWithoutBorder.Width + (borderSize * 2),
                    qrWithoutBorder.Height + (borderSize * 2)
                );

                using (Graphics graphics = Graphics.FromImage(qrCodeImage))
                {
                    graphics.DrawImage(qrWithoutBorder, borderSize, borderSize);
                }
            }

            // QRCode qrCode = new QRCode(qrCodeData);

            // string destFile = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")+"/"+Link;

            
            // // Tạo QR code không có quiet zones
            // Bitmap qrWithoutBorder = qrCode.GetGraphic(
            //     pixelsPerModule: 20,
            //     darkColor: Color.Black,
            //     lightColor: Color.White,
            //     icon: logoImage,
            //     iconSizePercent: 15,
            //     //iconBorderWidth: 5
            //     drawQuietZones: false
            // );

            // // Thêm border 10px xung quanh
            // int borderSize = 5;
            // Bitmap qrCodeImage = new Bitmap(
            //     qrWithoutBorder.Width + (borderSize * 2),
            //     qrWithoutBorder.Height + (borderSize * 2)
            // );

            // using (Graphics graphics = Graphics.FromImage(qrCodeImage))
            // {
            //     // Tô nền màu xanh
            //     //Color blueBackground = ColorTranslator.FromHtml("#0061AF");
            //     //graphics.Clear(blueBackground);
            //     // Vẽ QR code vào giữa với border 10px
            //     graphics.DrawImage(qrWithoutBorder, borderSize, borderSize);
            // }

            try
            {
                if (flagDelete)
                {
                    if (System.IO.File.Exists(destFile))
                    {
                        System.IO.File.Delete(destFile);                        
                    }                    
                }
                if (!System.IO.File.Exists(destFile))
                {
                    qrCodeImage.Save(destFile, System.Drawing.Imaging.ImageFormat.Png);                   
                }
                                    
            }
            catch(Exception e)
            {
                FileName = e.ToString();
            }
            

            return Folder + FileName;
        }

        public string? BuildDangKyTenMien(DMCoQuan ItemCoQuan, string Domain, Boolean flagDelete)
        {
            string FileName = ItemCoQuan.Code;
            string Folder = "/temp/dang-ky-ten-mien/";
            string fileRootPath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "wwwroot") + "/ReportTemplate/dang-ky-ten-mien.docx";
            string PathFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot") + Folder;
            if (!Directory.Exists(PathFolder))
            {
                Directory.CreateDirectory(PathFolder);
            }
            return FileName;
        }
    }
}
