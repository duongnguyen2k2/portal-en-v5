using API.Areas.Admin.Models.Banners;
using API.Areas.Admin.Models.Partial;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace API.Areas.Admin.Models.SangKienKinhNghiem
{
    public class SangKienKinhNghiem
    {
        public int Id { get; set; }
        public string? MaSo { get; set; }
        public string? Ten { get; set; }
        public int CapQuanLy { get; set; }
        public string? TenCapQuanLy { get; set; }
        public string? TenLinhVuc { get; set; }
        public string? TacGia { get; set; }
        public int LinhVuc { get; set; }
        public int IdCoQuan { get; set; }
        public string? DonViChuTri { get; set; }
        public DateTime ThoiGianThucHienTu { get; set; } = DateTime.Now;
        public DateTime ThoiGianThucHienDen { get; set; } = DateTime.Now;
        public string? KetQua { get; set; }
        public bool Status { get; set; }
        public bool Deleted { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime CreatedAt { get; set;}
        public DateTime UpdatedAt { get; set;}
        public string? Ids { get; set; }
        public string? Image { get; set; }
        public int TotalRows { get; set; }
    }

    public class SangKienKinhNghiemModel {
        public SangKienKinhNghiem Item { get; set; } = new SangKienKinhNghiem() { };
        public List<SangKienKinhNghiem> ListItems { get; set; } = new List<SangKienKinhNghiem>();
        public List<SelectListItem> ListCapQuanLy { get; set; } = new List<SelectListItem>(); //document level
        public List<SelectListItem> ListLinhVuc { get; set; } = new List<SelectListItem>(); //document field
        public List<SelectListItem> ListDMCoQuan { get; set; } = new List<SelectListItem>(); 
        public PartialPagination Pagination { get; set; } = new PartialPagination() { };
        public SearchSangKienKinhNghiem SearchData { get; set; } = new SearchSangKienKinhNghiem() { };
    }

    public class SearchSangKienKinhNghiem
    {
        public int CurrentPage { get; set; }
        public int ItemsPerPage { get; set; }
        public int LinhVuc { get; set; }
        public int CapQuanLy { get; set; }
        public int IdCoQuan { get; set; }
        public int AuthorId { get; set; }
        public int CreatedBy { get; set; }
        public string? Keyword { get; set; }
        public string? ShowStartDate { get; set; } = "01/01/" + (Int32.Parse(DateTime.Now.ToString("yyyy")) - 1).ToString();
        public string? ShowEndDate { get; set; } = DateTime.Now.ToString("dd/MM/yyyy");
        public int Status { get; set; } = -1;
        public int Id { get; set; } = 0;
    }
}
