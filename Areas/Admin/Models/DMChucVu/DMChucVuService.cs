using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using API.Areas.Admin.Models.DMChucVu;
using API.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace API.Areas.Admin.Models.DMChucVu
{
    public class DMChucVuService
    {
        public static async Task<dynamic> GhiLog(DateTime time_start, DateTime time_end, string chuoi)
        {

            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "temp");
            string filePath = System.IO.Path.Combine(path, "logerror.txt");

            decimal start = Convert.ToDecimal(string.Format("{0:ddMMyyHHmmss}", time_start));
            decimal end = Convert.ToDecimal(string.Format("{0:ddMMyyHHmmss}", time_end));
            chuoi = chuoi + "-" + time_start.ToString("yyyyMMdd:HH:mm:ss") + "-" + time_end.ToString("yyyyMMdd:HH:mm:ss") + "-" + (end - start).ToString();
            if (System.IO.File.Exists(filePath))
            {
                StreamWriter sWriter = new StreamWriter(filePath, true);//fs là 1 FileStream 
                sWriter.WriteLine(chuoi);
                sWriter.Flush();
                sWriter.Close();
                return true;
            }
            else
            {
                FileStream fs = new FileStream(filePath, FileMode.Create);//Tạo file mới tên là test.txt            
                StreamWriter sWriter = new StreamWriter(fs, Encoding.UTF8);//fs là 1 FileStream 
                sWriter.WriteLine(chuoi);
                sWriter.Flush();
                fs.Close();
                return true;
            }
        }

        public static List<DMChucVu> GetListPagination(SearchDMChucVu dto, string SecretId)
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
            var tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "DanhMuc_ChucVu",
                new string[] { "@flag", "@CurrentPage", "@ItemsPerPage", "@Keyword", "@IdCoQuan" },
                new object[] { "GetListPagination", dto.CurrentPage, dto.ItemsPerPage, dto.Keyword, dto.IdCoQuan });
            if (tabl == null)
            {
                return new List<DMChucVu>();
            }
            else
            {
                return (from r in tabl.AsEnumerable()
                        select new DMChucVu
                        {
                            Id = (int)r["Id"],
                            Title = (string)r["Title"],
                            Status = (bool)r["Status"],
                            IdCoQuan = (int)((r["IdCoQuan"] == System.DBNull.Value) ? 0 : r["IdCoQuan"]),
                            Description = (string)((r["Description"] == System.DBNull.Value) ? "" : r["Description"]),
                            Leader = (bool)r["Leader"],
                            Ids = MyModels.Encode((int)r["Id"], SecretId),
                            TotalRows = (int)r["TotalRows"],
                        }).ToList();
            }


        }

        public static DataTable GetList(Boolean Selected = true, int IdCoQuan = 0)
        {
            
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "DanhMuc_ChucVu",
                new string[] { "@flag", "@Selected", "@IdCoQuan" }, new object[] { "GetList", Selected , IdCoQuan });
            return tabl;

        }

        public static List<SelectListItem> GetListSelectItems(Boolean Selected = true, int IdCoQuan=0)
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "DanhMuc_ChucVu",
                new string[] { "@flag", "@Selected", "@IdCoQuan" }, new object[] { "GetList", Convert.ToDecimal(Selected), IdCoQuan });
            List<SelectListItem> ListItems = (from r in tabl.AsEnumerable()
                                              select new SelectListItem
                                              {
                                                  Value = (string)((r["Id"] == System.DBNull.Value) ? null : r["Id"].ToString()),
                                                  Text = (string)((r["Title"] == System.DBNull.Value) ? "" : r["Title"]),
                                              }).ToList();
            ListItems.Insert(0, (new SelectListItem { Text = "--- Chọn Chức Vụ ---", Value = "0" }));
            return ListItems;

        }

        public static DMChucVu GetItem(int Id, int IdCoQuan, string SecretId = null)
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "DanhMuc_ChucVu",
            new string[] { "@flag", "@Id", "@IdCoQuan" }, new object[] { "GetItem", Id, IdCoQuan });
            return (from r in tabl.AsEnumerable()
                    select new DMChucVu
                    {
                        Id = (int)r["Id"],
                        Title = (string)r["Title"],
                        Status = (bool)r["Status"],
                        Description = (string)((r["Description"] == System.DBNull.Value) ? "" : r["Description"]),
                        IdCoQuan = (int)((r["IdCoQuan"] == System.DBNull.Value) ? 0 : r["IdCoQuan"]),
                        Leader = (bool)r["Leader"],
                        Ids = MyModels.Encode((int)r["Id"], SecretId),
                    }).FirstOrDefault();
        }

        public static dynamic SaveItem(DMChucVu dto)
        {
            dto.Title = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.Title);
            dto.Description = API.Models.MyHelper.StringHelper.RemoveTagsIntroText(dto.Description);

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "DanhMuc_ChucVu",
            new string[] { "@flag","@Id", "@Title", "@Status", "@Description", "@Leader", "@CreatedBy", "@ModifiedBy", "@IdCoQuan" },
            new object[] { "SaveItem", dto.Id, dto.Title, dto.Status, dto.Description, dto.Leader,dto.CreatedBy,dto.ModifiedBy,dto.IdCoQuan });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }

        public static dynamic UpdateStatus(DMChucVu dto)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "DanhMuc_ChucVu",
            new string[] { "@flag", "@Id","@Status", "@ModifiedBy", "@IdCoQuan" },
            new object[] { "UpdateStatus", dto.Id,dto.Status, dto.ModifiedBy ,dto.IdCoQuan});
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }

        public static dynamic DeleteItem(DMChucVu dto)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "DanhMuc_ChucVu",
            new string[] { "@flag", "@Id", "@ModifiedBy", "@IdCoQuan" },
            new object[] { "DeleteItem", dto.Id, dto.ModifiedBy,dto.IdCoQuan});
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }


    }
}
