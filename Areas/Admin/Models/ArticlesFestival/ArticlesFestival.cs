using API.Areas.Admin.Models.Partial;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
/*https://docs.microsoft.com/en-us/aspnet/core/tutorials/razor-pages/validation?view=aspnetcore-2.2*/
namespace API.Areas.Admin.Models.ArticlesFestival
{
	public class ArticlesFestival
	{
		public int Id { get; set; }
		[Display(Name = "Tên bài viết")]
		[StringLength(60, MinimumLength = 3, ErrorMessage = "Độ dài chuỗi phải lớn hơn {2} Không quá {1} ký tự")]
		[Required(ErrorMessage = "Tên bài viết không được để trống")]
		public string? Title { get; set; }
		public string? Alias { get; set; }
		public string? Introtext { get; set; }
		public string? Description { get; set; }
		public string? AuthorName { get; set; }
		public DateTime PublishUp { get; set; } = DateTime.Now;
		public string? PublishUpShow { get; set; } = DateTime.Now.ToString("dd/MM/yyyy");
		public string? Image { get; set; }
		public List<ImageFile> ListImage { get; set; }
		public string? Str_ListImage { get; set; }
		public string? Metadesc { get; set; }
		public string? Metakey { get; set; }
		public string? Metadata { get; set; }
		public int IdCoQuan { get; set; } = 0;
		public string? TenCoQuan { get; set; }
		public int ImageCount { get; set; }
		public Boolean Status { get; set; }
		public int CreatedBy { get; set; } = 0;
		public int ModifiedBy { get; set; } = 0;
		public string? Ids { get; set; }
		public int TotalRows { get; set; } = 0;
	}

	public class ArticlesFestivalModel
	{
		public ArticlesFestival Item { get; set; } = new ArticlesFestival() { };
		public List<ArticlesFestival> ListItems { get; set; } = new List<ArticlesFestival>();
        public SearchArticlesFestival SearchData { get; set; } = new SearchArticlesFestival() { };
        public PartialPagination Pagination { get; set; } = new PartialPagination() { };
	}
	public class ImageFile
	{
		public string? Title { get; set; }
		public int Ordering { get; set; }
		public string? ImagePath { get; set; }
	}


	public class SearchArticlesFestival
	{
		public string? Keyword { get; set; }
		public int CurrentPage { get; set; }
		public int ItemsPerPage { get; set; }
		public int Status { get; set; } = -1;
		public string? KeywordMobile { get; set; }
	}
}
