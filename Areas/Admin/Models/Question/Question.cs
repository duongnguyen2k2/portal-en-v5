using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using API.Areas.Admin.Models.Partial;
using Microsoft.AspNetCore.Mvc.Rendering;
/*https://docs.microsoft.com/en-us/aspnet/core/tutorials/razor-pages/validation?view=aspnetcore-2.2*/
namespace API.Areas.Admin.Models.Question
{
    public class Question
    {
        public int Id { get; set; }

        [Display(Name = "Tên")]
        [StringLength(1024, MinimumLength = 3, ErrorMessage= "Độ dài chuỗi phải lớn hơn {2} Không quá {1} ký tự")]
        [Required(ErrorMessage = "Nội dung khảo sát không được để trống")]
        public string? Title { get; set; }           
        public string? TitleSurvey { get; set; }           
        public Boolean Status { get; set; }
        public int IdCoQuan { get; set; }
        public int SurveyId { get; set; }
        public string? SurveyIds { get; set; }
        public string? Ids { get; set; }
        public string? Token { get; set; }
        public int TotalRows { get; set; } = 0;
        public int CreatedBy { get; set; } = 0;
        public int ModifiedBy { get; set; } = 0;
        public int Ordering { get; set; } = 0;
    }

    public class QuestionModel {
        public List<Question> ListItems { get; set; } = new List<Question>();       
        public SearchQuestion SearchData { get; set; } = new SearchQuestion() { };
        public Question Item { get; set; } = new Question() { };
        public PartialPagination Pagination { get; set; } = new PartialPagination() { };
		public List<SelectListItem> ListSurvey { get; set; } = new List<SelectListItem>();
    }
    public class SearchQuestion {
        public int CurrentPage { get; set; }
        public int ItemsPerPage { get; set; }
        public int SurveyId { get; set; }
        public string? Keyword { get; set; }
        public string? SurveyIds { get; set; }
    }
}
