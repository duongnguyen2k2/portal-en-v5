using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Areas.Admin.Models.SiteVisit
{
    public class SiteVisit
    {
        public int SiteVisitedCounter { get; set; }
        public int Id { get; set; }
    }
    public class SiteVisitDetail
    {
        public DateTime DateCreated { get; set; }
        public int Id { get; set; }
        public int Amount { get; set; }
        public int ShowDateCreated { get; set; }
    }
    public class SiteVisitResult
    {
        public string? Total { get; set; }
        public string? STR_Total { get; set; }
        public string? DateNow { get; set; }
        public string? DateOfWeek { get; set; }
        public string? DateOfMonth { get; set; }
        public string? Yesterday { get; set; }
        public string? OnlineUserCounter { get; set; }
    }
}
