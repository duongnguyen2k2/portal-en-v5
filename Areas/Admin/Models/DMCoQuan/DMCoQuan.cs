using API.Areas.Admin.Models.Partial;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using DocumentFormat.OpenXml.Drawing.Charts;


namespace API.Areas.Admin.Models.DMCoQuan
{
    public class DMCoQuan
    {
        public int Id { get; set; }

        [Display(Name = "Tên")]
        [StringLength(500, MinimumLength = 3, ErrorMessage = "Độ dài chuỗi phải lớn hơn {2} Không quá {1} ký tự")]
        [Required(ErrorMessage = "Tên cơ quan không được để trống")]

        public string? Title { get; set; }                      
        public string? Code { get; set; }
        public string? CodeDemo { get; set; }
        public string? Description { get; set; }        
        public Boolean Status { get; set; }       
        public string? Ids { get; set; }
        public int TotalRows { get; set; } = 0;
        public int CreatedBy { get; set; } = 0;
        public int ModifiedBy { get; set; } = 0;
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public Boolean Deleted { get; set; }
        public int ParentId { get; set; }
        public int CategoryId { get; set; }
        public int Selected { get; set; }
        public string? TitleCategory { get; set; }        
        public int Level { get; set; } = 0;      
        public string? Metadesc { get; set; }
        public string? Metakey { get; set; }        
        public string? Images { get; set; }        
        public string? Address { get; set; }        
        public string? Telephone { get; set; }
        public string? Fax { get; set; }
        public string? Email { get; set; }
        public string? TemplateName { get; set; }
        public string? Icon { get; set; }

        [StringLength(50, MinimumLength = 3, ErrorMessage = "Độ dài Tên folder phải lớn hơn {2} Không quá {1} ký tự")]
        [Required(ErrorMessage = "Tên Folder Upload không được để trống")]
        public string? FolderUpload { get; set; }
        public string? CompanyName { get; set; }        
        public string? Slogan { get; set; }        
        public string? Facebook { get; set; }
        public string? Twitter { get; set; }
        public string? Youtube { get; set; }
        public string? CssName { get; set; }
        public string? DataHeader { get; set; }
        public string? DataFooter { get; set; }
        public string? CodeMotCua { get; set; }
        public string? CodeMotCuaCha { get; set; }
        public string? CodeVanBan { get; set; }
        public string? CodeLCT { get; set; }
        public int PositionBannerHome { get; set; }
        public string? CTNC { get; set; }        
        public string? ThongBao { get; set; }
        public string? Metadata { get; set; }
        public string? Culture { get; set; }
        public string? Msg { get; set; }
        public Metadata MetadataCV { get; set; } = new Metadata() { };
        public string? DomainName{ get; set; }
    }
    public class DMCoQuanConfig
    {
        public int Id { get; set; }
        public string? Ids { get; set; }
        public string? DataHeader { get; set; }
        public string? DataFooter { get; set; }
        public string? LinkBanDo { get; set; }
    }
    public class DMCoQuanModel
    {
        public List<DMCoQuan> ListItems { get; set; } = new List<DMCoQuan>();
        public List<SelectListItem> ListItemsCoQuan { get; set; } = new List<SelectListItem>() { };
        public List<SelectListItem> ListItemsLoaiCoQuan { get; set; } = new List<SelectListItem>() { };
        public List<SelectListItem> ListTemplate { get; set; } = new List<SelectListItem>() { };
        public List<SelectListItem> ListItemsDanhMuc { get; set; } = new List<SelectListItem>() { };
        public SearchDMCoQuan SearchData { get; set; }  = new SearchDMCoQuan() { };
        public DMCoQuan Item { get; set; } = new DMCoQuan() { };
        public DMCoQuanConfig Config { get; set; } = new DMCoQuanConfig() { };
        public PartialPagination Pagination { get; set; } = new PartialPagination() { };
    }
    public class SearchDMCoQuan
    {
        public int CategoryId { get; set; }
        public int ParentId { get; set; }
        public int CurrentPage { get; set; }
        public int ItemsPerPage { get; set; }
        public string? Keyword { get; set; }
        public int Status { get; set; } = -1;
    }

    public class Metadata
    {
        public string? CodeMotCuaCha { get; set; }
        public string? UrlImage { get; set; }
        public string? UrlIcon { get; set; }
        public string? MailSettingsEmail { get; set; }
        public string? MailSettingsDisplayName { get; set; }
        public string? MailSettingsPassword { get; set; }
        public string? MailSettingsHost { get; set; }
        public string? MailSettingsPort { get; set; }
        public string? Placename { get; set; }
        public string? Position { get; set; }
        public string? Region { get; set; }        
        public string? MST { get; set; }
        public string? ThuTucHanhChinh { get; set; }
        public string? ThuTucHanhChinhXa { get; set; }
        public string? ThuTucHanhChinhHuyen { get; set; }
        public string? Favicon { get; set; }
        public string? DescNewsHot { get; set; }
        public string? DescNewsFeatured { get; set; }
        public string? DescFooter { get; set; }
        public string? DescFooterEn { get; set; }
        public string? DescImages { get; set; }
        public int FlagSession { get; set; } = 1;
        public int FlagComment { get; set; } = 1;
        public int FlagFeaturedHome { get; set; } = 1;
        public int FlagVideosHome { get; set; } = 1;
        public int FlagAuthorText { get; set; } = 0;
        public int FlagStatusArticle { get; set; } = 0;
        public string? FacebookVerification { get; set; }
        public string? ZaloVerification { get; set; }
        public string? FBAppId { get; set; }
        public string? ZaloAppId { get; set; }
        public string? LinkBanDo { get; set; }        
        public string? SmartVoiceTokenID { get; set; }        
        public string? SmartVoiceTokenKey { get; set; }        
        public string? SmartVoiceAccessToken { get; set; }
        public string? SmartVoiceRegion { get; set; } = "female_north";
        public double SmartVoiceSpeed { get; set; } = 1;
        public string? FacebookFanpage { get; set; }        
        public string? FacebookApp { get; set; }
        public string? LinkIPV6 { get; set; }
        public int flagDisMarqueeHome { get; set; } = 0;
        public int MarqueeSpeed { get; set; } = 100;
        public int MarqueeGap { get; set; } = 100;
        public string? MarqueeDirection { get; set; } = "left";
        public string? ArticelReportTemplate { get; set; } = "DanhSachBaiViet.xlsx";
        public int flagTaiLieuHop { get; set; } = 0;
        public int flagArticleMoney { get; set; } = 1;
        public int flagArticleQuantityImage { get; set; } = 1;
        public int flagShareApi { get; set; } = 1;
        public Boolean IsEnabledInforKnow { get; set; }
    }


}
