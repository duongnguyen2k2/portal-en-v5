using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using API.Areas.Admin.Models.DocumentRefersCategories;
using API.Models;
using Newtonsoft.Json;


namespace API.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DocumentRefersCategoriesController : Controller
    {
        public IActionResult IndexTaiLieuHop([FromQuery] SearchDocumentRefersCategories dto)
        {
            int TotalItems = 0;
            dto.TaiLieuHop = 1;
            dto.IdCoQuan = int.Parse(HttpContext.Request.Headers["IdCoQuan"]);
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            DocumentRefersCategoriesModel data = new DocumentRefersCategoriesModel() { SearchData = dto };
            data.ListItems = DocumentRefersCategoriesService.GetListPagination(data.SearchData, API.Models.Settings.SecretId + ControllerName);
            if (data.ListItems != null && data.ListItems.Count() > 0)
            {
                TotalItems = data.ListItems[0].TotalRows;
            }
            data.Pagination = new Models.Partial.PartialPagination() { CurrentPage = data.SearchData.CurrentPage, ItemsPerPage = data.SearchData.ItemsPerPage, TotalItems = TotalItems, QueryString = Request.QueryString.ToString() };

            return View(data);
        }

        public IActionResult Index([FromQuery] SearchDocumentRefersCategories dto)
        {
            int TotalItems = 0;
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            dto.IdCoQuan = int.Parse(HttpContext.Request.Headers["IdCoQuan"]);
            dto.TaiLieuHop = 0;
            DocumentRefersCategoriesModel data = new DocumentRefersCategoriesModel() { SearchData = dto};
            data.ListItems = DocumentRefersCategoriesService.GetListPagination(data.SearchData, API.Models.Settings.SecretId + ControllerName);            
            if (data.ListItems != null && data.ListItems.Count() > 0)
            {
                TotalItems = data.ListItems[0].TotalRows;
            }
            data.Pagination = new Models.Partial.PartialPagination() { CurrentPage = data.SearchData.CurrentPage, ItemsPerPage = data.SearchData.ItemsPerPage, TotalItems = TotalItems, QueryString = Request.QueryString.ToString() };

            return View(data);
        }

        public IActionResult SaveItemTaiLieuHop(string Id = null)
        {
            DocumentRefersCategoriesModel data = new DocumentRefersCategoriesModel();
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            int IdDC = Int32.Parse(MyModels.Decode(Id, API.Models.Settings.SecretId + ControllerName).ToString());
            data.SearchData = new SearchDocumentRefersCategories() { CurrentPage = 0, ItemsPerPage = 10, Keyword = "" };
            if (IdDC == 0)
            {
                data.Item = new DocumentRefersCategories();
            }
            else
            {
                data.Item = DocumentRefersCategoriesService.GetItem(IdDC, API.Models.Settings.SecretId + ControllerName,true);
            }


            return View(data);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveItemTaiLieuHop(DocumentRefersCategories model)
        {
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            int IdDC = Int32.Parse(MyModels.Decode(model.Ids, API.Models.Settings.SecretId + ControllerName).ToString());
            DocumentRefersCategoriesModel data = new DocumentRefersCategoriesModel() { Item = model };
            if (ModelState.IsValid)
            {
                if (model.Id == IdDC)
                {
                    model.CreatedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    model.ModifiedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    model.TaiLieuHop = true;
                    model.IdCoQuan = int.Parse(HttpContext.Request.Headers["IdCoQuan"]);
                    if (model.Id==0)
                    {
                        model.MatKhau = API.Models.MyHelper.StringHelper.RandomString(20);
                    }
                    DocumentRefersCategoriesService.SaveItem(model);
                    if (model.Id > 0)
                    {
                        TempData["MessageSuccess"] = "Cập nhật Tài liệu họp thành công";
                    }
                    else
                    {
                        TempData["MessageSuccess"] = "Thêm mới Tài liệu họp thành công";
                    }
                    return RedirectToAction("IndexTaiLieuHop");
                }
            }
            return View(data);
        }


        public IActionResult SaveItem(string Id=null)
        {
            DocumentRefersCategoriesModel data = new DocumentRefersCategoriesModel();
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            int IdDC = Int32.Parse(MyModels.Decode(Id, API.Models.Settings.SecretId + ControllerName).ToString());            
            data.SearchData = new SearchDocumentRefersCategories() { CurrentPage = 0, ItemsPerPage = 10, Keyword = ""};            
            if (IdDC == 0)
            {
                data.Item = new DocumentRefersCategories();
            }
            else {
                data.Item = DocumentRefersCategoriesService.GetItem(IdDC, API.Models.Settings.SecretId + ControllerName);
            }
            
           
            return View(data);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveItem(DocumentRefersCategories model)
        {
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            int IdDC = Int32.Parse(MyModels.Decode(model.Ids, API.Models.Settings.SecretId + ControllerName).ToString());
            DocumentRefersCategoriesModel data = new DocumentRefersCategoriesModel() { Item = model};            
            if (ModelState.IsValid)
            {
                if (model.Id == IdDC)
                {
                    model.CreatedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    model.ModifiedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    model.IdCoQuan = int.Parse(HttpContext.Request.Headers["IdCoQuan"]);
                    DocumentRefersCategoriesService.SaveItem(model);
                    if (model.Id > 0)
                    {
                        TempData["MessageSuccess"] = "Cập nhật thành công";
                    }
                    else {
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
            DocumentRefersCategories model = new DocumentRefersCategories() { Id = Int32.Parse(MyModels.Decode(Id, API.Models.Settings.SecretId + ControllerName).ToString()) };            
            try
            {
                if (model.Id > 0)
                {
                    model.CreatedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    model.ModifiedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    DocumentRefersCategoriesService.DeleteItem(model);
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
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            DocumentRefersCategories item = new DocumentRefersCategories() { Id = Int32.Parse(MyModels.Decode(Ids, API.Models.Settings.SecretId + ControllerName).ToString()), Status = Status };
            try
            {
                if (item.Id > 0)
                {
                    item.CreatedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    item.ModifiedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    dynamic UpdateStatus = DocumentRefersCategoriesService.UpdateStatus(item);
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
