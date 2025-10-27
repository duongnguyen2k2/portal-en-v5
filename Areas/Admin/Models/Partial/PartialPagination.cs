using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Areas.Admin.Models.Partial
{
    public class PartialPagination
    {
        public decimal TotalItems { get; set; }// Tong ca trang
        //public int TotalPage { get; set; }// Tong ca trang
        public decimal CurrentPage { get; set; } // trang hien tai
        public decimal ItemsPerPage { get; set; } // So record / 1 trang
        public decimal Link { get; set; } = 0;
        public string? QueryString { get; set; }
    }
}
