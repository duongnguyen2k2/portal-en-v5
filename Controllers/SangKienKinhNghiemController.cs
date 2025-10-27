using API.Areas.Admin.Models.DMCoQuan;
using API.Areas.Admin.Models.DocumentsField;
using API.Areas.Admin.Models.DocumentsLevel;
using API.Areas.Admin.Models.SangKienKinhNghiem;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace API.Controllers
{
    public class SangKienKinhNghiemController : Controller
    {
        public IActionResult Index([FromQuery] SearchSangKienKinhNghiem dto)
        {
            int TotalItems = 0;
            int IdCoQuan = 1;
            if (HttpContext.Session.GetString("IdCoQuan") != null && HttpContext.Session.GetString("IdCoQuan") != "")
            {
                IdCoQuan = int.Parse(HttpContext.Session.GetString("IdCoQuan"));
            }
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            SangKienKinhNghiemModel data = new SangKienKinhNghiemModel() { SearchData = dto };
            data.ListItems = SangKienKinhNghiemService.GetListPagination(data.SearchData, API.Models.Settings.SecretId + ControllerName);
            if (data.ListItems != null && data.ListItems.Count() > 0)
            {
                TotalItems = data.ListItems[0].TotalRows;
            }

            data.ListDMCoQuan = DMCoQuanService.GetListByLoaiCoQuan(0, 0, IdCoQuan, false);
            data.Pagination = new Areas.Admin.Models.Partial.PartialPagination() { CurrentPage = data.SearchData.CurrentPage, ItemsPerPage = data.SearchData.ItemsPerPage, TotalItems = TotalItems, QueryString = Request.QueryString.ToString() };
            data.ListLinhVuc = DocumentsFieldService.GetListSelectItems();
            data.ListCapQuanLy = DocumentsLevelService.GetListSelectItems();
            return View(data);
        }
    }
}
