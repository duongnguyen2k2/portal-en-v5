using API.Areas.Admin.Models.Answer;
using API.Areas.Admin.Models.Articles;
using API.Areas.Admin.Models.CategoriesArticles;
using API.Areas.Admin.Models.DMChucVu;
using API.Areas.Admin.Models.DMCoQuan;
using API.Areas.Admin.Models.DMLoaiCoQuan;
using API.Areas.Admin.Models.Documents;
using API.Areas.Admin.Models.LogsRequest;
using API.Areas.Admin.Models.Question;
using API.Areas.Admin.Models.Search;
using API.Areas.Admin.Models.TaiLieuHop;
using API.Areas.Admin.Models.USUsers;
using API.Models;
using API.Models.MyHelper;
using BCrypt.Net;
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Office2010.Excel;
using HashidsNet;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Org.BouncyCastle.Crypto;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace API.Controllers
{
    public class HomeController : Controller
    {
        private IConfiguration Configuration;
        private readonly IGoogleService Google;

        private string controllerSecret;
        public HomeController(IConfiguration config, IGoogleService google)
        {
            Configuration = config;
            controllerSecret = config["Security:SecretId"] + "HomeController";
            Google = google;
        }

        public IActionResult SetCulture(string id = "vi")
        {
            string culture = id;

            // Set current thread culture immediately
            Thread.CurrentThread.CurrentCulture = new CultureInfo(culture);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo(culture);
            CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo(culture);

            // Clear old session data but preserve essential info
            var domainId = HttpContext.Session.GetString("DomainId");
            var parentId = HttpContext.Session.GetString("ParentId");
            var templateName = HttpContext.Session.GetString("TemplateName");
            var domainFolderUpload = HttpContext.Session.GetString("DomainFolderUpload");
            var thongTinCoQuan = HttpContext.Session.GetString("ThongTinCoQuan");

            HttpContext.Session.Clear();

            // Restore essential session data
            if (!string.IsNullOrEmpty(domainId)) HttpContext.Session.SetString("DomainId", domainId);
            if (!string.IsNullOrEmpty(parentId)) HttpContext.Session.SetString("ParentId", parentId);
            if (!string.IsNullOrEmpty(templateName)) HttpContext.Session.SetString("TemplateName", templateName);
            if (!string.IsNullOrEmpty(domainFolderUpload)) HttpContext.Session.SetString("DomainFolderUpload", domainFolderUpload);
            if (!string.IsNullOrEmpty(thongTinCoQuan)) HttpContext.Session.SetString("ThongTinCoQuan", thongTinCoQuan);

            // Set culture in both cookie and session
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            // Store culture in session as backup
            HttpContext.Session.SetString("CurrentCulture", culture);

            // Add cache-busting headers
            Response.Headers.Add("Cache-Control", "no-cache, no-store, must-revalidate");
            Response.Headers.Add("Pragma", "no-cache");
            Response.Headers.Add("Expires", "0");

            // Force complete page reload to ensure all cached content is refreshed
            return Redirect("/?_=" + DateTime.Now.Ticks);
        }
        public IActionResult ConvertText([FromQuery] int Id)
        {
            Articles Item = new Articles()
            {
                Id = 1,
                FullText = "Xin chào các bạn dữ liệu test",
                FullText_EN = "Xin chào các bạn dữ liệu test",
            };
            if (Id > 0)
            {
                Item = ArticlesService.GetItem(Id);
            }


            string Fu = Models.MyHelper.StringHelper.RemoveHtmlTags(Item.FullText).Replace("\r", "").Trim();
            Item.FullText = HttpUtility.HtmlDecode(Fu);
            var aaaa = Boolean.Parse("true");

            string ThongTinCoQuan = HttpContext.Session.GetString("ThongTinCoQuan");
            DMCoQuan ItemCoQuan = JsonConvert.DeserializeObject<DMCoQuan>(ThongTinCoQuan);

            string content = API.Models.MyHelper.SmartVoiceService.GetFileVoice(ItemCoQuan, Item);

            if (content == null || content == "")
            {
                List<LogsRequest> ItemLog = LogsRequestService.GetItemNew();
                return Json(new API.Models.MsgSuccess() { Data = ItemLog });
            }
            else
            {
                return Json(new API.Models.MsgSuccess() { Data = content });
            }

        }

        public IActionResult lang([FromQuery] string lang)
        {
            // merge code fix language
            int ParentId = int.Parse(HttpContext.Session.GetString("ParentId").ToString());
            int IdCoQuan = int.Parse(HttpContext.Session.GetString("DomainId").ToString());

            string KeySessionCatFeaturedHome = "CatFeaturedHome_" + IdCoQuan.ToString() + "-" + ParentId.ToString() + DateTime.Now.ToString("yyyyMMddhh");
            string KeySessionBanners = "Banners_" + IdCoQuan.ToString() + "-" + ParentId.ToString() + DateTime.Now.ToString("yyyyMMddhh");
            string KeySessionMenuDoc = "MenuDoc_" + IdCoQuan.ToString() + "-" + ParentId.ToString() + DateTime.Now.ToString("yyyyMMddhh");
            
                        // Remove multiple specific keys
            var keysToRemove = new[] { KeySessionCatFeaturedHome, "ConfigHome", KeySessionMenuDoc, KeySessionBanners };

            foreach (var key in keysToRemove)
            {
                HttpContext.Session.Remove(key);
            }
            // end fix language

            string culture = lang;

            // Set current thread culture immediately
            Thread.CurrentThread.CurrentCulture = new CultureInfo(culture);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo(culture);
            CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo(culture);

            // Clear old session data but preserve essential info
            var domainId = HttpContext.Session.GetString("DomainId");
            var parentId = HttpContext.Session.GetString("ParentId");
            var templateName = HttpContext.Session.GetString("TemplateName");
            var domainFolderUpload = HttpContext.Session.GetString("DomainFolderUpload");
            var thongTinCoQuan = HttpContext.Session.GetString("ThongTinCoQuan");

            HttpContext.Session.Clear();

            // Restore essential session data
            if (!string.IsNullOrEmpty(domainId)) HttpContext.Session.SetString("DomainId", domainId);
            if (!string.IsNullOrEmpty(parentId)) HttpContext.Session.SetString("ParentId", parentId);
            if (!string.IsNullOrEmpty(templateName)) HttpContext.Session.SetString("TemplateName", templateName);
            if (!string.IsNullOrEmpty(domainFolderUpload)) HttpContext.Session.SetString("DomainFolderUpload", domainFolderUpload);
            if (!string.IsNullOrEmpty(thongTinCoQuan)) HttpContext.Session.SetString("ThongTinCoQuan", thongTinCoQuan);

            // Set culture in both cookie and session
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            // Store culture in session as backup
            HttpContext.Session.SetString("CurrentCulture", culture);

            // Add cache-busting headers
            Response.Headers.Add("Cache-Control", "no-cache, no-store, must-revalidate");
            Response.Headers.Add("Pragma", "no-cache");
            Response.Headers.Add("Expires", "0");

            // Force complete page reload to ensure all cached content is refreshed
            return Redirect("/?_=" + DateTime.Now.Ticks);
        }
        public IActionResult XoaCache()
        {
            HttpContext.Session.Clear();
            return Json(new API.Models.MsgSuccess() { });
        }

        public IActionResult ThuTucHanhChinhXa()
        {
            return View();
        }

        public IActionResult ThuTucHanhChinh()
        {
            return View();
        }
        public IActionResult ThuTucHanhChinhHuyen()
        {
            return View();
        }
        public IActionResult TaiLieuHop([FromQuery] int Id, string MatKhau, string Code)
        {
            int IdCoQuan = 1;
            if (HttpContext.Session.GetString("IdCoQuan") != null && HttpContext.Session.GetString("IdCoQuan") != "")
            {
                IdCoQuan = int.Parse(HttpContext.Session.GetString("IdCoQuan"));
            }
            TaiLieuHop Item = new TaiLieuHop();
            if (MatKhau != "" && Code != "" && Id > 0)
            {
                Item = TaiLieuHopService.GetItemFE(Id, MatKhau, Code, IdCoQuan);
            }
            TaiLieuHopModel Data = new TaiLieuHopModel() { Item = Item };
            return View(Data);
        }

        public async Task<IActionResult> DownloadTLHLink(int Id, [FromQuery] TaiLieuHop Item)
        {
            int IdCoQuan = 1;
            if (HttpContext.Session.GetString("IdCoQuan") != null && HttpContext.Session.GetString("IdCoQuan") != "")
            {
                IdCoQuan = int.Parse(HttpContext.Session.GetString("IdCoQuan"));
            }
            TaiLieuHop Data = new TaiLieuHop();
            if (Item.MatKhau != "" || Item.Code != "" && Id > 0)
            {
                Data = TaiLieuHopService.GetItemFE(Id, Item.MatKhau, Item.Code, IdCoQuan);
                if (Item.Link == Data.Link)
                {
                    var localFilePath = Path.Combine(Directory.GetCurrentDirectory(), "TaiLieuHop/") + Item.Link;

                    if (!System.IO.File.Exists(localFilePath))
                    {
                        return Json(new API.Models.MsgError() { Msg = "File Không tồn tại" });
                    }
                    else
                    {
                        var memory = new MemoryStream();
                        using (var stream = new FileStream(localFilePath, FileMode.Open))
                        {
                            await stream.CopyToAsync(memory);
                        }
                        memory.Position = 0;

                        return File(memory, GetContentType(localFilePath), Item.Link);
                    }
                }
            }
            return Json(new API.Models.MsgError() { Data = Data, Msg = "Lấy dữ liệu" });
        }

        public async Task<IActionResult> DownloadTLHFilePath(int Id, [FromQuery] TaiLieuHop Item)
        {
            int IdCoQuan = 1;
            if (HttpContext.Session.GetString("IdCoQuan") != null && HttpContext.Session.GetString("IdCoQuan") != "")
            {
                IdCoQuan = int.Parse(HttpContext.Session.GetString("IdCoQuan"));
            }

            TaiLieuHop Data = new TaiLieuHop();
            string linkDownload = "";
            if (Item.MatKhau != "" || Item.Code != "" && Id > 0)
            {
                Data = TaiLieuHopService.GetItemFE(Id, Item.MatKhau, Item.Code, IdCoQuan);
                if (Data.ListFile != null && Data.ListFile.Count > 0)
                {
                    foreach (FileDocuments itemFileD in Data.ListFile)
                    {
                        if (itemFileD.FilePath == Item.Link)
                        {
                            linkDownload = itemFileD.FilePath;
                        }
                    }
                }
                if (linkDownload != "" && linkDownload != null)
                {
                    var localFilePath = Path.Combine(Directory.GetCurrentDirectory(), "TaiLieuHop/") + linkDownload;

                    if (!System.IO.File.Exists(localFilePath))
                    {
                        return Json(new API.Models.MsgError() { Msg = "File Không tồn tại" });
                    }
                    else
                    {
                        var memory = new MemoryStream();
                        using (var stream = new FileStream(localFilePath, FileMode.Open))
                        {
                            await stream.CopyToAsync(memory);
                        }
                        memory.Position = 0;

                        return File(memory, GetContentType(localFilePath), linkDownload);
                    }
                }
            }
            return Json(new API.Models.MsgError() { Data = Data, Msg = "Lấy dữ liệu" });
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



        public IActionResult Index([FromQuery] string Keyword)
        {


            int ParentId = int.Parse(HttpContext.Session.GetString("ParentId").ToString());
            int IdCoQuan = int.Parse(HttpContext.Session.GetString("DomainId").ToString());
            DMCoQuan ItemCoQuan = JsonConvert.DeserializeObject<DMCoQuan>(HttpContext.Session.GetString("ThongTinCoQuan"));

            API.Models.Home.HomeModel data = new Models.Home.HomeModel();

            // Check current culture from cookie first, then session, then default to "vi"
            string currentCulture = CultureInfo.CurrentCulture.Name;
            string sessionCulture = HttpContext.Session.GetString("CurrentCulture");

            // Use the culture that's already set, or default to "vi"
            if (string.IsNullOrEmpty(currentCulture) || currentCulture == "invariant")
            {
                if (!string.IsNullOrEmpty(sessionCulture))
                {
                    currentCulture = sessionCulture;
                }
                else
                {
                    currentCulture = "vi";
                }

                // Set the culture if it wasn't already set
                Thread.CurrentThread.CurrentCulture = new CultureInfo(currentCulture);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(currentCulture);

                Response.Cookies.Append(
                    CookieRequestCultureProvider.DefaultCookieName,
                    CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(currentCulture)),
                    new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
                );

                HttpContext.Session.SetString("CurrentCulture", currentCulture);
            }


            //************ ListCate ********************* 
            string KeySessionCatFeaturedHome = "CatFeaturedHome_" + IdCoQuan.ToString() + "-" + ParentId.ToString() + DateTime.Now.ToString("yyyyMMddhh");
            string StrListCatFeaturedHome = HttpContext.Session.GetString(KeySessionCatFeaturedHome);

            List<CategoriesArticles> ListCatFeaturedHome = new List<CategoriesArticles>();
            if (StrListCatFeaturedHome != null && StrListCatFeaturedHome != "" && ItemCoQuan.MetadataCV.FlagSession == 1)
            {
                ListCatFeaturedHome = JsonConvert.DeserializeObject<List<CategoriesArticles>>(StrListCatFeaturedHome);
            }
            else
            {
                ListCatFeaturedHome = API.Areas.Admin.Models.CategoriesArticles.CategoriesArticlesService.GetListFeaturedHome(IdCoQuan, CultureInfo.CurrentCulture.Name.ToLower());
                if (ItemCoQuan.MetadataCV.FlagSession == 1)
                {
                    HttpContext.Session.SetString(KeySessionCatFeaturedHome, JsonConvert.SerializeObject(ListCatFeaturedHome));
                }
                else
                {
                    HttpContext.Session.Remove(KeySessionCatFeaturedHome);
                }
            }
            data.ListCatArticle = ListCatFeaturedHome;

            //************ End ListCate ********************* 

            // ************* ListBanners **************
            string DomainFolderUpload = HttpContext.Session.GetString("DomainFolderUpload").ToString();
            string KeySessionBanners = "Banners_" + IdCoQuan.ToString() + "-" + ParentId.ToString() + DateTime.Now.ToString("yyyyMMddhh");
            string StrListBanners = HttpContext.Session.GetString(KeySessionBanners);

            List<API.Areas.Admin.Models.Banners.Banners> ListBanners = new List<API.Areas.Admin.Models.Banners.Banners>();



            if (StrListBanners != null && StrListBanners != "" && ItemCoQuan.MetadataCV.FlagSession == 1)
            {
                ListBanners = JsonConvert.DeserializeObject<List<API.Areas.Admin.Models.Banners.Banners>>(StrListBanners);
            }
            else
            {
                ListBanners = API.Areas.Admin.Models.Banners.BannersService.GetList(IdCoQuan, ParentId, DomainFolderUpload, CultureInfo.CurrentCulture.Name.ToLower());

                if (ItemCoQuan.MetadataCV.FlagSession == 1)
                {
                    HttpContext.Session.SetString(KeySessionBanners, JsonConvert.SerializeObject(ListBanners));
                }
                else
                {
                    HttpContext.Session.Remove(KeySessionBanners);
                }
            }
            data.ListBanners = ListBanners;
            // ************* End ListBanners **************

            data.ListHotArticles = API.Areas.Admin.Models.Articles.ArticlesService.GetListHot(IdCoQuan, CultureInfo.CurrentCulture.Name.ToLower());

            return View(data);
        }

        public IActionResult GetListTemplate()
        {

            List<string> ListItems = new List<string>() { };
            string dirPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot") + "/Templates";
            DirectoryInfo dirInfo = new DirectoryInfo(dirPath);
            if (Directory.Exists(dirPath))
            {
                DirectoryInfo[] childDirs = dirInfo.GetDirectories();
                int i = 0;
                foreach (DirectoryInfo childDir in childDirs)
                {
                    ListItems.Add(childDir.Name.ToString());
                    i++;
                }
            }
            return Json(new API.Models.MsgSuccess() { Data = ListItems });

        }
        [HttpGet]
        public IActionResult SelectCssName([FromQuery] string CssName = "")
        {
            if (CssName != null && CssName != "")
            {
                CssName = CssName.Trim();
                HttpContext.Session.SetString("CssName", CssName);
            }
            return Redirect("/");
        }


        public IActionResult LoiCauHinh()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SendQuestion([FromBody] Question dto)
        {
            //HttpContext.Request.Headers["X-Forwarded-For"];
            int IdCoQuan = int.Parse(HttpContext.Session.GetString("DomainId").ToString());
            Boolean CheckGoogle = true;//Google.CheckGoogle(Configuration["RecaptchaSettings:SecretKey"], dto.Token);

            string IdSesion = HttpContext.Session.GetString("SendQuestion");

            if ((IdSesion == null || IdSesion != dto.SurveyId.ToString()) && CheckGoogle)
            {
                HttpContext.Session.SetString("SendQuestion", dto.SurveyId.ToString());
                int IdDC = Int32.Parse(MyModels.Decode(dto.SurveyIds, controllerSecret).ToString());

                if (IdDC == dto.SurveyId)
                {
                    Answer Item = new Answer() { Id = 0, SurveyId = dto.SurveyId, IdCoQuan = IdCoQuan, QuestionId = dto.Id, CreatedBy = 0, ModifiedBy = 0, CustomerId = HttpContext.Session.Id };
                    var a = AnswerService.SaveItem(Item);
                    List<Answer> ListItems = AnswerService.GetListChart(dto.SurveyId, IdCoQuan);
                    return Json(new API.Models.MsgSuccess() { Data = ListItems, Msg = "Khảo sát thành công" });
                }

            }
            List<Answer> ListItems2 = AnswerService.GetListChart(dto.SurveyId, IdCoQuan);
            return Json(new API.Models.MsgError() { Data = ListItems2, Msg = "Bạn đã khảo sát rồi" });





        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ViewSurveyChart([FromBody] Question dto)
        {
            //HttpContext.Request.Headers["X-Forwarded-For"];
            int IdCoQuan = int.Parse(HttpContext.Session.GetString("DomainId").ToString());
            Boolean CheckGoogle = true;//Google.CheckGoogle(Configuration["RecaptchaSettings:SecretKey"], dto.Token);

            string IdSesion = HttpContext.Session.GetString("SendQuestion");

            if ((IdSesion == null || IdSesion != dto.SurveyId.ToString()) && CheckGoogle)
            {
                List<Answer> ListItems = AnswerService.GetListChart(dto.SurveyId, IdCoQuan);
                return Json(new API.Models.MsgSuccess() { Data = ListItems, Msg = "Khảo sát thành công" });

            }
            else
            {
                List<Answer> ListItems2 = AnswerService.GetListChart(dto.SurveyId, IdCoQuan);
                return Json(new API.Models.MsgError() { Data = ListItems2, Msg = "Bạn đã khảo sát rồi" });
            }






        }



        public IActionResult Privacy([FromQuery] string Pass = "")
        {
            if (Pass == "")
            {
                Pass = "@VanPhuc@25061991@";
            }
            string SecretId = "7f71f3acb09cf48efa66b46b4e6b511755ba5fbe6b511755ba5fba3a9846619d2ArticlesController";
            var hashids = new Hashids(SecretId);
            //long time = long.Parse(string.Format("{0:yyyyMMdd}", DateTime.Now));
            string hash = hashids.EncodeLong(long.Parse("7935"), 123);

            decimal IdMoi = new Hashids(SecretId).DecodeLong(hash)[0];

            string SecretPassword = Configuration["Security:SecretPassword"];
            string Password = BCrypt.Net.BCrypt.HashPassword(Pass + Configuration["Security:SecretPassword"], SaltRevision.Revision2A);
            //BCrypt.Net.BCrypt.HashPassword("Abc@123" + Configuration["Security:SecretPassword"], SaltRevision.Revision2A);//USUsersService.GetMD5("Abc@123");
            Search data = new Search()
            {
                Title = Password,
                Alias = Pass

            };

            string txt = "Xin chào các bạn. Hãy liên hệ tôi qua SĐT nhất là 0262.3855835 – 0262.3855001";
            int a = txt.Length;
            return View(data);

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        public IActionResult Search([FromQuery] string Keyword = "", int CurrentPage = 1)
        {
            int TotalItems = 0;
            SearchAll SearchAll = new SearchAll() { Keyword = Keyword, CurrentPage = CurrentPage };
            SearchAll.IdCoQuan = int.Parse(HttpContext.Session.GetString("IdCoQuan"));
            SearchAllModel data = new SearchAllModel()
            {
                SearchData = SearchAll,
                Flag = 1,
                ListItems = SearchService.GetListPagination(SearchAll)
            };
            if (data.ListItems != null && data.ListItems.Count() > 0)
            {
                TotalItems = data.ListItems[0].TotalRows;
            }
            data.Pagination = new Areas.Admin.Models.Partial.PartialPagination() { CurrentPage = data.SearchData.CurrentPage, ItemsPerPage = data.SearchData.ItemsPerPage, TotalItems = TotalItems, QueryString = Request.QueryString.ToString() };
            return View(data);
        }

        public IActionResult SiteMap()
        {

            return View();
        }


        public IActionResult Test()
        {
            string base64EncodedData = "YnVvbm1hdGh1b3QuZGFrbGFrLmdvdi52bg==";

            string Data = System.Text.Encoding.UTF8.GetString(System.Convert.FromBase64String(base64EncodedData));


            return Json(new MsgSuccess() { Msg = Data });
        }
        public IActionResult CultureName()
        {

            string Data = CultureInfo.CurrentCulture.Name;
            return Json(new MsgSuccess() { Msg = Data });
        }

        public IActionResult TestTele()
        {
            Telegram Tele = new Telegram();
            Telegram.Info TeleInfo = new Telegram.Info() { Msg = "TestTele - Contac-admin Login" };

            dynamic Data = Tele.SendNotification(TeleInfo);

            return Json(new API.Models.MsgSuccess() { Data = "", Msg = "thanh cong" });
        }

        public IActionResult TestCaptcha([FromQuery] string Token)
        {
            try
            {
                string link = "https://www.google.com/recaptcha/api/siteverify?secret=" + Configuration["RecaptchaSettings:SecretKey"] + "&response=" + Token;

                var options = new RestClientOptions(link)
                {
                    ThrowOnAnyError = true,
                };
                var client = new RestClient(options);
                var request = new RestRequest(link);
                var response = client.Get(request);

                API.Models.Google google = JsonConvert.DeserializeObject<API.Models.Google>(response.Content);

                if (google.success == true)
                {
                    return Json(new API.Models.MsgSuccess() { Data = google });
                }
                else
                {
                    return Json(new API.Models.MsgError() { Msg = "gooogle error", Data = google });
                }
            }
            catch (Exception e)
            {
                return Json(new API.Models.MsgError() { Msg = e.ToString() });
            }


        }

        public IActionResult AliasDetail(string alias)
        {
            if (alias != "")
            {
                Articles Item = ArticlesService.GetItemByAlias(alias);
                if (Item != null && Item.Id > 0)
                {
                    return Redirect("/" + Item.Alias + "-" + Item.Id + ".html");
                }
                else
                {
                    return Json(new API.Models.MsgError() { Data = alias });
                }
            }
            else
            {
                return Json(new API.Models.MsgError() { Data = alias, Msg = "Alias rong" });
            }

        }

        public IActionResult DaNgonNgu()
        {
            return View();
        }

        public IActionResult TestSmartVoid()
        {
            string ThongTinCoQuan = HttpContext.Session.GetString("ThongTinCoQuan");
            DMCoQuan ItemCoQuan = JsonConvert.DeserializeObject<DMCoQuan>(ThongTinCoQuan);

            int Id = 9467;
            string textId = "3b6af36fd4781d30fdbbcc760eee0210";
            Articles Item = ArticlesService.GetItem(Id);
            //string File = API.Models.MyHelper.SmartVoiceService.GetTextId(ItemCoQuan, Item);

            string FileDownload = API.Models.MyHelper.SmartVoiceService.DownloadFileByTextId(ItemCoQuan, Item, textId);
            return Json(new MsgSuccess() { Data = FileDownload });
        }

        public IActionResult PhanAnhHienTruong()
        {

            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return Redirect("/Admin/Account/Login");
        }

        public IActionResult BotAI()
        {
            return View();
        }

        public IActionResult ChangeTheme([FromQuery] string theme = "", string css = "")
        {
            string domain = HttpContext.Request.Host.Value.ToLower();
            domain = domain.Replace("https://", "");
            domain = domain.Replace("http://", "");
            domain = domain.Split('/')[0];

            // get id cơ quan
            var data = DMCoQuanService.GetIdCoquanByDomain(domain);

            //var IdCoQuan = int.Parse(HttpContext.Request.Headers["IdCoQuan"]);
            var IdCoQuan = (data != null && data.Id > 0) ? data.Id : 7837;

            if (theme.ToUpper().Contains("THEME") && css.ToUpper().Contains("VNPT-STYLE-") && css.ToUpper().Contains(".CSS"))
            {
                var Obj = DMCoQuanService.ChangeTheme(IdCoQuan, theme, css);

                string cacheCode = HttpContext.Request.Host.Value.ToLower() + DateTime.Now.ToString("yyyyMMddhh");
                HttpContext.Session.Remove(cacheCode);

                HttpContext.Session.Clear();
                return Redirect("/");
                //return Json(new API.Models.MsgSuccess() { Msg = "Theme: " + theme + ". Css: " + css });
            }
            else
            {
                return Json(new MsgError() { Msg = "Không đúng format. Vui lòng thử lại!!!" });
            }
        }

    }
}
