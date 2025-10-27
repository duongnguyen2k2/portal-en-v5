using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using API.Areas.Admin.Models.Partial;
namespace API.Areas.Admin.Models.DocumentsType
{
    public class DocumentsType
    {
		public string? Ids { get; set; }
        public int TotalRows { get; set; }
        public int Id { get; set; }
 		public string? Title { get; set; }
 		public string? Alias { get; set; }
 		public string? Description { get; set; }
 		public Boolean Status { get; set; }
 		public Boolean Deleted { get; set; }
 		public int CreatedBy { get; set; }
 		public DateTime? CreatedDate { get; set; }
 		public int? ModifiedBy { get; set; }
 		public int IdParent { get; set; }
 		public DateTime? ModifiedDate { get; set; }
 		
    }
	
	public class DocumentsTypeModel {
        public List<DocumentsType> ListItems { get; set; } = new List<DocumentsType>();
        public SearchDocumentsType SearchData { get; set; } = new SearchDocumentsType() { };
        public DocumentsType Item { get; set; } = new DocumentsType() { };
        public PartialPagination Pagination { get; set; } = new PartialPagination() { };
    }

    public class SearchDocumentsType {
        public int CurrentPage { get; set; }
        public int ItemsPerPage { get; set; }
        public string? Keyword { get; set; }
    }
}
