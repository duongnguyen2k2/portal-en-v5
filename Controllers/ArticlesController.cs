using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using API.Areas.Admin.Models.ArticleComment;
using API.Areas.Admin.Models.Articles;
using API.Areas.Admin.Models.CategoriesArticles;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using API.Areas.Admin.Models.DMCoQuan;
using API.Models.MyHelper;
using API.Models;
using Newtonsoft.Json;
using System.Web;
using Microsoft.Extensions.Configuration;

namespace API.Controllers
{
    public class ArticlesController : Controller
    {
        private string controllerName = "ArticlesController";
        private string controllerSecret;
        private IConfiguration Configuration;
        public ArticlesController(IConfiguration config)
        {
            controllerSecret = config["Security:SecretId"] + controllerName;
            Configuration = config;
        }
        public IActionResult Index(string alias, int id, [FromQuery] SearchArticles dto)
        {
            int TotalItems = 0;
            int IdCoQuan = 1;
            if (HttpContext.Session.GetString("IdCoQuan") != null && HttpContext.Session.GetString("IdCoQuan") != "")
            {
                IdCoQuan = int.Parse(HttpContext.Session.GetString("IdCoQuan"));
            }

            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            CategoriesArticles categories = CategoriesArticlesService.GetItem(id, API.Models.Settings.SecretId + ControllerName,0, CultureInfo.CurrentCulture.Name.ToLower());

            dto.CatId = id;
            dto.IdCoQuan = IdCoQuan;
            dto.ShowStartDate = "01/01/2010";
            dto.Status = 1;
            ArticlesModel data = new ArticlesModel() { SearchData = dto, Categories = categories };
            data.ListItems = ArticlesService.GetListPagination(data.SearchData, API.Models.Settings.SecretId + ControllerName, CultureInfo.CurrentCulture.Name.ToLower());
            if (data.ListItems != null && data.ListItems.Count() > 0)
            {
                TotalItems = data.ListItems[0].TotalRows;
            }
            data.Pagination = new Areas.Admin.Models.Partial.PartialPagination() { CurrentPage = data.SearchData.CurrentPage, ItemsPerPage = data.SearchData.ItemsPerPage, TotalItems = TotalItems, QueryString = Request.QueryString.ToString() };
            return View(data);
        }

        public IActionResult Notification([FromQuery] SearchArticles dto)
        {

            int TotalItems = 0;
            int IdCoQuan = 1;
            if (HttpContext.Session.GetString("IdCoQuan") != null && HttpContext.Session.GetString("IdCoQuan") != "")
            {
                IdCoQuan = int.Parse(HttpContext.Session.GetString("IdCoQuan"));
            }
            dto.IdCoQuan = IdCoQuan;
            dto.ShowStartDate = "01/01/2000";
            dto.Status = 1;
            dto.FlagStatus = 2;
            dto.ItemsPerPage = 15;
            ArticlesModel data = new ArticlesModel() { SearchData = dto };
            data.ListItems = ArticlesService.GetListViewNotification(data.SearchData, CultureInfo.CurrentCulture.Name.ToLower());            
            if (data.ListItems != null && data.ListItems.Count() > 0)
            {
                TotalItems = data.ListItems[0].TotalRows;
            }
            data.Pagination = new Areas.Admin.Models.Partial.PartialPagination() { CurrentPage = data.SearchData.CurrentPage, ItemsPerPage = data.SearchData.ItemsPerPage, TotalItems = TotalItems, QueryString = Request.QueryString.ToString() };

            return View(data);
        }

        public IActionResult Featured([FromQuery] SearchArticles dto)
        {

            int TotalItems = 0;
            int IdCoQuan = 1;
            if (HttpContext.Session.GetString("IdCoQuan") != null && HttpContext.Session.GetString("IdCoQuan") != "")
            {
                IdCoQuan = int.Parse(HttpContext.Session.GetString("IdCoQuan"));
            }

            dto.IdCoQuan = IdCoQuan;
            dto.ShowStartDate = "01/01/2000";
            dto.Status = 1;
            dto.FlagStatus = 2;
            dto.ItemsPerPage = 15;
            ArticlesModel data = new ArticlesModel() { SearchData = dto };
            data.ListItems = ArticlesService.GetListViewFeatured(data.SearchData, CultureInfo.CurrentCulture.Name.ToLower());
            if (data.ListItems != null && data.ListItems.Count() > 0)
            {
                TotalItems = data.ListItems[0].TotalRows;
            }
            data.Pagination = new Areas.Admin.Models.Partial.PartialPagination() { CurrentPage = data.SearchData.CurrentPage, ItemsPerPage = data.SearchData.ItemsPerPage, TotalItems = TotalItems, QueryString = Request.QueryString.ToString() };
            return View(data);
        }

