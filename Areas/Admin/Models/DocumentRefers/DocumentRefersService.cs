using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using API.Areas.Admin.Models.DocumentRefers;
using API.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace API.Areas.Admin.Models.DocumentRefers
{
    public class DocumentRefersService
    {
        public static List<DocumentRefers> GetListPagination(SearchDocumentRefers dto, string SecretId)
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


            string str_sql = "GetListPagination";
            if (dto.Status != -1)
            {
                str_sql = "GetListPagination_Status";
            }
            if (dto.TaiLieuHop == true)
            {
                str_sql = "GetListPagination_TLH";
            }

            string StartDate = DateTime.ParseExact(dto.ShowStartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyyMMdd");
            string EndDate = DateTime.ParseExact(dto.ShowEndDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyyMMdd");

            var tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_DocumentRefers",
                new string[] { "@flag", "@CurrentPage", "@ItemsPerPage", "@Keyword","@StartDate", "@EndDate","@CatId" , "@IdCoQuan" },
                new object[] { str_sql, dto.CurrentPage, dto.ItemsPerPage, dto.Keyword, StartDate, EndDate,dto.CatId,dto.IdCoQuan });
            if (tabl == null)
            {
                return new List<DocumentRefers>();
            }
            else
            {
                return (from r in tabl.AsEnumerable()
                        select new DocumentRefers
                        {
                            Id = (int)r["Id"],
                            CatId= (int)r["CatId"],
                            Title = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
                            Alias = (string)((r["Alias"] == System.DBNull.Value) ? null : r["Alias"]),                            
                            LinkPrivate = (string)((r["LinkPrivate"] == System.DBNull.Value) ? null : r["LinkPrivate"]),
                            Link = (string)((r["Link"] == System.DBNull.Value) ? null : r["Link"]),
                            Link2 = (string)((r["Link2"] == System.DBNull.Value) ? null : r["Link2"]),
                            Introtext = (string)((r["Introtext"] == System.DBNull.Value) ? null : r["Introtext"]),
                            Status = (Boolean)r["Status"],
                            IssuedDate = (DateTime)((r["IssuedDate"] == System.DBNull.Value) ? DateTime.Now : r["IssuedDate"]), 
                            Author = (string)((r["Author"] == System.DBNull.Value) ? null : r["Author"]),
                            Ids = MyModels.Encode((int)r["Id"], SecretId),
                            TotalRows = (int)r["TotalRows"],
                        }).ToList();
            }


        }

        public static List<DocumentRefers> GetListPaginationFE(SearchDocumentRefers dto, string SecretId)
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


            string str_sql = "GetListPagination_FE";
           

            string StartDate = DateTime.ParseExact(dto.ShowStartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyyMMdd");
            string EndDate = DateTime.ParseExact(dto.ShowEndDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyyMMdd");

            var tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_DocumentRefers",
                new string[] { "@flag", "@CurrentPage", "@ItemsPerPage", "@Keyword", "@StartDate", "@EndDate", "@CatId", "@IdCoQuan" },
                new object[] { str_sql, dto.CurrentPage, dto.ItemsPerPage, dto.Keyword, StartDate, EndDate, dto.CatId, dto.IdCoQuan });
            if (tabl == null)
            {
                return new List<DocumentRefers>();
            }
            else
            {
                return (from r in tabl.AsEnumerable()
                        select new DocumentRefers
                        {
                            Id = (int)r["Id"],
                            CatId = (int)r["CatId"],
                            Title = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
                            Alias = (string)((r["Alias"] == System.DBNull.Value) ? null : r["Alias"]),
                            
                            LinkPrivate = (string)((r["LinkPrivate"] == System.DBNull.Value) ? null : r["LinkPrivate"]),
                            Link = (string)((r["Link"] == System.DBNull.Value) ? null : r["Link"]),
                            Link2 = (string)((r["Link2"] == System.DBNull.Value) ? null : r["Link2"]),
                            Introtext = (string)((r["Introtext"] == System.DBNull.Value) ? null : r["Introtext"]),
                            Status = (Boolean)r["Status"],
                            IssuedDate = (DateTime)((r["IssuedDate"] == System.DBNull.Value) ? DateTime.Now : r["IssuedDate"]),
                            Author = (string)((r["Author"] == System.DBNull.Value) ? null : r["Author"]),
                            Ids = MyModels.Encode((int)r["Id"], SecretId),
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

   

        public static DocumentRefers GetItem(decimal Id, string SecretId = null)
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_DocumentRefers",
            new string[] { "@flag", "@Id" }, new object[] { "GetItem", Id });
            return (from r in tabl.AsEnumerable()
                    select new DocumentRefers
                    {
                        Id = (int)r["Id"],
                        CatId = (int)r["CatId"],
                        Title = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
                        Alias = (string)((r["Alias"] == System.DBNull.Value) ? null : r["Alias"]),
                        
                        LinkPrivate = (string)((r["LinkPrivate"] == System.DBNull.Value) ? null : r["LinkPrivate"]),
                        Link = (string)((r["Link"] == System.DBNull.Value) ? null : r["Link"]),
                        Link2 = (string)((r["Link2"] == System.DBNull.Value) ? null : r["Link2"]),
                        Introtext = (string)((r["Introtext"] == System.DBNull.Value) ? null : r["Introtext"]),
                        FullText = (string)((r["FullText"] == System.DBNull.Value) ? null : r["FullText"]),                    
                        Status = (Boolean)r["Status"],
                        IssuedDate = (DateTime)((r["IssuedDate"] == System.DBNull.Value) ? DateTime.Now : r["IssuedDate"]),                    
                        IssuedDateShow = (string)((r["IssuedDate"] == System.DBNull.Value) ? DateTime.Now.ToString("dd/MM/yyyy") : (string)((DateTime)r["IssuedDate"]).ToString("dd/MM/yyyy")),  
                        Author = (string)((r["Author"] == System.DBNull.Value) ? null : r["Author"]),
                        Ids = MyModels.Encode((int)r["Id"], SecretId),
                    }).FirstOrDefault();
        }

        public static DocumentRefers GetItemFE(decimal Id, int IdCoQuan, string SecretId = null)
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_DocumentRefers",
            new string[] { "@flag", "@Id", "@IdCoQuan" }, new object[] { "GetItemFE", Id, IdCoQuan });
            return (from r in tabl.AsEnumerable()
                    select new DocumentRefers
                    {
                        Id = (int)r["Id"],
                        CatId = (int)r["CatId"],
                        Title = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
                        Alias = (string)((r["Alias"] == System.DBNull.Value) ? null : r["Alias"]),                        
                        LinkPrivate = (string)((r["LinkPrivate"] == System.DBNull.Value) ? null : r["LinkPrivate"]),
                        Link = (string)((r["Link"] == System.DBNull.Value) ? null : r["Link"]),
                        Link2 = (string)((r["Link2"] == System.DBNull.Value) ? null : r["Link2"]),
                        Introtext = (string)((r["Introtext"] == System.DBNull.Value) ? null : r["Introtext"]),
                        FullText = (string)((r["FullText"] == System.DBNull.Value) ? null : r["FullText"]),
                        Status = (Boolean)r["Status"],
                        IssuedDate = (DateTime)((r["IssuedDate"] == System.DBNull.Value) ? DateTime.Now : r["IssuedDate"]),
                        IssuedDateShow = (string)((r["IssuedDate"] == System.DBNull.Value) ? DateTime.Now.ToString("dd/MM/yyyy") : (string)((DateTime)r["IssuedDate"]).ToString("dd/MM/yyyy")),
                        Author = (string)((r["Author"] == System.DBNull.Value) ? null : r["Author"]),
                        Ids = MyModels.Encode((int)r["Id"], SecretId),
                    }).FirstOrDefault();
        }

        public static dynamic SaveItem(DocumentRefers dto)
        {
            dto.Author = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.Author);
            dto.Title = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.Title);
            dto.Introtext = API.Models.MyHelper.StringHelper.RemoveTagsIntroText(dto.Introtext);
            dto.FullText = API.Models.MyHelper.StringHelper.RemoveTagsFullText(dto.FullText);
            dto.Link = API.Models.MyHelper.StringHelper.RemoveTagsLink(dto.Link);
            dto.Link2 = API.Models.MyHelper.StringHelper.RemoveTagsLink(dto.Link2);
            dto.KeyLink = API.Models.MyHelper.StringHelper.RemoveTagsLink(dto.KeyLink);
            dto.LinkPrivate = API.Models.MyHelper.StringHelper.RemoveTagsLink(dto.LinkPrivate);

            dto.Alias = API.Models.MyHelper.StringHelper.UrlFriendly(dto.Title);

            DateTime IssuedDate = DateTime.ParseExact(dto.IssuedDateShow, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_DocumentRefers",
            new string[] { "@flag", "@Id", "@Title","@Link", "@LinkPrivate", "@KeyLink", "@Alias", "@Introtext", "@FullText", "@Status", "@IdCoQuan", "@CreatedBy", "@ModifiedBy", "@IssuedDate", "@Author","@Link2","@CatId"},
            new object[] { "SaveItem", dto.Id, dto.Title,dto.Link, dto.LinkPrivate,dto.KeyLink, dto.Alias, dto.Introtext,dto.FullText,dto.Status,dto.IdCoQuan, dto.CreatedBy, dto.ModifiedBy, IssuedDate,dto.Author,dto.Link2,dto.CatId });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }
        public static dynamic DeleteItem(DocumentRefers dto)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_DocumentRefers",
            new string[] { "@flag", "@Id", "@ModifiedBy", "@IdCoQuan" },
            new object[] { "DeleteItem", dto.Id, dto.ModifiedBy ,dto.IdCoQuan });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }

        public static dynamic UpdateStatus(DocumentRefers dto)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_DocumentRefers",
            new string[] { "@flag", "@Id", "@Status", "@ModifiedBy", "@IdCoQuan" },
            new object[] { "UpdateStatus", dto.Id, dto.Status, dto.ModifiedBy ,dto.IdCoQuan });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }


    }
}
