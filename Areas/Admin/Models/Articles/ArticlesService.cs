using API.Areas.Admin.Helpers;
using API.Areas.Admin.Models.Articles;
using API.Models;
using ClosedXML.Report;
//using DocumentFormat.OpenXml.Office2010.Excel;
using HtmlAgilityPack;
using HtmlAgilityPack.CssSelectors.NetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Org.BouncyCastle.Crypto;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using static API.Areas.Admin.Models.ManagerFile.ManagerFile;

namespace API.Areas.Admin.Models.Articles
{
    public class ArticlesService
    {
        public static Articles GetItemSTP(decimal Id, string SecretId = null)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionStringSTP, "SP_Articles",
            new string[] { "@flag", "@Id" }, new object[] { "GetItem", Id });
            Articles Item = (from r in tabl.AsEnumerable()
                             select new Articles
                             {
                                 Id = (int)r["Id"],
                                 Title = (string)r["Title"],
                                 Str_ListFile = (string)((r["Str_ListFile"] == System.DBNull.Value) ? null : r["Str_ListFile"]),
                                 Str_Link = (string)((r["Str_Link"] == System.DBNull.Value) ? null : r["Str_Link"]),
                                 Alias = (string)((r["Alias"] == System.DBNull.Value) ? null : r["Alias"]),
                                 CatId = (int)r["CatId"],
                                 IntroText = (string)((r["IntroText"] == System.DBNull.Value) ? "" : r["IntroText"]),
                                 FullText = (string)((r["FullText"] == System.DBNull.Value) ? "" : r["FullText"]),
                                 Status = (Boolean)r["Status"],
                                 CreatedBy = (int?)((r["CreatedBy"] == System.DBNull.Value) ? 0 : r["CreatedBy"]),
                                 ModifiedBy = (int?)((r["ModifiedBy"] == System.DBNull.Value) ? null : r["ModifiedBy"]),
                                 CreatedDate = (DateTime)((r["CreatedDate"] == System.DBNull.Value) ? null : r["CreatedDate"]),
                                 PublishUp = (DateTime)((r["PublishUp"] == System.DBNull.Value) ? null : r["PublishUp"]),
                                 PublishUpShow = (string)((r["PublishUp"] == System.DBNull.Value) ? DateTime.Now.ToString("dd/MM/yyyy") : (string)((DateTime)r["PublishUp"]).ToString("dd/MM/yyyy")),
                                 ModifiedDate = (DateTime?)((r["ModifiedDate"] == System.DBNull.Value) ? null : r["ModifiedDate"]),
                                 Metadesc = (string)((r["Metadesc"] == System.DBNull.Value) ? null : r["Metadesc"]),
                                 Metakey = (string)((r["Metakey"] == System.DBNull.Value) ? null : r["Metakey"]),
                                 Metadata = (string)((r["Metadata"] == System.DBNull.Value) ? null : r["Metadata"]),
                                 Language = (string)((r["Language"] == System.DBNull.Value) ? null : r["Language"]),
                                 Featured = (Boolean)((r["Featured"] == System.DBNull.Value) ? null : r["Featured"]),
                                 StaticPage = (Boolean)((r["StaticPage"] == System.DBNull.Value) ? null : r["StaticPage"]),
                                 Images = (string)((r["Images"] == System.DBNull.Value) ? null : r["Images"]),
                                                   
                                 Params = (string)((r["Params"] == System.DBNull.Value) ? null : r["Params"]),
                                 Ordering = (int?)((r["Ordering"] == System.DBNull.Value) ? null : r["Ordering"]),
                                 Deleted = (Boolean)((r["Deleted"] == System.DBNull.Value) ? null : r["Deleted"]),
                                 IdCoQuan = (int)((r["IdCoQuan"] == System.DBNull.Value) ? 0 : r["IdCoQuan"]),
                                 AuthorId = (int)((r["AuthorId"] == System.DBNull.Value) ? 0 : r["AuthorId"]),
                                 
                                 Ids = MyModels.Encode((int)r["Id"], SecretId),
                                 
                             }).FirstOrDefault();
            if (Item != null)
            {
                if (Item.Str_ListFile != null && Item.Str_ListFile != "")
                {
                    Item.ListFile = JsonConvert.DeserializeObject<List<FileArticle>>(Item.Str_ListFile);
                }
                if (Item.Str_Link != null && Item.Str_Link != "")
                {
                    Item.ListLinkArticle = JsonConvert.DeserializeObject<List<LinkArticle>>(Item.Str_Link);
                }
            }
          
