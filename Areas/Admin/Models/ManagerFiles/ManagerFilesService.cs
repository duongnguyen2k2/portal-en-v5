using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Xml;

namespace API.Areas.Admin.Models.ManagerFiles
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

        public static Settings GetSettingsInfo()
        {
            Settings item = new Settings() {
                Email = "",
                Secret = "047461bd0bfbd52096b7390e05a6d944",
                SecretId = "",
                EmailPass = ""

            };
            return item;
           
        }
    }
}