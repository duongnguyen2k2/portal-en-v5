using API.Areas.Admin.Models.Articles;
using API.Areas.Admin.Models.CategoriesArticles;
using API.Areas.Admin.Models.DMCoQuan;
using API.Areas.Admin.Models.Products;
using API.Models;
using HtmlAgilityPack.CssSelectors.NetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;

namespace API.Controllers
{
    public class ApiArticlesTransferController : Controller
    {
        private string controllerName = "ApiArticlesTransferController";
        private string controllerSecret;
        public ApiArticlesTransferController(IConfiguration config)
        {
            controllerSecret = config["Security:SecretId"] + controllerName;
        }
        public IActionResult Index()
        {
            return View();
        }

        //api
        public ActionResult<List<API.Areas.Admin.Models.DMCoQuan.DMCoQuan>> RetriveListDMCoQuan(string token)
        {
            string cmpToken = "4de6abe0-0ad7-424b-b504-344d9c3ce27c";
            if (token == null || token != cmpToken)
            {
                return Json(new MsgSuccess()
                {
                    Msg = "Token không hợp lệ!",
                    Code = "400",
                    Success = false,
                    //Data = null,
                });
            }
            else
            {
                ArticlesModel model = new ArticlesModel(){};  
                int ParentId = int.Parse(HttpContext.Session.GetString("ParentId"));
                model.ListDMCoQuan = DMCoQuanService.GetListByLoaiCoQuan(-1, 0, ParentId);
                return Json(new MsgSuccess()
                {
                    Data= model.ListDMCoQuan
                });
            }            
        }

        [HttpGet]
        public ActionResult GetListDMCoQuan()
        {
            string Domain = "";
            if (HttpContext.Request.IsHttps)
            {
                Domain = "https://" + HttpContext.Session.GetString("DomainName");
            }
            else
            {
                Domain = "http://" + HttpContext.Session.GetString("DomainName");
            }

            string link = Domain + "/ApiArticlesTransfer/RetriveListDMCoQuan?token=4de6abe0-0ad7-424b-b504-344d9c3ce27c";
            //string link = "https://localhost:44396/ApiArticlesTransfer/RetriveListDMCoQuan?token=4de6abe0-0ad7-424b-b504-344d9c3ce27c";

            var options = new RestClientOptions(link)
            {
                ThrowOnAnyError = true,
            };
            var client = new RestClient(options);
            var request = new RestRequest(link);
            var response = client.Get(request);
            MsgSuccess data = JsonConvert.DeserializeObject<MsgSuccess>(response.Content);
            return Json(data);
        }

        //api
        public ActionResult RetriveListCategoriesArticles(string token)
        {
            string cmpToken = "4de6abe0-0ad7-424b-b504-344d9c3ce27c";
            if (token == null || token != cmpToken)
            {
                return Json(new MsgSuccess()
                {
                    Msg = "Token không hợp lệ!",
                    Code = "400",
                    Success = false,
                    //Data = null,
                });
            }
            else
            {
                List<CategoriesArticles> ListItems = CategoriesArticlesService.GetList();
                return Json(new MsgSuccess() { Data = ListItems });
            }
        }

        [HttpGet]
        public ActionResult GetListCategoriesArticles() {
            string Domain = "";
            if (HttpContext.Request.IsHttps)
            {
                Domain = "https://" + HttpContext.Session.GetString("DomainName");
            }
            else
            {
                Domain = "http://" + HttpContext.Session.GetString("DomainName");
            }

            string link = Domain + "/ApiArticlesTransfer/RetriveListCategoriesArticles?token=4de6abe0-0ad7-424b-b504-344d9c3ce27c";
            //string link = "https://localhost:44396/ApiArticlesTransfer/RetriveListCategoriesArticles?token=4de6abe0-0ad7-424b-b504-344d9c3ce27c";

            var options = new RestClientOptions(link)
            {
                ThrowOnAnyError = true,
            };

            var client = new RestClient(options);
            var request = new RestRequest(link);
            var response = client.Get(request);
            MsgSuccess data = JsonConvert.DeserializeObject<MsgSuccess>(response.Content);
            return Json(data);
        }

