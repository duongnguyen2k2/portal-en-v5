using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using API.Areas.Admin.Models.Partial;
using Microsoft.AspNetCore.Mvc.Rendering;
/*https://docs.microsoft.com/en-us/aspnet/core/tutorials/razor-pages/validation?view=aspnetcore-2.2*/
namespace API.Areas.Admin.Models.CoSoSXKD
{
    public class CoSoSXKD
    {
        public int Id { get; set; }

        [Display(Name = "Tên")]
        [StringLength(60, MinimumLength = 3, ErrorMessage= "Độ dài chuỗi phải lớn hơn {2} Không quá {1} ký tự")]
        [Required(ErrorMessage = "Tên CS SXKD không được để trống")]
        public string? Title { get; set; }
        public string? IntroText { get; set; }        
        public string? FullText { get; set; }        
        public string? Address { get; set; }        
        public string? Latitude { get; set; }        
        public string? Longitude { get; set; }   
		public int IdPhuongXa { get; set; } = 0;
		public int IdQuanHuyen { get; set; } = 0;
        public Boolean Status { get; set; }        
        public string? Ids { get; set; }
        public string? DonViChuQuan { get; set; }
        public string? DiaChi_DonViChuQuan { get; set; }
        public string? CatTitle { get; set; }
        public string? MaHuyen { get; set; }
        public string? SoCCHN { get; set; }
        public string? Phone { get; set; }
        
        public int TotalRows { get; set; } = 0;
        public int CreatedBy { get; set; } = 0;
        public int ModifiedBy { get; set; } = 0;
        public int CatId { get; set; } = 0;
    }

    public class CoSoSXKDModel {
        public List<SelectListItem> ListItemsTinhThanh { get; set; } = new List<SelectListItem>() { };
        public List<SelectListItem> ListItemsDMQuanHuyen { get; set; } = new List<SelectListItem>() { };
        public List<SelectListItem> ListItemsDMPhuongXa { get; set; } = new List<SelectListItem>() { };
        public List<SelectListItem> ListCat { get; set; } = new List<SelectListItem>() { };
        public List<CoSoSXKD> ListItems { get; set; } = new List<CoSoSXKD>();
        public SearchCoSoSXKD SearchData { get; set; } = new SearchCoSoSXKD() { };
        public CoSoSXKD Item { get; set; } = new CoSoSXKD() { };
        public PartialPagination Pagination { get; set; } = new PartialPagination() { };
    }
    public class SearchCoSoSXKD {
        public int CurrentPage { get; set; }
        public int ItemsPerPage { get; set; }
        public int Status { get; set; }
        public int IdTinhThanh { get; set; }
        public int IdQuanHuyen { get; set; }
        public int IdPhuongXa { get; set; }
        public int CatId { get; set; }
        public string? Keyword { get; set; }
    }
}
