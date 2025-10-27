using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using API.Areas.Admin.Models.ImportDBOld;
using API.Areas.Admin.Models.CategoriesArticles;
using API.Areas.Admin.Models.Articles;
using API.Areas.Admin.Models.Documents;
using API.Models;
using Newtonsoft.Json;
using System.Net;
using System.Web;

namespace API.Areas.Admin.Controllers
{
    public class ImportDBOldController : Controller
    {
        [Area("Admin")]
        public IActionResult Index(int Category_Id = 0)
        {
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            ImportDBOldModel data = new ImportDBOldModel() { };
            if (Category_Id == 1)
            {
                data.ListItems = ImportDBOldService.GetList();
                if (data.ListItems != null && data.ListItems.Count() > 0)
                {
                    for (int i = 0; i < data.ListItems.Count(); i++)
                    {
                        string Alias = API.Models.MyHelper.StringHelper.UrlFriendly(data.ListItems[i].Title);
                        string jsonencode = API.Models.MyHelper.StringHelper.RemoveHtmlTags(data.ListItems[i].Introtext);
                        string Introtext = HttpUtility.HtmlDecode(jsonencode).Trim();
                        var k = API.Areas.Admin.Models.Articles.ArticlesService.UpdateAlias(data.ListItems[i].Id, Alias, Introtext.Trim());
                    }
                }
                return Json(new API.Models.MsgSuccess() { Data = data.ListItems.Count() });
            }
            else if (Category_Id == 2)
            {
                List<CategoriesArticles> listCat = CategoriesArticlesService.GetList();
                if (listCat != null && listCat.Count() > 0)
                {
                    for (int i = 0; i < listCat.Count(); i++)
                    {
                        string Alias = API.Models.MyHelper.StringHelper.UrlFriendly(listCat[i].Title);
                        var k = API.Areas.Admin.Models.CategoriesArticles.CategoriesArticlesService.UpdateAlias(listCat[i].Id, Alias);
                    }
                }
                return Json(new API.Models.MsgSuccess() { Data = listCat.Count() });
            }
            else if (Category_Id == 3)
            {
                List<Documents> ListDoc = DocumentsService.GetList();

                if (ListDoc != null && ListDoc.Count() > 0)
                {
                    for (int i = 0; i < ListDoc.Count(); i++)
                    {
                        string Alias = API.Models.MyHelper.StringHelper.UrlFriendly(ListDoc[i].Title);
                        var k = DocumentsService.UpdateAlias(ListDoc[i].Id, Alias);
                    }
                }
                return Json(new API.Models.MsgSuccess() { Data = ListDoc.Count() });
            }
            return Json(new API.Models.MsgSuccess() {  });



        }

        public IActionResult UpdateAlias()
        {
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            ImportDBOldModel data = new ImportDBOldModel() { };
            data.ListItems = ImportDBOldService.GetList();
            if (data.ListItems != null && data.ListItems.Count() > 0)
            {
                for (int i = 0; i < data.ListItems.Count(); i++)
                {
                    string Alias = API.Models.MyHelper.StringHelper.UrlFriendly(data.ListItems[i].Title);
                   // var k = API.Areas.Admin.Models.Articles.ArticlesService.UpdateAlias(data.ListItems[i].Id, Alias);
                }
            }
            return Json(new API.Models.MsgSuccess() { Data = data.ListItems.Count() });
        }
    }
}