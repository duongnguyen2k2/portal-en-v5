using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using API.Areas.Admin.Models.Partial;
namespace API.Areas.Admin.Models.LienHe
{
    public class LienHe
    {
		public string? Ids { get; set; }
        public int TotalRows { get; set; }
        public int Id { get; set; }
       
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Độ dài chuỗi phải lớn hơn {2} Không quá {1} ký tự")]
        [Required(ErrorMessage = "Tiêu đề không được để trống")]
        public string? Title { get; set; }
 		public Boolean Status { get; set; }
        [StringLength(500, MinimumLength = 3, ErrorMessage = "Độ dài chuỗi phải lớn hơn {2} Không quá {1} ký tự")]
        [Required(ErrorMessage = "Tên không được để trống")]
        public string? Fullname { get; set; }
        [Required(ErrorMessage = "Nội dung không được để trống")]
        public string? Introtext { get; set; }
 		public string? Description { get; set; }
 		public string? Phone { get; set; }
 		public string? Email { get; set; }
 		public string? Address { get; set; }
 		public Boolean Deleted { get; set; }
 		public int CreatedBy { get; set; }
 		public DateTime CreatedDate { get; set; }
        public string? CreatedDateShow { get; set; }
        public int? ModifiedBy { get; set; }
 		public DateTime? ModifiedDate { get; set; }
        public int Type { get; set; }
 		
        public int EventId { get; set; }
    }
	
	public class LienHeModel {
        public List<LienHe> ListItems { get; set; } = new List<LienHe>();
        public SearchLienHe SearchData { get; set; } = new SearchLienHe() { };
        public LienHe Item { get; set; } = new LienHe() { };
        public PartialPagination Pagination { get; set; } = new PartialPagination() { };
        public int eID { get; set; }
    }

    public class SearchLienHe {
        public int CurrentPage { get; set; }
        public int ItemsPerPage { get; set; }
        public string? Keyword { get; set; }
        public int Type { get; set; }
    }
}
