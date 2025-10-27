using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using API.Areas.Admin.Models.Partial;
using API.Areas.Admin.Models.CategoriesDoiThoai;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace API.Areas.Admin.Models.DoiThoai
{
    public class DoiThoai
    {
		public string? Ids { get; set; }
        public int TotalRows { get; set; }
        public int Id { get; set; }
 		public string? Alias { get; set; }
 		public string? Title { get; set; }
 		public string? Description { get; set; }
 		public string? CategoriesTitle { get; set; }
 		public Boolean Status { get; set; }
 		public Boolean Deleted { get; set; }
 		public int CreatedBy { get; set; }
 		public DateTime? CreatedDate { get; set; }
 		public int? ModifiedBy { get; set; }
 		public DateTime? ModifiedDate { get; set; }
 		public string? Link { get; set; }
 		public int SortOrder { get; set; }
 		public int CatId { get; set; }
 		public string? Target { get; set; }
 		public string? Image { get; set; }
 		public int IdCoQuan { get; set; }
 		public string? TenCoQuan { get; set; }
 		
    }
	
	public class DoiThoaiModel {
        public List<DoiThoai> ListItems { get; set; } = new List<DoiThoai>();
        public List<SelectListItem> ListItemsCategories { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> ListDMCoQuan { get; set; } = new List<SelectListItem>();
        public SearchDoiThoai SearchData { get; set; } = new SearchDoiThoai() { };
        public DoiThoai Item { get; set; } = new DoiThoai() { };
        public CategoriesDoiThoai.CategoriesDoiThoai Categories { get; set; } = new CategoriesDoiThoai.CategoriesDoiThoai() { };
        public PartialPagination Pagination { get; set; } = new PartialPagination() { };
    }

    public class SearchDoiThoai {
        public int CurrentPage { get; set; }
        public int ItemsPerPage { get; set; }
        public int CatId { get; set; }
        public int Status { get; set; }
        public int IdCoQuan { get; set; } = 0;
        public string? Keyword { get; set; }
    }
}
