using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Areas.Admin.Models.SYSParams
{
    public class SYSParams
    {
        public string? IdType { get; set; }
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Introtext { get; set; }
        public string? Icon { get; set; }
        public string? Alias { get; set; }
        public int Ordering { get; set; }
        public int CountItem { get; set; }
        public Boolean Selected;
    }
    public class SearchSYSParams {
        public Boolean Selected { get; set; }
        public string? IdType { get; set; }
        public string? Alias { get; set; }
    }

    public class SYSConfigModel
    {
        public SYSConfig Item { get; set; }
    }

    public class SYSConfig
    {
        public string? WebsiteName { get; set; }
        public string? CompanyName { get; set; }
        public string? Slogan { get; set; }
        public string? Address { get; set; }        
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Facebook { get; set; }
        public string? Twitter { get; set; }
        public string? Footer { get; set; }
        public string? SEODescription { get; set; }
        public string? SEOKeyword { get; set; }
        public string? Contact { get; set; }
        public string? Map { get; set; }
        public string? Youtube { get; set; }       
        public string? CountYTST { get; set; }       
        public string? CopyrightTitle { get; set; }       
        public string? CopyrightPhone { get; set; }       
        public string? CopyrightLink { get; set; }       
        public string? CTNC { get; set; }       
        public string? EMC { get; set; }       
        public string? ImgDomain { get; set; }
	}
}