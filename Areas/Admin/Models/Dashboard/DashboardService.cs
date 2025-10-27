using API.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;
using System.IO;
using ClosedXML.Excel;
using ClosedXML.Report;
namespace API.Areas.Admin.Models.Dashboard
{
    public class DashboardService
    {
        public static ReportMonth GetReportMonth(int IdCoQuan)
        {
            string Time = DateTime.Now.ToString("yyyyMM");
            string Start = Time + "00";
            string End = Time + "31";
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Dashboard",
                new string[] { "@flag", "@IdCoQuan", "@StartDay", "@EndDay" }, new object[] { "GetReportMonth", IdCoQuan,Int32.Parse(Start), Int32.Parse(End) });
            if (tabl == null)
            {
                return new ReportMonth();
            }
            else
            {
                return (from r in tabl.AsEnumerable()
                        select new ReportMonth
                        {
                            ThangDonVi = (int)((r["ThangDonVi"] == System.DBNull.Value) ? 0 : r["ThangDonVi"]),
                            ThangHuyen = (int)((r["ThangHuyen"] == System.DBNull.Value) ? 0 : r["ThangHuyen"]),
                            ThangTinh = (int)((r["ThangTinh"] == System.DBNull.Value) ? 0 : r["ThangTinh"]),
                            ThangTatCa = (int)((r["ThangTatCa"] == System.DBNull.Value) ? 0 : r["ThangTatCa"]),

                        }).FirstOrDefault();
            }

        }

        public static DataTable GetListCountVisit(ReportArticle dto)
        {                       
            DateTime StartDate = DateTime.ParseExact(dto.ShowStartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime EndDate = DateTime.ParseExact(dto.ShowEndDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            string Start = StartDate.ToString("yyyyMMdd");
            string End = EndDate.ToString("yyyyMMdd");

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Dashboard",
                new string[] { "@flag", "@IdCoQuan", "@StartDay", "@EndDay" }, new object[] { "GetListCountVisit", dto.IdCoQuan, Int32.Parse(Start), Int32.Parse(End) });
            return tabl;

        }
        public static DataTable BieuDoCot(ReportArticle dto)
        {                                  
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Dashboard",
                new string[] { "@flag", "@IdCoQuan", "@StartDay", "@EndDay" }, new object[] { "BieuDoCot", dto.IdCoQuan, Int32.Parse(dto.ShowStartDate), Int32.Parse(dto.ShowEndDate) });
            return tabl;

        }

        public static DataTable BieuDoLuotTruyCapCot(ReportArticle dto)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Dashboard",
                new string[] { "@flag", "@IdCoQuan", "@StartDay", "@EndDay" }, new object[] { "BieuDoLuotTruyCapCot", dto.IdCoQuan, Int32.Parse(dto.ShowStartDate), Int32.Parse(dto.ShowEndDate) });
            return tabl;

        }

        public static List<CountArticle> GetListCountArticles(ReportArticle dto)
        {
            DateTime StartDate = DateTime.ParseExact(dto.ShowStartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime EndDate = DateTime.ParseExact(dto.ShowEndDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            string Start = StartDate.ToString("yyyyMMdd");
            string End = EndDate.ToString("yyyyMMdd");

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Dashboard",
                new string[] { "@flag", "@IdCoQuan", "@StartDay", "@EndDay" }, new object[] { "GetListCountArticles", dto.IdCoQuan, Int32.Parse(Start), Int32.Parse(End) });

            return (from r in tabl.AsEnumerable()
                    select new CountArticle
                    {
                        IdCoQuan = (int)((r["IdCoQuan"] == System.DBNull.Value) ? 0 : r["IdCoQuan"]),
                        SoBaiViet = (int)((r["SoBaiViet"] == System.DBNull.Value) ? 0 : r["SoBaiViet"]),
                        SoHinhAnh = (int)((r["SoHinhAnh"] == System.DBNull.Value) ? 0 : r["SoHinhAnh"]),
                        Title = (string)((r["Title"] == System.DBNull.Value) ? "" : r["Title"]),

                    }).ToList();

        }


        public static string ExportCountArticles(ReportArticle dto)
        {
            string TenFileLuu = "bc-ket-qua-hoat-dong-nam-" + dto.IdCoQuan + "_" + string.Format("{0:ddMMyyHHmmss}" + ".xlsx", DateTime.Now);

            List<CountArticle> ListItems = DashboardService.GetListCountArticles(dto);

           
            string CreateDate = DateTime.Now.ToString("dd/MM/yyyy");
           
            
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "temp");
            string outputFile = System.IO.Path.Combine(path, TenFileLuu);

            string ReportTemplate = System.IO.Path.Combine(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ReportTemplate"), "bc-count-articles" + ".xlsx");
            var template = new XLTemplate(ReportTemplate);

            template.AddVariable("ShowStartDate", dto.ShowStartDate);
            template.AddVariable("ShowEndDate", dto.ShowEndDate);

            template.AddVariable("ListItems",ListItems);
            template.Generate();
            template.SaveAs(outputFile);
            return TenFileLuu;
        }
    }
}
