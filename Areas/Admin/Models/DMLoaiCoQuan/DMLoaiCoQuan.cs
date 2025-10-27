using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using API.Areas.Admin.Models.Partial;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace API.Areas.Admin.Models.DMLoaiCoQuan
{
    public class DMLoaiCoQuan
    {
        public int Id { get; set; }

        [Display(Name = "Tên")]
        [StringLength(60, MinimumLength = 3, ErrorMessage = "Độ dài chuỗi phải lớn hơn {2} Không quá {1} ký tự")]
        [Required(ErrorMessage = "Tên loại cơ quan không được để trống")]
        public string? Title { get; set; }
        public string? Code { get; set; }
        public string? Description { get; set; }
        public Boolean Status { get; set; }
        public string? Ids { get; set; }
        public int TotalRows { get; set; } = 0;
        public int CreatedBy { get; set; } = 0;
        public int ModifiedBy { get; set; } = 0;
    }
    public class DMLoaiCoQuanModel
    {
        public List<DMLoaiCoQuan> ListItems { get; set; } = new List<DMLoaiCoQuan>();
        public SearchDMLoaiCoQuan SearchData { get; set; } = new SearchDMLoaiCoQuan() { };
        public DMLoaiCoQuan Item { get; set; } = new DMLoaiCoQuan() { };
        public PartialPagination Pagination { get; set; } = new PartialPagination() { };
    }
    public class SearchDMLoaiCoQuan
    {
        public int CurrentPage { get; set; }
        public int ItemsPerPage { get; set; }
        public string? Keyword { get; set; }
    }
}