            return Item;
        }

        public static List<Articles> GetListPaginationSTP(SearchArticles dto, string SecretId)
        {

            if (dto.Keyword != null && dto.Keyword != "")
            {
                string[] arrKey = dto.Keyword.Split(':');
            }

            if (dto.CurrentPage <= 0)
            {
                dto.CurrentPage = 1;
            }
            if (dto.ItemsPerPage <= 0)
            {
                dto.ItemsPerPage = 15;
            }
            if (dto.Keyword == null)
            {
                dto.Keyword = "";
            }
            string StartDate = DateTime.ParseExact(dto.ShowStartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyyMMdd");
            string EndDate = DateTime.ParseExact(dto.ShowEndDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyyMMdd");
            string str_sql = "GetListPaginationByCatLienThong";
            Boolean Status = true;
            if (dto.Status == -1)
            {
                
            }
            else if (dto.Status == 0)
            {
                Status = false;
            }

            if (dto.Keyword != null && dto.Keyword != "")
            {
                string[] arrKey = dto.Keyword.Split(':');
                if (arrKey.Count() == 2)
                {
                    if (arrKey[0].ToLower() == "id")
                    {

                        dto.Id = int.Parse(arrKey[1]);
                        if (dto.Id > 0)
                        {
                            str_sql = "GetListPaginationKeyWord";
                        }
                    }
                }
            }

            var tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionStringSTP, "SP_Articles",
                new string[] { "@flag", "@Id", "@CurrentPage", "@ItemsPerPage", "@Keyword", "@CatId", "@IdCoQuan", "@AuthorId", "@StartDate", "@EndDate", "@CreatedBy", "@Status" },
                new object[] { str_sql, dto.Id, dto.CurrentPage, dto.ItemsPerPage, dto.Keyword, dto.CatId, dto.IdCoQuan, dto.AuthorId, StartDate, EndDate, dto.CreatedBy, Status });
            if (tabl == null)
            {
                return new List<Articles>();
            }
            else
            {
                try
                {


                    return (from r in tabl.AsEnumerable()
                            select new Articles
                            {
                                Id = (int)r["Id"],
                                Title = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
                                Alias = (string)((r["Alias"] == System.DBNull.Value) ? null : r["Alias"]),
                                CatId = (int)((r["CatId"] == System.DBNull.Value) ? 0 : r["CatId"]),                                
                                Category = (string)((r["Category"] == System.DBNull.Value) ? "" : r["Category"]),                                                              
                                StaticPage = (Boolean)((r["StaticPage"] == System.DBNull.Value) ? 0 : r["StaticPage"]),                               
                                Images = (string)((r["Images"] == System.DBNull.Value) ? null : r["Images"]),
                                IntroText = (string)((r["IntroText"] == System.DBNull.Value) ? null : r["IntroText"]),
                                Status = (Boolean)((r["Status"] == System.DBNull.Value) ? false : r["Status"]),                               
                                PublishUp = (DateTime)((r["PublishUp"] == System.DBNull.Value) ? null : r["PublishUp"]),
                                PublishUpShow = (string)((r["PublishUp"] == System.DBNull.Value) ? DateTime.Now.ToString("dd/MM/yyyy") : (string)((DateTime)r["PublishUp"]).ToString("dd/MM/yyyy")),
                                Ids = MyModels.Encode((int)r["Id"], SecretId),                                                               
                                TotalRows = (int)r["TotalRows"],
                                CreatedBy = (int?)((r["CreatedBy"] == System.DBNull.Value) ? 0 : r["CreatedBy"]),                                
                            }).ToList();
                }
                catch (Exception e)
                {
                    return new List<Articles>();
                }
            }


        }

        public static List<Articles> GetListPagination(SearchArticles dto, string secretId, string culture = "all")
        {
            if (dto == null)
            {
                return new List<Articles>(); // Early return on invalid dto
            }

            culture = API.Models.MyHelper.StringHelper.ConverLan(culture);

            // Normalize pagination and keyword
            dto.CurrentPage = Math.Max(1, dto.CurrentPage);
            dto.ItemsPerPage = Math.Max(10, dto.ItemsPerPage);
            dto.Keyword ??= string.Empty;

            // Safely parse dates to avoid ParseExact exceptions
            string? startDate = null;
            string? endDate = null;
            if (!string.IsNullOrEmpty(dto.ShowStartDate) && DateTime.TryParseExact(dto.ShowStartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var startParsed))
            {
                startDate = startParsed.ToString("yyyyMMdd");
            }
            if (!string.IsNullOrEmpty(dto.ShowEndDate) && DateTime.TryParseExact(dto.ShowEndDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var endParsed))
            {
                endDate = endParsed.ToString("yyyyMMdd");
            }

            // Determine SQL flag and status
            string strSql = "GetListPagination_Status";
            bool status = true;
            if (dto.Status == -1)
            {
                strSql = "GetListPagination";
            }
            else if (dto.Status == 0)
            {
                status = false;
            }
            else if (dto.Status == 2)
            {
                strSql = "GetListPaginationArticlesStatus";
            }

            // Handle keyword as ID search
            if (!string.IsNullOrEmpty(dto.Keyword))
            {
                var arrKey = dto.Keyword.Split(':');
                if (arrKey.Length == 2 && arrKey[0].ToLowerInvariant() == "id")
                {
                    if (int.TryParse(arrKey[1], out var parsedId) && parsedId > 0)
                    {
                        dto.Id = parsedId;
                        strSql = "GetListPaginationKeyWord";
                    }
                }
            }

            // //var tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Articles",
            // //    new string[] { "@flag", "@Id", "@CurrentPage", "@ItemsPerPage", "@Keyword", "@CatId", "@IdCoQuan", "@AuthorId", "@StartDate", "@EndDate", "@CreatedBy", "@Status", "@ArticlesStatusId", "@PaymentId" },
            // //    new object[] { str_sql, dto.Id, dto.CurrentPage, dto.ItemsPerPage, dto.Keyword, dto.CatId, dto.IdCoQuan, dto.AuthorId, StartDate, EndDate, dto.CreatedBy, Status, dto.ArticlesStatusId, dto.PaymentId });

            // var parameters = new List<string> { "@flag", "@Id", "@CurrentPage", "@ItemsPerPage", "@Keyword", "@CatId", "@IdCoQuan", "@AuthorId", "@StartDate", "@EndDate", "@CreatedBy", "@Status", "@ArticlesStatusId" };
            // var values = new List<object> { str_sql, dto.Id, dto.CurrentPage, dto.ItemsPerPage, dto.Keyword, dto.CatId, dto.IdCoQuan, dto.AuthorId, StartDate, EndDate, dto.CreatedBy, Status, dto.ArticlesStatusId };
            // // Case vhttdl: thêm điều kiện lọc trạng thái thanh toán bài viết
            // if (dto.PaymentId != 0)
            // {
            //     parameters.Add("@PaymentId");
            //     values.Add(dto.PaymentId);
            // }

            // var tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Articles", parameters.ToArray(), values.ToArray());

            
            // if (tabl == null)
            // Build dynamic parameters and values
            var parameters = new List<string> { "@flag", "@Id", "@CurrentPage", "@ItemsPerPage", "@Keyword", "@CatId", "@IdCoQuan", "@AuthorId", "@StartDate", "@EndDate", "@CreatedBy", "@Status", "@ArticlesStatusId" };
            var values = new List<object> { strSql, dto.Id, dto.CurrentPage, dto.ItemsPerPage, dto.Keyword, dto.CatId, dto.IdCoQuan, dto.AuthorId, startDate ?? string.Empty, endDate ?? string.Empty, dto.CreatedBy, status, dto.ArticlesStatusId };

            // Case vhttdl: Add PaymentId if set
            if (dto.PaymentId != 0)
            {
                parameters.Add("@PaymentId");
                values.Add(dto.PaymentId);
            }

            var table = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Articles", parameters.ToArray(), values.ToArray());

            if (table?.Rows.Count == 0)
            {
                return new List<Articles>(); // Early return empty
            }

            try
            {
                var options = new ArticleDataMapper.ArticleMappingOptions { SecretId = secretId };
                var articles = new List<Articles>();
                var applyEnOverrides = (culture == "en" && dto.Status == 1);

                foreach (DataRow row in table.Rows)
                {
                    var item = ArticleDataMapper.MapRowToArticle(row, ArticleDataMapper.ArticleVariant.Full, options); // Full variant for comprehensive fields (e.g., Category, AuthorName, TotalRows)
                    if (item == null) continue;

                    // Apply EN overrides only if conditions met (fallback to VI if EN null/empty)
                    if (applyEnOverrides)
                    {
                        ArticleDataMapper.ApplyCultureOverrides(item, culture);
                    }

                    articles.Add(item);
                }

                return articles;
            }
            catch (Exception)
            {
                // Log exception if logger available; for now, return empty as per original
                return new List<Articles>();
            }
        }

        
        public static List<Articles> GetListTransferPagination(SearchArticles dto, string SecretId)
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
            string StartDate = DateTime.ParseExact(dto.ShowStartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyyMMdd");
            string EndDate = DateTime.ParseExact(dto.ShowEndDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyyMMdd");

            string str_sql = "GetListTransferPagination";
            
            var tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Articles",
                new string[] { "@flag", "@CurrentPage", "@ItemsPerPage", "@Keyword", "@CatId", "@IdCoQuan", "@AuthorId", "@StartDate", "@EndDate", "@CreatedBy" },
                new object[] { str_sql, dto.CurrentPage, dto.ItemsPerPage, dto.Keyword, dto.CatId, dto.IdCoQuan, dto.AuthorId, StartDate, EndDate, dto.CreatedBy });
            if (tabl == null)
            {
                return new List<Articles>();
            }
            else
            {
                return (from r in tabl.AsEnumerable()
                        select new Articles
                        {
                            Id = (int)r["Id"],
                            Title = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
                            Alias = (string)((r["Alias"] == System.DBNull.Value) ? null : r["Alias"]),
                            CatId = (int)((r["CatId"] == System.DBNull.Value) ? 0 : r["CatId"]),
                            Category = (string)((r["Category"] == System.DBNull.Value) ? "" : r["Category"]),
                            IdCoQuan = (int)((r["IdCoQuan"] == System.DBNull.Value) ? 0 : r["IdCoQuan"]),
                            AuthorId = (int)((r["AuthorId"] == System.DBNull.Value) ? 0 : r["AuthorId"]),
                            AuthorName = (string)((r["AuthorName"] == System.DBNull.Value) ? null : r["AuthorName"]),
                            TenCoQuan = (string)((r["TenCoQuan"] == System.DBNull.Value) ? null : r["TenCoQuan"]),
                            Featured = (Boolean)((r["Featured"] == System.DBNull.Value) ? null : r["Featured"]),
                            FeaturedHome = (Boolean)((r["FeaturedHome"] == System.DBNull.Value) ? null : r["FeaturedHome"]),
                            StaticPage = (Boolean)((r["StaticPage"] == System.DBNull.Value) ? 0 : r["StaticPage"]),
                            Images = (string)((r["Images"] == System.DBNull.Value) ? null : r["Images"]),
                            IntroText = (string)((r["IntroText"] == System.DBNull.Value) ? null : r["IntroText"]),
                            Status = (Boolean)((r["Status"] == System.DBNull.Value) ? 0 : r["Status"]),
                            RootNewsFlag = (Boolean)((r["RootNewsFlag"] == System.DBNull.Value) ? false : r["RootNewsFlag"]),
                            RootNewsId = (int)((r["RootNewsId"] == System.DBNull.Value) ? 0 : r["RootNewsId"]),
                            PublishUp = (DateTime)((r["PublishUp"] == System.DBNull.Value) ? null : r["PublishUp"]),
                            PublishUpShow = (string)((r["PublishUp"] == System.DBNull.Value) ? DateTime.Now.ToString("dd/MM/yyyy") : (string)((DateTime)r["PublishUp"]).ToString("dd/MM/yyyy")),
                            CategoryTypeTitle = (string)((r["CategoryTypeTitle"] == System.DBNull.Value) ? null : r["CategoryTypeTitle"]),
                            CategoryTypeId = (int)((r["CategoryTypeId"] == System.DBNull.Value) ? 0 : r["CategoryTypeId"]),
                            QuantityImage = (int)((r["QuantityImage"] == System.DBNull.Value) ? 0 : r["QuantityImage"]),
                            Money = (Double)((r["Money"] == System.DBNull.Value) ? Double.Parse("0") : r["Money"]),
                            Author = (string)((r["Author"] == System.DBNull.Value) ? "" : r["Author"]),
                            FlagEdit = (Boolean)((r["FlagEdit"] == System.DBNull.Value) ? true : r["FlagEdit"]),
                            ModifiedByFullName = (string)((r["ModifiedByFullName"] == System.DBNull.Value) ? "" : r["ModifiedByFullName"]),
                            CreatedByFullName = (string)((r["CreatedByFullName"] == System.DBNull.Value) ? "" : r["CreatedByFullName"]),
                            Ids = MyModels.Encode((int)r["Id"], SecretId),
                            TotalRows = (int)r["TotalRows"],
                        }).ToList();
            }


        }

        public static List<SelectListItem> GetListCategoryType(int IdCoQuan = 0)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Articles",
               new string[] { "@flag", "@IdCoQuan" }, new object[] { "GetListCategoriesArticlesType", IdCoQuan });
            List<SelectListItem> ListItems = (from r in tabl.AsEnumerable()
                                              select new SelectListItem
                                              {
                                                  Value = (string)((r["Id"] == System.DBNull.Value) ? null : r["Id"].ToString()),
                                                  Text = (string)((r["Title"] == System.DBNull.Value) ? "" : r["Title"]),
                                              }).ToList();
            ListItems.Insert(0, (new SelectListItem { Text = "--- Chọn Loại Tin ---", Value = "0" }));
            return ListItems;           
        }

        public static List<SelectListItem> GetListLevelArticle()
        {

            List<SelectListItem> ListItems = new List<SelectListItem>();

            ListItems.Insert(0, (new SelectListItem { Text = "Xếp loại bài viết", Value = "0" }));
            ListItems.Insert(0, (new SelectListItem { Text = "A", Value = "1" }));
            ListItems.Insert(0, (new SelectListItem { Text = "B", Value = "2" }));
            ListItems.Insert(0, (new SelectListItem { Text = "C", Value = "3" }));
            ListItems.Insert(0, (new SelectListItem { Text = "D", Value = "4" }));
            return ListItems;

        }

        public static List<SelectListItem> GetListItemsStatus()
        {
            List<SelectListItem> ListItems = new List<SelectListItem>();
            ListItems.Insert(0, (new SelectListItem { Text = "--- Trạng Thái ---", Value = "-1" }));
            ListItems.Insert(1, (new SelectListItem { Text = "Chưa Duyệt", Value = "0" }));
            ListItems.Insert(2, (new SelectListItem { Text = "Đã Duyệt", Value = "1" }));
            return ListItems;
        }

        public static List<Articles> GetListPaginationByCat(string alias, SearchArticles dto, string secretId, string culture = "all")
        {
            if (string.IsNullOrEmpty(alias) || dto == null)
            {
                return new List<Articles>(); // Early return on invalid input
            }

            culture = API.Models.MyHelper.StringHelper.ConverLan(culture);

            // Normalize pagination and keyword
            dto.CurrentPage = Math.Max(1, dto.CurrentPage);
            dto.ItemsPerPage = Math.Max(10, dto.ItemsPerPage);
            dto.Keyword ??= string.Empty;

            var table = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Articles",
                new[] { "@flag", "@CurrentPage", "@ItemsPerPage", "@Keyword", "@CatAlias" },
                new object[] { "GetListPaginationByCat", dto.CurrentPage, dto.ItemsPerPage, dto.Keyword, alias });

            if (table?.Rows.Count == 0)
            {
                return new List<Articles>(); // Early return empty
            }

            var options = new ArticleDataMapper.ArticleMappingOptions { SecretId = secretId };
            var articles = new List<Articles>();
            var applyEnOverrides = (culture == "en" && dto.Status == 1);

            foreach (DataRow row in table.Rows)
            {
                var item = ArticleDataMapper.MapRowToArticle(row, ArticleDataMapper.ArticleVariant.Full, options); // Full variant for comprehensive fields (e.g., AuthorName, Category, TotalRows)
                if (item == null) continue;

                // Apply EN overrides only if conditions met (fallback to VI if EN null/empty)
                if (applyEnOverrides)
                {
                    ArticleDataMapper.ApplyCultureOverrides(item, culture);
                }

                articles.Add(item);
            }

            return articles;
        }

        

        public static List<SelectListItem> GetListItems(Boolean Selected = true)
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Articles",
                new string[] { "@flag", "@Selected" }, new object[] { "GetList", Convert.ToDecimal(Selected) });
            List<SelectListItem> ListItems = (from r in tabl.AsEnumerable()
                                              select new SelectListItem
                                              {
                                                  Value = (string)((r["Id"] == System.DBNull.Value) ? null : r["Id"].ToString()),
                                                  Text = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
                                              }).ToList();

            ListItems.Insert(0, (new SelectListItem { Text = "Chọn bài đăng", Value = "0" }));
            return ListItems;

        }


        public static List<Articles> GetListLogArticles(int Id, string SecretId)
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_LogArticles",
                new string[] { "@flag", "@Id" }, new object[] { "GetList", Id });
            if (tabl == null)
            {
                return new List<Articles>();
            }
            else
            {
                return (from r in tabl.AsEnumerable()
                        select new Articles
                        {
                            Id = (int)r["Id"],
                            Title = (string)r["Title"],
                            Alias = (string)((r["Alias"] == System.DBNull.Value) ? null : r["Alias"]),
                            CatId = (int)r["CatId"],
                            Category = (string)r["Category"],
                            AuthorId = (int)((r["AuthorId"] == System.DBNull.Value) ? 0 : r["AuthorId"]),
                            AuthorName = (string)((r["AuthorName"] == System.DBNull.Value) ? null : r["AuthorName"]),
                            IntroText = (string)((r["IntroText"] == System.DBNull.Value) ? null : r["IntroText"]),
                            FullText = (string)((r["FullText"] == System.DBNull.Value) ? null : r["FullText"]),
                            Status = (Boolean)r["Status"],
                            CreatedBy = (int?)((r["CreatedBy"] == System.DBNull.Value) ? null : r["CreatedBy"]),
                            CreatedByName = (string)((r["CreatedByName"] == System.DBNull.Value) ? "" : r["CreatedByName"]),
                            CreatedByFullName = (string)((r["CreatedByFullName"] == System.DBNull.Value) ? "" : r["CreatedByFullName"]),
                            ModifiedByFullName = (string)((r["ModifiedByFullName"] == System.DBNull.Value) ? "" : r["ModifiedByFullName"]),
                            ModifiedBy = (int?)((r["ModifiedBy"] == System.DBNull.Value) ? null : r["ModifiedBy"]),
                            CreatedDate = (DateTime)((r["CreatedDate"] == System.DBNull.Value) ? null : r["CreatedDate"]),
                            PublishUp = (DateTime)((r["PublishUp"] == System.DBNull.Value) ? DateTime.Now : r["PublishUp"]),
                            PublishUpShow = (string)((r["PublishUp"] == System.DBNull.Value) ? DateTime.Now.ToString("dd/MM/yyyy") : (string)((DateTime)r["PublishUp"]).ToString("dd/MM/yyyy")),
                            ModifiedDate = (DateTime?)((r["ModifiedDate"] == System.DBNull.Value) ? null : r["ModifiedDate"]),
                            Metadesc = (string)((r["Metadesc"] == System.DBNull.Value) ? null : r["Metadesc"]),
                            Metakey = (string)((r["Metakey"] == System.DBNull.Value) ? null : r["Metakey"]),
                            Metadata = (string)((r["Metadata"] == System.DBNull.Value) ? null : r["Metadata"]),
                            Language = (string)((r["Language"] == System.DBNull.Value) ? null : r["Language"]),
                            Featured = (Boolean)((r["Featured"] == System.DBNull.Value) ? null : r["Featured"]),
                            FeaturedHome = (Boolean)((r["FeaturedHome"] == System.DBNull.Value) ? null : r["FeaturedHome"]),
                            StaticPage = (Boolean)((r["StaticPage"] == System.DBNull.Value) ? null : r["StaticPage"]),
                            Images = (string)((r["Images"] == System.DBNull.Value) ? null : r["Images"]),
                            Params = (string)((r["Params"] == System.DBNull.Value) ? null : r["Params"]),
                            Author = (string)((r["Author"] == System.DBNull.Value) ? "" : r["Author"]),
                            
                            Ids = MyModels.Encode((int)r["Id"], SecretId),
                        }).ToList();
            }

        }
        public static List<SelectListItem> GetListStaticArticle(Boolean Selected = true)
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Articles",
                new string[] { "@flag", "@Selected" }, new object[] { "GetListStaticArticle", Convert.ToDecimal(Selected) });
            List<SelectListItem> ListItems = (from r in tabl.AsEnumerable()
                                              select new SelectListItem
                                              {
                                                  Value = (string)((r["Id"] == System.DBNull.Value) ? null : r["Id"].ToString()),
                                                  Text = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
                                              }).ToList();

            ListItems.Insert(0, (new SelectListItem { Text = "Chọn bài đăng", Value = "0" }));
            return ListItems;

        }

        public static List<Articles> TinNoiBatTuHuyen(int IdCoQuan)
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Articles",
                new string[] { "@flag", "@IdCoQuan" }, new object[] { "TinNoiBatTuHuyen", IdCoQuan });
            if (tabl == null)
            {
                return new List<Articles>();
            }
            else
            {
                return (from r in tabl.AsEnumerable()
                        select new Articles
                        {
                            Id = (int)r["Id"],
                            Title = (string)r["Title"],
                            Alias = (string)((r["Alias"] == System.DBNull.Value) ? null : r["Alias"]),
                            CatId = (int)r["CatId"],
                            IntroText = (string)((r["IntroText"] == System.DBNull.Value) ? null : r["IntroText"]),
                            TenCoQuan = (string)((r["TenCoQuan"] == System.DBNull.Value) ? null : r["TenCoQuan"]),
                            CodeCoQuan = (string)((r["CodeCoQuan"] == System.DBNull.Value) ? null : r["CodeCoQuan"]),
                            CreatedDate = (DateTime)((r["CreatedDate"] == System.DBNull.Value) ? null : r["CreatedDate"]),
                            Images = (string)((r["Images"] == System.DBNull.Value) ? null : r["Images"]),
                        }).ToList();
            }

        }

        public static List<Articles> ThongBaoTuHuyen(int IdCoQuan)
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Articles",
                new string[] { "@flag", "@IdCoQuan" }, new object[] { "ThongBaoTuHuyen", IdCoQuan });
            if (tabl == null)
            {
                return new List<Articles>();
            }
            else
            {
                return (from r in tabl.AsEnumerable()
                        select new Articles
                        {
                            Id = (int)r["Id"],
                            Title = (string)r["Title"],
                            Alias = (string)((r["Alias"] == System.DBNull.Value) ? null : r["Alias"]),
                            CatId = (int)r["CatId"],
                            IntroText = (string)((r["IntroText"] == System.DBNull.Value) ? null : r["IntroText"]),
                            TenCoQuan = (string)((r["TenCoQuan"] == System.DBNull.Value) ? null : r["TenCoQuan"]),
                            CodeCoQuan = (string)((r["CodeCoQuan"] == System.DBNull.Value) ? null : r["CodeCoQuan"]),
                            CreatedDate = (DateTime)((r["CreatedDate"] == System.DBNull.Value) ? null : r["CreatedDate"]),
                            Images = (string)((r["Images"] == System.DBNull.Value) ? null : r["Images"]),
                        }).ToList();
            }

        }

        public static List<Articles> LienThongTinCapXa(int IdCoQuan)
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Articles",
                new string[] { "@flag", "@IdCoQuan" }, new object[] { "LienThongTinCapXa", IdCoQuan });
            if (tabl == null)
            {
                return new List<Articles>();
            }
            else
            {
                return (from r in tabl.AsEnumerable()
                        select new Articles
                        {
                            Id = (int)r["Id"],
                            Title = (string)r["Title"],
                            Alias = (string)((r["Alias"] == System.DBNull.Value) ? null : r["Alias"]),
                            CatId = (int)r["CatId"],
                            IntroText = (string)((r["IntroText"] == System.DBNull.Value) ? null : r["IntroText"]),
                            TenCoQuan = (string)((r["TenCoQuan"] == System.DBNull.Value) ? null : r["TenCoQuan"]),
                            CodeCoQuan = (string)((r["CodeCoQuan"] == System.DBNull.Value) ? null : r["CodeCoQuan"]),                                                     
                            CreatedDate = (DateTime)((r["CreatedDate"] == System.DBNull.Value) ? null : r["CreatedDate"]),                            
                            Images = (string)((r["Images"] == System.DBNull.Value) ? null : r["Images"]),                          
                        }).ToList();
            }

        }

        public static List<Articles> GetList(Boolean Selected = true)
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Articles",
                new string[] { "@flag", "@Selected" }, new object[] { "GetList", Convert.ToDecimal(Selected) });
            if (tabl == null)
            {
                return new List<Articles>();
            }
            else
            {
                return (from r in tabl.AsEnumerable()
                        select new Articles
                        {
                            Id = (int)r["Id"],
                            Title = (string)r["Title"],
                            Alias = (string)((r["Alias"] == System.DBNull.Value) ? null : r["Alias"]),
                            CatId = (int)r["CatId"],
                            IntroText = (string)((r["IntroText"] == System.DBNull.Value) ? null : r["IntroText"]),
                            FullText = (string)((r["FullText"] == System.DBNull.Value) ? null : r["FullText"]),
                            Status = (Boolean)r["Status"],
                            CreatedBy = (int?)((r["CreatedBy"] == System.DBNull.Value) ? null : r["CreatedBy"]),
                            ModifiedBy = (int?)((r["ModifiedBy"] == System.DBNull.Value) ? null : r["ModifiedBy"]),
                            CreatedDate = (DateTime)((r["CreatedDate"] == System.DBNull.Value) ? null : r["CreatedDate"]),
                            PublishUp = (DateTime)((r["PublishUp"] == System.DBNull.Value) ? DateTime.Now : r["PublishUp"]),
                            PublishUpShow = (string)((r["PublishUp"] == System.DBNull.Value) ? DateTime.Now.ToString("dd/MM/yyyy") : r["PublishUp"]),
                            ModifiedDate = (DateTime?)((r["ModifiedDate"] == System.DBNull.Value) ? null : r["ModifiedDate"]),
                            Metadesc = (string)((r["Metadesc"] == System.DBNull.Value) ? null : r["Metadesc"]),
                            Metakey = (string)((r["Metakey"] == System.DBNull.Value) ? null : r["Metakey"]),
                            Metadata = (string)((r["Metadata"] == System.DBNull.Value) ? null : r["Metadata"]),
                            Language = (string)((r["Language"] == System.DBNull.Value) ? null : r["Language"]),
                            Featured = (Boolean)((r["Featured"] == System.DBNull.Value) ? null : r["Featured"]),
                            FeaturedHome = (Boolean)((r["FeaturedHome"] == System.DBNull.Value) ? null : r["FeaturedHome"]),
                            StaticPage = (Boolean)((r["StaticPage"] == System.DBNull.Value) ? null : r["StaticPage"]),
                            Images = (string)((r["Images"] == System.DBNull.Value) ? null : r["Images"]),
                            Params = (string)((r["Params"] == System.DBNull.Value) ? null : r["Params"]),
                            Ordering = (int?)((r["Ordering"] == System.DBNull.Value) ? null : r["Ordering"]),
                            Deleted = (Boolean)((r["Deleted"] == System.DBNull.Value) ? null : r["Deleted"]),
                            FlagEdit = (Boolean)((r["FlagEdit"] == System.DBNull.Value) ? true : r["FlagEdit"]),
                            TotalRows = (int)r["TotalRows"],
                        }).ToList();
            }

        }

        public static List<Articles> GetListRelativeNews(string alias, int catId, int idCoQuan, string culture = "all", int id = 0)
        {
            if (string.IsNullOrEmpty(alias) || catId <= 0)
            {
                return new List<Articles>(); // Early return on invalid input
            }

            culture = API.Models.MyHelper.StringHelper.ConverLan(culture);

            var table = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Articles",
                new[] { "@flag", "@Alias", "@CatId", "@IdCoQuan", "@Id" }, new object[] { "GetListRelativeNews", alias, catId, idCoQuan, id });

            if (table?.Rows.Count == 0)
            {
                return new List<Articles>(); // Early return empty
            }

            var options = new ArticleDataMapper.ArticleMappingOptions();
            var articles = new List<Articles>();

            foreach (DataRow row in table.Rows)
            {
                var item = ArticleDataMapper.MapRowToArticle(row, ArticleDataMapper.ArticleVariant.ByAlias, options); // ByAlias for matching fields
                if (item == null) continue;

                // Apply culture overrides (EN fallback to VI if EN null/empty)
                ArticleDataMapper.ApplyCultureOverrides(item, culture);

                articles.Add(item);
            }

            return articles;
        }

        

        public static List<Articles> GetListHot(int idCoQuan = 0, string culture = "all")
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(idCoQuan); // Validate, but allow 0 as per original logic

            culture = API.Models.MyHelper.StringHelper.ConverLan(culture);
            if (idCoQuan == 0)
            {
                idCoQuan = 1;
            }

            var table = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Articles",
                new[] { "@flag", "@IdCoQuan" }, new object[] { "GetListHot", idCoQuan });

            if (table?.Rows.Count == 0)
            {
                return new List<Articles>(); // Early return empty
            }

            var options = new ArticleDataMapper.ArticleMappingOptions();
            var articles = new List<Articles>();

            foreach (DataRow row in table.Rows)
            {
                var item = ArticleDataMapper.MapRowToArticle(row, ArticleDataMapper.ArticleVariant.ByAlias, options); // Use ByAlias for matching fields (e.g., FeaturedHome)
                if (item == null) continue;

                // Apply culture overrides (includes EN fallback to VI if EN null/empty)
                ArticleDataMapper.ApplyCultureOverrides(item, culture);

                articles.Add(item);
            }

            return articles;
        }

        public static List<Articles> GetListViewNotification(SearchArticles dto, string culture = "all")
        {
            if (dto == null)
            {
                return new List<Articles>(); // Early return on invalid dto
            }

            culture = API.Models.MyHelper.StringHelper.ConverLan(culture);

            // Normalize pagination and keyword
            dto.CurrentPage = Math.Max(1, dto.CurrentPage);
            dto.ItemsPerPage = Math.Max(10, dto.ItemsPerPage);
            dto.Keyword ??= string.Empty;

            var table = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Articles",
                new[] { "@flag", "@CurrentPage", "@ItemsPerPage", "@IdCoQuan" },
                new object[] { "GetListPagination_Notification", dto.CurrentPage, dto.ItemsPerPage, dto.IdCoQuan });

            if (table?.Rows.Count == 0)
            {
                return new List<Articles>(); // Early return empty
            }

            var options = new ArticleDataMapper.ArticleMappingOptions();
            var articles = new List<Articles>();
            var applyEnOverrides = (culture == "en" && dto.Status == 1);

            foreach (DataRow row in table.Rows)
            {
                var item = ArticleDataMapper.MapRowToArticle(row, ArticleDataMapper.ArticleVariant.ByAlias, options); // ByAlias for matching fields
                if (item == null) continue;

                // Apply EN overrides only if conditions met (fallback to VI if EN null/empty)
                if (applyEnOverrides)
                {
                    ArticleDataMapper.ApplyCultureOverrides(item, culture);
                }

                articles.Add(item);
            }

            return articles;
        }        

        public static List<Articles> GetListViewFeatured(SearchArticles dto, string culture = "all")
        {
            if (dto == null)
            {
                return new List<Articles>(); // Early return on invalid dto
            }

            culture = API.Models.MyHelper.StringHelper.ConverLan(culture);

            // Normalize pagination and keyword
            dto.CurrentPage = Math.Max(1, dto.CurrentPage);
            dto.ItemsPerPage = Math.Max(10, dto.ItemsPerPage);
            dto.Keyword ??= string.Empty;

            var table = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Articles",
                new[] { "@flag", "@CurrentPage", "@ItemsPerPage", "@IdCoQuan" },
                new object[] { "GetListPagination_Featured", dto.CurrentPage, dto.ItemsPerPage, dto.IdCoQuan });

            if (table?.Rows.Count == 0)
            {
                return new List<Articles>(); // Early return empty
            }

            var options = new ArticleDataMapper.ArticleMappingOptions();
            var articles = new List<Articles>();
            var applyEnOverrides = (culture == "en" && dto.Status == 1);

            foreach (DataRow row in table.Rows)
            {
                var item = ArticleDataMapper.MapRowToArticle(row, ArticleDataMapper.ArticleVariant.ByAlias, options); // ByAlias for matching fields
                if (item == null) continue;

                // Apply EN overrides only if conditions met (fallback to VI if EN null/empty)
                if (applyEnOverrides)
                {
                    ArticleDataMapper.ApplyCultureOverrides(item, culture);
                }

                articles.Add(item);
            }

            return articles;
        }

        public static List<Articles> GetListNotification(int idCoQuan = 0, string culture = "all")
        {
            culture = API.Models.MyHelper.StringHelper.ConverLan(culture);

            if (idCoQuan <= 0)
            {
                idCoQuan = 1;
            }

            var table = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Articles",
                new[] { "@flag", "@IdCoQuan" }, new object[] { "GetListNotification", idCoQuan });

            if (table?.Rows.Count == 0)
            {
                return new List<Articles>(); // Early return empty
            }

            var options = new ArticleDataMapper.ArticleMappingOptions();
            var articles = new List<Articles>();

            foreach (DataRow row in table.Rows)
            {
                var item = ArticleDataMapper.MapRowToArticle(row, ArticleDataMapper.ArticleVariant.ByAlias, options); // ByAlias for matching fields
                if (item == null) continue;

                // Apply EN overrides if culture is "en" (fallback to VI if EN null/empty)
                if (culture == "en")
                {
                    ArticleDataMapper.ApplyCultureOverrides(item, culture);
                }

                articles.Add(item);
            }

            return articles;
        }

        
        public static List<Articles> GetListTinTuc()
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Articles",
                new string[] { "@flag" }, new object[] { "GetListTinTuc" });
            if (tabl == null)
            {
                return new List<Articles>();
            }
            else
            {
                return (from r in tabl.AsEnumerable()
                        select new Articles
                        {
                            Id = (int)r["Id"],
                            Title = (string)r["Title"],
                            Alias = (string)((r["Alias"] == System.DBNull.Value) ? null : r["Alias"]),
                            CatId = (int)r["CatId"],
                            IntroText = (string)((r["IntroText"] == System.DBNull.Value) ? null : r["IntroText"]),
                            FullText = (string)((r["FullText"] == System.DBNull.Value) ? null : r["FullText"]),
                            Status = (Boolean)r["Status"],
                            CreatedBy = (int?)((r["CreatedBy"] == System.DBNull.Value) ? null : r["CreatedBy"]),
                            ModifiedBy = (int?)((r["ModifiedBy"] == System.DBNull.Value) ? null : r["ModifiedBy"]),
                            CreatedDate = (DateTime)((r["CreatedDate"] == System.DBNull.Value) ? null : r["CreatedDate"]),
                            PublishUp = (DateTime)((r["PublishUp"] == System.DBNull.Value) ? DateTime.Now : r["PublishUp"]),
                            PublishUpShow = (string)((r["PublishUp"] == System.DBNull.Value) ? DateTime.Now.ToString("dd/MM/yyyy") : (string)((DateTime)r["PublishUp"]).ToString("dd/MM/yyyy")),
                            ModifiedDate = (DateTime?)((r["ModifiedDate"] == System.DBNull.Value) ? null : r["ModifiedDate"]),
                            Metadesc = (string)((r["Metadesc"] == System.DBNull.Value) ? null : r["Metadesc"]),
                            Metakey = (string)((r["Metakey"] == System.DBNull.Value) ? null : r["Metakey"]),
                            Metadata = (string)((r["Metadata"] == System.DBNull.Value) ? null : r["Metadata"]),
                            Language = (string)((r["Language"] == System.DBNull.Value) ? null : r["Language"]),
                            Featured = (Boolean)((r["Featured"] == System.DBNull.Value) ? null : r["Featured"]),
                            FeaturedHome = (Boolean)((r["FeaturedHome"] == System.DBNull.Value) ? null : r["FeaturedHome"]),
                            StaticPage = (Boolean)((r["StaticPage"] == System.DBNull.Value) ? null : r["StaticPage"]),
                            Images = (string)((r["Images"] == System.DBNull.Value) ? null : r["Images"]),
                            Params = (string)((r["Params"] == System.DBNull.Value) ? null : r["Params"]),
                            Ordering = (int?)((r["Ordering"] == System.DBNull.Value) ? null : r["Ordering"]),
                            Author = (string)((r["Author"] == System.DBNull.Value) ? "" : r["Author"]),
                            Deleted = (Boolean)((r["Deleted"] == System.DBNull.Value) ? null : r["Deleted"])
                        }).ToList();
            }

        }
        public static List<Articles> GetListSuKien()
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Articles",
                new string[] { "@flag" }, new object[] { "GetListSuKien" });
            if (tabl == null)
            {
                return new List<Articles>();
            }
            else
            {
                return (from r in tabl.AsEnumerable()
                        select new Articles
                        {
                            Id = (int)r["Id"],
                            Title = (string)r["Title"],
                            Alias = (string)((r["Alias"] == System.DBNull.Value) ? null : r["Alias"]),
                            CatId = (int)r["CatId"],
                            IntroText = (string)((r["IntroText"] == System.DBNull.Value) ? null : r["IntroText"]),
                            FullText = (string)((r["FullText"] == System.DBNull.Value) ? null : r["FullText"]),
                            Status = (Boolean)r["Status"],
                            CreatedBy = (int?)((r["CreatedBy"] == System.DBNull.Value) ? null : r["CreatedBy"]),
                            ModifiedBy = (int?)((r["ModifiedBy"] == System.DBNull.Value) ? null : r["ModifiedBy"]),
                            CreatedDate = (DateTime)((r["CreatedDate"] == System.DBNull.Value) ? null : r["CreatedDate"]),
                            PublishUp = (DateTime)((r["PublishUp"] == System.DBNull.Value) ? DateTime.Now : r["PublishUp"]),
                            PublishUpShow = (string)((r["PublishUp"] == System.DBNull.Value) ? DateTime.Now.ToString("dd/MM/yyyy") : (string)((DateTime)r["PublishUp"]).ToString("dd/MM/yyyy")),
                            ModifiedDate = (DateTime?)((r["ModifiedDate"] == System.DBNull.Value) ? null : r["ModifiedDate"]),
                            Metadesc = (string)((r["Metadesc"] == System.DBNull.Value) ? null : r["Metadesc"]),
                            Metakey = (string)((r["Metakey"] == System.DBNull.Value) ? null : r["Metakey"]),
                            Metadata = (string)((r["Metadata"] == System.DBNull.Value) ? null : r["Metadata"]),
                            Language = (string)((r["Language"] == System.DBNull.Value) ? null : r["Language"]),
                            Featured = (Boolean)((r["Featured"] == System.DBNull.Value) ? null : r["Featured"]),
                            StaticPage = (Boolean)((r["StaticPage"] == System.DBNull.Value) ? null : r["StaticPage"]),
                            Images = (string)((r["Images"] == System.DBNull.Value) ? null : r["Images"]),
                            Params = (string)((r["Params"] == System.DBNull.Value) ? null : r["Params"]),
                            Ordering = (int?)((r["Ordering"] == System.DBNull.Value) ? null : r["Ordering"]),
                            Author = (string)((r["Author"] == System.DBNull.Value) ? "" : r["Author"]),
                            Deleted = (Boolean)((r["Deleted"] == System.DBNull.Value) ? null : r["Deleted"])
                        }).ToList();
            }

        }


        public static List<Articles> GetListNew(int catId = 0, int limit = 5, int idCoQuan = 1, string culture = "all")
        {
            culture = API.Models.MyHelper.StringHelper.ConverLan(culture);

            // Note: Limit param not passed to SP; assume handled internally or add "@Limit" if needed
            var table = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Articles",
                new[] { "@flag", "@CatId", "@IdCoQuan" }, new object[] { "GetListNew", catId, idCoQuan });

            if (table?.Rows.Count == 0)
            {
                return new List<Articles>(); // Early return empty
            }

            var options = new ArticleDataMapper.ArticleMappingOptions();
            var articles = new List<Articles>();

            foreach (DataRow row in table.Rows)
            {
                var item = ArticleDataMapper.MapRowToArticle(row, ArticleDataMapper.ArticleVariant.ByAlias, options); // ByAlias for matching fields (e.g., no FeaturedHome)
                if (item == null) continue;

                // Apply EN overrides if culture is "en" (fallback to VI if EN null/empty)
                if (culture == "en")
                {
                    ArticleDataMapper.ApplyCultureOverrides(item, culture);
                }

                articles.Add(item);
            }

            // Apply limit post-query if not handled by SP
            return articles.Take(limit).ToList();
        }
        

        public static Articles GetItem_NEW(decimal id, string secretId = null, string culture = "all", int status = -1)
        {
            var sql = status == 1 ? "GetItemFE" : "GetItem";
            culture = API.Models.MyHelper.StringHelper.ConverLan(culture);

            var table = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Articles",
                new[] { "@flag", "@Id" }, new object[] { sql, id });

            if (table.Rows.Count == 0) return null;

            var options = new ArticleDataMapper.ArticleMappingOptions { SecretId = secretId, ResetTmpFields = true };
            var item = ArticleDataMapper.MapRowToArticle(table.Rows[0], ArticleDataMapper.ArticleVariant.Full, options);
            if (item == null) return null;

            ArticleDataMapper.DeserializeJsonFields(item);
            ArticleDataMapper.ApplyCultureOverrides(item, culture);

            return item;
        }

        public static Articles GetItemByAlias_NEW(string alias, string secretId = null, string culture = "all")
        {
            ArgumentNullException.ThrowIfNull(alias); // Validate input

            culture = API.Models.MyHelper.StringHelper.ConverLan(culture);

            var table = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Articles",
                new[] { "@flag", "@Alias" }, new object[] { "GetItemByAlias", alias });

            if (table.Rows.Count == 0)
            {
                return null; // No data, return null early
            }

            var options = new ArticleDataMapper.ArticleMappingOptions { SecretId = secretId };
            var item = ArticleDataMapper.MapRowToArticle(table.Rows[0], ArticleDataMapper.ArticleVariant.ByAlias, options);
            if (item == null)
            {
                return null;
            }

            ArticleDataMapper.DeserializeJsonFields(item);
            ArticleDataMapper.ApplyCultureOverrides(item, culture);

            return item;
        }

        // Merge 
        public static Articles GetItem(decimal Id, string SecretId = null, string Culture = "all", int Status = -1)
        {
            string sql = "GetItem";
            if (Status == 1)
            {
                sql = "GetItemFE";
            }
            Culture = API.Models.MyHelper.StringHelper.ConverLan(Culture);
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Articles",
            new string[] { "@flag", "@Id" }, new object[] { sql, Id });
            Articles Item = (from r in tabl.AsEnumerable()
                             select new Articles
                             {
                                 Id = (int)r["Id"],
                                 Title = (string)r["Title"],
                                 Str_ListFile = (string)((r["Str_ListFile"] == System.DBNull.Value) ? null : r["Str_ListFile"]),
                                 Str_Link = (string)((r["Str_Link"] == System.DBNull.Value) ? null : r["Str_Link"]),
                                 Alias = (string)((r["Alias"] == System.DBNull.Value) ? null : r["Alias"]),
                                 CatId = (int)r["CatId"],
                                 IntroText = (string)((r["IntroText"] == System.DBNull.Value) ? "" : r["IntroText"]),
                                 FullText = (string)((r["FullText"] == System.DBNull.Value) ? "" : r["FullText"]),
                                 Status = (Boolean)r["Status"],
                                 CreatedBy = (int?)((r["CreatedBy"] == System.DBNull.Value) ? 0 : r["CreatedBy"]),
                                 ModifiedBy = (int?)((r["ModifiedBy"] == System.DBNull.Value) ? null : r["ModifiedBy"]),
                                 CreatedDate = (DateTime)((r["CreatedDate"] == System.DBNull.Value) ? null : r["CreatedDate"]),
                                 PublishUp = (DateTime)((r["PublishUp"] == System.DBNull.Value) ? null : r["PublishUp"]),
                                 PublishUpShow = (string)((r["PublishUp"] == System.DBNull.Value) ? DateTime.Now.ToString("dd/MM/yyyy") : (string)((DateTime)r["PublishUp"]).ToString("dd/MM/yyyy")),
                                 ModifiedDate = (DateTime?)((r["ModifiedDate"] == System.DBNull.Value) ? null : r["ModifiedDate"]),
                                 Metadesc = (string)((r["Metadesc"] == System.DBNull.Value) ? null : r["Metadesc"]),
                                 Metakey = (string)((r["Metakey"] == System.DBNull.Value) ? null : r["Metakey"]),
                                 Metadata = (string)((r["Metadata"] == System.DBNull.Value) ? null : r["Metadata"]),
                                 Language = (string)((r["Language"] == System.DBNull.Value) ? null : r["Language"]),
                                 Featured = (Boolean)((r["Featured"] == System.DBNull.Value) ? null : r["Featured"]),
                                 FeaturedHome = (Boolean)((r["FeaturedHome"] == System.DBNull.Value) ? null : r["FeaturedHome"]),
                                 StaticPage = (Boolean)((r["StaticPage"] == System.DBNull.Value) ? null : r["StaticPage"]),
                                 Images = (string)((r["Images"] == System.DBNull.Value) ? null : r["Images"]),
                                 FileItem = (string)((r["FileItem"] == System.DBNull.Value) ? null : r["FileItem"]),
                                 FileItem_EN = (string)((r["FileItem_EN"] == System.DBNull.Value) ? null : r["FileItem_EN"]),
                                 Params = (string)((r["Params"] == System.DBNull.Value) ? null : r["Params"]),
                                 Ordering = (int?)((r["Ordering"] == System.DBNull.Value) ? null : r["Ordering"]),
                                 Deleted = (Boolean)((r["Deleted"] == System.DBNull.Value) ? null : r["Deleted"]),
                                 IdCoQuan = (int)((r["IdCoQuan"] == System.DBNull.Value) ? 0 : r["IdCoQuan"]),
                                 AuthorId = (int)((r["AuthorId"] == System.DBNull.Value) ? 0 : r["AuthorId"]),
                                 RootNewsFlag = (Boolean)((r["RootNewsFlag"] == System.DBNull.Value) ? false : r["RootNewsFlag"]),
                                 RootNewsId = (int)((r["RootNewsId"] == System.DBNull.Value) ? 0 : r["RootNewsId"]),
                                 CategoryTypeId = (int)((r["CategoryTypeId"] == System.DBNull.Value) ? 0 : r["CategoryTypeId"]),
                                 QuantityImage = (int)((r["QuantityImage"] == System.DBNull.Value) ? 0 : r["QuantityImage"]),
                                 IdLevel = (int)((r["IdLevel"] == System.DBNull.Value) ? 0 : r["IdLevel"]),
                                 Money = (Double)((r["Money"] == System.DBNull.Value) ? Double.Parse("0") : r["Money"]),
                                 Author = (string)((r["Author"] == System.DBNull.Value) ? "" : r["Author"]),
                                 AuthorName = (string)((r["AuthorName"] == System.DBNull.Value) ? "" : r["AuthorName"]),
                                 FlagEdit = (Boolean)((r["FlagEdit"] == System.DBNull.Value) ? true : r["FlagEdit"]),
                                 Title_EN = (string)((r["Title_EN"] == System.DBNull.Value) ? null : r["Title_EN"]),
                                 Alias_EN = (string)((r["Alias_EN"] == System.DBNull.Value) ? null : r["Alias_EN"]),
                                 IntroText_EN = (string)((r["IntroText_EN"] == System.DBNull.Value) ? null : r["IntroText_EN"]),
                                 FullText_EN = (string)((r["FullText_EN"] == System.DBNull.Value) ? null : r["FullText_EN"]),
                                 Metadesc_EN = (string)((r["Metadesc_EN"] == System.DBNull.Value) ? null : r["Metadesc_EN"]),
                                 Metakey_EN = (string)((r["Metakey_EN"] == System.DBNull.Value) ? null : r["Metakey_EN"]),
                                 Metadata_EN = (string)((r["Metadata_EN"] == System.DBNull.Value) ? null : r["Metadata_EN"]),
                                 Ids = MyModels.Encode((int)r["Id"], SecretId),
                                 LinkRoot = (string)((r["LinkRoot"] == System.DBNull.Value) ? "" : r["LinkRoot"]),
                                 ArticlesStatusId = (int)((r["ArticlesStatusId"] == System.DBNull.Value) ? 0 : r["ArticlesStatusId"]),
                             }).FirstOrDefault();
            if (Item != null)
            {
                if (Item.Str_ListFile != null && Item.Str_ListFile != "")
                {
                    Item.ListFile = JsonConvert.DeserializeObject<List<FileArticle>>(Item.Str_ListFile);
                }
                if (Item.Str_Link != null && Item.Str_Link != "")
                {
                    Item.ListLinkArticle = JsonConvert.DeserializeObject<List<LinkArticle>>(Item.Str_Link);
                }

                if (Item.Title == "article_tmp")
                {
                    Item.Title = "";
                    Item.Alias = "";
                    Item.IntroText = "";
                    Item.FullText = "";
                }
            }
            if (Item.Metadata != null)
            {
                try
                {
                    Item.MetadataCV = JsonConvert.DeserializeObject<API.Models.MetaData>(Item.Metadata);
                }
                catch
                {
                    Item.MetadataCV = new API.Models.MetaData() { MetaTitle = Item.Title };
                }
            }
            else
            {
                Item.MetadataCV = new API.Models.MetaData() { MetaTitle = Item.Title, MetaH1 = Item.Title, MetaH3 = Item.Title };
            }

            if (Item.Metadata_EN != null)
            {
                try
                {
                    Item.MetadataCV_EN = JsonConvert.DeserializeObject<API.Models.MetaData>(Item.Metadata_EN);
                }
                catch
                {
                    Item.MetadataCV_EN = new API.Models.MetaData() { MetaTitle = Item.Title_EN, MetaH1 = Item.Title_EN, MetaH3 = Item.Title_EN };
                }
            }
            else
            {
                Item.MetadataCV_EN = new API.Models.MetaData() { MetaTitle = Item.Title_EN, MetaH1 = Item.Title_EN, MetaH3 = Item.Title_EN };
            }
            if (Culture == "en")
            {
                if (Item.Title_EN != null && Item.Title_EN != "") { Item.Title = Item.Title_EN; }
                if (Item.Alias_EN != null && Item.Alias_EN != "") { Item.Alias = Item.Alias_EN; }
                if (Item.IntroText_EN != null && Item.IntroText_EN != "") { Item.IntroText = Item.IntroText_EN; }
                if (Item.FullText_EN != null && Item.FullText_EN != "") { Item.FullText = Item.FullText_EN; }
                if (Item.Metadata_EN != null && Item.Metadata_EN != "") { Item.Metadata = Item.Metadata_EN; }
                if (Item.Metadesc_EN != null && Item.Metadesc_EN != "") { Item.Metadesc = Item.Metadesc_EN; }
                if (Item.Metakey_EN != null && Item.Metakey_EN != "") { Item.Metakey = Item.Metakey_EN; }
            }
            return Item;
        }
        public static Articles GetItemByAlias(string Alias, string SecretId = null, string Culture = "all")
        {
            Culture = API.Models.MyHelper.StringHelper.ConverLan(Culture);
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Articles",
            new string[] { "@flag", "@Alias" }, new object[] { "GetItemByAlias", Alias });
            Articles Item = (from r in tabl.AsEnumerable()
                             select new Articles
                             {
                                 Id = (int)r["Id"],
                                 Title = (string)r["Title"],
                                 Alias = (string)((r["Alias"] == System.DBNull.Value) ? null : r["Alias"]),
                                 Str_ListFile = (string)((r["Str_ListFile"] == System.DBNull.Value) ? null : r["Str_ListFile"]),
                                 Str_Link = (string)((r["Str_Link"] == System.DBNull.Value) ? null : r["Str_Link"]),
                                 CatId = (int)r["CatId"],
                                 IntroText = (string)((r["IntroText"] == System.DBNull.Value) ? null : r["IntroText"]),
                                 FullText = (string)((r["FullText"] == System.DBNull.Value) ? null : r["FullText"]),
                                 Status = (Boolean)r["Status"],
                                 CreatedBy = (int?)((r["CreatedBy"] == System.DBNull.Value) ? null : r["CreatedBy"]),
                                 ModifiedBy = (int?)((r["ModifiedBy"] == System.DBNull.Value) ? null : r["ModifiedBy"]),
                                 CreatedDate = (DateTime)((r["CreatedDate"] == System.DBNull.Value) ? null : r["CreatedDate"]),
                                 PublishUp = (DateTime)((r["PublishUp"] == System.DBNull.Value) ? null : r["PublishUp"]),
                                 PublishUpShow = (string)((r["PublishUp"] == System.DBNull.Value) ? DateTime.Now.ToString("dd/MM/yyyy") : (string)((DateTime)r["PublishUp"]).ToString("dd/MM/yyyy")),
                                 ModifiedDate = (DateTime?)((r["ModifiedDate"] == System.DBNull.Value) ? null : r["ModifiedDate"]),
                                 Metadesc = (string)((r["Metadesc"] == System.DBNull.Value) ? null : r["Metadesc"]),
                                 Metakey = (string)((r["Metakey"] == System.DBNull.Value) ? null : r["Metakey"]),
                                 Metadata = (string)((r["Metadata"] == System.DBNull.Value) ? null : r["Metadata"]),
                                 Language = (string)((r["Language"] == System.DBNull.Value) ? null : r["Language"]),
                                 Featured = (Boolean)((r["Featured"] == System.DBNull.Value) ? false : r["Featured"]),
								 FeaturedHome = (Boolean)((r["FeaturedHome"] == System.DBNull.Value) ? false : r["FeaturedHome"]),
                                 StaticPage = (Boolean)((r["StaticPage"] == System.DBNull.Value) ? null : r["StaticPage"]),
                                 Images = (string)((r["Images"] == System.DBNull.Value) ? null : r["Images"]),
                                 FileItem = (string)((r["FileItem"] == System.DBNull.Value) ? null : r["FileItem"]),
                                 FileItem_EN = (string)((r["FileItem_EN"] == System.DBNull.Value) ? null : r["FileItem_EN"]),
                                 Params = (string)((r["Params"] == System.DBNull.Value) ? null : r["Params"]),
                                 Ordering = (int?)((r["Ordering"] == System.DBNull.Value) ? null : r["Ordering"]),
                                 Deleted = (Boolean)((r["Deleted"] == System.DBNull.Value) ? null : r["Deleted"]),
                                 AuthorId = (int)((r["AuthorId"] == System.DBNull.Value) ? 0 : r["AuthorId"]),
                                 FlagEdit = (Boolean)((r["FlagEdit"] == System.DBNull.Value) ? true : r["FlagEdit"]),
                                 Ids = MyModels.Encode((int)r["Id"], SecretId),
                                 Hit = (int)r["Hit"],
                             }).FirstOrDefault();

            if (Item.Str_ListFile != null && Item.Str_ListFile != "")
            {
                Item.ListFile = JsonConvert.DeserializeObject<List<FileArticle>>(Item.Str_ListFile);
            }

            if (Item.Str_Link != null && Item.Str_Link != "")
            {
                Item.ListLinkArticle = JsonConvert.DeserializeObject<List<LinkArticle>>(Item.Str_Link);
            }
            if (Item.Metadata != null)
            {
                Item.MetadataCV = JsonConvert.DeserializeObject<API.Models.MetaData>(Item.Metadata);
            }
            else
            {
                Item.MetadataCV = new API.Models.MetaData() { MetaTitle = Item.Title, MetaH1 = Item.Title, MetaH3 = Item.Title };
            }

            if (Item.Metadata_EN != null)
            {
                try
                {
                    Item.MetadataCV_EN = JsonConvert.DeserializeObject<API.Models.MetaData>(Item.Metadata_EN);
                }
                catch
                {
                    Item.MetadataCV_EN = new API.Models.MetaData() { MetaTitle = Item.Title_EN, MetaH1 = Item.Title_EN, MetaH3 = Item.Title_EN };
                }
            }
            else
            {
                Item.MetadataCV_EN = new API.Models.MetaData() { MetaTitle = Item.Title_EN, MetaH1 = Item.Title_EN, MetaH3 = Item.Title_EN };
            }
            if (Culture == "en")
            {
                if (Item.Title_EN != null && Item.Title_EN != "") { Item.Title = Item.Title_EN; }
                if (Item.Alias_EN != null && Item.Alias_EN != "") { Item.Alias = Item.Alias_EN; }
                if (Item.IntroText_EN != null && Item.IntroText_EN != "") { Item.IntroText = Item.IntroText_EN; }
                if (Item.FullText_EN != null && Item.FullText_EN != "") { Item.FullText = Item.FullText_EN; }
                if (Item.Metadata_EN != null && Item.Metadata_EN != "") { Item.Metadata = Item.Metadata_EN; }
                if (Item.Metadesc_EN != null && Item.Metadesc_EN != "") { Item.Metadesc = Item.Metadesc_EN; }
                if (Item.Metakey_EN != null && Item.Metakey_EN != "") { Item.Metakey = Item.Metakey_EN; }

            }
            return Item;
        }

        public static Articles GetItemLogArticle(decimal Id, string SecretId = null)
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_LogArticles",
            new string[] { "@flag", "@Id" }, new object[] { "GetItem", Id });
            Articles Item = (from r in tabl.AsEnumerable()
                             select new Articles
                             {
                                 Id = (int)r["Id"],
                                 Title = (string)r["Title"],
                                 Str_ListFile = (string)((r["Str_ListFile"] == System.DBNull.Value) ? null : r["Str_ListFile"]),
                                 Str_Link = (string)((r["Str_Link"] == System.DBNull.Value) ? null : r["Str_Link"]),
                                 Alias = (string)((r["Alias"] == System.DBNull.Value) ? null : r["Alias"]),
                                 CatId = (int)r["CatId"],
                                 IntroText = (string)((r["IntroText"] == System.DBNull.Value) ? null : r["IntroText"]),
                                 FullText = (string)((r["FullText"] == System.DBNull.Value) ? null : r["FullText"]),
                                 Status = (Boolean)r["Status"],
                                 CreatedBy = (int?)((r["CreatedBy"] == System.DBNull.Value) ? null : r["CreatedBy"]),
                                 ModifiedBy = (int?)((r["ModifiedBy"] == System.DBNull.Value) ? null : r["ModifiedBy"]),
                                 CreatedDate = (DateTime)((r["CreatedDate"] == System.DBNull.Value) ? null : r["CreatedDate"]),
                                 PublishUp = (DateTime)((r["PublishUp"] == System.DBNull.Value) ? null : r["PublishUp"]),
                                 PublishUpShow = (string)((r["PublishUp"] == System.DBNull.Value) ? DateTime.Now.ToString("dd/MM/yyyy") : (string)((DateTime)r["PublishUp"]).ToString("dd/MM/yyyy")),
                                 ModifiedDate = (DateTime?)((r["ModifiedDate"] == System.DBNull.Value) ? null : r["ModifiedDate"]),
                                 Metadesc = (string)((r["Metadesc"] == System.DBNull.Value) ? null : r["Metadesc"]),
                                 Metakey = (string)((r["Metakey"] == System.DBNull.Value) ? null : r["Metakey"]),
                                 Metadata = (string)((r["Metadata"] == System.DBNull.Value) ? null : r["Metadata"]),
                                 Language = (string)((r["Language"] == System.DBNull.Value) ? null : r["Language"]),
                                 Featured = (Boolean)((r["Featured"] == System.DBNull.Value) ? null : r["Featured"]),
                                 StaticPage = (Boolean)((r["StaticPage"] == System.DBNull.Value) ? null : r["StaticPage"]),
                                 Images = (string)((r["Images"] == System.DBNull.Value) ? null : r["Images"]),
                                 FileItem = (string)((r["FileItem"] == System.DBNull.Value) ? null : r["FileItem"]),
                                 Params = (string)((r["Params"] == System.DBNull.Value) ? null : r["Params"]),
                                 Ordering = (int?)((r["Ordering"] == System.DBNull.Value) ? null : r["Ordering"]),
                                 IdCoQuan = (int)((r["IdCoQuan"] == System.DBNull.Value) ? 0 : r["IdCoQuan"]),
                                 AuthorId = (int)((r["AuthorId"] == System.DBNull.Value) ? 0 : r["AuthorId"]),
                                 Ids = MyModels.Encode((int)r["Id"], SecretId),
                             }).FirstOrDefault();
            if (Item.Str_ListFile != null && Item.Str_ListFile != "")
            {
                Item.ListFile = JsonConvert.DeserializeObject<List<FileArticle>>(Item.Str_ListFile);
            }
            if (Item.Str_Link != null && Item.Str_Link != "")
            {
                Item.ListLinkArticle = JsonConvert.DeserializeObject<List<LinkArticle>>(Item.Str_Link);
            }
            return Item;
        }

        

        public static dynamic SaveItem(Articles dto)
        {
            try
            {

            
                string Str_ListFile = null;
                string Str_Link = null;
                List<FileArticle> ListFileArticle = new List<FileArticle>();
                List<LinkArticle> ListLinkArticle = new List<LinkArticle>();
                if (dto.CatId == 0)
                {
                    dto.CatId = 1;
                }

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


                if (dto.ListLinkArticle != null && dto.ListLinkArticle.Count() > 0)
                {
                    for (int i = 0; i < dto.ListLinkArticle.Count(); i++)
                    {
                        if (dto.ListLinkArticle[i].Title != null && dto.ListLinkArticle[i].Title.Trim() != "" && dto.ListLinkArticle[i].Status == true)
                        {
                            ListLinkArticle.Add(dto.ListLinkArticle[i]);
                        }
                    }
                    if (ListLinkArticle != null && ListLinkArticle.Count() > 0)
                    {
                        Str_Link = JsonConvert.SerializeObject(ListLinkArticle);
                    }

                }
                if(dto.MetadataCV != null)
                {
                    if (dto.MetadataCV.MetaTitle == null || dto.MetadataCV.MetaTitle.Trim() == "")
                    {
                        dto.MetadataCV.MetaTitle = dto.Title;
                        dto.MetadataCV.MetaH1 = dto.Title;
                        dto.MetadataCV.MetaH3 = dto.Title;
                    }

                }
           

                if (dto.MetadataCV_EN != null)
                {
                    if (dto.MetadataCV_EN.MetaTitle == null || dto.MetadataCV_EN.MetaTitle.Trim() == "")
                    {
                        dto.MetadataCV_EN.MetaTitle = dto.Title_EN;
                        dto.MetadataCV_EN.MetaH1 = dto.Title_EN;
                        dto.MetadataCV_EN.MetaH3 = dto.Title_EN;
                    }
                    dto.Metadata_EN = JsonConvert.SerializeObject(dto.MetadataCV_EN);
                }
            
                if (dto.MetaMoneyCV != null)
                {
                    dto.MetaMoney = JsonConvert.SerializeObject(dto.MetaMoneyCV);
                }

                dto.Author = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.Author);
                dto.Metadesc = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.Metadesc);
                dto.Metadesc_EN = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.Metadesc_EN);
                dto.Metakey = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.Metakey);
                dto.Metakey_EN = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.Metakey_EN);
                dto.Metadata = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.Metadata);
                dto.Metadata_EN = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.Metadata_EN);
                dto.Title = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.Title);
                dto.Title_EN = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.Title_EN);
                dto.IntroText = API.Models.MyHelper.StringHelper.RemoveTagsIntroText(dto.IntroText);
                dto.IntroText_EN = API.Models.MyHelper.StringHelper.RemoveTagsIntroText(dto.IntroText_EN);
                dto.FullText = API.Models.MyHelper.StringHelper.RemoveTagsFullTextArticle(dto.FullText);
                dto.FullText_EN = API.Models.MyHelper.StringHelper.RemoveTagsFullTextArticle(dto.FullText_EN);

                dto.Alias = API.Models.MyHelper.StringHelper.UrlFriendly(dto.Title);
                dto.Alias_EN = API.Models.MyHelper.StringHelper.UrlFriendly(dto.Alias_EN);
			    dto.Images = API.Models.MyHelper.StringHelper.RemoveTagsLink(dto.Images);
			    dto.LinkRoot = API.Models.MyHelper.StringHelper.RemoveTagsLink(dto.LinkRoot);

			    DateTime NgayDang = DateTime.ParseExact(dto.PublishUpShow, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                dto.CreatedDate = DateTime.Now;
                DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Articles",
                new string[] { "@flag", "@Id", "@Title", "@Alias", "@CatId", "@IntroText", "@FullText", "@Status", "@CreatedBy", "@ModifiedBy", "@CreatedDate", "@ModifiedDate", "@Metadesc", "@Metakey", "@Metadata", "@Language", "@Featured", "@StaticPage", "@Images", "@FileItem", "@Params", "@Ordering", "@Deleted", "@IdCoQuan", "@FeaturedHome", "@PublishUp", "@Str_ListFile", "@Str_Link", "@LinkRoot", "@AuthorId", "@RootNewsId", "@RootNewsFlag", "@QuantityImage", "@Money", "@CategoryTypeId", "@Author" , "@FlagEdit", "@Title_EN", "@Alias_EN", "@IntroText_EN", "@FullText_EN", "@Metadesc_EN", "@Metakey_EN", "@Metadata_EN", "@MetaMoney", "@ArticlesStatusId" },
                new object[] { "SaveItem", dto.Id, dto.Title, dto.Alias, dto.CatId, dto.IntroText, dto.FullText, dto.Status, dto.CreatedBy, dto.ModifiedBy, dto.CreatedDate, dto.ModifiedDate, dto.Metadesc, dto.Metakey, dto.Metadata, dto.Language, dto.Featured, dto.StaticPage, dto.Images,dto.FileItem, dto.Params, dto.Ordering, dto.Deleted, dto.IdCoQuan, dto.FeaturedHome, NgayDang, Str_ListFile, Str_Link,dto.LinkRoot, dto.AuthorId,dto.RootNewsId ,dto.RootNewsFlag, dto.QuantityImage, dto.Money, dto.CategoryTypeId,dto.Author,dto.FlagEdit, dto.Title_EN, dto.Alias_EN, dto.IntroText_EN, dto.FullText_EN, dto.Metadesc_EN, dto.Metakey_EN, dto.Metadata_EN,dto.MetaMoney,dto.ArticlesStatusId });
                return (from r in tabl.AsEnumerable()
                        select new
                        {
                            N = (int)(r["N"]),
                        }).FirstOrDefault();
            }
            catch (Exception e)
            {
                return e;
            }

        }
        public static dynamic DeleteItem(Articles dto)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Articles",
            new string[] { "@flag", "@Id", "@ModifiedBy" },
            new object[] { "DeleteItem", dto.Id, dto.ModifiedBy });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }

        public static dynamic ChangeArticlesStatusId(Articles dto)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Articles",
            new string[] { "@flag", "@Id", "@ModifiedBy" },
            new object[] { "ChangeArticlesStatusId", dto.Id, dto.ModifiedBy });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }


        public static dynamic UpdateStatus(Articles dto)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Articles",
            new string[] { "@flag", "@Id", "@Status", "@ModifiedBy" },
            new object[] { "UpdateStatus", dto.Id, dto.Status, dto.ModifiedBy });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }
        public static dynamic UpdateStaticPage(Articles dto)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Articles",
            new string[] { "@flag", "@Id", "@StaticPage", "@ModifiedBy" },
            new object[] { "UpdateStaticPage", dto.Id, dto.StaticPage, dto.ModifiedBy });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }
        public static dynamic UpdateFeatured(Articles dto)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Articles",
            new string[] { "@flag", "@Id", "@Featured", "@ModifiedBy" },
            new object[] { "UpdateFeatured", dto.Id, dto.Featured, dto.ModifiedBy });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }

        public static dynamic UpdateFeaturedHome(Articles dto)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Articles",
            new string[] { "@flag", "@Id", "@FeaturedHome", "@ModifiedBy" },
            new object[] { "UpdateFeaturedHome", dto.Id, dto.FeaturedHome, dto.ModifiedBy });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }

        public static dynamic UpdateAlias(int Id, string Alias, string Introtext)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Articles",
            new string[] { "@flag", "@Id", "@Alias", "@Introtext" },
            new object[] { "UpdateAlias", Id, Alias, Introtext });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }
	public static dynamic UpdateLike(int Id)
		{
			DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Articles",
			new string[] { "@flag", "@Id" },
			new object[] { "UpdateLike", Id });
			return (from r in tabl.AsEnumerable()
					select new
					{
						N = (int)(r["N"]),
					}).FirstOrDefault();

		}
        public static dynamic UpdateFileAudio(int Id, string FileItem,string FileItem_EN)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Articles",
            new string[] { "@flag", "@Id", "@FileItem", "@FileItem_EN" },
            new object[] { "UpdateFileAudio", Id, FileItem , FileItem_EN });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }

        public static DataTable InsertArticlesTransfer(int Id, int IdCoQuan)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Articles",
            new string[] { "@flag", "@Id", "@IdCoQuan" },
            new object[] { "InsertArticlesTransfer", Id, IdCoQuan });
            return tabl;

        }

        public static DataTable UpdateArticlesTransfer(int Id, Articles dto)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Articles",
            new string[] { "@flag", "@Id", "@FullText", "@IntroText", "@Metadesc", "@Metakey", "@ModifiedBy", "@Str_Link", "@Str_ListFile", "@Images" },
            new object[] { "UpdateArticlesTransfer", Id, dto.FullText, dto.IntroText, dto.Metadesc, dto.Metakey, dto.ModifiedBy, dto.Str_Link, dto.Str_ListFile, dto.Images });
            return tabl;

        }

        // ************************* ArticleTMP ******************************************
        public static Articles GetArticleTMPByLink(string LinkRoot)
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Articles",
            new string[] { "@flag", "@LinkRoot" }, new object[] { "GetArticleTMPByLink", LinkRoot });
            Articles Item = (from r in tabl.AsEnumerable()
                             select new Articles
                             {
                                 Id = (int)r["Id"],
                                 Title = (string)r["Title"],
                                 Alias = (string)((r["Alias"] == System.DBNull.Value) ? null : r["Alias"]),
                                 Str_ListFile = (string)((r["Str_ListFile"] == System.DBNull.Value) ? null : r["Str_ListFile"]),
                                 Str_Link = (string)((r["Str_Link"] == System.DBNull.Value) ? null : r["Str_Link"]),
                                 CatId = (int)r["CatId"],
                                 IntroText = (string)((r["IntroText"] == System.DBNull.Value) ? null : r["IntroText"]),
                                 FullText = (string)((r["FullText"] == System.DBNull.Value) ? null : r["FullText"]),
                                 Status = (Boolean)r["Status"],
                                 CreatedBy = (int?)((r["CreatedBy"] == System.DBNull.Value) ? null : r["CreatedBy"]),
                                 ModifiedBy = (int?)((r["ModifiedBy"] == System.DBNull.Value) ? null : r["ModifiedBy"]),
                                 CreatedDate = (DateTime)((r["CreatedDate"] == System.DBNull.Value) ? null : r["CreatedDate"]),
                                 PublishUp = (DateTime)((r["PublishUp"] == System.DBNull.Value) ? null : r["PublishUp"]),
                                 PublishUpShow = (string)((r["PublishUp"] == System.DBNull.Value) ? DateTime.Now.ToString("dd/MM/yyyy") : (string)((DateTime)r["PublishUp"]).ToString("dd/MM/yyyy")),
                                 ModifiedDate = (DateTime?)((r["ModifiedDate"] == System.DBNull.Value) ? null : r["ModifiedDate"]),
                                 Metadesc = (string)((r["Metadesc"] == System.DBNull.Value) ? null : r["Metadesc"]),
                                 Metakey = (string)((r["Metakey"] == System.DBNull.Value) ? null : r["Metakey"]),
                                 Metadata = (string)((r["Metadata"] == System.DBNull.Value) ? null : r["Metadata"]),
                                 Language = (string)((r["Language"] == System.DBNull.Value) ? null : r["Language"]),
                                 Featured = (Boolean)((r["Featured"] == System.DBNull.Value) ? null : r["Featured"]),
                                 StaticPage = (Boolean)((r["StaticPage"] == System.DBNull.Value) ? null : r["StaticPage"]),
                                 Images = (string)((r["Images"] == System.DBNull.Value) ? null : r["Images"]),
                                 FileItem = (string)((r["FileItem"] == System.DBNull.Value) ? null : r["FileItem"]),
                                 Params = (string)((r["Params"] == System.DBNull.Value) ? null : r["Params"]),                                 
                                 Ordering = (int?)((r["Ordering"] == System.DBNull.Value) ? null : r["Ordering"]),
                                 Deleted = (Boolean)((r["Deleted"] == System.DBNull.Value) ? null : r["Deleted"]),
                                 AuthorId = (int)((r["AuthorId"] == System.DBNull.Value) ? 0 : r["AuthorId"]),
                                 LinkRoot = (string)((r["LinkRoot"] == System.DBNull.Value) ? null : r["LinkRoot"]),
								 LikeN = (int)((r["LikeN"] == System.DBNull.Value) ? 0 : r["LikeN"]),
                                 Hit = (int)r["Hit"],
                             }).FirstOrDefault();

            if (Item.Str_ListFile != null && Item.Str_ListFile != "")
            {
                Item.ListFile = JsonConvert.DeserializeObject<List<FileArticle>>(Item.Str_ListFile);
            }

            if (Item.Str_Link != null && Item.Str_Link != "")
            {
                Item.ListLinkArticle = JsonConvert.DeserializeObject<List<LinkArticle>>(Item.Str_Link);
            }
            return Item;
        }
        public static dynamic SaveItemTMP(Articles dto)
        {
            string Str_ListFile = null;
            string Str_Link = null;
            List<FileArticle> ListFileArticle = new List<FileArticle>();
            List<LinkArticle> ListLinkArticle = new List<LinkArticle>();
            if (dto.CatId == 0)
            {
                dto.CatId = 1;
            }

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


            if (dto.ListLinkArticle != null && dto.ListLinkArticle.Count() > 0)
            {
                for (int i = 0; i < dto.ListLinkArticle.Count(); i++)
                {
                    if (dto.ListLinkArticle[i].Title != null && dto.ListLinkArticle[i].Title.Trim() != "" && dto.ListLinkArticle[i].Status == true)
                    {
                        ListLinkArticle.Add(dto.ListLinkArticle[i]);
                    }
                }
                if (ListLinkArticle != null && ListLinkArticle.Count() > 0)
                {
                    Str_Link = JsonConvert.SerializeObject(ListLinkArticle);
                }

            }
            DateTime NgayDang = DateTime.Now;
            if (dto.PublishUpShow !=null && dto.PublishUpShow != "")
            {
                NgayDang = DateTime.ParseExact(dto.PublishUpShow, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
            
            dto.CreatedDate = DateTime.Now;
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Articles",
            new string[] { "@flag", "@Id", "@Title", "@Alias", "@CatId", "@IntroText", "@FullText", "@Status", "@CreatedBy", "@ModifiedBy", "@CreatedDate", "@ModifiedDate", "@Metadesc", "@Metakey", "@Metadata", "@Language", "@Featured", "@StaticPage", "@Images", "@FileItem", "@Params", "@Ordering", "@Deleted", "@IdCoQuan", "@FeaturedHome", "@PublishUp", "@Str_ListFile", "@Str_Link", "@AuthorId", "@RootNewsId", "@RootNewsFlag", "@QuantityImage", "@Money", "@CategoryTypeId", "@Author", "@DataFile", "@LinkRoot" },
            new object[] { "SaveItemTMP", dto.Id, dto.Title, dto.Alias, dto.CatId, dto.IntroText, dto.FullText, dto.Status, dto.CreatedBy, dto.ModifiedBy, dto.CreatedDate, dto.ModifiedDate, dto.Metadesc, dto.Metakey, dto.Metadata, dto.Language, dto.Featured, dto.StaticPage, dto.Images, dto.FileItem, dto.Params, dto.Ordering, dto.Deleted, dto.IdCoQuan, dto.FeaturedHome, NgayDang, Str_ListFile, Str_Link, dto.AuthorId, dto.RootNewsId, dto.RootNewsFlag, dto.QuantityImage, dto.Money, dto.CategoryTypeId, dto.Author, dto.DataFile, dto.LinkRoot });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }

        public static Articles GetItemTMP(decimal Id)
        {
            
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Articles",
            new string[] { "@flag", "@Id" }, new object[] { "GetItemTMP", Id });
            Articles Item = (from r in tabl.AsEnumerable()
                             select new Articles
                             {
                                 Id = (int)r["Id"],
                                 Title = (string)r["Title"],                                
                                 FullText = (string)((r["FullText"] == System.DBNull.Value) ? null : r["FullText"]),
                                 DataFile = (string)((r["DataFile"] == System.DBNull.Value) ? null : r["DataFile"]),                                
                             }).FirstOrDefault();
            if(Item!=null)
            {
                if(Item.DataFile != null && Item.DataFile != "")
                {
                    Item.ListDataFile = JsonConvert.DeserializeObject<List<TinymceFile>>(Item.DataFile);
                }                
            }
            return Item;
        }

        public static dynamic SaveItemTMPFulText(decimal Id, string FullTextCV)
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Articles",
            new string[] { "@flag", "@Id", "@FullTextCV" },
            new object[] { "SaveItemTMPFulText", Id, FullTextCV });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();
            
        }

        public static async Task<string> TaoFileBaoCao(SearchArticles dto, Boolean flag = false, DMCoQuan.DMCoQuan ItemCoQuan = null)
        {
            string ShowCurrentDate = "Đắk Lắk, ngày " + DateTime.Now.ToString("dd") + " tháng " + DateTime.Now.ToString("MM") + " năm " + DateTime.Now.ToString("yyyy");
            string CreateDate = DateTime.Now.ToString("dd/MM/yyyy");
            double Total = 0;
            List<Articles> ListItems = ArticlesService.GetListPagination(dto, "Hello");
            if (ListItems != null && ListItems.Count() > 0)
            {
                for (int i = 0; i < ListItems.Count(); i++)
                {
                    Total = Total + ListItems[i].Money;
                    if (ItemCoQuan.CategoryId == 2)
                    {
                        ListItems[i].LinkRoot = "https://"+ItemCoQuan.Code + "/" + ListItems[i].Alias + "-" + ListItems[i].Id + ".html";
                        ListItems[i].Link_EN = "https://"+ItemCoQuan.Code + "/" + ListItems[i].Alias_EN + "-" + ListItems[i].Id + ".html";

                    }
                    else
                    {
                        ListItems[i].LinkRoot = "http://" + ItemCoQuan.Code + "/" + ListItems[i].Alias + "-" + ListItems[i].Id + ".html";
                        ListItems[i].Link_EN = "http://" + ItemCoQuan.Code + "/" + ListItems[i].Alias_EN + "-" + ListItems[i].Id + ".html";
                    }
                        
                }
            }
            string TenFileLuu = "DanhSachBaiViet_" + string.Format("{0:ddMMyyHHmmss}" + ".xlsx", DateTime.Now);
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "temp");
            string outputFile = System.IO.Path.Combine(path, TenFileLuu);
            string ArticelReportTemplate = "";
            
            if (ItemCoQuan.MetadataCV.FlagAuthorText == 1)
            {
                ArticelReportTemplate = "DanhSachBaiVietAuthor.xlsx";
            }
            if (ItemCoQuan.Id == 7)
            {
                ArticelReportTemplate = "DanhSachBaiVietEakar.xlsx";
                
            }
            if (ItemCoQuan.FolderUpload == "vks")
            {
                ArticelReportTemplate = "DanhSachBaiVietVKS.xlsx";
                
            }
            if (ItemCoQuan.FolderUpload == "krongana")
            {
                ArticelReportTemplate = "DanhSachBaiVietKrongana.xlsx";                
            }
            if (ArticelReportTemplate == "")
            {
                ArticelReportTemplate = ItemCoQuan.MetadataCV.ArticelReportTemplate;
            }

            if (string.IsNullOrEmpty(ArticelReportTemplate))
            {
                ArticelReportTemplate = "DanhSachBaiViet.xlsx";
            }

            string ReportTemplate = System.IO.Path.Combine(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ReportTemplate"), ArticelReportTemplate);

            var template = new XLTemplate(ReportTemplate);
            template.AddVariable("ShowStartDate", dto.ShowStartDate);
            template.AddVariable("ShowEndDate", dto.ShowEndDate);
            template.AddVariable("ListItems", ListItems);
            template.AddVariable("ShowCurrentDate", ShowCurrentDate);
            template.AddVariable("Total", Total);
            template.AddVariable("CompanyName", ItemCoQuan.CompanyName);
            template.AddVariable("ShowTotal", API.Models.MyHelper.StringHelper.NumberToTextVN(Decimal.Parse(Total.ToString())));
            template.Generate();
            template.SaveAs(outputFile);
            return TenFileLuu;
        }

        public static string ConvertTinBai(string FullText,string host)
        {
            string FullTextCV ="";
            
            try
            {
                if (FullText != null)
                {
                    HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();
                    htmlDoc.LoadHtml(FullText);
                    IList<HtmlNode> nodesImg = htmlDoc.QuerySelectorAll("img");
                    IList<HtmlNode> nodesA = htmlDoc.QuerySelectorAll("a");
                    if (nodesImg.Count() > 0)
                    {

                        for (int imgI = 0; imgI < nodesImg.Count(); imgI++)
                        {
                            if (nodesImg[imgI].QuerySelector("img") != null)
                            {
                                string imgRoot = "";
                                if (nodesImg[imgI].Attributes["src"].Value.ToString().Contains("https://") || nodesImg[imgI].Attributes["src"].Value.ToString().Contains("http://"))
                                {
                                    imgRoot = HttpUtility.UrlDecode(nodesImg[imgI].Attributes["src"].Value.ToString());
                                }
                                else if (nodesImg[imgI].Attributes["src"].Value.ToString().Contains("data:image"))
                                {
                                    imgRoot = HttpUtility.UrlDecode(nodesImg[imgI].Attributes["src"].Value.ToString());
                                }
                                else
                                {
                                    imgRoot = host + HttpUtility.UrlDecode(nodesImg[imgI].Attributes["src"].Value.ToString());
                                }
                                htmlDoc.QuerySelectorAll("img")[imgI].Attributes["src"].Value = imgRoot;
                            }
                        }
                    }

                    if (nodesA.Count() > 0)
                    {
                        for (int nai = 0; nai < nodesA.Count(); nai++)
                        {
                            if (nodesA[nai].Attributes["href"] != null)
                            {
                                string imgRoot = "";
                                if (nodesA[nai].Attributes["src"].Value.ToString().Contains("https://") || nodesA[nai].Attributes["src"].Value.ToString().Contains("http://"))
                                {
                                    imgRoot = HttpUtility.UrlDecode(nodesA[nai].Attributes["src"].Value.ToString());
                                }
                                else
                                {
                                    imgRoot = host + HttpUtility.UrlDecode(nodesA[nai].Attributes["href"].Value.ToString());
                                }
                                htmlDoc.QuerySelectorAll("a")[nai].Attributes["href"].Value = imgRoot;
                            }
                        }
                    }
                    FullTextCV = htmlDoc.DocumentNode.OuterHtml;
                }
                
                return FullTextCV;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }


        }
    }
}
