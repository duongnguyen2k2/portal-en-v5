using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using API.Areas.Admin.Models.Menus;
using API.Areas.Admin.Models.SystemLog;
using API.Models;
using DocumentFormat.OpenXml.Office.CustomUI;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace API.Areas.Admin.Models.Menus
{
    public class MenusService
    {
        public static List<Menus> GetListPagination(SearchMenus dto, string SecretId, string Culture = "vi")
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
            var tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Menus",
                new string[] { "@flag", "@CurrentPage", "@ItemsPerPage", "@Keyword", "@IdCoQuan", "@CatId" },
                new object[] { "GetListPagination", dto.CurrentPage, dto.ItemsPerPage, dto.Keyword ,dto.IdCoQuan,dto.CatId});
            if (tabl == null)
            {
                return new List<Menus>();
            }
            else
            {
                return (from r in tabl.AsEnumerable()
					select new Menus
					{
						Id = (int)r["Id"],
 						Title = (string)r["Title"],
                        Title_EN = (string)((r["Title_EN"] == System.DBNull.Value) ? null: r["Title_EN"]),
                        Link_EN = (string)((r["Link_EN"] == System.DBNull.Value) ? null : r["Link_EN"]),
                        CatId = (int)r["CatId"],
 						StaticId = (int)r["StaticId"],
 						Link = (string)((r["Link"] == System.DBNull.Value) ? "" : r["Link"]),
                        Target = (string)((r["Target"] == System.DBNull.Value) ? "" : r["Target"]),
 						IdCoQuan = (int)((r["IdCoQuan"] == System.DBNull.Value) ? 0 : r["IdCoQuan"]),
                        TypeShowId = (int)((r["TypeShowId"] == System.DBNull.Value) ? 0 : r["TypeShowId"]),
 						TenCoQuan = (string)((r["TenCoQuan"] == System.DBNull.Value) ? 0 : r["TenCoQuan"]),
 						ParentId = (int)r["ParentId"], 						
 						Status = (Boolean)((r["Status"] == System.DBNull.Value) ? true : r["Status"]), 
 						Ordering = (int?)((r["Ordering"] == System.DBNull.Value) ? null : r["Ordering"]),
                        Icon = (string)((r["Icon"] == System.DBNull.Value) ? null : r["Icon"]),
                        Ids = MyModels.Encode((int)r["Id"], SecretId),						
					}).ToList();
            }


        }

        public static List<Menus> GetList(Boolean Selected = true,int IdCoQuan = 1, int CatId = 0, string Culture = "vi")
        {
            
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Menus",
                new string[] { "@flag", "@Selected", "@IdCoQuan", "@CatId" }, new object[] { "GetList", Convert.ToDecimal(Selected),IdCoQuan,CatId });
            if (tabl == null)
            {
                return new List<Menus>();
            }
            else
            {
                return (from r in tabl.AsEnumerable()
					select new Menus
					{
                        Id = (int)r["Id"],
                        Title = (string)r["Title"],
                        TitleMenu = (string)r["TitleMenu"],
                        Title_EN = (string)((r["Title_EN"] == System.DBNull.Value) ? null : r["Title_EN"]),
                        Link_EN = (string)((r["Link_EN"] == System.DBNull.Value) ? null : r["Link_EN"]),
                        Status = (Boolean)((r["Status"] == System.DBNull.Value) ? true : r["Status"]),
                        Link = (string)((r["Link"] == System.DBNull.Value) ? "" : r["Link"]),
                        Target = (string)((r["Target"] == System.DBNull.Value) ? "" : r["Target"]),
                        IdCoQuan = (int)((r["IdCoQuan"] == System.DBNull.Value) ? 0 : r["IdCoQuan"]),
                        CatId = (int)r["CatId"],
                        ParentId = (int)r["ParentId"],
                        TypeShowId = (int)((r["TypeShowId"] == System.DBNull.Value) ? 0 : r["TypeShowId"]),
                        Icon = (string)((r["Icon"] == System.DBNull.Value) ? null : r["Icon"])                        
					}).ToList();
            }

        }
        public static List<Menus> GetListByParrent(int ParentId = 0,int IdCoQuan=1,int CatId=0, int IdCoQuanCha = 1, string Culture = "vi")
        {
            string sql = "GetListByParrent";
            if (Culture.ToLower() == "en")
            {
                sql = "GetListByParrent_EN";
            }

            if (Startup._config["flagSystemLog"] == "1")
            {
                SystemLog.SystemLog itemLog = new SystemLog.SystemLog()
                {
                    Title = "GetListFeaturedHome",
                    Description = sql,
                    IdCoQuan = IdCoQuan
                };
                SystemLogService.SaveItem(itemLog);
            }

            List<Menus> menus = new List<Menus>();

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Menus",
                new string[] { "@flag", "@ParentId", "@IdCoQuan", "@CatId" }, new object[] { sql, ParentId, IdCoQuan, CatId });
            if (tabl.Rows.Count == 0)
            {
                if (IdCoQuan != 1)
                {
                    tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Menus",
                    new string[] { "@flag", "@ParentId", "@IdCoQuan", "@CatId" }, new object[] { sql, ParentId, IdCoQuanCha, CatId });

                    if (tabl.Rows.Count == 0)
                    {
                        menus = new List<Menus>();
                    }
                }
                
            }



            if (Culture == "en")
            {
                menus = (from r in tabl.AsEnumerable()
                         select new Menus
                         {
                             Id = (int)r["Id"],
                             Title = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
                             Link = (string)((r["Link_EN"] == System.DBNull.Value) ? null : r["Link_EN"]),
                             Alias = (string)((r["Alias"] == System.DBNull.Value) ? null : r["Alias"]),
                             CatId = (int)r["CatId"],
                             IdCoQuan = (int)r["IdCoQuan"],
                             StaticId = (int)r["StaticId"],                             
                             ParentId = (int)r["ParentId"],
                             ArticleId = (int?)((r["ArticleId"] == System.DBNull.Value) ? null : r["ArticleId"]),
                             Ordering = (int?)((r["Ordering"] == System.DBNull.Value) ? null : r["Ordering"]),
                             Icon = (string)((r["Icon"] == System.DBNull.Value) ? null : r["Icon"]),
                             ChildCount = (int)((r["ChildCount"] == System.DBNull.Value) ? null : r["ChildCount"]),
                         }).ToList();
            }
            else
            {
                menus = (from r in tabl.AsEnumerable()
                         select new Menus
                         {
                             Id = (int)r["Id"],
                             Title = (string)r["Title"],
                             Alias = (string)((r["Alias"] == System.DBNull.Value) ? null : r["Alias"]),
                             CatId = (int)r["CatId"],
                             TypeShowId = (int)((r["TypeShowId"] == System.DBNull.Value) ? 0 : r["TypeShowId"]),
                             IdCoQuan = (int)r["IdCoQuan"],
                             StaticId = (int)r["StaticId"],
                             Link = (string)((r["Link"] == System.DBNull.Value) ? null : r["Link"]),
                             Target = (string)((r["Target"] == System.DBNull.Value) ? "" : r["Target"]),
                             ParentId = (int)r["ParentId"],
                             ArticleId = (int?)((r["ArticleId"] == System.DBNull.Value) ? null : r["ArticleId"]),
                             Ordering = (int?)((r["Ordering"] == System.DBNull.Value) ? null : r["Ordering"]),
                             Icon = (string)((r["Icon"] == System.DBNull.Value) ? null : r["Icon"]),
                             ChildCount = (int)((r["ChildCount"] == System.DBNull.Value) ? null : r["ChildCount"]),
                         }).ToList();
            }

            

            for (int i = 0; i < menus.Count; i++)
            {
                if(menus[i].ChildCount > 0)
                {
                    menus[i].ListMenus = GetListByParrent(menus[i].Id, menus[i].IdCoQuan, menus[i].CatId,1,Culture);
                }
                
            }
            return menus;
        }
        public static List<SelectListItem> GetListItems(Boolean Selected = true, int IdCoQuan=1, int CatId=0, string Culture = "vi")
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Menus",
                new string[] { "@flag", "@Selected", "@IdCoQuan", "@CatId" }, new object[] { "GetList", Convert.ToDecimal(Selected), IdCoQuan , CatId });
            List<SelectListItem> ListItems = (from r in tabl.AsEnumerable()
                                              select new SelectListItem
                                              {
                                                  Value = (string)((r["Id"] == System.DBNull.Value) ? null : r["Id"].ToString()),
                                                  Text = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
                                              }).ToList();

            ListItems.Insert(0, (new SelectListItem { Text = "Chọn Menu", Value = "0" }));
            return ListItems;

        }

        public static List<TypeShow> GetListItemsTypeShow()
        {
            List<TypeShow> ListItems = new List<TypeShow>();
            ListItems.Add(new TypeShow()
            {
                Id = 0,Title= "Cả Máy tính và Điện thoại",
                Code="show-pc-mb"
            });
            ListItems.Add(new TypeShow()
            {
                Id = 1,
                Title = "Chỉ hiện máy tính",
                Code = "show-pc"
            });
            ListItems.Add(new TypeShow()
            {
                Id = 2,
                Title = "Chỉ hiện điện thoại",
                Code = "show-mb"
            });
            ListItems.Add(new TypeShow()
            {
                Id = 3,
                Title = "Hiện điện thoại dưới chứ Menu",
                Code = "show-end-menu"
            });

            return ListItems;
        }

        public static List<SelectListItem> GetListTypeShow(Boolean Selected = true)
        {
            List<SelectListItem> ListItems = new List<SelectListItem>();
            ListItems.Insert(0, (new SelectListItem { Text = "Cả Máy tính và Điện thoại", Value = "0" }));
            ListItems.Insert(1, (new SelectListItem { Text = "Chỉ hiện máy tính", Value = "1" }));
            ListItems.Insert(2, (new SelectListItem { Text = "Chỉ hiện điện thoại", Value = "2" }));
            ListItems.Insert(3, (new SelectListItem { Text = "Hiện điện thoại dưới chứ Menu", Value = "3" }));
            return ListItems;
        }

        public static List<SelectListItem> GetListCatTarget(Boolean Selected = true)
        {
            List<SelectListItem> ListItems = new List<SelectListItem>();
            ListItems.Insert(0, (new SelectListItem { Text = "Tab hiện tại", Value = "" }));
            ListItems.Insert(1, (new SelectListItem { Text = "Mở tab mới", Value = "_blank" }));            
            return ListItems;
        }


        public static List<SelectListItem> GetListCatItems(Boolean Selected = true)
        {
            List<SelectListItem> ListItems = new List<SelectListItem>();
            ListItems.Insert(0, (new SelectListItem { Text = "Menu Chính", Value = "0" }));
            ListItems.Insert(1, (new SelectListItem { Text = "Menu Dọc", Value = "1" }));
            ListItems.Insert(2, (new SelectListItem { Text = "Menu Footer", Value = "2" }));
            return ListItems;

        }


        public static Menus GetItem(decimal Id, string SecretId = null, string Culture = "vi")
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Menus",
            new string[] { "@flag", "@Id" }, new object[] { "GetItem", Id });
            Menus item = (from r in tabl.AsEnumerable()
                    select new Menus
                    {
                        Id = (int)r["Id"],
 						Title = (string)r["Title"],
                        Link_EN = (string)((r["Link_EN"] == System.DBNull.Value) ? null : r["Link_EN"]),
                        Title_EN = (string)((r["Title_EN"] == System.DBNull.Value) ? null : r["Title_EN"]),
                        CatId = (int)r["CatId"],
 						StaticId = (int)r["StaticId"],
 						Link = (string)((r["Link"] == System.DBNull.Value) ? null : r["Link"]),
                        TypeShowId = (int)((r["TypeShowId"] == System.DBNull.Value) ? 0 : r["TypeShowId"]),
                        Target = (string)((r["Target"] == System.DBNull.Value) ? "" : r["Target"]),
                        IdCoQuan = (int)((r["IdCoQuan"] == System.DBNull.Value) ? 0 : r["IdCoQuan"]),
 						ParentId = (int)r["ParentId"],
 						ModifiedBy = (int?)((r["ModifiedBy"] == System.DBNull.Value) ? null : r["ModifiedBy"]),
 						Status = (Boolean)((r["Status"] == System.DBNull.Value) ? true : r["Status"]), 						
 						ModifiedDate = (DateTime?)((r["ModifiedDate"] == System.DBNull.Value) ? null : r["ModifiedDate"]),
 						ArticleId = (int?)((r["ArticleId"] == System.DBNull.Value) ? null : r["ArticleId"]),
 						Ordering = (int?)((r["Ordering"] == System.DBNull.Value) ? null : r["Ordering"]),
                        Icon = (string)((r["Icon"] == System.DBNull.Value) ? null : r["Icon"]),
                        Ids = MyModels.Encode((int)r["Id"], SecretId),
                    }).FirstOrDefault();
            /*
            if(item != null && item.Id > 0)
            {
                item.Title = HttpUtility.HtmlDecode(item.Title);
                item.Title_EN = HttpUtility.HtmlDecode(item.Title_EN);
                item.Link = HttpUtility.HtmlDecode(item.Link);
                item.Link_EN = HttpUtility.HtmlDecode(item.Link_EN);
                item.Icon = HttpUtility.HtmlDecode(item.Icon);
            }*/
            return item;
        }

        public static dynamic SaveItem(Menus dto)
        {
			dto.Icon = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.Icon);
			dto.Title = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.Title);
			dto.Title_EN = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.Title_EN);
			dto.Target = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.Target);
			dto.Link = API.Models.MyHelper.StringHelper.RemoveTagsLink(dto.Link);
			dto.Link_EN = API.Models.MyHelper.StringHelper.RemoveTagsLink(dto.Link_EN);

			DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Menus",
            new string[] { "@flag","@Id","@Title","@CatId","@StaticId","@Link", "@Target", "@ParentId","@ModifiedBy","@Status","@Deleted","@ModifiedDate","@ArticleId","@Ordering","@Icon", "@IdCoQuan", "@TypeShowId", "@Title_EN" , "@Link_EN" },
            new object[] { "SaveItem",dto.Id,dto.Title,dto.CatId,dto.StaticId,dto.Link,dto.Target, dto.ParentId,dto.ModifiedBy,dto.Status,dto.Deleted,dto.ModifiedDate,dto.ArticleId,dto.Ordering,dto.Icon,dto.IdCoQuan,dto.TypeShowId ,dto.Title_EN,dto.Link_EN});
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }
        public static dynamic DeleteItem(Menus dto)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Menus",
            new string[] { "@flag", "@Id", "@ModifiedBy" },
            new object[] { "DeleteItem", dto.Id, dto.ModifiedBy});
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }
		
		public static dynamic UpdateStatus(Menus dto)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Menus",
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
