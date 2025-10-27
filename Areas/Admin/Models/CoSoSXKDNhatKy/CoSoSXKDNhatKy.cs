using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using API.Areas.Admin.Models.Partial;
using API.Areas.Admin.Models.CoSoSXKD;
/*https://docs.microsoft.com/en-us/aspnet/core/tutorials/razor-pages/validation?view=aspnetcore-2.2*/
namespace API.Areas.Admin.Models.CoSoSXKDNhatKy
{
    public class CoSoSXKDNhatKy
    {
        public int Id { get; set; }

        [Display(Name = "Tên")]
        [StringLength(60, MinimumLength = 3, ErrorMessage= "Độ dài chuỗi phải lớn hơn {2} Không quá {1} ký tự")]
        [Required(ErrorMessage = "Tên chức vụ không được để trống")]
        public string? Title { get; set; }
        public string? FullText { get; set; }        
        public Boolean Status { get; set; }
        public int IdCoSoSXKD { get; set; }
        public string? IdsCoSoSXKD { get; set; }
        public DateTime PublishUp { get; set; } = DateTime.Now;
        public string? PublishUpShow { get; set; } = DateTime.Now.ToString("dd/MM/yyyy");
        public string? Ids { get; set; }
        public int TotalRows { get; set; } = 0;
        public int CreatedBy { get; set; } = 0;
        public int ModifiedBy { get; set; } = 0;
    }

    public class CoSoSXKDNhatKyModel {
        public List<CoSoSXKDNhatKy> ListItems { get; set; } = new List<CoSoSXKDNhatKy>();
        public SearchCoSoSXKDNhatKy SearchData { get; set; } = new SearchCoSoSXKDNhatKy() { };
        public CoSoSXKD.CoSoSXKD CoSoSXKD { get; set; } = new CoSoSXKD.CoSoSXKD() { };
        public CoSoSXKDNhatKy Item { get; set; } = new CoSoSXKDNhatKy() { };
        public PartialPagination Pagination { get; set; } = new PartialPagination() { };
    }
    public class SearchCoSoSXKDNhatKy {
        public int CurrentPage { get; set; }
        public int ItemsPerPage { get; set; }
        public int IdCoSoSXKD { get; set; }
        public string? Keyword { get; set; }
        public string? IdsCoSoSXKD { get; set; }
    }
}