        //api
        [HttpPost]
        public ActionResult<List<API.Models.Rss.Articles>> RetriveByCat([FromBody] SearchArticles dto, string token)
        {
            string cmpToken = "4de6abe0-0ad7-424b-b504-344d9c3ce27c";
            if (token == null || token != cmpToken)
            {
                return Json(new MsgSuccess()
                {
                    Msg = "Token không hợp lệ!",
                    Code = "400",
                    Success = false,
                    Data = null,
                });
            }
            else
            {
                string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                ArticlesModel data = new ArticlesModel() {};
                List<API.Areas.Admin.Models.Articles.Articles> ListItems = new List<API.Areas.Admin.Models.Articles.Articles>();
                data.ListItems = ArticlesService.GetListTransferPagination(dto, API.Models.Settings.SecretId + ControllerName);

                if (data.ListItems != null && data.ListItems.Count > 0)
                {
                    for (int i = 0; i < data.ListItems.Count; i++)
                    {

                        ListItems.Add(new API.Areas.Admin.Models.Articles.Articles()
                        {
                            Id = data.ListItems[i].Id,
                            Title = data.ListItems[i].Title,
                            Alias = data.ListItems[i].Alias,
                            CatId = data.ListItems[i].CatId,
                            IntroText = data.ListItems[i].IntroText,
                            CreatedDate = data.ListItems[i].CreatedDate,
                            ModifiedDate = data.ListItems[i].ModifiedDate,
                            TotalRows = data.ListItems[i].TotalRows,
                        });
                    }
                }
                return Json(new MsgSuccess() { Data = ListItems });
            }
        }

        [HttpPost]
        public ActionResult GetByCat([FromBody] SearchArticles dto)
        {
            string Domain = "";
            if (HttpContext.Request.IsHttps)
            {
                Domain = "https://" + HttpContext.Session.GetString("DomainName");
            }
            else
            {
                Domain = "http://" + HttpContext.Session.GetString("DomainName");
            }

            string link = Domain + "/ApiArticlesTransfer/RetriveByCat?token=4de6abe0-0ad7-424b-b504-344d9c3ce27c";
            //string link = "https://localhost:44396/ApiArticlesTransfer/RetriveByCat?token=4de6abe0-0ad7-424b-b504-344d9c3ce27c";

            var options = new RestClientOptions(link)
            {
                ThrowOnAnyError = true,
            };
            /*SearchArticles dto = new SearchArticles();*/

            var client = new RestClient(options);
            var request = new RestRequest(link);
            request.AddBody(dto);
            var response = client.Post(request);
            MsgSuccess data = JsonConvert.DeserializeObject<MsgSuccess>(response.Content);
            return Json(data);
        }

        //api
        public ActionResult<List<API.Areas.Admin.Models.Articles.Articles>> RetriveItem(int id, string token)
        {
            string cmpToken = "4de6abe0-0ad7-424b-b504-344d9c3ce27c";
            if (token == null || token != cmpToken)
            {
                return Json(new MsgSuccess()
                {
                    Msg = "Token không hợp lệ!",
                    Code = "400",
                    Success = false,
                    //Data = null,
                });
            }
            else
            {
                int IdCoQuan = 1;
                if (HttpContext.Session.GetString("IdCoQuan") != null && HttpContext.Session.GetString("IdCoQuan") != "")
                {
                    IdCoQuan = int.Parse(HttpContext.Session.GetString("IdCoQuan"));
                }

                string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
               /* CategoriesArticles categories = CategoriesArticlesService.GetItem(id, API.Models.Settings.SecretId + ControllerName);
                ViewData["Title"] = "Api - " + categories.Title;*/
                ArticlesModel data = new ArticlesModel() { };
                List<API.Areas.Admin.Models.Articles.Articles> ListItems = new List<API.Areas.Admin.Models.Articles.Articles>();
                data.Item = ArticlesService.GetItem(id, API.Models.Settings.SecretId + ControllerName);

                string Domain = "";
                if (HttpContext.Request.IsHttps)
                {
                    Domain = "https://" + HttpContext.Session.GetString("DomainName");
                }
                else
                {
                    Domain = "http://" + HttpContext.Session.GetString("DomainName");
                }
                ListItems.Add(new API.Areas.Admin.Models.Articles.Articles()
                {
                    Id = data.Item.Id,
                    Title = data.Item.Title,
                    IntroText = data.Item.IntroText,
                    FullText= data.Item.FullText,
                });
                return Json(new MsgSuccess() { Data = data.Item });
                //return Json(data.Item);
            }
        }
        [HttpGet]
        public ActionResult GetItem(string id)
        {
            string Domain;
            if (HttpContext.Request.IsHttps)
            {
                Domain = "https://" + HttpContext.Session.GetString("DomainName");
            }
            else
            {
                Domain = "http://" + HttpContext.Session.GetString("DomainName");
            }

            string link = Domain + "/ApiArticlesTransfer/RetriveItem?token=4de6abe0-0ad7-424b-b504-344d9c3ce27c&id=" + id;
            //string link = "https://localhost:44396/ApiArticlesTransfer/RetriveItem?token=4de6abe0-0ad7-424b-b504-344d9c3ce27c&id=" + id;

            var options = new RestClientOptions(link)
            {
                ThrowOnAnyError = true,
            };
            var client = new RestClient(options);
            var request = new RestRequest(link);
            var response = client.Get(request);
            MsgSuccess data = JsonConvert.DeserializeObject<MsgSuccess>(response.Content);
            return Json(data);
        }

