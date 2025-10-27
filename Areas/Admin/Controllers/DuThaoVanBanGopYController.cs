using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using API.Areas.Admin.Models.DuThaoVanBanGopY;
using API.Models;
using Newtonsoft.Json;
using API.Areas.Admin.Models.DMLinhVuc;
using Microsoft.Extensions.Configuration;
namespace API.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DuThaoVanBanGopYController : Controller
    {
        private string controllerName = "DuThaoVanBanController";
        private string controllerSecret;
        public DuThaoVanBanGopYController(IConfiguration config)
        {
            controllerSecret = config["Security:SecretId"] + controllerName;
        }

        public IActionResult Index([FromQuery] SearchDuThaoVanBanGopY dto)
        {
            int TotalItems = 0;
            
            DuThaoVanBanGopYModel data = new DuThaoVanBanGopYModel() { SearchData = dto};
            data.ListItems = DuThaoVanBanGopYService.GetListPagination(data.SearchData, (controllerSecret + HttpContext.Request.Headers["UserName"]).ToString());            
            if (data.ListItems != null && data.ListItems.Count() > 0)
            {
                TotalItems = data.ListItems[0].TotalRows;
            }
            data.Pagination = new Models.Partial.PartialPagination() { CurrentPage = data.SearchData.CurrentPage, ItemsPerPage = data.SearchData.ItemsPerPage, TotalItems = TotalItems, QueryString = Request.QueryString.ToString() };
            
            return View(data);
        }

        public IActionResult SaveItem(string Id = null, int IdDuThao = 0, string IdsDuThao="")
        {
            DuThaoVanBanGopYModel data = new DuThaoVanBanGopYModel();
            
            int IdDC = Int32.Parse(MyModels.Decode(Id, (controllerSecret + HttpContext.Request.Headers["UserName"]).ToString()).ToString());            
            data.SearchData = new SearchDuThaoVanBanGopY() { CurrentPage = 0, ItemsPerPage = 10, Keyword = "", IdDuThao = IdDuThao };
            if (IdDC == 0)
            {
                data.Item = new DuThaoVanBanGopY() { IdDuThao = IdDuThao };
            }
            else {
                data.Item = DuThaoVanBanGopYService.GetItem(IdDC, (controllerSecret + HttpContext.Request.Headers["UserName"]).ToString());
            }
            

            return View(data);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveItem(DuThaoVanBanGopY model)
        {
            
            int IdDC = Int32.Parse(MyModels.Decode(model.Ids, (controllerSecret + HttpContext.Request.Headers["UserName"]).ToString()).ToString());
            DuThaoVanBanGopYModel data = new DuThaoVanBanGopYModel() { Item = model};            
            if (ModelState.IsValid)
            {
                if (model.Id == IdDC)
                {
                    model.CreatedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    model.ModifiedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    DuThaoVanBanGopYService.SaveItem(model);
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
            
            DuThaoVanBanGopY model = new DuThaoVanBanGopY() { Id = Int32.Parse(MyModels.Decode(Id, (controllerSecret + HttpContext.Request.Headers["UserName"]).ToString()).ToString()) };            
            try
            {
                if (model.Id > 0)
                {
                    model.CreatedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    model.ModifiedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    DuThaoVanBanGopYService.DeleteItem(model);
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
        public ActionResult UpdateStatus([FromQuery] string Ids, Boolean Status)
        {
            
            DuThaoVanBanGopY item = new DuThaoVanBanGopY() { Id = Int32.Parse(MyModels.Decode(Ids, (controllerSecret + HttpContext.Request.Headers["UserName"]).ToString()).ToString()), Status = Status };
            try
            {
                if (item.Id > 0)
                {
                    item.CreatedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    item.ModifiedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    dynamic UpdateStatus = DuThaoVanBanGopYService.UpdateStatus(item);
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
