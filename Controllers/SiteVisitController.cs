using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using API.Areas.Admin.Models.SiteVisit;
using Microsoft.AspNetCore.Http;

namespace API.Controllers
{
    public class SiteVisitController : Controller
    {
        public IActionResult Index()
        {
            int IdCoQuan = 1;
            if (HttpContext.Session.GetString("IdCoQuan") != null && HttpContext.Session.GetString("IdCoQuan") != "")
            {
                IdCoQuan = int.Parse(HttpContext.Session.GetString("IdCoQuan"));
            }

            SiteVisitDetail DateMonth = SiteVisitService.GetByDateMonth(IdCoQuan);
            SiteVisitDetail DateWeek = SiteVisitService.GetByDateWeek(IdCoQuan);
            SiteVisitDetail GetAll = SiteVisitService.GetAll(IdCoQuan);
            SiteVisitDetail DateNow = SiteVisitService.GetByDate(DateTime.Now, IdCoQuan);
            SiteVisitDetail LastDate = SiteVisitService.GetByDate(DateTime.Now.AddDays(-1), IdCoQuan);

            SiteVisitResult data = new SiteVisitResult()
            {
                Total = String.Format("{0:#,###,###.##}", GetAll.Amount)
            };
            if (DateMonth != null)
            {
                data.DateOfMonth = String.Format("{0:#,###,###.##}", DateMonth.Amount);
            }
            if (DateWeek != null)
            {
                data.DateOfWeek = String.Format("{0:#,###,###.##}", DateWeek.Amount);
            }
            if (DateNow != null)
            {
                data.DateNow = String.Format("{0:#,###,###.##}", DateNow.Amount);
            }
            if (LastDate != null)
            {
                data.Yesterday = String.Format("{0:#,###,###.##}", LastDate.Amount);
            }

            return Json(data);
        }
    }
}