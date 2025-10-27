using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using API.Areas.Admin.Models.Articles;
using API.Areas.Admin.Models.Documents;
using API.Areas.Admin.Models.TaiLieuHop;
using API.Models;
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.InkML;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using QRCoder;

namespace API.Areas.Admin.Models.TaiLieuHop
{
    public class TaiLieuHopService
    {
        public static List<TaiLieuHop> GetListPagination(SearchTaiLieuHop dto, string SecretId)
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


            string str_sql = "GetListPagination";
            if (dto.Status != -1)
            {
                str_sql = "GetListPagination_Status";
            }

            string StartDate = DateTime.ParseExact(dto.ShowStartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyyMMdd");
            string EndDate = DateTime.ParseExact(dto.ShowEndDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyyMMdd");

            var tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_TaiLieuHop",
                new string[] { "@flag", "@CurrentPage", "@ItemsPerPage", "@Keyword","@StartDate", "@EndDate","@CatId", "@IdCoQuan" },
                new object[] { str_sql, dto.CurrentPage, dto.ItemsPerPage, dto.Keyword, StartDate, EndDate,dto.CatId,dto.IdCoQuan });
            if (tabl == null)
            {
                return new List<TaiLieuHop>();
            }
            else
            {
                return (from r in tabl.AsEnumerable()
                        select new TaiLieuHop
                        {
                            Id = (int)r["Id"],
                            CatId= (int)r["CatId"],
                            Title = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
                            Alias = (string)((r["Alias"] == System.DBNull.Value) ? null : r["Alias"]),                           
                            Link = (string)((r["Link"] == System.DBNull.Value) ? null : r["Link"]),
                            Link2 = (string)((r["Link2"] == System.DBNull.Value) ? null : r["Link2"]),
                            Introtext = (string)((r["Introtext"] == System.DBNull.Value) ? null : r["Introtext"]),
                            FullText = (string)((r["FullText"] == System.DBNull.Value) ? null : r["FullText"]),
                            MatKhau = (string)((r["MatKhau"] == System.DBNull.Value) ? null : r["MatKhau"]),
                            Code = (string)((r["Code"] == System.DBNull.Value) ? null : r["Code"]),
                            Status = (Boolean)r["Status"],
                            IssuedDate = (DateTime)((r["IssuedDate"] == System.DBNull.Value) ? DateTime.Now : r["IssuedDate"]), 
                            Author = (string)((r["Author"] == System.DBNull.Value) ? null : r["Author"]),
                            Ids = MyModels.Encode((int)r["Id"], SecretId),
                            TotalRows = (int)r["TotalRows"],
                        }).ToList();
            }


        }

        public static List<SelectListItem> GetListItemsStatus()
        {
            List<SelectListItem> ListItems = new List<SelectListItem>();
            ListItems.Insert(0, (new SelectListItem { Text = "--- Trạng Thái ---", Value = "-1" }));
            ListItems.Insert(1, (new SelectListItem { Text = "Ẩn", Value = "0" }));
            ListItems.Insert(2, (new SelectListItem { Text = "Hiện", Value = "1" }));
            return ListItems;
        }

        public static TaiLieuHop GetItemFE(decimal Id,string MatKhau, string Code, int IdCoQuan)
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_TaiLieuHop",
            new string[] { "@flag", "@Id", "@MatKhau", "@Code", "@IdCoQuan" }, new object[] { "GetItemFE", Id, MatKhau,Code, IdCoQuan });
            TaiLieuHop Item= (from r in tabl.AsEnumerable()
                    select new TaiLieuHop
                    {
                        Id = (int)r["Id"],
                        CatId = (int)r["CatId"],
                        Title = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
                        Alias = (string)((r["Alias"] == System.DBNull.Value) ? null : r["Alias"]),
                        Link = (string)((r["Link"] == System.DBNull.Value) ? null : r["Link"]),
                        Link2 = (string)((r["Link2"] == System.DBNull.Value) ? null : r["Link2"]),
                        MatKhau = (string)((r["MatKhau"] == System.DBNull.Value) ? null : r["MatKhau"]),
                        Code = (string)((r["Code"] == System.DBNull.Value) ? null : r["Code"]),
                        Introtext = (string)((r["Introtext"] == System.DBNull.Value) ? null : r["Introtext"]),
                        FullText = (string)((r["FullText"] == System.DBNull.Value) ? null : r["FullText"]),
                        Str_ListFile = (string)((r["Str_ListFile"] == System.DBNull.Value) ? null : r["Str_ListFile"]),
                        Status = (Boolean)r["Status"],
                        IssuedDate = (DateTime)((r["IssuedDate"] == System.DBNull.Value) ? DateTime.Now : r["IssuedDate"]),
                        IssuedDateShow = (string)((r["IssuedDate"] == System.DBNull.Value) ? DateTime.Now.ToString("dd/MM/yyyy") : (string)((DateTime)r["IssuedDate"]).ToString("dd/MM/yyyy")),
                        Author = (string)((r["Author"] == System.DBNull.Value) ? null : r["Author"])                        
                    }).FirstOrDefault();

