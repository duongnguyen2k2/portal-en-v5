using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using API.Areas.Admin.Models.CategoriesAblums;
using API.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Hosting;
using API.Areas.Admin.Models.Ablums;
using API.Areas.Admin.Models.USUsers;
using API.Areas.Admin.Models.DMCoQuan;
//using System.Web.Http;

namespace API.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoriesAblumsController : Controller
    {
        public IActionResult Index([FromQuery] SearchCategoriesAblums dto)
        {
            var Login = HttpContext.Session.GetString("Login");
            USUsers MyInfo = JsonConvert.DeserializeObject<USUsers>(Login);
            if (dto.IdCoQuan == 0) { dto.IdCoQuan = MyInfo.IdCoQuan; }
            int TotalItems = 0;
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            CategoriesAblumsModel data = new CategoriesAblumsModel() { SearchData = dto };
            data.ListItems = CategoriesAblumsService.GetListPagination(data.SearchData, API.Models.Settings.SecretId + ControllerName);
            if (data.ListItems != null && data.ListItems.Count() > 0)
            {
                TotalItems = data.ListItems[0].TotalRows;
            }
            data.ListDMCoQuan = DMCoQuanService.GetListByLoaiCoQuan(0, 0, MyInfo.IdCoQuan);
            data.Pagination = new Models.Partial.PartialPagination() { CurrentPage = data.SearchData.CurrentPage, ItemsPerPage = data.SearchData.ItemsPerPage, TotalItems = TotalItems, QueryString = Request.QueryString.ToString() };

            return View(data);
        }
        
        public IActionResult SaveItem(string Id=null)
        {
            var Login = HttpContext.Session.GetString("Login");
            USUsers MyInfo = JsonConvert.DeserializeObject<USUsers>(Login);
            CategoriesAblumsModel data = new CategoriesAblumsModel();
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            int IdDC = Int32.Parse(MyModels.Decode(Id, API.Models.Settings.SecretId + ControllerName).ToString());            
            data.SearchData = new SearchCategoriesAblums() { CurrentPage = 0, ItemsPerPage = 10, Keyword = ""};
            data.ListItemsParents = CategoriesAblumsService.GetListItems();
            if (IdDC == 0)
            {
                data.Item = new CategoriesAblums() { IdCoQuan = MyInfo.IdCoQuan };
            }
            else {
                data.Item = CategoriesAblumsService.GetItem(IdDC, API.Models.Settings.SecretId + ControllerName);
                data.ListItemsAlbums = AblumsService.GetList(IdDC, API.Models.Settings.SecretId + ControllerName);
            }            
            return View(data);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveItem(CategoriesAblums model)
        {
            var Login = HttpContext.Session.GetString("Login");
            USUsers MyInfo = JsonConvert.DeserializeObject<USUsers>(Login);
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            int IdDC = Int32.Parse(MyModels.Decode(model.Ids, API.Models.Settings.SecretId + ControllerName).ToString());
            CategoriesAblumsModel data = new CategoriesAblumsModel() { Item = model };

            if (model.Id == IdDC)
            {
                model.CreatedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                model.ModifiedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                if (model.Id == 0)
                {
                    model.IdCoQuan = MyInfo.IdCoQuan;
                }
                var Cat = CategoriesAblumsService.SaveItem(model);
                if (model.ListItemsAlbums != null && model.ListItemsAlbums.Count() > 0)
                {
                    var updateCat = AblumsService.UpdateStatusCat(Cat.N);
                    for (int i = 0; i < model.ListItemsAlbums.Count(); i++)
                    {
                        try
                        {
                            model.ListItemsAlbums[i].IdCoQuan = model.IdCoQuan;
                            model.ListItemsAlbums[i].CatId = Cat.N;
                            if (model.ListItemsAlbums[i].Title == null || model.ListItemsAlbums[i].Title == "")
                            {
                                model.ListItemsAlbums[i].Title = model.Title;
                            }
                            if (model.ListItemsAlbums[i].Deleted == false)
                            {
                                AblumsService.SaveItem(model.ListItemsAlbums[i]);
                            }
                        }
                        catch
                        {

                        }
                    }
                }


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
          
            return View(data);
        }
        
        [ValidateAntiForgeryToken]
        public ActionResult DeleteItem(string Id)
        {
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            CategoriesAblums model = new CategoriesAblums() { Id = Int32.Parse(MyModels.Decode(Id, API.Models.Settings.SecretId + ControllerName).ToString()) };            
            try
            {
                if (model.Id > 0)
                {
                    model.CreatedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    model.ModifiedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    model.IdCoQuan = int.Parse(HttpContext.Request.Headers["IdCoQuan"]);

                    CategoriesAblumsService.DeleteItem(model);
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
            CategoriesAblums item = new CategoriesAblums() { Id = Int32.Parse(MyModels.Decode(Ids, API.Models.Settings.SecretId + ControllerName).ToString()), Status = Status };
            try
            {
                if (item.Id > 0)
                {
                    item.CreatedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    item.ModifiedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    dynamic UpdateStatus = CategoriesAblumsService.UpdateStatus(item);
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
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            CategoriesAblums item = new CategoriesAblums() { Id = Int32.Parse(MyModels.Decode(Ids, API.Models.Settings.SecretId + ControllerName).ToString()), Featured = Featured };
            try
            {
                if (item.Id > 0)
                {
                    item.CreatedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    item.ModifiedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    dynamic UpdateFeatured = CategoriesAblumsService.UpdateFeatured(item);
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

        public async Task<IActionResult> UPloadFiles()
        {           
            var form = Request.Form;            
            
            CategoriesAblums model = new CategoriesAblums() {
                Id = Convert.ToInt32(form["id"]),
                Ids = form["ids"],
                Status = Convert.ToBoolean(form["status"]),
                Description = form["desc"],
                Title = form["ten"],
                Images = form["Images"],
                Alias = form["Alias"],
                Featured = Boolean.Parse(form["Featured"]),
                ParentId = int.Parse(form["ParentId"]),
            };
            //ICollection<string> col = form.Keys;           
            
         
            var datafiles = JsonConvert.DeserializeObject<List<Ablums>>(form["data"]);
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            int IdDC = Int32.Parse(MyModels.Decode(model.Ids, API.Models.Settings.SecretId + ControllerName).ToString());
            //CategoriesAblumsModel data = new CategoriesAblumsModel() { Item = model };

            if (ModelState.IsValid)
            {
                if (model.Id == IdDC)
                {
                    model.CreatedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    model.ModifiedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    var Cat = CategoriesAblumsService.SaveItem(model);

                    foreach (IFormFile source in form.Files)
                    {
                        string filename = ContentDispositionHeaderValue.Parse(source.ContentDisposition).FileName.Trim('"');

                        filename = this.EnsureCorrectFilename(filename);

                        using (FileStream output = System.IO.File.Create(this.GetPathAndFilename(filename)))
                        {
                            await source.CopyToAsync(output);
                            Ablums alb = new Ablums();
                            alb.CatId = Convert.ToInt32(Cat.N);
                            alb.CreatedBy = model.CreatedBy;
                            alb.ModifiedBy = model.ModifiedBy;
                            alb.Images = filename;
                            AblumsService.SaveItem(alb);
                        }

                    }
                    if (datafiles != null)
                    {
                        for (var i = 0; i != datafiles.Count(); i++)
                        {
                            var item = datafiles[i];
                            if (item.Deleted == true) {
                                var fullPath = GetPathAndFilename(item.Images.ToString());
                                if (System.IO.File.Exists(fullPath))
                                {
                                    System.IO.File.Delete(fullPath);
                                    
                                }
                                Ablums alb = new Ablums() {
                                    Id = Convert.ToInt32(item.Id),
                                    ModifiedBy = model.ModifiedBy
                                };                            
                                AblumsService.DeleteItem(alb);
                            }
                            
                        }
                    }
                    TempData["MessageSuccess"] = "Cập nhật thành công";
                    return RedirectToAction("Index");
                }
            }

            return Json("asdfsd");
        }
        private string EnsureCorrectFilename(string filename)
        {
            if (filename.Contains("\\"))
                filename = filename.Substring(filename.LastIndexOf("\\") + 1);

            return filename;
        }
        //private readonly IHostingEnvironment _hostingEnvironment;
        private string GetPathAndFilename(string filename)
        {
            string filePath = Path.Combine("wwwroot", "Albums");
            return filePath + "\\" + filename;
        }
    }
}
