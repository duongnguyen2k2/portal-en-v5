using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HashidsNet;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;

namespace API.Models
{
    public class MyModels
    {
        public static string Encode(decimal id, string SecretId)
        {

            try
            {
                if (SecretId == null)
                {
                    SecretId = Models.Settings.SecretId;
                }
                var hashids = new Hashids(SecretId);
                long time = long.Parse(string.Format("{0:yyyyMMdd}", DateTime.Now));
                string hash = hashids.EncodeLong(long.Parse(id.ToString()), time);
                return hash;
            }
            catch
            {
                return "";
            }

        }
        public static decimal Decode(string ids, string SecretId)
        {

            try
            {
                if (SecretId == null)
                {
                    SecretId = Models.Settings.SecretId;
                }
                if (ids == null || ids == "")
                {
                    return 0;
                }
                else
                {

                    return new Hashids(SecretId).DecodeLong(ids)[0];
                }

            }
            catch
            {
                return -1;
            }

        }
        public static string StripHtml(string text, bool isRemoveScript, params string[] ignorableTags)
        {
            string txt_rp = "No javascript";
            if (!string.IsNullOrEmpty(text))
            {
                text = text.Replace("&lt;", "<");
                text = text.Replace("&gt;", ">");
                string ignorePattern = null;

                if (isRemoveScript)
                {
                    text = Regex.Replace(text, "<script[^<]*</script>", txt_rp, RegexOptions.IgnoreCase);
                }
                if (!ignorableTags.Contains("style"))
                {
                    text = Regex.Replace(text, "<style[^<]*</style>", txt_rp, RegexOptions.IgnoreCase);
                }
                foreach (string tag in ignorableTags)
                {
                    //the character b spoils the regex so replace it with strong
                    if (tag.Equals("b"))
                    {
                        text = text.Replace("<b>", "<strong>");
                        text = text.Replace("</b>", "</strong>");
                        if (ignorableTags.Contains("strong"))
                        {
                            ignorePattern = string.Format("{0}(?!strong)(?!/strong)", ignorePattern);
                        }
                    }
                    else
                    {
                        //Create ignore pattern fo the tags to ignore
                        ignorePattern = string.Format("{0}(?!{1})(?!/{1})", ignorePattern, tag);
                    }

                }
                //finally add the ignore pattern into regex <[^<]*> which is used to match all html tags
                ignorePattern = string.Format(@"<{0}[^<]*>", ignorePattern);
                text = Regex.Replace(text, ignorePattern, "", RegexOptions.IgnoreCase);
            }

            return text.Trim();
        }

        public static List<string> CountVisit()
        {

            List<string> ListAction = new List<string>();
            ListAction.Add("Articles");
            ListAction.Add("Home");
            ListAction.Add("Documents");
            ListAction.Add("HopTrucTuyen");
            ListAction.Add("Products");
            ListAction.Add("Albums");
            ListAction.Add("Audios");
            ListAction.Add("CategoriesArticles");
            ListAction.Add("Contacts");
            ListAction.Add("DuThaoVanBan");
            ListAction.Add("Reviews");
            ListAction.Add("Videos");
            ListAction.Add("WorkSchedules");

            return ListAction;
        }
}
    public class MsgError
    {
        public string? Msg { get; set; } = "Bạn không có quyền hehe";
        public Boolean Success { get; set; } = false;
        public string? Code { get; set; } = "Error";
        public dynamic Data { get; set; }
    }

    public class MsgSuccess
    {
        public string? Msg { get; set; } = "success connecttion";
        public Boolean Success { get; set; } = true;
        public string? Code { get; set; } = "OK";
        public dynamic Data { get; set; }
    }

    public class Settings
    {
        public static string? Email { get; set; }
        public static string? EmailPass { get; set; }
        public static string Secret { get; set; } = "70b4269b412a8af42b1f7b0d26eceff2" + DateTime.Now.ToString("yyyyMMdd");
        public static string SecretId { get; set; } = "55bb939da52b16140dc5bd91f6e6e163" + DateTime.Now.ToString("yyyyMMdd");
    }


    public class MetaData
    {
        public string? MetaTitle { get; set; } = "";
        public string? MetaH1 { get; set; } = "";
        public string? MetaH3 { get; set; } = "";
        public string? TextId { get; set; } = "";

    }

    public class Google
    {
        public Boolean success { get; set; }
        public string? hostname { get; set; }
    }

    public class ResultSave
    {
        public int Id { get; set; }
        public int N { get; set; }
    }



}
