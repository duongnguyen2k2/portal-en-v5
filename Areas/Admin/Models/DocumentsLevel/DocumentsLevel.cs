using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using API.Areas.Admin.Models.Partial;
namespace API.Areas.Admin.Models.DocumentsLevel
{
    public class DocumentsLevel
    {
        public string? Ids { get; set; }
        public int TotalRows { get; set; }
        public int Id { get; set; }
        public string? Alias { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public Boolean Status { get; set; }
        public Boolean Deleted { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

    }
	
	public class DocumentsLevelModel {
        public List<DocumentsLevel> ListItems { get; set; } = new List<DocumentsLevel>();
        public SearchDocumentsLevel SearchData { get; set; } = new SearchDocumentsLevel() { };
        public DocumentsLevel Item { get; set; } = new DocumentsLevel() { };
        public PartialPagination Pagination { get; set; } = new PartialPagination() { };
    }

    public class SearchDocumentsLevel {
        public int CurrentPage { get; set; }
        public int ItemsPerPage { get; set; }
        public string? Keyword { get; set; }
    }
}
