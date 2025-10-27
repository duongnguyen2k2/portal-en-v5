using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API.Areas.Admin.Models.WorkSchedules;
using API.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace API.Areas.Admin.Models.WorkSchedules
{
    public class WorkSchedulesService
    {
        

        public static List<WorkSchedules> GetListPagination(SearchWorkSchedules dto, string SecretId)
        {
            if (dto.CurrentPage <= 0)
            {
                dto.CurrentPage = 1;
            }
            if (dto.ItemsPerPage <= 0)
            {
                dto.ItemsPerPage = 10;
            }
            if (dto.Keyword == null)
            {
                dto.Keyword = "";
            }

            string str_sql = "GetListPagination_Status";

            Boolean Status = true;
            if (dto.Status == -1)
            {
                str_sql = "GetListPagination";
            }
            else if (dto.Status == 0)
            {
                Status = false;
            }

            var tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_WorkSchedules",
                new string[] { "@flag", "@CurrentPage", "@ItemsPerPage", "@Keyword", "@ShowYear", "@ShowWeek", "@Status", "@IdCoQuan" },
                new object[] { str_sql, dto.CurrentPage, dto.ItemsPerPage, dto.Keyword,dto.ShowYear,dto.ShowWeek , Status,dto.IdCoQuan });
            if (tabl == null)
            {
                return new List<WorkSchedules>();
            }
            else
            {
                return (from r in tabl.AsEnumerable()
                        select new WorkSchedules
                        {
                            Id = (int)r["Id"],
                            Title = (string)((r["Title"] == System.DBNull.Value) ? "" : r["Title"]),
                            Status = (bool)r["Status"],
                            ShowWeek = (int)((r["ShowWeek"] == System.DBNull.Value) ? 0 : r["ShowWeek"]),
                            ShowYear = (int)((r["ShowYear"] == System.DBNull.Value) ? 0 : r["ShowYear"]),
                            IntroText = (string)((r["IntroText"] == System.DBNull.Value) ? "" : r["IntroText"]),                       
                            Ids = MyModels.Encode((int)r["Id"], SecretId),
                            TotalRows = (int)r["TotalRows"],
                        }).ToList();
            }


        }

        public static List<WorkSchedules> GetList(Boolean Selected = true)
        {
            
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_WorkSchedules",
                new string[] { "@flag", "@Selected" }, new object[] { "GetList", Selected });
            if (tabl == null)
            {
                return new List<WorkSchedules>();
            }
            else
            {
                return (from r in tabl.AsEnumerable()
                        select new WorkSchedules
                        {
                            Id = (int)r["Id"],
                            Title = (string)((r["Title"] == System.DBNull.Value) ? "" : r["Title"]),
                            Status = (bool)r["Status"],
                            ShowWeek = (int)((r["ShowWeek"] == System.DBNull.Value) ? 0 : r["ShowWeek"]),
                            ShowYear = (int)((r["ShowYear"] == System.DBNull.Value) ? 0 : r["ShowYear"]),                            
                        }).ToList();
            }

        }

        public static WorkSchedules GetItemByWeek(int ShowYear, int ShowWeek, int IdCoQuan)
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_WorkSchedules",
            new string[] { "@flag", "@ShowYear", "@ShowWeek", "@IdCoQuan" }, new object[] { "GetItemByWeek", ShowYear, ShowWeek, IdCoQuan });
            return (from r in tabl.AsEnumerable()
                    select new WorkSchedules
                    {
                        Id = (int)r["Id"],
                        Title = (string)((r["Title"] == System.DBNull.Value) ? "" : r["Title"]),
                        Status = (bool)r["Status"],
                        ShowWeek = (int)((r["ShowWeek"] == System.DBNull.Value) ? 0 : r["ShowWeek"]),
                        ShowYear = (int)((r["ShowYear"] == System.DBNull.Value) ? 0 : r["ShowYear"]),
                        IntroText = (string)((r["IntroText"] == System.DBNull.Value) ? "" : r["IntroText"]),
                        FullText = (string)((r["FullText"] == System.DBNull.Value) ? "" : r["FullText"]),                        
                    }).FirstOrDefault();
        }

        public static WorkSchedules GetItem(int Id, string SecretId = null)
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_WorkSchedules",
            new string[] { "@flag", "@Id" }, new object[] { "GetItem", Id });
            return (from r in tabl.AsEnumerable()
                    select new WorkSchedules
                    {
                        Id = (int)r["Id"],
                        Title = (string)((r["Title"] == System.DBNull.Value) ? "" : r["Title"]),
                        Status = (bool)r["Status"],
                        ShowWeek = (int)((r["ShowWeek"] == System.DBNull.Value) ? 0 : r["ShowWeek"]),
                        ShowYear = (int)((r["ShowYear"] == System.DBNull.Value) ? 0 : r["ShowYear"]),
                        IntroText = (string)((r["IntroText"] == System.DBNull.Value) ? "" : r["IntroText"]),
                        FullText = (string)((r["FullText"] == System.DBNull.Value) ? "" : r["FullText"]),                       
                        Ids = MyModels.Encode((int)r["Id"], SecretId),

                    }).FirstOrDefault();
        }

        public static dynamic SaveItem(WorkSchedules dto)
        {
            dto.Title = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.Title);
            dto.IntroText = API.Models.MyHelper.StringHelper.RemoveTagsIntroText(dto.IntroText);
            dto.FullText = API.Models.MyHelper.StringHelper.RemoveTagsFullText(dto.FullText);


            DateTime FirstDateOfWeek = WorkSchedulesService.FirstDateOfWeekISO8601(dto.ShowYear, dto.ShowWeek);
            string Title = "Tuần " + dto.ShowWeek + " (" + FirstDateOfWeek.ToString("dd/MM/yyyy") + "->" + FirstDateOfWeek.AddDays(6).ToString("dd/MM/yyyy")+")";

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_WorkSchedules",
            new string[] { "@flag","@Id", "@Title", "@Status", "@IntroText", "@FullText", "@ShowWeek", "@ShowYear",  "@IdCoQuan", "@CreatedBy", "@ModifiedBy" },
            new object[] { "SaveItem", dto.Id, Title, dto.Status, dto.IntroText, dto.FullText,dto.ShowWeek,dto.ShowYear,  dto.IdCoQuan, dto.CreatedBy,dto.ModifiedBy });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }

        public static dynamic SaveItemTitle(WorkSchedules dto)
        {
           
            DateTime FirstDateOfWeek = WorkSchedulesService.FirstDateOfWeekISO8601(dto.ShowYear, dto.ShowWeek);
            string Title = "Tuần " + dto.ShowWeek + " (" + FirstDateOfWeek.ToString("dd/MM/yyyy") + "->" + FirstDateOfWeek.AddDays(6).ToString("dd/MM/yyyy") + ")";

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_WorkSchedules",
            new string[] { "@flag", "@Id", "@Title"},
            new object[] { "SaveItemTitle", dto.Id, Title });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }




        public static dynamic UpdateStatus(WorkSchedules dto)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_WorkSchedules",
            new string[] { "@flag", "@Id","@Status", "@ModifiedBy" },
            new object[] { "UpdateStatus", dto.Id,dto.Status, dto.ModifiedBy });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }

        public static dynamic DeleteItem(WorkSchedules dto)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_WorkSchedules",
            new string[] { "@flag", "@Id", "@ModifiedBy" },
            new object[] { "DeleteItem", dto.Id, dto.ModifiedBy});
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }

        public static IList<DateTime> GetFirstDayOfWeekDates(CultureInfo cultureInfo, int year)
        {
            var lastDateOfYear = new DateTime(year, 12, 31);
            var firstDate = new DateTime(year, 1, 1);
            var dayOfWeek = cultureInfo.DateTimeFormat.FirstDayOfWeek;

            while (firstDate.DayOfWeek != dayOfWeek)
            {
                firstDate = firstDate.AddDays(1);
            }

            var numberOfWeeksInYear = cultureInfo.Calendar.GetWeekOfYear(lastDateOfYear, cultureInfo.DateTimeFormat.CalendarWeekRule, dayOfWeek);

            var firstDayOfWeekDates = new List<DateTime>();
            firstDayOfWeekDates.Add(firstDate);

            var currentDate = firstDate;

            for (int i = 0; i < numberOfWeeksInYear; i++)
            {
                var weekLater = currentDate.AddDays(7);

                if (weekLater.Year == year)
                {
                    currentDate = weekLater;
                    firstDayOfWeekDates.Add(currentDate);
                }
            }

            return firstDayOfWeekDates;
        }

        public static DateTime FirstDateOfWeekISO8601(int year, int weekOfYear)
        {
            DateTime jan1 = new DateTime(year, 1, 1);
            int daysOffset = DayOfWeek.Thursday - jan1.DayOfWeek;

            // Use first Thursday in January to get first week of the year as
            // it will never be in Week 52/53
            DateTime firstThursday = jan1.AddDays(daysOffset);
            var cal = CultureInfo.CurrentCulture.Calendar;
            int firstWeek = cal.GetWeekOfYear(firstThursday, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            var weekNum = weekOfYear;
            // As we're adding days to a date in Week 1,
            // we need to subtract 1 in order to get the right date for week #1
            if (firstWeek == 1)
            {
                weekNum -= 1;
            }

            // Using the first Thursday as starting week ensures that we are starting in the right year
            // then we add number of weeks multiplied with days
            var result = firstThursday.AddDays(weekNum * 7);

            // Subtract 3 days from Thursday to get Monday, which is the first weekday in ISO8601
            return result.AddDays(-3);
        }

    }
}
