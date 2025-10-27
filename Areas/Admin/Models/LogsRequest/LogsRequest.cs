using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using API.Areas.Admin.Models.Partial;
namespace API.Areas.Admin.Models.LogsRequest
{
    public class LogsRequest
    {
		public string? Ids { get; set; }
        public int TotalRows { get; set; }
        public int Id { get; set; }
       
        [StringLength(500, MinimumLength = 3, ErrorMessage = "Độ dài chuỗi phải lớn hơn {2} Không quá {1} ký tự")]
        [Required(ErrorMessage = "Tiêu đề không được để trống")]
        public string? Title { get; set; } 	      
        public string? Introtext { get; set; }
 		public string? Description { get; set; }
        public int CreatedBy { get; set; } = 0;
 		public int IdCoQuan { get; set; } = 0;
 		public DateTime CreatedDate { get; set; } 		
 		public string? CreatedDateShow { get; set; } 		
 		public int? ModifiedBy { get; set; } 		
 		public DateTime? ModifiedDate { get; set; }
 		
    }
	
	public class LogsRequestModel {
        public List<LogsRequest> ListItems { get; set; }       
        public SearchLogsRequest SearchData { get; set; }
        public LogsRequest Item { get; set; }
        public PartialPagination Pagination { get; set; } = new PartialPagination() { };
    }

    public class SearchLogsRequest {
        public int CurrentPage { get; set; }
        public int ItemsPerPage { get; set; }
        public int IdCoQuan { get; set; }
        public int Status { get; set; } = -1;
        public string? Keyword { get; set; }
        public string? Cookie { get; set; }
    }
}
