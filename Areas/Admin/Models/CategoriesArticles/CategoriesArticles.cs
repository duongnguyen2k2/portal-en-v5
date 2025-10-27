using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using API.Areas.Admin.Models.Partial;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace API.Areas.Admin.Models.CategoriesArticles
{
    public class CategoriesArticles
    {
		public string? Ids { get; set; }
        public int TotalRows { get; set; }
        public int Id { get; set; }
        [Display(Name = "Tên")]
        [StringLength(250, MinimumLength = 3, ErrorMessage = "Độ dài chuỗi phải lớn hơn {2} Không quá {1} ký tự")]
        [Required(ErrorMessage = "Thể loại tin tức không được để trống")]
        public string? Title { get; set; }
        public string? Title_EN { get; set; }
        public string? TitleRoot { get; set; }
 		public string? Alias { get; set; }
 		public string? Description { get; set; }
 		public string? Icon { get; set; }
 		public string? LinkSiteRoot { get; set; }
 		public int? ParentId { get; set; }
 		public Boolean Status { get; set; }
 		public Boolean FeaturedHome { get; set; }
        public Boolean Layout { get; set; }
        public Boolean Deleted { get; set; }
 		
 		public int CreatedBy { get; set; }
 		public int PageEnd { get; set; }
 		public int PageStart { get; set; }
 		public DateTime? CreatedDate { get; set; }
 		public int? ModifiedBy { get; set; }
 		public DateTime? ModifiedDate { get; set; }
 		public string? Metadesc { get; set; }
 		public string? Metakey { get; set; }
 		public string? Metadata { get; set; }
        
        public string? Images { get; set; }
        public string? Params { get; set; }
        public int Ordering { get; set; }
        public int OrderingHome { get; set; }
        public int? Hits { get; set; }
        public int IdCoQuan { get; set; }
        public string? Culture { get; set; }
        public string? Link { get; set; }

        public API.Models.MetaData MetadataCV { get; set; } = new API.Models.MetaData() { };

    }
	
	public class CategoriesArticlesModel {
        public List<CategoriesArticles> ListItems { get; set; } = new List<CategoriesArticles>();
        public List<SelectListItem> ListItemsDanhMuc { get; set; } = new List<SelectListItem>();
        public SearchCategoriesArticles SearchData { get; set; } = new SearchCategoriesArticles() { };
        public CategoriesArticles Item { get; set; } = new CategoriesArticles() { };
        public PartialPagination Pagination { get; set; } = new PartialPagination() { };
    }

    public class SearchCategoriesArticles {
        public int CurrentPage { get; set; }
        public int ItemsPerPage { get; set; }
        public int IdCoQuan { get; set; }
        public string? Keyword { get; set; }
    }

    public class MetaData
    {
        public string? MetaTitle { get; set; } = "";
        public string? MetaH1 { get; set; } = "";
        public string? MetaH3 { get; set; } = "";

    }
}