        [HttpPost]
        public ActionResult SaveItemJson([FromBody] Articles model, string token)
        {
            string cmpToken = "4de6abe0-0ad7-424b-b504-344d9c3ce27c";
            if (token == null || token != cmpToken)
            {
                return Json(new MsgSuccess()
                {
                    Msg = "Token không hợp lệ!",
                    Code = "400",
                    Success = false,
                    //Data = null,
                });
            }
            else
            {
                int IdDC = 0;
                if (model != null)
                {
                    DMCoQuan CoQuan = new DMCoQuan() { };
                    CoQuan = DMCoQuanService.GetItem(model.IdCoQuan);
                    string DomainName = CoQuan.DomainName;
                    IdDC = Int32.Parse(MyModels.Decode(model.Ids, controllerSecret + HttpContext.Request.Headers["UserName"]).ToString());
                    ArticlesModel data = new ArticlesModel() { Item = model };
                    if (ModelState.IsValid)
                    {
                        if(model.RootNewsId.ToString() == null || model.RootNewsId.ToString() == "" || model.RootNewsId == 0)
                        {
                            model.RootNewsId = model.Id;
                        }else
                        {
                            model.RootNewsId = model.RootNewsId;
                        }
                        model.Id = 0;
                        model.Status = false;
                        model.LinkRoot = DomainName + "/articles-transfer-api/" + model.Alias+ "-" + model.RootNewsId + ".html";
                        string Fultext = "<iframe class=\"articles-transfer-iframe\" id=\"frame_bai_viet\" title=\"Chi tiết tin tức\" style=\"width: 100%; overflow: hidden; min-height: 1500px; border: 0px;\" src=\""+ model.LinkRoot +"\"></iframe>";
                        model.FullText= Fultext;
                        
                        // Start Table Category ----
                        DataTable tbItem = new DataTable();
                        tbItem.Columns.Add("ProductId", typeof(int));
                        tbItem.Columns.Add("CatId", typeof(int));
                        if (model.Alias == null || model.Alias == "")
                        {
                            model.Alias = API.Models.MyHelper.StringHelper.UrlFriendly(model.Title);
                        }

                        var DataSave = ArticlesService.SaveItem(model);

                        return Json(new MsgSuccess() { Data = MyModels.Encode(DataSave.N, controllerSecret + HttpContext.Request.Headers["UserName"]) });
                    }
                    else
                    {
                        var DataError = ModelState.Values.SelectMany(v => v.Errors)
                                       .Select(v => v.ErrorMessage + " " + v.Exception).ToList();

                        return Json(new MsgError() { Data = DataError });
                    }
                }
                else
                {
                    return Json(new MsgError() { Data = null });
                }
            }
        }

        [HttpPost]
        public ActionResult StoreItem([FromBody] Articles dto)
        {
            string Domain = "";
            if (HttpContext.Request.IsHttps)
            {
                Domain = "https://" + HttpContext.Session.GetString("DomainName");
            }
            else
            {
                Domain = "http://" + HttpContext.Session.GetString("DomainName");
            }

            string link = Domain + "/ApiArticlesTransfer/SaveItemJson?token=4de6abe0-0ad7-424b-b504-344d9c3ce27c";
            //string link = "https://localhost:44396/ApiArticlesTransfer/SaveItemJson?token=4de6abe0-0ad7-424b-b504-344d9c3ce27c";

            var options = new RestClientOptions(link)
            {
                ThrowOnAnyError = true,
            };
            /*SearchArticles dto = new SearchArticles();*/

            var client = new RestClient(options);
            var request = new RestRequest(link);
            request.AddBody(dto);
            var response = client.Post(request);
            MsgSuccess data = JsonConvert.DeserializeObject<MsgSuccess>(response.Content);
            return Json(data);
        }

        public IActionResult GetItemIframe(string alias, int Id)
        {
            /*int IdCoQuan = 1;
            if (HttpContext.Session.GetString("IdCoQuan") != null && HttpContext.Session.GetString("IdCoQuan") != "")
            {
                IdCoQuan = int.Parse(HttpContext.Session.GetString("IdCoQuan"));
            }*/
            ArticlesModel data = new ArticlesModel();
            data.Item = ArticlesService.GetItem(Id, controllerSecret + HttpContext.Request.Headers["UserName"]);
            /*CategoriesArticles categories = CategoriesArticlesService.GetItem(data.Item.CatId, "", 0, CultureInfo.CurrentCulture.Name.ToLower());
            data.Categories = categories;
            if (categories.Id != 0)
            {
                data.ListItems = ArticlesService.GetListRelativeNews(alias, categories.Id, IdCoQuan, CultureInfo.CurrentCulture.Name.ToLower());
            }*/
            return View(data);
        }
    }
}
