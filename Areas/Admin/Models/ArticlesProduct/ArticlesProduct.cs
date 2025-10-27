using API.Areas.Admin.Models.Partial;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace API.Areas.Admin.Models.ArticlesProduct
{
	public class ArticlesProduct
	{
		public string? Ids { get; set; }
		public int Id { get; set; }
		[Display(Name = "Tên")]
		[StringLength(550, MinimumLength = 3, ErrorMessage = "Độ dài chuỗi phải lớn hơn {2} Không quá {1} ký tự")]
		[Required(ErrorMessage = "Tiêu đề không được để trống")]
		public string? Title { get; set; }
		public string? Alias { get; set; }
		[Range(1, Int32.MaxValue, ErrorMessage = "Thể loại bài viết sản phẩm không được để trống")]
		public int IdCatAP { get; set; }
		[Range(1, Int32.MaxValue, ErrorMessage = "Nhóm nông sản không được để trống")]
		public int IdCatProduct { get; set; }
		public int IdManufacturer { get; set; }
		public int IdProduct { get; set; }
		public string? IntroText { get; set; }
		public string? FullText { get; set; }
		public string? Images { get; set; }
		[Required(ErrorMessage = "Ngày đăng không được để trống")]
		public DateTime PublishUp { get; set; } = DateTime.Now;
		public string? PublishUpShow { get; set; } = DateTime.Now.ToString("dd/MM/yyyy");
		public string? AuthorName { get; set; }
		public int AuthorId { get; set; }
		public string? Author { get; set; }
		public string? Language { get; set; }
		public int? Ordering { get; set; }
		public string? Metadesc { get; set; }
		public string? Metakey { get; set; }
		public string? Metadata { get; set; }
		public List<FileArticle> ListFile { get; set; } = new List<FileArticle>() { };
        public List<LinkArticle> ListLinkArticle { get; set; } = new List<LinkArticle> { new LinkArticle() };
        public string? Str_ListFile { get; set; }
		public string? Str_Link { get; set; }
		public string? FileItem { get; set; }
		public int IdCoQuan { get; set; }
		public string? TenCoQuan { get; set; }
		public string? CodeCoQuan { get; set; }
		public string? CatAPName { get; set; }
		public string? CatProductName { get; set; }
		public string? ManufacturerName { get; set; }
		public int TotalRows { get; set; }

		public Boolean Status { get; set; }
		public int? CreatedBy { get; set; }
		public int? ModifiedBy { get; set; }
		public DateTime CreatedDate { get; set; }
		public DateTime? ModifiedDate { get; set; }
		public Boolean Deleted { get; set; }

		//public int LikeN { get; set; }
		//public Int64 STT { get; set; }
		//public string? Address { get; set; }
		//public string? CreatedByName { get; set; }
		//public string? CreatedByFullName { get; set; }
		//public Boolean Featured { get; set; }
		//public Boolean Notification { get; set; }
		//public Boolean FeaturedHome { get; set; }
		//public Boolean Comment { get; set; } = false;
		//public Boolean StaticPage { get; set; }
		//public string? Category { get; set; }
		//public string? Icon { get; set; }
		//public string? Params { get; set; }
		//public int Hit { get; set; }
		//public int RootNewsId { get; set; }
		//public int QuantityImage { get; set; }
		//public Double Money { get; set; }
		//public int IdLevel { get; set; } = 0;
		//public string? LevelTitle { get; set; }
		//public int CategoryTypeId { get; set; }
		//public string? CategoryTypeTitle { get; set; } = "";
		//public string? DataFile { get; set; } = "";
		//public string? LinkRoot { get; set; } = "";
		//public Boolean RootNewsFlag { get; set; }
		//public Boolean FlagEdit { get; set; } = true;
	}



	public class FileArticle
	{
		public string? FilePath { get; set; }
	}

	public class LinkArticle
	{
		public string? Title { get; set; }
		public string? Link { get; set; }
		public Boolean Status { get; set; }
	}
	public class ArticlesProductModel
	{
		public List<ArticlesProduct> ListItems { get; set; } = new List<ArticlesProduct>();
        public SearchArticlesProduct SearchData { get; set; } = new SearchArticlesProduct() { };
        public CategoriesArticles.CategoriesArticles Categories { get; set; } = null; 
		public List<SelectListItem> ListItemsDanhMuc { get; set; } = new List<SelectListItem>() { };
        public List<SelectListItem> ListItemsManufacturer { get; set; } = new List<SelectListItem>() { };
        public List<SelectListItem> ListItemCategoriesProduct { get; set; } = new List<SelectListItem>() { };
        // List Item Categories Articles Product
        public List<SelectListItem> ListItemCategoriesAP { get; set; } = new List<SelectListItem>() { };
        public ArticlesProduct Item { get; set; } = new ArticlesProduct() { };
        public PartialPagination Pagination { get; set; } = new PartialPagination() { }; 
		public List<SelectListItem> ListItemsAuthors { get; set; } = new List<SelectListItem>() { };
        public List<SelectListItem> ListItemsCreatedBy { get; set; } = new List<SelectListItem>() { };
		public List<SelectListItem> ListItemsStatus { get; set; } = new List<SelectListItem>() { };
		public List<SelectListItem> ListDMCoQuan { get; set; } = new List<SelectListItem>() { };
		public List<SelectListItem> ListCategoryType { get; set; } = new List<SelectListItem>() { };
		public List<SelectListItem> ListLevelArticlesProduct { get; set; }	 = new List<SelectListItem>() { };
    }

	public class SearchArticlesProduct
	{
		public int CurrentPage { get; set; }
		public int ItemsPerPage { get; set; }
		public int IdProduct { get; set; }
		public int IdCatProduct { get; set; }
		public int IdCatAP { get; set; }
		public int IdCoQuan { get; set; }
		public int AuthorId { get; set; }
		public int CreatedBy { get; set; }
		public int FlagStatus { get; set; }
		public string? Keyword { get; set; }
		public string? ShowStartDate { get; set; } = "01/01/" + (Int32.Parse(DateTime.Now.ToString("yyyy")) - 1).ToString();
		public string? ShowEndDate { get; set; } = "01/01/" + (Int32.Parse(DateTime.Now.ToString("yyyy")) + 1).ToString();
		public int Status { get; set; } = -1;
		public int Id { get; set; } = 0;
		public int IdManufacturer { get; set; } = 0;
		public string? Ids { get; set; }
	}
}
