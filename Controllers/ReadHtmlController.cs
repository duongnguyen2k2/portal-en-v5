using API.Models.ReadHtml;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using HtmlAgilityPack.CssSelectors.NetCore;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.AspNetCore.Http;
using System.Text.RegularExpressions;
using System.Web;

namespace API.Controllers
{
    public class ReadHtmlController : Controller
    {
        public string UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/105.0.0.0 Safari/537.36";
        public IActionResult TestIframe()
        {

            return View();
        }
        public async Task<IActionResult> SoGiaoDucThongBao()
        {
            string Link = "http://gddt.daklak.gov.vn/";
            List<ReadHtml> ListNews = new List<ReadHtml>();

            using var clientSession = new HttpClient();
            clientSession.DefaultRequestHeaders.Add("User-Agent", UserAgent);
            var contentSession = await clientSession.GetStringAsync(Link);
            string Cookie = ReadHtmlService.GetCookieVT(contentSession);

            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", UserAgent);
            client.DefaultRequestHeaders.Add("Cookie", Cookie);
            var content = await client.GetStringAsync(Link);

            HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(content);

            IList<HtmlNode> nodes = doc.QuerySelectorAll("div.w-notification ul li");

            if (nodes.Count() > 0)
            {
                for (int i = 0; i < nodes.Count(); i++)
                {

                    HtmlNode node = nodes[i].QuerySelector("a");
                    HtmlNode nodeLale = nodes[i].QuerySelector("label");

                    ReadHtml item = new ReadHtml()
                    {
                        Title = node.InnerText.Replace("\n", "").Trim(),
                        Link = node.Attributes["href"].Value.ToString(),
                        Time = nodeLale.InnerText
                    };
                    ListNews.Add(item);
                }
            }

            return Json(new API.Models.MsgSuccess() { Data = ListNews });
        }

        public async Task<IActionResult> SoGiaoDuc()
        {
            string Link = "http://gddt.daklak.gov.vn/chuyenmuc/tin-tuc-su-kien/";
            List<ReadHtml> ListNews = new List<ReadHtml>();

            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", "C# console program");
            var content = await client.GetStringAsync(Link);

            HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(content);

            IList<HtmlNode> nodes = doc.QuerySelectorAll("#main-content .cat-content .cat-item");

            if (nodes.Count() > 0)
            {
                for (int i = 0; i < nodes.Count(); i++)
                {

                    HtmlNode nodeImg = nodes[i].QuerySelector(".cat-thumb a img");

                    IList<HtmlNode> nodeCpostInfo = nodes[i].QuerySelectorAll(".cat-caption .date p span");
                    
                    HtmlNode nodeTitle = nodes[i].QuerySelector(".cat-caption h2 a");

                    ReadHtml item = new ReadHtml()
                    {
                        Title = nodeTitle.InnerText.Replace("\n", "").Trim(),
                        Link = nodeTitle.Attributes["href"].Value.ToString(),
                        Time = nodeTitle.InnerText
                    };

                    if (nodeCpostInfo.Count() > 1)
                    {
                        HtmlNode nodeDate = nodeCpostInfo[1].QuerySelector("label");
                        if (nodeDate != null)
                        {
                            item.Time = nodeDate.InnerText;
                        }
                    }
                    
                    ListNews.Add(item);
                }
            }

            return Json(new API.Models.MsgSuccess() { Data = ListNews });
        }

        public async Task<IActionResult> Huyen_2()
        {


            string Link = "https://cumgar.daklak.gov.vn/";
            List<ReadHtml> ListNews = new List<ReadHtml>();

            List<API.Areas.Admin.Models.Articles.Articles> NotificationList = API.Areas.Admin.Models.Articles.ArticlesService.GetListNotification(2);

            if (NotificationList != null && NotificationList.Count > 0)
            {
                for (int i = 0; i < NotificationList.Count; i++)
                {
                    ListNews.Add(new ReadHtml()
                    {
                        Title = NotificationList[i].Title,
                        Link = Link + NotificationList[i].Alias + "-" + NotificationList[i].Id + ".html",
                        Time = NotificationList[i].PublishUpShow,
                        IntroText = NotificationList[i].IntroText,
                        FullText = NotificationList[i].FullText,
                        Image = NotificationList[i].Images,
                    });
                }
            }
            return Json(new API.Models.MsgSuccess() { Data = ListNews });
            
        }

