using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Areas.Admin.Models.ManagerFiles
{
    public class MIMETypesService
    {
        public static List<MIMETypes> getList()
        {
            List<MIMETypes> listMIMETypes = new List<MIMETypes>();
            listMIMETypes.Add(new MIMETypes() { Extension = ".csv", KindOfDocument = "Comma-separated values (CSV)", MIMEType = "text/csv" });
            listMIMETypes.Add(new MIMETypes() { Extension = ".doc", KindOfDocument = "Microsoft Word", MIMEType = "application/msword" });
            listMIMETypes.Add(new MIMETypes() { Extension = ".docx", KindOfDocument = "Microsoft Word (OpenXML)", MIMEType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document" });
            listMIMETypes.Add(new MIMETypes() { Extension = ".eot", KindOfDocument = "MS Embedded OpenType fonts", MIMEType = "application/vnd.ms-fontobject" });
            listMIMETypes.Add(new MIMETypes() { Extension = ".epub", KindOfDocument = "Electronic publication (EPUB)", MIMEType = "application/epub+zip" });
            listMIMETypes.Add(new MIMETypes() { Extension = ".gif", KindOfDocument = "Graphics Interchange Format (GIF)", MIMEType = "image/gif" });
            listMIMETypes.Add(new MIMETypes() { Extension = ".ico", KindOfDocument = "Icon format", MIMEType = "image/x-icon" });
            listMIMETypes.Add(new MIMETypes() { Extension = ".jpeg", KindOfDocument = "JPEG images", MIMEType = "image/jpeg" });
            listMIMETypes.Add(new MIMETypes() { Extension = ".jpg", KindOfDocument = "JPEG images", MIMEType = "image/jpeg" });
            listMIMETypes.Add(new MIMETypes() { Extension = ".mpeg", KindOfDocument = "MPEG Video", MIMEType = "video/mpeg" });
            listMIMETypes.Add(new MIMETypes() { Extension = ".pdf", KindOfDocument = "Adobe Portable Document Format (PDF)", MIMEType = "application/pdf" });
            listMIMETypes.Add(new MIMETypes() { Extension = ".ppt", KindOfDocument = "Microsoft PowerPoint", MIMEType = "application/vnd.ms-powerpoint" });
            listMIMETypes.Add(new MIMETypes() { Extension = ".pptx", KindOfDocument = "Microsoft PowerPoint (OpenXML)", MIMEType = "application/vnd.openxmlformats-officedocument.presentationml.presentation" });
            listMIMETypes.Add(new MIMETypes() { Extension = ".rar", KindOfDocument = "RAR archive", MIMEType = "application/x-rar-compressed" });
            listMIMETypes.Add(new MIMETypes() { Extension = ".xls", KindOfDocument = "Microsoft Excel", MIMEType = "application/vnd.ms-excel" });
            listMIMETypes.Add(new MIMETypes() { Extension = ".xlsx", KindOfDocument = "Microsoft Excel (OpenXML)", MIMEType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" });
            listMIMETypes.Add(new MIMETypes() { Extension = ".xml", KindOfDocument = "XML", MIMEType = "application/xml" });
            listMIMETypes.Add(new MIMETypes() { Extension = ".xul", KindOfDocument = "XUL", MIMEType = "application/vnd.mozilla.xul+xml" });
            listMIMETypes.Add(new MIMETypes() { Extension = ".zip", KindOfDocument = "ZIP archive", MIMEType = "application/zip" });
            listMIMETypes.Add(new MIMETypes() { Extension = ".3gp", KindOfDocument = "3GPP audio/video container", MIMEType = "video/3gpp audio / 3gpp if it doesn't contain video" });
            listMIMETypes.Add(new MIMETypes() { Extension = ".3g2", KindOfDocument = "3GPP2 audio/video container", MIMEType = "video/3gpp2 audio / 3gpp2 if it doesn't contain video" });
            listMIMETypes.Add(new MIMETypes() { Extension = ".7z", KindOfDocument = "7-zip archive", MIMEType = "application/x-7z-compressed" });
            return listMIMETypes;
        }

        public static List<String> getlistImages()
        {
            List<String> listMIMETypes = new List<string>() { "jpeg", ".jpg",".png", ".gif" , ".webp", ".bmp" , ".jfif" };

            return listMIMETypes;
        }
        public static List<String> getlistHDSD()
        {
            List<String> listMIMETypes = new List<string>() { ".doc", ".docx", ".pdf", ".ppt", ".pptx", ".xls", ".xlsx" };
            return listMIMETypes;
        }

        public static List<String> GetListTinymce()
        {
            List<String> listMIMETypes = new List<string>() { ".csv", ".doc", ".docx", ".pdf", ".ppt", ".pptx", ".jpeg", ".jpg", ".png", ".gif", ".rar", ".zip", ".mp3", ".wav", ".mp4", ".xls", ".xlsx", ".webp", ".bmp", ".jfif", ".ico",".psd" };
            return listMIMETypes;
        }
    }
}