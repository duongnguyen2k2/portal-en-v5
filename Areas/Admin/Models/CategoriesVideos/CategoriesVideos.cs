using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using API.Areas.Admin.Models.Partial;

using Microsoft.AspNetCore.Mvc.Rendering;

namespace API.Areas.Admin.Models.CategoriesVideos
{
    public class CategoriesVideos
    {
		public string? Ids { get; set; }
        public int TotalRows { get; set; }
        public int Id { get; set; }
        

        [Display(Name = "Tên")]
        [StringLength(1024, MinimumLength = 3, ErrorMessage = "Độ dài chuỗi phải lớn hơn {2} Không quá {1} ký tự")]
        [Required(ErrorMessage = "Tên không được để trống")]

        public string? Title { get; set; }
        public string? TitleEn { get; set; }
        public string? Alias { get; set; }
 		public string? Description { get; set; }
 		public string? Images { get; set; }
 		public Boolean Status { get; set; }
 		public Boolean Featured { get; set; }
 		public Boolean Deleted { get; set; }
 		public int CreatedBy { get; set; }
 		public int ParentId { get; set; }
 		public int Ordering { get; set; }
 		public DateTime? CreatedDate { get; set; }
 		public int? ModifiedBy { get; set; }
 		public DateTime? ModifiedDate { get; set; }

        public string? Metadesc { get; set; }
        public string? Metakey { get; set; }
        public string? Metadata { get; set; }
        public List<Audios.Audios> ListItemsAudios { get; set; } = new List<Audios.Audios>();


    }
	
	public class CategoriesVideosModel {
        public List<CategoriesVideos> ListItems { get; set; } = new List<CategoriesVideos>();        
        public List<Audios.Audios> ListItemsAudios { get; set; } = new List<Audios.Audios>();
        public List<SelectListItem> ListItemsParents { get; set; } = new List<SelectListItem>();
        public SearchCategoriesVideos SearchData { get; set; } = new SearchCategoriesVideos() { };
        public CategoriesVideos Item { get; set; } = new CategoriesVideos() { };
        public PartialPagination Pagination { get; set; } = new PartialPagination() { };
        
    }

    public class SearchCategoriesVideos {
        public int CurrentPage { get; set; }
        public int ItemsPerPage { get; set; }
        public string? Keyword { get; set; }
        
    }
}
