using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using API.Areas.Admin.Models.AnToanThongTin;
using API.Models;
namespace API.Areas.Admin.Models.AnToanThongTin
{
    public class AnToanThongTinService
    {
        public static List<AnToanThongTin> GetListPagination(SearchAnToanThongTin dto, string SecretId)
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
            var tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_AnToanThongTin",
                new string[] { "@flag", "@CurrentPage", "@ItemsPerPage", "@Keyword" },
                new object[] { "GetListPagination", dto.CurrentPage, dto.ItemsPerPage, dto.Keyword });
            if (tabl == null)
            {
                return new List<AnToanThongTin>();
            }
            else
            {
                return (from r in tabl.AsEnumerable()
					select new AnToanThongTin
					{
						Id = (int)r["Id"],
 						Title = (string)r["Title"],
 						Status = (Boolean)r["Status"],
 						Fullname = (string)((r["Fullname"] == System.DBNull.Value) ? null : r["Fullname"]),
 						Phone = (string)((r["Phone"] == System.DBNull.Value) ? null : r["Phone"]),
 						Email = (string)((r["Email"] == System.DBNull.Value) ? null : r["Email"]),
 						Introtext = (string)((r["Introtext"] == System.DBNull.Value) ? null : r["Introtext"]),
 						Description = (string)((r["Description"] == System.DBNull.Value) ? null : r["Description"]),
 						Address = (string)((r["Address"] == System.DBNull.Value) ? null : r["Address"]), 						
						Ids = MyModels.Encode((int)r["Id"], SecretId),
						TotalRows = (int)r["TotalRows"],
					}).ToList();
            }


        }

        public static List<AnToanThongTin> GetList(Boolean Selected = true)
        {
            
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_AnToanThongTin",
                new string[] { "@flag", "@Selected" }, new object[] { "GetList", Convert.ToDecimal(Selected) });
            if (tabl == null)
            {
                return new List<AnToanThongTin>();
            }
            else
            {
                return (from r in tabl.AsEnumerable()
					select new AnToanThongTin
					{
						Id = (int)r["Id"],
 						Title = (string)r["Title"],
 						Status = (Boolean)r["Status"],
 						Fullname = (string)((r["Fullname"] == System.DBNull.Value) ? null : r["Fullname"]),
 						Phone = (string)((r["Phone"] == System.DBNull.Value) ? null : r["Phone"]),
 						Email = (string)((r["Email"] == System.DBNull.Value) ? null : r["Email"]),
 						Address = (string)((r["Address"] == System.DBNull.Value) ? null : r["Address"])									
					}).ToList();
            }

        }

        public static AnToanThongTin GetItem(decimal Id, string SecretId = null)
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_AnToanThongTin",
            new string[] { "@flag", "@Id" }, new object[] { "GetItem", Id });
            return (from r in tabl.AsEnumerable()
                    select new AnToanThongTin
                    {
                        Id = (int)r["Id"],
 						Title = (string)r["Title"],
 						Status = (Boolean)r["Status"],
 						Fullname = (string)((r["Fullname"] == System.DBNull.Value) ? null : r["Fullname"]),
 						Phone = (string)((r["Phone"] == System.DBNull.Value) ? null : r["Phone"]),
 						Email = (string)((r["Email"] == System.DBNull.Value) ? null : r["Email"]),
 						Address = (string)((r["Address"] == System.DBNull.Value) ? null : r["Address"]),
 						Introtext = (string)((r["Introtext"] == System.DBNull.Value) ? null : r["Introtext"]),
 						Description = (string)((r["Description"] == System.DBNull.Value) ? null : r["Description"]),
 						Deleted = (Boolean)r["Deleted"],
 						CreatedBy = (int)r["CreatedBy"],
 						CreatedDate = (DateTime)r["CreatedDate"],
 						ModifiedBy = (int?)((r["ModifiedBy"] == System.DBNull.Value) ? null : r["ModifiedBy"]),
 						ModifiedDate = (DateTime?)((r["ModifiedDate"] == System.DBNull.Value) ? null : r["ModifiedDate"]),
                        Ids = MyModels.Encode((int)r["Id"], SecretId),
                    }).FirstOrDefault();
        }

        public static dynamic SaveItem(AnToanThongTin dto)
        {
            
            dto.Title = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.Title);
            dto.Description = API.Models.MyHelper.StringHelper.RemoveTagsFullText(dto.Description);
            dto.Introtext = API.Models.MyHelper.StringHelper.RemoveTagsIntroText(dto.Introtext);
            dto.Fullname = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.Fullname);
            dto.Phone = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.Phone);
            dto.Email = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.Email);
            dto.Address = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.Address);
            
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_AnToanThongTin",
            new string[] { "@flag","@Id","@Title","@Introtext","@Description","@Status","@Fullname","@Phone","@Email","@Address","@CreatedBy","@ModifiedBy" },
            new object[] { "SaveItem",dto.Id,dto.Title,dto.Introtext,dto.Description,dto.Status,dto.Fullname,dto.Phone,dto.Email,dto.Address,dto.CreatedBy,dto.ModifiedBy });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }
        public static dynamic DeleteItem(AnToanThongTin dto)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_AnToanThongTin",
            new string[] { "@flag", "@Id", "@ModifiedBy" },
            new object[] { "DeleteItem", dto.Id, dto.ModifiedBy});
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }

        public static dynamic UpdateStatus(AnToanThongTin dto)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_AnToanThongTin",
            new string[] { "@flag", "@Id","@Status", "@ModifiedBy" },
            new object[] { "UpdateStatus", dto.Id,dto.Status, dto.ModifiedBy });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }



    }
}
