using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using API.Areas.Admin.Models.DocumentRefersCategories;
using API.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace API.Areas.Admin.Models.DocumentRefersCategories
{
    public class DocumentRefersCategoriesService
    {
        public static List<DocumentRefersCategories> GetListPagination(SearchDocumentRefersCategories dto, string SecretId)
        {
            string sql = "GetListPagination";

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
            if (dto.TaiLieuHop == 1)
            {
                sql = "GetListPaginationTaiLieuHop";
            }
            var tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_DocumentRefersCategories",
                new string[] { "@flag", "@CurrentPage", "@ItemsPerPage", "@Keyword", "@IdCoQuan" },
                new object[] { sql, dto.CurrentPage, dto.ItemsPerPage, dto.Keyword,dto.IdCoQuan });
            if (tabl == null)
            {
                return new List<DocumentRefersCategories>();
            }
            else
            {
                return (from r in tabl.AsEnumerable()
					select new DocumentRefersCategories
					{
						Id = (int)r["Id"],
 						Title = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
                        Alias = (string)((r["Alias"] == System.DBNull.Value) ? null : r["Alias"]),
                        Description = (string)((r["Description"] == System.DBNull.Value) ? null : r["Description"]),
                        MatKhau = (string)((r["MatKhau"] == System.DBNull.Value) ? null : r["MatKhau"]),
 						Status = (Boolean)r["Status"], 						
						Ids = MyModels.Encode((int)r["Id"], SecretId),
						TotalRows = (int)r["TotalRows"],
					}).ToList();
            }


        }

        public static List<DocumentRefersCategories> GetListPaginationFE(SearchDocumentRefersCategories dto, string SecretId)
        {
            string sql = "GetListPagination_FE";
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
            if (dto.TaiLieuHop == 1)
            {
                sql = "GetListPaginationTaiLieuHop_FE";
            }
            var tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_DocumentRefersCategories",
                new string[] { "@flag", "@CurrentPage", "@ItemsPerPage", "@Keyword","@IdCoQuan" },
                new object[] { sql, dto.CurrentPage, dto.ItemsPerPage, dto.Keyword,dto.IdCoQuan });
            if (tabl == null)
            {
                return new List<DocumentRefersCategories>();
            }
            else
            {
                return (from r in tabl.AsEnumerable()
                        select new DocumentRefersCategories
                        {
                            Id = (int)r["Id"],
                            Title = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
                            Alias = (string)((r["Alias"] == System.DBNull.Value) ? null : r["Alias"]),
                            Description = (string)((r["Description"] == System.DBNull.Value) ? null : r["Description"]),
                            Status = (Boolean)r["Status"],
                            Ids = MyModels.Encode((int)r["Id"], SecretId),
                            TotalRows = (int)r["TotalRows"],
                        }).ToList();
            }


        }

        public static List<DocumentRefersCategories> GetList(Boolean Selected = true)
        {
            
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_DocumentRefersCategories",
                new string[] { "@flag", "@Selected" }, new object[] { "GetList", Convert.ToDecimal(Selected) });
            if (tabl == null)
            {
                return new List<DocumentRefersCategories>();
            }
            else
            {
                return (from r in tabl.AsEnumerable()
					select new DocumentRefersCategories
					{
						Id = (int)r["Id"],
 						Title = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
                        Alias = (string)((r["Alias"] == System.DBNull.Value) ? null : r["Alias"]),
                        Description = (string)((r["Description"] == System.DBNull.Value) ? null : r["Description"]),
 						Status = (Boolean)r["Status"] 						
					}).ToList();
            }

        }

        public static List<SelectListItem> GetListSelectItems(int IdCoQuan = 0, Boolean TaiLieuHop=false)
        {
            string str = "GetList";
            if (TaiLieuHop == true)
            {
                str = "GetListTaiLieuHop";
            }
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_DocumentRefersCategories",
                new string[] { "@flag", "@Selected", "@IdCoQuan" }, new object[] { str, true, IdCoQuan });
            List<SelectListItem> ListItems = (from r in tabl.AsEnumerable()
                                              select new SelectListItem
                                              {
                                                  Value = (string)((r["Id"] == System.DBNull.Value) ? null : r["Id"].ToString()),
                                                  Text = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
                                              }).ToList();

            ListItems.Insert(0, (new SelectListItem { Text = "--- Chọn danh mục  ---", Value = "0" }));
            return ListItems;

        }

        public static DocumentRefersCategories GetItem(decimal Id, string SecretId = null,Boolean TaiLieuHop=false)
        {
            string sql = "GetItem";
            if (TaiLieuHop == true)
            {
                sql = "GetItemTaiLieuHop";
            }
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_DocumentRefersCategories",
            new string[] { "@flag", "@Id" }, new object[] { sql, Id });
            return (from r in tabl.AsEnumerable()
                    select new DocumentRefersCategories
                    {
                        Id = (int)r["Id"],
 						Title = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
                        Alias = (string)((r["Alias"] == System.DBNull.Value) ? null : r["Alias"]),
                        Description = (string)((r["Description"] == System.DBNull.Value) ? null : r["Description"]),
                        MatKhau = (string)((r["MatKhau"] == System.DBNull.Value) ? null : r["MatKhau"]),
 						Status = (Boolean)r["Status"],
                        TaiLieuHop = (Boolean)r["TaiLieuHop"], 						
                        Ids = MyModels.Encode((int)r["Id"], SecretId),
                    }).FirstOrDefault();
        }

        public static DocumentRefersCategories GetItemTLH(decimal Id, string MatKhau)
        {
            string sql = "GetItemTaiLieuHop";
           
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_DocumentRefersCategories",
            new string[] { "@flag", "@Id" , "@MatKhau" }, new object[] { sql, Id, MatKhau });
            return (from r in tabl.AsEnumerable()
                    select new DocumentRefersCategories
                    {
                        Id = (int)r["Id"],
                        Title = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
                        Alias = (string)((r["Alias"] == System.DBNull.Value) ? null : r["Alias"]),
                        Description = (string)((r["Description"] == System.DBNull.Value) ? null : r["Description"]),
                        MatKhau = (string)((r["MatKhau"] == System.DBNull.Value) ? null : r["MatKhau"]),
                        Status = (Boolean)r["Status"],
                        TaiLieuHop = (Boolean)r["TaiLieuHop"],                        
                    }).FirstOrDefault();
        }

        public static dynamic SaveItem(DocumentRefersCategories dto)
        {
            dto.Title = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.Title);
            dto.Description = API.Models.MyHelper.StringHelper.RemoveTagsIntroText(dto.Description);

            dto.Alias = API.Models.MyHelper.StringHelper.UrlFriendly(dto.Title);

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_DocumentRefersCategories",
            new string[] { "@flag","@Id","@Title", "@Alias", "@Description","@Status","@CreatedBy","@ModifiedBy", "@TaiLieuHop", "@MatKhau", "@IdCoQuan" },
            new object[] { "SaveItem",dto.Id,dto.Title,dto.Alias,dto.Description,dto.Status,dto.CreatedBy,dto.ModifiedBy,dto.TaiLieuHop,dto.MatKhau,dto.IdCoQuan });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }
        public static dynamic DeleteItem(DocumentRefersCategories dto)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_DocumentRefersCategories",
            new string[] { "@flag", "@Id", "@ModifiedBy" },
            new object[] { "DeleteItem", dto.Id, dto.ModifiedBy});
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }
		
		public static dynamic UpdateStatus(DocumentRefersCategories dto)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_DocumentRefersCategories",
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
