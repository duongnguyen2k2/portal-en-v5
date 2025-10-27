using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using API.Areas.Admin.Models.Partial;
/*https://docs.microsoft.com/en-us/aspnet/core/tutorials/razor-pages/validation?view=aspnetcore-2.2*/
namespace API.Areas.Admin.Models.Answer
{
    public class Answer
    {
        public int Id { get; set; }
        public string? CustomerId { get; set; }
        public string? QuestionTitle { get; set; }
        public string? SurveyTitle { get; set; }
        public int QuestionId { get; set; }        
        public int SurveyId { get; set; }        
        public int IdCoQuan { get; set; }        
        public string? Ids { get; set; }
        public int TotalRows { get; set; } = 0;
        public int CreatedBy { get; set; } = 0;
        public int ModifiedBy { get; set; } = 0;
        public DateTime CreatedDate { get; set; }
        public int Ordering { get; set; } = 0;
    }

    public class AnswerModel {
        public List<Answer> ListItems { get; set; }       
        public SearchAnswer SearchData { get; set; }
        public Answer Item { get; set; }
        public PartialPagination Pagination { get; set; } = new PartialPagination() { };
    }
    public class SearchAnswer {
        public int CurrentPage { get; set; }
        public int ItemsPerPage { get; set; }
        public string? Keyword { get; set; }
		public int QuestionId { get; set; }        
        public int SurveyId { get; set; }
        public int IdCoQuan { get; set; }
    }
}
