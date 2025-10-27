using API.Models;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;

namespace API.Areas.Admin.Models.SangKienKinhNghiem
{
    public class SangKienKinhNghiemService
    {
        public static List<SangKienKinhNghiem> GetListPagination(SearchSangKienKinhNghiem dto, string SecretId, string Culture = "all")
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

            string StartDate = DateTime.ParseExact(dto.ShowStartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyyMMdd");
            string EndDate = DateTime.ParseExact(dto.ShowEndDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyyMMdd");
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

            var tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_SangKienKinhNghiem",
                new string[] { "@flag", "@Id", "@CurrentPage", "@ItemsPerPage", "@Keyword", "@CapQuanLy", "@LinhVuc", "@IdCoQuan", "@StartDate", "@EndDate", "@CreatedBy", "@Status" },
                new object[] { str_sql, dto.Id, dto.CurrentPage, dto.ItemsPerPage, dto.Keyword, dto.CapQuanLy, dto.LinhVuc, dto.IdCoQuan, StartDate, EndDate, dto.CreatedBy, Status });
            if (tabl == null)
            {
                return new List<SangKienKinhNghiem>();
            }
            else
            {
                try
                {
                    
                    return (from r in tabl.AsEnumerable()
                            select new SangKienKinhNghiem
                            {
                                Id = (int)r["Id"],
                                MaSo = (string)((r["MaSo"] == System.DBNull.Value) ? null : r["MaSo"]),
                                Ten = (string)((r["Ten"] == System.DBNull.Value) ? null : r["Ten"]),
                                CapQuanLy = (int)((r["CapQuanLy"] == System.DBNull.Value) ? 0 : r["CapQuanLy"]),
                                IdCoQuan = (int)((r["IdCoQuan"] == System.DBNull.Value) ? 0 : r["IdCoQuan"]),
                                LinhVuc = (int)((r["LinhVuc"] == System.DBNull.Value) ? 0 : r["LinhVuc"]),
                                DonViChuTri = (string)((r["DonViChuTri"] == System.DBNull.Value) ? null : r["DonViChuTri"]),
                                Status = (Boolean)((r["Status"] == System.DBNull.Value) ? false : r["Status"]),
                                KetQua = (string)((r["KetQua"] == System.DBNull.Value) ? null : r["KetQua"]),
                                Ids = MyModels.Encode((int)r["Id"], SecretId),
                                TotalRows = (int)r["TotalRows"],
                                CreatedBy = (int)((r["CreatedBy"] == System.DBNull.Value) ? 0 : r["CreatedBy"]),
                                TacGia = (string)((r["TacGia"] == System.DBNull.Value) ? null : r["TacGia"]),
                                TenCapQuanLy = (string)((r["TenCapQuanLy"] == System.DBNull.Value) ? null : r["TenCapQuanLy"]),
                                TenLinhVuc = (string)((r["TenLinhVuc"] == System.DBNull.Value) ? null : r["TenLinhVuc"]),
                                Image = (string)((r["Image"] == System.DBNull.Value) ? null : r["Image"]),
                            }).ToList();


                }
                catch (Exception e)
                {
                    return new List<SangKienKinhNghiem>();
                }
            }
        }

        public static List<SelectListItem> GetListItemsStatus()
        {
            List<SelectListItem> ListItems = new List<SelectListItem>();
            ListItems.Insert(0, (new SelectListItem { Text = "--- Trạng Thái ---", Value = "-1" }));
            ListItems.Insert(1, (new SelectListItem { Text = "Chưa Duyệt", Value = "0" }));
            ListItems.Insert(2, (new SelectListItem { Text = "Đã Duyệt", Value = "1" }));
            return ListItems;
        }

        public static List<SelectListItem> GetListItemsLinhVuc()
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_DocumentsField",
                new string[] { "@flag", "@Selected" }, new object[] { "GetList", true });
            List<SelectListItem> ListItems = (from r in tabl.AsEnumerable()
                                              select new SelectListItem
                                              {
                                                  Value = (string)((r["Id"] == System.DBNull.Value) ? null : r["Id"].ToString()),
                                                  Text = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
                                              }).ToList();

