using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using API.Areas.Admin.Models.Partial;
using Microsoft.AspNetCore.Mvc.Rendering;
/*https://docs.microsoft.com/en-us/aspnet/core/tutorials/razor-pages/validation?view=aspnetcore-2.2*/
namespace API.Areas.Admin.Models.DuThaoVanBan
{
    public class DuThaoVanBan
    {
        public int Id { get; set; }        
        public string? Title { get; set; }
        public string? Introtext { get; set; }        
        public Boolean Status { get; set; }
        public int FieldId { get; set; }
        public string? TitleField { get; set; }
        public string? Link { get; set; }
        public string? Ids { get; set; }
        public int TotalRows { get; set; } = 0;
        public int CreatedBy { get; set; } = 0;
        public int IdCoQuan { get; set; } = 0;
        public int ModifiedBy { get; set; } = 0;
        public string? PublishUpShow { get; set; } = DateTime.Now.ToString("dd/MM/yyyy");
        public string? PublishDownShow { get; set; } = DateTime.Now.ToString("dd/MM/yyyy");
        public DateTime PublishUp { get; set; } = DateTime.Now;
        public DateTime PublishDown { get; set; } = DateTime.Now;
    }

    public class DuThaoVanBanModel {
        public List<DuThaoVanBan> ListItems { get; set; } = new List<DuThaoVanBan>() { };       
        public SearchDuThaoVanBan SearchData { get; set; } = new SearchDuThaoVanBan() { };
        public DuThaoVanBan Item { get; set; } = new DuThaoVanBan() { };
        public PartialPagination Pagination { get; set; } = new PartialPagination() { };
        public List<SelectListItem> ListDocumentsField { get; set; } = new List<SelectListItem>() { }; 
    }
    public class SearchDuThaoVanBan {
        public int CurrentPage { get; set; }
        public int ItemsPerPage { get; set; }
        public string? Keyword { get; set; }
        public int FieldId { get; set; }
        public int IdCoQuan { get; set; }
    }
}
