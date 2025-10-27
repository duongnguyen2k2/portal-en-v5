using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using API.Areas.Admin.Models.Partial;
using Microsoft.AspNetCore.Mvc.Rendering;
/*https://docs.microsoft.com/en-us/aspnet/core/tutorials/razor-pages/validation?view=aspnetcore-2.2*/
namespace API.Areas.Admin.Models.HopTrucTuyen
{
    public class HopTrucTuyen
    {
        public int Id { get; set; }

        [Display(Name = "Tên")]
        [StringLength(2000, MinimumLength = 3, ErrorMessage = "Độ dài chuỗi phải lớn hơn {2} Không quá {1} ký tự")]
        [Required(ErrorMessage = "Tiêu đề không được để trống")]        
        public string? Title { get; set; }
        public string? Code { get; set; }
        public string? TenCoQuan { get; set; }
        public string? Description { get; set; }
        public Boolean Status { get; set; }
        public DateTime PublishUp { get; set; } = DateTime.Now;
        public DateTime PublishDown { get; set; } = DateTime.Now;
        public string? PublishUpShow { get; set; } = DateTime.Now.ToString("dd/MM/yyyyy");
        public string? PublishDownShow { get; set; } = DateTime.Now.ToString("dd/MM/yyyyy");
        public string? Ids { get; set; }
        public int TotalRows { get; set; } = 0;
        public int CreatedBy { get; set; } = 0;
        public int ModifiedBy { get; set; } = 0;
        public int IdCoQuan { get; set; } = 0;
        public int CatId { get; set; } = 1;
        public string? Link { get; set; }
        public string? Images { get; set; }
        public string? Alias { get; set; }
        public string? CatTitle { get; set; }
        public string? Str_ListFile { get; set; }
        public string? FullText { get; set; }
        public List<FileArticle> ListFile { get; set; }        
    }

    public class FileArticle
    {
        public string? FilePath { get; set; }
        public string? Title { get; set; }
        public Boolean Status { get; set; }
    }


    public class HopTrucTuyenModel
    {
        public List<HopTrucTuyen> ListItems { get; set; } = new List<HopTrucTuyen>();
        public List<SelectListItem> ListDMCoQuan { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> ListCat { get; set; } = new List<SelectListItem>();
        public SearchHopTrucTuyen SearchData { get; set; } = new SearchHopTrucTuyen() { };
        public HopTrucTuyen Item { get; set; } = new HopTrucTuyen() { };
        public PartialPagination Pagination { get; set; } = new PartialPagination() { };
    }
    public class SearchHopTrucTuyen
    {
        public int CurrentPage { get; set; }
        public int ItemsPerPage { get; set; }
        public int IdCoQuan { get; set; }
        public int CatId { get; set; }
        public string? Keyword { get; set; }
        public string? ShowStartDate { get; set; } = "01/01/" + DateTime.Now.ToString("yyyy");
        public string? ShowEndDate { get; set; } = DateTime.Now.ToString("dd/MM/yyyy");
        public int Status { get; set; } = -1;
    }
}
