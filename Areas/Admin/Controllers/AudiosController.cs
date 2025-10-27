using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using API.Areas.Admin.Models.Audios;
using API.Models;
using Newtonsoft.Json;
using API.Areas.Admin.Models.USUsers;
using Microsoft.Extensions.Configuration;
using API.Areas.Admin.Models.CategoriesAudios;

namespace API.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AudiosController : Controller
    {
        private string controllerName = "AudiosController";
        private string controllerSecret;
        public AudiosController(IConfiguration config)
        {

            controllerSecret = config["Security:SecretId"] + controllerName;
        }

        public IActionResult Index([FromQuery] SearchAudios dto)
        {
            var Login = HttpContext.Session.GetString("Login");
            USUsers MyInfo = JsonConvert.DeserializeObject<USUsers>(Login);
            int TotalItems = 0;
			dto.IdCoQuan = int.Parse(HttpContext.Request.Headers["IdCoQuan"]);
			AudiosModel data = new AudiosModel() { SearchData = dto};
            data.ListItems = AudiosService.GetListPagination(data.SearchData, this.controllerSecret);            
            if (data.ListItems != null && data.ListItems.Count() > 0)
            {
                TotalItems = data.ListItems[0].TotalRows;
            }
            
            data.Pagination = new Models.Partial.PartialPagination() { CurrentPage = data.SearchData.CurrentPage, ItemsPerPage = data.SearchData.ItemsPerPage, TotalItems = TotalItems, QueryString = Request.QueryString.ToString() };
            data.ListItemsCat = CategoriesAudiosService.GetListItems();
            return View(data);
        }

        public IActionResult SaveItem(string Id=null)
        {
            var Login = HttpContext.Session.GetString("Login");
            USUsers MyInfo = JsonConvert.DeserializeObject<USUsers>(Login);
            AudiosModel data = new AudiosModel() {  };
            
            int IdDC = Int32.Parse(MyModels.Decode(Id, this.controllerSecret).ToString());            
            data.SearchData = new SearchAudios() { CurrentPage = 0, ItemsPerPage = 10, Keyword = ""};
            data.ListItemsCat = CategoriesAudiosService.GetListItems();
            if (IdDC == 0)
            {
                data.Item = new Audios() { CatId = 0 };
            }
            else {
                data.Item = AudiosService.GetItem(IdDC, this.controllerSecret);
            }
            
           
            return View(data);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveItem(Audios model)
        {
            var Login = HttpContext.Session.GetString("Login");
            USUsers MyInfo = JsonConvert.DeserializeObject<USUsers>(Login);

            
            int IdDC = Int32.Parse(MyModels.Decode(model.Ids, this.controllerSecret).ToString());
            AudiosModel data = new AudiosModel() { Item = model};
            data.ListItemsCat = CategoriesAudiosService.GetListItems();
            if (ModelState.IsValid)
            {
                if (model.Id == IdDC)
                {
					model.IdCoQuan = int.Parse(HttpContext.Request.Headers["IdCoQuan"]);
					model.CreatedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    model.ModifiedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    model.Alias = API.Models.MyHelper.StringHelper.UrlFriendly(model.Title);

                    AudiosService.SaveItem(model);
                    if (model.Id > 0)
                    {
                        TempData["MessageSuccess"] = "Cập nhật Truyền Thanh thành công";
                    }
                    else {
                        TempData["MessageSuccess"] = "Thêm mới Truyền Thanh thành công";
                    }
                    return RedirectToAction("Index");
                }
            }
            return View(data);
        }
        
        [ValidateAntiForgeryToken]
        public ActionResult DeleteItem(string Id)
        {
            
            Audios model = new Audios() { Id = Int32.Parse(MyModels.Decode(Id, this.controllerSecret).ToString()) };            
            try
            {
                if (model.Id > 0)
                {
                    model.CreatedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    model.ModifiedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    AudiosService.DeleteItem(model);
                    TempData["MessageSuccess"] = "Xóa thành công";
                    return Json(new MsgSuccess());
                }
                else {
                    TempData["MessageError"] = "Xóa Truyền Thanh Không thành công";
                    return Json(new MsgError());
                }
                
            }
            catch {
                TempData["MessageSuccess"] = "Xóa Truyền Thanh không thành công";
                return Json(new MsgError());
            }
            

        }
		
		[ValidateAntiForgeryToken]
        public ActionResult UpdateStatus([FromQuery] string Ids, Boolean Status)
        {
            
            Audios item = new Audios() { Id = Int32.Parse(MyModels.Decode(Ids, this.controllerSecret).ToString()), Status = Status };
            try
            {
                if (item.Id > 0)
                {
                    item.CreatedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    item.ModifiedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    dynamic UpdateStatus = AudiosService.UpdateStatus(item);
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
            
            Audios item = new Audios() { Id = Int32.Parse(MyModels.Decode(Ids, this.controllerSecret).ToString()), Featured = Featured };
            try
            {
                if (item.Id > 0)
                {
                    item.CreatedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    item.ModifiedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    dynamic UpdateFeatured = AudiosService.UpdateFeatured(item);
                    TempData["MessageSuccess"] = "Cập nhật Nổi Bật thành công";
                    return Json(new MsgSuccess());
                }
                else
                {
                    TempData["MessageError"] = "Cập nhật Nổi Bật Không thành công";
                    return Json(new MsgError());
                }
            }
            catch
            {
                TempData["MessageError"] = "Cập nhật Nổi Bật không thành công";
                return Json(new MsgError());
            }
        }

    }
}
