using API.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;

namespace API.Areas.Admin.Models.DMLoaiPhongBan
{
    public class DMLoaiPhongBanServices
    {

        public static List<DMLoaiPhongBan> GetListPagination(SearchDMLoaiPhongBan dto, string SecretId)
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
            var tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "DanhMuc_LoaiPhongBan",
                new string[] { "@flag", "@CurrentPage", "@ItemsPerPage", "@Keyword", "@IdCoQuan" },
                new object[] { "GetListPagination", dto.CurrentPage, dto.ItemsPerPage, dto.Keyword, dto.IdCoQuan });
            if (tabl == null)
            {
                return new List<DMLoaiPhongBan>();
            }
            else
            {
                return (from r in tabl.AsEnumerable()
                        select new DMLoaiPhongBan
                        {
                            Id = (int)r["Id"],
                            Title = (string)r["Title"],
                            IdCoQuan = (int)((r["IdCoQuan"] == System.DBNull.Value) ? 0 : r["IdCoQuan"]),
                            Description = (string)((r["Description"] == System.DBNull.Value) ? null : r["Description"]),
                            Code = (string)((r["Code"] == System.DBNull.Value) ? null : r["Code"]),
                            Ids = MyModels.Encode((int)r["Id"], SecretId),
                            TotalRows = (int)r["TotalRows"],
                            Status = (bool)r["Status"],
                        }).ToList();
            }
        }

        public static DataTable GetList(Boolean Selected = true, int IdType = 0, int IdCoQuan = 0)
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "DanhMuc_LoaiPhongBan",
                new string[] { "@flag", "@Selected", "@IdCoQuan" },
                new object[] { "GetList", Selected, IdCoQuan });
            return tabl;

        }

        public static DataTable GetListAPI(Boolean Selected = true, int IdType = 0, int IdCoQuan = 0)
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "DanhMuc_LoaiPhongBan",
                new string[] { "@flag", "@Selected", "@IdCoQuan" },
                new object[] { "GetListAPI", IdType, IdCoQuan });
            return tabl;

        }

        public static List<SelectListItem> GetListSelectItems(Boolean Selected = true)
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "DanhMuc_LoaiPhongBan",
                new string[] { "@flag", "@Selected" }, new object[] { "GetList", Convert.ToDecimal(Selected) });
            List<SelectListItem> ListItems = (from r in tabl.AsEnumerable()
                                              select new SelectListItem
                                              {
                                                  Value = (string)((r["Id"] == System.DBNull.Value) ? null : r["Id"].ToString()),
                                                  Text = (string)((r["Title"] == System.DBNull.Value) ? "" : r["Title"]),
                                              }).ToList();
            ListItems.Insert(0, (new SelectListItem { Text = "--- Chọn Danh mục ---", Value = "0" }));
            return ListItems;

        }

        public static DMLoaiPhongBan GetItem(int Id, string SecretId = null)
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "DanhMuc_LoaiPhongBan",
            new string[] { "@flag", "@Id" }, new object[] { "GetItem", Id });
            return (from r in tabl.AsEnumerable()
                    select new DMLoaiPhongBan
                    {
                        Id = (int)r["Id"],
                        Title = (string)r["Title"],
                        Description = (string)((r["Description"] == System.DBNull.Value) ? null : r["Description"]),
                        Code = (string)((r["Code"] == System.DBNull.Value) ? null : r["Code"]),
                        Status = (bool)r["Status"],
                        IdCoQuan = (int)((r["IdCoQuan"] == System.DBNull.Value) ? 0 : r["IdCoQuan"]),
                        Ids = MyModels.Encode((int)r["Id"], SecretId),
                    }).FirstOrDefault();
        }

        public static dynamic SaveItem(DMLoaiPhongBan dto)
        {
            dto.Title = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.Title);
            dto.Description = API.Models.MyHelper.StringHelper.RemoveTagsIntroText(dto.Description);

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "DanhMuc_LoaiPhongBan",
            new string[] { "@flag", "@Id", "@Title", "@Description", "@Status", "@CreatedBy", "@ModifiedBy", "@Code", "@IdCoQuan" },
            new object[] { "SaveItem", dto.Id, dto.Title, dto.Description, dto.Status, dto.CreatedBy, dto.ModifiedBy, dto.Code, dto.IdCoQuan });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }

        public static dynamic UpdateStatus(DMLoaiPhongBan dto)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "DanhMuc_LoaiPhongBan",
            new string[] { "@flag", "@Id", "@Status", "@ModifiedBy" },
            new object[] { "UpdateStatus", dto.Id, dto.Status, dto.ModifiedBy });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }

        public static dynamic DeleteItem(DMLoaiPhongBan dto)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "DanhMuc_LoaiPhongBan",
            new string[] { "@flag", "@Id", "@ModifiedBy" },
            new object[] { "DeleteItem", dto.Id, dto.ModifiedBy });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }

    }
}
