using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using Ganss.Xss;
using MimeKit.Text;
namespace API.Models.MyHelper
{
    public class StringHelper
    {
        public static string RandomString(int length)
        {
            Random random = new Random();

            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static string PasswordGenerate(int length=9)
        {
            Random random = new Random();

            const string chars = "abcdefghjklmnpqrstuwxyzABCDEFGHIJKLMNPQRSTUWXYZ123456789!@#$*";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static string GetUniqueKey(int size)
        {
            char[] chars =
                "abcdefghijklmnpqrstuvwxyzABCDEFGHIJKLMNPQRSTUVWXYZ123456789".ToCharArray();
            byte[] data = new byte[size];
            using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
            {
                crypto.GetBytes(data);
            }
            StringBuilder result = new StringBuilder(size);
            foreach (byte b in data)
            {
                result.Append(chars[b % (chars.Length)]);
            }
            return result.ToString().Trim();
        }


        public static string ConverLan(string Culture)
        {
            string Lang = Culture.ToLower(); ;

            if (Lang == "vi-vn" || Lang == "vn")
            {
                Lang = "vi";
            }


            if ((Lang.IndexOf("vi") > -1) || (Lang.IndexOf("vn") > -1))
            {
                Lang = "vi";
            }

            if ((Lang.IndexOf("en") > -1) || (Lang.IndexOf("us") > -1) || (Lang.IndexOf("gb") > -1))
            {
                Lang = "en";
            }

            if (Lang != "en")
            {
                Lang = "vi";
            }
            if(Lang!="vi" && Lang != "en")
            {
                Lang = "vi";
            }
            return Lang;
        }


        public static string DayweekVN(DateTime date)
        {
            string d = date.DayOfWeek.ToString();
            switch (d)
            {
                case "Monday":
                    return "Thứ hai";
                case "Tuesday":
                    return "Thứ ba";
                case "Wednesday":
                    return "Thứ tư";
                case "Thursday":
                    return "Thứ năm";
                case "Friday":
                    return "Thứ sáu";
                case "Saturday":
                    return "Thứ bảy";
                case "Sunday":
                    return "Chủ nhật";
                default:
                    return ("");
            }
        }

        public static string ConvertNumberToDisplay(string Num)
        {
            CultureInfo cul = CultureInfo.GetCultureInfo("vi-VN");   // try with "en-US"
            string str = double.Parse(Num).ToString("#,###", cul.NumberFormat);
            return str;
        }

        public static string StringReadmore(string text, int n = 200, string str_end = "...")
        {
            if (text != null && text != "")
            {
                text = text.Trim();
            }


            string new_text = text;
            if (text != null && text.Length > n)
            {
                int str_limit = text.IndexOf(" ", n);
                if (str_limit > n)
                {
                    new_text = text.Substring(0, str_limit);
                }
                else
                {
                    new_text = text.Substring(0, n);
                }

                if (new_text.LastIndexOf(",") == new_text.Length - 1)
                {
                    new_text = new_text.Remove(new_text.Length - 1);
                }
                return new_text + str_end;
            }
            else
            {
                return new_text;
            }


        }
        public static string RemoveEventJavascript(string html)
        {            
            var sanitizer = new HtmlSanitizer();
            var sanitizedHtml = sanitizer.Sanitize(html);
            var decodedHtml = WebUtility.HtmlDecode(sanitizedHtml);
            return decodedHtml;

        }
        public static string RemoveTagsLink(string html)
        {
            if (html != null && html != "")
            {                
                var sanitizer = new HtmlSanitizer();
                sanitizer.AllowedSchemes.Add("mailto");
                var sanitizedHtml = sanitizer.Sanitize(html);
                var decodedHtml = WebUtility.HtmlDecode(sanitizedHtml);
                return decodedHtml;
            }
            else
            {
                return "";
            }

        }

        public static string RemoveTagsTitle(string html)
        {
            if (html != null && html != "")
            {                
                html = RemoveEventJavascript(html);
                return html.Trim();
            }
            else
            {
                return "";
            }

        }

        public static string RemoveTagsIntroText(string html)
        {
            if (html != null && html != "")
            {               
                html = RemoveEventJavascript(html);
                return html;
            }
            else
            {
                return "";
            }
        }

        public static string RemoveEventFulltextArticle(string html)
        {
            var sanitizer = new HtmlSanitizer();
            sanitizer.AllowedTags.Add("iframe");
            sanitizer.AllowedTags.Add("audio");
            sanitizer.AllowedTags.Add("video");
            sanitizer.AllowedTags.Add("source");
            sanitizer.AllowedAttributes.Add("class");
            sanitizer.AllowedAttributes.Add("controls");
            sanitizer.AllowedAttributes.Add("toggle");
            sanitizer.AllowedAttributes.Add("selected");
            sanitizer.AllowedAttributes.Add("target");
            sanitizer.AllowedAttributes.Add("id");
            sanitizer.AllowedAttributes.Add("role");
            sanitizer.AllowedAttributes.Add("data-toggle");
            sanitizer.AllowedAttributes.Add("data-target");
            sanitizer.AllowedAttributes.Add("aria-controls");
            sanitizer.AllowedAttributes.Add("aria-selected");
            var sanitizedHtml = sanitizer.Sanitize(html);

            var decodedHtml = WebUtility.HtmlDecode(sanitizedHtml);
            return decodedHtml;

        }
        public static string RemoveTagsFullText(string html)
        {
            if (html != null && html != "")
            {                
                html = RemoveEventJavascript(html);
                return html;
            }
            else
            {
                return "";
            }

        }

        public static string RemoveTagsFullTextArticle(string html)
        {
            if (html != null && html != "")
            {
                html = RemoveEventFulltextArticle(html);
                return html;
            }
            else
            {
                return "";
            }

        }

        public static string RemoveHtmlTags(object source)
        {
            if(source == null)
            {
                return "";
            }
            else
            {
                RegexOptions options = RegexOptions.None;
                Regex regex = new Regex(@"[ ]{2,}", options);
                source = regex.Replace(source.ToString(), @" ");

                return Regex.Replace(source.ToString(), "<.*?>", string.Empty);
            }
            

        }
        /// <summary>
        /// Creates a URL And SEO friendly slug
        /// </summary>
        /// <param name="text">Text to slugify</param>
        /// <param name="maxLength">Max length of slug</param>
        /// <returns>URL and SEO friendly string</returns>
        public static string UrlFriendly(string text, int maxLength = 0)
        {
            if(text!=null && text != "")
            {
                System.Text.RegularExpressions.Regex rRemScript = new System.Text.RegularExpressions.Regex(@"<script[^>]*>[\s\S]*?</script>");
                text = rRemScript.Replace(text, "").Trim();

                // Return empty value if text is null
                if (text == null) return "";
                var normalizedString = text
                    // Make lowercase
                    .ToLowerInvariant()
                    // Normalize the text
                    .Normalize(NormalizationForm.FormD);
                var stringBuilder = new StringBuilder();
                var stringLength = normalizedString.Length;
                var prevdash = false;
                var trueLength = 0;
                char c;
                for (int i = 0; i < stringLength; i++)
                {
                    c = normalizedString[i];
                    switch (CharUnicodeInfo.GetUnicodeCategory(c))
                    {
                        // Check if the character is a letter or a digit if the character is a
                        // international character remap it to an ascii valid character
                        case UnicodeCategory.LowercaseLetter:
                        case UnicodeCategory.UppercaseLetter:
                        case UnicodeCategory.DecimalDigitNumber:
                            if (c < 128)
                                stringBuilder.Append(c);
                            else
                                stringBuilder.Append(ConstHelper.RemapInternationalCharToAscii(c));
                            prevdash = false;
                            trueLength = stringBuilder.Length;
                            break;
                        // Check if the character is to be replaced by a hyphen but only if the last character wasn't
                        case UnicodeCategory.SpaceSeparator:
                        case UnicodeCategory.ConnectorPunctuation:
                        case UnicodeCategory.DashPunctuation:
                        case UnicodeCategory.OtherPunctuation:
                        case UnicodeCategory.MathSymbol:
                            if (!prevdash)
                            {
                                stringBuilder.Append('-');
                                prevdash = true;
                                trueLength = stringBuilder.Length;
                            }
                            break;
                    }
                    // If we are at max length, stop parsing
                    if (maxLength > 0 && trueLength >= maxLength)
                        break;
                }
                // Trim excess hyphens
                var result = stringBuilder.ToString().Trim('-');
                // Remove any excess character to meet maxlength criteria
                return maxLength <= 0 || result.Length <= maxLength ? result : result.Substring(0, maxLength);
            }
            else
            {
                return "";
            }
            
        }

        public static class ConstHelper
        {
            /// <summary>
            /// Remaps international characters to ascii compatible ones
            /// based of: https://meta.stackexchange.com/questions/7435/non-us-ascii-characters-dropped-from-full-profile-url/7696#7696
            /// </summary>
            /// <param name="c">Charcter to remap</param>
            /// <returns>Remapped character</returns>
            public static string RemapInternationalCharToAscii(char c)
            {
                string s = c.ToString().ToLowerInvariant();
                if ("àåáâäãåą".Contains(s))
                {
                    return "a";
                }
                else if ("èéêëę".Contains(s))
                {
                    return "e";
                }
                else if ("ìíîïı".Contains(s))
                {
                    return "i";
                }
                else if ("òóôõöøőð".Contains(s))
                {
                    return "o";
                }
                else if ("ùúûüŭů".Contains(s))
                {
                    return "u";
                }
                else if ("çćčĉ".Contains(s))
                {
                    return "c";
                }
                else if ("żźž".Contains(s))
                {
                    return "z";
                }
                else if ("śşšŝ".Contains(s))
                {
                    return "s";
                }
                else if ("ñń".Contains(s))
                {
                    return "n";
                }
                else if ("ýÿ".Contains(s))
                {
                    return "y";
                }
                else if ("ğĝ".Contains(s))
                {
                    return "g";
                }
                else if (c == 'ř')
                {
                    return "r";
                }
                else if (c == 'ł')
                {
                    return "l";
                }
                else if (c == 'đ')
                {
                    return "d";
                }
                else if (c == 'ß')
                {
                    return "ss";
                }
                else if (c == 'þ')
                {
                    return "th";
                }
                else if (c == 'ĥ')
                {
                    return "h";
                }
                else if (c == 'ĵ')
                {
                    return "j";
                }
                else
                {
                    return "";
                }
            }
        }

        public static string ConvertHTMLToPlanText(string source, Boolean htmlr = true)
        {
            try
            {
                string result;

                // Remove HTML Development formatting
                // Replace line breaks with space
                // because browsers inserts space
                result = source.Replace("\r", " ");
                // Replace line breaks with space
                // because browsers inserts space
                result = result.Replace("\n", " ");
                // Remove step-formatting
                result = result.Replace("\t", string.Empty);
                // Remove repeating spaces because browsers ignore them
                result = Regex.Replace(result,
                                                                      @"( )+", " ");

                // Remove the header (prepare first by clearing attributes)
                result = Regex.Replace(result,
                         @"<( )*head([^>])*>", "<head>",
                         RegexOptions.IgnoreCase);
                result = Regex.Replace(result,
                         @"(<( )*(/)( )*head( )*>)", "</head>",
                         RegexOptions.IgnoreCase);
                result = Regex.Replace(result,
                         "(<head>).*(</head>)", string.Empty,
                         RegexOptions.IgnoreCase);

                // remove all scripts (prepare first by clearing attributes)
                result = Regex.Replace(result,
                         @"<( )*script([^>])*>", "<script>",
                         RegexOptions.IgnoreCase);
                result = Regex.Replace(result,
                         @"(<( )*(/)( )*script( )*>)", "</script>",
                         RegexOptions.IgnoreCase);
                //result = Regex.Replace(result,
                //         @"(<script>)([^(<script>\.</script>)])*(</script>)",
                //         string.Empty,
                //         RegexOptions.IgnoreCase);
                result = Regex.Replace(result,
                         @"(<script>).*(</script>)", string.Empty,
                         RegexOptions.IgnoreCase);

                // remove all styles (prepare first by clearing attributes)
                result = Regex.Replace(result,
                         @"<( )*style([^>])*>", "<style>",
                         RegexOptions.IgnoreCase);
                result = Regex.Replace(result,
                         @"(<( )*(/)( )*style( )*>)", "</style>",
                         RegexOptions.IgnoreCase);
                result = Regex.Replace(result,
                         "(<style>).*(</style>)", string.Empty,
                         RegexOptions.IgnoreCase);

                // insert tabs in spaces of <td> tags
                result = Regex.Replace(result,
                         @"<( )*td([^>])*>", "\t",
                         RegexOptions.IgnoreCase);

                // insert line breaks in places of <BR> and <LI> tags
                result = Regex.Replace(result,
                         @"<( )*br( )*>", "\r",
                         RegexOptions.IgnoreCase);
                result = Regex.Replace(result,
                         @"<( )*li( )*>", "\r",
                         RegexOptions.IgnoreCase);

                // insert line paragraphs (double line breaks) in place
                // if <P>, <DIV> and <TR> tags
                result = Regex.Replace(result,
                         @"<( )*div([^>])*>", "\r\r",
                         RegexOptions.IgnoreCase);
                result = Regex.Replace(result,
                         @"<( )*tr([^>])*>", "\r\r",
                         RegexOptions.IgnoreCase);
                result = Regex.Replace(result,
                         @"<( )*p([^>])*>", "\r\r",
                         RegexOptions.IgnoreCase);

                // Remove remaining tags like <a>, links, images,
                // comments etc - anything that's enclosed inside < >
                result = Regex.Replace(result,
                         @"<[^>]*>", string.Empty,
                         RegexOptions.IgnoreCase);

                // replace special characters:
                result = Regex.Replace(result,
                         @" ", " ",
                         RegexOptions.IgnoreCase);

                result = Regex.Replace(result,
                         @"&bull;", " * ",
                         RegexOptions.IgnoreCase);
                result = Regex.Replace(result,
                         @"&lsaquo;", "<",
                         RegexOptions.IgnoreCase);
                result = Regex.Replace(result,
                         @"&rsaquo;", ">",
                         RegexOptions.IgnoreCase);
                result = Regex.Replace(result,
                         @"&trade;", "(tm)",
                         RegexOptions.IgnoreCase);
                result = Regex.Replace(result,
                         @"&frasl;", "/",
                         RegexOptions.IgnoreCase);
                result = Regex.Replace(result,
                         @"&lt;", "<",
                         RegexOptions.IgnoreCase);
                result = Regex.Replace(result,
                         @"&gt;", ">",
                         RegexOptions.IgnoreCase);
                result = Regex.Replace(result,
                         @"&copy;", "(c)",
                         RegexOptions.IgnoreCase);
                result = Regex.Replace(result,
                         @"&reg;", "(r)",
                         RegexOptions.IgnoreCase);
                // Remove all others. More can be added, see
                // http://hotwired.lycos.com/webmonkey/reference/special_characters/
                result = Regex.Replace(result,
                         @"&(.{2,6});", string.Empty,
                         RegexOptions.IgnoreCase);

                // for testing
                //Regex.Replace(result,
                //       this.txtRegex.Text,string.Empty,
                //       RegexOptions.IgnoreCase);

                // make line breaking consistent
                result = result.Replace("\n", "\r");

                // Remove extra line breaks and tabs:
                // replace over 2 breaks with 2 and over 4 tabs with 4.
                // Prepare first to remove any whitespaces in between
                // the escaped characters and remove redundant tabs in between line breaks
                result = Regex.Replace(result,
                         "(\r)( )+(\r)", "\r\r",
                         RegexOptions.IgnoreCase);
                result = Regex.Replace(result,
                         "(\t)( )+(\t)", "\t\t",
                         RegexOptions.IgnoreCase);
                result = Regex.Replace(result,
                         "(\t)( )+(\r)", "\t\r",
                         RegexOptions.IgnoreCase);
                result = Regex.Replace(result,
                         "(\r)( )+(\t)", "\r\t",
                         RegexOptions.IgnoreCase);
                // Remove redundant tabs
                result = Regex.Replace(result,
                         "(\r)(\t)+(\r)", "\r\r",
                         RegexOptions.IgnoreCase);
                // Remove multiple tabs following a line break with just one tab
                result = Regex.Replace(result,
                         "(\r)(\t)+", "\r\t",
                         RegexOptions.IgnoreCase);
                // Initial replacement target string for line breaks
                string breaks = "\r\r\r";
                // Initial replacement target string for tabs
                string tabs = "\t\t\t\t\t";
                for (int index = 0; index < result.Length; index++)
                {
                    result = result.Replace(breaks, "\r\r");
                    result = result.Replace(tabs, "\t\t\t\t");
                    breaks = breaks + "\r";
                    tabs = tabs + "\t";
                }

                if (htmlr)
                {
                    result = result.Replace("\r", "");
                }
                // That's it.
                return result;
            }
            catch
            {

                return "";
            }
        }
        public static String NumberToTextVN(decimal total)
        {
            try
            {
                string rs = "";
                total = Math.Round(total, 0);
                string[] ch = { "không", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín" };
                string[] rch = { "lẻ", "mốt", "", "", "", "lăm" };
                string[] u = { "", "mươi", "trăm", "ngàn", "", "", "triệu", "", "", "tỷ", "", "", "ngàn", "", "", "triệu" };
                string nstr = total.ToString();

                int[] n = new int[nstr.Length];
                int len = n.Length;
                for (int i = 0; i < len; i++)
                {
                    n[len - 1 - i] = Convert.ToInt32(nstr.Substring(i, 1));
                }

                for (int i = len - 1; i >= 0; i--)
                {
                    if (i % 3 == 2)// số 0 ở hàng trăm
                    {
                        if (n[i] == 0 && n[i - 1] == 0 && n[i - 2] == 0) continue;//nếu cả 3 số là 0 thì bỏ qua không đọc
                    }
                    else if (i % 3 == 1) // số ở hàng chục
                    {
                        if (n[i] == 0)
                        {
                            if (n[i - 1] == 0) { continue; }// nếu hàng chục và hàng đơn vị đều là 0 thì bỏ qua.
                            else
                            {
                                rs += " " + rch[n[i]]; continue;// hàng chục là 0 thì bỏ qua, đọc số hàng đơn vị
                            }
                        }
                        if (n[i] == 1)//nếu số hàng chục là 1 thì đọc là mười
                        {
                            rs += " mười"; continue;
                        }
                    }
                    else if (i != len - 1)// số ở hàng đơn vị (không phải là số đầu tiên)
                    {
                        if (n[i] == 0)// số hàng đơn vị là 0 thì chỉ đọc đơn vị
                        {
                            if (i + 2 <= len - 1 && n[i + 2] == 0 && n[i + 1] == 0) continue;
                            rs += " " + (i % 3 == 0 ? u[i] : u[i % 3]);
                            continue;
                        }
                        if (n[i] == 1)// nếu là 1 thì tùy vào số hàng chục mà đọc: 0,1: một / còn lại: mốt
                        {
                            rs += " " + ((n[i + 1] == 1 || n[i + 1] == 0) ? ch[n[i]] : rch[n[i]]);
                            rs += " " + (i % 3 == 0 ? u[i] : u[i % 3]);
                            continue;
                        }
                        if (n[i] == 5) // cách đọc số 5
                        {
                            if (n[i + 1] != 0) //nếu số hàng chục khác 0 thì đọc số 5 là lăm
                            {
                                rs += " " + rch[n[i]];// đọc số 
                                rs += " " + (i % 3 == 0 ? u[i] : u[i % 3]);// đọc đơn vị
                                continue;
                            }
                        }
                    }

                    rs += (rs == "" ? " " : ", ") + ch[n[i]];// đọc số
                    rs += " " + (i % 3 == 0 ? u[i] : u[i % 3]);// đọc đơn vị
                }
                if (rs[rs.Length - 1] != ' ')
                    rs += " đồng";
                else
                    rs += "đồng";

                if (rs.Length > 2)
                {
                    string rs1 = rs.Substring(0, 2);
                    rs1 = rs1.ToUpper();
                    rs = rs.Substring(2);
                    rs = rs1 + rs;
                }
                return rs.Trim().Replace("lẻ,", "lẻ").Replace("mươi,", "mươi").Replace("trăm,", "trăm").Replace("mười,", "mười");
            }
            catch
            {
                return "";
            }

        }
    }
}
