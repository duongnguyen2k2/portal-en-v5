using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using API.Areas.Admin.Models.Partial;

namespace API.Areas.Admin.Models.DMTonGiao
{
    public class DMTonGiao
    {

        public int Id { get; set; }

        [Display(Name = "Tên")]
        [StringLength(60, MinimumLength = 3, ErrorMessage = "Độ dài chuỗi phải lớn hơn {2} Không quá {1} ký tự")]
        [Required(ErrorMessage = "Tên Tôn giáo không được để trống")]
        public string? Title { get; set; }
        public string? Description { get; set; }
        public Boolean Status { get; set; }
        public string? Ids { get; set; }
        public int TotalRows { get; set; } = 0;
        public int CreatedBy { get; set; } = 0;
        public int ModifiedBy { get; set; } = 0;
    }
    public class DMTonGiaoModel
    {
        public List<DMTonGiao> ListItems { get; set; } = new List<DMTonGiao>();
        public SearchDMTonGiao SearchData { get; set; }= new SearchDMTonGiao() { };
        public DMTonGiao Item { get; set; } = new DMTonGiao() { };
        public PartialPagination Pagination { get; set; } = new PartialPagination() { };
    }
    public class SearchDMTonGiao
    {
        public int CurrentPage { get; set; }
        public int ItemsPerPage { get; set; }
        public string? Keyword { get; set; }
    }
}
