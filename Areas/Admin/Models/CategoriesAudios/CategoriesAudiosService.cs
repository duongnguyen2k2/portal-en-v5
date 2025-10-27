using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using API.Areas.Admin.Models.CategoriesAudios;
using API.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace API.Areas.Admin.Models.CategoriesAudios
{
    public class CategoriesAudiosService
    {
        public static List<CategoriesAudios> GetListPagination(SearchCategoriesAudios dto, string SecretId)
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
            var tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_CategoriesAudios",
                new string[] { "@flag", "@CurrentPage", "@ItemsPerPage", "@Keyword"  },
                new object[] { "GetListPagination", dto.CurrentPage, dto.ItemsPerPage, dto.Keyword  });
            if (tabl == null)
            {
                return new List<CategoriesAudios>();
            }
            else
            {
                return (from r in tabl.AsEnumerable()
					select new CategoriesAudios
					{
						Id = (int)r["Id"],
 						Title = (string)r["Title"],
 						Description = (string)((r["Description"] == System.DBNull.Value) ? null : r["Description"]),
 						Alias = (string)((r["Alias"] == System.DBNull.Value) ? null : r["Alias"]),
 						Images = (string)((r["Images"] == System.DBNull.Value) ? null : r["Images"]),
                        Ordering = (int)((r["Ordering"] == System.DBNull.Value) ? 0 : r["Ordering"]),
 						Status = (Boolean)r["Status"],
                        Featured = (Boolean)r["Featured"],
                        TotalRows = (int)r["TotalRows"],
                        Ids = MyModels.Encode((int)r["Id"], SecretId)						
					}).ToList();
            }


        }

        public static List<SelectListItem> GetListItems(Boolean Selected = true)
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_CategoriesAudios",
                new string[] { "@flag", "@Selected" }, new object[] { "GetList", Convert.ToDecimal(Selected) });
            List<SelectListItem> ListItems = (from r in tabl.AsEnumerable()
                                              select new SelectListItem
                                              {
                                                  Value = (string)((r["Id"] == System.DBNull.Value) ? null : r["Id"].ToString()),
                                                  Text = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
                                              }).ToList();

            ListItems.Insert(0, (new SelectListItem { Text = "--- Chọn Danh mục ---", Value = "0" }));
            return ListItems;

        }

        public static List<CategoriesAudios> GetList(Boolean Selected = true)
        {
            
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_CategoriesAudios",
                new string[] { "@flag", "@Selected" }, new object[] { "GetList", Convert.ToDecimal(Selected)  });
            if (tabl == null)
            {
                return new List<CategoriesAudios>();
            }
            else
            {
                return (from r in tabl.AsEnumerable()
					select new CategoriesAudios
					{
						Id = (int)r["Id"],
 						Title = (string)r["Title"],
 						Alias = (string)r["Alias"],
 						Description = (string)((r["Description"] == System.DBNull.Value) ? null : r["Description"]),
                        Ordering = (int)((r["Ordering"] == System.DBNull.Value) ? 0 : r["Ordering"]),
                        Images = (string)((r["Images"] == System.DBNull.Value) ? null : r["Images"]),
 						Status = (Boolean)r["Status"] ,
                        Featured = (Boolean)r["Featured"] ,								
					}).ToList();
            }

        }

        public static List<CategoriesAudios> GetListFeatured(int IdCoQuan=1)
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_CategoriesAudios",
                new string[] { "@flag", "@IdCoQuan" }, new object[] { "GetListFeatured", IdCoQuan });
            if (tabl == null)
            {
                return new List<CategoriesAudios>();
            }
            else
            {
                return (from r in tabl.AsEnumerable()
                        select new CategoriesAudios
                        {
                            Id = (int)r["Id"],
                            Title = (string)r["Title"],
                            Alias = (string)((r["Alias"] == System.DBNull.Value) ? null : r["Alias"]),
                            Description = (string)((r["Description"] == System.DBNull.Value) ? null : r["Description"]),
                            Ordering = (int)((r["Ordering"] == System.DBNull.Value) ? 0 : r["Ordering"]),
                            Images = (string)((r["Images"] == System.DBNull.Value) ? null : r["Images"]),                            
                        }).ToList();
            }

        }

        public static CategoriesAudios GetItem(int Id, string SecretId = null)
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_CategoriesAudios",
            new string[] { "@flag", "@Id"}, new object[] { "GetItem", Id});
            return (from r in tabl.AsEnumerable()
                    select new CategoriesAudios
                    {
                        Id = (int)r["Id"],
 						Title = (string)r["Title"],
 						Description = (string)((r["Description"] == System.DBNull.Value) ? null : r["Description"]),
                        Ordering = (int)((r["Ordering"] == System.DBNull.Value) ? 0 : r["Ordering"]),
                        Alias = (string)((r["Alias"] == System.DBNull.Value) ? "" : r["Alias"]),
 						Images = (string)((r["Images"] == System.DBNull.Value) ? null : r["Images"]), 						
 						Status = (Boolean)r["Status"],
                        Featured = (Boolean)r["Featured"],                        
                        Ids = MyModels.Encode((int)r["Id"], SecretId),
                    }).FirstOrDefault();
        }

        public static dynamic SaveItem(CategoriesAudios dto)
        {
            dto.Title = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.Title);
            dto.Description = API.Models.MyHelper.StringHelper.RemoveTagsIntroText(dto.Description);
            dto.Images = API.Models.MyHelper.StringHelper.RemoveTagsLink(dto.Images);
            dto.Alias = API.Models.MyHelper.StringHelper.UrlFriendly(dto.Title);

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_CategoriesAudios",
            new string[] { "@flag","@Id","@Title", "@Alias", "@Featured", "@Ordering", "@Description","@Images","@Status","@CreatedBy","@ModifiedBy", "@ParentId"  },
            new object[] { "SaveItem",dto.Id,dto.Title,dto.Alias,dto.Featured,dto.Ordering, dto.Description,dto.Images,dto.Status,dto.CreatedBy,dto.ModifiedBy,dto.ParentId});
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }
        public static dynamic DeleteItem(CategoriesAudios dto)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_CategoriesAudios",
            new string[] { "@flag", "@Id", "@ModifiedBy" },
            new object[] { "DeleteItem", dto.Id, dto.ModifiedBy });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }
        public static dynamic UpdateStatus(CategoriesAudios dto)
        {
            
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_CategoriesAudios",
            new string[] { "@flag", "@Id","@Status", "@ModifiedBy"},
            new object[] { "UpdateStatus", dto.Id,dto.Status, dto.ModifiedBy});
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }

        public static dynamic UpdateFeatured(CategoriesAudios dto)
        {
           
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_CategoriesAudios",
            new string[] { "@flag", "@Id", "@Featured", "@ModifiedBy" },
            new object[] { "UpdateFeatured", dto.Id, dto.Featured, dto.ModifiedBy  });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }

    }
}
