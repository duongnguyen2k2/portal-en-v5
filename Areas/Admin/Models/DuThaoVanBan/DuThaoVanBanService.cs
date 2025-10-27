using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API.Areas.Admin.Models.DuThaoVanBan;
using API.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace API.Areas.Admin.Models.DuThaoVanBan
{
    public class DuThaoVanBanService
    {

        public static List<DuThaoVanBan> GetListPagination(SearchDuThaoVanBan dto, string SecretId, Boolean flagFE = false)
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
            string sql = "GetListPagination";
            if (flagFE)
            {
                sql = "GetListPaginationFE";
            }
            var tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_DuThaoVanBan",
                new string[] { "@flag", "@CurrentPage", "@ItemsPerPage", "@Keyword", "@FieldId", "@IdCoQuan" },
                new object[] { sql, dto.CurrentPage, dto.ItemsPerPage, dto.Keyword,dto.FieldId ,dto.IdCoQuan });
            if (tabl == null)
            {
                return new List<DuThaoVanBan>();
            }
            else
            {
                return (from r in tabl.AsEnumerable()
                        select new DuThaoVanBan
                        {
                            Id = (int)r["Id"],
                            Title = (string)((r["Title"] == System.DBNull.Value) ? "" : r["Title"]),
                            Status = (bool)r["Status"],
                            TitleField = (string)((r["TitleField"] == System.DBNull.Value) ? "" : r["TitleField"]),                            
                            Introtext = (string)((r["Introtext"] == System.DBNull.Value) ? "" : r["Introtext"]),
                            PublishUp = (DateTime)((r["PublishUp"] == System.DBNull.Value) ? null : r["PublishUp"]),
                            PublishUpShow = (string)((r["PublishUp"] == System.DBNull.Value) ? DateTime.Now.ToString("dd/MM/yyyy") : (string)((DateTime)r["PublishUp"]).ToString("dd/MM/yyyy")),
                            PublishDown = (DateTime)((r["PublishDown"] == System.DBNull.Value) ? null : r["PublishDown"]),
                            PublishDownShow = (string)((r["PublishDown"] == System.DBNull.Value) ? DateTime.Now.ToString("dd/MM/yyyy") : (string)((DateTime)r["PublishDown"]).ToString("dd/MM/yyyy")),
                            Link = (string)((r["Link"] == System.DBNull.Value) ? "" : r["Link"]),                            
                            Ids = MyModels.Encode((int)r["Id"], SecretId),
                            TotalRows = (int)r["TotalRows"],
                        }).ToList();
            }


        }

        public static List<DuThaoVanBan> GetList(Boolean Selected = true)
        {
            
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_DuThaoVanBan",
                new string[] { "@flag", "@Selected" }, new object[] { "GetList", Selected });
            return (from r in tabl.AsEnumerable()
                    select new DuThaoVanBan
                    {
                        Id = (int)r["Id"],
                        Title = (string)r["Title"],                        
                        Introtext = (string)((r["Introtext"] == System.DBNull.Value) ? "" : r["Introtext"])                   
                    }).ToList();

        }

     
        public static DuThaoVanBan GetItem(int Id, string SecretId = null)
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_DuThaoVanBan",
            new string[] { "@flag", "@Id" }, new object[] { "GetItem", Id });
            return (from r in tabl.AsEnumerable()
                    select new DuThaoVanBan
                    {
                        Id = (int)r["Id"],
                        Title = (string)((r["Title"] == System.DBNull.Value) ? "" : r["Title"]),
                        Status = (bool)r["Status"],
                        Link = (string)((r["Link"] == System.DBNull.Value) ? "" : r["Link"]),
                        Introtext = (string)((r["Introtext"] == System.DBNull.Value) ? "" : r["Introtext"]),
                        FieldId = (int)((r["FieldId"] == System.DBNull.Value) ? 0 : r["FieldId"]),
                        PublishUp = (DateTime)((r["PublishUp"] == System.DBNull.Value) ? null : r["PublishUp"]),
                        PublishUpShow = (string)((r["PublishUp"] == System.DBNull.Value) ? DateTime.Now.ToString("dd/MM/yyyy") : (string)((DateTime)r["PublishUp"]).ToString("dd/MM/yyyy")),
                        PublishDown = (DateTime)((r["PublishDown"] == System.DBNull.Value) ? null : r["PublishDown"]),
                        PublishDownShow = (string)((r["PublishDown"] == System.DBNull.Value) ? DateTime.Now.ToString("dd/MM/yyyy") : (string)((DateTime)r["PublishDown"]).ToString("dd/MM/yyyy")),
                        Ids = MyModels.Encode((int)r["Id"], SecretId),
                    }).FirstOrDefault();
        }

        public static dynamic SaveItem(DuThaoVanBan dto)
        {
            DateTime NgayDang = DateTime.ParseExact(dto.PublishUpShow, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime NgayHetHan = DateTime.ParseExact(dto.PublishDownShow, "dd/MM/yyyy", CultureInfo.InvariantCulture);
			dto.Title = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.Title);
            dto.Introtext = API.Models.MyHelper.StringHelper.RemoveTagsIntroText(dto.Introtext);
            dto.Link = API.Models.MyHelper.StringHelper.RemoveTagsLink(dto.Link);

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_DuThaoVanBan",
            new string[] { "@flag","@Id", "@Title", "@Status", "@Introtext", "@FieldId", "@Link", "@IdCoQuan", "@CreatedBy", "@ModifiedBy", "@PublishUp", "@PublishDown" },
            new object[] { "SaveItem", dto.Id, dto.Title, dto.Status, dto.Introtext, dto.FieldId,dto.Link,dto.IdCoQuan,dto.CreatedBy,dto.ModifiedBy,NgayDang, NgayHetHan });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }

        public static dynamic UpdateStatus(DuThaoVanBan dto)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_DuThaoVanBan",
            new string[] { "@flag", "@Id","@Status", "@ModifiedBy" },
            new object[] { "UpdateStatus", dto.Id,dto.Status, dto.ModifiedBy });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }

        public static dynamic DeleteItem(DuThaoVanBan dto)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_DuThaoVanBan",
            new string[] { "@flag", "@Id", "@ModifiedBy" },
            new object[] { "DeleteItem", dto.Id, dto.ModifiedBy});
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }


    }
}
