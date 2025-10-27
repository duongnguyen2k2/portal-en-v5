using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using API.Areas.Admin.Models.Reviews;
using API.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace API.Areas.Admin.Models.Reviews
{
    public class ReviewsService
    {
        public static List<Reviews> GetListPagination(SearchReviews dto, string SecretId)
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
            var tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Reviews",
                new string[] { "@flag", "@CurrentPage", "@ItemsPerPage", "@Keyword" , "@IdCoQuan" },
                new object[] { "GetListPagination", dto.CurrentPage, dto.ItemsPerPage, dto.Keyword ,dto.IdCoQuan });
            if (tabl == null)
            {
                return new List<Reviews>();
            }
            else
            {
                return (from r in tabl.AsEnumerable()
                        select new Reviews
                        {
                            Id = (int)r["Id"],
                            Title = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
                            FullName = (string)((r["FullName"] == System.DBNull.Value) ? null : r["FullName"]),
                            Description = (string)((r["Description"] == System.DBNull.Value) ? null : r["Description"]),
                            ReviewDate = (DateTime)((r["ReviewDate"] == System.DBNull.Value) ? null : r["ReviewDate"]),                            
                            Featured = (Boolean)((r["Featured"] == System.DBNull.Value) ? 0 : r["Featured"]),
                            Status = (Boolean)((r["Status"] == System.DBNull.Value) ? null : r["Status"]),
                            Introtext = (string)((r["Introtext"] == System.DBNull.Value) ? null : r["Introtext"]),
                            Star = (int)r["Star"],                            
                            Image = (string)((r["Image"] == System.DBNull.Value) ? null : r["Image"]),
                            Ids = MyModels.Encode((int)r["Id"], SecretId),
                        }).ToList();
            }


        }
        public static List<Reviews> GetListFeatured(int IdCoQuan)
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Reviews",
                new string[] { "@flag", "@IdCoQuan" }, new object[] { "GetListFeatured", IdCoQuan });
            if (tabl == null)
            {
                return new List<Reviews>();
            }
            else
            {
                return (from r in tabl.AsEnumerable()
                        select new Reviews
                        {
                            Id = (int)r["Id"],
                            Title = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
                            FullName = (string)((r["FullName"] == System.DBNull.Value) ? null : r["FullName"]),
                            Description = (string)((r["Description"] == System.DBNull.Value) ? null : r["Description"]),
                            ReviewDate = (DateTime)((r["ReviewDate"] == System.DBNull.Value) ? null : r["ReviewDate"]),
                            Featured = (Boolean)((r["Featured"] == System.DBNull.Value) ? 0 : r["Featured"]),
                            Status = (Boolean)((r["Status"] == System.DBNull.Value) ? null : r["Status"]),
                            Introtext = (string)((r["Introtext"] == System.DBNull.Value) ? null : r["Introtext"]),
                            Star = (int)r["Star"],
                            Image = (string)((r["Image"] == System.DBNull.Value) ? null : r["Image"]),
                        }).ToList();
            }

        }



        public static List<SelectListItem> GetListStar()
        {

            List<SelectListItem> ListItems = new List<SelectListItem>();
            for (int i = 0; i < 6; i++) {
                ListItems.Insert(i, (new SelectListItem { Text = i.ToString(), Value =i.ToString() }));
            }                        
            return ListItems;
        }

        public static Reviews GetItem(decimal Id, string SecretId = null)
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Reviews",
            new string[] { "@flag", "@Id" }, new object[] { "GetItem", Id });
            return (from r in tabl.AsEnumerable()
                    select new Reviews
                    {
                        Id = (int)r["Id"],
                        Title = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
                        FullName = (string)((r["FullName"] == System.DBNull.Value) ? null : r["FullName"]),
                        Description = (string)((r["Description"] == System.DBNull.Value) ? null : r["Description"]),
                        ReviewDate = (DateTime)((r["ReviewDate"] == System.DBNull.Value) ? null : r["ReviewDate"]),
                        ReviewDateShow = (string)((r["ReviewDate"] == System.DBNull.Value) ? DateTime.Now.ToString("dd/MM/yyyy") : (string)((DateTime)r["ReviewDate"]).ToString("dd/MM/yyyy")),
                        Featured = (Boolean)((r["Featured"] == System.DBNull.Value) ? 0 : r["Featured"]),
                        Status = (Boolean)((r["Status"] == System.DBNull.Value) ? null : r["Status"]),
                        Introtext = (string)((r["Introtext"] == System.DBNull.Value) ? null : r["Introtext"]),
                        Star = (int)r["Star"],
                        IdCoQuan = (int)r["IdCoQuan"],
                        Ids = MyModels.Encode((int)r["Id"], SecretId),
                    }).FirstOrDefault();
        }

        public static dynamic SaveItem(Reviews dto)
        {
            dto.Title = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.Title);
            dto.FullName = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.FullName);
            dto.Image = API.Models.MyHelper.StringHelper.RemoveTagsLink(dto.Image);
            dto.Description = API.Models.MyHelper.StringHelper.RemoveTagsIntroText(dto.Description);

            DateTime ReviewsDate = DateTime.ParseExact(dto.ReviewDateShow, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Reviews",
            new string[] { "@flag", "@Id", "@Title", "@Description", "@Status", "@Featured", "@CreatedBy", "@ModifiedBy", "@Introtext", "@Star", "@FullName", "@ReviewDate", "@Image", "@IdCoQuan" },
            new object[] { "SaveItem", dto.Id, dto.Title, dto.Description, dto.Status,dto.Featured, dto.CreatedBy, dto.ModifiedBy, dto.Introtext, dto.Star, dto.FullName, ReviewsDate, dto.Image,dto.IdCoQuan});
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }
        public static dynamic DeleteItem(Reviews dto)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Reviews",
            new string[] { "@flag", "@Id", "@ModifiedBy" },
            new object[] { "DeleteItem", dto.Id, dto.ModifiedBy });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }

        public static dynamic UpdateStatus(Reviews dto)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Reviews",
            new string[] { "@flag", "@Id", "@Status", "@ModifiedBy" },
            new object[] { "UpdateStatus", dto.Id, dto.Status, dto.ModifiedBy });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }

        public static dynamic UpdateFeatured(Reviews dto)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Reviews",
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
