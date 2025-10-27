using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using API.Areas.Admin.Models.Partial;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace API.Areas.Admin.Models.Documents
{
    public class Documents
    {
        public string? Ids { get; set; }
        public int TotalRows { get; set; }
        public int Id { get; set; }
        [Display(Name = "Tên")]
        [StringLength(2000, MinimumLength = 3, ErrorMessage = "Độ dài tiêu đề chuỗi phải lớn hơn {2} Không quá {1} ký tự")]
        [Required(ErrorMessage = "Tiêu đề không được để trống")]
        public string? Title { get; set; }
        public string? Alias { get; set; }
        [Display(Name = "Số ký hiệu")]
        [StringLength(60, MinimumLength = 1, ErrorMessage = "Độ dài Số ký hiệu phải lớn hơn {2} Không quá {1} ký tự")]
        [Required(ErrorMessage = "Số ký hiệu không được để trống")]
        public string? Code { get; set; }
        public string? Introtext { get; set; }
        public string? FullText { get; set; }
        public string? Link { get; set; }
        public Boolean Status { get; set; }
        public Boolean Deleted { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime IssuedDate { get; set; }
        public string? IssuedDateShow { get; set; } = DateTime.Now.ToString("dd/MM/yyyy");
        public DateTime EffectiveDate { get; set; }
        public string? EffectiveDateShow { get; set; } = DateTime.Now.ToString("dd/MM/yyyy");
        public List<FileDocuments> ListFile { get; set; } = new List<FileDocuments>() { };
        public int CatId { get; set; }
        public int IdCoQuan { get; set; }
        public int TypeId { get; set; }
        public int FieldId { get; set; }
        public int LevelId { get; set; }
        public string? OrganizationName { get; set; }        
        public string? Str_ListFile { get; set; }
        public string? LinkRoot { get; set; }
        public string? DataContentHtml { get; set; }
        public string? ExpriedStatus { get; set; }
        public string? SignBy { get; set; }
        public string? DuLieuThongTin { get; set; }
        public string? FieldTitle { get; set; }
        public string? TypeTitle { get; set; }
        public string? LevelTitle { get; set; }
        public Boolean Featured { get; set; }
        public Boolean Notification { get; set; }
        public Boolean FeaturedHome { get; set; }

    }

    public class DuLieuThongTin
    {
        public string? CoQuanBanHanh { get; set; }//Cơ quan ban hành
        public string? LoaiVanBan { get; set; }//Loại văn bản
        public string? LinhVuc { get; set; }//Lĩnh vực
        public string? CapBanHanh { get; set; }//Cấp ban hành
    }

    public class DocumentsModel
    {
        public List<Documents> ListItems { get; set; } = new List<Documents> { };
        public SearchDocuments SearchData { get; set; } = new SearchDocuments() { };
        public Documents Item { get; set; } = new Documents() { };
        public PartialPagination Pagination { get; set; } = new PartialPagination() { };
        public List<SelectListItem> ListDocumentsCategories { get; set; } = new List<SelectListItem> { };
        public List<SelectListItem> ListDocumentsType { get; set; } = new List<SelectListItem> { };
        public List<SelectListItem> ListDocumentsField { get; set; } = new List<SelectListItem> { };
        public List<SelectListItem> ListDocumentsLevel { get; set; } = new List<SelectListItem> { };
        public List<SelectListItem> ListItemsStatus { get; set; } = new List<SelectListItem> { };
        public List<SelectListItem> ListDMCoQuan { get; set; } = new List<SelectListItem> { };
    }

    public class FileDocuments
    {
        public string? FilePath { get; set; }
        public string? Title { get; set; }
    }

    public class SearchDocuments
    {
        public int CurrentPage { get; set; }
        public int ItemsPerPage { get; set; }
        public string? Keyword { get; set; }
        public string? Title { get; set; }
        public int CatId { get; set; }
        public int TypeId { get; set; }
        public int FieldId { get; set; }
        public int LevelId { get; set; }
        public string? OrganizationName { get; set; }
        public DateTime IssuedDateStart { get; set; }
        public DateTime IssuedDateEnd { get; set; } = DateTime.Now;
        public DateTime EffectiveDateStart { get; set; }
        public DateTime EffectiveDateEnd { get; set; } = DateTime.Now;
        public string? ShowStartDate { get; set; } = "01/01/" + (Int32.Parse(DateTime.Now.ToString("yyyy")) - 1).ToString();
        public string? ShowEndDate { get; set; } = DateTime.Now.ToString("dd/MM/yyyy");
        public int Status { get; set; } = -1;
        public int IdCoQuan { get; set; }

    }
}