        public IActionResult GetByCat(string alias, int id, [FromQuery] SearchArticles dto)
        {
            int TotalItems = 0;
            int IdCoQuan = 1;
            if (HttpContext.Session.GetString("IdCoQuan") != null && HttpContext.Session.GetString("IdCoQuan") != "")
            {
                IdCoQuan = int.Parse(HttpContext.Session.GetString("IdCoQuan"));
            }

            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            CategoriesArticles categories = CategoriesArticlesService.GetItem(id, API.Models.Settings.SecretId + ControllerName,0, CultureInfo.CurrentCulture.Name.ToLower());

            dto.CatId = id;
            dto.IdCoQuan = IdCoQuan;
            dto.ShowStartDate = "01/01/2010";
            dto.Status = 1;
            ArticlesModel data = new ArticlesModel() { SearchData = dto, Categories = categories };
            data.ListItems = ArticlesService.GetListPagination(data.SearchData, API.Models.Settings.SecretId + ControllerName, CultureInfo.CurrentCulture.Name.ToLower());
            if (data.ListItems != null && data.ListItems.Count() > 0)
            {
                TotalItems = data.ListItems[0].TotalRows;
            }
            data.Pagination = new Areas.Admin.Models.Partial.PartialPagination() { CurrentPage = data.SearchData.CurrentPage, ItemsPerPage = data.SearchData.ItemsPerPage, TotalItems = TotalItems, QueryString = Request.QueryString.ToString() };
            return View(data);
        }

        public IActionResult Detail(string alias,int id)
        {
            int IdCoQuan = 1;            
            if (HttpContext.Session.GetString("IdCoQuan") != null && HttpContext.Session.GetString("IdCoQuan") != "")
            {
                IdCoQuan = int.Parse(HttpContext.Session.GetString("IdCoQuan"));
            }

            ArticlesModel data = new ArticlesModel();
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            data.SearchData = new SearchArticles() { CurrentPage = 0, ItemsPerPage = 10, Keyword = "" };
            data.ListItemsDanhMuc = CategoriesArticlesService.GetListItems();
            data.Item = ArticlesService.GetItem(id, controllerSecret, CultureInfo.CurrentCulture.Name.ToLower());

            // If no article found, redirect to Error action in HomeController
            if (data.Item == null)
            {
                return RedirectToAction("Error", "Home");
            }

            // Null-safe: Fallback CatId = 0 nếu data.Item null hoặc CatId null
            int catId = data?.Item?.CatId ?? 0;
            CategoriesArticles categories = CategoriesArticlesService.GetItem(catId, "", 0, CultureInfo.CurrentCulture.Name.ToLower());

            // Null-safe assignment with coalescing
            data.Categories = categories ?? new CategoriesArticles();

            // Conditional call only if Id > 0 (avoids unnecessary service invocation)
            data.ListItems = (categories?.Id ?? 0) > 0
                ? ArticlesService.GetListRelativeNews(alias, categories.Id, IdCoQuan, CultureInfo.CurrentCulture.Name.ToLower(), id)
                : new List<Articles>();

            return View(data);
        }


