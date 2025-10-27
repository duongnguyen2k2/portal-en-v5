using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using API.Areas.Admin.Models.Articles;
using API.Areas.Admin.Models.CategoriesArticles;
using API.Areas.Admin.Models.DMCoQuan;
using API.Areas.Admin.Models.USUsers;
using API.Models;
using API.Models.Utilities;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace API.Areas.Admin.Controllers
{
    [Area("Admin")]
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
        public IActionResult ViewLog(string Id = null)
        {
            
            ArticlesModel data = new ArticlesModel() { };
            int IdDC = Int32.Parse(MyModels.Decode(Id, controllerSecret+ HttpContext.Request.Headers["UserName"]).ToString());
            data.ListItems = ArticlesService.GetListLogArticles(IdDC, controllerSecret+ HttpContext.Request.Headers["UserName"]);
            return View(data);
        }

        [HttpPost]
        public async Task<dynamic> TaoFileBaoCao([FromBody] SearchArticles dto)
        {
            try
            {
                dto.ItemsPerPage = 1000;
                string ThongTinCoQuan = HttpContext.Session.GetString("ThongTinCoQuan");
                DMCoQuan ItemCoQuan = JsonConvert.DeserializeObject<DMCoQuan>(ThongTinCoQuan);
                dto.IdCoQuan = int.Parse(HttpContext.Request.Headers["IdCoQuan"]);
                ArticlesModel data = new ArticlesModel() { SearchData = dto };                
                return new API.Models.MsgSuccess() { Data = await ArticlesService.TaoFileBaoCao(data.SearchData,false, ItemCoQuan) };
            }
            catch (Exception e)
            {
                return new API.Models.MsgError() { Msg = e.Message };
            }
        }

        public IActionResult Index([FromQuery] SearchArticles dto)
        {
            int TotalItems = 0;
            
            dto.IdCoQuan = int.Parse(HttpContext.Request.Headers["IdCoQuan"]);
            int IdGroup = Int32.Parse(HttpContext.Session.GetString("IdGroup"));
            int ParentIdCat = 0;
            if (IdGroup == 6)
            {
                ParentIdCat = Int32.Parse(Configuration["IdCatCompat"]);
            }
            ArticlesModel data = new ArticlesModel() { SearchData = dto };
            data.ListItems = ArticlesService.GetListPagination(data.SearchData, controllerSecret+ HttpContext.Request.Headers["UserName"]);
            data.ListItemsDanhMuc = CategoriesArticlesService.GetListItems(true, ParentIdCat, "all", 1);
            data.ListItemsAuthors = API.Areas.Admin.Models.USUsers.USUsersService.GetListItemsAuthor(4, dto.IdCoQuan);
            data.ListItemsCreatedBy = API.Areas.Admin.Models.USUsers.USUsersService.GetListItemsAuthor(3, dto.IdCoQuan);
            data.ListItemsStatus = ArticlesService.GetListItemsStatus();
            
            if (data.ListItems != null && data.ListItems.Count() > 0)
            {
                TotalItems = data.ListItems[0].TotalRows;
            }
            
            HttpContext.Session.SetString("STR_Action_Link_" + controllerName, Request.QueryString.ToString());
            data.Pagination = new Models.Partial.PartialPagination() { CurrentPage = data.SearchData.CurrentPage, ItemsPerPage = data.SearchData.ItemsPerPage, TotalItems = TotalItems, QueryString = Request.QueryString.ToString() };
            
            return View(data);
        }
        public IActionResult IndexByStatus([FromQuery] SearchArticles dto)
        {
            int TotalItems = 0;
            
            dto.IdCoQuan = int.Parse(HttpContext.Request.Headers["IdCoQuan"]);
            int IdGroup = Int32.Parse(HttpContext.Session.GetString("IdGroup"));

            dto.IdCoQuan = int.Parse(HttpContext.Request.Headers["IdCoQuan"]);
            dto.CreatedBy = int.Parse(HttpContext.Request.Headers["Id"]);
            dto.Status = 2;
            

            int ParentIdCat = 0;
            if (IdGroup == 6)
            {
                ParentIdCat = Int32.Parse(Configuration["IdCatCompat"]);
            }
            ArticlesModel data = new ArticlesModel() { 
                SearchData = dto,
                Item = new Articles() { Title = "article_tmp", ArticlesStatusId = 0
                , Status = true,Featured = true, FeaturedHome = true
                , CatId = 0, PublishUpShow = DateTime.Now.ToString("dd/MM/yyyy") , Deleted = false
                }
            };
            data.ListItems = ArticlesService.GetListPagination(data.SearchData, controllerSecret+ HttpContext.Request.Headers["UserName"]);
            data.ListItemsDanhMuc = CategoriesArticlesService.GetListItems(true, ParentIdCat);
            data.ListItemsAuthors = API.Areas.Admin.Models.USUsers.USUsersService.GetListItemsAuthor(4, dto.IdCoQuan);
            data.ListItemsCreatedBy = API.Areas.Admin.Models.USUsers.USUsersService.GetListItemsAuthor(3, dto.IdCoQuan);
            data.ListItemsStatus = ArticlesService.GetListItemsStatus();
            
            if (data.ListItems != null && data.ListItems.Count() > 0)
            {
                TotalItems = data.ListItems[0].TotalRows;
            }
            
            HttpContext.Session.SetString("STR_Action_Link_" + controllerName, Request.QueryString.ToString());
            data.Pagination = new Models.Partial.PartialPagination() { CurrentPage = data.SearchData.CurrentPage, ItemsPerPage = data.SearchData.ItemsPerPage, TotalItems = TotalItems, QueryString = Request.QueryString.ToString() };
            
            return View(data);
        }

        public IActionResult GetItemLogArticle(string Id = null)
        {
            Articles Item = new Articles();
            
            int IdDC = Int32.Parse(MyModels.Decode(Id, controllerSecret+ HttpContext.Request.Headers["UserName"]).ToString());
            if (IdDC > 0)
            {
                Item = ArticlesService.GetItemLogArticle(IdDC, controllerSecret+ HttpContext.Request.Headers["UserName"]);
            }
            return Json(Item);
        }

        public IActionResult GetItem(string Id = null)
        {
            Articles Item = new Articles();
            
            int IdDC = Int32.Parse(MyModels.Decode(Id, controllerSecret+ HttpContext.Request.Headers["UserName"]).ToString());
            if (IdDC > 0)
            {
                Item = ArticlesService.GetItem(IdDC, controllerSecret+ HttpContext.Request.Headers["UserName"]);
            }
            return Json(Item);
        }

        public IActionResult ArticlesTransfer(string Id = null)
        {
            Articles Item = new Articles();
            
            Articles ItemTransfer = new Articles();
            
            int IdDC = Int32.Parse(MyModels.Decode(Id, controllerSecret+ HttpContext.Request.Headers["UserName"]).ToString());
            if (IdDC > 0)
            {
                Item = ArticlesService.GetItem(IdDC, controllerSecret+ HttpContext.Request.Headers["UserName"]);
                if (Item != null && Item.Id > 0) {
                    ItemTransfer = ArticlesService.GetItem(Item.RootNewsId, controllerSecret+ HttpContext.Request.Headers["UserName"]);
                }
            }
            return Json(ItemTransfer);
        }
        [HttpPost]
        public IActionResult UpdateArticlesTransfer([FromQuery] string Id = null, int RootNewsId = 0)
        {
            
            Articles Item = new Articles();
            
            
            int IdDC = Int32.Parse(MyModels.Decode(Id, controllerSecret+ HttpContext.Request.Headers["UserName"]).ToString());
            if (IdDC > 0)
            {
                Articles ItemTransfer = ArticlesService.GetItem(RootNewsId);
                if (ItemTransfer != null && ItemTransfer.Id > 0)
                {
                    try
                    {
                        var res = ArticlesService.UpdateArticlesTransfer(IdDC,ItemTransfer);
                        return Json(new API.Models.MsgSuccess() { Msg = "Cập nhật tin liên thông Thành Công", Data = res.Rows[0][0] });
                    }
                    catch (Exception e)
                    {
                        return Json(new API.Models.MsgError() { Msg = e.Message });
                    }
                }
                else {
                    return Json(new API.Models.MsgError() { Msg = "Tin liên thông nguồn không hợp lệ" });
                }
                
            }
            return Json(new MsgSuccess() { Data = 0 , Msg = "Bạn không có quyền cập nhật tin liên thông"});
        }

        public IActionResult SaveItem(string Id = null)
        {
            int IdCoQuan = int.Parse(HttpContext.Request.Headers["IdCoQuan"]);
            ArticlesModel data = new ArticlesModel();
            int IdGroup = Int32.Parse(HttpContext.Session.GetString("IdGroup"));
            int ParentIdCat = 0;
            if (IdGroup == 6)
            {
                ParentIdCat = Int32.Parse(Configuration["IdCatCompat"]);
            }

            int IdDC = Int32.Parse(MyModels.Decode(Id, controllerSecret+ HttpContext.Request.Headers["UserName"]).ToString());
            data.SearchData = new SearchArticles() { CurrentPage = 0, ItemsPerPage = 10, Keyword = "" };

            data.ListItemsDanhMuc = CategoriesArticlesService.GetListItems(true,ParentIdCat);
            data.ListItemsAuthors = API.Areas.Admin.Models.USUsers.USUsersService.GetListItemsAuthor(4, IdCoQuan);
            data.ListCategoryType = ArticlesService.GetListCategoryType();
            data.ListLevelArticles = ArticlesService.GetListLevelArticle();

            Articles Item = new Articles() { PublishUp = DateTime.Now, PublishUpShow = DateTime.Now.ToString("dd/MM/yyyy") };
            if (IdDC > 0)
            {
                Item = ArticlesService.GetItem(IdDC, controllerSecret+ HttpContext.Request.Headers["UserName"]);
            }
            data.Item = Item;            
            return View(data);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveItem(Articles data)
        {
            string ThongTinCoQuan = HttpContext.Session.GetString("ThongTinCoQuan");
            DMCoQuan ItemCoQuan = JsonConvert.DeserializeObject<DMCoQuan>(ThongTinCoQuan);
            int IdCoQuan = int.Parse(HttpContext.Request.Headers["IdCoQuan"]);
            var Login = HttpContext.Session.GetString("Login");
            USUsers MyInfo = JsonConvert.DeserializeObject<USUsers>(Login);
            int IdGroup = Int32.Parse(HttpContext.Session.GetString("IdGroup"));
            int ParentIdCat = 0;
            if (IdGroup == 6)
            {
                ParentIdCat = Int32.Parse(Configuration["IdCatCompat"]);
            }
            ArticlesModel model = new ArticlesModel() { Item = data };
            
            int IdDC = Int32.Parse(MyModels.Decode(model.Item.Ids, controllerSecret+ HttpContext.Request.Headers["UserName"]).ToString());

            if (ModelState.IsValid)
            {
                if (data.Alias == null || data.Alias == "")
                {
                    model.Item.Alias = API.Models.MyHelper.StringHelper.UrlFriendly(data.Title);
                }
              
                if (model.Item.Id == IdDC)
                {
                    model.Item.CreatedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    model.Item.ModifiedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    model.Item.IdCoQuan = int.Parse(HttpContext.Request.Headers["IdCoQuan"]);

                    if (MyInfo.IdGroup == 3)
                    {
                        if (ItemCoQuan.MetadataCV.FlagStatusArticle == 0)
                        {
                            if (IdDC > 0)
                            {
                                Articles ItemRoot = ArticlesService.GetItem(IdDC, controllerSecret + HttpContext.Request.Headers["UserName"]);
                                if (ItemRoot.Status == false)
                                {
                                    model.Item.Status = false;
                                }
                            }
                            else
                            {
                                model.Item.Status = false;
                            }
                        }
                        
                       
                    }
                    try
                    {
                        ArticlesService.SaveItem(model.Item);
                        TempData["MessageSuccess"] = "Cập nhật thành công";
                        string Str_Url = HttpContext.Session.GetString("STR_Action_Link_" + controllerName);
                        if (data.ArticlesStatusId == 0)
                        {
                            
                            return Redirect("/Admin/Articles/IndexByStatus");
                        }
                        else
                        {
                            if (Str_Url != null && Str_Url != "")
                            {
                                return Redirect("/Admin/Articles/Index" + Str_Url);
                            }
                            else
                            {
                                return RedirectToAction("Index");
                            }
                        }
                        
                        
                    }
                    catch
                    {

                    }

                }
            }
            model.SearchData = new SearchArticles() { CurrentPage = 0, ItemsPerPage = 10, Keyword = "" };
            model.ListItemsDanhMuc = CategoriesArticlesService.GetListItems(true,ParentIdCat);
            model.ListItemsAuthors = API.Areas.Admin.Models.USUsers.USUsersService.GetListItemsAuthor(4, IdCoQuan);
            model.ListCategoryType = ArticlesService.GetListCategoryType();
            model.ListLevelArticles = ArticlesService.GetListLevelArticle();
            return View(model);
        }

        [ValidateAntiForgeryToken]
        public ActionResult DeleteItem(string Id)
        {
            
            Articles model = new Articles() { Id = Int32.Parse(MyModels.Decode(Id, controllerSecret+ HttpContext.Request.Headers["UserName"]).ToString()) };
            try
            {
                if (model.Id > 0)
                {
                    model.CreatedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    model.ModifiedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    ArticlesService.DeleteItem(model);
                    TempData["MessageSuccess"] = "Xóa thành công";
                    return Json(new MsgSuccess());
                }
                else
                {
                    TempData["MessageError"] = "Xóa Không thành công";
                    return Json(new MsgError());
                }

            }
            catch
            {
                TempData["MessageSuccess"] = "Xóa không thành công";
                return Json(new MsgError());
            }


        }

        [ValidateAntiForgeryToken]
        public ActionResult UpdateStatus([FromQuery] string Ids, Boolean Status)
        {
            string ThongTinCoQuan = HttpContext.Session.GetString("ThongTinCoQuan");
            DMCoQuan ItemCoQuan = JsonConvert.DeserializeObject<DMCoQuan>(ThongTinCoQuan);
            Articles item = new Articles() { Id = Int32.Parse(MyModels.Decode(Ids, controllerSecret+ HttpContext.Request.Headers["UserName"]).ToString()), Status = Status };
            try
            {
                if (item.Id > 0)
                {
                    item.CreatedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    item.ModifiedBy = int.Parse(HttpContext.Request.Headers["Id"]);

                    var Login = HttpContext.Session.GetString("Login");
                    USUsers MyInfo = JsonConvert.DeserializeObject<USUsers>(Login);

                    if (MyInfo.IdGroup == 3)
                    {
                        if (ItemCoQuan.MetadataCV.FlagStatusArticle == 0)
                        {
                            Articles ItemRoot = ArticlesService.GetItem(item.Id, controllerSecret + HttpContext.Request.Headers["UserName"]);
                            if (ItemRoot.Status == false)
                            {
                                item.Status = false;
                            }
                        }
                       
                    }

                    dynamic UpdateStatus = ArticlesService.UpdateStatus(item);
                    if (item.Status == true)
                    {
                        TempData["MessageSuccess"] = "Cập nhật Trạng Thái duyệt bài thành công";
                    }
                    else {
                        TempData["MessageSuccess"] = "Cập nhật Trạng Thái Hủy duyệt bài thành công";
                    }
                    
                    return Json(new MsgSuccess());
                }
                else
                {
                    TempData["MessageError"] = "Cập nhật Trạng Thái duyệt bài Không thành công";
                    return Json(new MsgError());
                }
            }
            catch
            {
                TempData["MessageSuccess"] = "Cập nhật Trạng Thái không thành công";
                return Json(new MsgError());
            }
        }

        [ValidateAntiForgeryToken]
        public ActionResult UpdateStaticPage([FromQuery] string Ids, Boolean StaticPage)
        {
            
            Articles item = new Articles() { Id = Int32.Parse(MyModels.Decode(Ids, controllerSecret+ HttpContext.Request.Headers["UserName"]).ToString()), StaticPage = StaticPage };
            try
            {
                if (item.Id > 0)
                {
                    item.CreatedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    item.ModifiedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    dynamic UpdateStaticPage = ArticlesService.UpdateStaticPage(item);
                    TempData["MessageSuccess"] = "Cập nhật StaticPage thành công";
                    return Json(new MsgSuccess());
                }
                else
                {
                    TempData["MessageError"] = "Cập nhật StaticPage Không thành công";
                    return Json(new MsgError());
                }
            }
            catch
            {
                TempData["MessageSuccess"] = "Cập nhật StaticPage không thành công";
                return Json(new MsgError());
            }
        }
        [ValidateAntiForgeryToken]
        public ActionResult UpdateFeatured([FromQuery] string Ids, Boolean Featured)
        {
            
            Articles item = new Articles() { Id = Int32.Parse(MyModels.Decode(Ids, controllerSecret+ HttpContext.Request.Headers["UserName"]).ToString()), Featured = Featured };
            try
            {
                if (item.Id > 0)
                {
                    item.CreatedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    item.ModifiedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    dynamic UpdateFeatured = ArticlesService.UpdateFeatured(item);
                    TempData["MessageSuccess"] = "Cập nhật tin Nổi bật thành công";
                    return Json(new MsgSuccess());
                }
                else
                {
                    TempData["MessageError"] = "Cập nhật tin Nổi bật Không thành công";
                    return Json(new MsgError());
                }
            }
            catch
            {
                TempData["MessageSuccess"] = "Cập nhật tin Nổi bật không thành công";
                return Json(new MsgError());
            }
        }

        [ValidateAntiForgeryToken]
        public ActionResult UpdateFeaturedHome([FromQuery] string Ids, Boolean FeaturedHome)
        {
            
            Articles item = new Articles() { Id = Int32.Parse(MyModels.Decode(Ids, controllerSecret+ HttpContext.Request.Headers["UserName"]).ToString()), FeaturedHome = FeaturedHome };
            try
            {
                if (item.Id > 0)
                {
                    item.CreatedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    item.ModifiedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    dynamic UpdateFeatured = ArticlesService.UpdateFeaturedHome(item);
                    TempData["MessageSuccess"] = "Cập nhật tin tức Thông báo thành công";
                    return Json(new MsgSuccess());
                }
                else
                {
                    TempData["MessageError"] = "Cập nhật tin tức Thông báo Không thành công";
                    return Json(new MsgError());
                }
            }
            catch
            {
                TempData["MessageSuccess"] = "Cập nhật tin tức Thông báo thành công";
                return Json(new MsgError());
            }
        }

        //[ValidateAntiForgeryToken]
        public ActionResult CreateAudio([FromQuery] string Ids, int Id)
        {
            string Version = "v2";
            int IdDC = Int32.Parse(MyModels.Decode(Ids, controllerSecret + HttpContext.Request.Headers["UserName"]).ToString());
            string ThongTinCoQuan = HttpContext.Session.GetString("ThongTinCoQuan");
            DMCoQuan ItemCoQuan = JsonConvert.DeserializeObject<DMCoQuan>(ThongTinCoQuan);
            Articles Item = ArticlesService.GetItem(IdDC);
            try
            {
                if (Item.Id > 0)
                {
                    string Fu = API.Models.MyHelper.StringHelper.RemoveHtmlTags(Item.FullText).Replace("\r", "").Trim();
                    if(Item.FullText_EN!=null && Item.FullText_EN != "")
                    {
                        string Fu_EN = API.Models.MyHelper.StringHelper.RemoveHtmlTags(Item.FullText_EN).Replace("\r", "").Trim();
                        Item.FullText_EN = HttpUtility.HtmlDecode(Fu_EN);
                    }
                    else
                    {
                        Item.FullText_EN = "";
                    }
                    
                    Item.FullText = HttpUtility.HtmlDecode(Fu);
                    
                    if(Item.FullText.Length<1800 && Item.FullText_EN.Length < 1800)
                    {
                        Item.FileItem = API.Models.MyHelper.SmartVoiceService.GetFileVoice(ItemCoQuan, Item);
                        Item.FileItem_EN = API.Models.MyHelper.SmartVoiceService.GetFileVoiceEN(ItemCoQuan, Item);

                        dynamic UpdateFeatured = ArticlesService.UpdateFileAudio(Item.Id, Item.FileItem, Item.FileItem_EN);
                        if (Item.FileItem == "" && Item.FileItem_EN == "")
                        {
                            return Json(new MsgError() { Msg = "Tạo file âm thanh Lỗi(" + Version + ")" });

                        }
                        else
                        {
                            if (Item.FileItem_EN == "")
                            {
                                return Json(new MsgSuccess() { Msg = "Tạo file âm thanh bài viết tiếng việt thành công ("+ Version + ")", Code = Version });
                            }
                            else
                            {
                                return Json(new MsgSuccess() { Msg = "Tạo file âm thanh thành công (" + Version + ")", Code = Version });
                            }

                        }
                    }
                    else
                    {
                        Version = "v1";
                        if (Item.FullText.Length > 10)
                        {
                            Item.FileItem = API.Models.MyHelper.SmartVoiceService.GetTextId(ItemCoQuan, Item);
                        }

                        if (Item.FullText_EN.Length > 10)
                        {
                            Item.FileItem_EN = API.Models.MyHelper.SmartVoiceService.GetTextIdEN(ItemCoQuan, Item);
                        }
                            

                        if (Item.FileItem == "" && Item.FileItem_EN == "")
                        {
                            return Json(new MsgError() { Msg = "Dữ liệu rỗng. Tạo file âm thanh text id bị Lỗi (" + Version + ")", Code = Version });
                        }
                        else
                        {
                            return Json(new MsgSuccess() { Msg = "Tạo file âm thanh thành công (" + Version + ")", Data = Item, Code = Version });
                        }
                                 
                        
                    }
                     
                }
                else
                {                    
                    return Json(new MsgError() { Msg = "Cập nhật file âm thanh thất bại. Xin vui lòng tạo lại(" + Version + ")", Code = Version });
                }
            }
            catch(Exception e)
            {                
                return Json(new MsgError() { Msg = "Tạo file không thành công (" + Version + ")" + e.Message, Code = Version });
            }
        }
        [HttpPost]
        public ActionResult DownloadAudioV1([FromBody] Articles dto,[FromQuery] string Ids, int Id)
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
                    if (dto.FileItem_EN==null || dto.FileItem_EN.Trim() == "")
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
        public ActionResult ChangeArticlesStatusId(string Id)
        {
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString() + "_" + HttpContext.Request.Headers["UserName"];

            int IdDC = Int32.Parse(MyModels.Decode(Id, controllerSecret + HttpContext.Request.Headers["UserName"]).ToString());

            Articles model = new Articles() { Id = IdDC };
            try
            {
                if (model.Id > 0)
                {
                    model.CreatedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    model.ModifiedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    ArticlesService.ChangeArticlesStatusId(model);
                    TempData["MessageSuccess"] = "Chuyển bài viết " + model.Title + " sang chính thức thành công";
                    return Json(new MsgSuccess());
                }
                else
                {
                    TempData["MessageError"] = "Chuyển bài viết Không thành công";
                    return Json(new MsgError());
                }
            }
            catch
            {
                TempData["MessageSuccess"] = "Chuyển bài viết không thành công";
                return Json(new MsgError());
            }


        }

    }
}