            ListItems.Insert(0, (new SelectListItem { Text = "--- Chọn Lĩnh Vực ---", Value = "0" }));
            return ListItems;
        }

        public static List<SelectListItem> GetListItemsCapQuanLy()
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_DocumentsLevel",
                new string[] { "@flag", "@Selected" }, new object[] { "GetList", true });
            List<SelectListItem> ListItems = (from r in tabl.AsEnumerable()
                                              select new SelectListItem
                                              {
                                                  Value = (string)((r["Id"] == System.DBNull.Value) ? null : r["Id"].ToString()),
                                                  Text = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
                                              }).ToList();

            ListItems.Insert(0, (new SelectListItem { Text = "--- Cấp quản lý ---", Value = "0" }));
            return ListItems;
        }

        public static List<SangKienKinhNghiem> GetList(Boolean Selected = true)
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_SangKienKinhNghiem",
                new string[] { "@flag", "@Selected" }, new object[] { "GetList", Convert.ToDecimal(Selected) });
            if (tabl == null)
            {
                return new List<SangKienKinhNghiem>();
            }
            else
            {
                return (from r in tabl.AsEnumerable()
                        select new SangKienKinhNghiem
                        {
                            Id = (int)r["Id"],
                            MaSo = (string)((r["MaSo"] == System.DBNull.Value) ? null : r["MaSo"]),
                            Ten = (string)((r["Ten"] == System.DBNull.Value) ? null : r["Ten"]),
                            CapQuanLy = (int)((r["CapQuanLy"] == System.DBNull.Value) ? 0 : r["CapQuanLy"]),
                            IdCoQuan = (int)((r["IdCoQuan"] == System.DBNull.Value) ? 0 : r["IdCoQuan"]),
                            LinhVuc = (int)((r["LinhVuc"] == System.DBNull.Value) ? 0 : r["LinhVuc"]),
                            DonViChuTri = (string)((r["DonViChuTri"] == System.DBNull.Value) ? null : r["DonViChuTri"]),
                            Status = (Boolean)((r["Status"] == System.DBNull.Value) ? false : r["Status"]),
                            KetQua = (string)((r["KetQua"] == System.DBNull.Value) ? null : r["KetQua"]),
                            CreatedBy = (int)((r["CreatedBy"] == System.DBNull.Value) ? 0 : r["CreatedBy"]),
                            TacGia = (string)((r["TacGia"] == System.DBNull.Value) ? null : r["TacGia"]),
                            TenCapQuanLy = (string)((r["TenCapQuanLy"] == System.DBNull.Value) ? null : r["TenCapQuanLy"]),
                            TenLinhVuc = (string)((r["TenLinhVuc"] == System.DBNull.Value) ? null : r["TenLinhVuc"]),
                            Image = (string)((r["Image"] == System.DBNull.Value) ? null : r["Image"]),
                        }).ToList();
            }

        }

        public static SangKienKinhNghiem GetItem(decimal Id, string SecretId = null, string Culture = "all")
        {
            Culture = API.Models.MyHelper.StringHelper.ConverLan(Culture);
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_SangKienKinhNghiem",
            new string[] { "@flag", "@Id" }, new object[] { "GetItem", Id });
            SangKienKinhNghiem Item = (from r in tabl.AsEnumerable()
                                       select new SangKienKinhNghiem
                                       {
                                           Id = (int)r["Id"],
                                           MaSo = (string)((r["MaSo"] == System.DBNull.Value) ? null : r["MaSo"]),
                                           Ten = (string)((r["Ten"] == System.DBNull.Value) ? null : r["Ten"]),
                                           CapQuanLy = (int)((r["CapQuanLy"] == System.DBNull.Value) ? 0 : r["CapQuanLy"]),
                                           IdCoQuan = (int)((r["IdCoQuan"] == System.DBNull.Value) ? 0 : r["IdCoQuan"]),
                                           LinhVuc = (int)((r["LinhVuc"] == System.DBNull.Value) ? 0 : r["LinhVuc"]),
                                           DonViChuTri = (string)((r["DonViChuTri"] == System.DBNull.Value) ? null : r["DonViChuTri"]),
                                           Status = (Boolean)((r["Status"] == System.DBNull.Value) ? false : r["Status"]),
                                           KetQua = (string)((r["KetQua"] == System.DBNull.Value) ? null : r["KetQua"]),
                                           TacGia = (string)((r["TacGia"] == System.DBNull.Value) ? null : r["TacGia"]),
                                           Ids = MyModels.Encode((int)r["Id"], SecretId),
                                           CreatedBy = (int)((r["CreatedBy"] == System.DBNull.Value) ? 0 : r["CreatedBy"]),
                                           Image = (string)((r["Image"] == System.DBNull.Value) ? null : r["Image"]),
                                       }).FirstOrDefault();
            return Item;
        }

        public static dynamic SaveItem(SangKienKinhNghiem dto)
        {
            dto.CreatedAt = DateTime.Now;
            DateTime ShowPublish = DateTime.ParseExact(dto.CreatedAt.ToString("dd/MM/yyyy"), "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_SangKienKinhNghiem",
            new string[] { "@flag", "@Id", "@Ten", "@MaSo", "@CapQuanLy","@DonViChuTri", "@IdCoQuan", "@LinhVuc", "@Status", "@CreatedBy", "@UpdatedBy", "@CreatedAt", "@UpdatedAt", "@KetQua", "@ShowPublish" , "@ThoiGianThucHienTu", "@ThoiGianThucHienDen", "@TacGia" ,"@Image"},
            new object[] { "SaveItem", dto.Id, dto.Ten, dto.MaSo, dto.CapQuanLy, dto.DonViChuTri, dto.IdCoQuan, dto.LinhVuc, dto.Status, dto.CreatedBy, dto.UpdatedBy, dto.CreatedAt, dto.UpdatedAt, dto.KetQua, ShowPublish, dto.ThoiGianThucHienTu, dto.ThoiGianThucHienDen, dto.TacGia , dto.Image});
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }
        public static dynamic DeleteItem(SangKienKinhNghiem dto)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_SangKienKinhNghiem",
            new string[] { "@flag", "@Id", "@UpdatedBy" },
            new object[] { "DeleteItem", dto.Id, dto.UpdatedBy });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }

        public static dynamic UpdateStatus(SangKienKinhNghiem dto)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_SangKienKinhNghiem",
            new string[] { "@flag", "@Id", "@Status", "@UpdatedBy" },
            new object[] { "UpdateStatus", dto.Id, dto.Status, dto.UpdatedBy });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }
    }
}