        [ValidateAntiForgeryToken]
        public ActionResult GetAudio([FromQuery] string Ids, int Id)
        {
            string Version = "v2";
            DMCoQuan ItemCoQuan = JsonConvert.DeserializeObject<DMCoQuan>(HttpContext.Session.GetString("ThongTinCoQuan"));
            string Msg = "";
            int IdDC = Int32.Parse(MyModels.Decode(Ids, controllerSecret).ToString());
            Articles item = new Articles() { Id = 0 };
            if (IdDC == Id && IdDC > 0)
            {
                item = ArticlesService.GetItem(IdDC);
            }

            try
            {
                if (item.Id > 0)
                {
                    /*  ---------------- Gửi thông báo Tele -----------------------  */
                    Telegram Tele = new Telegram();
                    Telegram.Info TeleInfo = new Telegram.Info() { Msg = ItemCoQuan.Code + "-GetAudio-" + item.Id.ToString() };
                    dynamic Data = Tele.SendNotification(TeleInfo);
                    /*  ---------------- End Gửi thông báo Tele -----------------------  */

                    try
                    {
                        string Fu = API.Models.MyHelper.StringHelper.RemoveHtmlTags(item.FullText).Replace("\r", "").Trim();
                        if (item.FullText_EN != null && item.FullText_EN != "")
                        {
                            string Fu_EN = API.Models.MyHelper.StringHelper.RemoveHtmlTags(item.FullText_EN).Replace("\r", "").Trim();
                            item.FullText_EN = HttpUtility.HtmlDecode(Fu_EN);
                        }

                        item.FullText = HttpUtility.HtmlDecode(Fu);
                        // Sử dụng V2
                        if (item.FullText.Length < 1800 && item.FullText_EN.Length < 1800)
                        {
                            item.FileItem = API.Models.MyHelper.SmartVoiceService.GetFileVoice(ItemCoQuan, item);
                            if (item.FullText_EN != null && item.FullText_EN != "")
                            {
                                item.FileItem_EN = API.Models.MyHelper.SmartVoiceService.GetFileVoiceEN(ItemCoQuan, item);
                            }


                            dynamic UpdateFeatured = ArticlesService.UpdateFileAudio(item.Id, item.FileItem, item.FileItem_EN);
                            if (item.FileItem == "")
                            {
                                return Json(new MsgError() { Msg = "Đọc bài viết Lỗi", Code = Version });

                            }
                            else
                            {
                                if (item.FileItem_EN == "")
                                {
                                    return Json(new MsgSuccess() { Msg = "Đọc bài viết bài viết tiếng việt thành công", Code = Version });
                                }
                                else
                                {
                                    return Json(new MsgSuccess() { Msg = "Đọc bài viết thành công", Code = Version });
                                }

                            }
                        }
                        else
                        {
                            // sử dụng V1
                            Version = "v1";
                            if (item.FullText.Length > 10)
                            {
                                item.FileItem = API.Models.MyHelper.SmartVoiceService.GetTextId(ItemCoQuan, item);
                            }

                            if (item.FullText_EN.Length > 10)
                            {
                                item.FileItem_EN = API.Models.MyHelper.SmartVoiceService.GetTextIdEN(ItemCoQuan, item);
                            }


                            if (item.FileItem == "" && item.FileItem_EN == "")
                            {
                                return Json(new MsgError() { Msg = "Dữ liệu rỗng. Tạo file âm thanh text id bị Lỗi (" + Version + ")", Code = Version });
                            }
                            else
                            {
                                return Json(new MsgSuccess() { Msg = "Tạo file âm thanh thành công (" + Version + ")", Data = item, Code = Version });
                            }
                        }

                        
                    }
                    catch (Exception ex)
                    {
                        Msg = "Đọc bài viết Không thành công";
                        return Json(new MsgError() { Msg = Msg, Data = ItemCoQuan.Code, Code = Version });
                    }

                }
                else
                {
                    Msg = "Đọc bài viết Không thành công";
                    return Json(new MsgError() { Msg = Msg, Code = Version });
                }
            }
            catch
            {
                Msg = "Đọc bài viết không thành công";
                return Json(new MsgError() { Msg = Msg, Code = Version });
            }
        }

