using API.Areas.Admin.Models.Partial;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace API.Areas.Admin.Models.CategoriesArticlesProduct
{
	public class CategoriesArticlesProduct
	{
		public string? Ids { get; set; }
		public int TotalRows { get; set; }
		public int Id { get; set; }
		[Display(Name = "Tên")]
		[StringLength(250, MinimumLength = 3, ErrorMessage = "Độ dài chuỗi phải lớn hơn {2} Không quá {1} ký tự")]
		[Required(ErrorMessage = "Thể loại tin tức không được để trống")]
		public string? Title { get; set; }
		public string? TitleRoot { get; set; }
		public string? Alias { get; set; }
		public string? Description { get; set; }
		public string? Icon { get; set; }
		public int? IdParent { get; set; }
		public Boolean Status { get; set; }
		public Boolean FeaturedHome { get; set; }
		public Boolean Deleted { get; set; }

		public int CreatedBy { get; set; }
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

	}

	public class CategoriesArticlesProductMobile
	{
		public int Id { get; set; }
		public string? Title { get; set; }
		public string? Alias { get; set; }
		public string? Icon { get; set; }
		public string? Description { get; set; }
		public string? Images { get; set; }
		public int Ordering { get; set; }
		public string? Ids { get; set; }
		public int TotalRows { get; set; }
	}

	public class CategoriesArticlesProductModel
	{
		public List<CategoriesArticlesProduct> ListItems { get; set; } = new List<CategoriesArticlesProduct>();
		public List<SelectListItem> ListItemsDanhMuc { get; set; } = new List<SelectListItem>();
		public SearchCategoriesArticlesProduct SearchData { get; set; } = new SearchCategoriesArticlesProduct() { };
		public CategoriesArticlesProduct Item { get; set; } = new CategoriesArticlesProduct() { };
		public CategoriesArticlesProductMobile ItemMobile { get; set; } = new CategoriesArticlesProductMobile() { };
		public PartialPagination Pagination { get; set; } = new PartialPagination() { };
	}

	public class SearchCategoriesArticlesProduct
	{
		public int CurrentPage { get; set; }
		public int ItemsPerPage { get; set; }
		public int IdCoQuan { get; set; }
		public string? Keyword { get; set; }
	}
}
