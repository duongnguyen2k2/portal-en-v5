using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API.Areas.Admin.Models.HopTrucTuyen;
using API.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using QRCoder;

namespace API.Areas.Admin.Models.HopTrucTuyen
{
    public class HopTrucTuyenService
    {
        public static List<HopTrucTuyen> GetListPagination(SearchHopTrucTuyen dto, string SecretId)
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

            var tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_HopTrucTuyen",
                new string[] { "@flag", "@CurrentPage", "@ItemsPerPage", "@Keyword", "@IdCoQuan", "@CatId", "@Status" },
                new object[] { str_sql, dto.CurrentPage, dto.ItemsPerPage, dto.Keyword, dto.IdCoQuan, dto.CatId , Status });

            if (tabl == null)
            {
                return new List<HopTrucTuyen>();
            }
            else
            {
                return (from r in tabl.AsEnumerable()
                        select new HopTrucTuyen
                        {
                            Id = (int)r["Id"],
                            Title = (string)r["Title"],
                            Status = (bool)r["Status"],
                            Description = (string)((r["Description"] == System.DBNull.Value) ? "" : r["Description"]),
                            Code = (string)((r["Code"] == System.DBNull.Value) ? "" : r["Code"]),
                            TenCoQuan = (string)((r["TenCoQuan"] == System.DBNull.Value) ? "" : r["TenCoQuan"]),
                            Alias = (string)((r["Alias"] == System.DBNull.Value) ? "" : r["Alias"]),
                            Images = (string)((r["Images"] == System.DBNull.Value) ? "" : r["Images"]),
                            Link = (string)((r["Link"] == System.DBNull.Value) ? "" : r["Link"]),                            
                            CatId = (int)((r["CatId"] == System.DBNull.Value) ? 0 : r["CatId"]),
                            IdCoQuan = (int)((r["IdCoQuan"] == System.DBNull.Value) ? 0 : r["IdCoQuan"]),
                            PublishUp = (DateTime)((r["PublishUp"] == System.DBNull.Value) ? null : r["PublishUp"]),
                            PublishUpShow = (string)((r["PublishUp"] == System.DBNull.Value) ? DateTime.Now.ToString("dd/MM/yyyy") : (string)((DateTime)r["PublishUp"]).ToString("dd/MM/yyyy")),
                            PublishDown = (DateTime)((r["PublishDown"] == System.DBNull.Value) ? null : r["PublishDown"]),
                            PublishDownShow = (string)((r["PublishDown"] == System.DBNull.Value) ? DateTime.Now.ToString("dd/MM/yyyy") : (string)((DateTime)r["PublishDown"]).ToString("dd/MM/yyyy")),
                            Ids = MyModels.Encode((int)r["Id"], SecretId),
                            TotalRows = (int)r["TotalRows"],
                        }).ToList();
            }


        }

        public static DataTable GetList(Boolean Selected = true)
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_HopTrucTuyen",
                new string[] { "@flag", "@Selected" }, new object[] { "GetList", Selected });
            return tabl;

        }

        public static List<SelectListItem> GetListSelectItems(Boolean Selected = true)
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_HopTrucTuyen",
                new string[] { "@flag", "@Selected" }, new object[] { "GetList", Convert.ToDecimal(Selected) });
            List<SelectListItem> ListItems = (from r in tabl.AsEnumerable()
                                              select new SelectListItem
                                              {
                                                  Value = (string)((r["Id"] == System.DBNull.Value) ? null : r["Id"].ToString()),
                                                  Text = (string)((r["Title"] == System.DBNull.Value) ? "" : r["Title"]),
                                              }).ToList();
            ListItems.Insert(0, (new SelectListItem { Text = "--- Chọn Chức Vụ ---", Value = "0" }));
            return ListItems;

        }

        public static List<HopTrucTuyen> GetListNew(int ItemsPerPage,int IdCoQuan)
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_HopTrucTuyen",
            new string[] { "@flag", "@ItemsPerPage", "@IdCoQuan" }, new object[] { "GetListNew", ItemsPerPage , IdCoQuan });

            List<HopTrucTuyen> ListItems = (from r in tabl.AsEnumerable()
                                 select new HopTrucTuyen
                                 {
                                     Id = (int)r["Id"],
                                     Title = (string)r["Title"],
                                     Status = (bool)r["Status"],
                                     Description = (string)((r["Description"] == System.DBNull.Value) ? "" : r["Description"]),
                                     Alias = (string)((r["Alias"] == System.DBNull.Value) ? "" : r["Alias"]),
                                     Code = (string)((r["Code"] == System.DBNull.Value) ? "" : r["Code"]),
                                     Images = (string)((r["Images"] == System.DBNull.Value) ? "" : r["Images"]),
                                     Link = (string)((r["Link"] == System.DBNull.Value) ? "" : r["Link"]),
                                     CatId = (int)((r["CatId"] == System.DBNull.Value) ? 0 : r["CatId"]),
                                     IdCoQuan = (int)((r["IdCoQuan"] == System.DBNull.Value) ? 0 : r["IdCoQuan"]),
                                     PublishUp = (DateTime)((r["PublishUp"] == System.DBNull.Value) ? null : r["PublishUp"]),
                                     PublishUpShow = (string)((r["PublishUp"] == System.DBNull.Value) ? DateTime.Now.ToString("dd/MM/yyyy") : (string)((DateTime)r["PublishUp"]).ToString("dd/MM/yyyy")),
                                     PublishDown = (DateTime)((r["PublishDown"] == System.DBNull.Value) ? null : r["PublishDown"]),
                                     PublishDownShow = (string)((r["PublishDown"] == System.DBNull.Value) ? DateTime.Now.ToString("dd/MM/yyyy") : (string)((DateTime)r["PublishDown"]).ToString("dd/MM/yyyy"))                                     
                                 }).ToList();
            
            return ListItems;

        }

        public static HopTrucTuyen GetItemByCode(int Id, string Code, string SecretId = null)
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_HopTrucTuyen",
            new string[] { "@flag", "@Id", "@Code" }, new object[] { "GetItemByCode", Id, Code });

            HopTrucTuyen Item = (from r in tabl.AsEnumerable()
                    select new HopTrucTuyen
                    {
                        Id = (int)r["Id"],
                        Code = (string)r["Code"],
                        Title = (string)r["Title"],
                        Status = (bool)r["Status"],                        
                        Description = (string)((r["Description"] == System.DBNull.Value) ? "" : r["Description"]),
                        Str_ListFile = (string)((r["Str_ListFile"] == System.DBNull.Value) ? "" : r["Str_ListFile"]),
                        FullText = (string)((r["FullText"] == System.DBNull.Value) ? "" : r["FullText"]),
                        TenCoQuan = (string)((r["TenCoQuan"] == System.DBNull.Value) ? "" : r["TenCoQuan"]),
                        Alias = (string)((r["Alias"] == System.DBNull.Value) ? "" : r["Alias"]),
                        Images = (string)((r["Images"] == System.DBNull.Value) ? "" : r["Images"]),
                        Link = (string)((r["Link"] == System.DBNull.Value) ? "" : r["Link"]),
                        CatId = (int)((r["CatId"] == System.DBNull.Value) ? 0 : r["CatId"]),
                        IdCoQuan = (int)((r["IdCoQuan"] == System.DBNull.Value) ? 0 : r["IdCoQuan"]),
                        PublishUp = (DateTime)((r["PublishUp"] == System.DBNull.Value) ? null : r["PublishUp"]),
                        PublishUpShow = (string)((r["PublishUp"] == System.DBNull.Value) ? DateTime.Now.ToString("dd/MM/yyyy") : (string)((DateTime)r["PublishUp"]).ToString("dd/MM/yyyy")),
                        PublishDown = (DateTime)((r["PublishDown"] == System.DBNull.Value) ? null : r["PublishDown"]),
                        PublishDownShow = (string)((r["PublishDown"] == System.DBNull.Value) ? DateTime.Now.ToString("dd/MM/yyyy") : (string)((DateTime)r["PublishDown"]).ToString("dd/MM/yyyy")),
                        Ids = MyModels.Encode((int)r["Id"], SecretId),
                    }).FirstOrDefault();

            if (Item != null)
            {
                if (Item.Str_ListFile != null && Item.Str_ListFile != "")
                {
                    Item.ListFile = JsonConvert.DeserializeObject<List<FileArticle>>(Item.Str_ListFile);
                }

            }
            else
            {
                Item = new HopTrucTuyen() { Code = "" };
            }
            return Item;

        }
        public static HopTrucTuyen GetItem(int Id, string SecretId = null)
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_HopTrucTuyen",
            new string[] { "@flag", "@Id" }, new object[] { "GetItem", Id });

            HopTrucTuyen Item = (from r in tabl.AsEnumerable()
                    select new HopTrucTuyen
                    {
                        Id = (int)r["Id"],
                        Code = (string)r["Code"],
                        Title = (string)r["Title"],
                        Status = (bool)r["Status"],                        
                        Description = (string)((r["Description"] == System.DBNull.Value) ? "" : r["Description"]),
                        Str_ListFile = (string)((r["Str_ListFile"] == System.DBNull.Value) ? "" : r["Str_ListFile"]),
                        FullText = (string)((r["FullText"] == System.DBNull.Value) ? "" : r["FullText"]),
                        TenCoQuan = (string)((r["TenCoQuan"] == System.DBNull.Value) ? "" : r["TenCoQuan"]),
                        Alias = (string)((r["Alias"] == System.DBNull.Value) ? "" : r["Alias"]),
                        Images = (string)((r["Images"] == System.DBNull.Value) ? "" : r["Images"]),
                        Link = (string)((r["Link"] == System.DBNull.Value) ? "" : r["Link"]),
                        CatId = (int)((r["CatId"] == System.DBNull.Value) ? 0 : r["CatId"]),
                        IdCoQuan = (int)((r["IdCoQuan"] == System.DBNull.Value) ? 0 : r["IdCoQuan"]),
                        PublishUp = (DateTime)((r["PublishUp"] == System.DBNull.Value) ? null : r["PublishUp"]),
                        PublishUpShow = (string)((r["PublishUp"] == System.DBNull.Value) ? DateTime.Now.ToString("dd/MM/yyyy") : (string)((DateTime)r["PublishUp"]).ToString("dd/MM/yyyy")),
                        PublishDown = (DateTime)((r["PublishDown"] == System.DBNull.Value) ? null : r["PublishDown"]),
                        PublishDownShow = (string)((r["PublishDown"] == System.DBNull.Value) ? DateTime.Now.ToString("dd/MM/yyyy") : (string)((DateTime)r["PublishDown"]).ToString("dd/MM/yyyy")),
                        Ids = MyModels.Encode((int)r["Id"], SecretId),
                    }).FirstOrDefault();

            if (Item != null)
            {
                if (Item.Str_ListFile != null && Item.Str_ListFile != "")
                {
                    Item.ListFile = JsonConvert.DeserializeObject<List<FileArticle>>(Item.Str_ListFile);
                }
               
            }
            return Item;

        }

        public static List<SelectListItem> GetListCat(Boolean Selected = true)
        {
            List<SelectListItem> ListItems = new List<SelectListItem>();
            ListItems.Insert(0, (new SelectListItem { Text = "Google Meet", Value = "1" }));
            ListItems.Insert(1, (new SelectListItem { Text = "Zoom", Value = "2" }));
            return ListItems;
        }

        public static ResultSave SaveItem(HopTrucTuyen dto,string DomainFolderUpload,string DomainName)
        {
            string Str_ListFile = null;
            List<FileArticle> ListFileArticle = new List<FileArticle>();
            if (dto.ListFile != null && dto.ListFile.Count() > 0)
            {
                for (int i = 0; i < dto.ListFile.Count(); i++)
                {
                    if (dto.ListFile[i].FilePath != null && dto.ListFile[i].FilePath.Trim() != "")
                    {
                        ListFileArticle.Add(dto.ListFile[i]);
                    }
                }
                if (ListFileArticle != null && ListFileArticle.Count() > 0)
                {
                    Str_ListFile = JsonConvert.SerializeObject(ListFileArticle);
                }

            }

            dto.Alias = API.Models.MyHelper.StringHelper.UrlFriendly(dto.Title);

            if (dto.Id == 0)
            {
                dto.Code = API.Models.MyHelper.StringHelper.GetUniqueKey(10);
            }

            DateTime PublishUp = DateTime.ParseExact(dto.PublishUpShow, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime PublishDown = DateTime.ParseExact(dto.PublishDownShow, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_HopTrucTuyen",
            new string[] { "@flag", "@Id", "@Title", "@Code", "@Alias", "@Link", "@Status", "@Description", "@FullText", "@CreatedBy", "@ModifiedBy", "@CatId", "@Str_ListFile", "@PublishUp", "@PublishDown", "@Images", "@IdCoQuan" },
            new object[] { "SaveItem", dto.Id, dto.Title,dto.Code,dto.Alias,dto.Link, dto.Status, dto.Description,dto.FullText, dto.CreatedBy, dto.ModifiedBy,dto.CatId, Str_ListFile , PublishUp , PublishDown, dto.Images ,dto.IdCoQuan});
            ResultSave rs = (from r in tabl.AsEnumerable()
                    select new ResultSave
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

            

            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(DomainName + "/hop-truc-tuyen/tai-lieu.html?Id=" + rs.N + "&Code=" + dto.Code, QRCodeGenerator.ECCLevel.Q);

            PngByteQRCode qrCode = new PngByteQRCode(qrCodeData);


            string destFile = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "qrcode_hoptructuyen/" + DomainFolderUpload + "/" + "qr_htt_" + rs.N + "_" + dto.Code + ".png");

            if (dto.Id > 0)
            {
                File.Delete(destFile);
            }

            byte[] qrCodeBytes = qrCode.GetGraphic(20);
            File.WriteAllBytes(destFile, qrCodeBytes);


            return rs;

        }

        public static dynamic UpdateStatus(HopTrucTuyen dto)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_HopTrucTuyen",
            new string[] { "@flag", "@Id", "@Status", "@ModifiedBy" },
            new object[] { "UpdateStatus", dto.Id, dto.Status, dto.ModifiedBy });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }

        public static dynamic DeleteItem(HopTrucTuyen dto)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_HopTrucTuyen",
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
