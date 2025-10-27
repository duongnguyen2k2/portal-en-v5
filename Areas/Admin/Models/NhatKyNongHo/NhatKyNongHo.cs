using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using API.Areas.Admin.Models.Partial;
using API.Areas.Admin.Models.CoSoSXKD;
/*https://docs.microsoft.com/en-us/aspnet/core/tutorials/razor-pages/validation?view=aspnetcore-2.2*/
namespace API.Areas.Admin.Models.NhatKyNongHo
{
    public class NhatKyNongHo
    {
        public int Id { get; set; }

        [Display(Name = "Tên")]
        [StringLength(60, MinimumLength = 3, ErrorMessage= "Độ dài chuỗi phải lớn hơn {2} Không quá {1} ký tự")]
        [Required(ErrorMessage = "Tên không được để trống")]
        public string? Title { get; set; }
        public string? FullText { get; set; }        
        public Boolean Status { get; set; }
        public int IdCoSoSXKD { get; set; }
        public int IdNguonGocCayTrong { get; set; }
        public string? IdsNguonGocCayTrong { get; set; }
        public DateTime PublishUp { get; set; } = DateTime.Now;
        public string? PublishUpShow { get; set; } = DateTime.Now.ToString("dd/MM/yyyy");
        public string? Ids { get; set; }
        public int TotalRows { get; set; } = 0;
        public int CreatedBy { get; set; } = 0;
        public int ModifiedBy { get; set; } = 0;
    }

    public class NhatKyNongHoModel {
        public List<NhatKyNongHo> ListItems { get; set; } = new List<NhatKyNongHo>();
        public SearchNhatKyNongHo SearchData { get; set; } = new SearchNhatKyNongHo() { };
        public NguonGocCayTrong.NguonGocCayTrong NguonGocCayTrong { get; set; } = new NguonGocCayTrong.NguonGocCayTrong() { };
        public NhatKyNongHo Item { get; set; } = new NhatKyNongHo() { };
        public PartialPagination Pagination { get; set; } = new PartialPagination() { };
    }
    public class SearchNhatKyNongHo {
        public int CurrentPage { get; set; }
        public int ItemsPerPage { get; set; }
        
        public string? Keyword { get; set; }
        public int IdNguonGocCayTrong { get; set; }
        public string? IdsNguonGocCayTrong { get; set; }
    }
}
