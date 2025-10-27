using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using API.Areas.Admin.Models.Partial;

using Microsoft.AspNetCore.Mvc.Rendering;

namespace API.Areas.Admin.Models.ArticleComment
{
    public class ArticleComment
    {
		public string? Ids { get; set; }
        public int TotalRows { get; set; }
        public int Id { get; set; }
 		public string? Title { get; set; }
 		public string? FullName { get; set; }
 		public string? Email { get; set; }
 		public string? Address { get; set; }
 		public string? Description { get; set; }
 		public string? IP { get; set; }
 		public string? ArticleTitle { get; set; }
 		public Boolean Status { get; set; }
 		public Boolean Deleted { get; set; }
 		public int CreatedBy { get; set; }
 		public DateTime CreatedDate { get; set; }
 		public int? ModifiedBy { get; set; }
 		public DateTime? ModifiedDate { get; set; } 			
 		public int ArticleId { get; set; } 		
 		public string? Image { get; set; }
 		public int IdCoQuan { get; set; }
 		public string? TenCoQuan { get; set; }
 		public string? Captcha { get; set; }
 		public string? ArticleIds { get; set; }
 		
    }
	
	public class ArticleCommentModel {
        public List<ArticleComment> ListItems { get; set; } = new List<ArticleComment>();
        public SearchArticleComment SearchData { get; set; } = new SearchArticleComment() { };
        public ArticleComment Item { get; set; } = new ArticleComment() { };
        public Articles.Articles ArticlesItem { get; set; } = new Articles.Articles() { };
        public ArticlesProduct.ArticlesProduct ArticlesProductItem { get; set; } = new ArticlesProduct.ArticlesProduct() { };
        public PartialPagination Pagination { get; set; } = new PartialPagination() { };
    }

    public class SearchArticleComment {
        public int CurrentPage { get; set; }
        public int ItemsPerPage { get; set; }
        public int ArticleId { get; set; }
        public string? ArticleIds { get; set; }
        public int IdCoQuan { get; set; } = 0;
        public string? Keyword { get; set; }
        public int Status { get; set; } = -1;
    }
}
