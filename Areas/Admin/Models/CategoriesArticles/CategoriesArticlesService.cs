using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using API.Areas.Admin.Models.CategoriesArticles;
using API.Areas.Admin.Models.CategoriesLanguage;
using API.Areas.Admin.Models.SystemLog;
using API.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace API.Areas.Admin.Models.CategoriesArticles
{
    public class CategoriesArticlesService
    {
        public static List<CategoriesArticles> GetListPagination(SearchCategoriesArticles dto, string SecretId, string Culture = "all")
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
            var tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_CategoriesArticles",
                new string[] { "@flag", "@CurrentPage", "@ItemsPerPage", "@Keyword", "@IdCoQuan" },
                new object[] { "GetListPagination", dto.CurrentPage, dto.ItemsPerPage, dto.Keyword, dto.IdCoQuan });
            if (tabl == null)
            {
                return new List<CategoriesArticles>();
            }
            else
            {
                return (from r in tabl.AsEnumerable()
					select new CategoriesArticles
					{
						Id = (int)r["Id"],
 						Title = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
                        TitleRoot = (string)((r["TitleRoot"] == System.DBNull.Value) ? null : r["TitleRoot"]),
 						Alias = (string)((r["Alias"] == System.DBNull.Value) ? null : r["Alias"]),
                        Icon = (string)((r["Icon"] == System.DBNull.Value) ? null : r["Icon"]), 						
 						ParentId = (int?)((r["ParentId"] == System.DBNull.Value) ? null : r["ParentId"]),
 						Status = (Boolean)r["Status"],
                        FeaturedHome = (Boolean)((r["FeaturedHome"] == System.DBNull.Value) ? false : r["FeaturedHome"]),
                        Layout = (Boolean)((r["Layout"] == System.DBNull.Value) ? false : r["Layout"]),
                        Hits = (int?)((r["Hits"] == System.DBNull.Value) ? null : r["Hits"]),
                        Images = (string)((r["Images"] == System.DBNull.Value) ? null : r["Images"]),
                        Params = (string)((r["Params"] == System.DBNull.Value) ? null : r["Params"]),
                        Ordering = (int)((r["Ordering"] == System.DBNull.Value) ? 0 : r["Ordering"]),
                        Ids = MyModels.Encode((int)r["Id"], SecretId),						
					}).ToList();
            }


        }

        public static List<CategoriesArticles> GetListImport()
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_CategoriesArticles",
                new string[] { "@flag"}, new object[] { "GetListImport"});
            if (tabl == null)
            {
                return new List<CategoriesArticles>();
            }
            else
            {
                return (from r in tabl.AsEnumerable()
                        select new CategoriesArticles
                        {
                            Id = (int)r["Id"],
                            Title = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
                            Alias = (string)((r["Alias"] == System.DBNull.Value) ? null : r["Alias"]),
                            Icon = (string)((r["Icon"] == System.DBNull.Value) ? null : r["Icon"]),
                            ParentId = (int?)((r["ParentId"] == System.DBNull.Value) ? null : r["ParentId"]),
                            PageStart = (int)((r["PageStart"] == System.DBNull.Value) ? 0 : r["PageStart"]),
                            PageEnd = (int)((r["PageEnd"] == System.DBNull.Value) ? 0 : r["PageEnd"]),                            
                            LinkSiteRoot = (string)((r["LinkSiteRoot"] == System.DBNull.Value) ? null : r["LinkSiteRoot"]),

                        }).ToList();
            }

        }

        public static List<CategoriesArticles> GetList(Boolean Selected = true,int IdCoQuan=1, string Culture = "all")
        {
            Culture = API.Models.MyHelper.StringHelper.ConverLan(Culture);

            string sql = "GetList";

            if (Culture != "vi")
            {
                sql = sql + "_" + Culture.ToUpper();
            }
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_CategoriesArticles",
                new string[] { "@flag", "@Selected" , "@IdCoQuan" }, new object[] { sql, Convert.ToDecimal(Selected), IdCoQuan });
            if (tabl == null)
            {
                return new List<CategoriesArticles>();
            }
            else
            {
                return (from r in tabl.AsEnumerable()
					select new CategoriesArticles
					{
						Id = (int)r["Id"],
 						Title = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
 						Alias = (string)((r["Alias"] == System.DBNull.Value) ? null : r["Alias"]),
                        Icon = (string)((r["Icon"] == System.DBNull.Value) ? null : r["Icon"]),
 						ParentId = (int?)((r["ParentId"] == System.DBNull.Value) ? null : r["ParentId"]),
                        Images = (string)((r["Images"] == System.DBNull.Value) ? null : r["Images"]),                        
                        FeaturedHome = (Boolean)((r["FeaturedHome"] == System.DBNull.Value) ? false : r["FeaturedHome"]),
                        Layout = (Boolean)((r["Layout"] == System.DBNull.Value) ? false : r["Layout"]),

                    }).ToList();
            }

        }

        public static List<CategoriesArticles> GetListAll(Boolean Selected = true, int IdCoQuan = 1)
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_CategoriesArticles",
                new string[] { "@flag", "@Selected", "@IdCoQuan" }, new object[] { "GetListAll", Convert.ToDecimal(Selected), IdCoQuan });
            if (tabl == null)
            {
                return new List<CategoriesArticles>();
            }
            else
            {
                return (from r in tabl.AsEnumerable()
                        select new CategoriesArticles
                        {
                            Id = (int)r["Id"],
                            Title = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
                            Alias = (string)((r["Alias"] == System.DBNull.Value) ? null : r["Alias"]),
                            Icon = (string)((r["Icon"] == System.DBNull.Value) ? null : r["Icon"]),
                            ParentId = (int?)((r["ParentId"] == System.DBNull.Value) ? null : r["ParentId"]),
                            Images = (string)((r["Images"] == System.DBNull.Value) ? null : r["Images"]),
                            Status = (Boolean)r["Status"],

                            Metadesc = (string)((r["Metadesc"] == System.DBNull.Value) ? null : r["Metadesc"]),
                            Metakey = (string)((r["Metakey"] == System.DBNull.Value) ? null : r["Metakey"]),
                            Metadata = (string)((r["Metadata"] == System.DBNull.Value) ? null : r["Metadata"]),

                            Params = (string)((r["Params"] == System.DBNull.Value) ? null : r["Params"]),
                            Ordering = (int)((r["Ordering"] == System.DBNull.Value) ? null : r["Ordering"]),
                            Hits = (int?)((r["Hits"] == System.DBNull.Value) ? null : r["Hits"]),
                        }).ToList();
            }

        }
        public static List<CategoriesArticles> GetListFeaturedHome(int IdCoQuan=1, string Culture = "all")
        {
            Culture = API.Models.MyHelper.StringHelper.ConverLan(Culture);

            string sql = "GetListFeaturedHome";

            if (Culture != "vi")
            {
                sql = sql + "_" + Culture.ToUpper();
            }
            if (Startup._config["flagSystemLog"] =="1")
            {
                SystemLog.SystemLog itemLog = new SystemLog.SystemLog()
                {
                    Title = "GetListFeaturedHome",
                    Description = sql,
                    IdCoQuan = IdCoQuan
                };
                SystemLogService.SaveItem(itemLog);
            }
            

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_CategoriesArticles",
                new string[] { "@flag", "@IdCoQuan" }, new object[] { sql, IdCoQuan });
            if (tabl == null)
            {
                return new List<CategoriesArticles>();
            }
            else
            {
                return (from r in tabl.AsEnumerable()
                        select new CategoriesArticles
                        {
                            Id = (int)r["Id"],
                            Icon = (string)((r["Icon"] == System.DBNull.Value) ? "" : r["Icon"]),
                            Description = (string)((r["Description"] == System.DBNull.Value) ? "" : r["Description"]),
                            Title = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
                            Alias = (string)((r["Alias"] == System.DBNull.Value) ? null : r["Alias"]),
                            ParentId = (int?)((r["ParentId"] == System.DBNull.Value) ? null : r["ParentId"]),
                            Layout = (Boolean)((r["Layout"] == System.DBNull.Value) ? false : r["Layout"]),
                            Culture = Culture,
                        }).ToList();
            }

        }
        public static List<SelectListItem> GetListItems(Boolean Selected = true, int ParentId=0, string Culture = "all", int Status_id = -1)
        {
            string sql = "GetList";
            if (ParentId > 0) {
                sql = "GetListCatMenuChild";
            }
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_CategoriesArticles",
                new string[] { "@flag", "@Selected", "@ParentId", "@Status_id" }, new object[] { sql, Convert.ToDecimal(Selected), ParentId, Status_id });
            List<SelectListItem> ListItems = (from r in tabl.AsEnumerable()
                                              select new SelectListItem
                                              {
                                                  Value = (string)((r["Id"] == System.DBNull.Value) ? null : r["Id"].ToString()),
                                                  Text = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
                                              }).ToList();

            ListItems.Insert(0, (new SelectListItem { Text = "Chọn Danh mục", Value = "0" }));
            return ListItems;

        }

        public static CategoriesArticles GetItem(decimal Id, string SecretId = null,int IdCoQuan=1, string Culture = "vi")
        {
            string sql = "GetItem";
            Culture = API.Models.MyHelper.StringHelper.ConverLan(Culture);
            if (Culture != "vi")
            {
                sql = sql + "_" + Culture.ToUpper();
            }
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_CategoriesArticles",
            new string[] { "@flag", "@Id", "@IdCoQuan" }, new object[] { sql, Id, IdCoQuan });
            CategoriesArticles Item = (from r in tabl.AsEnumerable()
                    select new CategoriesArticles
                    {
                        Id = (int)r["Id"],
 						Title = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
 						Alias = (string)((r["Alias"] == System.DBNull.Value) ? null : r["Alias"]),
                        Icon = (string)((r["Icon"] == System.DBNull.Value) ? null : r["Icon"]),
 						Description = (string)((r["Description"] == System.DBNull.Value) ? null : r["Description"]),
 						ParentId = (int?)((r["ParentId"] == System.DBNull.Value) ? null : r["ParentId"]),
 						Status = (Boolean)r["Status"], 						
                        FeaturedHome = (Boolean)((r["FeaturedHome"] == System.DBNull.Value) ? false : r["FeaturedHome"]),
                        Layout = (Boolean)((r["Layout"] == System.DBNull.Value) ? false : r["Layout"]),
                        Metadesc = (string)((r["Metadesc"] == System.DBNull.Value) ? null : r["Metadesc"]),
 						Metakey = (string)((r["Metakey"] == System.DBNull.Value) ? null : r["Metakey"]),
 						Metadata = (string)((r["Metadata"] == System.DBNull.Value) ? null : r["Metadata"]),
                        Images = (string)((r["Images"] == System.DBNull.Value) ? null : r["Images"]),
                        Params = (string)((r["Params"] == System.DBNull.Value) ? null : r["Params"]),
                        Ordering = (int)((r["Ordering"] == System.DBNull.Value) ? 0 : r["Ordering"]),
                        OrderingHome = (int)((r["OrderingHome"] == System.DBNull.Value) ? 0 : r["OrderingHome"]),
                        Hits = (int?)((r["Hits"] == System.DBNull.Value) ? null : r["Hits"]),
                        Ids = MyModels.Encode((int)r["Id"], SecretId),
                    }).FirstOrDefault();

            if (Item != null && Item.Id > 0)
            {
                if (Item.Title == null)
                {
                    Item.Title = "";
                }
                if (Item.Metadata != null)
                {
                    Item.MetadataCV = JsonConvert.DeserializeObject<API.Models.MetaData>(Item.Metadata);
                }

            }
            return Item;
        }
        public static CategoriesArticles GetItemByAlias(string Alias, string SecretId = null, string Culture = "all")
        {
            string sql = "GetItemByAlias";
            Culture = API.Models.MyHelper.StringHelper.ConverLan(Culture);
            if (Culture != "vi")
            {
                sql = sql + "_" + Culture.ToUpper();
            }

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_CategoriesArticles",
            new string[] { "@flag", "@Alias" }, new object[] { "GetItemByAlias", Alias });
            CategoriesArticles Item = (from r in tabl.AsEnumerable()
                    select new CategoriesArticles
                    {
                        Id = (int)r["Id"],
                        Title = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
                        Alias = (string)((r["Alias"] == System.DBNull.Value) ? null : r["Alias"]),
                        Icon = (string)((r["Icon"] == System.DBNull.Value) ? null : r["Icon"]),
                        Description = (string)((r["Description"] == System.DBNull.Value) ? null : r["Description"]),
                        ParentId = (int?)((r["ParentId"] == System.DBNull.Value) ? null : r["ParentId"]),
                        Status = (Boolean)r["Status"],                        
                        Metadesc = (string)((r["Metadesc"] == System.DBNull.Value) ? null : r["Metadesc"]),
                        Metakey = (string)((r["Metakey"] == System.DBNull.Value) ? null : r["Metakey"]),
                        Metadata = (string)((r["Metadata"] == System.DBNull.Value) ? null : r["Metadata"]),
                        Images = (string)((r["Images"] == System.DBNull.Value) ? null : r["Images"]),
                        Params = (string)((r["Params"] == System.DBNull.Value) ? null : r["Params"]),
                        Ordering = (int)((r["Ordering"] == System.DBNull.Value) ? 0 : r["Ordering"]),
                        Hits = (int?)((r["Hits"] == System.DBNull.Value) ? null : r["Hits"]),
                        Ids = MyModels.Encode((int)r["Id"], SecretId),
                    }).FirstOrDefault();

            if (Item != null && Item.Id > 0)
            {
                if (Item.Title == null)
                {
                    Item.Title = "";
                }
                if (Item.Metadata != null)
                {
                    Item.MetadataCV = JsonConvert.DeserializeObject<API.Models.MetaData>(Item.Metadata);
                }

            }

            return Item;
        }

        public static List<CategoriesArticles> GetListChild(int Id, string Culture = "all")
        {
            string sql = "GetListChild";
            Culture = API.Models.MyHelper.StringHelper.ConverLan(Culture);
            if (Culture != "vi")
            {
                sql = sql + "_" + Culture.ToUpper();
            }

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_CategoriesArticles",
            new string[] { "@flag", "@Id" }, new object[] { sql, Id });
            return (from r in tabl.AsEnumerable()
                    select new CategoriesArticles
                    {
                        Id = (int)r["Id"],
                        Title = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
                        Alias = (string)((r["Alias"] == System.DBNull.Value) ? null : r["Alias"]),
                        Icon = (string)((r["Icon"] == System.DBNull.Value) ? null : r["Icon"]),
                        Description = (string)((r["Description"] == System.DBNull.Value) ? null : r["Description"]),
                        ParentId = (int?)((r["ParentId"] == System.DBNull.Value) ? null : r["ParentId"]),
                        Status = (Boolean)r["Status"],                       
                        Metadesc = (string)((r["Metadesc"] == System.DBNull.Value) ? null : r["Metadesc"]),
                        Metakey = (string)((r["Metakey"] == System.DBNull.Value) ? null : r["Metakey"]),
                        Metadata = (string)((r["Metadata"] == System.DBNull.Value) ? null : r["Metadata"]),
                        Images = (string)((r["Images"] == System.DBNull.Value) ? null : r["Images"]),
                        Params = (string)((r["Params"] == System.DBNull.Value) ? null : r["Params"]),
                        Ordering = (int)((r["Ordering"] == System.DBNull.Value) ? 0 : r["Ordering"]),
                        Hits = (int?)((r["Hits"] == System.DBNull.Value) ? null : r["Hits"]),                        
                    }).ToList();
        }

        public static dynamic SaveItem(CategoriesArticles dto, string Culture = "all")
        {
            dto.Title = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.Title);
            dto.Description = API.Models.MyHelper.StringHelper.RemoveTagsIntroText(dto.Description);

            if (dto.Alias == null || dto.Alias == "")
            {
                dto.Alias = API.Models.MyHelper.StringHelper.UrlFriendly(dto.Title);
            }
            if (dto.MetadataCV != null)
            {
                if (dto.MetadataCV.MetaTitle == null || dto.MetadataCV.MetaTitle == "")
                {
                    dto.MetadataCV.MetaTitle = dto.Title;
                }
            }
            else
            {
                dto.MetadataCV = new API.Models.MetaData() { MetaTitle = dto.Title };
            }
           
            dto.Metadata = JsonConvert.SerializeObject(dto.MetadataCV);
            if(dto.Culture != null)
            {
                if (dto.Culture.ToLower() == "vi")
                {
                    DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_CategoriesArticles",
                  new string[] { "@flag", "@Id", "@Title", "@Alias", "@Description", "@ParentId", "@Status", "@Deleted", "@CreatedBy", "@ModifiedBy", "@Metadesc", "@Metakey", "@Metadata", "@Images", "@Params", "@Ordering", "@Hits", "@FeaturedHome", "@IdCoQuan", "@Icon", "@Layout" },
                  new object[] { "SaveItem", dto.Id, dto.Title, dto.Alias, dto.Description, dto.ParentId, dto.Status, dto.Deleted, dto.CreatedBy, dto.ModifiedBy, dto.Metadesc, dto.Metakey, dto.Metadata, dto.Images, dto.Params, dto.Ordering, dto.Hits, dto.FeaturedHome, dto.IdCoQuan, dto.Icon, dto.Layout});
                    return (from r in tabl.AsEnumerable()
                            select new
                            {
                                N = (int)(r["N"]),
                            }).FirstOrDefault();
                }
                else
                {
                    var a = CategoriesLanguageService.SaveItem(new CategoriesLanguage.CategoriesLanguage()
                    {
                        TypeId = 1,
                        IdRoot = dto.Id,
                        LanguageCode = dto.Culture,
                        Title = dto.Title,
                        Alias = dto.Alias,
                        Description = dto.Description,
                        Metakey = dto.Metakey,
                        Metadesc = dto.Metadesc,
                        Metadata = dto.Metadata,
                        MetadataCV = dto.MetadataCV,
                    });
                    return new { N = dto.Id };
                }
            }
            else
            {
                var a = CategoriesLanguageService.SaveItem(new CategoriesLanguage.CategoriesLanguage()
                {
                    TypeId = 1,
                    IdRoot = dto.Id,
                    LanguageCode = "vi",
                    Title = dto.Title,
                    Alias = dto.Alias,
                    Description = dto.Description,
                    Metakey = dto.Metakey,
                    Metadesc = dto.Metadesc,
                    Metadata = dto.Metadata,
                    MetadataCV = dto.MetadataCV,
                });
                return new { N = dto.Id };
            }
        }

        public static dynamic SaveItemInfo(CategoriesArticles dto, string Culture = "all")
        {
            if (dto.Alias == null || dto.Alias == "")
            {
                dto.Alias = API.Models.MyHelper.StringHelper.UrlFriendly(dto.Title);
            }
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_CategoriesArticles",
            new string[] { "@flag", "@Id", "@Description", "@Status",  "@ModifiedBy", "@Metadesc", "@Metakey", "@Metadata", "@Images", "@Ordering", "@Hits", "@FeaturedHome", "@IdCoQuan", "@Icon" },
            new object[] { "SaveItemInfo", dto.Id,  dto.Description, dto.Status,  dto.ModifiedBy, dto.Metadesc, dto.Metakey, dto.Metadata, dto.Images,  dto.Ordering, dto.Hits, dto.FeaturedHome, dto.IdCoQuan, dto.Icon });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }
        public static dynamic DeleteItem(CategoriesArticles dto)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_CategoriesArticles",
            new string[] { "@flag", "@Id", "@ModifiedBy" },
            new object[] { "DeleteItem", dto.Id, dto.ModifiedBy});
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }

        public static dynamic UpdateFeaturedHome(CategoriesArticles dto)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_CategoriesArticles",
            new string[] { "@flag", "@Id", "@IdCoQuan", "@FeaturedHome", "@ModifiedBy" },
            new object[] { "UpdateFeaturedHome", dto.Id,dto.IdCoQuan, dto.FeaturedHome, dto.ModifiedBy });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }

        public static dynamic UpdateStatus(CategoriesArticles dto)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_CategoriesArticles",
            new string[] { "@flag", "@Id", "@IdCoQuan", "@Status", "@ModifiedBy" },
            new object[] { "UpdateStatus", dto.Id,dto.IdCoQuan,dto.Status, dto.ModifiedBy });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }

        public static dynamic UpdateAlias(int Id, string Alias)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_CategoriesArticles",
            new string[] { "@flag", "@Id", "@Alias" },
            new object[] { "UpdateAlias", Id, Alias });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }



    }
}
