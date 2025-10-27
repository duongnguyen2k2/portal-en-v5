using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using API.Areas.Admin.Models.Partial;

namespace API.Areas.Admin.Models.DMTrinhDo
{
    public class DMTrinhDo
    {

        public int Id { get; set; }

        [Display(Name = "Tên")]
        [StringLength(60, MinimumLength = 3, ErrorMessage = "Độ dài chuỗi phải lớn hơn {2} Không quá {1} ký tự")]
        [Required(ErrorMessage = "Tên Trình độ không được để trống")]
        public string? Title { get; set; }
        public string? Description { get; set; }
        public Boolean Status { get; set; }
        public string? Ids { get; set; }
        public int TotalRows { get; set; } = 0;
        public int CreatedBy { get; set; } = 0;
        public int ModifiedBy { get; set; } = 0;
    }
    public class DMTrinhDoModel
    {
        public List<DMTrinhDo> ListItems { get; set; } = new List<DMTrinhDo>();
        public SearchDMTrinhDo SearchData { get; set; } = new SearchDMTrinhDo() { };
        public DMTrinhDo Item { get; set; } = new DMTrinhDo() { };
        public PartialPagination Pagination { get; set; } = new PartialPagination() { };
    }
    public class SearchDMTrinhDo
    {
        public int CurrentPage { get; set; }
        public int ItemsPerPage { get; set; }
        public string? Keyword { get; set; }
    }
}
