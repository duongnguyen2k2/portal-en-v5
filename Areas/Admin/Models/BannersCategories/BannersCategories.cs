using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using API.Areas.Admin.Models.Partial;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace API.Areas.Admin.Models.BannersCategories
{
    public class BannersCategories
    {
		public string? Ids { get; set; }
        public int TotalRows { get; set; }
        public int Id { get; set; }
        public int IdCoQuan { get; set; }
 		public string? Title { get; set; }
 		public string? Description { get; set; }
 		public Boolean Status { get; set; }
 		public Boolean Deleted { get; set; }
 		public int CreatedBy { get; set; }
 		public DateTime? CreatedDate { get; set; }
 		public int? ModifiedBy { get; set; }
 		public DateTime? ModifiedDate { get; set; }
 		
    }
	
	public class BannersCategoriesModel {
        public List<BannersCategories> ListItems { get; set; }  = new List<BannersCategories>();
        public List<SelectListItem> ListDMCoQuan { get; set; } = new List<SelectListItem>();       
        public SearchBannersCategories SearchData { get; set; } = new SearchBannersCategories() { };
        public BannersCategories Item { get; set; } = new BannersCategories() { };
        public PartialPagination Pagination { get; set; } = new PartialPagination() { };
    }

    public class SearchBannersCategories {
        public int CurrentPage { get; set; }
        public int ItemsPerPage { get; set; }
        public string? Keyword { get; set; }
        public int IdCoQuan { get; set; }
    }
}
