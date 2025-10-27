using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using API.Areas.Admin.Models.CoSoSXKDNhatKy;
using API.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace API.Areas.Admin.Models.CoSoSXKDNhatKy
{
    public class CoSoSXKDNhatKyService
    {
        public static List<CoSoSXKDNhatKy> GetListPagination(SearchCoSoSXKDNhatKy dto, string SecretId)
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
            var tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_CoSoSXKDNhatKy",
                new string[] { "@flag", "@CurrentPage", "@ItemsPerPage", "@Keyword" ,"@IdCoSoSXKD"},
                new object[] { "GetListPagination", dto.CurrentPage, dto.ItemsPerPage, dto.Keyword , dto.IdCoSoSXKD});
            if (tabl == null)
            {
                return new List<CoSoSXKDNhatKy>();
            }
            else
            {
                return (from r in tabl.AsEnumerable()
                        select new CoSoSXKDNhatKy
                        {
                            Id = (int)r["Id"],
                            Title = (string)r["Title"],
                            Status = (bool)r["Status"],
                            FullText = (string)((r["FullText"] == System.DBNull.Value) ? "" : r["FullText"]),
                            IdCoSoSXKD = (int)((r["IdCoSoSXKD"] == System.DBNull.Value) ? 0 : r["IdCoSoSXKD"]),                            
                            Ids = MyModels.Encode((int)r["Id"], SecretId),
                            TotalRows = (int)r["TotalRows"],
                        }).ToList();
            }


        }

        public static DataTable GetList(Boolean Selected = true)
        {
            
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_CoSoSXKDNhatKy",
                new string[] { "@flag", "@Selected" }, new object[] { "GetList", Selected });
            return tabl;

        }

        public static List<SelectListItem> GetListSelectItems(Boolean Selected = true)
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_CoSoSXKDNhatKy",
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

        public static CoSoSXKDNhatKy GetItem(int Id, string SecretId = null)
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_CoSoSXKDNhatKy",
            new string[] { "@flag", "@Id" }, new object[] { "GetItem", Id });
            return (from r in tabl.AsEnumerable()
                    select new CoSoSXKDNhatKy
                    {
                        Id = (int)r["Id"],
                        Title = (string)r["Title"],
                        Status = (bool)r["Status"],
                        FullText = (string)((r["FullText"] == System.DBNull.Value) ? "" : r["FullText"]),
						IdCoSoSXKD = (int)((r["IdCoSoSXKD"] == System.DBNull.Value) ? 0 : r["IdCoSoSXKD"]),                         
                        Ids = MyModels.Encode((int)r["Id"], SecretId),
                    }).FirstOrDefault();
        }

        public static dynamic SaveItem(CoSoSXKDNhatKy dto)
        {
            dto.Title = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.Title);
            dto.FullText = API.Models.MyHelper.StringHelper.RemoveTagsFullText(dto.FullText);
            
            DateTime NgayDang = DateTime.ParseExact(dto.PublishUpShow, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_CoSoSXKDNhatKy",
            new string[] { "@flag","@Id", "@Title", "@Status", "@PublishUp", "@FullText", "@IdCoSoSXKD", "@CreatedBy", "@ModifiedBy" },
            new object[] { "SaveItem", dto.Id, dto.Title, dto.Status, NgayDang, dto.FullText, dto.IdCoSoSXKD,dto.CreatedBy,dto.ModifiedBy });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }

        public static dynamic UpdateStatus(CoSoSXKDNhatKy dto)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_CoSoSXKDNhatKy",
            new string[] { "@flag", "@Id","@Status", "@ModifiedBy" },
            new object[] { "UpdateStatus", dto.Id,dto.Status, dto.ModifiedBy });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }

        public static dynamic DeleteItem(CoSoSXKDNhatKy dto)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_CoSoSXKDNhatKy",
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
