using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using API.Areas.Admin.Models.TaiLieuHop;
using API.Models;
using Newtonsoft.Json;
using API.Areas.Admin.Models.DocumentRefersCategories;
using DocumentFormat.OpenXml.InkML;
using System.Net.Http;
using API.Areas.Admin.Models.USUsers;

namespace API.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TaiLieuHopController : Controller
    {
        public IActionResult Index([FromQuery] SearchTaiLieuHop dto)
        {
            int TotalItems = 0;
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            dto.IdCoQuan = int.Parse(HttpContext.Request.Headers["IdCoQuan"]);
            int IdGroup = Int32.Parse(HttpContext.Session.GetString("IdGroup"));
            TaiLieuHopModel data = new TaiLieuHopModel() { SearchData = dto };
            data.ListItems = TaiLieuHopService.GetListPagination(data.SearchData, API.Models.Settings.SecretId + ControllerName);
            if (data.ListItems != null && data.ListItems.Count() > 0)
            {
                TotalItems = data.ListItems[0].TotalRows;
            }
            data.Pagination = new Models.Partial.PartialPagination() { CurrentPage = data.SearchData.CurrentPage, ItemsPerPage = data.SearchData.ItemsPerPage, TotalItems = TotalItems, QueryString = Request.QueryString.ToString() };
            data.ListItemsStatus = TaiLieuHopService.GetListItemsStatus();
            data.ListDocumentsCategories = DocumentRefersCategoriesService.GetListSelectItems();
            return View(data);
        }

        public IActionResult SaveItem(string Id = null)
        {
            int IdCoQuan = int.Parse(HttpContext.Request.Headers["IdCoQuan"]);
            int IdGroup = Int32.Parse(HttpContext.Session.GetString("IdGroup"));

            TaiLieuHopModel data = new TaiLieuHopModel();
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            int IdDC = Int32.Parse(MyModels.Decode(Id, API.Models.Settings.SecretId + ControllerName).ToString());
            data.SearchData = new SearchTaiLieuHop() { CurrentPage = 0, ItemsPerPage = 10, Keyword = "" };
            data.ListDocumentsCategories = DocumentRefersCategoriesService.GetListSelectItems();
            TaiLieuHop Item = new TaiLieuHop() {
                IssuedDateShow = DateTime.Now.ToString("dd/MM/yyyy"),
                Status=true,
                IdCoQuan = int.Parse(HttpContext.Request.Headers["IdCoQuan"])
            };
            if (IdDC > 0)
            {
                Item = TaiLieuHopService.GetItem(IdDC, IdCoQuan, API.Models.Settings.SecretId + ControllerName);
            }
            if (Item == null)
            {
                Item = new TaiLieuHop();
            }
            data.Item = Item;

            return View(data);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveItem(TaiLieuHop model)
        {
            int IdCoQuan = int.Parse(HttpContext.Request.Headers["IdCoQuan"]);
            int IdGroup = Int32.Parse(HttpContext.Session.GetString("IdGroup"));

            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            int IdDC = Int32.Parse(MyModels.Decode(model.Ids, API.Models.Settings.SecretId + ControllerName).ToString());
            TaiLieuHopModel data = new TaiLieuHopModel() { Item = model };
            data.ListDocumentsCategories = DocumentRefersCategoriesService.GetListSelectItems();
            if (ModelState.IsValid)
            {
                if (model.Id == IdDC)
                {
                    model.CreatedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    model.ModifiedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    if (model.Id == 0)
                    {
                        model.MatKhau = API.Models.MyHelper.StringHelper.RandomString(20);
                        model.Code = USUsersService.GetMD5(API.Models.MyHelper.StringHelper.RandomString(20)+ DateTime.Now.ToString("yyyyMMddhh"));
                        model.IdCoQuan = IdCoQuan;
                    }
                    try
                    {
                        string Domain = "";
                        string StrThongTinCoQuan = HttpContext.Session.GetString("ThongTinCoQuan");
                        API.Areas.Admin.Models.DMCoQuan.DMCoQuan ItemCoQuan = new API.Areas.Admin.Models.DMCoQuan.DMCoQuan();
                        ItemCoQuan = JsonConvert.DeserializeObject<API.Areas.Admin.Models.DMCoQuan.DMCoQuan>(StrThongTinCoQuan);
                        string version = DateTime.Now.ToString("yyyyMMddhh");
                        if (HttpContext.Request.IsHttps)
                        {
                            Domain = "https://" + ItemCoQuan.Code;
                        }
                        else
                        {
                            Domain = "http://" + ItemCoQuan.Code;
                        }

                        TaiLieuHopService.SaveItem(model, Domain);
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
                    catch
                    {
                        TempData["MessageError"] = "Lỗi khi lưu dữ liệu";
                    }

                }
            }
            return View(data);
        }

        [ValidateAntiForgeryToken]
        public ActionResult DeleteItem(string Id)
        {
			int IdCoQuan = int.Parse(HttpContext.Request.Headers["IdCoQuan"]);
			int IdGroup = Int32.Parse(HttpContext.Session.GetString("IdGroup"));

			string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            TaiLieuHop model = new TaiLieuHop() { Id = Int32.Parse(MyModels.Decode(Id, API.Models.Settings.SecretId + ControllerName).ToString()) };
            try
            {
                if (model.Id > 0)
                {
                    model.CreatedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    model.ModifiedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    model.IdCoQuan = IdCoQuan;
                    TaiLieuHopService.DeleteItem(model);
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
			int IdCoQuan = int.Parse(HttpContext.Request.Headers["IdCoQuan"]);
			int IdGroup = Int32.Parse(HttpContext.Session.GetString("IdGroup"));
			string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            TaiLieuHop item = new TaiLieuHop() { Id = Int32.Parse(MyModels.Decode(Ids, API.Models.Settings.SecretId + ControllerName).ToString()), Status = Status };
            try
            {
                if (item.Id > 0)
                {
                    item.CreatedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    item.ModifiedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    item.IdCoQuan = IdCoQuan;
                    dynamic UpdateStatus = TaiLieuHopService.UpdateStatus(item);
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
