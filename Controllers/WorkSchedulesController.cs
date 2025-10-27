using API.Areas.Admin.Models.WorkSchedules;
using API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class WorkSchedulesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Idesk()
        {
            int IdCoQuan = 1;

            if (HttpContext.Session.GetString("IdCoQuan") != null && HttpContext.Session.GetString("IdCoQuan") != "")
            {
                IdCoQuan = int.Parse(HttpContext.Session.GetString("IdCoQuan"));
            }

            API.Areas.Admin.Models.DMCoQuan.DMCoQuan ItemCoQuan = API.Areas.Admin.Models.DMCoQuan.DMCoQuanService.GetItem(IdCoQuan);

            return View(ItemCoQuan);
        }

        [HttpPost]
        public IActionResult GetListWeek([FromBody] SearchWorkSchedules dto)
        {
            DateTime NgayDang = DateTime.Now;
            CultureInfo ciCurr = CultureInfo.CurrentCulture;
            int WeekOfYear = ciCurr.Calendar.GetWeekOfYear(NgayDang, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            int NWeek = ISOWeek.GetWeeksInYear(dto.ShowYear);
            List<WorkSchedulesWeek> ListItems = new List<WorkSchedulesWeek>();
            var ci = new CultureInfo("vi-VN");
            var dates = WorkSchedulesService.GetFirstDayOfWeekDates(ci, DateTime.Now.Year);
            foreach (var dt in dates)
            {
                ListItems.Add(new WorkSchedulesWeek { Id = ciCurr.Calendar.GetWeekOfYear(dt, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday), Title = dt.ToString("dd/MM/yyyy") + " -> " + dt.AddDays(6).ToString("dd/MM/yyyy") });
            }

            return Json(new MsgSuccess() { Data = new { ListWeek = ListItems, WeekOfYear = WeekOfYear } });
        }


        public IActionResult DongBoDuLieu() {

            List<WorkSchedules> ListItems = WorkSchedulesService.GetList();
            for (int i = 0; i < ListItems.Count(); i++)
            {
                WorkSchedulesService.SaveItemTitle(ListItems[i]);
            }
            return Json(new MsgSuccess() { Data = ListItems });
        }

        [HttpPost]
        public IActionResult GetItem([FromBody] SearchWorkSchedules dto)
        {
            dto.IdCoQuan = 1;

            if (HttpContext.Session.GetString("IdCoQuan") != null && HttpContext.Session.GetString("IdCoQuan") != "")
            {
                dto.IdCoQuan = int.Parse(HttpContext.Session.GetString("IdCoQuan"));
            }

            if (dto.ShowWeek == 0) {
                DateTime NgayDang = DateTime.Now;
                CultureInfo ciCurr = CultureInfo.CurrentCulture;
                dto.ShowWeek = ciCurr.Calendar.GetWeekOfYear(NgayDang, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
                
            }
            WorkSchedules Item = new WorkSchedules();
            if (dto.ShowYear > 0 && dto.ShowWeek>0)
            {
                Item = WorkSchedulesService.GetItemByWeek(dto.ShowYear, dto.ShowWeek,dto.IdCoQuan);
            }
            return Json(new MsgSuccess() { Data = Item, Msg = "Lấy Lịch công tác thành công" });
        }


    }
}
