using API.Areas.Admin.Models.Partial;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Areas.Admin.Models.Reviews
{
    public class Reviews
    {
        public string? Ids { get; set; }
        public int TotalRows { get; set; }
        public int Id { get; set; }
        [Display(Name = "Tiêu đề")]
        [StringLength(500, MinimumLength = 2, ErrorMessage = "Độ dài chuỗi phải lớn hơn {2} Không quá {1} ký tự")]
        [Required(ErrorMessage = "Tiêu đề không được để trống")]
        public string? Title { get; set; }
        public string? Description { get; set; }
        [Display(Name = "Tên")]
        [StringLength(1024, MinimumLength = 1, ErrorMessage = "Độ dài chuỗi phải lớn hơn {2} Không quá {1} ký tự")]
        [Required(ErrorMessage = "Tên không được để trống")]
        public string? FullName { get; set; }
        public Boolean Status { get; set; }
        public Boolean Deleted { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int Star { get; set; } = 5;
        public Boolean Featured { get; set; }
        public string? Introtext { get; set; }
        public DateTime ReviewDate { get; set; }
        public string? Image { get; set; }
        public int IdCoQuan { get; set; }
        public string? ReviewDateShow { get; set; } = DateTime.Now.ToString("dd/MM/yyyyy");

    }

    public class ReviewsModel
    {
        public List<Reviews> ListItems { get; set; } = new List<Reviews>();
        public List<SelectListItem> ListStar { get; set; } = new List<SelectListItem>();
        public SearchReviews SearchData { get; set; } = new SearchReviews() { };
        public Reviews Item { get; set; } = new Reviews() { };
        public PartialPagination Pagination { get; set; } = new PartialPagination() { };
    }

    public class SearchReviews
    {
        public int CurrentPage { get; set; }
        public int ItemsPerPage { get; set; }      
        public int IdCoQuan { get; set; }      
        public string? Keyword { get; set; }
    }
}
