using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using API.Areas.Admin.Models.CategoriesArticles;
using API.Models;
using API.Models.Utilities;
using Microsoft.Extensions.Configuration;
using API.Areas.Admin.Models.CategoriesLanguage;

namespace API.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoriesArticlesController : Controller
    {
        private string controllerName = "CategoriesArticlesController";
        private string controllerSecret;
        public CategoriesArticlesController(IConfiguration config)
        {
            controllerSecret = config["Security:SecretId"] + controllerName;
        }

        public IActionResult Index([FromQuery] SearchCategoriesArticles dto)
        {
            int TotalItems = 0;
            
            dto.IdCoQuan = int.Parse(HttpContext.Request.Headers["IdCoQuan"]);

            CategoriesArticlesModel data = new CategoriesArticlesModel() { SearchData = dto};
            data.ListItems = CategoriesArticlesService.GetListPagination(data.SearchData, controllerSecret + HttpContext.Request.Headers["UserName"]);            
            if (data.ListItems != null && data.ListItems.Count() > 0)
            {
                TotalItems = data.ListItems[0].TotalRows;
            }
            data.Pagination = new Models.Partial.PartialPagination() { CurrentPage = data.SearchData.CurrentPage, ItemsPerPage = data.SearchData.ItemsPerPage, TotalItems = TotalItems, QueryString = Request.QueryString.ToString() };

            return View(data);
        }

        public IActionResult SaveItem([FromQuery] string Culture = "vi", string Id = null)
        {
            int IdCoQuan =  int.Parse(HttpContext.Request.Headers["IdCoQuan"]);
            CategoriesArticlesModel data = new CategoriesArticlesModel();
            
            int IdDC = Int32.Parse(MyModels.Decode(Id, controllerSecret + HttpContext.Request.Headers["UserName"]).ToString());
            data.ListItemsDanhMuc = CategoriesArticlesService.GetListItems();
            data.SearchData = new SearchCategoriesArticles() { CurrentPage = 0, ItemsPerPage = 10, Keyword = ""};            
            if (IdDC == 0)
            {
                data.Item = new CategoriesArticles() { Culture = Culture.ToLower() };
            }
            else {

                data.Item = CategoriesArticlesService.GetItem(IdDC, controllerSecret + HttpContext.Request.Headers["UserName"], IdCoQuan, Culture);
                data.Item.Culture = Culture.ToLower();
            }
            
           
            return View(data);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveItem(CategoriesArticlesModel model)
        {
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            model.Item.IdCoQuan = int.Parse(HttpContext.Request.Headers["IdCoQuan"]);
            if (model.Item.Culture == null) {
                model.Item.Culture = "vi";
            }
            int IdDC = Int32.Parse(MyModels.Decode(model.Item.Ids, controllerSecret + HttpContext.Request.Headers["UserName"]).ToString());
            CategoriesArticlesModel data = model;
            string Link = "/Admin/" + ControllerName + "/Index";
            if (model.Item.Link != null && model.Item.Link.Trim() != "")
            {
                Link = model.Item.Link;
            }
            if ((model.Item.Culture == "vi" && ModelState.IsValid) || model.Item.Culture != "vi")
            {
                if (model.Item.Id == IdDC)
                {
                    model.Item.CreatedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    model.Item.ModifiedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    var Obj = CategoriesArticlesService.SaveItem(model.Item);
                    if (Obj.N == -2)
                    {
                        TempData["MessageError"] = "Chọn danh mục cha không hợp lệ";
                        data.ListItemsDanhMuc = CategoriesArticlesService.GetListItems();
                        if (model.Item.Culture.ToLower() != "vi")
                        {
                            CategoriesLanguage Lang = CategoriesLanguageService.GetItem(data.Item.Id, 1, model.Item.Culture.ToLower(), API.Models.Settings.SecretId + ControllerName);
                            if (Lang != null && Lang.Id > 0)
                            {
                                model.Item.Title = Lang.Title;
                                model.Item.Alias = Lang.Alias;
                                model.Item.Description = Lang.Description;
                                model.Item.Metadata = Lang.Metadata;
                                model.Item.Metadesc = Lang.Metadesc;
                                model.Item.Metakey = Lang.Metakey;
                            }
                            else
                            {
                                model.Item.Title = "";
                                model.Item.Alias = "";
                                model.Item.Description = "";
                                model.Item.Metadata = "";
                                model.Item.Metadesc = "";
                                model.Item.Metakey = "";
                            }
                        }
                        return View(data);
                    }
                    TempData["MessageSuccess"] = "Cập nhật thành công";
                    return Redirect(Link);
                }
            }
                
            data.ListItemsDanhMuc = CategoriesArticlesService.GetListItems();
            return View(data);
        }

        public IActionResult SaveItemInfo(string Id = null)
        {
            int IdCoQuan = int.Parse(HttpContext.Request.Headers["IdCoQuan"]);
            CategoriesArticlesModel data = new CategoriesArticlesModel();

            int IdDC = Int32.Parse(MyModels.Decode(Id, controllerSecret + HttpContext.Request.Headers["UserName"]).ToString());
            data.ListItemsDanhMuc = CategoriesArticlesService.GetListItems();
            data.SearchData = new SearchCategoriesArticles() { CurrentPage = 0, ItemsPerPage = 10, Keyword = "" };
            if (IdDC == 0)
            {
                data.Item = new CategoriesArticles();
            }
            else
            {
                data.Item = CategoriesArticlesService.GetItem(IdDC, controllerSecret + HttpContext.Request.Headers["UserName"], IdCoQuan);
            }
            data.Item.Ordering = data.Item.OrderingHome;

            return View(data);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveItemInfo(CategoriesArticlesModel model)
        {

            model.Item.IdCoQuan = int.Parse(HttpContext.Request.Headers["IdCoQuan"]);

            int IdDC = Int32.Parse(MyModels.Decode(model.Item.Ids, controllerSecret + HttpContext.Request.Headers["UserName"]).ToString());
            CategoriesArticlesModel data = model;
            if (ModelState.IsValid)
            {

                if (model.Item.Id == IdDC)
                {
                    model.Item.CreatedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    model.Item.ModifiedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    var Obj = CategoriesArticlesService.SaveItemInfo(model.Item);
                    if (Obj.N == -2)
                    {
                        TempData["MessageError"] = "Chọn danh mục cha không hợp lệ";
                        data.ListItemsDanhMuc = CategoriesArticlesService.GetListItems();
                        return View(data);
                    }
                    TempData["MessageSuccess"] = "Cập nhật thành công";
                    return RedirectToAction("Index");
                }
            }
            data.ListItemsDanhMuc = CategoriesArticlesService.GetListItems();
            return View(data);
        }
        [ValidateAntiForgeryToken]
        public ActionResult DeleteItem(string Id)
        {
            
            CategoriesArticles model = new CategoriesArticles() { Id = Int32.Parse(MyModels.Decode(Id, controllerSecret + HttpContext.Request.Headers["UserName"]).ToString()) };            
            try
            {
                if (model.Id > 0)
                {
                    model.CreatedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    model.ModifiedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    CategoriesArticlesService.DeleteItem(model);
                    TempData["MessageSuccess"] = "Xóa thành công";
                    return Json(new MsgSuccess());
                }
                else {
                    TempData["MessageError"] = "Xóa Không thành công";
                    return Json(new MsgError());
                }
                
            }
            catch {
                TempData["MessageSuccess"] = "Xóa không thành công";
                return Json(new MsgError());
            }
            

        }

        [ValidateAntiForgeryToken]
        public ActionResult UpdateFeaturedHome([FromQuery] string Ids, Boolean FeaturedHome)
        {
            

            CategoriesArticles item = new CategoriesArticles() { Id = Int32.Parse(MyModels.Decode(Ids, controllerSecret + HttpContext.Request.Headers["UserName"]).ToString()), FeaturedHome = FeaturedHome, IdCoQuan = int.Parse(HttpContext.Request.Headers["IdCoQuan"]) };
            try
            {
                if (item.Id > 0)
                {
                    item.CreatedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    item.ModifiedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    dynamic UpdateStatus = CategoriesArticlesService.UpdateFeaturedHome(item);
                    TempData["MessageSuccess"] = "Cập nhật Hiện trang chủ thành công";
                    return Json(new MsgSuccess());
                }
                else
                {
                    TempData["MessageError"] = "Cập nhật Hiện trang chủ Không thành công";
                    return Json(new MsgError());
                }
            }
            catch
            {
                TempData["MessageSuccess"] = "Cập nhật Hiện trang chủ không thành công";
                return Json(new MsgError());
            }
        }

        [ValidateAntiForgeryToken]
        public ActionResult UpdateStatus([FromQuery] string Ids, Boolean Status)
        {
            
            CategoriesArticles item = new CategoriesArticles() { Id = Int32.Parse(MyModels.Decode(Ids, controllerSecret + HttpContext.Request.Headers["UserName"]).ToString()), Status = Status };
            try
            {
                if (item.Id > 0)
                {
                    item.CreatedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    item.ModifiedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    dynamic UpdateStatus = CategoriesArticlesService.UpdateStatus(item);
                    TempData["MessageSuccess"] = "Cập nhật Trạng Thái thành công";
                    return Json(new MsgSuccess());
                }
                else
                {
                    TempData["MessageError"] = "Cập nhật Trạng Thái Không thành công";
                    return Json(new MsgError());
                }
            }
            catch
            {
                TempData["MessageSuccess"] = "Cập nhật Trạng Thái không thành công";
                return Json(new MsgError());
            }
        }
    }
}
