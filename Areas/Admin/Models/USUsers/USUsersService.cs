using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Areas.Admin.Models.USUsers;
using API.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Globalization;
//using API.Areas.Admin.Models.DMPhongBan;
//using DocumentFormat.OpenXml.Drawing;

namespace API.Areas.Admin.Models.USUsers
{
    public class USUsersService
    {
        public const string SecretXSRF = "7f71f3acb09cf48e22b99c0b694c238ddschangkho"; // your symmetric
        //public const string SecretPassword = "d9c44cd30dab934f5da6b3e60d296421"; // your symmetric
        public static List<USUsers> GetListPagination(SearchUSUsers dto, string SecretId)
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

            var tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "user_users",
                new string[] { "@flag", "@CurrentPage", "@ItemsPerPage", "@Keyword", "@IdCoQuan", "@IdGroup", "@Status" },
                new object[] { str_sql, dto.CurrentPage, dto.ItemsPerPage, dto.Keyword,dto.IdCoQuan,dto.IdGroup, Status });
            if (tabl == null)
            {
                return new List<USUsers>();
            }
            else
            {
                return (from r in tabl.AsEnumerable()
                        select new USUsers
                        {
                            Id = (int)r["Id"],
                            UserName = (string)r["UserName"],
                            Status = (byte)r["Status"],
                            Gender = (byte)r["Gender"],
                            Telephone = (string)((r["Telephone"] == System.DBNull.Value) ? "" : r["Telephone"]),
                            Specialize = (string)((r["Specialize"] == System.DBNull.Value) ? "" : r["Specialize"]),
                            FullName = (string)((r["FullName"] == System.DBNull.Value) ? "" : r["FullName"]),
                            GroupsTitle = (string)((r["GroupsTitle"] == System.DBNull.Value) ? "" : r["GroupsTitle"]),
                            Email = (string)((r["Email"] == System.DBNull.Value) ? "" : r["Email"]),
                            Ids = MyModels.Encode((int)r["Id"], SecretId),
                            TotalRows = (int)r["TotalRows"],
                            IdCoQuan = (int)r["IdCoQuan"],
                            IdRegency = (int)((r["IdRegency"] == System.DBNull.Value) ? 0 : r["IdRegency"]),                            
                            TenChucVu = (string)((r["TenChucVu"] == System.DBNull.Value) ? "" : r["TenChucVu"]),
                            TenCoQuan = (string)((r["TenCoQuan"] == System.DBNull.Value) ? "" : r["TenCoQuan"]),
                            Address = (string)((r["Address"] == System.DBNull.Value) ? "" : r["Address"]),
                            TenDanToc = (string)((r["TenDanToc"] == System.DBNull.Value) ? "" : r["TenDanToc"]),
                            TenTonGiao = (string)((r["TenTonGiao"] == System.DBNull.Value) ? "" : r["TenTonGiao"]),
                            TenTrinhDo = (string)((r["TenTrinhDo"] == System.DBNull.Value) ? "" : r["TenTrinhDo"]),
                            ShowDangVien = (string)((r["ShowDangVien"] == System.DBNull.Value) ? "" : r["ShowDangVien"]),
                            BirthdayShow = (string)((r["BirthdayShow"] == System.DBNull.Value) ? "" : r["BirthdayShow"]),
                            TitlePhongBan = (string)((r["TitlePhongBan"] == System.DBNull.Value) ? "" : r["TitlePhongBan"]),
                            IdPhongBan = (int)((r["IdPhongBan"] == System.DBNull.Value) ? 0 : r["IdPhongBan"]),
                            Avatar = (string)((r["Avatar"] == System.DBNull.Value) ? "" : r["Avatar"]),
                            NamSinh = (byte)r["NamSinh"],                            
                            DangVien = (int)((r["DangVien"] == System.DBNull.Value) ? 0 : r["DangVien"]),
                            Ordering = (int)((r["Ordering"] == System.DBNull.Value) ? 0 : r["Ordering"])
                        }).ToList();
            }


        }

        public static List<USUsers> GetAllListUsersByGroup(SearchUSUsers dto, string SecretId)
        {


            var tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "user_users",
                new string[] { "@flag", "@CurrentPage", "@ItemsPerPage", "@Keyword", "@IdCoQuan", "@IdGroup" },
                new object[] { "GetAllListUsersByGroup", dto.CurrentPage, dto.ItemsPerPage, dto.Keyword, dto.IdCoQuan,dto.IdGroup });
            if (tabl == null)
            {
                return new List<USUsers>();
            }
            else
            {
                return (from r in tabl.AsEnumerable()
                        select new USUsers
                        {
                            Id = (int)r["Id"],                           
                            Specialize = (string)((r["Specialize"] == System.DBNull.Value) ? "" : r["Specialize"]),
                            Telephone = (string)((r["Telephone"] == System.DBNull.Value) ? "" : r["Telephone"]),
                            Gender = (byte)r["Gender"],
                            Status = (byte)r["Status"],
                            FullName = (string)((r["FullName"] == System.DBNull.Value) ? "" : r["FullName"]),
                            GroupsTitle = (string)((r["GroupsTitle"] == System.DBNull.Value) ? "" : r["GroupsTitle"]),
                            Email = (string)((r["Email"] == System.DBNull.Value) ? "" : r["Email"]),
                            IdCoQuan = (int)r["IdCoQuan"],
                            IdRegency = (int)((r["IdRegency"] == System.DBNull.Value) ? 0 : r["IdRegency"]),
                            Ordering = (int)((r["Ordering"] == System.DBNull.Value) ? 0 : r["Ordering"]),
                            TenChucVu = (string)((r["TenChucVu"] == System.DBNull.Value) ? "" : r["TenChucVu"]),
                            TenCoQuan = (string)((r["TenCoQuan"] == System.DBNull.Value) ? "" : r["TenCoQuan"]),                           
                            Avatar = (string)((r["Avatar"] == System.DBNull.Value) ? "" : r["Avatar"]),
                            
                            /* TenDanToc = (string)((r["TenDanToc"] == System.DBNull.Value) ? "" : r["TenDanToc"]),
                            TenTonGiao = (string)((r["TenTonGiao"] == System.DBNull.Value) ? "" : r["TenTonGiao"]),
                            TenTrinhDo = (string)((r["TenTrinhDo"] == System.DBNull.Value) ? "" : r["TenTrinhDo"]),
                            ShowDangVien = (string)((r["ShowDangVien"] == System.DBNull.Value) ? "" : r["ShowDangVien"]),
                            BirthdayShow = (string)((r["BirthdayShow"] == System.DBNull.Value) ? "" : r["BirthdayShow"]),
                            NamSinh = (byte)r["NamSinh"],
                            DangVien = (int)((r["DangVien"] == System.DBNull.Value) ? 0 : r["DangVien"]),*/
                        }).ToList();
            }


        }


        public static List<USUsers> GetAllListUsersByCoQuan(SearchUSUsers dto, string SecretId)
        {
            

            var tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "user_users",
                new string[] { "@flag", "@CurrentPage", "@ItemsPerPage", "@Keyword", "@IdCoQuan" },
                new object[] { "GetAllListUsersByCoQuan", dto.CurrentPage, dto.ItemsPerPage, dto.Keyword, dto.IdCoQuan });
            if (tabl == null)
            {
                return new List<USUsers>();
            }
            else
            {
                return (from r in tabl.AsEnumerable()
                        select new USUsers
                        {
                            Id = (int)r["Id"],
                            //UserName = (string)r["UserName"],
                            
                            Specialize = (string)((r["Specialize"] == System.DBNull.Value) ? "" : r["Specialize"]),
                            Telephone = (string)((r["Telephone"] == System.DBNull.Value) ? "" : r["Telephone"]),
                            Gender = (byte)r["Gender"],
                            Status = (byte)r["Status"],                            
                            FullName = (string)((r["FullName"] == System.DBNull.Value) ? "" : r["FullName"]),
                            GroupsTitle = (string)((r["GroupsTitle"] == System.DBNull.Value) ? "" : r["GroupsTitle"]),
                            Email = (string)((r["Email"] == System.DBNull.Value) ? "" : r["Email"]),                                                        
                            IdCoQuan = (int)r["IdCoQuan"],
                            IdRegency = (int)((r["IdRegency"] == System.DBNull.Value) ? 0 : r["IdRegency"]),
                            Ordering = (int)((r["Ordering"] == System.DBNull.Value) ? 0 : r["Ordering"]),
                            TenChucVu = (string)((r["TenChucVu"] == System.DBNull.Value) ? "" : r["TenChucVu"]),
                            TenCoQuan = (string)((r["TenCoQuan"] == System.DBNull.Value) ? "" : r["TenCoQuan"]),
                            /*
                            Address = (string)((r["Address"] == System.DBNull.Value) ? "" : r["Address"]),
                            TenDanToc = (string)((r["TenDanToc"] == System.DBNull.Value) ? "" : r["TenDanToc"]),
                            TenTonGiao = (string)((r["TenTonGiao"] == System.DBNull.Value) ? "" : r["TenTonGiao"]),
                            TenTrinhDo = (string)((r["TenTrinhDo"] == System.DBNull.Value) ? "" : r["TenTrinhDo"]),
                            ShowDangVien = (string)((r["ShowDangVien"] == System.DBNull.Value) ? "" : r["ShowDangVien"]),
                            BirthdayShow = (string)((r["BirthdayShow"] == System.DBNull.Value) ? "" : r["BirthdayShow"]),
                            NamSinh = (byte)r["NamSinh"],
                            DangVien = (int)((r["DangVien"] == System.DBNull.Value) ? 0 : r["DangVien"]),*/
                        }).ToList();
            }


        }

        public static DataTable GetListByGroupPB(int IdGroup, int IdCoQuan, int IdPhongBan = 0)
        {

            var tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "user_users",
            new string[] { "@flag", "@IdGroup", "@IdPhongBan", "@IdCoQuan" },
                new object[] { "GetListByGroup", IdGroup, IdPhongBan, IdCoQuan });
            return tabl;
        }

        public static List<USUsers> GetListByGroup(int IdGroup, int IdCoQuan, int IdPhongBan=0)
        {

            var tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "user_users",
            new string[] { "@flag", "@IdGroup"},
                new object[] { "GetListByGroup", IdGroup });
            return (from r in tabl.AsEnumerable()
                    select new USUsers
                    {
                        Id = (int)r["Id"],
                        UserName = (string)r["UserName"],
                        Status = (byte)r["Status"],
                        Gender = (byte)r["Gender"],
                        Telephone = (string)((r["Telephone"] == System.DBNull.Value) ? "" : r["Telephone"]),
                        Specialize = (string)((r["Specialize"] == System.DBNull.Value) ? "" : r["Specialize"]),
                        FullName = (string)((r["FullName"] == System.DBNull.Value) ? "" : r["FullName"]),
                        GroupsTitle = (string)((r["GroupsTitle"] == System.DBNull.Value) ? "" : r["GroupsTitle"]),
                        Email = (string)((r["Email"] == System.DBNull.Value) ? "" : r["Email"]),
                        TotalRows = (int)r["TotalRows"],
                        IdCoQuan = (int)r["IdCoQuan"],
                        TenCoQuan = (string)((r["TenCoQuan"] == System.DBNull.Value) ? "" : r["TenCoQuan"]),
                        IdRegency = (int)((r["IdRegency"] == System.DBNull.Value) ? 0 : r["IdRegency"]),
                        TenChucVu = (string)((r["TenChucVu"] == System.DBNull.Value) ? "" : r["TenChucVu"]),
                        TitlePhongBan = (string)((r["TitlePhongBan"] == System.DBNull.Value) ? "" : r["TitlePhongBan"]),
                        IdPhongBan = (int)((r["IdPhongBan"] == System.DBNull.Value) ? 0 : r["IdPhongBan"]),
                        
                    }).ToList();
        }

         public static List<SelectListItem> GetListItemsAuthor(int IdGroup, int IdCoQuan)
        {

            var tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "user_users",
                new string[] { "@flag", "@IdGroup", "@IdCoQuan" },
                new object[] { "GetListByGroup", IdGroup, IdCoQuan });
            List<SelectListItem> ListItems = (from r in tabl.AsEnumerable()
                                            select new SelectListItem
                                            {
                                                Value = (string)((r["Id"] == System.DBNull.Value) ? null : r["Id"].ToString()),
                                                Text = (string)((r["FullName"] == System.DBNull.Value) ? "Không có tên" : r["FullName"]),
                                            }).ToList();
            ListItems.Insert(0, (new SelectListItem { Text = "Chọn Tác Giả", Value = "0" }));
            return ListItems;
        }


        public static List<USUsers> GetListByCoQuan(int IdCoQuan)
        {

            var tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "user_users",
                new string[] { "@flag", "@IdCoQuan" },
                new object[] { "GetListByCoQuan", IdCoQuan });
            return (from r in tabl.AsEnumerable()
                    select new USUsers
                    {
                        Id = (int)r["Id"],
                        UserName = (string)r["UserName"],
                        Gender = (byte)r["Gender"],
                        Telephone = (string)((r["Telephone"] == System.DBNull.Value) ? "" : r["Telephone"]),
                        Specialize = (string)((r["Specialize"] == System.DBNull.Value) ? "" : r["Specialize"]),
                        FullName = (string)((r["FullName"] == System.DBNull.Value) ? "" : r["FullName"]),
                        GroupsTitle = (string)((r["GroupsTitle"] == System.DBNull.Value) ? "" : r["GroupsTitle"]),
                        Email = (string)((r["Email"] == System.DBNull.Value) ? "" : r["Email"]),
                        IdCoQuan = (int)r["IdCoQuan"],
                        TenCoQuan = (string)((r["TenCoQuan"] == System.DBNull.Value) ? "" : r["TenCoQuan"]),
                        IdRegency = (int)((r["IdRegency"] == System.DBNull.Value) ? 0 : r["IdRegency"]),
                        TenChucVu = (string)((r["TenChucVu"] == System.DBNull.Value) ? "" : r["TenChucVu"]),
                    }).ToList();


        }


        public static UserToken BuildUserToken(dynamic item)
        {
            return new UserToken()
            {
                UserName = item.UserName,
                Email = item.Email,
                Id = item.Id,
                IdCoQuan = item.IdCoQuan,
                FullName = item.FullName,
                UserCode = item.UserCode,
                IdGroup = item.IdGroup,
                Avatar = item.Avatar,
                Telephone = item.Telephone,
                Birthday = item.Birthday,
                IdHuyen = item.IdHuyen,
                IdTinhThanh = item.IdTinhThanh,
                ManufacturerId = item.ManufacturerId
            };

        }


        public static dynamic ChangePassword(int Id, string Password)
        {
            try
            {
                DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "user_users",
                    new string[] { "@flag", "@Id", "@Password", "@ModifiedDate" },
                    new object[] { "ChangePassword", Id, Password, DateTime.Now });
                return new { N = Id };
            }
            catch
            {
                return new { N = 0 };
            }
        }

       
        public static List<SelectListItem> GetStatusSelectItems()
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "user_users",
               new string[] { "@flag" }, new object[] { "GetListTrangThai" });
            List<SelectListItem> ListItems = (from r in tabl.AsEnumerable()
                                              select new SelectListItem
                                              {
                                                  Value = (string)((r["Id"] == System.DBNull.Value) ? null : r["Id"].ToString()),
                                                  Text = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
                                              }).ToList();
            return ListItems;
        }

        public static List<SelectListItem> GetDangVienSelectItems()
        {
            List<SelectListItem> ListItems = new List<SelectListItem>();
            ListItems.Add(new SelectListItem() { Value = "0", Text = "Không" });
            ListItems.Add(new SelectListItem() { Value = "1", Text = "Đảng Viên" });
            return ListItems;
        }

        public static List<SelectListItem> GetYearSelectItems(Boolean flag = true)
        {
            List<SelectListItem> ListItems = new List<SelectListItem>();
            int Year = Int32.Parse(DateTime.Now.ToString("yyyy").ToString());
            if (flag) {
                ListItems.Add(new SelectListItem() { Value = "0", Text = "--- Chọn Năm ---" });
            }            
            for (int i = 2018; i <= Year; i++) {
                ListItems.Add(new SelectListItem() { Value = i.ToString(), Text = i.ToString() });
            }                        
            return ListItems;
        }

        public static List<SelectListItem> GetThamGiaHoiSelectItems()
        {
            List<SelectListItem> ListItems = new List<SelectListItem>();
            int Year = Int32.Parse(DateTime.Now.ToString("yyyy").ToString());
            ListItems.Add(new SelectListItem() { Value = "-1", Text = "--- Tất Cả ---" });
            ListItems.Add(new SelectListItem() { Value = "1", Text = "Tham Gia Hội" });
            ListItems.Add(new SelectListItem() { Value = "0", Text = "Rời Hội" });

            return ListItems;
        }



        public static USUsers GetItem(int Id, string SecretId = null)
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "user_users",
            new string[] { "@flag", "@Id" }, new object[] { "GetItem", Id });
            USUsers item = (from r in tabl.AsEnumerable()
                            select new USUsers
                            {
                                Id = (int)r["Id"],
                                IdGroup = (int)r["IdGroup"],
                                FullName = (string)((r["FullName"] == System.DBNull.Value) ? "" : r["FullName"]),
                                UserName = (string)((r["UserName"] == System.DBNull.Value) ? "" : r["UserName"]),
                                UserCode = (string)((r["UserCode"] == System.DBNull.Value) ? "" : r["UserCode"]),
                                Specialize = (string)((r["Specialize"] == System.DBNull.Value) ? "" : r["Specialize"]),
                                Avatar = (string)((r["Avatar"] == System.DBNull.Value) ? "" : r["Avatar"]),
                                Birthday = (int)((r["Birthday"] == System.DBNull.Value) ? Int32.Parse(DateTime.Now.ToString("yyyyMMdd")) : r["Birthday"]),
                                Status = (byte)r["Status"],
                                Gender = (byte)r["Gender"],
                                Email = (string)((r["Email"] == System.DBNull.Value) ? "" : r["Email"]),
                                Description = (string)((r["Description"] == System.DBNull.Value) ? "" : r["Description"]),
                                Telephone = (string)((r["Telephone"] == System.DBNull.Value) ? "" : r["Telephone"]),
                                IdCoQuan = (int)((r["IdCoQuan"] == System.DBNull.Value) ? 0 : r["IdCoQuan"]),
                                IdRegency = (int)((r["IdRegency"] == System.DBNull.Value) ? 0 : r["IdRegency"]),
                                IdTrinhDo = (int)((r["IdTrinhDo"] == System.DBNull.Value) ? 0 : r["IdTrinhDo"]),
                                IdTonGiao = (int)((r["IdTonGiao"] == System.DBNull.Value) ? 0 : r["IdTonGiao"]),
                                IdDanToc = (int)((r["IdDanToc"] == System.DBNull.Value) ? 0 : r["IdDanToc"]),
                                DangVien = (int)((r["DangVien"] == System.DBNull.Value) ? 0 : r["DangVien"]),
                                Address = (string)((r["Address"] == System.DBNull.Value) ? "" : r["Address"]),                                
                                IdPhongBan = (int)((r["IdPhongBan"] == System.DBNull.Value) ? 0 : r["IdPhongBan"]),
                                Ordering = (int)((r["Ordering"] == System.DBNull.Value) ? 0 : r["Ordering"]),
                                NamSinh = (byte)r["NamSinh"],                               
                                Ids = MyModels.Encode((int)r["Id"], SecretId)
                            }).FirstOrDefault();

            if (item.Birthday == 0)
            {
                item.BirthdayShow = DateTime.Now.ToString("dd/MM/yyyy");
            }
            else
            {
                item.BirthdayShow = DateTime.ParseExact(item.Birthday.ToString(), "yyyyMMdd", CultureInfo.InvariantCulture).ToString("dd/MM/yyyy");
            }



            return item;
        }

        public static USUsers GetItemBE(int Id, string SecretId = null)
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "user_users",
            new string[] { "@flag", "@Id" }, new object[] { "GetItemBE", Id });
            USUsers item = (from r in tabl.AsEnumerable()
                            select new USUsers
                            {
                                Id = (int)r["Id"],
                                IdGroup = (int)r["IdGroup"],
                                FullName = (string)((r["FullName"] == System.DBNull.Value) ? "" : r["FullName"]),
                                UserName = (string)((r["UserName"] == System.DBNull.Value) ? "" : r["UserName"]),
                                UserCode = (string)((r["UserCode"] == System.DBNull.Value) ? "" : r["UserCode"]),
                                Specialize = (string)((r["Specialize"] == System.DBNull.Value) ? "" : r["Specialize"]),
                                Avatar = (string)((r["Avatar"] == System.DBNull.Value) ? "" : r["Avatar"]),
                                Birthday = (int)((r["Birthday"] == System.DBNull.Value) ? Int32.Parse(DateTime.Now.ToString("yyyyMMdd")) : r["Birthday"]),
                                Status = (byte)r["Status"],
                                Gender = (byte)r["Gender"],
                                Email = (string)((r["Email"] == System.DBNull.Value) ? "" : r["Email"]),
                                Description = (string)((r["Description"] == System.DBNull.Value) ? "" : r["Description"]),
                                Telephone = (string)((r["Telephone"] == System.DBNull.Value) ? "" : r["Telephone"]),
                                IdCoQuan = (int)((r["IdCoQuan"] == System.DBNull.Value) ? 0 : r["IdCoQuan"]),
                                IdRegency = (int)((r["IdRegency"] == System.DBNull.Value) ? 0 : r["IdRegency"]),
                                IdTrinhDo = (int)((r["IdTrinhDo"] == System.DBNull.Value) ? 0 : r["IdTrinhDo"]),
                                IdTonGiao = (int)((r["IdTonGiao"] == System.DBNull.Value) ? 0 : r["IdTonGiao"]),
                                IdDanToc = (int)((r["IdDanToc"] == System.DBNull.Value) ? 0 : r["IdDanToc"]),
                                DangVien = (int)((r["DangVien"] == System.DBNull.Value) ? 0 : r["DangVien"]),
                                Address = (string)((r["Address"] == System.DBNull.Value) ? "" : r["Address"]),
                                IdPhongBan = (int)((r["IdPhongBan"] == System.DBNull.Value) ? 0 : r["IdPhongBan"]),
                                NamSinh = (byte)r["NamSinh"],
                              
                                ManufacturerAddress = (string)((r["ManufacturerAddress"] == System.DBNull.Value) ? "" : r["ManufacturerAddress"]),
                                Ids = MyModels.Encode((int)r["Id"], SecretId)
                            }).FirstOrDefault();

            if (item.Birthday == 0)
            {
                item.BirthdayShow = DateTime.Now.ToString("dd/MM/yyyy");
            }
            else
            {
                item.BirthdayShow = DateTime.ParseExact(item.Birthday.ToString(), "yyyyMMdd", CultureInfo.InvariantCulture).ToString("dd/MM/yyyy");
            }



            return item;
        }

        public static USUsers GetItemByGroup(int Id, int IdGroup)
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "user_users",
            new string[] { "@flag", "@Id", "@IdGroup" }, new object[] { "GetItemByGroup", Id, IdGroup });
            return (from r in tabl.AsEnumerable()
                    select new USUsers
                    {
                        Id = (int)r["Id"],
                        IdGroup = (int)r["IdGroup"],
                        FullName = (string)((r["FullName"] == System.DBNull.Value) ? "" : r["FullName"]),
                        UserName = (string)((r["UserName"] == System.DBNull.Value) ? "" : r["UserName"]),
                        UserCode = (string)((r["UserCode"] == System.DBNull.Value) ? "" : r["UserCode"]),
                        Specialize = (string)((r["Specialize"] == System.DBNull.Value) ? "" : r["Specialize"]),
                        TenChucVu = (string)((r["TenChucVu"] == System.DBNull.Value) ? "" : r["TenChucVu"]),
                        TenCoQuan = (string)((r["TenCoQuan"] == System.DBNull.Value) ? "" : r["TenCoQuan"]),
                        Avatar = (string)((r["Avatar"] == System.DBNull.Value) ? "" : r["Avatar"]),
                        Status = (byte)r["Status"],
                        Gender = (byte)r["Gender"],
                        Email = (string)((r["Email"] == System.DBNull.Value) ? "" : r["Email"]),
                        Description = (string)((r["Description"] == System.DBNull.Value) ? "" : r["Description"]),
                        Telephone = (string)((r["Telephone"] == System.DBNull.Value) ? "" : r["Telephone"]),
                        IdRegency = (int)((r["IdRegency"] == System.DBNull.Value) ? 0 : r["IdRegency"]),
                        IdCoQuan = (int)((r["IdCoQuan"] == System.DBNull.Value) ? 0 : r["IdCoQuan"]),
                        IdTrinhDo = (int)((r["IdTrinhDo"] == System.DBNull.Value) ? 0 : r["IdTrinhDo"]),
                        IdTonGiao = (int)((r["IdTonGiao"] == System.DBNull.Value) ? 0 : r["IdTonGiao"]),
                        IdDanToc = (int)((r["IdDanToc"] == System.DBNull.Value) ? 0 : r["IdDanToc"]),
                        DangVien = (int)((r["DangVien"] == System.DBNull.Value) ? 0 : r["DangVien"]),
                    }).FirstOrDefault();
        }




        public static USUsersLogin CheckLogin(string UserName)
        {
            //string md5pass = USUsersService.GetMD5(Password);
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "user_users",
                new string[] { "@flag", "@UserName" },
                new object[] { "Login", UserName.ToLower() });

            try
            {
                return (from r in tabl.AsEnumerable()
                        select new USUsersLogin
                        {
                            Id = (int)r["Id"],
                            IdGroup = (int)r["IdGroup"],
                            UserName = (string)r["UserName"],
                            Password = (string)r["Password"],
                            ListMenuId = (string)((r["ListMenuId"] == System.DBNull.Value) ? "" : r["ListMenuId"]),
                            ListCatId = (string)((r["ListCatId"] == System.DBNull.Value) ? "" : r["ListCatId"]),
                            Avatar = (string)((r["Avatar"] == System.DBNull.Value) ? "" : r["Avatar"]),
                            FullName = (string)((r["FullName"] == System.DBNull.Value) ? "" : r["FullName"]),
                            UserCode = (string)((r["UserCode"] == System.DBNull.Value) ? "" : r["UserCode"]),
                            Gender = (byte)r["Gender"],
                            IdCoQuan = (int)r["IdCoQuan"],
                            TenCoQuan = (string)((r["TenCoQuan"] == System.DBNull.Value) ? "" : r["TenCoQuan"]),
                            Email = (string)((r["Email"] == System.DBNull.Value) ? "" : r["Email"]),
                            Telephone = (string)((r["Telephone"] == System.DBNull.Value) ? "" : r["Telephone"]),
                            CompanyName = (string)((r["CompanyName"] == System.DBNull.Value) ? "" : r["CompanyName"]),
                        }).FirstOrDefault();
            }catch(Exception e)
            {
                return null;
            }
        }

        public static dynamic SaveItem(USUsers dto, USUsers MyInfo = null)
        {
            
            if (MyInfo != null && MyInfo.IdGroup!=1)
            {
                if (dto.IdGroup == 1)
                {
                    dto.IdGroup = MyInfo.IdGroup;
                }
            }

            

            dto.Specialize = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.Specialize);
            dto.UserCode = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.UserCode);
            dto.UserName = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.UserName);
            dto.FullName = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.FullName);
            dto.Telephone = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.Telephone);
            dto.Email = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.Email);
            dto.Address = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.Address);
            dto.Fax = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.Fax);
            dto.Description = API.Models.MyHelper.StringHelper.RemoveTagsFullText(dto.Description);      
            dto.Avatar = API.Models.MyHelper.StringHelper.RemoveTagsLink(dto.Avatar);      

            DateTime Birthday = DateTime.Now;
            
            if (dto.NamSinh == 0)
            {
                if (dto.BirthdayShow != null)
                {
                    Birthday = DateTime.ParseExact(dto.BirthdayShow, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
            }
            else
            {
                if (dto.Birthday == 0)
                {
                    dto.Birthday = Int32.Parse(DateTime.Now.ToString("yyyy"));
                }
            }

            
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "user_users",
            new string[] { "@flag", "@Id", "@UserName", "@IdCoQuan", "@Password", "@UserCode", "@FullName", "@Telephone", "@Email", "@Birthday", "@Fax", "@IdGroup", "@Description", "@Status", "@ModifiedBy", "@CreatedBy", "@Gender", "@Specialize", "@IdRegency", "@IdDanToc", "@IdTonGiao", "@IdTrinhDo", "@Address", "@DangVien", "@NamSinh", "@Ordering", "@Avatar", "@IdPhongBan", "@ManufacturerId" },
            new object[] { "SaveItem", dto.Id, dto.UserName, dto.IdCoQuan, dto.Password, dto.UserCode, dto.FullName, dto.Telephone, dto.Email, dto.Birthday, dto.Fax, dto.IdGroup, dto.Description, dto.Status, dto.ModifiedBy, dto.CreatedBy, dto.Gender, dto.Specialize, dto.IdRegency, dto.IdDanToc, dto.IdTonGiao, dto.IdTrinhDo, dto.Address, dto.DangVien, dto.NamSinh, dto.Ordering,dto.Avatar,dto.IdPhongBan, dto.ManufacturerId });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }

        public static dynamic SaveImportExcelAll(DataTable tbItem)
        {
            try
            {
                DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "user_users",
                new string[] { "@flag", "@TBL_US_USERS" },
                new object[] { "SaveImportExcelAll", tbItem });
                return tabl;
            }
            catch (Exception e)
            {
                return e.Message.ToString();
            }

        }

        public static dynamic DeleteItem(USUsers dto)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "user_users",
            new string[] { "@flag", "@Id", "@ModifiedBy" },
            new object[] { "DeleteItem", dto.Id, dto.ModifiedBy });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }

        public static dynamic SaveAccountInfo(USUsers dto)
        {
            dto.UserCode = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.UserCode);
            dto.UserName = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.UserName);
            dto.FullName = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.FullName);
            dto.Telephone = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.Telephone);
            dto.Email = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.Email);
            dto.Address = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.Address);
            dto.Fax = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.Fax);
            dto.Description = API.Models.MyHelper.StringHelper.RemoveTagsFullText(dto.Description);      


            DateTime Birthday = DateTime.Now;
            if (dto.BirthdayShow != null)
            {
                Birthday = DateTime.ParseExact(dto.BirthdayShow, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
            dto.Birthday = Int32.Parse(Birthday.ToString("yyyyMMdd"));
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "user_users",
            new string[] { "@flag", "@Id", "@FullName", "@Status", "@Telephone", "@Email", "@Description", "@Gender", "@Password", "@Birthday", "@UserCode", "@Specialize", "@IdRegency", "@IdDanToc", "@IdTonGiao", "@IdTrinhDo", "@Address", "@Avatar" },
            new object[] { "SaveAccountInfo", dto.Id, dto.FullName, dto.Status, dto.Telephone, dto.Email, dto.Description, dto.Gender, dto.Password, dto.Birthday, dto.UserCode, dto.Specialize, dto.IdRegency, dto.IdDanToc, dto.IdTonGiao, dto.IdTrinhDo, dto.Address,dto.Avatar });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }

        public static dynamic SaveAccountAvatar(USUsers dto)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "user_users",
            new string[] { "@flag", "@Id", "@Avatar" },
            new object[] { "SaveAccountAvatar", dto.Id, dto.Avatar });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }

        public static string GetMD5(string str)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] bHash = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
            StringBuilder sbHash = new StringBuilder();
            foreach (byte b in bHash)
            {
                sbHash.Append(String.Format("{0:x2}", b));
            }
            return sbHash.ToString();
        }

        public static XSRF CreateXSRF(string SecurityDateTime = null, string Token = null)
        {
            XSRF item = new XSRF();
            if (SecurityDateTime == null || SecurityDateTime == "")
            {
                item.DateTime = string.Format("{0:ddMMyyHHmmss}", DateTime.Now);
            }
            string Secret = USUsersService.SecretXSRF + item.DateTime + Token;
            item.X_XSRF_TOKEN = USUsersService.GetMD5(Secret);
            return item;
        }

        public static Boolean CheckXSRF(XSRF item)
        {
            Boolean flag = false;
            try
            {
                string Secret = USUsersService.SecretXSRF + item.DateTime + item.Token;
                string X_XSRF_TOKEN = USUsersService.GetMD5(Secret);
                if (X_XSRF_TOKEN == item.X_XSRF_TOKEN)
                { flag = true; }
                return flag;
            }
            catch
            {
                return false;
            }

        }

        public static Boolean ValidateStrongPassword(string password)
        {
            Boolean upp = false;
            Boolean low = false;
            Boolean num = false;
            Boolean spc = false;
            Boolean cnt = false;
            Boolean noRpt = true;//Assume three repeating characters not found            
            Boolean rng = true;// Assume characters are within acceptable range
            if (password.Length > 7)
            {
                cnt = true;
                int pos = 0;
                for (int i = 0; i < password.Length; i++)
                {
                    foreach (char eachChar in "ABCDEFGHIJKLMNOPQRSTUVWXYZ")
                    {
                        if (password[i] == eachChar) { upp = true; }
                    }
                    foreach (char eachChar in "abcdefghijklmnopqrstuvwxyz")
                    {
                        if (password[i] == eachChar) { low = true; }
                    }
                    foreach (char eachChar in "0123456789")
                    {
                        if (password[i] == eachChar) { num = true; }
                    }
                    foreach (char eachChar in "!@#$%^&*()_-=+/\';><,.>")
                    {
                        if (password[i] == eachChar) { spc = true; }
                    }

                    if (pos < password.Length - 2)
                    {
                        if (password[i] == password[i + 1] && password[i] == password[i + 2])
                        {
                            noRpt = false;
                        }
                        else
                        {
                            pos++;
                        }
                    }

                }
            }
            if (cnt == true && upp == true && low == true && num == true && spc == true && noRpt == true && rng == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public static string GenerateToken(UserToken UserToken, int expireMinutes = 20)
        {
            Models.ManagerFiles.Settings setting = Models.ManagerFiles.ManagerFilesService.GetSettingsInfo();
            string Secret = setting.Secret;
            var symmetricKey = Convert.FromBase64String(Secret);
            var tokenHandler = new JwtSecurityTokenHandler();

            var now = DateTime.UtcNow;
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                    {
                        new Claim("Id", UserToken.Id.ToString()),
                        new Claim("Email", UserToken.Email),
                        new Claim("UserName", UserToken.UserName),
                        new Claim("IdCoQuan", UserToken.IdCoQuan.ToString()),
                        new Claim("IdHuyen", UserToken.IdHuyen.ToString()),
                        new Claim("IdTinhThanh", UserToken.IdTinhThanh.ToString()),
                        new Claim("IdGroup", UserToken.IdGroup.ToString()),
                    }),

                Expires = now.AddMinutes(Convert.ToInt32(expireMinutes)),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(symmetricKey), SecurityAlgorithms.HmacSha256Signature)
            };
            var stoken = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(stoken);
            return token;
        }

        public static bool ValidateToken(string token, out UserToken UserToken)
        {
            UserToken = new UserToken();
            var tokenHandler = new JwtSecurityTokenHandler();            
            var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

            if (jwtToken == null)
            {
                return false;
            }

            var a = jwtToken.Claims.GetType();
            var b = jwtToken.Claims.ToArray();
            Dictionary<string, string> dAttributes = new Dictionary<string, string>();
            for (int k = 0; k < b.Count(); k++)
            {
                string type = b[k].Type;
                string value = b[k].Value;
                dAttributes.Add(type, value);

            }
            UserToken.Id = int.Parse(dAttributes["Id"]);
            UserToken.Email = dAttributes["Email"];
            UserToken.UserName = dAttributes["UserName"];
            UserToken.IdCoQuan = Int32.Parse(dAttributes["IdCoQuan"].ToString());
            UserToken.IdHuyen = Int32.Parse(dAttributes["IdHuyen"].ToString());
            UserToken.IdGroup = Int32.Parse(dAttributes["IdGroup"].ToString());

            Models.ManagerFiles.Settings setting = Models.ManagerFiles.ManagerFilesService.GetSettingsInfo();
            string Secret = setting.Secret;
            byte[] key = Convert.FromBase64String(Secret);
            TokenValidationParameters parameters = new TokenValidationParameters()
            {
                RequireExpirationTime = true,
                ValidateIssuer = false,
                ValidateAudience = false,
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };
            SecurityToken securityToken;
            try
            {
                ClaimsPrincipal principal = tokenHandler.ValidateToken(token, parameters, out securityToken);
                Boolean akkk = principal.Identity.IsAuthenticated;
                var clam = principal.Claims;
                // Get the claims values
                var exp = principal.Claims.Where(c => c.Type == "exp")
                                   .Select(c => c.Value).SingleOrDefault();

                return akkk;
            }
            catch (Exception)
            {
                return false;
            }


        }


        public static dynamic UpdateStatus(USUsers dto,string flag)
        {
            string str_sql = "UpdateStatus";
            if (flag == "DangVien") {
                str_sql = "UpdateDangVien";
            }
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "user_users",
            new string[] { "@flag", "@Id", "@Status", "@DangVien", "@ModifiedBy" },
            new object[] { str_sql, dto.Id, dto.Status,dto.Status, dto.ModifiedBy });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }

        public static dynamic UpdateDeleted(USUsers dto)
        {
            string str_sql = "UpdateDeleted";           
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "user_users",
            new string[] { "@flag", "@Id", "@Status", "@DangVien", "@ModifiedBy" },
            new object[] { str_sql, dto.Id, dto.Status, dto.Status, dto.ModifiedBy });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();
        }
        public static dynamic SaveUserLog(USUsersLog dto)
        {
                 
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "user_users",
            new string[] { "@flag", "@Browser", "@Platform", "@Id", "@LoginInfo", "@Ip", "@Token" , "@Description" },
            new object[] { "SaveUserLog", dto.Browser, dto.Platform, dto.IdUser, dto.LoginInfo,dto.Ip,dto.Token,dto.Description });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();
        }

    }
}
