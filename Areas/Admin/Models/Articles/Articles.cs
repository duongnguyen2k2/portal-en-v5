using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using API.Areas.Admin.Models.Partial;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using API.Areas.Admin.Models.Articles;
using API.Areas.Admin.Models.USUsers;
using static API.Areas.Admin.Models.ManagerFile.ManagerFile;

namespace API.Areas.Admin.Models.Articles
{
    public class Articles
    {
        public string? Ids { get; set; }
        public int TotalRows { get; set; }
        public int Id { get; set; }
        [Display(Name = "Tên")]
        [StringLength(550, MinimumLength = 3, ErrorMessage = "Độ dài chuỗi phải lớn hơn {2} Không quá {1} ký tự")]
        [Required(ErrorMessage = "Tiêu đề không được để trống")]
        public string? Title { get; set; }
        public string? Alias { get; set; }
        public int CatId { get; set; }
        public int LikeN { get; set; }
        public Int64 STT { get; set; }
        public string? IntroText { get; set; }
        public string? Address { get; set; }
        public string? FullText { get; set; }
        public string? AuthorRoot { get; set; }
        public Boolean Status { get; set; }
        public int? CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? ModifiedDate { get; set; } = DateTime.Now;
		public string? Metadesc { get; set; }
        public string? Metakey { get; set; }
        public string? Metadata { get; set; }
        public string? Language { get; set; }
        public Boolean Featured { get; set; }
        public Boolean Notification { get; set; }
        public Boolean FeaturedHome { get; set; }
        public Boolean Comment { get; set; } = false;
        public Boolean StaticPage { get; set; }
        public string? Images { get; set; }
        public string? FileItem { get; set; }
        public string? Category { get; set; }
        public string? Icon { get; set; }
        public string? Params { get; set; }
        public int? Ordering { get; set; }
        public Boolean Deleted { get; set; }
        public int Hit { get; set; }
        public int AuthorId { get; set; }
        public int IdCoQuan { get; set; }
        public string? TenCoQuan { get; set; }
        public string? CodeCoQuan { get; set; }
        public string? AuthorName { get; set; }
        public string? Author { get; set; }

        [Required(ErrorMessage = "Ngày đăng không được để trống")]
        public DateTime PublishUp { get; set; } = DateTime.Now;
        public string? PublishUpShow { get; set; } = DateTime.Now.ToString("dd/MM/yyyy");
        public List<FileArticle> ListFile { get; set; } = new List<FileArticle>() { };
        public List<LinkArticle> ListLinkArticle { get; set; } = new List<LinkArticle> { new LinkArticle() };
        public string? Str_ListFile { get; set; }
        public string? Str_Link { get; set; }
        public string? CreatedByName { get; set; }
        public string? CreatedByFullName { get; set; }
        public int RootNewsId { get; set; }
        public int QuantityImage { get; set; }
        public Double Money { get; set; }
        public int IdLevel { get; set; } = 0;
        public string? LevelTitle { get; set; }
        public int CategoryTypeId { get; set; }
        public string? CategoryTypeTitle { get; set; } = "";
        public string? DataFile { get; set; } = "";
        public string? LinkRoot { get; set; } = "";
        public Boolean RootNewsFlag { get; set; }
        public Boolean FlagEdit { get; set; } = true;
        public string? Title_EN { get; set; }
        public string? Alias_EN { get; set; }
        public string? IntroText_EN { get; set; }
        public string? FullText_EN { get; set; }
        public string? Metadesc_EN { get; set; }
        public string? Metakey_EN { get; set; }
        public string? FileItem_EN { get; set; }
        public API.Models.MetaData MetadataCV_EN { get; set; } = new API.Models.MetaData() { };
        public API.Models.MetaData MetadataCV { get; set; } = new API.Models.MetaData() { };
        public API.Areas.Admin.Models.Articles.MetaMoney MetaMoneyCV { get; set; } = new MetaMoney() { };
        public string? MetaMoney { get; set; } = null;
        public string? Metadata_EN { get; set; } = null;
        public string? Link_EN { get; set; } = null;
        public string? TenPhongBan { get; set; } = null;
        public string? ModifiedByFullName { get; set; } = null;        
        public List<TinymceFile> ListDataFile { get; set; } = new List<TinymceFile> { };
        public int ArticlesStatusId { get; set; } = 1;

    }

    public class FileArticle
    {
        public int Id { get; set; }
        public int RootId { get; set; }
        public string? FilePath { get; set; }
        public string? FileName { get; set; }
    }

    public class LinkArticle
    {
        public string? Title { get; set; }
        public string? Link { get; set; }
        public Boolean Status { get; set; }
    }

    public class MetaMoney
    {
        public double HeSoTinBai { get; set; }
        public double HeSoHinhAnh { get; set; }
        public int NhuanButTinBai { get; set; }
        public int NhuanButAnh { get; set; }
        public string? LoaiHinhAnh { get; set; } = "";        
    }
    public class ArticlesModel
    {
        public List<Articles> ListItems { get; set; } = new List<Articles>();
        public SearchArticles SearchData { get; set; } = new SearchArticles();
        public CategoriesArticles.CategoriesArticles Categories { get; set; } = null;
        public List<SelectListItem> ListItemsDanhMuc { get; set; } = new List<SelectListItem>() { };
        public Articles Item { get; set; } = new Articles() { };
        public PartialPagination Pagination { get; set; } = new PartialPagination() { };
        public List<SelectListItem> ListItemsAuthors { get; set; } = new List<SelectListItem>(){ };
        public List<SelectListItem> ListItemsCreatedBy { get; set; } = new List<SelectListItem>() { };
        public List<SelectListItem> ListItemsStatus { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> ListDMCoQuan { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> ListCategoryType { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> ListLevelArticles { get; set; } = new List<SelectListItem>() { };
    }

    public class SearchArticles
    {
        public int CurrentPage { get; set; }
        public int ItemsPerPage { get; set; }
        public int CatId { get; set; }
        public int IdCoQuan { get; set; }
        public int AuthorId { get; set; }
        public int CreatedBy { get; set; }
        public int FlagStatus { get; set; }
        public string? Keyword { get; set; }
        public string? ShowStartDate { get; set; } = "01/01/" + (Int32.Parse(DateTime.Now.ToString("yyyy"))-3).ToString();
        public string? ShowEndDate { get; set; } = DateTime.Now.AddYears(1).ToString("dd/MM/yyyy");
        public int Status { get; set; } = -1;
        public int Id { get; set; } =0;
        public int ArticlesStatusId { get; set; }
        public int PaymentId { get; set; }
    }


}
