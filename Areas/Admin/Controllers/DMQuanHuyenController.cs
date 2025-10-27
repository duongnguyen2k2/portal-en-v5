using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using API.Areas.Admin.Models.DMQuanHuyen;
using API.Areas.Admin.Models.DMTinhThanh;
using API.Models;
using Newtonsoft.Json;

namespace API.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DMQuanHuyenController : Controller
    {
        public IActionResult Index([FromQuery] SearchDMQuanHuyen dto)
        {
            int TotalItems = 0;
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            DMQuanHuyenModel data = new DMQuanHuyenModel();
            data.SearchData = dto;
            HttpContext.Session.SetString(ControllerName + "_Search", JsonConvert.SerializeObject(data.SearchData));
            data.ListItems = DMQuanHuyenService.GetListPagination(data.SearchData, API.Models.Settings.SecretId + ControllerName);
            data.ListItemsTinhThanh = DMTinhThanhService.GetListSelectItems();            
            if (data.ListItems != null && data.ListItems.Count() > 0)
            {
                TotalItems = data.ListItems[0].TotalRows;
            }
            data.Pagination = new Models.Partial.PartialPagination() { CurrentPage = data.SearchData.CurrentPage, ItemsPerPage = data.SearchData.ItemsPerPage, TotalItems = TotalItems,QueryString = Request.QueryString.ToString() };
            
            return View(data);
        }

        public IActionResult SaveItem(string Id = null)
        {
            DMQuanHuyenModel data = new DMQuanHuyenModel();
            int IdDC = Int32.Parse(MyModels.Decode(Id, API.Models.Settings.SecretId + "DMQuanHuyen").ToString());            
            data.Item = DMQuanHuyenService.GetItem(IdDC, API.Models.Settings.SecretId + "DMQuanHuyen".ToString());
            return View(data);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SaveItem(DMQuanHuyen model)
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

        }

        public IActionResult GetListByJson([FromQuery]int IdTinhThanh = 0)
        {
            List<DMQuanHuyen> ListItems = DMQuanHuyenService.GetListByTinhThanh(true, IdTinhThanh);
            return Json(ListItems);
        }
    }
}