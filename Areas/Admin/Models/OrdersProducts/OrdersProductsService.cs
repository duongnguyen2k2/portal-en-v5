using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API.Areas.Admin.Models.OrdersProducts;
using API.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace API.Areas.Admin.Models.OrdersProducts
{
    public class OrdersProductsService
    {
       

        public static List<OrdersProducts> GetList(Boolean Selected = true)
        {
            
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_OrdersProducts",
                new string[] { "@flag", "@Selected" }, new object[] { "GetList", Selected });
            return (from r in tabl.AsEnumerable()
                    select new OrdersProducts
                    {
                        Id = (int)r["Id"],
                        Title = (string)r["Title"],
                        ProductId = (int)((r["ProductId"] == System.DBNull.Value) ? 0 : r["ProductId"]),
                        Total = (double)((r["Total"] == System.DBNull.Value) ? 0 : r["Total"]),                        
                        ProductsTitle = (string)((r["ProductsTitle"] == System.DBNull.Value) ? "" : r["ProductsTitle"]),                        
                    }).ToList();

        }

      
        public static OrdersProducts GetItem(int Id, string SecretId = null)
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_OrdersProducts",
            new string[] { "@flag", "@Id" }, new object[] { "GetItem", Id });
            return (from r in tabl.AsEnumerable()
                    select new OrdersProducts
                    {
                        Id = (int)r["Id"],
                        Title = (string)r["Title"],
                        ProductId = (int)((r["ProductId"] == System.DBNull.Value) ? 0 : r["ProductId"]),
                        Total = (double)((r["Total"] == System.DBNull.Value) ? 0 : r["Total"]),
                        Ids = MyModels.Encode((int)r["Id"], SecretId),
                    }).FirstOrDefault();
        }

        public static dynamic SaveItem(OrdersProducts dto)
        {
            dto.Title = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.Title);
            
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_OrdersProducts",
            new string[] { "@flag","@Id", "@Title", "@ProductId", "@Total",  "@CreatedBy", "@ModifiedBy" },
            new object[] { "SaveItem", dto.Id, dto.Title, dto.ProductId, dto.Total, dto.CreatedBy,dto.ModifiedBy });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }

      
        public static dynamic DeleteItem(OrdersProducts dto)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_OrdersProducts",
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
