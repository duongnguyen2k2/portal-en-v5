using API.Areas.Admin.Models.DMCoQuan;
using API.Areas.Admin.Models.USUsers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
namespace API.Controllers
{
    public class UsersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult BanLanhDao([FromQuery] SearchUSUsers dto)
        {
            int IdCoQuan = 1;
            if (HttpContext.Session.GetString("IdCoQuan") != null && HttpContext.Session.GetString("IdCoQuan") != "")
            {
                IdCoQuan = int.Parse(HttpContext.Session.GetString("IdCoQuan"));
            }

            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            dto.IdCoQuan = IdCoQuan;
            dto.IdGroup = 6;
            USUsersModel data = new USUsersModel() { SearchData = dto };            
            data.ListItems = USUsersService.GetAllListUsersByGroup(dto, ControllerName);

            return View(data);
        }

        public IActionResult CoCauHanhChinh([FromQuery] SearchUSUsers dto)
        {
            int IdCoQuan = 1;
            if (HttpContext.Session.GetString("IdCoQuan") != null && HttpContext.Session.GetString("IdCoQuan") != "")
            {
                IdCoQuan = int.Parse(HttpContext.Session.GetString("IdCoQuan"));
            }

            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            dto.IdCoQuan = IdCoQuan;
            dto.IdGroup = 7;
            USUsersModel data = new USUsersModel() { SearchData = dto };
            data.ListItems = USUsersService.GetAllListUsersByGroup(dto, ControllerName);

            return View(data);
        }

        [HttpPost]
        public IActionResult GetListHome([FromBody] SearchUSUsers dto)
        {

            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            USUsersModel data = new USUsersModel() { SearchData = dto };
            List<DMCoQuan> ListDMCoQuan = DMCoQuanService.GetListByParent(dto.IdLevel);
            data.ListItems = USUsersService.GetAllListUsersByCoQuan(data.SearchData, API.Models.Settings.SecretId + ControllerName);
            return Json(new API.Models.MsgSuccess() { Data = new { ListItems = data.ListItems, ListDMCoQuan  = ListDMCoQuan } });
        }

        [HttpPost]
        public IActionResult GetListItems([FromBody] SearchUSUsers dto)
        {
            
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            USUsersModel data = new USUsersModel() { SearchData = dto };  
            data.ListItems = USUsersService.GetAllListUsersByCoQuan(data.SearchData, API.Models.Settings.SecretId + ControllerName);
            return Json(new API.Models.MsgSuccess(){ Data  = new { ListItems  = data.ListItems } });
        }
    }
}
