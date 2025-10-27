using API.Models;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Session;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
namespace API.Areas.Admin.Models.DMCoQuan
{
    public class DMCoQuanService
    {
        public static List<DMCoQuan> GetListPagination(SearchDMCoQuan dto, string SecretId)
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

            var tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "DanhMuc_CoQuan",
                new string[] { "@flag", "@CurrentPage", "@ItemsPerPage", "@Keyword", "@CategoryId", "@ParentId", "@Status" },
                new object[] { str_sql, dto.CurrentPage, dto.ItemsPerPage, dto.Keyword, dto.CategoryId, dto.ParentId, Status });
            if (tabl == null)
            {
                return new List<DMCoQuan>();
            }
            else
            {
                return (from r in tabl.AsEnumerable()
                        select new DMCoQuan
                        {
                            Id = (int)r["Id"],
                            Title = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
                            Code = (string)((r["Code"] == System.DBNull.Value) ? null : r["Code"]),
                            TitleCategory = (string)((r["TitleCategory"] == System.DBNull.Value) ? null : r["TitleCategory"]),
                            FolderUpload = (string)((r["FolderUpload"] == System.DBNull.Value) ? null : r["FolderUpload"]),
                            TemplateName = (string)((r["TemplateName"] == System.DBNull.Value) ? null : r["TemplateName"]),
                            CodeVanBan = (string)((r["CodeVanBan"] == System.DBNull.Value) ? null : r["CodeVanBan"]),
                            CodeMotCua = (string)((r["CodeMotCua"] == System.DBNull.Value) ? null : r["CodeMotCua"]),
                            CodeLCT = (string)((r["CodeLCT"] == System.DBNull.Value) ? null : r["CodeLCT"]),
                            CssName = (string)((r["CssName"] == System.DBNull.Value) ? null : r["CssName"]),
                            Description = (string)((r["Description"] == System.DBNull.Value) ? null : r["Description"]),
                            Status = (bool)r["Status"],
                            CategoryId = (int)((r["CategoryId"] == System.DBNull.Value) ? 0 : r["CategoryId"]),
                            ParentId = (int)((r["ParentId"] == System.DBNull.Value) ? 0 : r["ParentId"]),
                            Ids = MyModels.Encode((int)r["Id"], SecretId),
                            TotalRows = (int)r["TotalRows"]
                        }).ToList();
            }
        }



        public static DataTable GetList(Boolean CHON = true)
        {
            decimal chon = Convert.ToDecimal(CHON);
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "DanhMuc_CoQuan",
                new string[] { "@flag", "@sCHON" }, new object[] { "GetList", chon });
            return tabl;

        }
        public static List<SelectListItem> GetListByLoaiCoQuan(int CategoryId, int Def = 0, int ParentId = 0, Boolean flagTitle = true)
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "DanhMuc_CoQuan",
                new string[] { "@flag", "@Selected", "@CategoryId", "@ParentId" }, new object[] { "GetListByLoaiCoQuan", 1, CategoryId, ParentId });

            List<SelectListItem> ListItems = new List<SelectListItem>();
            if (flagTitle)
            {
                ListItems = (from r in tabl.AsEnumerable()
                             select new SelectListItem
                             {
                                 Value = (string)((r["Id"] == System.DBNull.Value) ? null : r["Id"].ToString()),
                                 Text = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
                             }).ToList();
            }
            else
            {
                ListItems = (from r in tabl.AsEnumerable()
                             select new SelectListItem
                             {
                                 Value = (string)((r["Id"] == System.DBNull.Value) ? null : r["Id"].ToString()),
                                 Text = (string)((r["CompanyName"] == System.DBNull.Value) ? null : r["CompanyName"]),
                             }).ToList();
            }

            if (Def == 0)
            {
                ListItems.Insert(0, (new SelectListItem { Text = "Chọn tất cả", Value = "0" }));
            }
            return ListItems;
        }

        public static List<DMCoQuan> GetListByParent(int ParentId = 0)
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "DanhMuc_CoQuan",
                new string[] { "@flag", "@ParentId" }, new object[] { "GetListByParent", ParentId });

            if (tabl == null)
            {
                return new List<DMCoQuan>();
            }
            else
            {
                return (from r in tabl.AsEnumerable()
                        select new DMCoQuan
                        {
                            Id = (int)r["Id"],
                            Title = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
                            Code = (string)((r["Code"] == System.DBNull.Value) ? null : r["Code"]),
                            CodeDemo = (string)((r["CodeDemo"] == System.DBNull.Value) ? null : r["CodeDemo"]),
                            FolderUpload = (string)((r["FolderUpload"] == System.DBNull.Value) ? null : r["FolderUpload"]),
                        }).ToList();
            }
        }

        public static List<DMCoQuan> GetListByParentAPI(int ParentId = 0)
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "DanhMuc_CoQuan",
                new string[] { "@flag", "@ParentId" }, new object[] { "GetListByParent", ParentId });

            if (tabl == null)
            {
                return new List<DMCoQuan>();
            }
            else
            {
                return (from r in tabl.AsEnumerable()
                        select new DMCoQuan
                        {
                            Id = (int)r["Id"],
                            Title = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
                        }).ToList();
            }
        }

        public static List<SelectListItem> GetListByParent(int Def = 0, int ParentId = 0, Boolean flagTitle = true)
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "DanhMuc_CoQuan",
                new string[] { "@flag", "@ParentId" }, new object[] { "GetListByParent", ParentId });

            List<SelectListItem> ListItems = new List<SelectListItem>();

            if (flagTitle)
            {
                ListItems = (from r in tabl.AsEnumerable()
                             select new SelectListItem
                             {
                                 Value = (string)((r["Id"] == System.DBNull.Value) ? null : r["Id"].ToString()),
                                 Text = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
                             }).ToList();
            }
            else
            {
                ListItems = (from r in tabl.AsEnumerable()
                             select new SelectListItem
                             {
                                 Value = (string)((r["Id"] == System.DBNull.Value) ? null : r["Id"].ToString()),
                                 Text = (string)((r["CompanyName"] == System.DBNull.Value) ? null : r["CompanyName"]),
                             }).ToList();
            }
            if (Def == 0)
            {
                ListItems.Insert(0, (new SelectListItem { Text = "Chọn Cơ Quan cha", Value = "0" }));
            }
            return ListItems;
        }

        public static List<SelectListItem> GetListTemplate()
        {
            List<SelectListItem> ListItems = new List<SelectListItem>() { };
            string dirPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot") + "/Templates";
            DirectoryInfo dirInfo = new DirectoryInfo(dirPath);
            if (Directory.Exists(dirPath))
            {
                DirectoryInfo[] childDirs = dirInfo.GetDirectories();
                int i = 0;
                foreach (DirectoryInfo childDir in childDirs)
                {
                    ListItems.Insert(i, (new SelectListItem { Text = childDir.Name.ToString(), Value = childDir.Name.ToString() }));
                    i++;
                }
            }

            return ListItems;
        }

        public static DMCoQuan GetItem(int Id, string SecretId = null, string Culture = "vi")
        {
            string sql = "GetItem";
            Culture = API.Models.MyHelper.StringHelper.ConverLan(Culture);
            if (Culture != "vi")
            {
                sql = sql + "_" + Culture.ToUpper();
            }

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "DanhMuc_CoQuan",
            new string[] { "@flag", "@Id" }, new object[] { sql, Id });
            DMCoQuan Item = (from r in tabl.AsEnumerable()
                             select new DMCoQuan
                             {
                                 Id = (int)r["Id"],
                                 Title = (string)r["Title"],
                                 Code = (string)((r["Code"] == System.DBNull.Value) ? null : r["Code"]),
                                 CompanyName = (string)((r["CompanyName"] == System.DBNull.Value) ? null : r["CompanyName"]),
                                 CodeVanBan = (string)((r["CodeVanBan"] == System.DBNull.Value) ? null : r["CodeVanBan"]),
                                 CodeMotCua = (string)((r["CodeMotCua"] == System.DBNull.Value) ? null : r["CodeMotCua"]),
                                 CodeLCT = (string)((r["CodeLCT"] == System.DBNull.Value) ? null : r["CodeLCT"]),
                                 Slogan = (string)((r["Slogan"] == System.DBNull.Value) ? null : r["Slogan"]),
                                 Facebook = (string)((r["Facebook"] == System.DBNull.Value) ? null : r["Facebook"]),
                                 Twitter = (string)((r["Twitter"] == System.DBNull.Value) ? null : r["Twitter"]),
                                 Youtube = (string)((r["Youtube"] == System.DBNull.Value) ? null : r["Youtube"]),
                                 FolderUpload = (string)((r["FolderUpload"] == System.DBNull.Value) ? null : r["FolderUpload"]),
                                 Description = (string)((r["Description"] == System.DBNull.Value) ? null : r["Description"]),
                                 CssName = (string)((r["CssName"] == System.DBNull.Value) ? null : r["CssName"]),
                                 PositionBannerHome = (int)((r["PositionBannerHome"] == System.DBNull.Value) ? 0 : r["PositionBannerHome"]),
                                 Status = (bool)r["Status"],
                                 CategoryId = (int)r["CategoryId"],
                                 ParentId = (int)r["ParentId"],
                                 Metadesc = (string)((r["Metadesc"] == System.DBNull.Value) ? null : r["Metadesc"]),
                                 Metakey = (string)((r["Metakey"] == System.DBNull.Value) ? null : r["Metakey"]),
                                 Images = (string)((r["Images"] == System.DBNull.Value) ? null : r["Images"]),
                                 TemplateName = (string)((r["TemplateName"] == System.DBNull.Value) ? "" : r["TemplateName"]),
                                 Email = (string)((r["Email"] == System.DBNull.Value) ? null : r["Email"]),
                                 Address = (string)((r["Address"] == System.DBNull.Value) ? "" : r["Address"]),
                                 Telephone = (string)((r["Telephone"] == System.DBNull.Value) ? "" : r["Telephone"]),
                                 Fax = (string)((r["Fax"] == System.DBNull.Value) ? "" : r["Fax"]),
                                 Icon = (string)((r["Icon"] == System.DBNull.Value) ? "" : r["Icon"]),
                                 DataHeader = (string)((r["DataHeader"] == System.DBNull.Value) ? "" : r["DataHeader"]),
                                 //DataFooter = (string)((r["DataFooter"] == System.DBNull.Value) ? "" : r["DataFooter"]),
                                 CTNC = (string)((r["CTNC"] == System.DBNull.Value) ? "" : r["CTNC"]),
                                 ThongBao = (string)((r["ThongBao"] == System.DBNull.Value) ? "Articles" : r["ThongBao"]),
                                 Metadata = (string)((r["Metadata"] == System.DBNull.Value) ? null : r["Metadata"]),
                                 Ids = MyModels.Encode((int)r["Id"], SecretId),
                                 DomainName = (string)((r["DomainName"] == System.DBNull.Value) ? "" : r["DomainName"]),
                             }).FirstOrDefault();

            if (Item != null)
            {
                if (Item.Metadata != null && Item.Metadata != "")
                {
                    try
                    {
                        Item.MetadataCV = JsonConvert.DeserializeObject<API.Areas.Admin.Models.DMCoQuan.Metadata>(Item.Metadata);
                    }
                    catch
                    {
                        Item.MetadataCV = new API.Areas.Admin.Models.DMCoQuan.Metadata() { UrlImage = "/uploads/" + Item.FolderUpload + "/banners/zalo_" + Item.FolderUpload + ".jpg", Placename = Item.Title };
                    }
                }
                else
                {
                    Item.MetadataCV = new API.Areas.Admin.Models.DMCoQuan.Metadata() { UrlImage = "/uploads/" + Item.FolderUpload + "/banners/zalo_" + Item.FolderUpload + ".jpg", Placename = Item.Title };
                }

                if (Item.MetadataCV.Placename == null || Item.MetadataCV.Placename == "")
                {
                    Item.MetadataCV.Placename = Item.Title;
                }
            }

            return Item;
        }
        public static DMCoQuanConfig GetDMCoQuanConfig(DMCoQuanConfig dto, int Id)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "DanhMuc_CoQuan",
            new string[] { "@flag", "@Id", "@DataHeader", "@DataFooter", "@LinkBanDo" }, new object[] { "GetConfig", Id, dto.DataHeader, dto.DataFooter, dto.LinkBanDo });
            return (from r in tabl.AsEnumerable()
                    select new DMCoQuanConfig
                    {
                        Id = (int)r["Id"],
                        DataHeader = (string)((r["DataHeader"] == System.DBNull.Value) ? null : r["DataHeader"]),
                        DataFooter = (string)((r["DataFooter"] == System.DBNull.Value) ? null : r["DataFooter"]),
                        LinkBanDo = (string)((r["LinkBanDo"] == System.DBNull.Value) ? null : r["LinkBanDo"]),
                    }).FirstOrDefault();
        }
        public static DataTable GetListChild(int ParentId = 0)
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "DanhMuc_CoQuan",
                new string[] { "@flag", "@ParentId" }, new object[] { "GetListChild", ParentId });

            return tabl;
        }

        public static DMCoQuan GetItemLevel(int Id)
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "DanhMuc_CoQuan",
            new string[] { "@flag", "@Id" }, new object[] { "GetItemLevel", Id });
            return (from r in tabl.AsEnumerable()
                    select new DMCoQuan
                    {
                        Id = (int)r["Id"],
                        Level = (int)r["Level"],
                        CategoryId = (int)((r["CategoryId"] == System.DBNull.Value) ? 0 : r["CategoryId"]),
                        Title = (string)r["Title"],
                    }).FirstOrDefault();
        }


        public static DMCoQuan GetItemByCode(string Code, Boolean flagDev, string CssName = "", string Culture = "vi")
        {


            string sql = "GetItemByCode";
            Culture = API.Models.MyHelper.StringHelper.ConverLan(Culture);
            if (Culture != "vi")
            {
                if (flagDev == true)
                {
                    sql = "GetItemByCodeDev" + "_" + Culture.ToUpper();
                }
                else
                {
                    sql = "GetItemByCode" + "_" + Culture.ToUpper();
                }


            }
            else
            {
                if (flagDev == true)
                {
                    sql = "GetItemByCodeDev";
                }
            }

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "DanhMuc_CoQuan",
            new string[] { "@flag", "@Code" }, new object[] { sql, Code.Trim() });
            DMCoQuan Item = (from r in tabl.AsEnumerable()
                             select new DMCoQuan
                             {
                                 Id = (int)r["Id"],
                                 Title = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
                                 Code = (string)((r["Code"] == System.DBNull.Value) ? null : r["Code"]),
                                 CompanyName = (string)((r["CompanyName"] == System.DBNull.Value) ? null : r["CompanyName"]),
                                 CodeVanBan = (string)((r["CodeVanBan"] == System.DBNull.Value) ? null : r["CodeVanBan"]),
                                 CodeMotCua = (string)((r["CodeMotCua"] == System.DBNull.Value) ? null : r["CodeMotCua"]),
                                 CodeMotCuaCha = (string)((r["CodeMotCuaCha"] == System.DBNull.Value) ? null : r["CodeMotCuaCha"]),
                                 CodeLCT = (string)((r["CodeLCT"] == System.DBNull.Value) ? null : r["CodeLCT"]),
                                 Slogan = (string)((r["Slogan"] == System.DBNull.Value) ? "" : r["Slogan"]),
                                 Facebook = (string)((r["Facebook"] == System.DBNull.Value) ? "" : r["Facebook"]),
                                 Twitter = (string)((r["Twitter"] == System.DBNull.Value) ? "" : r["Twitter"]),
                                 Youtube = (string)((r["Youtube"] == System.DBNull.Value) ? "" : r["Youtube"]),
                                 Description = (string)((r["Description"] == System.DBNull.Value) ? "" : r["Description"]),
                                 CssName = (string)((r["CssName"] == System.DBNull.Value) ? "" : r["CssName"]),
                                 PositionBannerHome = (int)((r["PositionBannerHome"] == System.DBNull.Value) ? 0 : r["PositionBannerHome"]),
                                 Status = (bool)r["Status"],
                                 CategoryId = (int)r["CategoryId"],
                                 ParentId = (int)r["ParentId"],
                                 Metadesc = (string)((r["Metadesc"] == System.DBNull.Value) ? "" : r["Metadesc"]),
                                 FolderUpload = (string)((r["FolderUpload"] == System.DBNull.Value) ? "" : r["FolderUpload"]),
                                 Metakey = (string)((r["Metakey"] == System.DBNull.Value) ? "" : r["Metakey"]),
                                 TemplateName = (string)((r["TemplateName"] == System.DBNull.Value) ? "" : r["TemplateName"]),
                                 Email = (string)((r["Email"] == System.DBNull.Value) ? null : r["Email"]),
                                 Address = (string)((r["Address"] == System.DBNull.Value) ? "" : r["Address"]),
                                 Telephone = (string)((r["Telephone"] == System.DBNull.Value) ? "" : r["Telephone"]),
                                 Fax = (string)((r["Fax"] == System.DBNull.Value) ? "" : r["Fax"]),
                                 Icon = (string)((r["Icon"] == System.DBNull.Value) ? "" : r["Icon"]),
                                 Images = (string)((r["Images"] == System.DBNull.Value) ? null : r["Images"]),
                                 DataHeader = (string)((r["DataHeader"] == System.DBNull.Value) ? null : r["DataHeader"]),
                                 CTNC = (string)((r["CTNC"] == System.DBNull.Value) ? null : r["CTNC"]),
                                 ThongBao = (string)((r["ThongBao"] == System.DBNull.Value) ? "Articles" : r["ThongBao"]),
                                 Metadata = (string)((r["Metadata"] == System.DBNull.Value) ? null : r["Metadata"]),
                             }).FirstOrDefault();

            if (CssName != null && CssName != "")
            {
                Item.CssName = CssName;
            }

            if (Item != null)
            {
                if (Item.Metadata != null && Item.Metadata != "")
                {
                    try
                    {
                        Item.MetadataCV = JsonConvert.DeserializeObject<API.Areas.Admin.Models.DMCoQuan.Metadata>(Item.Metadata);
                    }
                    catch
                    {
                        Item.MetadataCV = new API.Areas.Admin.Models.DMCoQuan.Metadata() { UrlImage = "/uploads/" + Item.FolderUpload + "/banners/" + Item.FolderUpload + ".jpg", Placename = Item.Title };
                    }
                }
                else
                {
                    Item.MetadataCV = new API.Areas.Admin.Models.DMCoQuan.Metadata() { UrlImage = "/uploads/" + Item.FolderUpload + "/banners/" + Item.FolderUpload + ".jpg", Placename = Item.Title };
                }

                if (Item.MetadataCV.Placename == null || Item.MetadataCV.Placename == "")
                {
                    Item.MetadataCV.Placename = Item.Title;
                }
            }
            return Item;
        }

        public static dynamic SaveItemInfo(DMCoQuan dto)
        {
            if (dto.TemplateName == null || dto.TemplateName == "")
            {
                dto.TemplateName = "Default";
            }

            if (dto.MetadataCV.Placename == null || dto.MetadataCV.Placename.Trim() == "")
            {
                dto.MetadataCV.Placename = dto.Title;
            }

            if (dto.MetadataCV != null)
            {
                dto.Metadata = JsonConvert.SerializeObject(dto.MetadataCV);
            }

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "DanhMuc_CoQuan",
            new string[] { "@flag", "@Title", "@Id", "@Description", "@Address", "@Telephone", "@CreatedBy", "@ModifiedBy", "@Metadesc", "@Metakey", "@Fax", "@Images", "@Email", "@CompanyName", "@Slogan", "@Facebook", "@Twitter", "@Youtube", "@CodeMotCua", "@CodeVanBan", "@CodeLCT", "@CTNC", "@ThongBao", "@Metadata" },
            new object[] { "SaveItemInfo", dto.Title, dto.Id, dto.Description, dto.Address, dto.Telephone, dto.CreatedBy, dto.ModifiedBy, dto.Metadesc, dto.Metadesc, dto.Fax, dto.Images, dto.Email, dto.CompanyName, dto.Slogan, dto.Facebook, dto.Twitter, dto.Youtube, dto.CodeMotCua, dto.CodeVanBan, dto.CodeLCT, dto.CTNC, dto.ThongBao, dto.Metadata });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }
        public static dynamic SaveItem(DMCoQuan dto)
        {
            if (dto.TemplateName == null || dto.TemplateName == "")
            {
                dto.TemplateName = "Default";
            }

            if (dto.MetadataCV.Placename == null || dto.MetadataCV.Placename.Trim() == "")
            {
                dto.MetadataCV.Placename = dto.Title;
            }

            if (dto.MetadataCV != null)
            {
                dto.Metadata = JsonConvert.SerializeObject(dto.MetadataCV);
            }

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "DanhMuc_CoQuan",
            new string[] { "@flag", "@Title", "@Code", "@FolderUpload", "@Id", "@Description", "@Status", "@TemplateName", "@Icon", "@Address", "@Telephone", "@CreatedBy", "@ModifiedBy", "@CategoryId", "ParentId", "@Metadesc", "@Metakey", "@Fax", "@Images", "@Email", "@CompanyName", "@Slogan", "@Facebook", "@Twitter", "@Youtube", "@CssName", "@PositionBannerHome", "@DataHeader", "@CodeMotCua", "@CodeVanBan", "@CodeLCT", "@CTNC", "@ThongBao", "@Metadata", "@DomainName" },
            new object[] { "SaveItem", dto.Title, dto.Code, dto.FolderUpload, dto.Id, dto.Description, dto.Status, dto.TemplateName, dto.Icon, dto.Address, dto.Telephone, dto.CreatedBy, dto.ModifiedBy, dto.CategoryId, dto.ParentId, dto.Metadesc, dto.Metadesc, dto.Fax, dto.Images, dto.Email, dto.CompanyName, dto.Slogan, dto.Facebook, dto.Twitter, dto.Youtube, dto.CssName, dto.PositionBannerHome, dto.DataHeader, dto.CodeMotCua, dto.CodeVanBan, dto.CodeLCT, dto.CTNC, dto.ThongBao, dto.Metadata, dto.DomainName });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }
        public static dynamic DeleteItem(DMCoQuan dto)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "DanhMuc_CoQuan",
            new string[] { "@flag", "@Id", "@ModifiedBy" },
            new object[] { "DeleteItem", dto.Id, dto.ModifiedBy });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();
        }
        public static dynamic UpdateStatus(DMCoQuan dto)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "DanhMuc_CoQuan",
            new string[] { "@flag", "@Id", "@Status", "@ModifiedBy" },
            new object[] { "UpdateStatus", dto.Id, dto.Status, dto.ModifiedBy });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }

        public static List<string> danhsachBuonMaThuot()
        {
            /*
             List<string> ListItems = new List<string>() {
                "buonmathuot.daklak.gov.vn",
                "portal-bmt.vnptdaklak.vn",
                "tanlap.buonmathuot.daklak.gov.vn",
                "tanthanh.buonmathuot.daklak.gov.vn",
                "tanhoa.buonmathuot.daklak.gov.vn",
                "tuan.buonmathuot.daklak.gov.vn",
                "thongnhat.buonmathuot.daklak.gov.vn",
                "thanhcong.buonmathuot.daklak.gov.vn",
                "thanhnhat.buonmathuot.daklak.gov.vn",
                "thangloi.buonmathuot.daklak.gov.vn",
                "tanloi.buonmathuot.daklak.gov.vn",
                "khanhxuan.buonmathuot.daklak.gov.vn",
                "eatam.buonmathuot.daklak.gov.vn",
                "tanan.buonmathuot.daklak.gov.vn",
                "tantien.buonmathuot.daklak.gov.vn",
                "eatu.buonmathuot.daklak.gov.vn",
                "cuebur.buonmathuot.daklak.gov.vn",
                "hoathang.buonmathuot.daklak.gov.vn",
                "hoathuan.buonmathuot.daklak.gov.vn",
                "eakao.buonmathuot.daklak.gov.vn",
                "hoakhanh.buonmathuot.daklak.gov.vn",
                "hoaphu.buonmathuot.daklak.gov.vn",
                "hoaxuan.buonmathuot.daklak.gov.vn",
            };
            
             */

            // Buon Ma thuột

            List<string> ListItems = new List<string>() {
                "cG9ydGFsLXhhYm10LnZucHRkYWtsYWsudm4=",
                "bG9jYWxob3N0OjQ0Mzk2",
                "YnVvbm1hdGh1b3QuZGFrbGFrLmdvdi52bg==",
                "cG9ydGFsLWJtdC52bnB0ZGFrbGFrLnZu",
                "dGFubGFwLmJ1b25tYXRodW90LmRha2xhay5nb3Yudm4=",
                "dGFudGhhbmguYnVvbm1hdGh1b3QuZGFrbGFrLmdvdi52bg==",
                "dGFuaG9hLmJ1b25tYXRodW90LmRha2xhay5nb3Yudm4=",
                "dHVhbi5idW9ubWF0aHVvdC5kYWtsYWsuZ292LnZu",
                "dGhvbmduaGF0LmJ1b25tYXRodW90LmRha2xhay5nb3Yudm4=",
                "dGhhbmhjb25nLmJ1b25tYXRodW90LmRha2xhay5nb3Yudm4=",
                "dGhhbmhuaGF0LmJ1b25tYXRodW90LmRha2xhay5nb3Yudm4=",
                "dGhhbmdsb2kuYnVvbm1hdGh1b3QuZGFrbGFrLmdvdi52bg==",
                "dGFubG9pLmJ1b25tYXRodW90LmRha2xhay5nb3Yudm4=",
                "a2hhbmh4dWFuLmJ1b25tYXRodW90LmRha2xhay5nb3Yudm4=",
                "ZWF0YW0uYnVvbm1hdGh1b3QuZGFrbGFrLmdvdi52bg==",
                "dGFuYW4uYnVvbm1hdGh1b3QuZGFrbGFrLmdvdi52bg==",
                "dGFudGllbi5idW9ubWF0aHVvdC5kYWtsYWsuZ292LnZu",
                "ZWF0dS5idW9ubWF0aHVvdC5kYWtsYWsuZ292LnZu",
                "Y3VlYnVyLmJ1b25tYXRodW90LmRha2xhay5nb3Yudm4=",
                "aG9hdGhhbmcuYnVvbm1hdGh1b3QuZGFrbGFrLmdvdi52bg==",
                "aG9hdGh1YW4uYnVvbm1hdGh1b3QuZGFrbGFrLmdvdi52bg==",
                "ZWFrYW8uYnVvbm1hdGh1b3QuZGFrbGFrLmdvdi52bg==",
                "aG9ha2hhbmguYnVvbm1hdGh1b3QuZGFrbGFrLmdvdi52bg==",
                "aG9hcGh1LmJ1b25tYXRodW90LmRha2xhay5nb3Yudm4=",
                "aG9heHVhbi5idW9ubWF0aHVvdC5kYWtsYWsuZ292LnZu",
                "cG9ydGFsLWVhc3VwLnZucHRkYWtsYWsudm4=",
            };

            // End Buon Ma thuột
            /*
            List<string> ListItems = new List<string>() {
                "cG9ydGFsLWVhc3VwLnZucHRkYWtsYWsudm4=",
                "bG9jYWxob3N0OjQ0Mzk2",                    
            };
            */
            return ListItems;

        }
        public static Boolean KiemTraBMT(string Code)
        {
            return true;
            /*
            List<string> ListItems = DMCoQuanService.danhsachBuonMaThuot();
            if(Code!=null && Code != "")
            {
                for(int i = 0; i < ListItems.Count; i++)
                {
                    if (System.Text.Encoding.UTF8.GetString(System.Convert.FromBase64String(ListItems[i]))  == Code)
                    {
                        return true;
                    }
                }
            }
            return false;*/

        }
        public static dynamic SaveItemConfig(DMCoQuan dto)
        {
            if (dto.TemplateName == null || dto.TemplateName == "")
            {
                dto.TemplateName = "Default";
            }

            if (dto.MetadataCV.Placename == null || dto.MetadataCV.Placename.Trim() == "")
            {
                dto.MetadataCV.Placename = dto.Title;
            }

            if (dto.MetadataCV != null)
            {
                dto.Metadata = JsonConvert.SerializeObject(dto.MetadataCV);
            }
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "DanhMuc_CoQuan",
            new string[] { "@flag", "@Id", "@DataHeader", "@DataFooter", "@Metadata" },
            new object[] { "SaveItemConfig", dto.Id, dto.DataHeader, dto.DataFooter, dto.Metadata });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();
        }

        public static DMCoQuan GetIdCoquanByDomain(string DomainName = "")
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "DanhMuc_CoQuan",
                new string[] { "@flag", "@Code" }, 
                new object[] { "GetIdCoquanByDomain", DomainName });
            DMCoQuan Item = (from r in tabl.AsEnumerable()
                             select new DMCoQuan
                             {
                                 Id = (int)r["Id"],
                                 DomainName = (string)((r["Code"] == System.DBNull.Value) ? "" : r["Code"]),
                             }).FirstOrDefault();

            return Item;
        }

        public static dynamic ChangeTheme(int IdCoQuan, string theme, string css)
        {
            var TemplateName = "Default";
            var Css = "vnpt-style-5.css?v=" + DateTime.Now.ToString("yyyyMMddhh");

            if (!String.IsNullOrEmpty(theme))
            {
                TemplateName = theme;
            }

            if (!String.IsNullOrEmpty(css))
            {
                Css = css;
            }

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "DanhMuc_CoQuan",
            new string[] { "@flag", "@Id", "@TemplateName", "@CssName" },
            new object[] { "ChangeTheme", IdCoQuan, TemplateName, Css });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }
    }
}
