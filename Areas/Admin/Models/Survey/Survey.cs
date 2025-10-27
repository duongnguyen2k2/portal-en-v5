using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using API.Areas.Admin.Models.Partial;
/*https://docs.microsoft.com/en-us/aspnet/core/tutorials/razor-pages/validation?view=aspnetcore-2.2*/
namespace API.Areas.Admin.Models.Survey
{
    public class Survey
    {
        public int Id { get; set; }

        [Display(Name = "Tên")]
        [StringLength(1024, MinimumLength = 3, ErrorMessage= "Độ dài chuỗi phải lớn hơn {2} Không quá {1} ký tự")]
        [Required(ErrorMessage = "Nội dung khảo sát không được để trống")]
        public string? Title { get; set; }
        public string? Description { get; set; }        
        public Boolean Status { get; set; }
        public int IdCoQuan { get; set; }
        public string? Ids { get; set; }
        public int TotalRows { get; set; } = 0;
        public int CreatedBy { get; set; } = 0;
        public int ModifiedBy { get; set; } = 0;
        public int Ordering { get; set; } = 0;
        public List<Question.Question> ListQuestion { get; set; } = new List<Question.Question>() { };
    }

    public class SurveyModel {
        public List<Survey> ListItems { get; set; } = new List<Survey>() { };
        public SearchSurvey SearchData { get; set; } = new SearchSurvey() { };
        public Survey Item { get; set; } = new Survey() { };
        public PartialPagination Pagination { get; set; } = new PartialPagination() { };
    }
    public class SearchSurvey {
        public int CurrentPage { get; set; }
        public int ItemsPerPage { get; set; }
        public int IdCoQuan { get; set; }
        public string? Keyword { get; set; }
    }
}