            if (Item != null)
            {
                if (Item.Str_ListFile != null && Item.Str_ListFile != "")
                {
                    Item.ListFile = JsonConvert.DeserializeObject<List<FileDocuments>>(Item.Str_ListFile);
                }

            }
            return Item;
        }

        public static TaiLieuHop GetItem(decimal Id, int IdCoQuan, string SecretId = null)
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_TaiLieuHop",
            new string[] { "@flag", "@Id", "@IdCoQuan" }, new object[] { "GetItem", Id, IdCoQuan });
            TaiLieuHop Item= (from r in tabl.AsEnumerable()
                    select new TaiLieuHop
                    {
                        Id = (int)r["Id"],
                        CatId = (int)r["CatId"],
                        Title = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
                        Alias = (string)((r["Alias"] == System.DBNull.Value) ? null : r["Alias"]),                      
                        Link = (string)((r["Link"] == System.DBNull.Value) ? null : r["Link"]),
                        Link2 = (string)((r["Link2"] == System.DBNull.Value) ? null : r["Link2"]),
                        MatKhau = (string)((r["MatKhau"] == System.DBNull.Value) ? null : r["MatKhau"]),
                        Code = (string)((r["Code"] == System.DBNull.Value) ? null : r["Code"]),
                        Introtext = (string)((r["Introtext"] == System.DBNull.Value) ? null : r["Introtext"]),
                        FullText = (string)((r["FullText"] == System.DBNull.Value) ? null : r["FullText"]),
                        Str_ListFile = (string)((r["Str_ListFile"] == System.DBNull.Value) ? null : r["Str_ListFile"]),                    
                        Status = (Boolean)r["Status"],
                        IssuedDate = (DateTime)((r["IssuedDate"] == System.DBNull.Value) ? DateTime.Now : r["IssuedDate"]),                    
                        IssuedDateShow = (string)((r["IssuedDate"] == System.DBNull.Value) ? DateTime.Now.ToString("dd/MM/yyyy") : (string)((DateTime)r["IssuedDate"]).ToString("dd/MM/yyyy")),  
                        Author = (string)((r["Author"] == System.DBNull.Value) ? null : r["Author"]),
                        Ids = MyModels.Encode((int)r["Id"], SecretId),
                    }).FirstOrDefault();

            if (Item != null)
            {
                if (Item.Str_ListFile != null && Item.Str_ListFile != "")
                {
                    Item.ListFile = JsonConvert.DeserializeObject<List<FileDocuments>>(Item.Str_ListFile);
                }
              
            }
            return Item;
        }

        public static dynamic SaveItem(TaiLieuHop dto, string Domain="")
        {
            dto.Title = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.Title);
            dto.Code = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.Code);
            dto.Author = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.Author);
            dto.Link = API.Models.MyHelper.StringHelper.RemoveTagsLink(dto.Link);
            dto.Link2 = API.Models.MyHelper.StringHelper.RemoveTagsLink(dto.Link2);
            dto.Introtext = API.Models.MyHelper.StringHelper.RemoveTagsIntroText(dto.Introtext);

            if (dto.Alias == null || dto.Alias == "")
            {
                dto.Alias = API.Models.MyHelper.StringHelper.UrlFriendly(dto.Title);
            }
            DateTime IssuedDate = DateTime.ParseExact(dto.IssuedDateShow, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            string Str_ListFile = null;

            List<FileDocuments> ListFileArticle = new List<FileDocuments>();


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


            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_TaiLieuHop",
            new string[] { "@flag", "@Id", "@Title", "@MatKhau", "@Code", "@Link", "@Alias", "@Introtext","@Status", "@IdCoQuan", "@CreatedBy", "@ModifiedBy", "@IssuedDate", "@Author","@Link2","@CatId", "@Str_ListFile" },
            new object[] { "SaveItem", dto.Id, dto.Title,dto.MatKhau,dto.Code, dto.Link, dto.Alias, dto.Introtext,dto.Status,dto.IdCoQuan, dto.CreatedBy, dto.ModifiedBy, IssuedDate,dto.Author,dto.Link2,dto.CatId, Str_ListFile });
            ResultSave rs = (from r in tabl.AsEnumerable()
                             select new ResultSave
                             {
                                 Id = (int)(r["N"]),
                             }).FirstOrDefault();


            if (dto.Id == 0)
            {
                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                               

                string link = Domain + "/Home/TaiLieuHop?Id=" + @rs.Id + "&MatKhau=" + dto.MatKhau+"&Code="+dto.Code;
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(link, QRCodeGenerator.ECCLevel.Q);

                PngByteQRCode qrCode = new PngByteQRCode(qrCodeData);

                string destFile = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "qrcode_hoptructuyen/" + "qr_tlh_" + rs.Id + ".png");

                byte[] qrCodeBytes = qrCode.GetGraphic(20);
                File.WriteAllBytes(destFile, qrCodeBytes);
            }
            return rs;

        }
        public static dynamic DeleteItem(TaiLieuHop dto)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_TaiLieuHop",
            new string[] { "@flag", "@Id", "@ModifiedBy", "@IdCoQuan" },
            new object[] { "DeleteItem", dto.Id, dto.ModifiedBy ,dto.IdCoQuan });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }

        public static dynamic UpdateStatus(TaiLieuHop dto)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_TaiLieuHop",
            new string[] { "@flag", "@Id", "@Status", "@ModifiedBy", "@IdCoQuan" },
            new object[] { "UpdateStatus", dto.Id, dto.Status, dto.ModifiedBy ,dto.IdCoQuan });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }


    }
}
