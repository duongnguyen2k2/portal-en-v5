using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using API.Areas.Admin.Models.DMPhuongXa;
using API.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace API.Areas.Admin.Models.DMPhuongXa
{
    public class DMPhuongXaService
    {
        public static List<DMPhuongXa> GetListPagination(SearchDMPhuongXa dto, string SecretId)
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
            var tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "danhmuc_phuongxa",
                new string[] { "@flag", "@CurrentPage", "@ItemsPerPage", "@Keyword", "@IdQuanHuyen", "@IdTinhThanh" },
                new object[] { "GetListPagination", dto.CurrentPage, dto.ItemsPerPage, dto.Keyword, dto.IdQuanHuyen,dto.IdTinhThanh });
            if (tabl == null)
            {
                return new List<DMPhuongXa>();
            }
            else
            {
                return (from r in tabl.AsEnumerable()
                        select new DMPhuongXa
                        {
                            Id = (int)r["Id"],
                            Ten = (string)r["Ten"],
                            Ma = (string)((r["Ma"] == System.DBNull.Value) ? "" : r["Ma"]),
                            TrangThai = (bool)r["TrangThai"],
                            MoTa = (string)((r["MoTa"] == System.DBNull.Value) ? "" : r["MoTa"]),
                            MaQuanHuyen = (string)((r["MaQuanHuyen"] == System.DBNull.Value) ? "" : r["MaQuanHuyen"]),
                            TenQuanHuyen = (string)((r["TenQuanHuyen"] == System.DBNull.Value) ? "" : r["TenQuanHuyen"]),
                            TenTinhThanh = (string)((r["TenTinhThanh"] == System.DBNull.Value) ? "" : r["TenTinhThanh"]),
                            IdQuanHuyen = (int)r["IdQuanHuyen"],
                            Ids = MyModels.Encode((int)r["Id"], SecretId),
                            TotalRows = (int)r["TotalRows"],
                        }).ToList();
            }


        }

        public static List<DMPhuongXa> GetListByQuanHuyen(int IdQuanHuyen = 0)
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "danhmuc_phuongxa",
               new string[] { "@flag", "@IdQuanHuyen" }, new object[] { "GetListByQuanHuyen", IdQuanHuyen });

            if (tabl == null)
            {
                return new List<DMPhuongXa>();
            }
            else
            {
                return (from r in tabl.AsEnumerable()
                        select new DMPhuongXa
                        {
                            Id = (int)r["Id"],
                            Ten = (string)r["Ten"],
                            Ma = (string)((r["Ma"] == System.DBNull.Value) ? "" : r["Ma"]),

                            IdQuanHuyen = (int)r["IdQuanHuyen"],

                        }).ToList();
            }



        }

        public static List<SelectListItem> GetListSelectItems(int IdQuanHuyen = 0)
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "danhmuc_phuongxa",
               new string[] { "@flag", "@Selected", "@IdQuanHuyen" }, new object[] { "GetListByQuanHuyen", 1, IdQuanHuyen });
            List<SelectListItem> ListItems = (from r in tabl.AsEnumerable()
                                              select new SelectListItem
                                              {
                                                  Value = (string)((r["Id"] == System.DBNull.Value) ? null : r["Id"].ToString()),
                                                  Text = (string)((r["Ten"] == System.DBNull.Value) ? null : r["Ten"]),
                                              }).ToList();

            ListItems.Insert(0, (new SelectListItem { Text = "--- Chọn Phường Xã ---", Value = "0" }));
            return ListItems;

        }

        public static List<DMPhuongXa> GetList(Boolean CHON = true)
        {
            decimal chon = Convert.ToDecimal(CHON);
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "danhmuc_phuongxa",
                new string[] { "@flag", "@Selected" }, new object[] { "GetList", chon });
                return (from r in tabl.AsEnumerable()
                    select new DMPhuongXa
                    {
                        Id = (int)r["Id"],
                        Ten = (string)r["Ten"],
                        Ma = (string)((r["Ma"] == System.DBNull.Value) ? "" : r["Ma"]),
                        TrangThai = (bool)r["TrangThai"],
                        MoTa = (string)((r["MoTa"] == System.DBNull.Value) ? "" : r["MoTa"]),
                        MaQuanHuyen = (string)((r["MaQuanHuyen"] == System.DBNull.Value) ? "" : r["MaQuanHuyen"]),
                        IdQuanHuyen = (int)r["IdQuanHuyen"],                                                
                    }).ToList();

        }

        public static DMPhuongXa GetItem(decimal Id, string SecretId = null)
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "danhmuc_phuongxa",
            new string[] { "@flag", "@Id" }, new object[] { "GetItem", Id });
            return (from r in tabl.AsEnumerable()
                    select new DMPhuongXa
                    {
                        Id = (int)r["Id"],
                        Ten = (string)r["Ten"],
                        TrangThai = (bool)r["TrangThai"],
                        MoTa = (string)((r["MoTa"] == System.DBNull.Value) ? "" : r["MoTa"]),
                        Ma = (string)((r["Ma"] == System.DBNull.Value) ? "" : r["Ma"]),
                        MaQuanHuyen = (string)((r["MaQuanHuyen"] == System.DBNull.Value) ? "" : r["MaQuanHuyen"]),
                        IdQuanHuyen = (int)r["IdQuanHuyen"],
                        Ids = MyModels.Encode((int)r["Id"], SecretId),
                    }).FirstOrDefault();
        }

        public static dynamic SaveItem(DMPhuongXa dto)
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
           
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "danhmuc_phuongxa",
            new string[] { "@flag", "@Id", "@Ten", "@TrangThai", "@MoTa", "@Ma" },
            new object[] { "SaveItem", dto.Id, dto.Ten, dto.TrangThai, dto.MoTa, dto.Ma });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (decimal)(r["N"]),
                    }).FirstOrDefault();

        }

        public static dynamic DeleteItem(DMPhuongXa dto)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "danhmuc_phuongxa",
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
