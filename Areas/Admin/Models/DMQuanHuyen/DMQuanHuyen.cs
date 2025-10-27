using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Areas.Admin.Models.DMTinhThanh;
using API.Areas.Admin.Models.Partial;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace API.Areas.Admin.Models.DMQuanHuyen
{
    public class DMQuanHuyen
    {
        public int Id { get; set; }
        public string? Ten { get; set; }
        public string? Ma { get; set; }
        public string? MaTinhThanh { get; set; }        
        public int IdTinhThanh { get; set; }
        public bool TrangThai { get; set; }
        public string? MoTa { get; set; }
        public string? Ids { get; set; }
        public int TotalRows { get; set; }
        public string? TenTinhThanh { get; set; }
        public int CreatedBy { get; set; }
        public int ModifiedBy { get; set; }
    }

    public class DMQuanHuyenModel
    {
        public List<DMQuanHuyen> ListItems { get; set; } = new List<DMQuanHuyen>();
        public List<SelectListItem> ListItemsTinhThanh { get; set; } = new List<SelectListItem>();
        public SearchDMQuanHuyen SearchData { get; set; } = new SearchDMQuanHuyen() { };
        public DMQuanHuyen Item { get; set; } = new DMQuanHuyen() { };
        public PartialPagination Pagination { get; set; } = new PartialPagination() { };
    }
    public class SearchDMQuanHuyen
    {
        public int CurrentPage { get; set; }
        public int ItemsPerPage { get; set; }
        public int IdTinhThanh { get; set; }
        public string? Keyword { get; set; }
    }
}
