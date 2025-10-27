using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using API.Areas.Admin.Models.Partial;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace API.Areas.Admin.Models.USMenu
{
	public class USMenuPermission
	{
		public int Id { get; set; }
		public string? Title { get; set; }
		public string? PathName { get; set; }
		public string? Icon { get; set; }
		public string? Controller { get; set; }
		public int ChildCount { get; set; }
		public int IdParent { get; set; }
		public Boolean ShowMenu { get; set; } = true;
		public string? Areas { get; set; }
		public string? Styles { get; set; }
	}
	public class USMenu
	{
		public string? Ids { get; set; }
		public int TotalRows { get; set; }
		public int Id { get; set; }
		public string? Title { get; set; }
		public string? TreeMenu { get; set; }
		public string? TreePath { get; set; }
		public string? PathName { get; set; }
		public string? Icon { get; set; }
		public string? Controller { get; set; }
		public int ChildCount { get; set; }
		public int IdParent { get; set; }
		public int IdCoQuan { get; set; }
		public string? Styles { get; set; }
		public string? Description { get; set; }
		public string? Areas { get; set; }
		public int SortOrder { get; set; }
		public Boolean Status { get; set; }
		public int? CreatedBy { get; set; }
		public DateTime? CreatedDate { get; set; }
		public int? ModifiedBy { get; set; }
		public DateTime? ModifiedDate { get; set; }
		public Boolean Deleted { get; set; }
		public Boolean ShowMenu { get; set; }

	}

	public class USMenuModel
	{
		public List<USMenu> ListItems { get; set; } = new List<USMenu>();
        public SearchUSMenu SearchData { get; set; } = new SearchUSMenu() { };
		public USMenu Item { get; set; } = new USMenu() { };
        public List<SelectListItem> ListItemsMenus { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> ListDMCoQuan { get; set; } = new List<SelectListItem>();
		public PartialPagination Pagination { get; set; } = new PartialPagination() { };
	}

	public class SearchUSMenu
	{
		public int CurrentPage { get; set; }
		public int ItemsPerPage { get; set; }
		public int IdCoQuan { get; set; }
		public string? Keyword { get; set; }
	}
}
