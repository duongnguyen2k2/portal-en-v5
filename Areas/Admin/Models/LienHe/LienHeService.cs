using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using API.Areas.Admin.Models.LienHe;
using API.Models;
namespace API.Areas.Admin.Models.LienHe
{
    public class LienHeService
    {
        public static List<LienHe> GetListPagination(SearchLienHe dto, string SecretId)
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
            var tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Contacts",
                new string[] { "@flag", "@CurrentPage", "@ItemsPerPage", "@Keyword","@Type" },
                new object[] { "GetListPagination", dto.CurrentPage, dto.ItemsPerPage, dto.Keyword,dto.Type});
            if (tabl == null)
            {
                return new List<LienHe>();
            }
            else
            {
                return (from r in tabl.AsEnumerable()
					select new LienHe
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
                        CreatedDate = (DateTime)r["CreatedDate"],
                        CreatedDateShow = (string)((r["CreatedDate"] == System.DBNull.Value) ? DateTime.Now.ToString("dd/MM/yyyy") : (string)((DateTime)r["CreatedDate"]).ToString("dd/MM/yyyy HH:mm")),
                        TotalRows = (int)r["TotalRows"],
					}).ToList();
            }


        }



        public static List<LienHe> GetListPaginationFront(SearchLienHe dto, string SecretId)
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
            var tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Contacts",
                new string[] { "@flag", "@CurrentPage", "@ItemsPerPage", "@Keyword", "@Type" },
                new object[] { "GetListPaginationFront", dto.CurrentPage, dto.ItemsPerPage, dto.Keyword, dto.Type });
            if (tabl == null)
            {
                return new List<LienHe>();
            }
            else
            {
                return (from r in tabl.AsEnumerable()
                        select new LienHe
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
                            CreatedDate = (DateTime)r["CreatedDate"],
                            CreatedDateShow = (string)((r["CreatedDate"] == System.DBNull.Value) ? DateTime.Now.ToString("dd/MM/yyyy") : (string)((DateTime)r["CreatedDate"]).ToString("dd/MM/yyyy HH:mm")),
                            TotalRows = (int)r["TotalRows"],
                        }).ToList();
            }


        }

        public static List<LienHe> GetList(Boolean Selected = true)
        {
            
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Contacts",
                new string[] { "@flag", "@Selected" }, new object[] { "GetList", Convert.ToDecimal(Selected) });
            if (tabl == null)
            {
                return new List<LienHe>();
            }
            else
            {
                return (from r in tabl.AsEnumerable()
					select new LienHe
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

        public static LienHe GetItem(decimal Id, string SecretId = null)
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Contacts",
            new string[] { "@flag", "@Id" }, new object[] { "GetItem", Id });
            return (from r in tabl.AsEnumerable()
                    select new LienHe
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
                        CreatedDateShow = (string)((r["CreatedDate"] == System.DBNull.Value) ? DateTime.Now.ToString("dd/MM/yyyy") : (string)((DateTime)r["CreatedDate"]).ToString("dd/MM/yyyy HH:mm")),
                        ModifiedBy = (int?)((r["ModifiedBy"] == System.DBNull.Value) ? null : r["ModifiedBy"]),
 						ModifiedDate = (DateTime?)((r["ModifiedDate"] == System.DBNull.Value) ? null : r["ModifiedDate"]),
                        Ids = MyModels.Encode((int)r["Id"], SecretId),
                        Type= (int)r["Type"],
                        EventId= (int)r["EventId"]
                    }).FirstOrDefault();
        }

        public static dynamic SaveItem(LienHe dto)
        {
            dto.Title = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.Title);
            dto.Fullname = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.Fullname);
            dto.Phone = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.Phone);
            dto.Email = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.Email);
            dto.Address = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.Address);
            dto.Description = API.Models.MyHelper.StringHelper.RemoveTagsFullText(dto.Description);
            dto.Introtext = API.Models.MyHelper.StringHelper.RemoveTagsIntroText(dto.Introtext);

            if (dto.CreatedDate == null)
            {
                dto.CreatedDate = DateTime.Now;
            }

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Contacts",
            new string[] { "@flag","@Id","@Title","@Introtext","@Description","@Status","@Fullname","@Phone","@Email","@Address","@CreatedBy","@CreatedDate","@ModifiedBy","@Type","@EventId" },
            new object[] { "SaveItem",dto.Id,dto.Title,dto.Introtext,dto.Description,dto.Status,dto.Fullname,dto.Phone,dto.Email,dto.Address,dto.CreatedBy,dto.CreatedDate,dto.ModifiedBy,dto.Type,dto.EventId });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }
        public static dynamic DeleteItem(LienHe dto)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Contacts",
            new string[] { "@flag", "@Id", "@ModifiedBy" },
            new object[] { "DeleteItem", dto.Id, dto.ModifiedBy});
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }

        public static dynamic UpdateStatus(LienHe dto)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Contacts",
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
