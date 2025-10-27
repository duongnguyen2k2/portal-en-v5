using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using API.Areas.Admin.Models.Documents;
using API.Models;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace API.Areas.Admin.Models.Documents
{
    public class DocumentsService
    {
        public static List<Documents> GetListPagination(SearchDocuments dto, string SecretId)
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
            string StartDate = DateTime.ParseExact(dto.ShowStartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyyMMdd");
            string EndDate = DateTime.ParseExact(dto.ShowEndDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyyMMdd");
            var tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Documents",
                new string[] { "@flag", "@CurrentPage", "@ItemsPerPage", "@Keyword", "@TypeId", "@LevelId", "@FieldId", "@CatId", "@StartDate", "@EndDate" , "@Status", "@IdCoQuan" },
                new object[] { str_sql, dto.CurrentPage, dto.ItemsPerPage, dto.Keyword, dto.TypeId, dto.LevelId, dto.FieldId, dto.CatId, StartDate, EndDate, Status,dto.IdCoQuan });
            if (tabl == null)
            {
                return new List<Documents>();
            }
            else
            {
                return (from r in tabl.AsEnumerable()
                        select new Documents
                        {
                            Id = (int)r["Id"],
                            Title = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
                            Alias = (string)((r["Alias"] == System.DBNull.Value) ? null : r["Alias"]),
                            Code = (string)((r["Code"] == System.DBNull.Value) ? null : r["Code"]),                           
                            Link = (string)((r["Link"] == System.DBNull.Value) ? null : r["Link"]),
                            Introtext = (string)((r["Introtext"] == System.DBNull.Value) ? null : r["Introtext"]),
                            Status = (Boolean)r["Status"],
                            IssuedDate = (DateTime)((r["IssuedDate"] == System.DBNull.Value) ? DateTime.Now : r["IssuedDate"]),
                            EffectiveDate = (DateTime)((r["EffectiveDate"] == System.DBNull.Value) ? DateTime.Now : r["EffectiveDate"]),
                            IssuedDateShow = (string)((r["IssuedDate"] == System.DBNull.Value) ? DateTime.Now.ToString("dd/MM/yyyy") : (string)((DateTime)r["IssuedDate"]).ToString("dd/MM/yyyy")),
                            EffectiveDateShow = (string)((r["EffectiveDate"] == System.DBNull.Value) ? DateTime.Now.ToString("dd/MM/yyyy") : (string)((DateTime)r["EffectiveDate"]).ToString("dd/MM/yyyy")),
                            CatId = (int)r["CatId"],
                            TypeId = (int)r["TypeId"],
                            FieldId = (int)r["FieldId"],
                            LevelId = (int)r["LevelId"],
                            OrganizationName = (string)((r["OrganizationName"] == System.DBNull.Value) ? null : r["OrganizationName"]),
                            Str_ListFile = (string)((r["Str_ListFile"] == System.DBNull.Value) ? null : r["Str_ListFile"]),
                            Featured = (Boolean)((r["Featured"] == System.DBNull.Value) ? false : r["Featured"]),
                            FeaturedHome = (Boolean)((r["FeaturedHome"] == System.DBNull.Value) ? false : r["FeaturedHome"]),
                            Ids = MyModels.Encode((int)r["Id"], SecretId),
                        TotalRows = (int)r["TotalRows"],
                        }).ToList();
            }


        }

        public static List<Documents> GetListNew(int Limit = 0, int IdCoQuan=0, string Culture = "all")
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Documents",
                new string[] { "@flag", "@Limit", "@IdCoQuan" }, new object[] { "GetListNew", Limit, IdCoQuan });
            if (tabl == null)
            {
                return new List<Documents>();
            }
            else
            {
                return (from r in tabl.AsEnumerable()
                        select new Documents
                        {
                            Id = (int)r["Id"],
                            Title = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
                            Alias = (string)((r["Alias"] == System.DBNull.Value) ? null : r["Alias"]),
                            Introtext = (string)((r["Introtext"] == System.DBNull.Value) ? null : r["Introtext"]),
                            IssuedDate = (DateTime)((r["IssuedDate"] == System.DBNull.Value) ? DateTime.Now : r["IssuedDate"]),
                        }).ToList();
            }

        }
        public static List<Documents> GetListFeaturedHome(int Limit = 0, int IdCoQuan=0, string Culture = "all")
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Documents",
                new string[] { "@flag", "@Limit", "@IdCoQuan" }, new object[] { "GetListFeaturedHome", Limit, IdCoQuan });
            if (tabl == null)
            {
                return new List<Documents>();
            }
            else
            {
                return (from r in tabl.AsEnumerable()
                        select new Documents
                        {
                            Id = (int)r["Id"],
                            Title = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
                            Alias = (string)((r["Alias"] == System.DBNull.Value) ? null : r["Alias"]),
                            Introtext = (string)((r["Introtext"] == System.DBNull.Value) ? null : r["Introtext"]),
                            IssuedDate = (DateTime)((r["IssuedDate"] == System.DBNull.Value) ? DateTime.Now : r["IssuedDate"]),
                        }).ToList();
            }

        }

        public static List<Documents> GetListNotification(SearchDocuments dto)
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
            string str_sql = "GetListNotification";
            
            var tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Documents",
                new string[] { "@flag", "@CurrentPage", "@ItemsPerPage", "@Keyword", "@TypeId", "@LevelId", "@FieldId", "@CatId", "@IdCoQuan" },
                new object[] { str_sql, dto.CurrentPage, dto.ItemsPerPage, dto.Keyword, dto.TypeId, dto.LevelId, dto.FieldId, dto.CatId,  dto.IdCoQuan });
            if (tabl == null)
            {
                return new List<Documents>();
            }
            else
            {
                return (from r in tabl.AsEnumerable()
                        select new Documents
                        {
                            Id = (int)r["Id"],
                            Title = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
                            Alias = (string)((r["Alias"] == System.DBNull.Value) ? null : r["Alias"]),
                            Code = (string)((r["Code"] == System.DBNull.Value) ? null : r["Code"]),
                            Link = (string)((r["Link"] == System.DBNull.Value) ? null : r["Link"]),
                            Introtext = (string)((r["Introtext"] == System.DBNull.Value) ? null : r["Introtext"]),
                            Status = (Boolean)r["Status"],
                            IssuedDate = (DateTime)((r["IssuedDate"] == System.DBNull.Value) ? DateTime.Now : r["IssuedDate"]),
                            EffectiveDate = (DateTime)((r["EffectiveDate"] == System.DBNull.Value) ? DateTime.Now : r["EffectiveDate"]),
                            IssuedDateShow = (string)((r["IssuedDate"] == System.DBNull.Value) ? DateTime.Now.ToString("dd/MM/yyyy") : (string)((DateTime)r["IssuedDate"]).ToString("dd/MM/yyyy")),
                            EffectiveDateShow = (string)((r["EffectiveDate"] == System.DBNull.Value) ? DateTime.Now.ToString("dd/MM/yyyy") : (string)((DateTime)r["EffectiveDate"]).ToString("dd/MM/yyyy")),
                            CatId = (int)r["CatId"],
                            TypeId = (int)r["TypeId"],
                            FieldId = (int)r["FieldId"],
                            LevelId = (int)r["LevelId"],
                            OrganizationName = (string)((r["OrganizationName"] == System.DBNull.Value) ? null : r["OrganizationName"]),
                            Str_ListFile = (string)((r["Str_ListFile"] == System.DBNull.Value) ? null : r["Str_ListFile"]),
                            Featured = (Boolean)((r["Featured"] == System.DBNull.Value) ? false : r["Featured"]),
                            FeaturedHome = (Boolean)((r["FeaturedHome"] == System.DBNull.Value) ? false : r["FeaturedHome"]),                            
                            TotalRows = (int)r["TotalRows"],
                        }).ToList();
            }


        }


        public static List<SelectListItem> GetListItemsStatus()
        {
            List<SelectListItem> ListItems = new List<SelectListItem>();
            ListItems.Insert(0, (new SelectListItem { Text = "--- Trạng Thái ---", Value = "-1" }));
            ListItems.Insert(1, (new SelectListItem { Text = "Ẩn", Value = "0" }));
            ListItems.Insert(2, (new SelectListItem { Text = "Hiện", Value = "1" }));
            return ListItems;
        }

     
        public static List<Documents> GetList(Boolean Selected = true, int IdCoQuan=0)
        {
            
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Documents",
                new string[] { "@flag", "@Selected", "@IdCoQuan" }, new object[] { "GetList", Convert.ToDecimal(Selected), IdCoQuan });
            if (tabl == null)
            {
                return new List<Documents>();
            }
            else
            {
                return (from r in tabl.AsEnumerable()
					select new Documents
					{
						Id = (int)r["Id"],
                        Title = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),                       
                        Introtext = (string)((r["Introtext"] == System.DBNull.Value) ? null : r["Introtext"]), 						
					}).ToList();
            }

        }

        public static Documents GetItem(decimal Id, string SecretId = null, int IdCoQuan = 0)
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Documents",
            new string[] { "@flag", "@Id", "@IdCoQuan" }, new object[] { "GetItem", Id, IdCoQuan });

            Documents Item = (from r in tabl.AsEnumerable()
                              select new Documents
                              {
                                  Id = (int)r["Id"],
                                  Title = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
                                  Alias = (string)((r["Alias"] == System.DBNull.Value) ? null : r["Alias"]),
                                  Code = (string)((r["Code"] == System.DBNull.Value) ? null : r["Code"]),
                                  Link = (string)((r["Link"] == System.DBNull.Value) ? null : r["Link"]),
                                  Introtext = (string)((r["Introtext"] == System.DBNull.Value) ? null : r["Introtext"]),
                                  FullText = (string)((r["FullText"] == System.DBNull.Value) ? null : r["FullText"]),
                                  Status = (Boolean)r["Status"],
                                  IssuedDate = (DateTime)((r["IssuedDate"] == System.DBNull.Value) ? DateTime.Now : r["IssuedDate"]),
                                  EffectiveDate = (DateTime)((r["EffectiveDate"] == System.DBNull.Value) ? DateTime.Now : r["EffectiveDate"]),
                                  IssuedDateShow = (string)((r["IssuedDate"] == System.DBNull.Value) ? DateTime.Now.ToString("dd/MM/yyyy") : (string)((DateTime)r["IssuedDate"]).ToString("dd/MM/yyyy")),
                                  EffectiveDateShow = (string)((r["EffectiveDate"] == System.DBNull.Value) ? DateTime.Now.ToString("dd/MM/yyyy") : (string)((DateTime)r["EffectiveDate"]).ToString("dd/MM/yyyy")),
                                  CatId = (int)r["CatId"],
                                  IdCoQuan = (int)r["IdCoQuan"],
                                  TypeId = (int)r["TypeId"],
                                  FieldId = (int)r["FieldId"],
                                  LevelId = (int)r["LevelId"],
                                  FieldTitle = (string)((r["FieldTitle"] == System.DBNull.Value) ? null : r["FieldTitle"]),
                                  TypeTitle = (string)((r["TypeTitle"] == System.DBNull.Value) ? null : r["TypeTitle"]),
                                  LevelTitle = (string)((r["LevelTitle"] == System.DBNull.Value) ? null : r["LevelTitle"]),
                                  OrganizationName = (string)((r["OrganizationName"] == System.DBNull.Value) ? null : r["OrganizationName"]),
                                  Str_ListFile = (string)((r["Str_ListFile"] == System.DBNull.Value) ? null : r["Str_ListFile"]),
                                  Featured = (Boolean)((r["Featured"] == System.DBNull.Value) ? null : r["Featured"]),
                                  FeaturedHome = (Boolean)((r["FeaturedHome"] == System.DBNull.Value) ? null : r["FeaturedHome"]),
                                  Ids = MyModels.Encode((int)r["Id"], SecretId),
                              }).FirstOrDefault();


            if (Item != null)
            {
                if (Item.Str_ListFile != null && Item.Str_ListFile != "")
                {
                    Item.ListFile = JsonConvert.DeserializeObject<List<FileDocuments>>(Item.Str_ListFile);
                }

            }
            return Item;
        }



        public static dynamic SaveItem(Documents dto)
        {
            string Str_ListFile = null;

            List<FileDocuments> ListFileArticle = new List<FileDocuments>();


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
            
            dto.Title = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.Title);
            dto.OrganizationName = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.OrganizationName);
            dto.Code = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.Code);
            dto.OrganizationName = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.OrganizationName);
            dto.Introtext = API.Models.MyHelper.StringHelper.RemoveTagsIntroText(dto.Introtext);          
            dto.FullText = API.Models.MyHelper.StringHelper.RemoveTagsFullText(dto.FullText);
            dto.Link = API.Models.MyHelper.StringHelper.RemoveTagsLink(dto.Link);
            dto.LinkRoot = API.Models.MyHelper.StringHelper.RemoveTagsLink(dto.LinkRoot);
            dto.Alias = API.Models.MyHelper.StringHelper.UrlFriendly(dto.Title);
            
            DateTime IssuedDate = DateTime.ParseExact(dto.IssuedDateShow, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime EffectiveDate = DateTime.ParseExact(dto.EffectiveDateShow, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            try
            {
                DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Documents",
            new string[] { "@flag", "@Id", "@Title", "@Code", "@Link", "@Alias", "@Featured", "@FeaturedHome", "@Introtext", "@FullText", "@Status", "@CreatedBy", "@ModifiedBy", "@IssuedDate", "@EffectiveDate", "@CatId", "@TypeId", "@FieldId", "@LevelId", "@OrganizationName", "@Str_ListFile", "@IdCoQuan" },
            new object[] { "SaveItem", dto.Id, dto.Title, dto.Code, dto.Link, dto.Alias, dto.Featured, dto.FeaturedHome, dto.Introtext, dto.FullText, dto.Status, dto.CreatedBy, dto.ModifiedBy, IssuedDate, EffectiveDate, dto.CatId, dto.TypeId, dto.FieldId, dto.LevelId, dto.OrganizationName, Str_ListFile, dto.IdCoQuan });
                return (from r in tabl.AsEnumerable()
                        select new
                        {
                            N = (int)(r["N"]),
                        }).FirstOrDefault();
            }
            catch (Exception ex) {
                return ex;
            }
            

        }
        public static dynamic DeleteItem(Documents dto)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Documents",
            new string[] { "@flag", "@Id", "@ModifiedBy", "@IdCoQuan" },
            new object[] { "DeleteItem", dto.Id, dto.ModifiedBy ,dto.IdCoQuan });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }

        public static dynamic UpdateStatus(Documents dto)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Documents",
            new string[] { "@flag", "@Id", "@Status", "@ModifiedBy", "@IdCoQuan" },
            new object[] { "UpdateStatus", dto.Id, dto.Status, dto.ModifiedBy ,dto.IdCoQuan });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }

        public static dynamic UpdateFeatured(Documents dto)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Documents",
            new string[] { "@flag", "@Id", "@Featured", "@ModifiedBy" },
            new object[] { "UpdateFeatured", dto.Id, dto.Featured, dto.ModifiedBy });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }

        public static dynamic UpdateFeaturedHome(Documents dto)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Documents",
            new string[] { "@flag", "@Id", "@FeaturedHome", "@ModifiedBy" },
            new object[] { "UpdateFeaturedHome", dto.Id, dto.FeaturedHome, dto.ModifiedBy });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }

        public static dynamic UpdateIntrotext(int Id, string Introtext,int IdCoQuan=0)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Documents",
            new string[] { "@flag", "@Id", "@Introtext", "@IdCoQuan" },
            new object[] { "UpdateIntrotext", Id, Introtext, IdCoQuan });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();
        }

        public static dynamic UpdateAlias(int Id, string Alias)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Documents",
            new string[] { "@flag", "@Id", "@Alias" },
            new object[] { "UpdateAlias", Id, Alias });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }



        // *********************   TMP ***************************************

        public static dynamic SaveItemTMPDuLieuThongTin(Documents dto)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Documents",
            new string[] { "@flag", "@Id", "@LevelId", "@TypeId", "@FieldId" },
            new object[] { "SaveItemTMPDuLieuThongTin", dto.Id, dto.LevelId,dto.TypeId,dto.FieldId });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }


        public static List<Documents> GetListItemsTMP(int IdCoQuan)
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Documents",
            new string[] { "@flag", "@IdCoQuan" }, new object[] { "GetListItemsTMP", IdCoQuan });

            List<Documents> ListItems = (from r in tabl.AsEnumerable()
                              select new Documents
                              {
                                  Id = (int)r["Id"],
                                  Title = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
                                  Alias = (string)((r["Alias"] == System.DBNull.Value) ? null : r["Alias"]),
                                  Code = (string)((r["Code"] == System.DBNull.Value) ? null : r["Code"]),
                                  Link = (string)((r["Link"] == System.DBNull.Value) ? null : r["Link"]),
                                  Introtext = (string)((r["Introtext"] == System.DBNull.Value) ? null : r["Introtext"]),
                                  FullText = (string)((r["FullText"] == System.DBNull.Value) ? null : r["FullText"]),
                                  Status = (Boolean)r["Status"],
                                  IssuedDate = (DateTime)((r["IssuedDate"] == System.DBNull.Value) ? DateTime.Now : r["IssuedDate"]),
                                  EffectiveDate = (DateTime)((r["EffectiveDate"] == System.DBNull.Value) ? DateTime.Now : r["EffectiveDate"]),
                                  IssuedDateShow = (string)((r["IssuedDate"] == System.DBNull.Value) ? DateTime.Now.ToString("dd/MM/yyyy") : (string)((DateTime)r["IssuedDate"]).ToString("dd/MM/yyyy")),
                                  EffectiveDateShow = (string)((r["EffectiveDate"] == System.DBNull.Value) ? DateTime.Now.ToString("dd/MM/yyyy") : (string)((DateTime)r["EffectiveDate"]).ToString("dd/MM/yyyy")),
                                  CatId = (int)r["CatId"],
                                  IdCoQuan = (int)r["IdCoQuan"],
                                  TypeId = (int)r["TypeId"],
                                  FieldId = (int)r["FieldId"],
                                  LevelId = (int)r["LevelId"],
                                  OrganizationName = (string)((r["OrganizationName"] == System.DBNull.Value) ? null : r["OrganizationName"]),
                                  Str_ListFile = (string)((r["Str_ListFile"] == System.DBNull.Value) ? null : r["Str_ListFile"]),
                                  LinkRoot = (string)((r["LinkRoot"] == System.DBNull.Value) ? null : r["LinkRoot"]),
                                  DuLieuThongTin = (string)((r["DuLieuThongTin"] == System.DBNull.Value) ? null : r["DuLieuThongTin"]),
                              }).ToList();

            return ListItems;
        }

        public static Documents GetItemTMPByLink(string Link)
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Documents",
            new string[] { "@flag", "@LinkRoot" }, new object[] { "GetItemTMPByLink", Link });

            Documents Item = (from r in tabl.AsEnumerable()
                              select new Documents
                              {
                                  Id = (int)r["Id"],
                                  Title = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
                                  Alias = (string)((r["Alias"] == System.DBNull.Value) ? null : r["Alias"]),
                                  Code = (string)((r["Code"] == System.DBNull.Value) ? null : r["Code"]),
                                  Link = (string)((r["Link"] == System.DBNull.Value) ? null : r["Link"]),
                                  Introtext = (string)((r["Introtext"] == System.DBNull.Value) ? null : r["Introtext"]),
                                  FullText = (string)((r["FullText"] == System.DBNull.Value) ? null : r["FullText"]),
                                  Status = (Boolean)r["Status"],
                                  IssuedDate = (DateTime)((r["IssuedDate"] == System.DBNull.Value) ? DateTime.Now : r["IssuedDate"]),
                                  EffectiveDate = (DateTime)((r["EffectiveDate"] == System.DBNull.Value) ? DateTime.Now : r["EffectiveDate"]),
                                  IssuedDateShow = (string)((r["IssuedDate"] == System.DBNull.Value) ? DateTime.Now.ToString("dd/MM/yyyy") : (string)((DateTime)r["IssuedDate"]).ToString("dd/MM/yyyy")),
                                  EffectiveDateShow = (string)((r["EffectiveDate"] == System.DBNull.Value) ? DateTime.Now.ToString("dd/MM/yyyy") : (string)((DateTime)r["EffectiveDate"]).ToString("dd/MM/yyyy")),
                                  CatId = (int)r["CatId"],
                                  IdCoQuan = (int)r["IdCoQuan"],
                                  TypeId = (int)r["TypeId"],
                                  FieldId = (int)r["FieldId"],
                                  LevelId = (int)r["LevelId"],
                                  OrganizationName = (string)((r["OrganizationName"] == System.DBNull.Value) ? null : r["OrganizationName"]),
                                  Str_ListFile = (string)((r["Str_ListFile"] == System.DBNull.Value) ? null : r["Str_ListFile"]),
                                  LinkRoot = (string)((r["LinkRoot"] == System.DBNull.Value) ? null : r["LinkRoot"])
                              }).FirstOrDefault();


            if (Item != null)
            {
                if (Item.Str_ListFile != null && Item.Str_ListFile != "")
                {
                    Item.ListFile = JsonConvert.DeserializeObject<List<FileDocuments>>(Item.Str_ListFile);
                }

            }
            return Item;
        }

        public static dynamic SaveItemTMP(Documents dto)
        {
            string Str_ListFile = null;

            List<FileDocuments> ListFileArticle = new List<FileDocuments>();


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

            if (dto.Alias == null || dto.Alias == "")
            {
                dto.Alias = API.Models.MyHelper.StringHelper.UrlFriendly(dto.Title);
            }
            DateTime IssuedDate = DateTime.ParseExact(dto.IssuedDateShow, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime EffectiveDate = DateTime.ParseExact(dto.EffectiveDateShow, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Documents",
            new string[] { "@flag", "@Id", "@Title", "@Code", "@Link", "@Alias", "@Introtext", "@FullText", "@Status", "@CreatedBy", "@ModifiedBy", "@IssuedDate", "@EffectiveDate", "@CatId", "@TypeId", "@FieldId", "@LevelId", "@OrganizationName", "@Str_ListFile", "@IdCoQuan", "@LinkRoot" , "@DataContentHtml", "@ExpriedStatus", "@SignBy", "@DuLieuThongTin" },
            new object[] { "SaveItemTMP", dto.Id, dto.Title, dto.Code, dto.Link, dto.Alias, dto.Introtext, dto.FullText, dto.Status, dto.CreatedBy, dto.ModifiedBy, IssuedDate, EffectiveDate, dto.CatId, dto.TypeId, dto.FieldId, dto.LevelId, dto.OrganizationName, Str_ListFile, dto.IdCoQuan, dto.LinkRoot,dto.DataContentHtml,dto.ExpriedStatus,dto.SignBy,dto.DuLieuThongTin});
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();
        }
    }
}
