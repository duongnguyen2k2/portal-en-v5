using API.Areas.Admin.Models.Partial;
using System;
using System.Collections.Generic;

namespace API.Areas.Admin.Models.HTX
{
    public class HTX
    {
        public string? Ids { get; set; }
        public int TotalRows { get; set; }
        public int Id { get; set; }
        public int IdCoQuan { get; set; }
        public string? Title { get; set; }
        public string? TitleEn { get; set; }
        public string? Address { get; set; }
        public string? AddressEn { get; set; }
        public string? Description { get; set; }
        public string? DescriptionEn { get; set; }
        public Boolean Status { get; set; }
        public Boolean Deleted { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? Phone { get; set; }
        public string? Image { get; set; }
    }

    public class HTXModel
    {
        public HTX Item { get; set; } = new HTX() { };
        public List<HTX> ListItems { get; set; } = new List<HTX>();
        public SearchHTX SearchData { get; set; } = new SearchHTX() { };
        public PartialPagination Pagination { get; set; } = new PartialPagination() { };
    }

    public class SearchHTX
    {
        public int CurrentPage { get; set; }
        public int ItemsPerPage { get; set; }
        public int IdCoQuan { get; set; } = 0;
        public string? Keyword { get; set; }
    }
}
