using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using API.Areas.Admin.Models.Articles;
using API.Areas.Admin.Models.DuThaoVanBanGopY;
using API.Models;
using Newtonsoft.Json;

namespace API.Areas.Admin.Models.DuThaoVanBanGopY
{
    public class DuThaoVanBanGopYService
    {
        public static List<DuThaoVanBanGopY> GetListPagination(SearchDuThaoVanBanGopY dto, string SecretId, Boolean flagSql = false)
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
            if (flagSql)
            {
                sql = "GetListPaginationStatus";
            }
            var tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_DuThaoVanBanGopY",
                new string[] { "@flag", "@CurrentPage", "@ItemsPerPage", "@Keyword" , "@IdDuThao" },
                new object[] { sql, dto.CurrentPage, dto.ItemsPerPage, dto.Keyword,dto.IdDuThao });
            if (tabl == null)
            {
                return new List<DuThaoVanBanGopY>();
            }
            else
            {
                return (from r in tabl.AsEnumerable()
					select new DuThaoVanBanGopY
					{
						Id = (int)r["Id"],
 						Title = (string)r["Title"],
 						Status = (Boolean)r["Status"],
 						Fullname = (string)((r["Fullname"] == System.DBNull.Value) ? null : r["Fullname"]),
 						IntrotextDuThao = (string)((r["IntrotextDuThao"] == System.DBNull.Value) ? null : r["IntrotextDuThao"]),
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

      

        public static DuThaoVanBanGopY GetItem(decimal Id, string SecretId = null)
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_DuThaoVanBanGopY",
            new string[] { "@flag", "@Id" }, new object[] { "GetItem", Id });
            DuThaoVanBanGopY Item = (from r in tabl.AsEnumerable()
                    select new DuThaoVanBanGopY
                    {
                        Id = (int)r["Id"],
 						Title = (string)r["Title"],
 						Status = (Boolean)r["Status"],
 						Fullname = (string)((r["Fullname"] == System.DBNull.Value) ? null : r["Fullname"]),
                        Str_ListFile = (string)((r["Str_ListFile"] == System.DBNull.Value) ? null : r["Str_ListFile"]),
                        Phone = (string)((r["Phone"] == System.DBNull.Value) ? null : r["Phone"]),
 						Email = (string)((r["Email"] == System.DBNull.Value) ? null : r["Email"]),
 						Address = (string)((r["Address"] == System.DBNull.Value) ? null : r["Address"]),
 						Introtext = (string)((r["Introtext"] == System.DBNull.Value) ? null : r["Introtext"]),
 						Description = (string)((r["Description"] == System.DBNull.Value) ? null : r["Description"]), 						
 						Deleted = (Boolean)r["Deleted"],
 						CreatedBy = (int)r["CreatedBy"],
 						CreatedDate = (DateTime)r["CreatedDate"],
 						IdDuThao = (int)((r["IdDuThao"] == System.DBNull.Value) ? null : r["IdDuThao"]),
 						ModifiedBy = (int?)((r["ModifiedBy"] == System.DBNull.Value) ? null : r["ModifiedBy"]),
 						ModifiedDate = (DateTime?)((r["ModifiedDate"] == System.DBNull.Value) ? null : r["ModifiedDate"]),
                        Ids = MyModels.Encode((int)r["Id"], SecretId),
                        
                    }).FirstOrDefault();

            if (Item != null)
            {
                if (Item.Str_ListFile != null && Item.Str_ListFile != "")
                {
                    Item.ListFile = JsonConvert.DeserializeObject<List<FileArticle>>(Item.Str_ListFile);
                }
                
            }
            return Item;
        }

        public static dynamic SaveItem(DuThaoVanBanGopY dto)
        {
            string Str_ListFile = null;
            List<FileArticle> ListFileArticle = new List<FileArticle>();
            if (dto.ListFile != null && dto.ListFile.Count() > 0)
            {
                for (int i = 0; i < dto.ListFile.Count(); i++)
                {
                    if (dto.ListFile[i].FilePath != null && dto.ListFile[i].FilePath.Trim() != "")
                    {
                        ListFileArticle.Add(dto.ListFile[i]);
                    }
                }
                if (ListFileArticle != null && ListFileArticle.Count() > 0)
                {
                    Str_ListFile = JsonConvert.SerializeObject(ListFileArticle);
                }

            }

            dto.Fullname = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.Fullname);
            dto.Title = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.Title);
            dto.Phone = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.Phone);
            dto.Email = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.Email);
            dto.Address = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.Address);
            dto.Introtext = API.Models.MyHelper.StringHelper.RemoveTagsIntroText(dto.Introtext);
            dto.Description = API.Models.MyHelper.StringHelper.RemoveTagsFullText(dto.Description);
                      
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_DuThaoVanBanGopY",
            new string[] { "@flag","@Id","@Title","@Introtext","@Description", "@Str_ListFile", "@Status","@Fullname","@Phone","@Email","@Address","@CreatedBy","@ModifiedBy", "@IdDuThao"},
            new object[] { "SaveItem",dto.Id,dto.Title,dto.Introtext,dto.Description, Str_ListFile, dto.Status,dto.Fullname,dto.Phone,dto.Email,dto.Address,dto.CreatedBy,dto.ModifiedBy,dto.IdDuThao});
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();
        }
        public static dynamic DeleteItem(DuThaoVanBanGopY dto)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_DuThaoVanBanGopY",
            new string[] { "@flag", "@Id", "@ModifiedBy" },
            new object[] { "DeleteItem", dto.Id, dto.ModifiedBy});
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }

        public static dynamic UpdateStatus(DuThaoVanBanGopY dto)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_DuThaoVanBanGopY",
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
