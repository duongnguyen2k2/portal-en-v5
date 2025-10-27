using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Areas.Admin.Models.CategoriesLanguage
{
    public class CategoriesLanguage
    {
        public string? Ids { get; set; }
        public int TotalRows { get; set; }
        public int Id { get; set; }
        public int TypeId { get; set; } // 1: CategoriesArticles,2: CategoriesAblums,3: CategoriesProducts
        public int IdRoot { get; set; }
        [Display(Name = "Tên")]
        [StringLength(60, MinimumLength = 3, ErrorMessage = "Độ dài chuỗi phải lớn hơn {2} Không quá {1} ký tự")]
        [Required(ErrorMessage = "Thể loại tin tức không được để trống")]
        public string? LanguageCode { get; set; }
        public string? Introtext { get; set; }
        public string? Title { get; set; }
        public string? Alias { get; set; }
        public string? Description { get; set; }
        public string? Metadesc { get; set; }
        public string? Metakey { get; set; }
        public string? Metadata { get; set; }
        public API.Models.MetaData MetadataCV { get; set; }
    }
}
