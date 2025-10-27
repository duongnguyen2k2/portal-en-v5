using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using API.Areas.Admin.Models.Partial;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace API.Areas.Admin.Models.DocumentsCategories
{
    public class DocumentsCategories
    {
		public string? Ids { get; set; }
        public int TotalRows { get; set; }
        public int Id { get; set; }
        public int IdCoQuan { get; set; }
 		public string? Title { get; set; }
 		public string? Alias { get; set; }
 		public string? Description { get; set; }
 		public Boolean Status { get; set; }
 		public Boolean Deleted { get; set; }
 		public int CreatedBy { get; set; }
 		public DateTime? CreatedDate { get; set; }
 		public int? ModifiedBy { get; set; }
 		public DateTime? ModifiedDate { get; set; }
 		
    }
	
	public class DocumentsCategoriesModel {
        public List<DocumentsCategories> ListItems { get; set; } = new List<DocumentsCategories>();
        public SearchDocumentsCategories SearchData { get; set; } = new SearchDocumentsCategories() { };
        public DocumentsCategories Item { get; set; } = new DocumentsCategories() { };
        public PartialPagination Pagination { get; set; } = new PartialPagination() { };
        public List<SelectListItem> ListDMCoQuan { get; set; } = new List<SelectListItem> { };
    }

    public class SearchDocumentsCategories {
        public int CurrentPage { get; set; }
        public int ItemsPerPage { get; set; }
        public string? Keyword { get; set; }
        public int IdCoQuan { get; set; }
    }
}