        public IActionResult Huyen_4()
        {


            string Link = "https://buonmathuot.daklak.gov.vn/";
            List<ReadHtml> ListNews = new List<ReadHtml>();

            List<API.Areas.Admin.Models.Articles.Articles> NotificationList = API.Areas.Admin.Models.Articles.ArticlesService.GetListNotification(4);

            if (NotificationList != null && NotificationList.Count > 0)
            {
                for (int i = 0; i < NotificationList.Count; i++)
                {
                    ListNews.Add(new ReadHtml()
                    {
                        Title = NotificationList[i].Title,
                        Link = Link + NotificationList[i].Alias + "-" + NotificationList[i].Id + ".html",
                        Time = NotificationList[i].PublishUpShow,
                        IntroText = NotificationList[i].IntroText,
                        FullText = NotificationList[i].FullText,
                        Image = NotificationList[i].Images,
                    });
                }
            }
            return Json(new API.Models.MsgSuccess() { Data = ListNews });

        }

        public IActionResult Huyen_6()
        {
            string Link = "https://cukuin.daklak.gov.vn/";
            List<ReadHtml> ListNews = new List<ReadHtml>();

            List<API.Areas.Admin.Models.Articles.Articles> NotificationList = API.Areas.Admin.Models.Articles.ArticlesService.GetListNotification(6);

            if(NotificationList!=null && NotificationList.Count > 0)
            {
                for(int i = 0; i < NotificationList.Count; i++)
                {
                    ListNews.Add(new ReadHtml()
                    {
                        Title = NotificationList[i].Title,
                        Link = Link+NotificationList[i].Alias+"-"+ NotificationList[i].Id+".html",
                        Time = NotificationList[i].PublishUpShow,
                        IntroText = NotificationList[i].IntroText,
                        FullText = NotificationList[i].FullText,
                        Image = NotificationList[i].Images,
                    });
                }
            }
            return Json(new API.Models.MsgSuccess() { Data = ListNews });
        }

        public IActionResult Notification_Huyen_Root([FromQuery] int IdCoQuan)
        {
            string Link = ReadHtmlService.ListHuyen(IdCoQuan);
            
            List<ReadHtml> ListNews = new List<ReadHtml>();

            List<API.Areas.Admin.Models.Articles.Articles> ListItems = API.Areas.Admin.Models.Articles.ArticlesService.GetListNotification(IdCoQuan);

            if (ListItems != null && ListItems.Count > 0)
            {
                for (int i = 0; i < ListItems.Count; i++)
                {
                    ListNews.Add(new ReadHtml()
                    {
                        Title = ListItems[i].Title,
                        Link = Link + ListItems[i].Alias + "-" + ListItems[i].Id + ".html",
                        Time = ListItems[i].PublishUpShow,
                        IntroText = ListItems[i].IntroText,
                        FullText = ListItems[i].FullText,
                        Image = ListItems[i].Images,
                    });
                }
            }
            return Json(new API.Models.MsgSuccess() { Data = ListNews });
        }
        public IActionResult Hot_Huyen_Root([FromQuery] int IdCoQuan)
        {
            string Link = ReadHtmlService.ListHuyen(IdCoQuan);
            
            List<ReadHtml> ListNews = new List<ReadHtml>();
                        
            List<API.Areas.Admin.Models.Articles.Articles> ListItems = API.Areas.Admin.Models.Articles.ArticlesService.GetListHot(IdCoQuan);

            if (ListItems != null && ListItems.Count > 0)
            {
                for (int i = 0; i < ListItems.Count; i++)
                {
                    ListNews.Add(new ReadHtml()
                    {
                        Title = ListItems[i].Title,
                        Link = Link + ListItems[i].Alias + "-" + ListItems[i].Id + ".html",
                        Time = ListItems[i].PublishUpShow,
                        IntroText = ListItems[i].IntroText,
                        FullText = ListItems[i].FullText,
                        Image = ListItems[i].Images,
                    });
                }
            }
            return Json(new API.Models.MsgSuccess() { Data = ListNews });
        }

