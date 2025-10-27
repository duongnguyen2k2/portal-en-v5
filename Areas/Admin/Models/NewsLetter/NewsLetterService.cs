using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using API.Areas.Admin.Models.NewsLetter;
using API.Models;
namespace API.Areas.Admin.Models.NewsLetter
{
    public class NewsLetterService
    {
        public static List<NewsLetter> GetListPagination(SearchNewsLetter dto, string SecretId)
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
            var tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_NewsLetter",
                new string[] { "@flag", "@CurrentPage", "@ItemsPerPage", "@Keyword" },
                new object[] { "GetListPagination", dto.CurrentPage, dto.ItemsPerPage, dto.Keyword });
            if (tabl == null)
            {
                return new List<NewsLetter>();
            }
            else
            {
                return (from r in tabl.AsEnumerable()
					select new NewsLetter
					{
						Id = (int)r["Id"],
 						Title = (string)r["Title"],
 						Status = (Boolean)r["Status"],
 						Email = (string)((r["Email"] == System.DBNull.Value) ? null : r["Email"]), 						
						Ids = MyModels.Encode((int)r["Id"], SecretId),
						TotalRows = (int)r["TotalRows"],
					}).ToList();
            }


        }

        public static List<NewsLetter> GetList(Boolean Selected = true)
        {
            
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_NewsLetter",
                new string[] { "@flag", "@Selected" }, new object[] { "GetList", Convert.ToDecimal(Selected) });
            if (tabl == null)
            {
                return new List<NewsLetter>();
            }
            else
            {
                return (from r in tabl.AsEnumerable()
					select new NewsLetter
					{
						Id = (int)r["Id"],
 						Title = (string)r["Title"],
 						Status = (Boolean)r["Status"],
 						Email = (string)((r["Email"] == System.DBNull.Value) ? null : r["Email"]), 						
						TotalRows = (int)r["TotalRows"],
					}).ToList();
            }

        }

        public static NewsLetter GetItem(decimal Id, string SecretId = null)
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_NewsLetter",
            new string[] { "@flag", "@Id" }, new object[] { "GetItem", Id });
            return (from r in tabl.AsEnumerable()
                    select new NewsLetter
                    {
                        Id = (int)r["Id"],
 						Title = (string)r["Title"],
 						Status = (Boolean)r["Status"],
 						Email = (string)((r["Email"] == System.DBNull.Value) ? null : r["Email"]),
 						Deleted = (Boolean)r["Deleted"],
 						CreatedBy = (int)r["CreatedBy"],
 						CreatedDate = (DateTime)r["CreatedDate"],
 						ModifiedBy = (int?)((r["ModifiedBy"] == System.DBNull.Value) ? null : r["ModifiedBy"]),
 						ModifiedDate = (DateTime?)((r["ModifiedDate"] == System.DBNull.Value) ? null : r["ModifiedDate"]),
                        Ids = MyModels.Encode((int)r["Id"], SecretId),
                    }).FirstOrDefault();
        }

        public static dynamic SaveItem(NewsLetter dto)
        {
            dto.Title = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.Title);
            dto.Email = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.Email);

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_NewsLetter",
            new string[] { "@flag","@Id","@Title","@Status","@Email","@CreatedBy","@ModifiedBy"},
            new object[] { "SaveItem",dto.Id,dto.Title,dto.Status,dto.Email,dto.CreatedBy,dto.ModifiedBy });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }
        public static dynamic DeleteItem(NewsLetter dto)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_NewsLetter",
            new string[] { "@flag", "@Id", "@ModifiedBy" },
            new object[] { "DeleteItem", dto.Id, dto.ModifiedBy});
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }
		
		public static dynamic UpdateStatus(NewsLetter dto)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_NewsLetter",
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
