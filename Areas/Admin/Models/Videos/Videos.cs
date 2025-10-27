using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using API.Areas.Admin.Models.Partial;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace API.Areas.Admin.Models.Videos
{
    public class Videos
    {
		public string? Ids { get; set; }
        public int TotalRows { get; set; }
        public int Id { get; set; }
 		public string? Title { get; set; }
 		public string? TitleEn { get; set; }
 		public string? Alias { get; set; }
 		public string? TitleType { get; set; }
 		public string? Description { get; set; }
 		public string? DescriptionEn { get; set; }
 		public string? Link { get; set; }
        public string? LinkSv { get; set; }
        public int CatId { get; set; } = 1;
        public string? CatTitle { get; set; }
        public string? Image { get; set; }
 		public Boolean Status { get; set; }
 		public Boolean Featured { get; set; }
 		public Boolean Deleted { get; set; }
 		public int CreatedBy { get; set; }
 		public string? CreatedByTitle { get; set; }
 		public DateTime CreatedDate { get; set; }
 		public int? ModifiedBy { get; set; }
 		public DateTime? ModifiedDate { get; set; }
 		public int IdType { get; set; }
 		public int IdCoQuan { get; set; }
        public int Ordering { get; set; }

    }
	
	public class VideosModel {
        public List<Videos> ListItems { get; set; } = new List<Videos>() { };
        public List<SelectListItem> ListDMCoQuan { get; set; } = new List<SelectListItem>() { };
        public List<SelectListItem> ListItemsType { get; set; } = new List<SelectListItem>() { };
        public List<SelectListItem> ListItemsCat { get; set; } = new List<SelectListItem>() { };
        public SearchVideos SearchData { get; set; } = new SearchVideos() {  };
        public Videos Item { get; set; } = new Videos() { };
        public PartialPagination Pagination { get; set; } = new PartialPagination() { };
        public CategoriesVideos.CategoriesVideos Categories { get; set; } = null;
        public int IdType { get; set; }
    }

    public class SearchVideos {
        public int CurrentPage { get; set; }
        public int ItemsPerPage { get; set; }
        public int IdType { get; set; }
        public string? Keyword { get; set; }
        public int Status { get; set; } = -1;
        public int IdCoQuan { get; set; }
        public int CatId { get; set; } = 1;
    }
}
