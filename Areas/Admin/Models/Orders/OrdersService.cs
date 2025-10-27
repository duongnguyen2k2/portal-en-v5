using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API.Areas.Admin.Models.Orders;
using API.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace API.Areas.Admin.Models.Orders
{
    public class OrdersService
    {
       
        public static List<Orders> GetListPagination(SearchOrders dto, string SecretId)
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
            var tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Orders",
                new string[] { "@flag", "@CurrentPage", "@ItemsPerPage", "@Keyword" },
                new object[] { "GetListPagination", dto.CurrentPage, dto.ItemsPerPage, dto.Keyword });
            if (tabl == null)
            {
                return new List<Orders>();
            }
            else
            {
                return (from r in tabl.AsEnumerable()
                        select new Orders
                        {
                            Id = (int)r["Id"],
                            FullName = (string)r["FullName"],                          
                            Note = (string)((r["Note"] == System.DBNull.Value) ? "" : r["Note"]),
                            Address = (string)((r["Address"] == System.DBNull.Value) ? "" : r["Address"]),
                            Telephone = (string)((r["Telephone"] == System.DBNull.Value) ? "" : r["Telephone"]),
                            Email = (string)((r["Email"] == System.DBNull.Value) ? "" : r["Email"]),
                            StatusId = (int)((r["StatusId"] == System.DBNull.Value) ? 0 : r["StatusId"]),                          
                            Ids = MyModels.Encode((int)r["Id"], SecretId),
                            TotalRows = (int)r["TotalRows"],
                        }).ToList();
            }


        }

        public static DataTable GetList(Boolean Selected = true)
        {
            
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Orders",
                new string[] { "@flag", "@Selected" }, new object[] { "GetList", Selected });
            return tabl;

        }      

        public static Orders GetItem(int Id, string SecretId = null)
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Orders",
            new string[] { "@flag", "@Id" }, new object[] { "GetItem", Id });
            return (from r in tabl.AsEnumerable()
                    select new Orders
                    {
                        Id = (int)r["Id"],                        
                        Note = (string)((r["Note"] == System.DBNull.Value) ? "" : r["Note"]),                        
                        Address = (string)((r["Address"] == System.DBNull.Value) ? "" : r["Address"]),                        
                        Email = (string)((r["Email"] == System.DBNull.Value) ? "" : r["Email"]),                        
                        Telephone = (string)((r["Telephone"] == System.DBNull.Value) ? "" : r["Telephone"]),                        
                        FullName = (string)((r["FullName"] == System.DBNull.Value) ? "" : r["FullName"]),
                        Total = (double)((r["Total"] == System.DBNull.Value) ? 0: r["Total"]),
                        Amount = (double)((r["Amount"] == System.DBNull.Value) ? 0: r["Amount"]),
                        StatusId = (int)((r["StatusId"] == System.DBNull.Value) ? 0: r["StatusId"]),                         
                        Ids = MyModels.Encode((int)r["Id"], SecretId),
                    }).FirstOrDefault();
        }

        public static dynamic SaveItem(Orders dto, DataTable tbItem)
        {
            dto.Address = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.Address);
            dto.FullName = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.FullName);
            dto.Title = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.Title);
            dto.Description = API.Models.MyHelper.StringHelper.RemoveTagsIntroText(dto.Description);

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Orders",
            new string[] { "@flag","@Id", "@Title", "@Email", "@Telephone", "@FullName", "@Address", "@Note", "@Total", "@StatusId",  "@CreatedBy", "@ModifiedBy", "@TBL_OrdersProducts" },
            new object[] { "SaveItem", dto.Id, dto.FullName, dto.Email,dto.Telephone,dto.FullName,dto.Address,dto.Note, dto.Total, dto.StatusId, dto.CreatedBy,dto.ModifiedBy,tbItem });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }

        public static dynamic UpdateStatus(Orders dto)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Orders",
            new string[] { "@flag", "@Id", "@StatusId", "@ModifiedBy" },
            new object[] { "UpdateStatus", dto.Id,dto.StatusId, dto.ModifiedBy });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }

        public static dynamic DeleteItem(Orders dto)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Orders",
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
