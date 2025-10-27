using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using API.Areas.Admin.Models.DMPhuongXa;
using API.Areas.Admin.Models.DMQuanHuyen;
using API.Areas.Admin.Models.DMTinhThanh;
using API.Models;
using Newtonsoft.Json;

namespace API.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DMPhuongXaController : Controller
    {
        public IActionResult Index([FromQuery] SearchDMPhuongXa dto)
        {
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            int TotalItems = 0;
            DMPhuongXaModel data = new DMPhuongXaModel();
            data.SearchData = dto;
            HttpContext.Session.SetString(ControllerName + "_Search", JsonConvert.SerializeObject(data.SearchData));
            data.ListItems = DMPhuongXaService.GetListPagination(data.SearchData, API.Models.Settings.SecretId + ControllerName);            
            data.ListItemsTinhThanh = DMTinhThanhService.GetListSelectItems();
            data.ListItemsQuanHuyen = DMQuanHuyenService.GetListSelectItems(data.SearchData.IdTinhThanh);
            if (data.ListItems != null && data.ListItems.Count() > 0)
            {
                TotalItems = data.ListItems[0].TotalRows;
            }
            data.Pagination = new Models.Partial.PartialPagination() { CurrentPage = data.SearchData.CurrentPage, ItemsPerPage = data.SearchData.ItemsPerPage, TotalItems = TotalItems, QueryString = Request.QueryString.ToString() };
            return View(data);
        }
        public IActionResult GetListJson([FromQuery] int IdQuanHuyen)
        {
            List<DMPhuongXa> ListItems = DMPhuongXaService.GetListByQuanHuyen(IdQuanHuyen);
            return Json(new MsgSuccess() { Data = ListItems });
        }
        /*
        public IActionResult SaveItem(string Id = null)
        {
            DMPhuongXaModel data = new DMPhuongXaModel();
            int IdDC = Int32.Parse(MyModels.Decode(Id, API.Models.Settings.SecretId + "DMPhuongXa").ToString());
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            data.SearchData = new SearchDMPhuongXa() { CurrentPage = 0, ItemsPerPage = 10, Keyword = "" };
            if (HttpContext.Session.GetString(ControllerName + "_Search") != null)
            {
                data.SearchData = JsonConvert.DeserializeObject<SearchDMPhuongXa>(HttpContext.Session.GetString(ControllerName + "_Search"));
            }
            data.Item = DMPhuongXaService.GetItem(IdDC, API.Models.Settings.SecretId + ControllerName);
            return View(data);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SaveItem(DMPhuongXa model)
        {
            if (!ModelState.IsValid)
            {

                return View(model);
            }
            else
            {
                TempData["MessageSuccess"] = "Cập nhật thành công";
                return RedirectToAction("Index");

            }

        }*/
    }
}