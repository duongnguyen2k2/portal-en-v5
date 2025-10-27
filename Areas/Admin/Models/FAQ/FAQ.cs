using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using API.Areas.Admin.Models.Partial;
/*https://docs.microsoft.com/en-us/aspnet/core/tutorials/razor-pages/validation?view=aspnetcore-2.2*/
namespace API.Areas.Admin.Models.FAQ
{
    public class FAQ
    {
        public int Id { get; set; }

        [Display(Name = "Tiêu đề")]
        [StringLength(250, MinimumLength = 3, ErrorMessage= "Độ dài chuỗi phải lớn hơn {2} Không quá {1} ký tự")]
        [Required(ErrorMessage = "Tiêu đề không được để trống")]
        public string? Title { get; set; }
        public string? Description { get; set; }        
        public Boolean Status { get; set; }        
        public string? Ids { get; set; }
        public int TotalRows { get; set; } = 0;
        public int CreatedBy { get; set; } = 0;
        public int ModifiedBy { get; set; } = 0;
    }

    public class FAQModel {
        public List<FAQ> ListItems { get; set; } = new List<FAQ>();
        public SearchFAQ SearchData { get; set; } = new SearchFAQ() { };
        public FAQ Item { get; set; } = new FAQ() { };
        public PartialPagination Pagination { get; set; } = new PartialPagination() { };
    }
    public class SearchFAQ {
        public int CurrentPage { get; set; }
        public int ItemsPerPage { get; set; }
        public string? Keyword { get; set; }
    }
}