        public async Task<IActionResult> Huyen_6_Old()
        {
            string Link = "https://cukuin.daklak.gov.vn/";
            List<ReadHtml> ListNews = new List<ReadHtml>();

            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", "C# console program");
            var content = await client.GetStringAsync(Link);

            HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(content);

            IList<HtmlNode> nodes = doc.QuerySelectorAll(".marquee ul li");

            if (nodes.Count() > 0)
            {
                for (int i = 0; i < nodes.Count(); i++)
                {

                    HtmlNode node = nodes[i].QuerySelector("a");

                    ReadHtml item = new ReadHtml()
                    {
                        Title = node.InnerText.Replace("\n", "").Trim(),
                        Link = "http://cukuin.daklak.gov.vn" + node.Attributes["href"].Value.ToString(),
                        Time = ""
                    };
                    ListNews.Add(item);
                }
            }

            return Json(new API.Models.MsgSuccess() { Data = ListNews });
        }
        
        public async Task<IActionResult> Huyen_14()
        {
            string Link = "https://krongpac.daklak.gov.vn/web/guest/trang-chu";
            List<ReadHtml> ListNews = new List<ReadHtml>();

            using var clientSession = new HttpClient();
            clientSession.DefaultRequestHeaders.Add("User-Agent", UserAgent);
            var contentSession = await clientSession.GetStringAsync(Link);
            string Cookie = ReadHtmlService.GetCookieVT(contentSession);

            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", UserAgent);
            client.DefaultRequestHeaders.Add("Cookie", Cookie);
            var content = await client.GetStringAsync(Link);

            HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(content);

            IList<HtmlNode> nodes = doc.QuerySelectorAll("#portlet_101_INSTANCE_X3EPcQtArkOK marquee ul li");

            if (nodes.Count() > 0)
            {
                for (int i = 0; i < nodes.Count(); i++)
                {

                    HtmlNode node = nodes[i].QuerySelector("a");

                    ReadHtml item = new ReadHtml()
                    {
                        Title = node.InnerText.Replace("\n", "").Trim(),
                        Link = "https://krongpac.daklak.gov.vn" + node.Attributes["href"].Value.ToString(),
                        Time = ""
                    };
                    ListNews.Add(item);
                }
            }

            return Json(new API.Models.MsgSuccess() { Data = ListNews });
        }

        public IActionResult ThongBaoTuHuyen([FromQuery] int IdCoQuan=10)
        {
            
            List<ReadHtml> ListNews = new List<ReadHtml>();
            List<API.Areas.Admin.Models.Articles.Articles> ListArticle = Areas.Admin.Models.Articles.ArticlesService.ThongBaoTuHuyen(IdCoQuan);
            if (ListArticle != null && ListArticle.Count() > 0)
            {
                for (int i = 0; i < ListArticle.Count(); i++)
                {
                    ReadHtml item = new ReadHtml()
                    {
                        Title = ListArticle[i].Title,
                        Link = "https://"+ListArticle[i].CodeCoQuan+"/"+ ListArticle[i].Alias+"-"+ ListArticle[i].Id+".html",
                        Time = ListArticle[i].CreatedDate.ToString("dd/MM/yyyy"),
                        IntroText = ListArticle[i].IntroText,
                    };
                    ListNews.Add(item);
                }
            }
            return Json(new API.Models.MsgSuccess() { Data = ListNews });
        }

