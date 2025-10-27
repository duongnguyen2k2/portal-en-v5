using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using API.Areas.Admin.Models.Documents;
using API.Areas.Admin.Models.DocumentsCategories;
using API.Areas.Admin.Models.DocumentsType;
using API.Areas.Admin.Models.DocumentsField;
using API.Areas.Admin.Models.DocumentsLevel;
using API.Models;
using Newtonsoft.Json;
using API.Areas.Admin.Models.USUsers;
using Microsoft.Extensions.Configuration;

namespace API.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DocumentsController : Controller
    {
        private string controllerName = "DocumentsController";
        private string controllerSecret;
        public DocumentsController(IConfiguration config)
        {

            controllerSecret = config["Security:SecretId"] + controllerName;
        }
        public IActionResult Index([FromQuery] SearchDocuments dto)
        {
            var Login = HttpContext.Session.GetString("Login");
            USUsers MyInfo = JsonConvert.DeserializeObject<USUsers>(Login);

            int TotalItems = 0;
            
            dto.IdCoQuan = MyInfo.IdCoQuan;
            DocumentsModel data = new DocumentsModel() { SearchData = dto };
            data.ListItems = DocumentsService.GetListPagination(data.SearchData, controllerSecret+ HttpContext.Request.Headers["UserName"]);
            if (data.ListItems != null && data.ListItems.Count() > 0)
            {
                TotalItems = data.ListItems[0].TotalRows;
            }
            data.Pagination = new Models.Partial.PartialPagination() { CurrentPage = data.SearchData.CurrentPage, ItemsPerPage = data.SearchData.ItemsPerPage, TotalItems = TotalItems, QueryString = Request.QueryString.ToString() };
            data.ListDocumentsCategories = DocumentsCategoriesService.GetListSelectItems();
            data.ListDocumentsType = DocumentsTypeService.GetListSelectItems();
            data.ListDocumentsField = DocumentsFieldService.GetListSelectItems();
            data.ListDocumentsLevel = DocumentsLevelService.GetListSelectItems();
            data.ListItemsStatus = DocumentsService.GetListItemsStatus();
            return View(data);
        }

        public IActionResult SaveItem(string Id = null)
        {
            var Login = HttpContext.Session.GetString("Login");
            USUsers MyInfo = JsonConvert.DeserializeObject<USUsers>(Login);
            DocumentsModel data = new DocumentsModel();
            
            int IdDC = Int32.Parse(MyModels.Decode(Id, controllerSecret+ HttpContext.Request.Headers["UserName"]).ToString());
            data.SearchData = new SearchDocuments() { CurrentPage = 0, ItemsPerPage = 10, Keyword = "" };
            data.ListDocumentsCategories = DocumentsCategoriesService.GetListSelectItems();
            data.ListDocumentsType = DocumentsTypeService.GetListSelectItems();
            data.ListDocumentsField = DocumentsFieldService.GetListSelectItems();
            data.ListDocumentsLevel = DocumentsLevelService.GetListSelectItems();
            Documents Item = new Documents() {
                IssuedDateShow = DateTime.Now.ToString("dd/MM/yyyy"),
                EffectiveDateShow = DateTime.Now.ToString("dd/MM/yyyy"),
                IdCoQuan = MyInfo.IdCoQuan
            };
            if (IdDC > 0)
            {
                Item = DocumentsService.GetItem(IdDC, controllerSecret+ HttpContext.Request.Headers["UserName"]);
            }
          
            data.Item = Item;

            return View(data);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveItem(Documents model)
        {
            var Login = HttpContext.Session.GetString("Login");
            USUsers MyInfo = JsonConvert.DeserializeObject<USUsers>(Login);

            
            int IdDC = Int32.Parse(MyModels.Decode(model.Ids, controllerSecret+ HttpContext.Request.Headers["UserName"]).ToString());
            DocumentsModel data = new DocumentsModel() { Item = model };
            data.ListDocumentsCategories = DocumentsCategoriesService.GetListSelectItems();
            data.ListDocumentsType = DocumentsTypeService.GetListSelectItems();
            data.ListDocumentsField = DocumentsFieldService.GetListSelectItems();
            data.ListDocumentsLevel = DocumentsLevelService.GetListSelectItems();
            if (ModelState.IsValid)
            {
                if (model.Id == IdDC)
                {
                    model.CreatedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    model.ModifiedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    try
                    {
                        if (model.Id == 0)
                        {
                            model.IdCoQuan = MyInfo.IdCoQuan;
                        }
                        var a = DocumentsService.SaveItem(model);
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
                    catch(Exception e)
                    {
                        TempData["MessageError"] = "Lỗi khi lưu dữ liệu ("+e.Message+")";
                    }

                }
            }
            return View(data);
        }

        [ValidateAntiForgeryToken]
        public ActionResult DeleteItem(string Id)
        {
            
            Documents model = new Documents() { Id = Int32.Parse(MyModels.Decode(Id, controllerSecret+ HttpContext.Request.Headers["UserName"]).ToString()) };
            try
            {
                if (model.Id > 0)
                {
                    model.CreatedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    model.ModifiedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    DocumentsService.DeleteItem(model);
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
            
            Documents item = new Documents() { Id = Int32.Parse(MyModels.Decode(Ids, controllerSecret+ HttpContext.Request.Headers["UserName"]).ToString()), Status = Status };
            try
            {
                if (item.Id > 0)
                {
                    item.CreatedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    item.ModifiedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    dynamic UpdateStatus = DocumentsService.UpdateStatus(item);
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

            Documents item = new Documents() { Id = Int32.Parse(MyModels.Decode(Ids, controllerSecret + HttpContext.Request.Headers["UserName"]).ToString()), Featured = Featured };
            try
            {
                if (item.Id > 0)
                {
                    item.CreatedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    item.ModifiedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    dynamic UpdateFeatured = DocumentsService.UpdateFeatured(item);
                    TempData["MessageSuccess"] = "Cập nhật Featured thành công";
                    return Json(new MsgSuccess());
                }
                else
                {
                    TempData["MessageError"] = "Cập nhật Featured Không thành công";
                    return Json(new MsgError());
                }
            }
            catch
            {
                TempData["MessageSuccess"] = "Cập nhật Featured không thành công";
                return Json(new MsgError());
            }
        }

        [ValidateAntiForgeryToken]
        public ActionResult UpdateFeaturedHome([FromQuery] string Ids, Boolean FeaturedHome)
        {

            Documents item = new Documents() { Id = Int32.Parse(MyModels.Decode(Ids, controllerSecret + HttpContext.Request.Headers["UserName"]).ToString()), FeaturedHome = FeaturedHome };
            try
            {
                if (item.Id > 0)
                {
                    item.CreatedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    item.ModifiedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    dynamic UpdateFeatured = DocumentsService.UpdateFeaturedHome(item);
                    TempData["MessageSuccess"] = "Cập nhật Featured Home thành công";
                    return Json(new MsgSuccess());
                }
                else
                {
                    TempData["MessageError"] = "Cập nhật Featured Home Không thành công";
                    return Json(new MsgError());
                }
            }
            catch
            {
                TempData["MessageSuccess"] = "Cập nhật Featured không thành công";
                return Json(new MsgError());
            }
        }
    }
}
