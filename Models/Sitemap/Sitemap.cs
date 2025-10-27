using System;

namespace API.Models.Sitemap
{
    public class SitemapUrl
    {
        public string Loc { get; set; }
        public DateTime LastMod { get; set; }
        public string ChangeFreq { get; set; }
        public double Priority { get; set; }
    }
}
