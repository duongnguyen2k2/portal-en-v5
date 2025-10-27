using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Areas.Admin.Models.Dashboard;
using API.Areas.Admin.Models.SYSParams;
using API.Models;
using API.Models.Home;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
namespace API.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        private string controllerName = "HomeController";
        private string controllerSecret;
        private IConfiguration Configuration;
        public HomeController(IConfiguration config)
        {
            controllerSecret = config["Security:SecretId"] + controllerName;
            Configuration = config;
        }


        [HttpGet]
        public IActionResult BieuDoCot(ReportArticle dto)
        {
            dto.IdCoQuan = int.Parse(HttpContext.Request.Headers["IdCoQuan"]);
            dto.ShowStartDate = dto.Nam.ToString() + "0101";
            dto.ShowEndDate = dto.Nam.ToString() + "1231";
            var Data = DashboardService.BieuDoCot(dto);
            return Json(new MsgSuccess() { Data = Data });
        }

        [HttpGet]
        public IActionResult BieuDoLuotTruyCapCot(ReportArticle dto)
        {
            dto.IdCoQuan = int.Parse(HttpContext.Request.Headers["IdCoQuan"]);
            dto.ShowStartDate = dto.Nam.ToString() + "0101";
            dto.ShowEndDate = dto.Nam.ToString() + "1231";
            var Data = DashboardService.BieuDoLuotTruyCapCot(dto);
            return Json(new MsgSuccess() { Data = Data });
        }
        [HttpPost]
        public IActionResult ExportCountArticles([FromBody] ReportArticle dto)
        {
            dto.IdCoQuan = int.Parse(HttpContext.Request.Headers["IdCoQuan"]);
            
            string TenFileLuu = DashboardService.ExportCountArticles(dto);
            return Json(new MsgSuccess() { Data = TenFileLuu });
        }


        [HttpGet]
        public IActionResult GetReportArticle()
        {
            int IdCoQuan = int.Parse(HttpContext.Request.Headers["IdCoQuan"]);
            var Data = DashboardService.GetReportMonth(IdCoQuan);
            return Json(new MsgSuccess() { Data = Data, Msg = "Cơ Quan cha =" + IdCoQuan });
        }

        [HttpPost]
        public IActionResult GetListCountVisit([FromBody] ReportArticle dto)
        {
            dto.IdCoQuan = int.Parse(HttpContext.Request.Headers["IdCoQuan"]);
            var Data = DashboardService.GetListCountVisit(dto);
            return Json(new MsgSuccess() { Data = Data, Msg = "Cơ Quan cha =" + dto.IdCoQuan });
        }
        [HttpPost]
        public IActionResult GetListCountArticles([FromBody] ReportArticle dto)
        {
            dto.IdCoQuan = int.Parse(HttpContext.Request.Headers["IdCoQuan"]);
            var Data = DashboardService.GetListCountArticles(dto);
            return Json(new MsgSuccess() { Data = Data , Msg = "Cơ Quan cha ="+ dto.IdCoQuan });
        }

        public IActionResult Index()
        {
            HomeModel data = new HomeModel();
            data.ListItemsNam = new List<SelectListItem>();
            int k = 0;
            for(int i= 2015; i < int.Parse(DateTime.Now.AddYears(+1).ToString("yyyy")); i++)
            {
                data.ListItemsNam.Insert(k, (new SelectListItem { Text = "Năm "+i.ToString(), Value = i.ToString() }));
                k++;
            }
            return View(data);
        }

        [HttpGet]
        public IActionResult SetSeccionMenu(int Id)
        {
            HttpContext.Session.SetInt32("IdMenu", Id);
            return Json(new MsgSuccess() { });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SaveItem(SYSConfig model)
        {
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();            
            SYSConfigModel data = new SYSConfigModel() { Item = model };
            if (ModelState.IsValid)
            {                
                SYSParamsService.SaveConfig(model);
                TempData["MessageSuccess"] = "Cập nhật thành công";
                return RedirectToAction("SaveItem");
            }
            return View(data);
        }

        public IActionResult SaveItem()
        {
            SYSConfigModel Model = new SYSConfigModel() {
                Item = SYSParamsService.GetItemConfig()
            };
            return View(Model);
        }
	}
}


/*
 
select a.IdCoQuan, cq.Title,Count(a.Id) as SoBaiViet
from Articles a
left join DM_CoQuan cq ON cq.Id = a.IdCoQuan
where a.IdCoQuan in (select Id from DM_CoQuan where ParentId=14)
and a.ShowPublishUp>20211230
group by IdCoQuan,cq.Title

select a.IdCoQuan, cq.Title,Count(a.Amount) as LuotTruyCap
from SiteVisitDetail a
left join DM_CoQuan cq ON cq.Id = a.IdCoQuan
where a.IdCoQuan in (select Id from DM_CoQuan where ParentId=14)
and a.ShowDateCreated>20211230
group by IdCoQuan,cq.Title
 
 */