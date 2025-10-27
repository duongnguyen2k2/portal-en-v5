using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Xml;

namespace API.Models.ManagerFiles
{
    public class ManagerFilesService
    {
        public static string SecretId = "683ab60ef327a2f7bda5fbb21c7c9281";
        public static string ValidateFileName(string url)
        {
            List<string> listErr = new List<string>();
            listErr.Add("/");
            listErr.Add("*");
            listErr.Add("?");
            listErr.Add("<");
            listErr.Add(">");
            listErr.Add("|");
            listErr.Add("\\");
            listErr.Add("..");
            listErr.Add(":");
            for (int i = 0; i < listErr.Count(); i++) {
                url = url.Replace(listErr[i], "-");
            }
            return url;
        }

        public static string GetExtension(string Path) {
            string filePath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", Path);
            string Extension = System.IO.Path.GetExtension(filePath);
            List<String> listMIMETypes = MIMETypesService.GetListReadFileOnline();           
            //string TenFileLuu = "tong-hop-dien-sinh-hoat" + string.Format("{0:ddMMyyHHmmss}", DateTime.Now);
            //string  url = System.IO.Path.Combine(path, TenFileLuu + ".xlsx");
            return Extension.ToLower();

        }

        public static Settings GetSettingsInfo() {
            Settings item = new Settings();
            return item;
        }


    }
}