using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using API.Areas.Admin.Models.Partial;
/*https://docs.microsoft.com/en-us/aspnet/core/tutorials/razor-pages/validation?view=aspnetcore-2.2*/
namespace API.Areas.Admin.Models.Orders
{
    public class Orders
    {
        public int Id { get; set; }

        
        public string? Title { get; set; }
        [Display(Name = "Tên")]
        [StringLength(60, MinimumLength = 3, ErrorMessage = "Độ dài chuỗi phải lớn hơn {2} Không quá {1} ký tự")]
        [Required(ErrorMessage = "Tên không được để trống")]
        public string? FullName { get; set; }
        [Display(Name = "Địa chỉ")]
        [StringLength(500, MinimumLength = 3, ErrorMessage = "Độ dài chuỗi phải lớn hơn {2} Không quá {1} ký tự")]
        [Required(ErrorMessage = "Địa chỉ không được để trống")]
        public string? Address { get; set; }
        [Display(Name = "Email")]
        [StringLength(60, MinimumLength = 3, ErrorMessage = "Độ dài chuỗi phải lớn hơn {2} Không quá {1} ký tự")]
        [Required(ErrorMessage = "Email không được để trống")]
        public string? Email { get; set; }
        public string? Telephone { get; set; }
        public double Total { get; set; }
        public int StatusId { get; set; }
        public string? Description { get; set; }        
        public string? Note { get; set; }        
         
        public string? Ids { get; set; }
        public int TotalRows { get; set; } = 0;
        public int CreatedBy { get; set; } = 0;
        public int ModifiedBy { get; set; } = 0;
        public Double Amount { get; set; } = 0;
        
    }

    public class OrdersModel {
        public List<Orders> ListItems { get; set; } = new List<Orders>();
        public SearchOrders SearchData { get; set; } = new SearchOrders() { };
        public Orders Item { get; set; } = new Orders() { };
        public PartialPagination Pagination { get; set; } = new PartialPagination() { };
        public List<API.Areas.Admin.Models.Products.Products> ListCart { get; set; } = new List<API.Areas.Admin.Models.Products.Products>();
    }
    public class SearchOrders {
        public int CurrentPage { get; set; }
        public int ItemsPerPage { get; set; }
        public string? Keyword { get; set; }
    }
}
