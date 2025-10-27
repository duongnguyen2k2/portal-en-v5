using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using API.Areas.Admin.Models.Partial;
using Microsoft.AspNetCore.Mvc.Rendering;
using API.Areas.Admin.Models.DMCoQuan;
namespace API.Areas.Admin.Models.Menus
{
    public class Menus
    {
		public string? Ids { get; set; }
        public int TotalRows { get; set; }
        public int Id { get; set; }
        public int IdCoQuan { get; set; }
 		public string? TitleMenu { get; set; }
 		public string? TenCoQuan { get; set; }
 		public string? Title { get; set; }
 		public string? Alias { get; set; }
        public string? Title_EN { get; set; }
        public string? Link_EN { get; set; }
        public int CatId { get; set; }
 		public int StaticId { get; set; }
 		public string? Link { get; set; }
 		public int ParentId { get; set; }
 		public int? ModifiedBy { get; set; }
 		public Boolean Status { get; set; }
 		public Boolean Deleted { get; set; }
 		public DateTime? ModifiedDate { get; set; }
 		public int? ArticleId { get; set; }
 		public int? Ordering { get; set; } 		
        public int ChildCount { get; set; }
        public int TypeShowId { get; set; }
        public string? Icon { get; set; }
        public string? Target { get; set; }
        public List<Menus> ListMenus { get; set; } = new List<Menus>();
    }

    public class TypeShow
    {
        public string? Title { get; set; }
        public string? Code { get; set; }
        public int Id { get; set; }
    }


    public class MenusModel {
        public List<Menus> ListItems { get; set; }    = new List<Menus>();   
        public SearchMenus SearchData { get; set; }= new SearchMenus();
        public List<SelectListItem> ListItemsCategories { get; set; } = new List<SelectListItem>() { };
        public List<SelectListItem> ListItemsTypeShow { get; set; } = new List<SelectListItem>() { };

        public List<SelectListItem> ListItemsMenus { get; set; } = new List<SelectListItem>() { };
        public List<SelectListItem> ListCatItems { get; set; } = new List<SelectListItem>() { };
        public List<SelectListItem> ListCatTarget { get; set; } = new List<SelectListItem> { };
        public List<SelectListItem> ListItemsArticle { get; set; } = new List<SelectListItem>() { };
        public Menus Item { get; set; } = new Menus() { };
        public PartialPagination Pagination { get; set; } = new PartialPagination() { };
        public List<SelectListItem> ListDMCoQuan { get; set; } = new List<SelectListItem>(){ };

    }

    public class SearchMenus {
        public int CurrentPage { get; set; }
        public int ItemsPerPage { get; set; }
        public int CatId { get; set; } = 0;
        public string? Keyword { get; set; }
        public int IdCoQuan { get; set; } = 1;

    }
}
