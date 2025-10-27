using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using API.Areas.Admin.Models.CoSoSXKD;
using API.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace API.Areas.Admin.Models.CoSoSXKD
{
    public class CoSoSXKDService
    {
        public static List<CoSoSXKD> GetListPagination(SearchCoSoSXKD dto, string SecretId)
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
            var tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_CoSoSXKD",
                new string[] { "@flag", "@CurrentPage", "@ItemsPerPage", "@Keyword","@Status","@IdTinhThanh","@IdQuanHuyen" ,"@IdPhuongXa", "@CatId" },
                new object[] { str_sql, dto.CurrentPage, dto.ItemsPerPage, dto.Keyword,Status,dto.IdTinhThanh,dto.IdQuanHuyen,dto.IdPhuongXa,dto.CatId });
            if (tabl == null)
            {
                return new List<CoSoSXKD>();
            }
            else
            {
                return (from r in tabl.AsEnumerable()
                        select new CoSoSXKD
                        {
                            Id = (int)r["Id"],
                            Title = (string)r["Title"],
                            Status = (bool)r["Status"],
                            CatTitle = (string)((r["CatTitle"] == System.DBNull.Value) ? "" : r["CatTitle"]),
                            IntroText = (string)((r["IntroText"] == System.DBNull.Value) ? "" : r["IntroText"]),
                            Latitude = (string)((r["Latitude"] == System.DBNull.Value) ? "" : r["Latitude"]),
                            Longitude = (string)((r["Longitude"] == System.DBNull.Value) ? "" : r["Longitude"]),
                            Address = (string)((r["Address"] == System.DBNull.Value) ? "" : r["Address"]),
                            IdPhuongXa = (int)((r["IdPhuongXa"] == System.DBNull.Value) ? 0 : r["IdPhuongXa"]),                            
                            Ids = MyModels.Encode((int)r["Id"], SecretId),
                            TotalRows = (int)r["TotalRows"],
                        }).ToList();
            }


        }

        public static List<SelectListItem> GetListSelectItemsCat(Boolean Selected = true)
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_CoSoSXKD",
                new string[] { "@flag", "@Selected" }, new object[] { "GetListCat", Convert.ToDecimal(Selected) });
            List<SelectListItem> ListItems = (from r in tabl.AsEnumerable()
                                              select new SelectListItem
                                              {
                                                  Value = (string)((r["Id"] == System.DBNull.Value) ? null : r["Id"].ToString()),
                                                  Text = (string)((r["Title"] == System.DBNull.Value) ? "" : r["Title"]),
                                              }).ToList();
            ListItems.Insert(0, (new SelectListItem { Text = "--- Chọn Loại ---", Value = "0" }));
            return ListItems;

        }

        public static DataTable GetList(Boolean Selected = true)
        {
            
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_CoSoSXKD",
                new string[] { "@flag", "@Selected" }, new object[] { "GetList", Selected });
            return tabl;

        }
     

        public static CoSoSXKD GetItem(int Id, string SecretId = null)
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_CoSoSXKD",
            new string[] { "@flag", "@Id" }, new object[] { "GetItem", Id });
            return (from r in tabl.AsEnumerable()
                    select new CoSoSXKD
                    {
                        Id = (int)r["Id"],
                        Title = (string)r["Title"],
                        Status = (bool)r["Status"],
                        IntroText = (string)((r["IntroText"] == System.DBNull.Value) ? "" : r["IntroText"]),
                        FullText = (string)((r["FullText"] == System.DBNull.Value) ? "" : r["FullText"]),
						Latitude = (string)((r["Latitude"] == System.DBNull.Value) ? "" : r["Latitude"]),
						Longitude = (string)((r["Longitude"] == System.DBNull.Value) ? "" : r["Longitude"]),
						Address = (string)((r["Address"] == System.DBNull.Value) ? "" : r["Address"]),
						IdPhuongXa = (int)((r["IdPhuongXa"] == System.DBNull.Value) ? 0 : r["IdPhuongXa"]),
						CatId = (int)((r["CatId"] == System.DBNull.Value) ? 0 : r["CatId"]),
						IdQuanHuyen = (int)((r["IdQuanHuyen"] == System.DBNull.Value) ? 0 : r["IdQuanHuyen"]),
                        Ids = MyModels.Encode((int)r["Id"], SecretId),
                    }).FirstOrDefault();
        }

        public static dynamic SaveItem(CoSoSXKD dto)
        {
            dto.Longitude = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.Longitude);
            dto.Latitude = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.Latitude);
            dto.Title = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.Title);
            dto.IntroText = API.Models.MyHelper.StringHelper.RemoveTagsIntroText(dto.IntroText);
            dto.Address = API.Models.MyHelper.StringHelper.RemoveTagsIntroText(dto.Address);
            dto.FullText = API.Models.MyHelper.StringHelper.RemoveTagsFullText(dto.FullText);

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_CoSoSXKD",
            new string[] { "@flag","@Id", "@Title", "@Status","@IntroText", "@FullText", "@IdPhuongXa","@Address","@Latitude","@Longitude","@CatId", "@CreatedBy", "@ModifiedBy" },
            new object[] { "SaveItem", dto.Id, dto.Title, dto.Status,dto.IntroText, dto.FullText, dto.IdPhuongXa,dto.Address,dto.Latitude,dto.Longitude,dto.CatId, dto.CreatedBy,dto.ModifiedBy });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }

        public static dynamic UpdateStatus(CoSoSXKD dto)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_CoSoSXKD",
            new string[] { "@flag", "@Id","@Status", "@ModifiedBy" },
            new object[] { "UpdateStatus", dto.Id,dto.Status, dto.ModifiedBy });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }

        public static dynamic DeleteItem(CoSoSXKD dto)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_CoSoSXKD",
            new string[] { "@flag", "@Id", "@ModifiedBy" },
            new object[] { "DeleteItem", dto.Id, dto.ModifiedBy});
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }


    }
}
