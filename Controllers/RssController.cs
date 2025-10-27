using Microsoft.AspNetCore.Mvc;
using API.Areas.Admin.Models.CategoriesArticles;
using API.Areas.Admin.Models.Articles;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using API.Models;
using API.Models.Utilities;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Collections.Generic;

namespace API.Controllers
{
    public class RssController : Controller
    {
        private string controllerName = "RssController";
        private string controllerSecret;
        public RssController(IConfiguration config)
        {
            controllerSecret = config["Security:SecretId"] + controllerName;
        }
        public IActionResult Index()
        {
            SearchCategoriesArticles dto = new SearchCategoriesArticles();
            CategoriesArticlesModel data = new CategoriesArticlesModel() { SearchData = dto };
            data.ListItems = CategoriesArticlesService.GetListPagination(data.SearchData, controllerSecret + HttpContext.Request.Headers["UserName"]);
           
            return View(data);
        }
        [Produces("application/xml")]
        public ActionResult<List<API.Models.Rss.Articles>> GetByCat(string alias, int id, [FromQuery] SearchArticles dto)
        {
            
            int IdCoQuan = 1;
            if (HttpContext.Session.GetString("IdCoQuan") != null && HttpContext.Session.GetString("IdCoQuan") != "")
            {
                IdCoQuan = int.Parse(HttpContext.Session.GetString("IdCoQuan"));
            }

            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            CategoriesArticles categories = CategoriesArticlesService.GetItem(id, API.Models.Settings.SecretId + ControllerName);

            dto.CatId = id;
            dto.IdCoQuan = IdCoQuan;
            dto.ShowStartDate = "01/01/2010";
            dto.Status = 1;
            dto.ItemsPerPage = 20;
            ArticlesModel data = new ArticlesModel() { SearchData = dto, Categories = categories };
            List<API.Models.Rss.Articles> ListItems = new List<Models.Rss.Articles>();
            data.ListItems = ArticlesService.GetListPagination(data.SearchData, API.Models.Settings.SecretId + ControllerName);

            string Domain = "";
            if (HttpContext.Request.IsHttps)
            {
                Domain = "https://" + HttpContext.Session.GetString("DomainName");
            }
            else
            {
                Domain = "http://" + HttpContext.Session.GetString("DomainName");
            }

            if (data.ListItems != null && data.ListItems.Count() > 0)
            {
                for (int i = 0; i < data.ListItems.Count(); i++)
                {

                    ListItems.Add(new Models.Rss.Articles()
                    {
                        Id = data.ListItems[i].Id,
                        Title = data.ListItems[i].Title,
                        Alias = data.ListItems[i].Alias,
                        CatId = data.ListItems[i].CatId,
                        IntroText = data.ListItems[i].IntroText,
                        Images = Domain + data.ListItems[i].Images,
                        CreatedDate = data.ListItems[i].CreatedDate,
                        ModifiedDate = data.ListItems[i].ModifiedDate,
                        Link = Domain + "/" + data.ListItems[i].Alias + "-" + data.ListItems[i].Id.ToString() + ".html",
                    });
                }
            }
            return ListItems;            
        }

        [Produces("application/xml")]
        public ActionResult<List<API.Models.Rss.Articles>> Featured([FromQuery] SearchArticles dto)
        {            
            int IdCoQuan = 1;
            
            if (HttpContext.Session.GetString("IdCoQuan") != null && HttpContext.Session.GetString("IdCoQuan") != "")
            {
                IdCoQuan = int.Parse(HttpContext.Session.GetString("IdCoQuan"));
            }

            dto.IdCoQuan = IdCoQuan;
            dto.ShowStartDate = "01/01/2000";
            dto.Status = 1;
            dto.FlagStatus = 2;
            dto.ItemsPerPage = 20;
            ArticlesModel data = new ArticlesModel() { SearchData = dto };
            data.ListItems = ArticlesService.GetListViewFeatured(data.SearchData);
            List<API.Models.Rss.Articles> ListItems = new List<Models.Rss.Articles>();

            string Domain = "";
            if (HttpContext.Request.IsHttps)
            {
                Domain = "https://" + HttpContext.Session.GetString("DomainName");
            }
            else
            {
                Domain = "http://" + HttpContext.Session.GetString("DomainName");
            }

            if (data.ListItems != null && data.ListItems.Count() > 0)
            {
                for (int i = 0; i < data.ListItems.Count(); i++)
                {

                    ListItems.Add(new Models.Rss.Articles()
                    {
                        Id = data.ListItems[i].Id,
                        Title = data.ListItems[i].Title,
                        Alias = data.ListItems[i].Alias,
                        CatId = data.ListItems[i].CatId,
                        IntroText = data.ListItems[i].IntroText,
                        Images = Domain  + data.ListItems[i].Images,
                        CreatedDate = data.ListItems[i].CreatedDate,
                        ModifiedDate = data.ListItems[i].ModifiedDate,
                        Link = Domain + "/" + data.ListItems[i].Alias + "-" + data.ListItems[i].Id.ToString() + ".html",
                    });
                }
            }
            return ListItems;

        }


        [Produces("application/xml")]
        public ActionResult<List<API.Models.Rss.Articles>> Notification([FromQuery] SearchArticles dto)
        {
            
            int IdCoQuan = 1;
            if (HttpContext.Session.GetString("IdCoQuan") != null && HttpContext.Session.GetString("IdCoQuan") != "")
            {
                IdCoQuan = int.Parse(HttpContext.Session.GetString("IdCoQuan"));
            }
            string Domain = "";
            if (HttpContext.Request.IsHttps)
            {
                Domain = "https://" + HttpContext.Session.GetString("DomainName");
            }
            else
            {
                Domain = "http://" + HttpContext.Session.GetString("DomainName");
            }
            dto.IdCoQuan = IdCoQuan;
            dto.ShowStartDate = "01/01/2000";
            dto.Status = 1;
            dto.FlagStatus = 2;
            dto.ItemsPerPage = 15;
            ArticlesModel data = new ArticlesModel() { SearchData = dto };
            data.ListItems = ArticlesService.GetListViewNotification(data.SearchData);

            List<API.Models.Rss.Articles> ListItems = new List<Models.Rss.Articles>();

            if (data.ListItems != null && data.ListItems.Count() > 0)
            {
                for(int i = 0; i < data.ListItems.Count(); i++)
                {
                    
                    ListItems.Add(new Models.Rss.Articles()
                    {
                        Id = data.ListItems[i].Id,
                        Title = data.ListItems[i].Title,
                        Alias = data.ListItems[i].Alias,
                        CatId = data.ListItems[i].CatId,
                        IntroText = data.ListItems[i].IntroText,
                        Images = Domain  + data.ListItems[i].Images,
                        CreatedDate = data.ListItems[i].CreatedDate,
                        ModifiedDate = data.ListItems[i].ModifiedDate,
                        Link = Domain + "/" + data.ListItems[i].Alias + "-" + data.ListItems[i].Id.ToString() + ".html",
                    });
                }
            }


            return ListItems;
        }
    }
}
