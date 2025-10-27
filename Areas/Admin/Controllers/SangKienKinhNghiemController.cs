using API.Areas.Admin.Models.SangKienKinhNghiem;
using API.Areas.Admin.Models.DMCoQuan;
using API.Areas.Admin.Models.USUsers;
using API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Linq;
using API.Areas.Admin.Models.DocumentsField;
using API.Areas.Admin.Models.DocumentsLevel;

namespace API.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SangKienKinhNghiemController : Controller
    {
        public IActionResult Index([FromQuery] SearchSangKienKinhNghiem dto)
        {
            var Login = HttpContext.Session.GetString("Login");
            USUsers MyInfo = JsonConvert.DeserializeObject<USUsers>(Login);
            int TotalItems = 0;
            if (dto.IdCoQuan == 0) { dto.IdCoQuan = MyInfo.IdCoQuan; }
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            SangKienKinhNghiemModel data = new SangKienKinhNghiemModel() { SearchData = dto };
            data.ListItems = SangKienKinhNghiemService.GetListPagination(data.SearchData, API.Models.Settings.SecretId + ControllerName);
            if (data.ListItems != null && data.ListItems.Count() > 0)
            {
                TotalItems = data.ListItems[0].TotalRows;
            }

            data.ListDMCoQuan = DMCoQuanService.GetListByLoaiCoQuan(0, 0, MyInfo.IdCoQuan, false);
            data.Pagination = new Models.Partial.PartialPagination() { CurrentPage = data.SearchData.CurrentPage, ItemsPerPage = data.SearchData.ItemsPerPage, TotalItems = TotalItems, QueryString = Request.QueryString.ToString() };
            data.ListLinhVuc = DocumentsFieldService.GetListSelectItems();
            data.ListCapQuanLy = DocumentsLevelService.GetListSelectItems();
            return View(data);
        }

        public IActionResult SaveItem(string Id = null, int LinhVuc = 1, int IdCoQuan = 1, int CapQuanLy = 1)
        {

            SangKienKinhNghiemModel data = new SangKienKinhNghiemModel();
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            int IdDC = Int32.Parse(MyModels.Decode(Id, API.Models.Settings.SecretId + ControllerName).ToString());
            data.SearchData = new SearchSangKienKinhNghiem() { CurrentPage = 0, ItemsPerPage = 10, Keyword = "", LinhVuc = LinhVuc, IdCoQuan = IdCoQuan, CapQuanLy = CapQuanLy };
            data.ListLinhVuc = DocumentsFieldService.GetListSelectItems();
            data.ListCapQuanLy = DocumentsLevelService.GetListSelectItems();
            data.ListDMCoQuan = DMCoQuanService.GetListByParent(1, int.Parse(HttpContext.Request.Headers["IdCoQuan"]));
            if (IdDC == 0)
            {
                data.Item = new SangKienKinhNghiem() { LinhVuc = LinhVuc, CapQuanLy = CapQuanLy };
            }
            else
            {
                data.Item = SangKienKinhNghiemService.GetItem(IdDC, API.Models.Settings.SecretId + ControllerName);
            }


            return View(data);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveItem(SangKienKinhNghiem model)
        {
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            int IdDC = Int32.Parse(MyModels.Decode(model.Ids, API.Models.Settings.SecretId + ControllerName).ToString());
            SangKienKinhNghiemModel data = new SangKienKinhNghiemModel() { Item = model };

            if (ModelState.IsValid)
            {
                if (model.Id == IdDC)
                {
                    model.CreatedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    model.UpdatedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    model.CreatedAt = DateTime.Now;
                    model.UpdatedAt = DateTime.Now;
                    SangKienKinhNghiemService.SaveItem(model);
                    if (model.Id > 0)
                    {
                        TempData["MessageSuccess"] = "Cập nhật thành công";
                    }
                    else
                    {
                        TempData["MessageSuccess"] = "Thêm mới thành công";
                    }
                    return RedirectToAction("Index", new { LinhVuc = model.LinhVuc, CapQuanLy = model.CapQuanLy, IdCoQuan = int.Parse(HttpContext.Request.Headers["IdCoQuan"]) });
                }
            }
            else
            {
                data.ListLinhVuc = DocumentsFieldService.GetListSelectItems();
                data.ListCapQuanLy = DocumentsLevelService.GetListSelectItems();
                data.ListDMCoQuan = DMCoQuanService.GetListByParent(1, int.Parse(HttpContext.Request.Headers["IdCoQuan"]));
            }
            return View(data);
        }

        [ValidateAntiForgeryToken]
        public ActionResult DeleteItem(string Id)
        {
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            SangKienKinhNghiem model = new SangKienKinhNghiem() { Id = Int32.Parse(MyModels.Decode(Id, API.Models.Settings.SecretId + ControllerName).ToString()) };
            try
            {
                if (model.Id > 0)
                {
                    model.CreatedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    model.UpdatedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    SangKienKinhNghiemService.DeleteItem(model);
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
            SangKienKinhNghiem item = new SangKienKinhNghiem() { Id = Int32.Parse(MyModels.Decode(Ids, API.Models.Settings.SecretId + ControllerName).ToString()), Status = Status };
            try
            {
                if (item.Id > 0)
                {
                    item.CreatedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    item.UpdatedBy = int.Parse(HttpContext.Request.Headers["Id"]);
                    dynamic UpdateStatus = SangKienKinhNghiemService.UpdateStatus(item);
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
