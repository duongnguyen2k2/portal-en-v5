using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using API.Areas.Admin.Models.Partial;
/*https://docs.microsoft.com/en-us/aspnet/core/tutorials/razor-pages/validation?view=aspnetcore-2.2*/
namespace API.Areas.Admin.Models.WorkSchedules
{
    public class WorkSchedules
    {
        public int Id { get; set; }
            
        public string? Title { get; set; }
        public string? IntroText { get; set; }
        public string? FullText { get; set; }
        public Boolean Status { get; set; }        
        public string? Ids { get; set; }
        public int ShowWeek { get; set; } = 0;
        public int IdCoQuan { get; set; } = 0;
        public int ShowYear { get; set; } = 0;
        public int TotalRows { get; set; } = 0;
        public int CreatedBy { get; set; } = 0;
        public int ModifiedBy { get; set; } = 0;      
    }

    public class WorkSchedulesModel {
        public List<WorkSchedules> ListItems { get; set; } = new List<WorkSchedules>() { };
        public SearchWorkSchedules SearchData { get; set; } = new SearchWorkSchedules() { };
        public WorkSchedules Item { get; set; } = new WorkSchedules() { };
        public PartialPagination Pagination { get; set; } = new PartialPagination() { };
    }
    public class SearchWorkSchedules {
        public int CurrentPage { get; set; }
        public int ItemsPerPage { get; set; }
        public int ShowYear { get; set; }
        public int ShowWeek { get; set; }
        public int IdCoQuan { get; set; }
        public int Status { get; set; }
        public string? Keyword { get; set; }
    }

    public class WorkSchedulesWeek
    { 
        public int Id { get; set; }
        public string? Title { get; set; }
    }
}
