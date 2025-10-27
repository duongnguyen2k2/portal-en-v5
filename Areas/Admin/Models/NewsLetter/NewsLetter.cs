using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using API.Areas.Admin.Models.Partial;
namespace API.Areas.Admin.Models.NewsLetter
{
    public class NewsLetter
    {
		public string? Ids { get; set; }
        public int TotalRows { get; set; }
        public int Id { get; set; }
 		public string? Title { get; set; }
 		public Boolean Status { get; set; }
        [Required(ErrorMessage = "Email không được để trống")]
        [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
        public string? Email { get; set; }
 		public Boolean Deleted { get; set; }
 		public int CreatedBy { get; set; }
 		public DateTime CreatedDate { get; set; }
 		public int? ModifiedBy { get; set; }
 		public DateTime? ModifiedDate { get; set; }
 		
    }
	
	public class NewsLetterModel {
        public List<NewsLetter> ListItems { get; set; } = new List<NewsLetter>();       
        public SearchNewsLetter SearchData { get; set; } = new SearchNewsLetter() { };
        public NewsLetter Item { get; set; } = new NewsLetter() { };
        public PartialPagination Pagination { get; set; } = new PartialPagination() { };
    }

    public class SearchNewsLetter {
        public int CurrentPage { get; set; }
        public int ItemsPerPage { get; set; }
        public string? Keyword { get; set; }
    }
}
