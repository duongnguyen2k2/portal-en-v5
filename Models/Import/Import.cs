using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using API.Areas.Admin.Models.Partial;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using API.Models.Import;
using API.Areas.Admin.Models.USUsers;
using static API.Areas.Admin.Models.ManagerFile.ManagerFile;

namespace API.Models.Import
{
    public class Import
    {
        public string? Ids { get; set; }
        public int TotalRows { get; set; }
        public int Id { get; set; }
        [Display(Name = "Tên")]
        [StringLength(500, MinimumLength = 3, ErrorMessage = "Độ dài chuỗi phải lớn hơn {2} Không quá {1} ký tự")]
        [Required(ErrorMessage = "Tiêu đề không được để trống")]
        public string? Title { get; set; }
        public string? Alias { get; set; }
        [Range(1, Int32.MaxValue, ErrorMessage = "Danh mục không được để trống")]
        public int CatId { get; set; }
        public string? IntroText { get; set; }
        public string? FullText { get; set; }
        public Boolean Status { get; set; }
        public int? CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? Metadesc { get; set; }
        public string? Metakey { get; set; }
        public string? Metadata { get; set; }
        public string? Language { get; set; }
        public Boolean Featured { get; set; }
        public Boolean Notification { get; set; }
        public Boolean FeaturedHome { get; set; }
        public Boolean StaticPage { get; set; }
        public string? Images { get; set; }
        public string? Category { get; set; }
        public IFormFile Icon { get; set; }
        public string? Params { get; set; }
        public int? Ordering { get; set; }
        public Boolean Deleted { get; set; }
        public int Hit { get; set; }
        public int AuthorId { get; set; }
        public int IdCoQuan { get; set; }
        public string? TenCoQuan { get; set; }
        public string? AuthorName { get; set; }
        public string? ExternalLink { get; set; }

        [Required(ErrorMessage = "Ngày đăng không được để trống")]
        public DateTime PublishUp { get; set; } = DateTime.Now;
        public string? PublishUpShow { get; set; } = DateTime.Now.ToString("dd/MM/yyyyy");
        public List<LinkArticle> ListLinkArticle { get; set; }
        public string? Str_ListFile { get; set; }
        public List<FileArticle> ListFile { get; set; }
        public string? Str_Link { get; set; }
        public string? CreatedByName { get; set; }
        public string? CreatedByFullName { get; set; }

        public string? AudioFile { get; set; }
        public string? LangId { get; set; }
        public string? Author { get; set; }
        public string? DataFile { get; set; }
    }

    public class ImportExcel
    {
              
     
        public string? Title { get; set; }        
        public string? Category { get; set; }       
        public string? PublishUpShow { get; set; } = DateTime.Now.ToString("dd/MM/yyyyy");
        public DateTime PublishUp { get; set; }
       
    }

    public class FileArticle
    {
        public string? FilePath { get; set; }
    }

    public class LinkArticle
    {
        public string? Title { get; set; }
        public string? Link { get; set; }
        public Boolean Status { get; set; }
    }
    public class ImportModel
    {
        public List<Import> ListItems { get; set; }
        public SearchImport SearchData { get; set; }
        public List<SelectListItem> ListItemsDanhMuc { get; set; }
        public Import Item { get; set; }
        public PartialPagination Pagination { get; set; }
        public List<SelectListItem> ListItemsAuthors { get; set; }
        public List<SelectListItem> ListItemsCreatedBy { get; set; }
        public List<SelectListItem> ListItemsStatus { get; set; }

        public List<TinymceFile> ListItemsFile { get; set; }
    }

    public class SearchImport
    {
        public int CurrentPage { get; set; }
        public int ItemsPerPage { get; set; }
        public string? Cookie { get; set; }
        public string? Link { get; set; }
        public int CatId { get; set; }
        public int IdCoQuan { get; set; }
        public int AuthorId { get; set; }
        public int CreatedBy { get; set; }
        public string? Keyword { get; set; }
        public string? ShowStartDate { get; set; } = "01/01/2010";
        public string? ShowEndDate { get; set; } = DateTime.Now.ToString("dd/MM/yyyy");
        public int Status { get; set; } = -1;
      
    }


}
