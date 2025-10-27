using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using API.Areas.Admin.Models.DMTinhThanh;
using API.Models;
using Newtonsoft.Json;

namespace API.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DMTinhThanhController : Controller
    {
        public IActionResult Index()
        {
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            DMTinhThanhModel data = new DMTinhThanhModel() {
                SearchData = new SearchDMTinhThanh() { CurrentPage = 0, ItemsPerPage = 10, Keyword = "" }
            };
            
            HttpContext.Session.SetString(ControllerName + "_Search", JsonConvert.SerializeObject(data.SearchData));
            data.ListItems = DMTinhThanhService.GetListPagination(data.SearchData, API.Models.Settings.SecretId + "DMTinhThanh");
            return View(data);
        }

        public IActionResult SaveItem(string Id = null)
        {
            DMTinhThanhModel data = new DMTinhThanhModel();

            int IdDC = Int32.Parse(MyModels.Decode(Id, API.Models.Settings.SecretId + "DMTinhThanh").ToString());
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            data.SearchData = new SearchDMTinhThanh() { CurrentPage = 0, ItemsPerPage = 10, Keyword = "" };
            if (HttpContext.Session.GetString(ControllerName + "_Search") != null)
            {
                data.SearchData = JsonConvert.DeserializeObject<SearchDMTinhThanh>(HttpContext.Session.GetString(ControllerName + "_Search"));
            }
            data.Item = DMTinhThanhService.GetItem(IdDC, API.Models.Settings.SecretId + "DMTinhThanh".ToString());
            return View(data);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SaveItem(DMTinhThanh model)
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
    }
}