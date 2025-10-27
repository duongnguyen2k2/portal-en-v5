using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using API.Areas.Admin.Models.DMTinhThanh;
using API.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace API.Areas.Admin.Models.DMTinhThanh
{
    public class DMTinhThanhService
    {
        public static List<DMTinhThanh> GetListPagination(SearchDMTinhThanh dto, string SecretId)
        {
            if (dto.CurrentPage <= 0)
            {
                dto.CurrentPage = 1;
            }
            var tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "DanhMuc_TinhThanh",
                new string[] { "@flag", "@CurrentPage", "@ItemsPerPage", "@Keyword" },
                new object[] { "GetListPagination", dto.CurrentPage, dto.ItemsPerPage, dto.Keyword });
            if (tabl == null)
            {
                return new List<DMTinhThanh>();
            }
            else
            {
                return (from r in tabl.AsEnumerable()
                        select new DMTinhThanh
                        {
                            Id = (int?)((r["Id"] == System.DBNull.Value) ? null : r["Id"]),
                            Ten = (string)((r["Ten"] == System.DBNull.Value) ? null : r["Ten"]),
                            Ma = (string)((r["Ma"] == System.DBNull.Value) ? null : r["Ma"]),
                            MoTa = (string)((r["MoTa"] == System.DBNull.Value) ? null : r["MoTa"]),
                            TrangThai = (Boolean?)((r["TrangThai"] == System.DBNull.Value) ? null : r["TrangThai"]),
                            CreatedBy = (int?)((r["CreatedBy"] == System.DBNull.Value) ? null : r["CreatedBy"]),
                            CreatedDate = (DateTime?)((r["CreatedDate"] == System.DBNull.Value) ? null : r["CreatedDate"]),
                            ModifiedBy = (int?)((r["ModifiedBy"] == System.DBNull.Value) ? null : r["ModifiedBy"]),
                            ModifiedDate = (DateTime?)((r["ModifiedDate"] == System.DBNull.Value) ? null : r["ModifiedDate"]),
                            Deleted = (Boolean?)((r["Deleted"] == System.DBNull.Value) ? null : r["Deleted"]),
                            Ids = MyModels.Encode((int)r["Id"], SecretId),
                            TotalRows = (int)r["TotalRows"],
                        }).ToList();
            }


        }
        public static List<SelectListItem> GetListSelectItems(Boolean Selected = true)
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "DanhMuc_TinhThanh",
                new string[] { "@flag", "@Selected" }, new object[] { "GetList", Convert.ToDecimal(Selected) });
            List < SelectListItem > ListItems = (from r in tabl.AsEnumerable()
                                                 select new SelectListItem
                                                 {
                                                     Value = (string)((r["Id"] == System.DBNull.Value) ? null : r["Id"].ToString()),
                                                     Text = (string)((r["Ten"] == System.DBNull.Value) ? null : r["Ten"]),
                                                 }).ToList();

            ListItems.Insert(0, (new SelectListItem { Text = "--- Chọn Tỉnh Thành ---", Value = "0" }));
            return ListItems;

        }
        public static List<DMTinhThanh> GetList(Boolean Selected = true)
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "DanhMuc_TinhThanh",
                new string[] { "@flag", "@Selected" }, new object[] { "GetList", Convert.ToDecimal(Selected) });
            return (from r in tabl.AsEnumerable()
                    select new DMTinhThanh
                    {
                        Id = (int?)((r["Id"] == System.DBNull.Value) ? null : r["Id"]),
                        Ten = (string)((r["Ten"] == System.DBNull.Value) ? null : r["Ten"]),
                        Ma = (string)((r["Ma"] == System.DBNull.Value) ? null : r["Ma"]),
                        MoTa = (string)((r["MoTa"] == System.DBNull.Value) ? null : r["MoTa"]),
                        TrangThai = (Boolean?)((r["TrangThai"] == System.DBNull.Value) ? null : r["TrangThai"]),                       
                    }).ToList();

        }

        public static DMTinhThanh GetItem(decimal Id, string SecretId = null)
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "DanhMuc_TinhThanh",
            new string[] { "@flag", "@Id" }, new object[] { "GetItem", Id });
            return (from r in tabl.AsEnumerable()
                    select new DMTinhThanh
                    {
                        Id = (int?)((r["Id"] == System.DBNull.Value) ? null : r["Id"]),
                        Ten = (string)((r["Ten"] == System.DBNull.Value) ? null : r["Ten"]),
                        Ma = (string)((r["Ma"] == System.DBNull.Value) ? null : r["Ma"]),
                        MoTa = (string)((r["MoTa"] == System.DBNull.Value) ? null : r["MoTa"]),
                        TrangThai = (Boolean?)((r["TrangThai"] == System.DBNull.Value) ? null : r["TrangThai"]),
                        CreatedBy = (int?)((r["CreatedBy"] == System.DBNull.Value) ? null : r["CreatedBy"]),
                        CreatedDate = (DateTime?)((r["CreatedDate"] == System.DBNull.Value) ? null : r["CreatedDate"]),
                        ModifiedBy = (int?)((r["ModifiedBy"] == System.DBNull.Value) ? null : r["ModifiedBy"]),
                        ModifiedDate = (DateTime?)((r["ModifiedDate"] == System.DBNull.Value) ? null : r["ModifiedDate"]),
                        Deleted = (Boolean?)((r["Deleted"] == System.DBNull.Value) ? null : r["Deleted"]),
                        Ids = MyModels.Encode((int)r["Id"], SecretId),
                    }).FirstOrDefault();
        }

        public static dynamic SaveItem(DMTinhThanh dto)
        {
            Regex rRemScript = new Regex(@"<script[^>]*>[\s\S]*?</script>");
            dto.Ten = rRemScript.Replace(dto.Ten, "").Trim();
            if (dto.MoTa != null && dto.MoTa != "")
            {
                dto.MoTa = rRemScript.Replace(dto.MoTa, "").Trim();
            }
            if (dto.Ma != null && dto.Ma != "")
            {
                dto.Ma = rRemScript.Replace(dto.Ma, "").Trim();
            }

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "DanhMuc_TinhThanh",
            new string[] { "@flag", "@Id", "@Ten", "@Ma", "@MoTa", "@TrangThai", "@CreatedBy", "@CreatedDate", "@ModifiedBy", "@ModifiedDate", "@Deleted" },
            new object[] { "SaveItem", dto.Id, dto.Ten, dto.Ma, dto.MoTa, dto.TrangThai, dto.CreatedBy, dto.CreatedDate, dto.ModifiedBy, dto.ModifiedDate, dto.Deleted });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }
        public static dynamic DeleteItem(DMTinhThanh dto)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "DanhMuc_TinhThanh",
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
