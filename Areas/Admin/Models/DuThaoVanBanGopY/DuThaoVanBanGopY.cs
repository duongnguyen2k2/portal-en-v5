using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using API.Areas.Admin.Models.Articles;
using API.Areas.Admin.Models.Partial;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace API.Areas.Admin.Models.DuThaoVanBanGopY
{
    public class DuThaoVanBanGopY
    {
		public string? Ids { get; set; }
        public int TotalRows { get; set; }
        public int Id { get; set; }
       
        [StringLength(500, MinimumLength = 3, ErrorMessage = "Độ dài chuỗi phải lớn hơn {2} Không quá {1} ký tự")]
        [Required(ErrorMessage = "Tiêu đề không được để trống")]
        public string? Title { get; set; }
 		public Boolean Status { get; set; }
        [StringLength(250, MinimumLength = 3, ErrorMessage = "Độ dài chuỗi phải lớn hơn {2} Không quá {1} ký tự")]
        [Required(ErrorMessage = "Tên không được để trống")]
        public string? Fullname { get; set; }
        
        public string? IntrotextDuThao { get; set; }
        public string? Introtext { get; set; }
        [StringLength(500, MinimumLength = 3, ErrorMessage = "Độ dài chuỗi phải lớn hơn {2} Không quá {1} ký tự")]
        [Required(ErrorMessage = "Nội dung không được để trống")]
        public string? Description { get; set; }
 		public string? Phone { get; set; }
 		public string? Email { get; set; }
 		public string? Address { get; set; }
 		public Boolean Deleted { get; set; }
 		public int CreatedBy { get; set; } 		
 		public int IdDuThao { get; set; } 		
 		public DateTime CreatedDate { get; set; }
 		public string? CreatedDateShow { get; set; }
 		public int? ModifiedBy { get; set; }
 		public DateTime? ModifiedDate { get; set; }
        public List<FileArticle> ListFile { get; set; }        
        public string? Str_ListFile { get; set; }
        public string? IdsDuThao { get; set; }
        public string? Token { get; set; }

    }
	
	public class DuThaoVanBanGopYModel 
    {
        public List<DuThaoVanBanGopY> ListItems { get; set; } = new List<DuThaoVanBanGopY>();
        public SearchDuThaoVanBanGopY SearchData { get; set; } = new SearchDuThaoVanBanGopY() { };
        public DuThaoVanBanGopY Item { get; set; }  = new DuThaoVanBanGopY() { };
        public PartialPagination Pagination { get; set; } = new PartialPagination() { };
        
    }

    public class SearchDuThaoVanBanGopY {
        public int IdDuThao { get; set; }
        public string? IdsDuThao { get; set; }
        public int CurrentPage { get; set; }
        public int ItemsPerPage { get; set; }
        public string? Keyword { get; set; }
    }
}
