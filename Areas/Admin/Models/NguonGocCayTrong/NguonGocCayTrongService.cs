using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using API.Areas.Admin.Models.NguonGocCayTrong;
using API.Areas.Admin.Models.USUsers;
using API.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace API.Areas.Admin.Models.NguonGocCayTrong
{
    public class NguonGocCayTrongService
    {
        public static List<NguonGocCayTrong> GetListPagination(SearchNguonGocCayTrong dto, string SecretId)
        {
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
            str_sql = "GetListPagination";

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
            var tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_NguonGocCayTrong",
                new string[] { "@flag", "@CurrentPage", "@ItemsPerPage", "@Keyword", "@Status", "@IdTinhThanh", "@IdQuanHuyen", "@IdPhuongXa" },
                new object[] { str_sql, dto.CurrentPage, dto.ItemsPerPage, dto.Keyword, Status, dto.IdTinhThanh, dto.IdQuanHuyen, dto.IdPhuongXa });
            if (tabl == null)
            {
                return new List<NguonGocCayTrong>();
            }
            else
            {
                return (from r in tabl.AsEnumerable()
                        select new NguonGocCayTrong
                        {
                            Id = (int)r["Id"],
                            
                            Status = (bool)r["Status"],                           
                            UserName = (string)((r["UserName"] == System.DBNull.Value) ? "" : r["UserName"]),
                            //Title = (string)((r["Title"] == System.DBNull.Value) ? "" : r["Title"]),
                            LinkFile = (string)((r["LinkFile"] == System.DBNull.Value) ? "" : r["LinkFile"]),
                            FullName = (string)((r["FullName"] == System.DBNull.Value) ? "" : r["FullName"]),
                            Latitude = (string)((r["Latitude"] == System.DBNull.Value) ? "" : r["Latitude"]),
                            Longitude = (string)((r["Longitude"] == System.DBNull.Value) ? "" : r["Longitude"]),
                             TenTinhThanh= (string)((r["TenTinhThanh"] == System.DBNull.Value) ? "" : r["TenTinhThanh"]),
                             TenQuanHuyen= (string)((r["TenQuanHuyen"] == System.DBNull.Value) ? "" : r["TenQuanHuyen"]),
                             TenPhuongXa= (string)((r["TenPhuongXa"] == System.DBNull.Value) ? "" : r["TenPhuongXa"]),
                            Address = (string)((r["Address"] == System.DBNull.Value) ? "" : r["Address"]),
                            IdPhuongXa = (int)((r["IdPhuongXa"] == System.DBNull.Value) ? 0 : r["IdPhuongXa"]),
                            Ids = MyModels.Encode((int)r["Id"], SecretId),
                            TotalRows = (int)r["TotalRows"],
                        }).ToList();
            }


        }


        public static List<NguonGocCayTrong> GetListPaginationFE(SearchNguonGocCayTrong dto, string SecretId)
        {
            string str_sql = "GetListPaginationFE";
            
            

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
            var tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_NguonGocCayTrong",
                new string[] { "@flag", "@CurrentPage", "@ItemsPerPage", "@Keyword",  "@IdTinhThanh", "@IdQuanHuyen", "@IdPhuongXa" },
                new object[] { str_sql, dto.CurrentPage, dto.ItemsPerPage, dto.Keyword, dto.IdTinhThanh, dto.IdQuanHuyen, dto.IdPhuongXa });
            if (tabl == null)
            {
                return new List<NguonGocCayTrong>();
            }
            else
            {
                return (from r in tabl.AsEnumerable()
                        select new NguonGocCayTrong
                        {
                            Id = (int)r["Id"],

                            Status = (bool)r["Status"],
                            UserName = (string)((r["UserName"] == System.DBNull.Value) ? "" : r["UserName"]),
                            Description = (string)((r["Description"] == System.DBNull.Value) ? "" : r["Description"]),
                            LinkFile = (string)((r["LinkFile"] == System.DBNull.Value) ? "" : r["LinkFile"]),
                            FullName = (string)((r["FullName"] == System.DBNull.Value) ? "" : r["FullName"]),
                            Latitude = (string)((r["Latitude"] == System.DBNull.Value) ? "" : r["Latitude"]),
                            Longitude = (string)((r["Longitude"] == System.DBNull.Value) ? "" : r["Longitude"]),
                            TenTinhThanh = (string)((r["TenTinhThanh"] == System.DBNull.Value) ? "" : r["TenTinhThanh"]),
                            TenQuanHuyen = (string)((r["TenQuanHuyen"] == System.DBNull.Value) ? "" : r["TenQuanHuyen"]),
                            TenPhuongXa = (string)((r["TenPhuongXa"] == System.DBNull.Value) ? "" : r["TenPhuongXa"]),
                            Address = (string)((r["Address"] == System.DBNull.Value) ? "" : r["Address"]),
                            IdPhuongXa = (int)((r["IdPhuongXa"] == System.DBNull.Value) ? 0 : r["IdPhuongXa"]),
                            Ids = MyModels.Encode((int)r["Id"], SecretId),
                            TotalRows = (int)r["TotalRows"],
                        }).ToList();
            }


        }

        public static DataTable GetList(Boolean Selected = true)
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_NguonGocCayTrong",
                new string[] { "@flag", "@Selected" }, new object[] { "GetList", Selected });
            return tabl;

        }

        public static NguonGocCayTrong GetItemFE(int Id, string SecretId = null)
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_NguonGocCayTrong",
            new string[] { "@flag", "@Id" }, new object[] { "GetItemFE", Id });
            return (from r in tabl.AsEnumerable()
                    select new NguonGocCayTrong
                    {
                        Id = (int)r["Id"],

                        Status = (bool)r["Status"],
                        Description = (string)((r["Description"] == System.DBNull.Value) ? "" : r["Description"]),
                        UserName = (string)((r["UserName"] == System.DBNull.Value) ? "" : r["UserName"]),
                        LinkFile = (string)((r["LinkFile"] == System.DBNull.Value) ? "" : r["LinkFile"]),
                        FullName = (string)((r["FullName"] == System.DBNull.Value) ? "" : r["FullName"]),
                        Latitude = (string)((r["Latitude"] == System.DBNull.Value) ? "" : r["Latitude"]),
                        Longitude = (string)((r["Longitude"] == System.DBNull.Value) ? "" : r["Longitude"]),
                        Address = (string)((r["Address"] == System.DBNull.Value) ? "" : r["Address"]),
                        IdPhuongXa = (int)((r["IdPhuongXa"] == System.DBNull.Value) ? 0 : r["IdPhuongXa"]),
                        IdQuanHuyen = (int)((r["IdQuanHuyen"] == System.DBNull.Value) ? 0 : r["IdQuanHuyen"]),
                        Ids = MyModels.Encode((int)r["Id"], SecretId),
                    }).FirstOrDefault();
        }
        public static NguonGocCayTrong GetItem(int Id, string SecretId = null)
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_NguonGocCayTrong",
            new string[] { "@flag", "@Id" }, new object[] { "GetItem", Id });
            return (from r in tabl.AsEnumerable()
                    select new NguonGocCayTrong
                    {
                        Id = (int)r["Id"],                        
                        Status = (bool)r["Status"],
                        Description = (string)((r["Description"] == System.DBNull.Value) ? "" : r["Description"]),
                        UserName = (string)((r["UserName"] == System.DBNull.Value) ? "" : r["UserName"]),
                        LinkFile = (string)((r["LinkFile"] == System.DBNull.Value) ? "" : r["LinkFile"]),
                        FullName = (string)((r["FullName"] == System.DBNull.Value) ? "" : r["FullName"]),
                        Latitude = (string)((r["Latitude"] == System.DBNull.Value) ? "" : r["Latitude"]),
                        Longitude = (string)((r["Longitude"] == System.DBNull.Value) ? "" : r["Longitude"]),
                        Address = (string)((r["Address"] == System.DBNull.Value) ? "" : r["Address"]),
                        IdPhuongXa = (int)((r["IdPhuongXa"] == System.DBNull.Value) ? 0 : r["IdPhuongXa"]),
                        IdQuanHuyen = (int)((r["IdQuanHuyen"] == System.DBNull.Value) ? 0 : r["IdQuanHuyen"]),
                        Ids = MyModels.Encode((int)r["Id"], SecretId),
                    }).FirstOrDefault();
        }

        public static dynamic SaveItem(NguonGocCayTrong dto)
        {
            dto.Title = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.Title);
            dto.UserName = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.UserName);
            dto.FullName = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.FullName);
            dto.Address = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.Address);
            dto.Latitude = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.Latitude);
            dto.Longitude = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.Longitude);
            dto.Description = API.Models.MyHelper.StringHelper.RemoveTagsIntroText(dto.Description);
            dto.LinkFile = API.Models.MyHelper.StringHelper.RemoveTagsLink(dto.LinkFile);

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_NguonGocCayTrong",
            new string[] { "@flag", "@Id", "@Title", "@Status", "@Description", "@UserName", "@LinkFile", "@FullName", "@Password", "@IdPhuongXa", "@Address", "@Latitude", "@Longitude", "@CreatedBy", "@ModifiedBy" },
            new object[] { "SaveItem", dto.Id, dto.Title, dto.Status, dto.Description, dto.UserName,dto.LinkFile,dto.FullName,dto.Password, dto.IdPhuongXa, dto.Address, dto.Latitude, dto.Longitude, dto.CreatedBy, dto.ModifiedBy });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }

        public static dynamic SaveItemInfo(NguonGocCayTrong dto)
        {
            dto.Title = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.Title);
            dto.UserName = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.UserName);
            dto.FullName = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.FullName);
            dto.Address = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.Address);
            dto.Latitude = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.Latitude);
            dto.Longitude = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.Longitude);
            dto.Description = API.Models.MyHelper.StringHelper.RemoveTagsIntroText(dto.Description);
            dto.LinkFile = API.Models.MyHelper.StringHelper.RemoveTagsLink(dto.LinkFile);

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_NguonGocCayTrong",
            new string[] { "@flag", "@Id", "@Title", "@Description","@FullName", "@IdPhuongXa", "@Address", "@Latitude", "@Longitude", "@CreatedBy", "@ModifiedBy" },
            new object[] { "SaveItemInfo", dto.Id, dto.Title, dto.Description,  dto.FullName, dto.IdPhuongXa, dto.Address, dto.Latitude, dto.Longitude, dto.CreatedBy, dto.ModifiedBy });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }

        public static NguonGocCayTrong CheckPassword(int Id, string Password)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_NguonGocCayTrong",
                new string[] { "@flag", "@Id", "@Password" }, new object[] { "CheckPassword", Id, Password });

            return (from r in tabl.AsEnumerable()
                    select new NguonGocCayTrong
                    {
                        Id = (int)r["Id"],                        
                        Password = (string)((r["Password"] == System.DBNull.Value) ? "" : r["Password"]),
                        UserName = (string)((r["UserName"] == System.DBNull.Value) ? "" : r["UserName"]),
                    }).FirstOrDefault();
        }

        public static dynamic ChangePassword(int Id, string Password)
        {
            try
            {
                DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_NguonGocCayTrong",
                    new string[] { "@flag", "@Id", "@Password" },
                    new object[] { "ChangePassword", Id, Password});
                return new { N = Id };
            }
            catch
            {
                return new { N = 0 };
            }
        }

        public static dynamic UpdateStatus(NguonGocCayTrong dto)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_NguonGocCayTrong",
            new string[] { "@flag", "@Id", "@Status", "@ModifiedBy" },
            new object[] { "UpdateStatus", dto.Id, dto.Status, dto.ModifiedBy });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }

        public static dynamic DeleteItem(NguonGocCayTrong dto)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_NguonGocCayTrong",
            new string[] { "@flag", "@Id", "@ModifiedBy" },
            new object[] { "DeleteItem", dto.Id, dto.ModifiedBy });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }

        public static NguonGocCayTrong CheckLogin(string UserName, string Password)
        {
            string md5pass = USUsersService.GetMD5(Password);
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_NguonGocCayTrong",
                new string[] { "@flag", "@UserName", "@Password" },
                new object[] { "Login", UserName, md5pass });

            return (from r in tabl.AsEnumerable()
                    select new NguonGocCayTrong
                    {
                        Id = (int)r["Id"],
                        Status = (bool)r["Status"],
                        Description = (string)((r["Description"] == System.DBNull.Value) ? "" : r["Description"]),
                        UserName = (string)((r["UserName"] == System.DBNull.Value) ? "" : r["UserName"]),
                        LinkFile = (string)((r["LinkFile"] == System.DBNull.Value) ? "" : r["LinkFile"]),
                        FullName = (string)((r["FullName"] == System.DBNull.Value) ? "" : r["FullName"]),
                        Latitude = (string)((r["Latitude"] == System.DBNull.Value) ? "" : r["Latitude"]),
                        Longitude = (string)((r["Longitude"] == System.DBNull.Value) ? "" : r["Longitude"]),
                        Address = (string)((r["Address"] == System.DBNull.Value) ? "" : r["Address"]),
                        IdPhuongXa = (int)((r["IdPhuongXa"] == System.DBNull.Value) ? 0 : r["IdPhuongXa"]),
                        IdQuanHuyen = (int)((r["IdQuanHuyen"] == System.DBNull.Value) ? 0 : r["IdQuanHuyen"]),
                    }).FirstOrDefault();
        }
    }
}
