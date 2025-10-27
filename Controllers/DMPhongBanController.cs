using API.Areas.Admin.Models.Articles;
using API.Areas.Admin.Models.DMPhongBan;
using API.Areas.Admin.Models.USUsers;
using API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Linq;
namespace API.Controllers
{
    public class DMPhongBanController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult DanhBaDienTu([FromQuery] SearchDMPhongBan dto)
        {
            int IdCoQuan = 1;
            int TotalItems = 0;
            if (HttpContext.Session.GetString("IdCoQuan") != null && HttpContext.Session.GetString("IdCoQuan") != "")
            {
                IdCoQuan = int.Parse(HttpContext.Session.GetString("IdCoQuan"));
            }
            dto.Status = true;
            dto.IdCoQuan = IdCoQuan;
            DMPhongBanModel data = new DMPhongBanModel() { SearchData = dto };

            data.ListItems = DMPhongBanService.GetListPagination(data.SearchData, "d");
            if (data.ListItems != null && data.ListItems.Count() > 0)
            {
                TotalItems = data.ListItems[0].TotalRows;
            }
            data.Pagination = new Areas.Admin.Models.Partial.PartialPagination() { CurrentPage = data.SearchData.CurrentPage, ItemsPerPage = data.SearchData.ItemsPerPage, TotalItems = TotalItems, QueryString = Request.QueryString.ToString() };
            data.ListItemsDanhMuc = DMPhongBanService.GetListTypes();
            return View(data);
        }
    }
}
