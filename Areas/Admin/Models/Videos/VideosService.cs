using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using API.Areas.Admin.Models.Videos;
using API.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace API.Areas.Admin.Models.Videos
{
    public class VideosService
    {
        public static List<Videos> GetListPagination(SearchVideos dto, string SecretId, int CategoryId=0, string Culture = "vi")
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

            var tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Videos",
                new string[] { "@flag", "@CurrentPage", "@ItemsPerPage", "@Keyword", "@IdCoQuan", "@Status" , "@CatId" },
                new object[] { str_sql, dto.CurrentPage, dto.ItemsPerPage, dto.Keyword ,dto.IdCoQuan, Status,dto.CatId });
            if (tabl == null)
            {
                return new List<Videos>();
            }
            else
            {
                if (Culture == "vi")
                {
                    return (from r in tabl.AsEnumerable()
                            select new Videos
                            {
                                Id = (int)r["Id"],
                                Title = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
                                Alias = (string)((r["Alias"] == System.DBNull.Value) ? null : r["Alias"]),
                                IdType = (int)((r["IdType"] == System.DBNull.Value) ? null : r["IdType"]),
                                Description = (string)((r["Description"] == System.DBNull.Value) ? null : r["Description"]),
                                Link = (string)((r["Link"] == System.DBNull.Value) ? null : r["Link"]),
                                LinkSv = (string)((r["LinkSv"] == System.DBNull.Value) ? null : r["LinkSv"]),
                                CatId = (int)((r["CatId"] == System.DBNull.Value) ? null : r["CatId"]),
                                CatTitle = (string)((r["CatTitle"] == System.DBNull.Value) ? "" : r["CatTitle"]),
                                CreatedByTitle = (string)((r["CreatedByTitle"] == System.DBNull.Value) ? "" : r["CreatedByTitle"]),
                                Image = (string)((r["Image"] == System.DBNull.Value) ? null : r["Image"]),
                                Ordering = (int)((r["Ordering"] == System.DBNull.Value) ? 0 : r["Ordering"]),
                                Status = (Boolean)r["Status"],
                                Featured = (Boolean)r["Featured"],
                                Ids = MyModels.Encode((int)r["Id"], SecretId),
                                TotalRows = (int)r["TotalRows"],
                            }).ToList();
                }
                else
                {
                    return (from r in tabl.AsEnumerable()
                            select new Videos
                            {
                                Id = (int)r["Id"],
                                Title = (string)((r["TitleEn"] == System.DBNull.Value) ? null : r["TitleEn"]),
                                Alias = (string)((r["Alias"] == System.DBNull.Value) ? null : r["Alias"]),
                                IdType = (int)((r["IdType"] == System.DBNull.Value) ? null : r["IdType"]),
                                Description = (string)((r["DescriptionEn"] == System.DBNull.Value) ? null : r["DescriptionEn"]),
                                Link = (string)((r["Link"] == System.DBNull.Value) ? null : r["Link"]),
                                LinkSv = (string)((r["LinkSv"] == System.DBNull.Value) ? null : r["LinkSv"]),
                                CatId = (int)((r["CatId"] == System.DBNull.Value) ? null : r["CatId"]),
                                CatTitle = (string)((r["CatTitle"] == System.DBNull.Value) ? "" : r["CatTitle"]),
                                CreatedByTitle = (string)((r["CreatedByTitle"] == System.DBNull.Value) ? "" : r["CreatedByTitle"]),
                                Image = (string)((r["Image"] == System.DBNull.Value) ? null : r["Image"]),
                                Ordering = (int)((r["Ordering"] == System.DBNull.Value) ? 0 : r["Ordering"]),
                                Status = (Boolean)r["Status"],
                                Featured = (Boolean)r["Featured"],
                                Ids = MyModels.Encode((int)r["Id"], SecretId),
                                TotalRows = (int)r["TotalRows"],
                            }).ToList();
                }
            }
        }

        public static List<SelectListItem> GetListSelectItemsType(Boolean Selected = true)
        {
            List<SelectListItem> ListItems = new List<SelectListItem>();
            ListItems.Insert(0, (new SelectListItem { Text = "Đưa lên server", Value = "0" }));
            ListItems.Insert(1, (new SelectListItem { Text = "Youtube", Value = "1" }));
            ListItems.Insert(2, (new SelectListItem { Text = "Youtube List", Value = "2" }));
            return ListItems;
        }

        public static List<Videos> GetListNew(int IdCoQuan)
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Videos",
                new string[] { "@flag", "@IdCoQuan" }, new object[] { "GetListNew", IdCoQuan });
            if (tabl == null)
            {
                return new List<Videos>();
            }
            else
            {
                return (from r in tabl.AsEnumerable()
                        select new Videos
                        {
                            Id = (int)r["Id"],
                            Title = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
                            Alias = (string)((r["Alias"] == System.DBNull.Value) ? "" : r["Alias"]),
                            IdType = (int)((r["IdType"] == System.DBNull.Value) ? null : r["IdType"]),
                            LinkSv = (string)((r["LinkSv"] == System.DBNull.Value) ? null : r["LinkSv"]),
                            Description = (string)((r["Description"] == System.DBNull.Value) ? null : r["Description"]),
                            Link = (string)((r["Link"] == System.DBNull.Value) ? null : r["Link"]),
                            CatId = (int)((r["CatId"] == System.DBNull.Value) ? null : r["CatId"]),
                            Image = (string)((r["Image"] == System.DBNull.Value) ? null : r["Image"]),
                            Ordering = (int)((r["Ordering"] == System.DBNull.Value) ? 0 : r["Ordering"]),
                            CreatedDate = (DateTime)((r["CreatedDate"] == System.DBNull.Value) ? DateTime.Now : r["CreatedDate"]),
                        }).ToList();
            }

        }
        public static List<Videos> GetListFeatured(int IdCoQuan, string Culture = "vi")
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Videos",
                new string[] { "@flag", "@IdCoQuan" }, new object[] { "GetListFeatured",IdCoQuan });
            if (tabl == null)
            {
                return new List<Videos>();
            }
            else
            {
                if(Culture == "vi")
                {
                    return (from r in tabl.AsEnumerable()
                            select new Videos
                            {
                                Id = (int)r["Id"],
                                Title = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
                                Alias = (string)((r["Alias"] == System.DBNull.Value) ? null : r["Alias"]),
                                IdType = (int)((r["IdType"] == System.DBNull.Value) ? null : r["IdType"]),
                                LinkSv = (string)((r["LinkSv"] == System.DBNull.Value) ? null : r["LinkSv"]),
                                Description = (string)((r["Description"] == System.DBNull.Value) ? null : r["Description"]),
                                Link = (string)((r["Link"] == System.DBNull.Value) ? null : r["Link"]),
                                CatId = (int)((r["CatId"] == System.DBNull.Value) ? null : r["CatId"]),
                                Image = (string)((r["Image"] == System.DBNull.Value) ? null : r["Image"]),
                                Ordering = (int)((r["Ordering"] == System.DBNull.Value) ? 0 : r["Ordering"]),
                                CreatedDate = (DateTime)((r["CreatedDate"] == System.DBNull.Value) ? 0 : r["CreatedDate"]),
                            }).ToList();
                }
                else
                {
                    return (from r in tabl.AsEnumerable()
                            select new Videos
                            {
                                Id = (int)r["Id"],
                                Title = (string)((r["TitleEn"] == System.DBNull.Value) ? r["Title"] : r["TitleEn"]),
                                Alias = (string)((r["Alias"] == System.DBNull.Value) ? null : r["Alias"]),
                                IdType = (int)((r["IdType"] == System.DBNull.Value) ? null : r["IdType"]),
                                LinkSv = (string)((r["LinkSv"] == System.DBNull.Value) ? null : r["LinkSv"]),
                                Description = (string)((r["DescriptionEn"] == System.DBNull.Value) ? null : r["DescriptionEn"]),
                                Link = (string)((r["Link"] == System.DBNull.Value) ? null : r["Link"]),
                                CatId = (int)((r["CatId"] == System.DBNull.Value) ? null : r["CatId"]),
                                Image = (string)((r["Image"] == System.DBNull.Value) ? null : r["Image"]),
                                Ordering = (int)((r["Ordering"] == System.DBNull.Value) ? 0 : r["Ordering"]),
                                CreatedDate = (DateTime)((r["CreatedDate"] == System.DBNull.Value) ? 0 : r["CreatedDate"]),
                            }).ToList();
                }

            }

        }

        public static List<Videos> GetListRelative(int IdCoQuan, int CatId, string Culture = "vi")
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Videos",
                new string[] { "@flag", "@IdCoQuan", "@CatId" }, new object[] { "GetListRelative", IdCoQuan, CatId });
            if (tabl == null)
            {
                return new List<Videos>();
            }
            else
            {
                if (Culture == "vi")
                {
                    return (from r in tabl.AsEnumerable()
                            select new Videos
                            {
                                Id = (int)r["Id"],
                                Title = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
                                Alias = (string)((r["Alias"] == System.DBNull.Value) ? null : r["Alias"]),
                                IdType = (int)((r["IdType"] == System.DBNull.Value) ? null : r["IdType"]),
                                LinkSv = (string)((r["LinkSv"] == System.DBNull.Value) ? null : r["LinkSv"]),
                                Description = (string)((r["Description"] == System.DBNull.Value) ? null : r["Description"]),
                                Link = (string)((r["Link"] == System.DBNull.Value) ? null : r["Link"]),
                                CatId = (int)((r["CatId"] == System.DBNull.Value) ? null : r["CatId"]),
                                Image = (string)((r["Image"] == System.DBNull.Value) ? null : r["Image"]),
                                Ordering = (int)((r["Ordering"] == System.DBNull.Value) ? 0 : r["Ordering"]),
                                CreatedDate = (DateTime)((r["CreatedDate"] == System.DBNull.Value) ? 0 : r["CreatedDate"]),
                            }).ToList();
                }
                else
                {
                    return (from r in tabl.AsEnumerable()
                            select new Videos
                            {
                                Id = (int)r["Id"],
                                Title = (string)((r["TitleEn"] == System.DBNull.Value) ? r["Title"] : r["TitleEn"]),
                                Alias = (string)((r["Alias"] == System.DBNull.Value) ? null : r["Alias"]),
                                IdType = (int)((r["IdType"] == System.DBNull.Value) ? null : r["IdType"]),
                                LinkSv = (string)((r["LinkSv"] == System.DBNull.Value) ? null : r["LinkSv"]),
                                Description = (string)((r["DescriptionEn"] == System.DBNull.Value) ? null : r["DescriptionEn"]),
                                Link = (string)((r["Link"] == System.DBNull.Value) ? null : r["Link"]),
                                CatId = (int)((r["CatId"] == System.DBNull.Value) ? null : r["CatId"]),
                                Image = (string)((r["Image"] == System.DBNull.Value) ? null : r["Image"]),
                                Ordering = (int)((r["Ordering"] == System.DBNull.Value) ? 0 : r["Ordering"]),
                                CreatedDate = (DateTime)((r["CreatedDate"] == System.DBNull.Value) ? 0 : r["CreatedDate"]),
                            }).ToList();
                }

            }

        }

        public static List<Videos> GetList(Boolean Selected = true, string Culture = "vi")
        {
            
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Videos",
                new string[] { "@flag", "@Selected" }, new object[] { "GetList", Convert.ToDecimal(Selected) });
            if (tabl == null)
            {
                return new List<Videos>();
            }
            else
            {
                return (from r in tabl.AsEnumerable()
					select new Videos
					{
						Id = (int)r["Id"],
 						Title = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
                        IdType = (int)((r["IdType"] == System.DBNull.Value) ? null : r["IdType"]),
                        Description = (string)((r["Description"] == System.DBNull.Value) ? null : r["Description"]),
 						Link = (string)((r["Link"] == System.DBNull.Value) ? null : r["Link"]),
                        LinkSv = (string)((r["LinkSv"] == System.DBNull.Value) ? null : r["LinkSv"]),
                        CatId = (int)((r["CatId"] == System.DBNull.Value) ? null : r["CatId"]),
 						Image = (string)((r["Image"] == System.DBNull.Value) ? null : r["Image"]),
                        Ordering = (int)((r["Ordering"] == System.DBNull.Value) ? 0 : r["Ordering"]),
                    }).ToList();
            }

        }

        public static Videos GetItemAdmin(decimal Id, string SecretId = null, string Culture = "vi")
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Videos",
            new string[] { "@flag", "@Id" }, new object[] { "GetItem", Id });
            return (from r in tabl.AsEnumerable()
                    select new Videos
                    {
                        Id = (int)r["Id"],
                        Title = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
                        TitleEn = (string)((r["TitleEn"] == System.DBNull.Value) ? null : r["TitleEN"]),
                        Alias = (string)((r["Alias"] == System.DBNull.Value) ? null : r["Alias"]),
                        IdType = (int)((r["IdType"] == System.DBNull.Value) ? null : r["IdType"]),
                        Description = (string)((r["Description"] == System.DBNull.Value) ? null : r["Description"]),
                        DescriptionEn = (string)((r["DescriptionEn"] == System.DBNull.Value) ? null : r["DescriptionEn"]),
                        Link = (string)((r["Link"] == System.DBNull.Value) ? null : r["Link"]),
                        LinkSv = (string)((r["LinkSv"] == System.DBNull.Value) ? null : r["LinkSv"]),
                        CatId = (int)((r["CatId"] == System.DBNull.Value) ? null : r["CatId"]),
                        Image = (string)((r["Image"] == System.DBNull.Value) ? null : r["Image"]),
                        Status = (Boolean)r["Status"],
                        Featured = (Boolean)r["Featured"],
                        Deleted = (Boolean)r["Deleted"],
                        CreatedBy = (int)r["CreatedBy"],
                        CreatedDate = (DateTime)((r["CreatedDate"] == System.DBNull.Value) ? null : r["CreatedDate"]),
                        CreatedByTitle = (string)((r["CreatedByTitle"] == System.DBNull.Value) ? "" : r["CreatedByTitle"]),
                        ModifiedBy = (int?)((r["ModifiedBy"] == System.DBNull.Value) ? null : r["ModifiedBy"]),
                        ModifiedDate = (DateTime?)((r["ModifiedDate"] == System.DBNull.Value) ? null : r["ModifiedDate"]),
                        Ordering = (int)((r["Ordering"] == System.DBNull.Value) ? 0 : r["Ordering"]),
                        Ids = MyModels.Encode((int)r["Id"], SecretId),
                    }).FirstOrDefault();
        }


        public static Videos GetItem(decimal Id, string SecretId = null, string Culture = "vi")
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Videos",
            new string[] { "@flag", "@Id" }, new object[] { "GetItem", Id });
            if(Culture == "vi")
            {
                return (from r in tabl.AsEnumerable()
                        select new Videos
                        {
                            Id = (int)r["Id"],
                            Title = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
                            Alias = (string)((r["Alias"] == System.DBNull.Value) ? null : r["Alias"]),
                            IdType = (int)((r["IdType"] == System.DBNull.Value) ? null : r["IdType"]),
                            Description = (string)((r["Description"] == System.DBNull.Value) ? null : r["Description"]),
                            Link = (string)((r["Link"] == System.DBNull.Value) ? null : r["Link"]),
                            LinkSv = (string)((r["LinkSv"] == System.DBNull.Value) ? null : r["LinkSv"]),
                            CatId = (int)((r["CatId"] == System.DBNull.Value) ? null : r["CatId"]),
                            Image = (string)((r["Image"] == System.DBNull.Value) ? null : r["Image"]),
                            Status = (Boolean)r["Status"],
                            Featured = (Boolean)r["Featured"],
                            Deleted = (Boolean)r["Deleted"],
                            CreatedBy = (int)r["CreatedBy"],
                            CreatedDate = (DateTime)((r["CreatedDate"] == System.DBNull.Value) ? null : r["CreatedDate"]),
                            CreatedByTitle = (string)((r["CreatedByTitle"] == System.DBNull.Value) ? "" : r["CreatedByTitle"]),
                            ModifiedBy = (int?)((r["ModifiedBy"] == System.DBNull.Value) ? null : r["ModifiedBy"]),
                            ModifiedDate = (DateTime?)((r["ModifiedDate"] == System.DBNull.Value) ? null : r["ModifiedDate"]),
                            Ordering = (int)((r["Ordering"] == System.DBNull.Value) ? 0 : r["Ordering"]),
                            Ids = MyModels.Encode((int)r["Id"], SecretId),
                        }).FirstOrDefault();
            }
            else
            {
                return (from r in tabl.AsEnumerable()
                        select new Videos
                        {
                            Id = (int)r["Id"],
                            Title = (string)((r["TitleEn"] == System.DBNull.Value) ? null : r["TitleEN"]),
                            Alias = (string)((r["Alias"] == System.DBNull.Value) ? null : r["Alias"]),
                            IdType = (int)((r["IdType"] == System.DBNull.Value) ? null : r["IdType"]),
                            Description = (string)((r["DescriptionEn"] == System.DBNull.Value) ? null : r["DescriptionEn"]),
                            Link = (string)((r["Link"] == System.DBNull.Value) ? null : r["Link"]),
                            LinkSv = (string)((r["LinkSv"] == System.DBNull.Value) ? null : r["LinkSv"]),
                            CatId = (int)((r["CatId"] == System.DBNull.Value) ? null : r["CatId"]),
                            Image = (string)((r["Image"] == System.DBNull.Value) ? null : r["Image"]),
                            Status = (Boolean)r["Status"],
                            Featured = (Boolean)r["Featured"],
                            Deleted = (Boolean)r["Deleted"],
                            CreatedBy = (int)r["CreatedBy"],
                            CreatedDate = (DateTime)((r["CreatedDate"] == System.DBNull.Value) ? null : r["CreatedDate"]),
                            CreatedByTitle = (string)((r["CreatedByTitle"] == System.DBNull.Value) ? "" : r["CreatedByTitle"]),
                            ModifiedBy = (int?)((r["ModifiedBy"] == System.DBNull.Value) ? null : r["ModifiedBy"]),
                            ModifiedDate = (DateTime?)((r["ModifiedDate"] == System.DBNull.Value) ? null : r["ModifiedDate"]),
                            Ordering = (int)((r["Ordering"] == System.DBNull.Value) ? 0 : r["Ordering"]),
                            Ids = MyModels.Encode((int)r["Id"], SecretId),
                        }).FirstOrDefault();
            }
        }

        public static dynamic SaveItem(Videos dto)
        {
			dto.Title = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.Title);
			dto.Description = API.Models.MyHelper.StringHelper.RemoveTagsIntroText(dto.Description);
			dto.Image = API.Models.MyHelper.StringHelper.RemoveTagsLink(dto.Image);
			dto.Link = API.Models.MyHelper.StringHelper.RemoveTagsLink(dto.Link);
			dto.LinkSv = API.Models.MyHelper.StringHelper.RemoveTagsLink(dto.LinkSv);
			dto.Alias = API.Models.MyHelper.StringHelper.UrlFriendly(dto.Title);
            if (dto.IdType == 1)
            {                
				dto.Link = dto.Link.Replace("https://www.youtube.com/watch?v=", "");
                dto.Link = dto.Link.Replace("www.youtube.com/watch?v=", "");
                dto.Link = dto.Link.Replace("youtube.com/watch?v=", "");
                dto.Link = dto.Link.Replace("https://youtu.be/", "");
				dto.Link = dto.Link.Replace("https://www.youtube.com/embed/", "");
                dto.Link = dto.Link.Split("&").First();
            }


            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Videos",
            new string[] { "@flag","@Id","@Title", "@TitleEn", "@Alias", "@Featured", "@Description", "@DescriptionEn","@Link", "@LinkSv", "@CatId","@Image","@Status","@Deleted","@CreatedBy","@CreatedDate","@ModifiedBy","@ModifiedDate", "@IdType", "@IdCoQuan", "@Ordering" },
            new object[] { "SaveItem",dto.Id,dto.Title, dto.TitleEn, dto.Alias,dto.Featured, dto.Description, dto.DescriptionEn, dto.Link,dto.LinkSv,dto.CatId,dto.Image,dto.Status,dto.Deleted,dto.CreatedBy,dto.CreatedDate,dto.ModifiedBy,dto.ModifiedDate,dto.IdType,dto.IdCoQuan,dto.Ordering });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }
        public static dynamic DeleteItem(Videos dto)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Videos",
            new string[] { "@flag", "@Id", "@ModifiedBy" },
            new object[] { "DeleteItem", dto.Id, dto.ModifiedBy});
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }
		
		public static dynamic UpdateStatus(Videos dto)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Videos",
            new string[] { "@flag", "@Id","@Status", "@ModifiedBy" },
            new object[] { "UpdateStatus", dto.Id,dto.Status, dto.ModifiedBy });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }

        public static dynamic UpdateFeatured(Videos dto)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Videos",
            new string[] { "@flag", "@Id", "@Featured", "@ModifiedBy" },
            new object[] { "UpdateFeatured", dto.Id, dto.Featured, dto.ModifiedBy });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }


    }
}
