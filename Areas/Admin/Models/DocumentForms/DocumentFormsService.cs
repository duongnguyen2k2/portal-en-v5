using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using API.Areas.Admin.Models.DocumentForms;
using API.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace API.Areas.Admin.Models.DocumentForms
{
    public class DocumentFormsService
    {
        public static List<DocumentForms> GetListPagination(SearchDocumentForms dto, string SecretId)
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

            string StartDate = DateTime.ParseExact(dto.ShowStartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyyMMdd");
            string EndDate = DateTime.ParseExact(dto.ShowEndDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyyMMdd");

            var tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_DocumentForms",
                new string[] { "@flag", "@CurrentPage", "@ItemsPerPage", "@Keyword","@StartDate", "@EndDate" },
                new object[] { str_sql, dto.CurrentPage, dto.ItemsPerPage, dto.Keyword, StartDate, EndDate });
            if (tabl == null)
            {
                return new List<DocumentForms>();
            }
            else
            {
                return (from r in tabl.AsEnumerable()
                        select new DocumentForms
                        {
                            Id = (int)r["Id"],
                            Title = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
                            Code = (string)((r["Code"] == System.DBNull.Value) ? null : r["Code"]),
                            Alias = (string)((r["Alias"] == System.DBNull.Value) ? null : r["Alias"]),                           
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

   

        public static DocumentForms GetItem(decimal Id, string SecretId = null)
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_DocumentForms",
            new string[] { "@flag", "@Id" }, new object[] { "GetItem", Id });
            return (from r in tabl.AsEnumerable()
                    select new DocumentForms
                    {
                        Id = (int)r["Id"],
                        Title = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
                        Code = (string)((r["Code"] == System.DBNull.Value) ? null : r["Code"]),
                        Alias = (string)((r["Alias"] == System.DBNull.Value) ? null : r["Alias"]),                      
                        Link = (string)((r["Link"] == System.DBNull.Value) ? null : r["Link"]),
                        Link2 = (string)((r["Link2"] == System.DBNull.Value) ? null : r["Link2"]),
                        Introtext = (string)((r["Introtext"] == System.DBNull.Value) ? null : r["Introtext"]),                    
                        Status = (Boolean)r["Status"],
                        IssuedDate = (DateTime)((r["IssuedDate"] == System.DBNull.Value) ? DateTime.Now : r["IssuedDate"]),                    
                        IssuedDateShow = (string)((r["IssuedDate"] == System.DBNull.Value) ? DateTime.Now.ToString("dd/MM/yyyy") : (string)((DateTime)r["IssuedDate"]).ToString("dd/MM/yyyy")),  
                        Author = (string)((r["Author"] == System.DBNull.Value) ? null : r["Author"]),
                        Ids = MyModels.Encode((int)r["Id"], SecretId),
                    }).FirstOrDefault();
        }

        public static dynamic SaveItem(DocumentForms dto)
        {
            dto.Title = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.Title);
            dto.Introtext = API.Models.MyHelper.StringHelper.RemoveTagsIntroText(dto.Introtext);
            dto.Link = API.Models.MyHelper.StringHelper.RemoveTagsLink(dto.Link);
            dto.Link2 = API.Models.MyHelper.StringHelper.RemoveTagsLink(dto.Link2);

            dto.Alias = API.Models.MyHelper.StringHelper.UrlFriendly(dto.Title);

            DateTime IssuedDate = DateTime.ParseExact(dto.IssuedDateShow, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_DocumentForms",
            new string[] { "@flag", "@Id", "@Title","@Link", "@Alias", "@Introtext","@Status", "@IdCoQuan", "@CreatedBy", "@ModifiedBy", "@IssuedDate", "@Author","@Link2","@Code" },
            new object[] { "SaveItem", dto.Id, dto.Title,dto.Link, dto.Alias, dto.Introtext,dto.Status,dto.IdCoQuan, dto.CreatedBy, dto.ModifiedBy, IssuedDate,dto.Author,dto.Link2,dto.Code });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }
        public static dynamic DeleteItem(DocumentForms dto)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_DocumentForms",
            new string[] { "@flag", "@Id", "@ModifiedBy", "@IdCoQuan" },
            new object[] { "DeleteItem", dto.Id, dto.ModifiedBy ,dto.IdCoQuan });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }

        public static dynamic UpdateStatus(DocumentForms dto)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_DocumentForms",
            new string[] { "@flag", "@Id", "@Status", "@ModifiedBy", "@IdCoQuan" },
            new object[] { "UpdateStatus", dto.Id, dto.Status, dto.ModifiedBy,dto.IdCoQuan });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }


    }
}
