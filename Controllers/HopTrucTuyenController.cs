using API.Areas.Admin.Models.HopTrucTuyen;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class HopTrucTuyenController : Controller
    {
        private string controllerName = "AccountController";
        private string controllerSecret;
        public HopTrucTuyenController(IConfiguration config)
        {
            controllerSecret = config["Security:SecretId"] + controllerName;
        }
        public IActionResult TaiLieu(int Id, [FromQuery] string Code)
        {
            HopTrucTuyen Item = new HopTrucTuyen();
            if(Code!=null && Code != "")
            {
                Item = HopTrucTuyenService.GetItemByCode(Id, Code.Trim());
            }
            HopTrucTuyenModel data = new HopTrucTuyenModel() { Item = Item };
            return View(data);
        }


        public IActionResult Index(string alias, int id, [FromQuery] SearchHopTrucTuyen dto)
        {
            int TotalItems = 0;
            dto.IdCoQuan = id;
            dto.Status = 1;
            HopTrucTuyenModel data = new HopTrucTuyenModel() { SearchData = dto };

            data.ListItems = HopTrucTuyenService.GetListPagination(data.SearchData, controllerSecret);
            if (data.ListItems != null && data.ListItems.Count() > 0)
            {
                TotalItems = data.ListItems[0].TotalRows;
            }
            data.Pagination = new Areas.Admin.Models.Partial.PartialPagination() { CurrentPage = data.SearchData.CurrentPage, ItemsPerPage = data.SearchData.ItemsPerPage, TotalItems = TotalItems, QueryString = Request.QueryString.ToString() };

            return View(data);
        }
        public IActionResult PhongGiaoDuc([FromQuery] SearchHopTrucTuyen dto)
        {
            int TotalItems = 0;
            int IdCoQuan = 1;
            if (HttpContext.Session.GetString("IdCoQuan") != null && HttpContext.Session.GetString("IdCoQuan") != "")
            {
                IdCoQuan = int.Parse(HttpContext.Session.GetString("IdCoQuan"));
            }

            dto.IdCoQuan = IdCoQuan;
            dto.ShowStartDate = "01/01/2010";
            dto.Status = 1;
            HopTrucTuyenModel data = new HopTrucTuyenModel() { SearchData = dto };

            data.ListItems = HopTrucTuyenService.GetListPagination(data.SearchData, controllerSecret);
            if (data.ListItems != null && data.ListItems.Count() > 0)
            {
                TotalItems = data.ListItems[0].TotalRows;
            }
            data.Pagination = new Areas.Admin.Models.Partial.PartialPagination() { CurrentPage = data.SearchData.CurrentPage, ItemsPerPage = data.SearchData.ItemsPerPage, TotalItems = TotalItems, QueryString = Request.QueryString.ToString() };

            return View(data);
        }
    }
}
