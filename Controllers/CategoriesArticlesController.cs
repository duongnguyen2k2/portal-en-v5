using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using API.Areas.Admin.Models.Articles;
using API.Areas.Admin.Models.CategoriesArticles;

namespace API.Controllers
{
    public class CategoriesArticlesController : Controller
    {
        public IActionResult Index(string alias)
        {
            return View();
        }

        public IActionResult GetListChildCat(string alias, int id)
        {
            CategoriesArticlesModel data = new CategoriesArticlesModel() { ListItems = null };

            CategoriesArticles categories = CategoriesArticlesService.GetItem(id, API.Models.Settings.SecretId + "CategoriesArticles");

            if (categories != null)
            {
                List<CategoriesArticles> ListItems = CategoriesArticlesService.GetListChild(id);
                data = new CategoriesArticlesModel() { ListItems = ListItems ,Item = categories };
            }
            
            return View(data);
        }

        public IActionResult ListChildCatNoImage(string alias, int id)
        {
            CategoriesArticlesModel data = new CategoriesArticlesModel() { ListItems = null };

            CategoriesArticles categories = CategoriesArticlesService.GetItem(id, API.Models.Settings.SecretId + "CategoriesArticles");

            if (categories != null)
            {
                List<CategoriesArticles> ListItems = CategoriesArticlesService.GetListChild(id);
                data = new CategoriesArticlesModel() { ListItems = ListItems, Item = categories };
            }

            return View(data);
        }
    }
}