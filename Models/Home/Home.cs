using API.Areas.Admin.Models.Articles;
using API.Areas.Admin.Models.Banners;
using API.Areas.Admin.Models.CategoriesArticles;
using API.Areas.Admin.Models.CategoriesProducts;
using API.Areas.Admin.Models.Menus;
using API.Areas.Admin.Models.Products;
using API.Areas.Admin.Models.SYSParams;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models.Home
{
    public class HomeModel
    {
        public List<CategoriesProducts> ListCatProduct { get; set; }
        public List<CategoriesArticles> ListCatArticle { get; set; }
        public List<Articles> ListFeaturedArticles { get; set; }
        public List<Articles> ListNewArticles { get; set; }
        public List<Products> ListNewProducts { get; set; }
        public List<Articles> ListHotArticles { get; set; }
        public List<Products> ListFeaturedProducts { get; set; }
        public List<Banners> ListPartner { get; set; }
        public List<Banners> ListBanners { get; set; }
        public List<SelectListItem> ListItemsNam { get; set; }

    }
    public class PartialHeader
    {
        public SYSConfig SYSConfig { get; set; }
        public List<API.Areas.Admin.Models.Menus.Menus> ListMenus { get; set; }

    }
    
    public class PartialNewFeatured
    {
        public SYSConfig SYSConfig { get; set; }
        public API.Areas.Admin.Models.DMCoQuan.DMCoQuan ItemCoQuan { get; set; }
        public List<API.Areas.Admin.Models.Banners.Banners> ListBanners { get; set; }
        public List<Articles> ListHotArticles { get; set; }

    }

    public class PartialBanners
    {
        public SYSConfig SYSConfig { get; set; }
        public API.Areas.Admin.Models.DMCoQuan.DMCoQuan ItemCoQuan { get; set; }
        public List<API.Areas.Admin.Models.Banners.Banners> ListBanners { get; set; }

    }

    public class PartialListCatFeaturedHome
    {
        public SYSConfig SYSConfig { get; set; }
        public API.Areas.Admin.Models.DMCoQuan.DMCoQuan ItemCoQuan { get; set; }
        public List<API.Areas.Admin.Models.Banners.Banners> ListBanners { get; set; }
        public List<CategoriesArticles> ListCatFeaturedHome { get; set; }

    }

    public class PartialFooter
    {
        public SYSConfig SYSConfig { get; set; }
        public List<Banners> ListBanner { get; set; }
    }

}
