using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using API.Areas.Admin.Models.Partial;
using API.Areas.Admin.Models.USMenu;
namespace API.Areas.Admin.Models.USGroups
{
    public class USGroups
    {
        public string? Ids { get; set; }
        public int TotalRows { get; set; }
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? ListMenuId { get; set; }
        public List<int> ListMenu { get; set; }
        public string? ListCatId { get; set; }
        public List<int> ListCat { get; set; }
        public Boolean Status { get; set; }
        public int? CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
        public Boolean? Deleted { get; set; }

    }

    public class USGroupsModel
    {
        public List<USGroups> ListItems { get; set; } = new List<USGroups>();
        public List<USMenu.USMenu> ListItemsMenu { get; set; } = new List<USMenu.USMenu>();
        public List<CategoriesArticles.CategoriesArticles> ListCategoriesArticle { get; set; } = new List<CategoriesArticles.CategoriesArticles>();
        public SearchUSGroups SearchData { get; set; } = new SearchUSGroups() { };
        public USGroups Item { get; set; } = new USGroups() { };
        public PartialPagination Pagination { get; set; } = new PartialPagination() { };
    }

    public class SearchUSGroups
    {
        public int CurrentPage { get; set; }
        public int ItemsPerPage { get; set; }
        public string? Keyword { get; set; }
    }
}