        public IActionResult TinNoiBatTuHuyen([FromQuery] int IdCoQuan=10)
        {
            
            List<ReadHtml> ListNews = new List<ReadHtml>();
            List<API.Areas.Admin.Models.Articles.Articles> ListArticle = Areas.Admin.Models.Articles.ArticlesService.TinNoiBatTuHuyen(IdCoQuan);
            if (ListArticle != null && ListArticle.Count() > 0)
            {
                for (int i = 0; i < ListArticle.Count(); i++)
                {
                    ReadHtml item = new ReadHtml()
                    {
                        Title = ListArticle[i].Title,
                        Link = "https://" + ListArticle[i].CodeCoQuan + "/" + ListArticle[i].Alias + "-" + ListArticle[i].Id + ".html",
                        Time = ListArticle[i].CreatedDate.ToString("dd/MM/yyyy"),
                        IntroText = ListArticle[i].IntroText,
                    };
                    ListNews.Add(item);
                }
            }
            return Json(new API.Models.MsgSuccess() { Data = ListNews });
        }

        public IActionResult Xa()
        {
            int IdCoQuan = int.Parse(HttpContext.Session.GetString("IdCoQuan"));
            List<ReadHtml> ListNews = new List<ReadHtml>();
            List<API.Areas.Admin.Models.Articles.Articles> ListArticle = Areas.Admin.Models.Articles.ArticlesService.LienThongTinCapXa(IdCoQuan);
            if (ListArticle != null && ListArticle.Count() > 0)
            {
                for (int i = 0; i < ListArticle.Count(); i++)
                {
                    ReadHtml item = new ReadHtml()
                    {
                        Title = ListArticle[i].Title,
                        Link = "http://"+ListArticle[i].CodeCoQuan+"/"+ ListArticle[i].Alias+"-"+ ListArticle[i].Id+".html",
                        Time = ListArticle[i].CreatedDate.ToString("dd/MM/yyyy"),
                        IntroText = ListArticle[i].IntroText,
                    };
                    ListNews.Add(item);
                }
            }
            return Json(new API.Models.MsgSuccess() { Data = ListNews });
        }

        public IActionResult DakLak()
        {
            string Link = "https://daklak.gov.vn/trang-chu/-/asset_publisher/Wc4AMpniWT49/rss";
            List<ReadHtml> ListNews = new List<ReadHtml>();
            XmlDocument doc1 = new XmlDocument();
            try
            {
                doc1.Load(Link);
                XmlElement root = doc1.DocumentElement;
                XmlNodeList nodes = root.SelectNodes("/rss/channel/item");
                foreach (XmlNode node in nodes)
                {
                    string tempf = node["title"].InnerText;
                    string tempc = node["link"].InnerText;
                    string feels = node["description"].InnerText;
                    string pubDate = node["pubDate"].InnerText;
                    ReadHtml item = new ReadHtml()
                    {
                        Title = node["title"].InnerText,
                        Link = node["link"].InnerText,
                        Time = node["pubDate"].InnerText,
                        IntroText = node["description"].InnerText,
                    };
                    ListNews.Add(item);
                }


                return Json(new API.Models.MsgSuccess() { Data = ListNews });
            }
            catch(Exception e) {
                return Json(new API.Models.MsgError() { Data = ListNews, Msg=e.ToString() });
            }
           
        }


        public IActionResult BoGiaoDuc()
        {
            string Link = "https://moet.gov.vn/rss/Pages/index.aspx?ItemID=54";
            List<ReadHtml> ListNews = new List<ReadHtml>();
            XmlDocument doc1 = new XmlDocument();
            doc1.Load(Link);
            XmlElement root = doc1.DocumentElement;
            XmlNodeList nodes = root.SelectNodes("/rss/channel/item");
            foreach (XmlNode node in nodes)
            {
                string tempf = node["title"].InnerText;
                string tempc = node["link"].InnerText;
                string feels = node["description"].InnerText;
                DateTime pubDate = DateTime.Now;
                try
                {
                    pubDate = DateTime.Parse(node["pubDate"].InnerText);
                }
                catch { 
                
                }

                ReadHtml item = new ReadHtml()
                {
                    Title = node["title"].InnerText,
                    Link = node["link"].InnerText,
                    Time = pubDate.ToString("dd/MM/yyyy"),
                    IntroText = node["description"].InnerText,
                };
                ListNews.Add(item);
            }
            return Json(new API.Models.MsgSuccess() { Data = ListNews });
        }

    }
}

