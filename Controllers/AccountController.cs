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
    
    public class AccountController : Controller
    {
        private string controllerName = "AccountController";
        private string controllerSecret;
        public AccountController(IConfiguration config)
        {
            controllerSecret = config["Security:SecretId"] + controllerName;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult HopTrucTuyen([FromQuery] SearchHopTrucTuyen dto)
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

        public IActionResult HopTrucTuyenDetail(string alias, int id)
        {
            int IdCoQuan = 1;
            if (HttpContext.Session.GetString("IdCoQuan") != null && HttpContext.Session.GetString("IdCoQuan") != "")
            {
                IdCoQuan = int.Parse(HttpContext.Session.GetString("IdCoQuan"));
            }
            HopTrucTuyenModel data = new HopTrucTuyenModel();            
            data.SearchData = new SearchHopTrucTuyen() { CurrentPage = 0, ItemsPerPage = 10, Keyword = "" };            
            data.Item = HopTrucTuyenService.GetItem(id, controllerSecret);            
            return View(data);
        }

    }
}
