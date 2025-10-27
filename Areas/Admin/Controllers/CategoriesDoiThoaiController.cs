using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using API.Areas.Admin.Models.CategoriesDoiThoai;
using API.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Hosting;
using API.Areas.Admin.Models.Ablums;
using API.Areas.Admin.Models.USUsers;
using Microsoft.Extensions.Configuration;

namespace API.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoriesDoiThoaiController : Controller
    {
        private string controllerName = "CategoriesDoiThoaiController";
        private string controllerSecret;
        public CategoriesDoiThoaiController(IConfiguration config)
        {

            controllerSecret = config["Security:SecretId"] + controllerName;
        }

        public IActionResult Index([FromQuery] SearchCategoriesDoiThoai dto)
        {
            var Login = HttpContext.Session.GetString("Login");
            USUsers MyInfo = JsonConvert.DeserializeObject<USUsers>(Login);

            int TotalItems = 0;

            CategoriesDoiThoaiModel data = new CategoriesDoiThoaiModel() { SearchData = dto };
            data.ListItems = CategoriesDoiThoaiService.GetListPagination(data.SearchData, this.controllerSecret);
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
            CategoriesDoiThoaiModel data = new CategoriesDoiThoaiModel();

            int IdDC = Int32.Parse(MyModels.Decode(Id, this.controllerSecret).ToString());
            data.SearchData = new SearchCategoriesDoiThoai() { CurrentPage = 0, ItemsPerPage = 10, Keyword = "" };
            
            if (IdDC == 0)
            {
                data.Item = new CategoriesDoiThoai() { };
            }
            else
            {
                data.Item = CategoriesDoiThoaiService.GetItem(IdDC, controllerSecret);

            }
            return View(data);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SaveItem(CategoriesDoiThoai model)
        {
            var Login = HttpContext.Session.GetString("Login");
            USUsers MyInfo = JsonConvert.DeserializeObject<USUsers>(Login);

            int IdDC = Int32.Parse(MyModels.Decode(model.Ids, this.controllerSecret).ToString());
            CategoriesDoiThoaiModel data = new CategoriesDoiThoaiModel() { Item = model };

            if (model.Id == IdDC)
            {
                model.CreatedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                model.ModifiedBy = int.Parse(HttpContext.Request.Headers["Id"]);

                var Cat = CategoriesDoiThoaiService.SaveItem(model);

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
            
            CategoriesDoiThoai model = new CategoriesDoiThoai() { Id = Int32.Parse(MyModels.Decode(Id, this.controllerSecret).ToString()) };
            try
            {
                if (model.Id > 0)
                {
                    model.CreatedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    model.ModifiedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    CategoriesDoiThoaiService.DeleteItem(model);
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

            CategoriesDoiThoai item = new CategoriesDoiThoai() { Id = Int32.Parse(MyModels.Decode(Ids, this.controllerSecret).ToString()), Status = Status };
            try
            {
                if (item.Id > 0)
                {
                    item.CreatedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    item.ModifiedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    dynamic UpdateStatus = CategoriesDoiThoaiService.UpdateStatus(item);
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

            CategoriesDoiThoai item = new CategoriesDoiThoai() { Id = Int32.Parse(MyModels.Decode(Ids, this.controllerSecret).ToString()), Featured = Featured };
            try
            {
                if (item.Id > 0)
                {
                    item.CreatedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    item.ModifiedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    dynamic UpdateFeatured = CategoriesDoiThoaiService.UpdateFeatured(item);
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
