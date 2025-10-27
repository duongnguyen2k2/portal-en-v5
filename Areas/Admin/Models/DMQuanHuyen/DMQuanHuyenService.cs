using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using API.Areas.Admin.Models.DMQuanHuyen;
using API.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace API.Areas.Admin.Models.DMQuanHuyen
{
    public class DMQuanHuyenService
    {
        public static List<DMQuanHuyen> GetListPagination(SearchDMQuanHuyen dto, string SecretId)
        {
            if (dto.CurrentPage <= 0)
            {
                dto.CurrentPage = 1;
            }
            if (dto.ItemsPerPage <= 0)
            {
                dto.ItemsPerPage = 10;
            }
            if (dto.Keyword == null) {
                dto.Keyword = "";
            }
            var tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "danhmuc_quanhuyen",
                new string[] { "@flag", "@CurrentPage", "@ItemsPerPage", "@Keyword","@IdTinhThanh" },
                new object[] { "GetListPagination", dto.CurrentPage, dto.ItemsPerPage, dto.Keyword,dto.IdTinhThanh });
            if (tabl == null)
            {
                return new List<DMQuanHuyen>();
            }
            else
            {
                return (from r in tabl.AsEnumerable()
                        select new DMQuanHuyen
                        {
                            Id = (int)r["Id"],
                            Ten = (string)r["Ten"],
                            Ma = (string)((r["Ma"] == System.DBNull.Value) ? "" : r["Ma"]),
                            TrangThai = (bool)r["TrangThai"],
                            MoTa = (string)((r["MoTa"] == System.DBNull.Value) ? "" : r["MoTa"]),
                            MaTinhThanh = (string)((r["MaTinhThanh"] == System.DBNull.Value) ? "" : r["MaTinhThanh"]),
                            TenTinhThanh = (string)((r["TenTinhThanh"] == System.DBNull.Value) ? "" : r["TenTinhThanh"]),
                            IdTinhThanh = (int)r["IdTinhThanh"],
                            Ids = MyModels.Encode((int)r["Id"], SecretId),
                            TotalRows = (int)r["TotalRows"],
                        }).ToList();
            }


        }

        public static List<DMQuanHuyen> GetList(Boolean CHON = true)
        {
            decimal chon = Convert.ToDecimal(CHON);
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "danhmuc_quanhuyen",
                new string[] { "@flag", "@Selected"}, new object[] { "GetList", chon });
            return (from r in tabl.AsEnumerable()
                    select new DMQuanHuyen
                    {
                        Id = (int)r["Id"],
                        Ten = (string)r["Ten"],
                        Ma = (string)((r["Ma"] == System.DBNull.Value) ? "" : r["Ma"]),
                        TrangThai = (bool)r["TrangThai"],
                        MoTa = (string)((r["MoTa"] == System.DBNull.Value) ? "" : r["MoTa"]),                       
                        IdTinhThanh = (int)r["IdTinhThanh"]                       
                    }).ToList();

        }

        public static List<DMQuanHuyen> GetListByTinhThanh(Boolean CHON = true, int IdTinhThanh = 0)
        {
            decimal chon = Convert.ToDecimal(CHON);
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "danhmuc_quanhuyen",
                new string[] { "@flag", "@Selected", "@IdTinhThanh" }, new object[] { "GetListByTinhThanh", chon, IdTinhThanh });
            return (from r in tabl.AsEnumerable()
                    select new DMQuanHuyen
                    {
                        Id = (int)r["Id"],
                        Ten = (string)r["Ten"],
                        Ma = (string)((r["Ma"] == System.DBNull.Value) ? "" : r["Ma"]),                        
                        MoTa = (string)((r["MoTa"] == System.DBNull.Value) ? "" : r["MoTa"]),
                        MaTinhThanh = (string)((r["MaTinhThanh"] == System.DBNull.Value) ? "" : r["MaTinhThanh"]),
                        TenTinhThanh = (string)((r["TenTinhThanh"] == System.DBNull.Value) ? "" : r["TenTinhThanh"]),
                        IdTinhThanh = (int)r["IdTinhThanh"]
                    }).ToList();

        }

        public static List<SelectListItem> GetListSelectItems(int IdTinhThanh=0)
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "danhmuc_quanhuyen",
               new string[] { "@flag", "@Selected", "@IdTinhThanh" }, new object[] { "GetListByTinhThanh", 1, IdTinhThanh });
            List<SelectListItem> ListItems = (from r in tabl.AsEnumerable()
                                              select new SelectListItem
                                              {
                                                  Value = (string)((r["Id"] == System.DBNull.Value) ? null : r["Id"].ToString()),
                                                  Text = (string)((r["Ten"] == System.DBNull.Value) ? null : r["Ten"]),
                                              }).ToList();

            ListItems.Insert(0, (new SelectListItem { Text = "--- Chọn Quận Huyện ---", Value = "0" }));
            return ListItems;

        }
        public static DMQuanHuyen GetItem(decimal Id, string SecretId = null)
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "danhmuc_quanhuyen",
            new string[] { "@flag", "@Id" }, new object[] { "GetItem", Id });
            return (from r in tabl.AsEnumerable()
                    select new DMQuanHuyen
                    {
                        Id = (int)r["Id"],
                        Ten = (string)r["Ten"],
                        TrangThai = (bool)r["TrangThai"],
                        MoTa = (string)((r["MoTa"] == System.DBNull.Value) ? "" : r["MoTa"]),
                        Ma = (string)((r["Ma"] == System.DBNull.Value) ? "" : r["Ma"]),
                        MaTinhThanh = (string)((r["MaTinhThanh"] == System.DBNull.Value) ? "" : r["MaTinhThanh"]),
                        IdTinhThanh = (int)r["IdTinhThanh"],
                        Ids = MyModels.Encode((int)r["Id"], SecretId),
                    }).FirstOrDefault();
        }

        public static dynamic SaveItem(DMQuanHuyen dto)
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

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "danhmuc_quanhuyen",
            new string[] { "@flag", "@Id", "@Ten", "@TrangThai", "@MoTa", "@Ma" },
            new object[] { "SaveItem", dto.Id, dto.Ten, dto.TrangThai, dto.MoTa, dto.Ma });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }
        public static dynamic DeleteItem(DMQuanHuyen dto)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "danhmuc_quanhuyen",
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
