using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using API.Areas.Admin.Models.Articles;
using API.Models;
using API.Areas.Admin.Models.CategoriesArticles;
using API.Areas.Admin.Models.DMCoQuan;
using API.Models.Utilities;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;
using DocumentFormat.OpenXml.Drawing.Charts;
using HtmlAgilityPack;
using Microsoft.Extensions.Configuration;
using System.Web;
using HtmlAgilityPack.CssSelectors.NetCore;

namespace API.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ArticlesTransferController : Controller
    {
        public IActionResult IndexNam([FromQuery] SearchArticles dto)
        {
            return View();
        }

        public IActionResult Index([FromQuery] SearchArticles dto)
        {
            int IdCoQuan = int.Parse(HttpContext.Request.Headers["IdCoQuan"]);
            int TotalItems = 0;
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString() + "_" + HttpContext.Request.Headers["UserName"];
            ArticlesModel data = new ArticlesModel() { SearchData = dto };
            data.ListItems = ArticlesService.GetListTransferPagination(data.SearchData, API.Models.Settings.SecretId + ControllerName);
            data.ListItemsDanhMuc = CategoriesArticlesService.GetListItems();
            data.ListItemsAuthors = API.Areas.Admin.Models.USUsers.USUsersService.GetListItemsAuthor(4, IdCoQuan);
            data.ListItemsCreatedBy = API.Areas.Admin.Models.USUsers.USUsersService.GetListItemsAuthor(3,IdCoQuan);
            data.ListItemsStatus = ArticlesService.GetListItemsStatus();
            int ParentId = int.Parse(HttpContext.Session.GetString("ParentId"));
            data.ListDMCoQuan = DMCoQuanService.GetListByLoaiCoQuan(-1, 0, ParentId);
            if (data.ListItems != null && data.ListItems.Count() > 0)
            {
                TotalItems = data.ListItems[0].TotalRows;
            }

            HttpContext.Session.SetString("STR_Action_Link_" + ControllerName, Request.QueryString.ToString());
            data.Pagination = new Models.Partial.PartialPagination() { CurrentPage = data.SearchData.CurrentPage, ItemsPerPage = data.SearchData.ItemsPerPage, TotalItems = TotalItems, QueryString = Request.QueryString.ToString() };

            return View(data);
        }

        public IActionResult GetItem(string Id = null)
        {
            Articles Item = new Articles();
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString() + "_" + HttpContext.Request.Headers["UserName"];
            int IdDC = Int32.Parse(MyModels.Decode(Id, API.Models.Settings.SecretId + ControllerName).ToString());
            if (IdDC > 0)
            {
                Item = ArticlesService.GetItem(IdDC, API.Models.Settings.SecretId + ControllerName);
            }
            return Json(Item);
        }

        public IActionResult InsertArticlesTransfer(string Id = null)
        {
            Articles Item = new Articles();
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString() + "_" + HttpContext.Request.Headers["UserName"];
            int IdDC = Int32.Parse(MyModels.Decode(Id, API.Models.Settings.SecretId + ControllerName).ToString());

            try
            {
                var res = ArticlesService.InsertArticlesTransfer(IdDC, Int32.Parse(HttpContext.Request.Headers["IdCoQuan"]));
                var a = res.Rows[0][0];
                return Json(new API.Models.MsgSuccess() { Msg = "Lấy dữ liệu tin tức Thành Công",Data = res.Rows[0][0] });
            }
            catch(Exception e) {
                return Json(new API.Models.MsgError() { Msg = e.Message });
            }
        }

        private string controllerName = "ArticlesController";
        private string controllerSecret;
        private IConfiguration Configuration;
        public ArticlesTransferController(IConfiguration config)
        {
            controllerSecret = config["Security:SecretId"] + controllerName;
            Configuration = config;
        }

        public IActionResult IndexNew()
        {
            return View();
        }

        [HttpPost]
        public IActionResult IndexNew([FromBody] SearchArticles dto)
        {
            
            ArticlesModel data = new ArticlesModel() { SearchData = dto };
            data.ListItems = ArticlesService.GetListPaginationSTP(data.SearchData, controllerSecret + HttpContext.Request.Headers["UserName"]);
            
            return Json(new API.Models.MsgSuccess() { Data = data });
        }

        public ActionResult GetDetailTinBaiLienThong(int Id)
        {
            Articles ItemFromSTP = ArticlesService.GetItemSTP(Id);
            ItemFromSTP.FullText = ArticlesService.ConvertTinBai(ItemFromSTP.FullText, Configuration["LTTT:RootHost"]);
            return Json(new MsgSuccess() { Data = ItemFromSTP });
        }
        [HttpPost]
        public ActionResult LayTinBai([FromBody] Articles dto)
        {
            int Id = dto.Id;
            int IdCat = dto.CatId;
            string host = Configuration["LTTT:RootHost"];
            Articles ItemFromSTP = ArticlesService.GetItemSTP(Id);
            Articles ItemSaveToDB = ItemFromSTP;
            ItemSaveToDB.CatId = IdCat;
            ItemSaveToDB.IdCoQuan = int.Parse(HttpContext.Request.Headers["IdCoQuan"]);
            ItemSaveToDB.Images = host + ItemFromSTP.Images;
            ItemSaveToDB.RootNewsId = Id;
            ItemSaveToDB.RootNewsFlag = true;

            try
            {
                if (ItemFromSTP != null)
                {
                    HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();

                    if (ItemFromSTP.FullText != null)
                    {
                        htmlDoc.LoadHtml(ItemFromSTP.FullText);
                        IList<HtmlNode> nodesImg = htmlDoc.QuerySelectorAll("img");
                        IList<HtmlNode> nodesA = htmlDoc.QuerySelectorAll("a");
                        if (nodesImg.Count() > 0)
                        {

                            for (int imgI = 0; imgI < nodesImg.Count(); imgI++)
                            {
                                if (nodesImg[imgI].QuerySelector("img") != null)
                                {
                                    string imgRoot = "";
                                    if (nodesImg[imgI].Attributes["src"].Value.ToString().Contains("https://") || nodesImg[imgI].Attributes["src"].Value.ToString().Contains("http://"))
                                    {
                                        imgRoot = HttpUtility.UrlDecode(nodesImg[imgI].Attributes["src"].Value.ToString());
                                    }
                                    else if (nodesImg[imgI].Attributes["src"].Value.ToString().Contains("data:image"))
                                    {
                                        imgRoot = HttpUtility.UrlDecode(nodesImg[imgI].Attributes["src"].Value.ToString());
                                    }
                                    else
                                    {
                                        imgRoot = host + HttpUtility.UrlDecode(nodesImg[imgI].Attributes["src"].Value.ToString());
                                    }
                                    htmlDoc.QuerySelectorAll("img")[imgI].Attributes["src"].Value = imgRoot;
                                }
                            }
                        }

                        if (nodesA.Count() > 0)
                        {
                            for (int nai = 0; nai < nodesA.Count(); nai++)
                            {
                                if (nodesA[nai].Attributes["href"] != null)
                                {
                                    string imgRoot = "";
                                    if (nodesA[nai].Attributes["src"].Value.ToString().Contains("https://") || nodesA[nai].Attributes["src"].Value.ToString().Contains("http://"))
                                    {
                                        imgRoot = HttpUtility.UrlDecode(nodesA[nai].Attributes["src"].Value.ToString());
                                    }
                                    else
                                    {
                                        imgRoot = host + HttpUtility.UrlDecode(nodesA[nai].Attributes["href"].Value.ToString());
                                    }
                                    htmlDoc.QuerySelectorAll("a")[nai].Attributes["href"].Value = imgRoot;
                                }
                            }
                        }
                        ItemSaveToDB.FullText = htmlDoc.DocumentNode.OuterHtml;
                    }
                }
                ItemSaveToDB.Id = 0;
                ArticlesService.SaveItem(ItemSaveToDB);
                return Json(new MsgSuccess() { Msg = "Lấy bài viết liên thông thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new MsgError() { Msg = "Lấy bài viết liên thông thất bại! Lý do: " + ex });
                
            }
        }
    }
}