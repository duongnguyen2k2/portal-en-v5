using API.Areas.Admin.Models.Partial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Areas.Admin.Models.Search
{
    public class Search
    {
        public int Id { get;set;}
        public string? Title { get;set;}
        public string? Alias { get;set;}
        public int Flag { get;set;}        
        public int CatId { get;set;}        
        public string? IntroText { get; set; }
        public string? FullText { get; set; }
        public string? Images { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int TotalRows { get; set; }
    }

    public class SearchAll
    {
        public int CurrentPage { get; set; }
        public int ItemsPerPage { get; set; }
        public int CatId { get; set; }
        public int IdCoQuan { get; set; }
        public string? Keyword { get; set; }
    }

    public class SearchAllModel
    {
        public List<Search> ListItems { get; set; } = new List<Search>();
        public SearchAll SearchData { get; set; } = new SearchAll() { };    
        public int Flag { get; set; } 
        public PartialPagination Pagination { get; set; } = new PartialPagination() { };
    }
}
