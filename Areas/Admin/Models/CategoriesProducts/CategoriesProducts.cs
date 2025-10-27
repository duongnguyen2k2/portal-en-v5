using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using API.Areas.Admin.Models.Partial;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace API.Areas.Admin.Models.CategoriesProducts
{
    public class CategoriesProducts
    {
		public string? Ids { get; set; }
        public int TotalRows { get; set; }
        public int Id { get; set; }
        [Display(Name = "Tên")]
        [StringLength(500, MinimumLength = 3, ErrorMessage = "Độ dài chuỗi phải lớn hơn {2} Không quá {1} ký tự")]
        [Required(ErrorMessage = "Tên Thể loại sản phẩm không được để trống")]
        public string? Title { get; set; }
 		public string? Alias { get; set; }
 		public string? Code { get; set; }
        public string? Introtext { get; set; }
        public string? Description { get; set; }
 		public int? ParentId { get; set; }
 		public Boolean Status { get; set; }
 		public Boolean Deleted { get; set; } 		
 		public Boolean Featured { get; set; } 		
 		public int CreatedBy { get; set; }
 		public DateTime? CreatedDate { get; set; }
 		public int ModifiedBy { get; set; }
 		public DateTime ModifiedDate { get; set; }
 		public string? Metadesc { get; set; }
 		public string? Metakey { get; set; }
 		public string? Metadata { get; set; }
        public API.Models.MetaData MetadataCV { get; set; }
        public IFormFile Icon { get; set; }
        public string? Images { get; set; }
        public string? Params { get; set; }
        public int Ordering { get; set; }
        public string? StrPermission { get; set; }
        public int Hits { get; set; }
        public int QuantityProducts { get; set; } = 0;
        public int PerCat { get; set; } = 0;
        public string? Msg { get; set; }
    }
	
	public class CategoriesProductsModel {
        public List<CategoriesProducts> ListItems { get; set; } = new List<CategoriesProducts>();
        public List<SelectListItem> ListItemsDanhMuc { get; set; } = new List<SelectListItem>() { };
        public SearchCategoriesProducts SearchData { get; set; } = new SearchCategoriesProducts() { };
        public CategoriesProducts Item { get; set; } = new CategoriesProducts() { };
        public PartialPagination Pagination { get; set; } = new PartialPagination() { };
    }

    public class SearchCategoriesProducts {
        public int CurrentPage { get; set; }
        public int ItemsPerPage { get; set; }
        public string? Keyword { get; set; }
    }

    public class CatGroup
    {
        public int IdCat { get; set; }
        public int IdGroup { get; set; }
        public string? GroupTitle { get; set; }
        public string? CatName { get; set; }
        public string? IdsCat { get; set; }
        public string? IdsGroup { get; set; }
        public Boolean Selected { get; set; }
        public Boolean Status { get; set; }
    }
}
