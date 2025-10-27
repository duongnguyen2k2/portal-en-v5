using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using API.Areas.Admin.Models.Banners;
using API.Models;
using ClosedXML.Report.Utils;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace API.Areas.Admin.Models.Banners
{
    public class BannersService
    {
        public static List<Banners> GetListPagination(SearchBanners dto, string SecretId, string Culture = "all")
        {
            Culture = API.Models.MyHelper.StringHelper.ConverLan(Culture);

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
            var tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Banners",
                new string[] { "@flag", "@CurrentPage", "@ItemsPerPage", "@Keyword","@CatId", "@IdCoQuan" },
                new object[] { "GetListPagination", dto.CurrentPage, dto.ItemsPerPage, dto.Keyword,dto.CatId,dto.IdCoQuan });
            if (tabl == null)
            {
                return new List<Banners>();
            }
            else
            {
                return (from r in tabl.AsEnumerable()
					select new Banners
					{
						Id = (int)r["Id"],
 						Title = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
 						Description = (string)((r["Description"] == System.DBNull.Value) ? null : r["Description"]),
 						TenCoQuan = (string)((r["TenCoQuan"] == System.DBNull.Value) ? null : r["TenCoQuan"]),
 						IdCoQuan = (int)((r["IdCoQuan"] == System.DBNull.Value) ? null : r["IdCoQuan"]),
 						CategoriesTitle = (string)((r["CategoriesTitle"] == System.DBNull.Value) ? null : r["CategoriesTitle"]),
 						Status = (Boolean)((r["Status"] == System.DBNull.Value) ? null : r["Status"]), 						
 						Link = (string)((r["Link"] == System.DBNull.Value) ? null : r["Link"]),
 						SortOrder = (int)r["SortOrder"],
 						CatId = (int)((r["CatId"] == System.DBNull.Value) ? null : r["CatId"]),
 						Target = (string)((r["Target"] == System.DBNull.Value) ? null : r["Target"]),
 						Image = (string)((r["Image"] == System.DBNull.Value) ? null : r["Image"]),
						Ids = MyModels.Encode((int)r["Id"], SecretId),
                        Title_EN = (string)((r["Title_EN"] == System.DBNull.Value) ? null : r["Title_EN"]),
                        Image_EN = (string)((r["Image_EN"] == System.DBNull.Value) ? null : r["Image_EN"]),
                        Link_EN = (string)((r["Link_EN"] == System.DBNull.Value) ? null : r["Link_EN"]),
                        TotalRows = (int)r["TotalRows"],
                    }).ToList();
            }


        }

        public static List<SelectListItem> GetListItemsByCat(int CatId, int IdCoQuan = 1, string Culture = "vi")
        {
            Culture = API.Models.MyHelper.StringHelper.ConverLan(Culture);

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Banners",
                new string[] { "@flag", "@CatId", "@IdCoQuan" }, new object[] { "GetListByCat", CatId, IdCoQuan });
            List<SelectListItem> ListItems = (from r in tabl.AsEnumerable()
                                              select new SelectListItem
                                              {
                                                  Value = (string)((r["Link"] == System.DBNull.Value) ? null : r["Link"].ToString()),
                                                  Text = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
                                              }).ToList();

            ListItems.Insert(0, (new SelectListItem { Text = "-- Chọn website liên kết --", Value = "0" }));
            return ListItems;

        }

        public static List<Banners> GetListByCat(int CatId,int IdCoQuan=4, int IdCoQuanCha = 1, string Culture = "vi")
        {
            Culture = API.Models.MyHelper.StringHelper.ConverLan(Culture);

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Banners",
                new string[] { "@flag", "@CatId", "@IdCoQuan" }, new object[] { "GetListByCat", CatId, IdCoQuan });
            if (tabl == null)
            {
                return new List<Banners>();
            }
            else
            {
                if (Culture == "vi")
                {
                    return (from r in tabl.AsEnumerable()
                            select new Banners
                            {
                                Id = (int)r["Id"],
                                Title = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
                                Description = (string)((r["Description"] == System.DBNull.Value) ? null : r["Description"]),
                                Status = (Boolean)((r["Status"] == System.DBNull.Value) ? null : r["Status"]),
                                Link = (string)((r["Link"] == System.DBNull.Value) ? null : r["Link"]),
                                SortOrder = (int)r["SortOrder"],
                                CatId = (int)((r["CatId"] == System.DBNull.Value) ? null : r["CatId"]),
                                Target = (string)((r["Target"] == System.DBNull.Value) ? null : r["Target"]),
                                Image = (string)((r["Image"] == System.DBNull.Value) ? null : r["Image"]),
                                IdCoQuan = (int)((r["IdCoQuan"] == System.DBNull.Value) ? null : r["IdCoQuan"])
                            }).ToList();
                }
                else
                {
                    return (from r in tabl.AsEnumerable()
                            select new Banners
                            {
                                Id = (int)r["Id"],
                                Description = (string)((r["Description"] == System.DBNull.Value) ? null : r["Description"]),
                                Status = (Boolean)((r["Status"] == System.DBNull.Value) ? null : r["Status"]),
                                SortOrder = (int)r["SortOrder"],
                                CatId = (int)((r["CatId"] == System.DBNull.Value) ? null : r["CatId"]),
                                Target = (string)((r["Target"] == System.DBNull.Value) ? null : r["Target"]),
                                IdCoQuan = (int)((r["IdCoQuan"] == System.DBNull.Value) ? null : r["IdCoQuan"]),
                                Title = (string)((r["Title_EN"] == System.DBNull.Value) ? null : r["Title_EN"]),
                                Image = (string)((r["Image_EN"] == System.DBNull.Value) ? null : r["Image_EN"]),
                                Link = (string)((r["Link_EN"] == System.DBNull.Value) ? null : r["Link_EN"]),
                            }).ToList();

                }
            }

        }

        public static List<Banners> GetList(int IdCoQuan=0,int IdCoQuanCha = 1,string DomainFolderUpload="", string Culture = "vi")
        {
            Culture = API.Models.MyHelper.StringHelper.ConverLan(Culture);

            Boolean flag = false;
            List<Banners> ListItems = new List<Banners>();

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Banners",
                new string[] { "@flag", "@IdCoQuan" }, new object[] { "GetList", IdCoQuan });

            if (tabl.Rows.Count == 0)
            {
                if (IdCoQuan != 1)
                {
                    tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Banners",
                    new string[] { "@flag", "@IdCoQuan" }, new object[] { "GetList", IdCoQuanCha });
                    flag = true;
                }
                
            }
            if (Culture == "en")
            {
                ListItems = (from r in tabl.AsEnumerable()
                             select new Banners
                             {
                                 Id = (int)r["Id"],
                                 Title = (string)((r["Title_EN"] == System.DBNull.Value) ? r["Title"] : r["Title_EN"]),
                                 Description = (string)((r["Description"] == System.DBNull.Value) ? null : r["Description"]),
                                 Status = (Boolean)((r["Status"] == System.DBNull.Value) ? null : r["Status"]),
                                 Link = (string)((r["Link_EN"] == System.DBNull.Value) ? null : r["Link_EN"]),
                                 SortOrder = (int)r["SortOrder"],
                                 CatId = (int)((r["CatId"] == System.DBNull.Value) ? null : r["CatId"]),
                                 Target = (string)((r["Target"] == System.DBNull.Value) ? null : r["Target"]),
                                 Image = (string)((r["Image"] == System.DBNull.Value) ? null : r["Image"]),
                                 IdCoQuan = (int)((r["IdCoQuan"] == System.DBNull.Value) ? null : r["IdCoQuan"]),
                                 Title_EN = (string)((r["Title_EN"] == System.DBNull.Value) ? null : r["Title_EN"]),
                                 Image_EN = (string)((r["Image_EN"] == System.DBNull.Value) ? null : r["Image_EN"]),
                                 Link_EN = (string)((r["Link_EN"] == System.DBNull.Value) ? null : r["Link_EN"]),
                             }).ToList();
            }
            else
            {
                ListItems = (from r in tabl.AsEnumerable()
                             select new Banners
                             {
                                 Id = (int)r["Id"],
                                 Title = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
                                 Description = (string)((r["Description"] == System.DBNull.Value) ? null : r["Description"]),
                                 Status = (Boolean)((r["Status"] == System.DBNull.Value) ? null : r["Status"]),
                                 Link = (string)((r["Link"] == System.DBNull.Value) ? null : r["Link"]),
                                 SortOrder = (int)r["SortOrder"],
                                 CatId = (int)((r["CatId"] == System.DBNull.Value) ? null : r["CatId"]),
                                 Target = (string)((r["Target"] == System.DBNull.Value) ? null : r["Target"]),
                                 Image = (string)((r["Image"] == System.DBNull.Value) ? null : r["Image"]),
                                 IdCoQuan = (int)((r["IdCoQuan"] == System.DBNull.Value) ? null : r["IdCoQuan"]),
                                 Title_EN = (string)((r["Title_EN"] == System.DBNull.Value) ? null : r["Title_EN"]),
                                 Image_EN = (string)((r["Image_EN"] == System.DBNull.Value) ? null : r["Image_EN"]),
                                 Link_EN = (string)((r["Link_EN"] == System.DBNull.Value) ? null : r["Link_EN"]),
                             }).ToList();
            }

            if (ListItems != null && ListItems.Count() > 0 && flag)
            {
                for (int i = 0; i < ListItems.Count(); i++)
                {
                    if (ListItems[i].CatId == 1)
                    {
                        ListItems[i].CatId = 1000;
                    }
                    if (ListItems[i].Image_EN.IsNullOrWhiteSpace() && ListItems[i].Image != null)
                    {
                        ListItems[i].Image_EN = ListItems[i].Image;
                    }
                }
            }



            if (flag)
            {
                ListItems.Add(new Banners() { Title = "Slideshow 1", Image = "/uploads/slideshows/" + DomainFolderUpload + ".png", IdCoQuan = IdCoQuan, CatId = 1 });
            }
            return ListItems;

        }

        public static Banners GetItemByTitle(string Title)
        {
            

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Banners",
            new string[] { "@flag", "@Title" }, new object[] { "GetItemByTitle", Title });
            return (from r in tabl.AsEnumerable()
                    select new Banners
                    {
                        Id = (int)r["Id"],
                        Title = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
                        Title_EN = (string)((r["Title_EN"] == System.DBNull.Value) ? null : r["Title_EN"]),
                        Image_EN = (string)((r["Image_EN"] == System.DBNull.Value) ? null : r["Image_EN"]),
                        Link_EN = (string)((r["Link_EN"] == System.DBNull.Value) ? null : r["Link_EN"]),
                        Description = (string)((r["Description"] == System.DBNull.Value) ? null : r["Description"]),
                        IdCoQuan = (int)((r["IdCoQuan"] == System.DBNull.Value) ? null : r["IdCoQuan"]),
                        Status = (Boolean)((r["Status"] == System.DBNull.Value) ? null : r["Status"]),
                        Link = (string)((r["Link"] == System.DBNull.Value) ? null : r["Link"]),
                        SortOrder = (int)r["SortOrder"],
                        CatId = (int)((r["CatId"] == System.DBNull.Value) ? null : r["CatId"]),
                        Target = (string)((r["Target"] == System.DBNull.Value) ? null : r["Target"]),
                        Image = (string)((r["Image"] == System.DBNull.Value) ? null : r["Image"]),                        
                    }).FirstOrDefault();
        }

        public static Banners GetItem(decimal Id, string SecretId = null, string Culture = "vi")
        {
            Culture = API.Models.MyHelper.StringHelper.ConverLan(Culture);

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Banners",
            new string[] { "@flag", "@Id" }, new object[] { "GetItem", Id });
            return (from r in tabl.AsEnumerable()
                    select new Banners
                    {
                        Id = (int)r["Id"],
 						Title = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
 						Title_EN = (string)((r["Title_EN"] == System.DBNull.Value) ? null : r["Title_EN"]),
                        Image_EN = (string)((r["Image_EN"] == System.DBNull.Value) ? null : r["Image_EN"]),
                        Link_EN = (string)((r["Link_EN"] == System.DBNull.Value) ? null : r["Link_EN"]),
                        Description = (string)((r["Description"] == System.DBNull.Value) ? null : r["Description"]),                        
                        IdCoQuan = (int)((r["IdCoQuan"] == System.DBNull.Value) ? null : r["IdCoQuan"]),
                        Status = (Boolean)((r["Status"] == System.DBNull.Value) ? null : r["Status"]), 						 						
 						Link = (string)((r["Link"] == System.DBNull.Value) ? null : r["Link"]),
 						SortOrder = (int)r["SortOrder"],
 						CatId = (int)((r["CatId"] == System.DBNull.Value) ? null : r["CatId"]),
 						Target = (string)((r["Target"] == System.DBNull.Value) ? null : r["Target"]),
 						Image = (string)((r["Image"] == System.DBNull.Value) ? null : r["Image"]),
                        Ids = MyModels.Encode((int)r["Id"], SecretId),
                    }).FirstOrDefault();
        }

        public static dynamic SaveItem(Banners dto, string Culture = "vi")
        {
            dto.Title_EN = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.Title_EN);            
			dto.Title = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.Title);
			dto.Target = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.Target);
			dto.Description = API.Models.MyHelper.StringHelper.RemoveTagsIntroText(dto.Description);
			dto.Image = API.Models.MyHelper.StringHelper.RemoveTagsLink(dto.Image);
			dto.Image_EN = API.Models.MyHelper.StringHelper.RemoveTagsLink(dto.Image_EN);
			dto.Link_EN = API.Models.MyHelper.StringHelper.RemoveTagsLink(dto.Link_EN);

			DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Banners",
            new string[] { "@flag","@Id","@Title", "@Title_EN", "@Description","@Status","@CreatedBy","@ModifiedBy","@Link", "@Link_EN", "@SortOrder","@CatId","@Target","@Image", "@Image_EN", "@IdCoQuan" },
            new object[] { "SaveItem",dto.Id,dto.Title,dto.Title_EN,dto.Description,dto.Status,dto.CreatedBy,dto.ModifiedBy,dto.Link, dto.Link_EN, dto.SortOrder,dto.CatId,dto.Target,dto.Image, dto.Image_EN, dto.IdCoQuan});
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }
        public static dynamic DeleteItem(Banners dto)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Banners",
            new string[] { "@flag", "@Id", "@ModifiedBy" },
            new object[] { "DeleteItem", dto.Id, dto.ModifiedBy});
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }

        public static dynamic UpdateStatus(Banners dto)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Banners",
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
