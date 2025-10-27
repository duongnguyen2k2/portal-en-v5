using API.Areas.Admin.Models.CategoriesArticles;
using API.Areas.Admin.Models.DMCoQuan;
using API.Areas.Admin.Models.DMCoQuanLanguage;
using API.Areas.Admin.Models.DMLoaiCoQuan;
using API.Areas.Admin.Models.USUsers;
using API.Models;
using DocumentFormat.OpenXml.EMMA;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace API.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DMCoQuanController : Controller
    {
        public IActionResult Index([FromQuery] SearchDMCoQuan dto)
        {
            int TotalItems = 0;
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            DMCoQuanModel data = new DMCoQuanModel();
            dto.ParentId = int.Parse(HttpContext.Request.Headers["IdCoQuan"].ToString());
            data.SearchData = dto;
            data.ListItemsLoaiCoQuan = DMLoaiCoQuanService.GetListSelectItems();
            data.ListTemplate = DMCoQuanService.GetListTemplate();

            if (dto.CategoryId == 0 && data.ListItemsLoaiCoQuan != null && data.ListItemsLoaiCoQuan.Count() > 0)
            {
                data.SearchData.CategoryId = Convert.ToInt32(data.ListItemsLoaiCoQuan[0].Value);
            }

            data.ListItems = DMCoQuanService.GetListPagination(data.SearchData, API.Models.Settings.SecretId + ControllerName);
            if (data.ListItems != null && data.ListItems.Count() > 0)
            {
                TotalItems = data.ListItems[0].TotalRows;
            }
            data.Pagination = new Models.Partial.PartialPagination() { CurrentPage = data.SearchData.CurrentPage, ItemsPerPage = data.SearchData.ItemsPerPage, TotalItems = TotalItems, QueryString = Request.QueryString.ToString() };

            return View(data);
        }

        public IActionResult SaveItem([FromQuery] string Culture = "vi", string Id = null, int CategoryId = 0, int ParentId = 1, int IdKTTT = 1)
        {

            DMCoQuanModel data = new DMCoQuanModel();
            var Login = HttpContext.Session.GetString("Login");
            USUsers MyInfo = JsonConvert.DeserializeObject<USUsers>(Login);

            ParentId = int.Parse(HttpContext.Request.Headers["IdCoQuan"].ToString());
            data.ListItemsCoQuan = DMCoQuanService.GetListByLoaiCoQuan(0, 0, MyInfo.IdCoQuan);
            data.ListTemplate = DMCoQuanService.GetListTemplate();
            data.ListItemsLoaiCoQuan = DMLoaiCoQuanService.GetListSelectItems();
            data.ListItemsDanhMuc = CategoriesArticlesService.GetListItems();
            int IdDC = 0;
            if (!string.IsNullOrEmpty(Id))
            {
                IdDC = Int32.Parse(MyModels.Decode(Id, API.Models.Settings.SecretId + "DMCoQuan").ToString());
            }
            else
            {
                IdDC = IdKTTT;
            }

            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            data.SearchData = new SearchDMCoQuan() { CurrentPage = 0, ItemsPerPage = 10, Keyword = "" };
            if (IdDC == 0)
            {
                data.Item = new DMCoQuan() { Culture = Culture.ToLower() };
                data.Item.CategoryId = CategoryId;
                data.Item.ParentId = ParentId;
            }
            else
            {
                data.Item = DMCoQuanService.GetItem(IdDC, API.Models.Settings.SecretId + "DMCoQuan".ToString(), Culture);
                data.Config = DMCoQuanService.GetDMCoQuanConfig(data.Config, data.Item.Id);
                data.Item.Culture = Culture.ToLower();
            }


            return View(data);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveItem(DMCoQuan model)
        {
            var Login = HttpContext.Session.GetString("Login");
            USUsers MyInfo = JsonConvert.DeserializeObject<USUsers>(Login);

            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            int IdDC = Int32.Parse(MyModels.Decode(model.Ids, API.Models.Settings.SecretId + ControllerName).ToString());
            DMCoQuanModel data = new DMCoQuanModel();
            data.ListTemplate = DMCoQuanService.GetListTemplate();
            data.ListItemsLoaiCoQuan = DMLoaiCoQuanService.GetListSelectItems();

            data.Item = model;
            if (ModelState.IsValid)
            {
                if (model.Id == IdDC)
                {
                    model.CreatedBy = model.ModifiedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    var Obj = DMCoQuanService.SaveItem(model);
                    string cacheCode = HttpContext.Request.Host.Value.ToLower() + DateTime.Now.ToString("yyyyMMddhh");
                    HttpContext.Session.Remove(cacheCode);
                    if (Obj.N == -1)
                    {
                        TempData["MessageError"] = "Mã cơ quan trùng";
                        data.ListItemsCoQuan = DMCoQuanService.GetListByLoaiCoQuan(0, 0, MyInfo.IdCoQuan);

                        return View(data);
                    } else if (Obj.N == -2)
                    {
                        TempData["MessageError"] = "Chọn Cơ quan cha không hợp lệ";
                        data.ListItemsCoQuan = DMCoQuanService.GetListByLoaiCoQuan(0, 0, MyInfo.IdCoQuan);

                        return View(data);
                    } else
                    {
                        if (model.FolderUpload != null && model.FolderUpload.Trim() != "") {

                            string dirPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot") + "/uploads/" + model.FolderUpload;
                            if (!Directory.Exists(dirPath))
                            {
                                Directory.CreateDirectory(dirPath);
                            }
                        }
                        TempData["MessageSuccess"] = "Cập nhật thành công";
                        return RedirectToAction("Index", new { CategoryId = model.CategoryId });
                    }
                }
            }
            else
            {
                data.ListItemsCoQuan = DMCoQuanService.GetListByLoaiCoQuan(0, 0, MyInfo.IdCoQuan);
                data.ListItemsDanhMuc = CategoriesArticlesService.GetListItems();
            }
            return View(data);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveItemLanguage(DMCoQuan model)
        {
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            int IdDC = Int32.Parse(MyModels.Decode(model.Ids, API.Models.Settings.SecretId + ControllerName).ToString());

            if (model.Id > 0)
            {
                if (model.Id == IdDC)
                {
                    DMCoQuanLanguageService.SaveItem(model);
                }
            }
            return Redirect("/Admin/DMCoQuan/SaveItem/"+model.Ids+ "?Culture="+model.Culture);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveItemConfiguration(DMCoQuan model)
        {
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            int IdDC = Int32.Parse(MyModels.Decode(model.Ids, API.Models.Settings.SecretId + ControllerName).ToString());

            if (model.Id > 0)
            {
                if (model.Id == IdDC)
                {
                    DMCoQuanService.SaveItemConfig(model);
                }
            }
            return Redirect("/Admin/DMCoQuan/");
        }

        [ValidateAntiForgeryToken]
        public ActionResult DeleteItem(string Id)
        {
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            DMCoQuanModel data = new DMCoQuanModel();
            DMCoQuan item = new DMCoQuan() { Id = Int32.Parse(MyModels.Decode(Id, API.Models.Settings.SecretId + ControllerName).ToString()) };            
            try
            {
                if (item.Id > 0)
                {                    
                    item.ModifiedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    DMCoQuanService.DeleteItem(item);
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
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            DMCoQuan item = new DMCoQuan() { Id = Int32.Parse(MyModels.Decode(Ids, API.Models.Settings.SecretId + ControllerName).ToString()), Status = Status };
            try
            {
                if (item.Id > 0)
                {                    
                    item.ModifiedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    dynamic UpdateStatus = DMCoQuanService.UpdateStatus(item);
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