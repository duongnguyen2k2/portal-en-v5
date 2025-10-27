using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using API.Areas.Admin.Models.CategoriesVideos;
using API.Models;
using Newtonsoft.Json;
using API.Areas.Admin.Models.USUsers;
using Microsoft.Extensions.Configuration;

namespace API.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoriesVideosController : Controller
    {
        private string controllerName = "CategoriesVideosController";
        private string controllerSecret;

        private IConfiguration Configuration;
        public CategoriesVideosController(IConfiguration config)
        {
            Configuration = config;
            controllerSecret = config["Security:SecretId"] + controllerName;
        }

        public IActionResult Index([FromQuery] SearchCategoriesVideos dto)
        {
            var Login = HttpContext.Session.GetString("Login");
            USUsers MyInfo = JsonConvert.DeserializeObject<USUsers>(Login);

            int TotalItems = 0;

            CategoriesVideosModel data = new CategoriesVideosModel() { SearchData = dto };
            data.ListItems = CategoriesVideosService.GetListPagination(data.SearchData, this.controllerSecret);
            if (data.ListItems != null && data.ListItems.Count() > 0)
            {
                TotalItems = data.ListItems[0].TotalRows;
            }

            data.Pagination = new Models.Partial.PartialPagination() { CurrentPage = data.SearchData.CurrentPage, ItemsPerPage = data.SearchData.ItemsPerPage, TotalItems = TotalItems, QueryString = Request.QueryString.ToString() };

            return View(data);
        }

        public IActionResult SaveItem(string Id = null)
        {
            var Login = HttpContext.Session.GetString("Login");
            USUsers MyInfo = JsonConvert.DeserializeObject<USUsers>(Login);
            CategoriesVideosModel data = new CategoriesVideosModel();

            int IdDC = Int32.Parse(MyModels.Decode(Id, this.controllerSecret).ToString());
            data.SearchData = new SearchCategoriesVideos() { CurrentPage = 0, ItemsPerPage = 10, Keyword = "" };
            data.ListItemsParents = CategoriesVideosService.GetListItems();
            if (IdDC == 0)
            {
                data.Item = new CategoriesVideos() { };
            }
            else
            {
                if (Configuration["Security:SiteCode"] == "KTTT")
                {
                    data.Item = CategoriesVideosService.GetItemAdmin(IdDC, controllerSecret);
                }else
                {
                    data.Item = CategoriesVideosService.GetItem(IdDC, controllerSecret);
                }

            }
            return View(data);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SaveItem(CategoriesVideos model)
        {
            var Login = HttpContext.Session.GetString("Login");
            USUsers MyInfo = JsonConvert.DeserializeObject<USUsers>(Login);

            int IdDC = Int32.Parse(MyModels.Decode(model.Ids, this.controllerSecret).ToString());
            CategoriesVideosModel data = new CategoriesVideosModel() { Item = model };

            if (model.Id == IdDC)
            {
                model.CreatedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                model.ModifiedBy = int.Parse(HttpContext.Request.Headers["Id"]);

                var Cat = CategoriesVideosService.SaveItem(model);

                if (model.Id > 0)
                {
                    TempData["MessageSuccess"] = "Cập nhật Danh mục truyền thanh thành công";
                }
                else
                {
                    TempData["MessageSuccess"] = "Thêm mới Danh mục truyền thanh thành công";
                }
                return RedirectToAction("Index");
            }

            return View(data);
        }

        [ValidateAntiForgeryToken]
        public ActionResult DeleteItem(string Id)
        {
            
            CategoriesVideos model = new CategoriesVideos() { Id = Int32.Parse(MyModels.Decode(Id, this.controllerSecret).ToString()) };
            try
            {
                if (model.Id > 0)
                {
                    model.CreatedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    model.ModifiedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    CategoriesVideosService.DeleteItem(model);
                    TempData["MessageSuccess"] = "Xóa Danh mục truyền hình thành công";
                    return Json(new MsgSuccess());
                }
                else
                {
                    TempData["MessageError"] = "Xóa Danh mục truyền hình Không thành công";
                    return Json(new MsgError());
                }

            }
            catch
            {
                TempData["MessageSuccess"] = "Xóa Danh mục truyền hình không thành công";
                return Json(new MsgError());
            }


        }

        [ValidateAntiForgeryToken]
        public ActionResult UpdateStatus([FromQuery] string Ids, Boolean Status)
        {

            CategoriesVideos item = new CategoriesVideos() { Id = Int32.Parse(MyModels.Decode(Ids, this.controllerSecret).ToString()), Status = Status };
            try
            {
                if (item.Id > 0)
                {
                    item.CreatedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    item.ModifiedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    dynamic UpdateStatus = CategoriesVideosService.UpdateStatus(item);
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

        [ValidateAntiForgeryToken]
        public ActionResult UpdateFeatured([FromQuery] string Ids, Boolean Featured)
        {

            CategoriesVideos item = new CategoriesVideos() { Id = Int32.Parse(MyModels.Decode(Ids, this.controllerSecret).ToString()), Featured = Featured };
            try
            {
                if (item.Id > 0)
                {
                    item.CreatedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    item.ModifiedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    dynamic UpdateFeatured = CategoriesVideosService.UpdateFeatured(item);
                    TempData["MessageSuccess"] = "Cập nhật Nổi bật thành công";
                    return Json(new MsgSuccess());
                }
                else
                {
                    TempData["MessageError"] = "Cập nhật Nổi bật Không thành công";
                    return Json(new MsgError());
                }
            }
            catch
            {
                TempData["MessageSuccess"] = "Cập nhật Nổi bật không thành công";
                return Json(new MsgError());
            }
        }

    }
}
