using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using API.Areas.Admin.Models.CategoriesProducts;
using API.Models;
using API.Models.Utilities;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace API.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoriesProductsController : Controller
    {
        private string controllerName = "CategoriesProductsController";
        private string controllerSecret;
        public CategoriesProductsController(IConfiguration config)
        {
            controllerSecret = config["Security:SecretId"] + controllerName;
        }
        public IActionResult Index([FromQuery] SearchCategoriesProducts dto)
        {
            return View();
        }

        [HttpPost]
        public IActionResult GetListJson([FromBody] SearchCategoriesProducts dto)
        {
            List<CategoriesProducts> ListItems = CategoriesProductsService.GetListPagination(dto, this.controllerSecret);
            return Json(new MsgSuccess() { Data = ListItems });
        }

        [HttpPost]
        public IActionResult GetListAll([FromBody] SearchCategoriesProducts dto)
        {
            List<CategoriesProducts> ListItems = CategoriesProductsService.GetList();
            return Json(new MsgSuccess() { Data = ListItems });
        }

        [HttpPost]
        public IActionResult GetItem([FromQuery] string Id = null)
        {
            int IdDC = Int32.Parse(MyModels.Decode(Id, this.controllerSecret).ToString());
            CategoriesProducts Item = new CategoriesProducts();
            if (IdDC > 0)
            {
                Item = CategoriesProductsService.GetItem(IdDC, this.controllerSecret);
            }
            return Json(new MsgSuccess() { Data = Item, Msg = "Lấy thông tin thành công" });
        }

        public IActionResult SaveItem(string Id=null)
        {
            CategoriesProductsModel data = new CategoriesProductsModel();
            
            int IdDC = Int32.Parse(MyModels.Decode(Id, this.controllerSecret).ToString());
            data.ListItemsDanhMuc = CategoriesProductsService.GetListItems();
            data.SearchData = new SearchCategoriesProducts() { CurrentPage = 0, ItemsPerPage = 10, Keyword = ""};            
            if (IdDC == 0)
            {
                data.Item = new CategoriesProducts();
            }
            else {
                data.Item = CategoriesProductsService.GetItem(IdDC, this.controllerSecret);
            }
            
           
            return View(data);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveItem(CategoriesProductsModel model)
        {
            
            int IdDC = Int32.Parse(MyModels.Decode(model.Item.Ids, this.controllerSecret).ToString());
            CategoriesProductsModel data = model;            
            if (ModelState.IsValid)
            {
                if(model.Item.Icon != null)
                {
                    var Image =
                    await FileHelpers.ProcessFormFile(model.Item.Icon, ModelState);
                    if (Image.Length > 0)
                        model.Item.Images = Image;
                }
                
                if (model.Item.Id == IdDC)
                {
                    model.Item.CreatedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    model.Item.ModifiedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    var Obj = CategoriesProductsService.SaveItem(model.Item);
                    if (Obj.N == -2)
                    {
                        TempData["MessageError"] = "Chọn danh mục cha không hợp lệ";
                        data.ListItemsDanhMuc = CategoriesProductsService.GetListItems();
                        return View(data);
                    }
                    TempData["MessageSuccess"] = "Cập nhật thành công";
                    return RedirectToAction("Index");
                }
            }
            data.ListItemsDanhMuc = CategoriesProductsService.GetListItems();
            return View(data);
        }
        
        [ValidateAntiForgeryToken]
        public ActionResult DeleteItem(string Id)
        {
            
            CategoriesProducts model = new CategoriesProducts() { Id = Int32.Parse(MyModels.Decode(Id, this.controllerSecret).ToString()) };            
            try
            {
                if (model.Id > 0)
                {
                    model.CreatedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    model.ModifiedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    CategoriesProductsService.DeleteItem(model);
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

        [HttpGet]
        public IActionResult GetListGroupsByCat([FromQuery]string Ids)
        {
            int IdDC = Int32.Parse(MyModels.Decode(Ids, this.controllerSecret).ToString());
            List<CatGroup> ListItems = CategoriesProductsService.GetListGroupsByCat(IdDC, controllerSecret);
            return Json(new API.Models.MsgError() { Msg = "", Data = ListItems });
        }

        [HttpPost]
        public IActionResult SaveManagerCatGroup([FromBody] List<CatGroup> ListItems)
        {
            if (ListItems != null && ListItems.Count() > 0)
            {
                int IdCat = ListItems[0].IdCat;
                string IdsCat = ListItems[0].IdsCat;
                int IdDC = Int32.Parse(MyModels.Decode(IdsCat, this.controllerSecret).ToString());
                if (IdDC == IdCat)
                {
                    dynamic item = CategoriesProductsService.SaveManagerCatGroup(ListItems, IdCat);
                    return Json(new API.Models.MsgError() { Msg = "", Data = ListItems });
                }
                else
                {
                    return Json(new API.Models.MsgError() { Msg = "Bạn không có quyền,", Data = ListItems });
                }

            }
            else
            {
                return Json(new API.Models.MsgError() { Msg = "Bạn không có quyền. Xin vui lòng liên hệ Quản trị", Data = ListItems });
            }

        }

        [ValidateAntiForgeryToken]
        public ActionResult UpdateFeatured([FromQuery] string Ids, Boolean Featured)
        {
            

            CategoriesProducts item = new CategoriesProducts() { Id = Int32.Parse(MyModels.Decode(Ids, this.controllerSecret).ToString()), Featured = Featured };
            try
            {
                if (item.Id > 0)
                {
                    item.CreatedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    item.ModifiedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    dynamic UpdateFeatured = CategoriesProductsService.UpdateFeatured(item);
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
        public ActionResult UpdateStatus([FromQuery] string Ids, Boolean Status)
        {
            
            CategoriesProducts item = new CategoriesProducts() { Id = Int32.Parse(MyModels.Decode(Ids, this.controllerSecret).ToString()), Status = Status };
            try
            {
                if (item.Id > 0)
                {
                    item.CreatedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    item.ModifiedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    dynamic UpdateStatus = CategoriesProductsService.UpdateStatus(item);
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
