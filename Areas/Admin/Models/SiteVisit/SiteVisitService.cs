using API.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace API.Areas.Admin.Models.SiteVisit
{
    public class SiteVisitService
    {
      

        public static SiteVisitDetail GetByDate(DateTime DateCreated, int IdCoQuan)
        {
            int ShowDateCreated = int.Parse(DateCreated.ToString("yyyyMMdd"));
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_SiteVisit",
            new string[] { "@flag",  "@ShowDateCreated" , "@IdCoQuan" },
            new object[] { "GetByDate", ShowDateCreated , IdCoQuan });
            return (from r in tabl.AsEnumerable()
                    select new SiteVisitDetail
                    {
                        Amount = (int)(r["Amount"]),
                        ShowDateCreated = (int)(r["ShowDateCreated"]),
                        DateCreated = (DateTime)(r["DateCreated"]),
                    }).FirstOrDefault();
        }

        public static SiteVisitDetail GetAll(int IdCoQuan)
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_SiteVisit",
            new string[] { "@flag", "@IdCoQuan" },
            new object[] { "GetAll", IdCoQuan });
            return (from r in tabl.AsEnumerable()
                    select new SiteVisitDetail
                    {
                        Amount = (int)(r["Amount"]),                       
                    }).FirstOrDefault();

        }

        public static SiteVisitDetail GetByDateMonth(int IdCoQuan)
        {
            int thisWeekNumber = GetIso8601WeekOfYear(DateTime.Today);
            DateTime firstDayOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            //FirstDateOfWeek(DateTime.Now.Year, thisWeekNumber, CultureInfo.CurrentCulture);
            DateTime lastDateOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            int StartDate = int.Parse(firstDayOfMonth.ToString("yyyyMMdd"));
            int EndDate = int.Parse(lastDateOfMonth.ToString("yyyyMMdd"));
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_SiteVisit",
            new string[] { "@flag", "@StartDate", "@EndDate", "@IdCoQuan" },
            new object[] { "GetByDateMonth", StartDate, EndDate, IdCoQuan });
            return (from r in tabl.AsEnumerable()
                    select new SiteVisitDetail
                    {
                        Amount = (int)(r["Amount"]),
                    }).FirstOrDefault();

        }

        public static SiteVisitDetail GetByDateWeek(int IdCoQuan)
        {
            int thisWeekNumber = GetIso8601WeekOfYear(DateTime.Today);
            DateTime firstDayOfWeek = FirstDateOfWeek(DateTime.Now.Year, thisWeekNumber, CultureInfo.CurrentCulture);
            DateTime lastDateOfWeek = firstDayOfWeek.AddDays(7);

            int StartDate = int.Parse(firstDayOfWeek.ToString("yyyyMMdd"));
            int EndDate = int.Parse(lastDateOfWeek.ToString("yyyyMMdd"));
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_SiteVisit",
            new string[] { "@flag", "@StartDate", "@EndDate", "@IdCoQuan" },
            new object[] { "GetByDateWeek", StartDate, EndDate, IdCoQuan });
            return (from r in tabl.AsEnumerable()
                    select new SiteVisitDetail
                    {
                        Amount = (int)(r["Amount"]),
                    }).FirstOrDefault();

        }

        public static dynamic SaveItem(string ShowDateCreated, int IdCoQuan)
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_SiteVisit",
            new string[] { "@flag", "@ShowDateCreated", "@IdCoQuan" },
            new object[] { "InsertVisit", ShowDateCreated, IdCoQuan });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }

        public static int GetIso8601WeekOfYear(DateTime time)
        {
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                time = time.AddDays(3);
            }

            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }
        public static DateTime FirstDateOfWeek(int year, int weekOfYear, System.Globalization.CultureInfo ci)
        {
            DateTime jan1 = new DateTime(year, 1, 1);
            int daysOffset = (int)ci.DateTimeFormat.FirstDayOfWeek - (int)jan1.DayOfWeek;
            DateTime firstWeekDay = jan1.AddDays(daysOffset);
            int firstWeek = ci.Calendar.GetWeekOfYear(jan1, ci.DateTimeFormat.CalendarWeekRule, ci.DateTimeFormat.FirstDayOfWeek);
            if ((firstWeek <= 1 || firstWeek >= 52) && daysOffset >= -3)
            {
                weekOfYear -= 1;
            }
            return firstWeekDay.AddDays(weekOfYear * 7);
        }
    }
}
