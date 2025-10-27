using API.Areas.Admin.Models.Partial;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace API.Areas.Admin.Models.DMLoaiPhongBan
{
    public class DMLoaiPhongBan
    {
        public int Id { get; set; }
        public int IdCoQuan { get; set; }
        [Display(Name = "Tên")]
        [StringLength(160, MinimumLength = 3, ErrorMessage = "Độ dài chuỗi phải lớn hơn {2} Không quá {1} ký tự")]
        [Required(ErrorMessage = "Tên loại Phòng Ban không được để trống")]
        public string? Title { get; set; }
        public string? Code { get; set; }
        public string? Description { get; set; }
        public bool Deleted { get; set; }
        public bool Status { get; set; }
        public string? Ids { get; set; }
        public int TotalRows { get; set; } = 0;
        public int CreatedBy { get; set; } = 0;
        public int ModifiedBy { get; set; } = 0;
        public DateTime ModifiedAt { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class DMLoaiPhongBanModel
    {
        public List<DMLoaiPhongBan> ListItems { get; set; } = new List<DMLoaiPhongBan>();
        public SearchDMLoaiPhongBan SearchData { get; set; } = new SearchDMLoaiPhongBan() { };
        public DMLoaiPhongBan Item { get; set; } = new DMLoaiPhongBan() { };
        public PartialPagination Pagination { get; set; } = new PartialPagination() { };
        public List<SelectListItem> ListItemsDanhMuc { get; set; } = new List<SelectListItem>();
    }
    public class SearchDMLoaiPhongBan
    {
        public int CurrentPage { get; set; }
        public int ItemsPerPage { get; set; }
        public int IdType { get; set; }
        public int TotalItems { get; set; }
        public int IdCoQuan { get; set; }
        public Boolean Status { get; set; }
        public string? Keyword { get; set; }
    }
}
