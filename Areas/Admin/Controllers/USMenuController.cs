using API.Areas.Admin.Models.USMenu;
using API.Areas.Admin.Models.USUsers;
using API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace API.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class USMenuController : Controller
    {
        public IActionResult Index([FromQuery] SearchUSMenu dto)
        {
            int TotalItems = 0;
            var Login = HttpContext.Session.GetString("Login");
            USUsers MyInfo = JsonConvert.DeserializeObject<USUsers>(Login);
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            if (dto.IdCoQuan == 0) { dto.IdCoQuan = MyInfo.IdCoQuan; }
            USMenuModel data = new USMenuModel() { SearchData = dto };
            data.ListItems = USMenuService.GetListPagination(data.SearchData, API.Models.Settings.SecretId + ControllerName);
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
            USMenuModel data = new USMenuModel();
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            int IdDC = Int32.Parse(MyModels.Decode(Id, API.Models.Settings.SecretId + ControllerName).ToString());
            data.SearchData = new SearchUSMenu() { CurrentPage = 0, ItemsPerPage = 10, Keyword = "" };
            data.ListItemsMenus = USMenuService.GetListItems();
            if (IdDC == 0)
            {
                data.Item = new USMenu();
            }
            else
            {
                data.Item = USMenuService.GetItem(IdDC, API.Models.Settings.SecretId + ControllerName);
            }
            return View(data);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveItem(USMenu model)
        {
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            int IdDC = Int32.Parse(MyModels.Decode(model.Ids, API.Models.Settings.SecretId + ControllerName).ToString());
            USMenuModel data = new USMenuModel() { Item = model };
            if (ModelState.IsValid)
            {
                if (model.Id == IdDC)
                {
                    model.CreatedBy = model.ModifiedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    USMenuService.SaveItem(model);
                    if (model.Id > 0)
                    {
                        TempData["MessageSuccess"] = "Cập nhật thành công";
                    }
                    else
                    {
                        TempData["MessageSuccess"] = "Thêm mới thành công";
                    }
                    return RedirectToAction("Index");
                }
            }
            return View(data);
        }

        [ValidateAntiForgeryToken]
        public ActionResult DeleteItem(string Id)
        {
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            USMenu item = new USMenu() { Id = Int32.Parse(MyModels.Decode(Id, API.Models.Settings.SecretId + ControllerName).ToString()) };
            try
            {
                if (item.Id > 0)
                {
                    item.CreatedBy = item.ModifiedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    USMenuService.DeleteItem(item);
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
    }
}
