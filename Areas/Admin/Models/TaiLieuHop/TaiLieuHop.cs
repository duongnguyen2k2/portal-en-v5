using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using API.Areas.Admin.Models.Articles;
using API.Areas.Admin.Models.Documents;
using API.Areas.Admin.Models.Partial;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace API.Areas.Admin.Models.TaiLieuHop
{
    public class TaiLieuHop
    {
        public string? Ids { get; set; }
        public int TotalRows { get; set; }
        public int Id { get; set; }
        public int IdCoQuan { get; set; }
        public int CatId { get; set; }
        [Display(Name = "Tên")]
        [StringLength(250, MinimumLength = 3, ErrorMessage = "Độ dài tiêu đề chuỗi phải lớn hơn {2} Không quá {1} ký tự")]
        [Required(ErrorMessage = "Tiêu đề không được để trống")]
        public string? Title { get; set; }
        public string? Alias { get; set; }      
        public string? Introtext { get; set; }
        public string? FullText { get; set; }
        public string? Link { get; set; }
        public string? Link2 { get; set; }
        public Boolean Status { get; set; }
        public Boolean Deleted { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime IssuedDate { get; set; }
        public string? IssuedDateShow { get; set; } = DateTime.Now.ToString("dd/MM/yyyyy");
        public string? Author { get; set; }
        public string? Metadesc { get; set; }
        public string? Images { get; set; }
        public string? Code { get; set; }
        public string? Metakey { get; set; }
        public string? Metadata { get; set; }        
        public string? MatKhau { get; set; }
        public List<FileDocuments> ListFile { get; set; } = new List<FileDocuments>();
        public string? Str_ListFile { get; set; }

    }

    public class TaiLieuHopModel
    {
        public DocumentRefersCategories.DocumentRefersCategories Categories { get; set; } = new DocumentRefersCategories.DocumentRefersCategories();
        public List<TaiLieuHop> ListItems { get; set; } = new List<TaiLieuHop>();
        public List<SelectListItem> ListDocumentsCategories { get; set; } = new List<SelectListItem>();
        public SearchTaiLieuHop SearchData { get; set; } = new SearchTaiLieuHop() { };
        public PartialPagination PaginationMobile { get; set; } = new PartialPagination() { };
        public TaiLieuHop Item { get; set; } = new TaiLieuHop() { };
        public PartialPagination Pagination { get; set; } = new PartialPagination() { };      
        public List<SelectListItem> ListItemsStatus { get; set; } = new List<SelectListItem>();
    }

    public class SearchTaiLieuHop
    {
        public int CurrentPage { get; set; }
        public int ItemsPerPage { get; set; }
        public string? Keyword { get; set; }
        public string? Title { get; set; }
        public int CatId { get; set; }
        public int IdCoQuan { get; set; }
        public DateTime IssuedDateStart { get; set; }
        public DateTime IssuedDateEnd { get; set; } = DateTime.Now;
       
        public string? ShowStartDate { get; set; } = "01/01/1990";
        public string? ShowEndDate { get; set; } = DateTime.Now.ToString("dd/MM/yyyy");
        public int Status { get; set; } = -1;

    }
}
