using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using API.Areas.Admin.Models.Partial;
using Microsoft.AspNetCore.Mvc.Rendering;
/*https://docs.microsoft.com/en-us/aspnet/core/tutorials/razor-pages/validation?view=aspnetcore-2.2*/
namespace API.Areas.Admin.Models.DMPhongBan
{
    public class DMPhongBan
    {
        public int Id { get; set; }

        [Display(Name = "Tên")]
        [StringLength(550, MinimumLength = 3, ErrorMessage= "Độ dài chuỗi phải lớn hơn {2} Không quá {1} ký tự")]
        [Required(ErrorMessage = "Tên không được để trống")]
        public string? Title { get; set; }
        public string? Alias { get; set; }
        public string? Description { get; set; }        
        public Boolean Status { get; set; }        
        public string? Ids { get; set; }
        public int TotalRows { get; set; } = 0;
        public int CreatedBy { get; set; } = 0;
        public int ModifiedBy { get; set; } = 0;
        public int IdCoQuan { get; set; } = 0;
        public int IdType { get; set; } = 0; // Loại phòng ban
		public string? TypeTitle { get; set; }
        public Boolean Featured { get; set; }
        public string? Images { get; set; }        
        public string? Address { get; set; }
        public string? Telephone { get; set; }
        public string? Fax { get; set; }
        public string? Email { get; set; }
		public string? Youtube { get; set; }
		public string? Facebook { get; set; }
		public string? Link { get; set; }
        public int Ordering { get; set; } = 999;
    }

    public class DMPhongBanModel {
        public List<DMPhongBan> ListItems { get; set; } = new List<DMPhongBan>();
        public SearchDMPhongBan SearchData { get; set; } = new SearchDMPhongBan() { };
        public DMPhongBan Item { get; set; } = new DMPhongBan() { };
        public PartialPagination Pagination { get; set; } = new PartialPagination() { };
        public List<SelectListItem> ListItemsDanhMuc { get; set; } = new List<SelectListItem>() { };
    }
    public class SearchDMPhongBan {
        public int CurrentPage { get; set; }
        public int ItemsPerPage { get; set; }
        public int IdType { get; set; }
        public int TotalItems { get; set; }
        public int IdCoQuan { get; set; }
        public Boolean Status { get; set; }
        public string? Keyword { get; set; }
    }
}
