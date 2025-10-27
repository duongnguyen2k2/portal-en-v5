using API.Models.Sitemap;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace API.Controllers
{
    public class SitemapController : Controller
    {
        public IActionResult Index()
        {
            var urls = new List<SitemapUrl>
            {
                new SitemapUrl { Loc = "https://snnmt.daklak.gov.vn/", LastMod = DateTime.Now, ChangeFreq = "weekly", Priority = 1.0 },
                new SitemapUrl { Loc = "https://snnmt.daklak.gov.vn/tin-tuc", LastMod = DateTime.Now.AddDays(-1), ChangeFreq = "weekly", Priority = 0.8 }
            };

            XNamespace ns = "http://www.sitemaps.org/schemas/sitemap/0.9";

            var xml = new XDocument(
                new XDeclaration("1.0", "utf-8", null),
                new XElement(ns + "urlset",
                    urls.Select(u =>
                        new XElement(ns + "url",
                            new XElement(ns + "loc", u.Loc),
                            new XElement(ns + "lastmod", u.LastMod.ToString("yyyy-MM-dd")),
                            new XElement(ns + "changefreq", u.ChangeFreq),
                            new XElement(ns + "priority", u.Priority.ToString("F1"))
                        )
                    )
                )
            );

            return Content(xml.ToString(), "application/xml");
        }
    }
}
