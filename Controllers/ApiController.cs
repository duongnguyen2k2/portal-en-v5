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
using API.Areas.Admin.Models.DMTinhThanh;
using API.Areas.Admin.Models.DMPhuongXa;
using API.Areas.Admin.Models.DMQuanHuyen;
using API.Areas.Admin.Models.DMCoQuan;
using Newtonsoft.Json;
using API.Models.MyQRCode;
using DocumentFormat.OpenXml.InkML;

namespace API.Controllers
{
    public class ApiController : Controller
    {
        private string controllerName = "ApiController";
        private string controllerSecret;
        public ApiController(IConfiguration config)
        {
            controllerSecret = config["Security:SecretId"] + controllerName;
        }

        public IActionResult BuildDangKyTenMien([FromQuery] string Domain = "")
        {
            List<string> ListLinks = new List<string>();

            return Json(new MsgSuccess() { Data = ListLinks });
        }

        public IActionResult BuildQrCodeDomain([FromQuery] string Domain = "")
        {
            List<string> ListLinks = new List<string>();
            List<DMCoQuan> ListItems = DMCoQuanService.GetListByParent(1);
            foreach (DMCoQuan item in ListItems)
            {                
                MyQRCodeService service = new MyQRCodeService();                
                string Link = service.CreateQRDomain(item, item.Code, true);                
                //string Link2 = service.CreateQRDomain(item, item.Code, true);                
                ListLinks.Add(Link);
                //ListLinks.Add(Link2);
            }                    
            return Json(new MsgSuccess() { Data = ListLinks });
        }

        public IActionResult CreateQrCodeDomain([FromQuery] string Domain = "")
        {
            DMCoQuan ItemCoQuan = JsonConvert.DeserializeObject<DMCoQuan>(HttpContext.Session.GetString("ThongTinCoQuan"));

            Domain = "https://webxaphuong.vnptdaklak.vn/";
            if (string.IsNullOrEmpty(Domain))
            {
                Domain = HttpContext.Request.Host.Value.ToLower();
            }
            MyQRCodeService service = new MyQRCodeService();
            string Link = service.CreateQRDomain(ItemCoQuan, Domain, false);
            return Json(new MsgSuccess() { Data = Link });

        }
        public IActionResult GetListJsonDMTinhThanh()
        {
            List<DMTinhThanh> ListItems = DMTinhThanhService.GetList();
            return Json(new MsgSuccess() { Data = ListItems });
        }

        public IActionResult GetListJsonDMQuanHuyen([FromQuery] int IdTinhThanh)
        {
            List<DMQuanHuyen> ListItems = DMQuanHuyenService.GetListByTinhThanh(true, IdTinhThanh);
            return Json(new MsgSuccess() { Data = ListItems });
        }

        public IActionResult GetListJsonDMPhuongXa([FromQuery] int IdQuanHuyen)
        {
            List<DMPhuongXa> ListItems = DMPhuongXaService.GetListByQuanHuyen(IdQuanHuyen);
            return Json(new MsgSuccess() { Data = ListItems });
        }


        public IActionResult Index()
        {
            DMCoQuan ItemCoQuan = JsonConvert.DeserializeObject<DMCoQuan>(HttpContext.Session.GetString("ThongTinCoQuan"));
            SearchCategoriesArticles dto = new SearchCategoriesArticles();
            CategoriesArticlesModel data = new CategoriesArticlesModel() { SearchData = dto };
            if (ItemCoQuan.MetadataCV.flagShareApi == 1)
            {
                data.ListItems = CategoriesArticlesService.GetListPagination(data.SearchData, controllerSecret + HttpContext.Request.Headers["UserName"]);

            }
            else
            {
                return Redirect("/");
            }


            return View(data);
        }

        public IActionResult GetListCatArticleImport()
        {
            SearchCategoriesArticles dto = new SearchCategoriesArticles();
            CategoriesArticlesModel data = new CategoriesArticlesModel() { SearchData = dto };
            List<CategoriesArticles> ListItems = CategoriesArticlesService.GetListImport();
                ;
            return Json(new API.Models.MsgSuccess() { Data = ListItems });
        }

        public IActionResult GetListCatArticle()
        {
            DMCoQuan ItemCoQuan = JsonConvert.DeserializeObject<DMCoQuan>(HttpContext.Session.GetString("ThongTinCoQuan"));


            SearchCategoriesArticles dto = new SearchCategoriesArticles();
            CategoriesArticlesModel data = new CategoriesArticlesModel() { SearchData = dto };
            List<CategoriesArticles> ListItems = CategoriesArticlesService.GetListPagination(data.SearchData, controllerSecret + HttpContext.Request.Headers["UserName"]);
            return Json(new API.Models.MsgSuccess() { Data = ListItems });
        }
        [Produces("application/json")]
        public ActionResult<List<API.Models.Rss.Articles>> GetByCat(string alias, int id, [FromQuery] SearchArticles dto)
        {

            int IdCoQuan = 1;
            if (HttpContext.Session.GetString("IdCoQuan") != null && HttpContext.Session.GetString("IdCoQuan") != "")
            {
                IdCoQuan = int.Parse(HttpContext.Session.GetString("IdCoQuan"));
            }
            DMCoQuan ItemCoQuan = JsonConvert.DeserializeObject<DMCoQuan>(HttpContext.Session.GetString("ThongTinCoQuan"));
            
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            CategoriesArticles categories = CategoriesArticlesService.GetItem(id, API.Models.Settings.SecretId + ControllerName);

            dto.CatId = id;
            dto.IdCoQuan = IdCoQuan;
            dto.ShowStartDate = "01/01/2010";
            dto.Status = 1;
            dto.ItemsPerPage = 20;
            ViewData["Title"] = "Api - "+ categories.Title;
            ArticlesModel data = new ArticlesModel() { SearchData = dto, Categories = categories };
            List<API.Models.Rss.Articles> ListItems = new List<Models.Rss.Articles>();

            if (ItemCoQuan.MetadataCV.flagShareApi == 1)
            {
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
            }

            
            return ListItems;
        }

        [Produces("application/json")]
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
                        TotalRows = data.ListItems[i].TotalRows,
                        Title = data.ListItems[i].Title,
                        Alias = data.ListItems[i].Alias,
                        CatId = data.ListItems[i].CatId,
                        IntroText = data.ListItems[i].IntroText,
                        Images = Domain + data.ListItems[i].Images,
                        CreatedDate = data.ListItems[i].CreatedDate,
                        ModifiedDate = data.ListItems[i].ModifiedDate,
                        Link = Domain + "/" + data.ListItems[i].Alias+"-"+ data.ListItems[i].Id.ToString() + ".html",
                    });
                }
            }
            return ListItems;

        }


        [Produces("application/json")]
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
                for (int i = 0; i < data.ListItems.Count(); i++)
                {

                    ListItems.Add(new Models.Rss.Articles()
                    {
                        Id = data.ListItems[i].Id,
                        TotalRows = data.ListItems[i].TotalRows,
                        Title = data.ListItems[i].Title,
                        Alias = data.ListItems[i].Alias,
                        CatId = data.ListItems[i].CatId,
                        IntroText = data.ListItems[i].IntroText,
                        Images = Domain +  data.ListItems[i].Images,
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
