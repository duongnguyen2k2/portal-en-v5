using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using API.Areas.Admin.Models.ArticleComment;
using API.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace API.Areas.Admin.Models.ArticleComment
{
    public class ArticleCommentService
    {
        public static List<ArticleComment> GetListPagination(SearchArticleComment dto, string SecretId)
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

            if (dto.ArticleId == 0)
            {
                str_sql = "GetListPaginationAll";
            }

            var tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_ArticleComment",
                new string[] { "@flag", "@CurrentPage", "@ItemsPerPage", "@Keyword","@ArticleId", "@IdCoQuan", "@Status" },
                new object[] { str_sql, dto.CurrentPage, dto.ItemsPerPage, dto.Keyword,dto.ArticleId,dto.IdCoQuan, Status });
            if (tabl == null)
            {
                return new List<ArticleComment>();
            }
            else
            {
                return (from r in tabl.AsEnumerable()
					select new ArticleComment
					{
						Id = (int)r["Id"],
 						Title = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
 						Description = (string)((r["Description"] == System.DBNull.Value) ? null : r["Description"]),
 						TenCoQuan = (string)((r["TenCoQuan"] == System.DBNull.Value) ? null : r["TenCoQuan"]),
 						IdCoQuan = (int)((r["IdCoQuan"] == System.DBNull.Value) ? null : r["IdCoQuan"]),
 						ArticleTitle = (string)((r["ArticleTitle"] == System.DBNull.Value) ? null : r["ArticleTitle"]),
 						Status = (Boolean)((r["Status"] == System.DBNull.Value) ? null : r["Status"]), 						
 						IP = (string)((r["IP"] == System.DBNull.Value) ? null : r["IP"]), 						
 						ArticleId = (int)((r["ArticleId"] == System.DBNull.Value) ? null : r["ArticleId"]), 						
 						Image = (string)((r["Image"] == System.DBNull.Value) ? null : r["Image"]),
						Ids = MyModels.Encode((int)r["Id"], SecretId),
                        TotalRows = (int)r["TotalRows"],
                    }).ToList();
            }


        }

        

        public static ArticleComment GetItem(decimal Id, string SecretId = null)
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_ArticleComment",
            new string[] { "@flag", "@Id" }, new object[] { "GetItem", Id });
            return (from r in tabl.AsEnumerable()
                    select new ArticleComment
                    {
                        Id = (int)r["Id"],
 						Title = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
 						Description = (string)((r["Description"] == System.DBNull.Value) ? null : r["Description"]),                        
                        IdCoQuan = (int)((r["IdCoQuan"] == System.DBNull.Value) ? null : r["IdCoQuan"]),
                        Status = (Boolean)((r["Status"] == System.DBNull.Value) ? null : r["Status"]), 						 						
 						IP = (string)((r["IP"] == System.DBNull.Value) ? null : r["IP"]), 						
 						ArticleId = (int)((r["ArticleId"] == System.DBNull.Value) ? null : r["ArticleId"]), 						
 						Image = (string)((r["Image"] == System.DBNull.Value) ? null : r["Image"]),
                        Ids = MyModels.Encode((int)r["Id"], SecretId),
                    }).FirstOrDefault();
        }

        public static dynamic SaveItem(ArticleComment dto)
        {
            dto.Title = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.Title);
            dto.Description = API.Models.MyHelper.StringHelper.RemoveTagsIntroText(dto.Description);

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_ArticleComment",
            new string[] { "@flag","@Id","@Title","@Description","@Status","@CreatedBy","@ModifiedBy", "@IP", "@ArticleId","@Image" , "@IdCoQuan" },
            new object[] { "SaveItem",dto.Id,dto.Title,dto.Description,dto.Status,dto.CreatedBy,dto.ModifiedBy,dto.IP,dto.ArticleId,dto.Image ,dto.IdCoQuan});
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }
        public static dynamic DeleteItem(ArticleComment dto)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_ArticleComment",
            new string[] { "@flag", "@Id", "@ModifiedBy" },
            new object[] { "DeleteItem", dto.Id, dto.ModifiedBy});
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }

        public static dynamic UpdateStatus(ArticleComment dto)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_ArticleComment",
            new string[] { "@flag", "@Id", "@Status", "@ModifiedBy" },
            new object[] { "UpdateStatus", dto.Id,dto.Status, dto.ModifiedBy });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }


    }
}
