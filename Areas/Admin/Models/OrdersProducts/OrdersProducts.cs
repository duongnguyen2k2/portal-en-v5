using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using API.Areas.Admin.Models.Partial;
/*https://docs.microsoft.com/en-us/aspnet/core/tutorials/razor-pages/validation?view=aspnetcore-2.2*/
namespace API.Areas.Admin.Models.OrdersProducts
{
    public class OrdersProducts
    {
        public int Id { get; set; }        
        public string? Title { get; set; }
        public string? ProductsTitle { get; set; }
        public double Total { get; set; }        
        public int ProductId { get; set; }        
        public string? Ids { get; set; }
        public int TotalRows { get; set; } = 0;
        public int CreatedBy { get; set; } = 0;
        public int ModifiedBy { get; set; } = 0;
    }

    public class OrdersProductsModel {
        public List<OrdersProducts> ListItems { get; set; } = new List<OrdersProducts>() { };       
        public SearchOrdersProducts SearchData { get; set; } = new SearchOrdersProducts() { };
        public OrdersProducts Item { get; set; } = new OrdersProducts() { };
        public PartialPagination Pagination { get; set; } = new PartialPagination() { };
    }
    public class SearchOrdersProducts {
        public int CurrentPage { get; set; }
        public int ItemsPerPage { get; set; }
        public string? Keyword { get; set; }
    }
}
