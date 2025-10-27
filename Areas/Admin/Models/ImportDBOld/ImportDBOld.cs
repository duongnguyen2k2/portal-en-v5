using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Areas.Admin.Models.ImportDBOld
{
    public class ImportDBOld
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Introtext { get; set; }
        public string? Content { get; set; }
        public string? ImageUrl { get; set; }
        public int Category_Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedUser { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int ModifiedUser { get; set; }
        public Boolean Apply { get; set; }
        public Boolean Display { get; set; }
        public string? Note { get; set; }
        public int ApplyUser { get; set; }
        public string? Des { get; set; }
    }

    public class ImportDBOldModel
    {
        public List<ImportDBOld> ListItems { get; set; }
        public SearchImportDBOld SearchData { get; set; }
        public ImportDBOld Item { get; set; }
       
    }
    public class SearchImportDBOld
    {
        public int CurrentPage { get; set; }
        public int ItemsPerPage { get; set; }
        public string? Keyword { get; set; }
    }
}