        [HttpPost]
        public ActionResult DownloadAudioV1([FromBody] Articles dto, [FromQuery] string Ids, int Id)
        {
            string Version = "v1";
            try
            {
                string ThongTinCoQuan = HttpContext.Session.GetString("ThongTinCoQuan");
                DMCoQuan ItemCoQuan = JsonConvert.DeserializeObject<DMCoQuan>(ThongTinCoQuan);
                int IdDC = Int32.Parse(MyModels.Decode(Ids, controllerSecret + HttpContext.Request.Headers["UserName"]).ToString());
                if (IdDC > 0)
                {
                    Articles Item = ArticlesService.GetItem(IdDC);
                    string linkAudio = API.Models.MyHelper.SmartVoiceService.DownloadFileByTextId(ItemCoQuan, Item, dto.FileItem);
                    string linkAudioEN = "";
                    if (dto.FileItem_EN == null || dto.FileItem_EN.Trim() == "")
                    {

                    }
                    else
                    {
                        linkAudioEN = API.Models.MyHelper.SmartVoiceService.DownloadFileByTextIdEN(ItemCoQuan, Item, dto.FileItem_EN);
                    }


                    dynamic UpdateFeatured = ArticlesService.UpdateFileAudio(IdDC, linkAudio, linkAudioEN);

                    if (linkAudio == "" && linkAudioEN == "")
                    {
                        return Json(new MsgError() { Msg = "Tạo file âm thanh Lỗi (" + Version + ")" });

                    }
                    else
                    {
                        if (linkAudioEN == "")
                        {
                            return Json(new MsgSuccess() { Msg = "Tạo file âm thanh bài viết tiếng việt thành công", Data = Item });
                        }
                        else
                        {
                            return Json(new MsgSuccess() { Msg = "Tạo file âm thanh thành công (" + Version + ")", Data = Item });
                        }
                    }
                }
                else
                {
                    return Json(new MsgError() { Msg = "Tạo file không thành công (" + Version + ")" });
                }


            }
            catch (Exception e)
            {
                //TempData["MessageSuccess"] = "";
                return Json(new MsgError() { Msg = "Tạo file không thành công (" + Version + ")" + e.Message });
            }

        }
        [ValidateAntiForgeryToken]
        public ActionResult LikeArticle([FromQuery] string Ids, int Id)
        {
            //API.Models.MyHelper.Google Google = new Models.MyHelper.Google();
            // Boolean CheckGoogle = Google.CheckGoogle(Configuration["RecaptchaSettings:SecretKey"], model.Token, int.Parse(Configuration["flagDevCoQuan"].ToString()));
            DMCoQuan ItemCoQuan = JsonConvert.DeserializeObject<DMCoQuan>(HttpContext.Session.GetString("ThongTinCoQuan"));
            string Msg = "";
            Articles item = new Articles() { Id = Int32.Parse(MyModels.Decode(Ids, controllerSecret).ToString()) };
            try
            {
                if (item.Id > 0)
                {
					/*  ---------------- Gửi thông báo Tele -----------------------  */
					Telegram Tele = new Telegram();
					Telegram.Info TeleInfo = new Telegram.Info() { Msg = ItemCoQuan.Code + "-ArtLike-" + item.Id.ToString() };
					dynamic Data = Tele.SendNotification(TeleInfo);
					/*  ---------------- End Gửi thông báo Tele -----------------------  */

					dynamic UpdateFeatured = ArticlesService.UpdateLike(item.Id);
                    Msg = "Cập nhật Like thành công";
                    return Json(new MsgSuccess() { Msg = Msg,Data = ItemCoQuan.Code });
                }
                else
                {
                    Msg = "Cập nhật Like Không thành công";
                    return Json(new MsgError() { Msg = Msg });
                }
            }
            catch
            {
                Msg = "Cập nhật Like không thành công";
                return Json(new MsgError() { Msg = Msg });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult GetListComment([FromBody] SearchArticleComment dto)
        {
            int IdCoQuan = 1;
            if (HttpContext.Session.GetString("IdCoQuan") != null && HttpContext.Session.GetString("IdCoQuan") != "")
            {
                IdCoQuan = int.Parse(HttpContext.Session.GetString("IdCoQuan"));
            }

            ArticleCommentModel data = new ArticleCommentModel() { };
            int IdDC = Int32.Parse(MyModels.Decode(dto.ArticleIds, controllerSecret + HttpContext.Request.Headers["UserName"]).ToString());
            dto.Status = 1;
            dto.IdCoQuan = IdCoQuan;
            List<ArticleComment> ListItems = ArticleCommentService.GetListPagination(dto, controllerSecret + HttpContext.Request.Headers["UserName"]);
            return Json(new MsgSuccess() { Data = ListItems });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SendComment([FromBody] ArticleComment dto)
        {
            DMCoQuan ItemCoQuan = JsonConvert.DeserializeObject<DMCoQuan>(HttpContext.Session.GetString("ThongTinCoQuan"));
            int IdCoQuan = 1;
            Boolean flagSave = true;
            String msg = "Vui lòng nhập lại thông tin";
            if (HttpContext.Session.GetString("IdCoQuan") != null && HttpContext.Session.GetString("IdCoQuan") != "")
            {
                IdCoQuan = int.Parse(HttpContext.Session.GetString("IdCoQuan"));
            }

            if(dto.Captcha==null || dto.Captcha == "")
            {
                msg = "Mã captcha không được để trống";
                flagSave = false;
            }
            else
            {
                if (HttpContext.Session.GetString("CaptChaCode_ArticlesComment") != dto.Captcha)
                {
                    msg = "Mã captcha không hợp lệ. Vui lòng nhập lại mã Captcha";
                    flagSave = false;
                }
            }

            if (!flagSave)
            {
                return Json(new MsgError() { Data = dto, Msg = msg  });
            }
            else
            {
               

                ArticleCommentModel data = new ArticleCommentModel() { };
                int IdDC = Int32.Parse(MyModels.Decode(dto.ArticleIds, controllerSecret).ToString());
                Articles Item = new Articles();
                if (IdDC > 0 && IdDC == dto.ArticleId)
                {
                    Item = ArticlesService.GetItem(IdDC, controllerSecret);

                    if (Item != null)
                    {
                        dto.IdCoQuan = IdCoQuan;
                        ArticleCommentService.SaveItem(dto);
						/*  ---------------- Gửi thông báo Tele -----------------------  */
						Telegram Tele = new Telegram();
						Telegram.Info TeleInfo = new Telegram.Info() { Msg = ItemCoQuan.Code + "-ArtComment-" + Item.Id.ToString() + "- <b>" + Item.Title + "</b>" };
						dynamic Data = Tele.SendNotification(TeleInfo);
						/*  ---------------- End Gửi thông báo Tele -----------------------  */
						return Json(new MsgSuccess() { Data = Item });
                    }
                }

                return Json(new MsgError() { Data = Item });
            }
            
        }
    }
}