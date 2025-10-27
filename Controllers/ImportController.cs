using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Areas.Admin.Models.ImportDBOld;
using API.Models.ImportVB;
using API.Models.Import;
using Microsoft.AspNetCore.Mvc;
using HtmlAgilityPack;
using API.Areas.Admin.Models.Articles;
using System.Net;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using API.Areas.Admin.Models.Documents;
using API.Areas.Admin.Models.DocumentsType;
using API.Areas.Admin.Models.DocumentsField;
using API.Areas.Admin.Models.DocumentsLevel;
using System.Net.Http;
using API.Models.ReadHtml;
using System.Text;
using System.Web;
using HtmlAgilityPack.CssSelectors.NetCore;
using static API.Areas.Admin.Models.ManagerFile.ManagerFile;
using Newtonsoft.Json;
using API.Areas.Admin.Models.Contacts;
using System.Globalization;

namespace API.Controllers
{
    public class ImportController : Controller
    {
       
        
        public string Link = "https://buonmathuot.daklak.gov.vn/web/guest";
        public string LinkNoSSL = "http://cumgar.daklak.gov.vn/web/guest";
        public string Domain = "https://buonmathuot.daklak.gov.vn";
        public string UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/105.0.0.0 Safari/537.36";
        public async Task<IActionResult> GetItemChangeImage([FromQuery] int Id=2)
        {
            for(int i = 1; i < 1042; i++)
            {
                Id = i;
                Articles Item = ArticlesService.GetItemTMP(Id);
                if (Item != null)
                {
                    HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();
                    
                    if (Item.ListDataFile != null && Item.ListDataFile.Count > 0 && Item.FullText!=null)
                    {
                        htmlDoc.LoadHtml(Item.FullText);
                        string dirPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                        
                       

                        for (int j = 0; j < Item.ListDataFile.Count(); j++)
                        {
                            

                            IList<HtmlNode> nodesImg = htmlDoc.QuerySelectorAll("img");
                            IList<HtmlNode> nodesA = htmlDoc.QuerySelectorAll("a");
                            if (nodesImg.Count() > 0)
                            {
                                for (int imgI = 0; imgI < nodesImg.Count(); imgI++)
                                {
                                    if (nodesImg[imgI].QuerySelector("img") != null)
                                    {                                        
                                        string imgRoot =  HttpUtility.UrlDecode(nodesImg[imgI].Attributes["src"].Value.ToString());
                                        if (imgRoot == Item.ListDataFile[j].Path)
                                        {
                                            string img = Item.ListDataFile[j].Img.Replace(dirPath, "").Trim();

                                            nodesImg[imgI].Attributes["src"].Value = img;
                                            htmlDoc.QuerySelectorAll("img")[imgI].Attributes["src"].Value = img;
                                        }
                                    }
                                }
                            }

                            if (nodesA.Count() > 0)
                            {
                                for (int nai = 0; nai < nodesA.Count(); nai++)
                                {
                                    if (nodesA[nai].Attributes["href"] != null)
                                    {
                                        string imgRoot = HttpUtility.UrlDecode(nodesA[nai].Attributes["href"].Value.ToString());
                                        if (imgRoot == Item.ListDataFile[j].Path)
                                        {
                                            string img = Item.ListDataFile[j].Img.Replace(dirPath, "").Trim();

                                            nodesA[nai].Attributes["href"].Value = img;
                                            htmlDoc.QuerySelectorAll("a")[nai].Attributes["href"].Value = img;
                                        }
                                    }
                                }
                            }
                                
                        }
                        ArticlesService.SaveItemTMPFulText(Id, htmlDoc.DocumentNode.OuterHtml);
                    }
                    else
                    {
                        ArticlesService.SaveItemTMPFulText(Id, Item.FullText);
                    }
                    
                }

            }
            return Json(new API.Models.MsgSuccess() { Data = null });
        }

        public async Task<IActionResult> DownloadFileInContnetALLTMT()// Tai laij tat ca ca file
        {
            string Cookie = "";
            int Id = 0;
            for (int i = 1; i < 1042; i++)
            {
                Id = i;
                Articles Item = ArticlesService.GetItemTMP(Id);
                if (Item != null)
                {
                    SearchImport dto = new SearchImport();
                    
                    ImportModel data = new ImportModel() { SearchData = dto, Item = new Import() };

                    List<TinymceFile> ListFile = new List<TinymceFile>();

                    if (Item.FullText != null)
                    {
                        ListFile = ReadHtmlService.GetAllFileInContent(Item.FullText);

                        for (int k = 0; k < ListFile.Count(); k++)
                        {
                            API.Models.MsgSuccess FlagFileDownload = save_file_from_url(ListFile[k].Img, Domain + ListFile[k].Path, Cookie, UserAgent);
                        }

                        data.ListItemsFile = ListFile;
                    }
                    
                }
            }

            return Json(new API.Models.MsgSuccess() { Data = null });
        }

        public async Task<IActionResult> ImportHoiDap([FromQuery] int page=1)
        {
            string LinkRaw = "https://buonmathuot.daklak.gov.vn/web/guest/hoi-dap?p_p_id=Hoidap_WAR_Hoidapportlet&p_p_lifecycle=0&p_p_state=normal&p_p_mode=view&p_p_col_id=column-2&p_p_col_pos=1&p_p_col_count=2&_Hoidap_WAR_Hoidapportlet_delta=10&_Hoidap_WAR_Hoidapportlet_keywords=&_Hoidap_WAR_Hoidapportlet_advancedSearch=false&_Hoidap_WAR_Hoidapportlet_andOperator=true&_Hoidap_WAR_Hoidapportlet_resetCur=false&_Hoidap_WAR_Hoidapportlet_cur="+page;
            
            Boolean flagSave = false;
            using var clientSession = new HttpClient();
            clientSession.DefaultRequestHeaders.Add("User-Agent", UserAgent);
            var contentSession = await clientSession.GetStringAsync(Link);
            string Cookie = ReadHtmlService.GetCookieVT(contentSession);

            //Boolean flagLoadChild = false;
            SearchContacts dto = new SearchContacts()
            {

            };
            ContactsModel data = new ContactsModel() {  };
            data.ListItems = new List<Contacts>();
            dto.Cookie = Cookie;

            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", UserAgent);
            client.DefaultRequestHeaders.Add("Cookie", dto.Cookie);

            string content = await client.GetStringAsync(LinkRaw);

            HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();
            htmlDoc.LoadHtml(content);

            //Get Danh sách văn bản                     
            var ContactNodes = htmlDoc.DocumentNode.SelectNodes("//table[@class='taglib-search-iterator']//tr");
            var count = 0;
            if (ContactNodes != null)
            {
                foreach (HtmlNode node in ContactNodes)
                {
                    string childLink = "";
                    try
                    {
                        

                        count = count + 1;
                        if (count > 2)
                        {
                            //childLink = node.SelectSingleNode("//a[@class='detail-link']").GetAttributeValue("href", "");
                            childLink = node.SelectNodes("./td/div/div/div[4]/a").FirstOrDefault().GetAttributeValue("href", "");
                            string strid = node.SelectNodes("./td/div/div/div[2]/a").FirstOrDefault().GetAttributeValue("href", "").Replace("#","").Trim();                            
                            int IdRoot = Int32.Parse(strid);
                            Contacts ItemSave = ContactsService.GetItemTMP(IdRoot);

                            if (ItemSave!=null && ItemSave.Id > 0)
                            {
                                flagSave = true;
                            }
                            else
                            {
                                flagSave = true;
                            }

                            if (flagSave)
                            {
                                Contacts item = new Contacts()
                                {
                                    LinkRoot = childLink,
                                    IdCoQuan = 0,
                                    Link = "",
                                    IdRoot = IdRoot
                                };

                                await Task.Delay(2000);

                                using var clientChild = new HttpClient();
                                clientChild.DefaultRequestHeaders.Add("User-Agent", UserAgent);
                                clientChild.DefaultRequestHeaders.Add("Cookie", dto.Cookie);
                                string contentChild = await clientChild.GetStringAsync(childLink);

                                HtmlDocument htmlDoc2 = new HtmlAgilityPack.HtmlDocument();
                                htmlDoc2.LoadHtml(contentChild);


                                var RowNodes = htmlDoc2.DocumentNode.SelectNodes("//table[@class='table table-bordered']//tr");
                                if (RowNodes.Count() > 0)
                                {
                                    item.DataHtml = contentChild;
                                    item.Introtext = htmlDoc2.DocumentNode.SelectNodes("//table[@class='table table-bordered']//tr[1]/td[1]").FirstOrDefault().InnerText;
                                    item.Introtext = item.Introtext.Replace("Câu hỏi:", "").Trim();

                                    item.Fullname = htmlDoc2.DocumentNode.SelectNodes("//table[@class='table table-bordered']//tr[2]/td[1]").FirstOrDefault().InnerText;
                                    item.Fullname = item.Fullname.Replace("Người gửi:", "").Trim();
                                    item.Email = htmlDoc2.DocumentNode.SelectNodes("//table[@class='table table-bordered']//tr[2]/td[2]").FirstOrDefault().InnerText;
                                    item.Email = item.Email.Replace("Email:", "").Trim();
                                    item.Address = htmlDoc2.DocumentNode.SelectNodes("//table[@class='table table-bordered']//tr[3]/td[1]").FirstOrDefault().InnerText;
                                    item.Address = item.Address.Replace("Địa chỉ:", "").Trim();
                                    item.Phone = htmlDoc2.DocumentNode.SelectNodes("//table[@class='table table-bordered']//tr[3]/td[2]").FirstOrDefault().InnerText;
                                    item.Phone = item.Phone.Replace("Số điện thoại:", "").Trim();
                                    item.LinhVuc = htmlDoc2.DocumentNode.SelectNodes("//table[@class='table table-bordered']//tr[4]/td[1]").FirstOrDefault().InnerText;
                                    item.LinhVuc = item.LinhVuc.Replace("Lĩnh vực:", "").Trim();
                                    string NgayGui = htmlDoc2.DocumentNode.SelectNodes("//table[@class='table table-bordered']//tr[4]/td[2]").FirstOrDefault().InnerText;
                                    NgayGui = NgayGui.Replace("Ngày gửi:", "").Trim();
                                    if (NgayGui != null && NgayGui != "")
                                    {
                                        item.CreatedDate = DateTime.ParseExact(NgayGui, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                    }

                                    item.NguoiTraLoi = htmlDoc2.DocumentNode.SelectNodes("//table[@class='table table-bordered'][2]//tr[1]/td[1]").FirstOrDefault().InnerText;
                                    item.NguoiTraLoi = item.NguoiTraLoi.Replace("Người trả lời:", "").Trim();

                                    string NgayTraLoi = htmlDoc2.DocumentNode.SelectNodes("//table[@class='table table-bordered'][2]//tr[2]/td[1]").FirstOrDefault().InnerText;
                                    NgayTraLoi = NgayTraLoi.Replace("Ngày trả lời:", "").Trim();
                                    if (NgayTraLoi != null && NgayTraLoi != "")
                                    {
                                        item.NgayTraLoi = DateTime.ParseExact(NgayTraLoi, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                    }
                                    item.Description = htmlDoc2.DocumentNode.SelectNodes("//table[@class='table table-bordered'][2]//tr[3]/td[1]").FirstOrDefault().InnerText;
                                    item.Description = item.Description.Replace("Trả lời:", "").Trim();

                                    var nodeLinkHtml = htmlDoc2.DocumentNode.SelectNodes("//table[@class='table table-bordered'][2]//tr[4]/td[1]/a");
                                    if (nodeLinkHtml != null)
                                    {
                                        HtmlNode nodeLink = nodeLinkHtml.FirstOrDefault();
                                        if (nodeLink != null)
                                        {
                                            item.Link = nodeLink.GetAttributeValue("href", "");
                                        }
                                    }
                                    
                                    
                                    string hits = htmlDoc2.DocumentNode.SelectNodes("//table[@class='table table-bordered'][2]//tr[5]/td[1]").FirstOrDefault().InnerText;
                                    hits = hits.Replace("Lượt xem:", "").Trim();
                                    item.Hits = Int32.Parse(hits);

                                    if (item.Link!=null && item.Link!="")
                                    {
                                        InfoFile infoFile = ReadHtmlService.GetInfoFileByUrl(item.Link);
                                        string urlDownloadContact = Domain + item.Link;

                                        API.Models.MsgSuccess FlagFileDownload = save_file_from_url(infoFile.DirFilePath, urlDownloadContact, dto.Cookie, UserAgent);

                                        if (!FlagFileDownload.Success)
                                        {
                                            item.Link = urlDownloadContact;
                                        }
                                       
                                    }
                                    var a = ContactsService.SaveItemTMP(item);
                                    data.ListItems.Add(item);

                                }

                                
                            }
                            else
                            {
                                //data.ListItems.Add(ItemSave);
                            }
                            
                        }

                    }
                    catch (Exception e)
                    {

                    }
                }
            }
            return Json(new API.Models.MsgSuccess() { Data = data});
        }
        public IActionResult SearchLink()
        {
            
            string LinkWeb = "https://buonmathuot.daklak.gov.vn/web/guest/71/-/asset_publisher/GNA4XwuMk6av/content/tb-y-kien-ket-luan-chi-%C4%91ao-cua-%C4%91ong-chi-le-nam-cao-%E2%80%93-chu-tich-ubnd-huyen-tai-cuoc-hop-giao-ban-tuan-15-nam-2022";
            //LinkWeb.Replace(LinkNoSSL, Link);
            //LinkWeb.Replace(Link, "").Trim();
            Boolean flagLinkWebRoot = false;
            if (LinkWeb.Contains(Domain))
            {
                flagLinkWebRoot = true;
            }
            else if(LinkWeb.Contains(LinkNoSSL))
            {
                flagLinkWebRoot = true;
            }
            else if (LinkWeb.Contains(Link))
            {
                flagLinkWebRoot = true;
            }
            
            return Json(new API.Models.MsgSuccess() { Msg = flagLinkWebRoot.ToString(), Data = Link });
        }

        public IActionResult UpdateDocumentTMPInfo()
        {
            List<DocumentsType> loaivanban = DocumentsTypeService.GetList();
            List<DocumentsField> linhvucvanban = DocumentsFieldService.GetList();
            List<DocumentsLevel> coquanbanhanh = DocumentsLevelService.GetList();

            List<Documents> ListItems = DocumentsService.GetListItemsTMP(2);
            if (ListItems != null && ListItems.Count() > 0)
            {
                for (int i = 0; i < ListItems.Count(); i++)
                {
                    DuLieuThongTin duLieuThongTin = JsonConvert.DeserializeObject<DuLieuThongTin>(ListItems[i].DuLieuThongTin);

                    if (loaivanban != null && loaivanban.Count() > 0) {
                        for (int vb = 0; vb < loaivanban.Count(); vb++)
                        {
                            
                            if (loaivanban[vb].Title == duLieuThongTin.LoaiVanBan)
                            {
                                ListItems[i].TypeId = loaivanban[vb].Id;
                            }
                        }
                    }// Loai Van Ban

                    if (linhvucvanban != null && linhvucvanban.Count() > 0)
                    {
                        for (int lv = 0; lv < linhvucvanban.Count(); lv++)
                        {

                            if (linhvucvanban[lv].Title == duLieuThongTin.LinhVuc)
                            {
                                ListItems[i].FieldId = linhvucvanban[lv].Id;
                            }
                        }
                    }// Linh Vuc Van Ban

                    if (coquanbanhanh != null && coquanbanhanh.Count() > 0)
                    {
                        for (int lv = 0; lv < coquanbanhanh.Count(); lv++)
                        {

                            if (coquanbanhanh[lv].Title == duLieuThongTin.CoQuanBanHanh)
                            {
                                ListItems[i].LevelId = coquanbanhanh[lv].Id;
                            }
                        }
                    }// CoQuanBanHanh

                    DocumentsService.SaveItemTMPDuLieuThongTin(ListItems[i]);

                }
            }

            return Json(new API.Models.MsgSuccess() {  });
        }


        public async Task<IActionResult> DownLoadFileDocument([FromQuery] string url, string Cookie)
        {
            //url = "/attachment/vanbanphapquy/2017/157db6dafdf673eb840cfd92a6f0f776716.pdf";
            
           
            List<Documents> ListItems = DocumentsService.GetListItemsTMP(2); 
            if(ListItems!=null && ListItems.Count() > 0)
            {
                for(int i = 0; i < ListItems.Count(); i++)
                {
                    InfoFile infoFile = ReadHtmlService.GetInfoFileByUrl(ListItems[i].Link, true, Domain);
                    API.Models.MsgSuccess FlagFileDownload = save_file_from_url(infoFile.DirFilePath, Domain + infoFile.Path, Cookie, UserAgent);
                }
            }
            
            return Json(new API.Models.MsgSuccess() { Msg ="Thanh cong"});
        }


        public async Task<IActionResult> UpdateInfoArticleTMP([FromQuery] string url,string Cookie)
        {
            if(Cookie==null || Cookie == "")
            {
                using var clientSession = new HttpClient();
                clientSession.DefaultRequestHeaders.Add("User-Agent", UserAgent);
                var contentSession = await clientSession.GetStringAsync(Link);
                Cookie = ReadHtmlService.GetCookieVT(contentSession);
            }
            Import Item = await ReadHtmlService.UpdateInfoArticleByLink(Domain, url,UserAgent, Cookie);
            Articles ItemArticleTMP = ArticlesService.GetArticleTMPByLink(url);
            if (ItemArticleTMP != null && ItemArticleTMP.Id > 0)
            {
                if (Item != null && Item.Status == true)
                {
                    ItemArticleTMP.FullText = Item.FullText;
                    ItemArticleTMP.DataFile = Item.DataFile;
                }
            }
            return Json(new API.Models.MsgSuccess() { Data = Item });
        }

       

        public async Task<IActionResult> TestDownLoadImageDocumentsChoMa([FromQuery] string url)
        {
            if (url == null || url == "")
            {
                //url = "/documents/10181/751733/10-12+t%E1%BA%ADp+hu%E1%BA%A5n+tnmt+anh+Th%E1%BA%AFng.JPG/2c831795-c625-4ccc-9476-737978629eb8?t=1670639287248";
                //url = "/documents/10181/587890/599.pdf/410421db-2b53-455b-a4e4-aac395f3a498";
                url = "/documents/10181/741999/15738-41.pdf?version=1.0";
            }

            InfoFile infoFile = ReadHtmlService.GetInfoFileByUrl(url, true, Domain);
            
            using var clientSession = new HttpClient();
            clientSession.DefaultRequestHeaders.Add("User-Agent", UserAgent);
            var contentSession = await clientSession.GetStringAsync(Link);
            string Cookie = ReadHtmlService.GetCookieVT(contentSession);

            API.Models.MsgSuccess FlagFileDownload = save_file_from_url(infoFile.DirFilePath, Domain + infoFile.FilePath, Cookie, UserAgent);
            
            return Json(new API.Models.MsgSuccess() { Data = infoFile });
        }

        public async Task<IActionResult> TestDownLoadImageJournal([FromQuery] string url)
        {
            if(url==null || url == "")
            {
                url = "/image/journal/article?img_id=753873&=1670639419800";
            }

            InfoFile infoFile = ReadHtmlService.GetInfoFileByUrl(url,true, Domain);


            using var clientSession = new HttpClient();
            clientSession.DefaultRequestHeaders.Add("User-Agent", UserAgent);
            var contentSession = await clientSession.GetStringAsync(Link);
            string Cookie = ReadHtmlService.GetCookieVT(contentSession);

            API.Models.MsgSuccess FlagFileDownload = save_file_from_url(infoFile.DirFilePath, Domain + url, Cookie, UserAgent);

            return Json(new API.Models.MsgSuccess() { Data = infoFile });
        }

        public async Task<IActionResult> TestDownLoadImage([FromQuery] string url)
        {

            InfoFile infoFile = ReadHtmlService.GetInfoFileByUrl(url,true,Domain);
            
            using var clientSession = new HttpClient();
            clientSession.DefaultRequestHeaders.Add("User-Agent", UserAgent);
            var contentSession = await clientSession.GetStringAsync(Link);
            string Cookie = ReadHtmlService.GetCookieVT(contentSession);

            save_file_from_url(infoFile.DirFilePath, Domain + url, Cookie, UserAgent);

            return Json(new API.Models.MsgSuccess() { Data = infoFile });
        }
        public async Task<IActionResult> DownloadFileInContnet([FromQuery] SearchImport dto)
        {
            
            dto.Link = "http://buonmathuot.daklak.gov.vn/-/ket-luan-tai-buoi-lam-viec-voi-lanh-ao-phong-quan-ly-o-thi-va-oi-quan-ly-trat-tu-canh-quan-o-thi-thanh-pho-cong-tac-quan-ly-trat-tu-o-thi-tren-ia-ban-";
            ImportModel data = new ImportModel() { SearchData = dto, Item = new Import() };

            List<TinymceFile> ListFile = new List<TinymceFile>();
            
            if (dto.Link != null)
            {

                using var clientSession = new HttpClient();
                clientSession.DefaultRequestHeaders.Add("User-Agent", UserAgent);
                var contentSession = await clientSession.GetStringAsync(Link);
                string Cookie = ReadHtmlService.GetCookieVT(contentSession);

                using var clientChild = new HttpClient();
                //clientChild.DefaultRequestHeaders.Add("User-Agent", UserAgent);
                //clientChild.DefaultRequestHeaders.Add("Cookie", Cookie);
                var contentChild = await clientChild.GetStringAsync(dto.Link);

                HtmlDocument htmlDoc2 = new HtmlAgilityPack.HtmlDocument();
                htmlDoc2.LoadHtml(contentChild);
                string FullText = HttpUtility.HtmlDecode(htmlDoc2.DocumentNode.SelectSingleNode("//div[@class='journal-content-article']").InnerHtml);

                ListFile = ReadHtmlService.GetAllFileInContent(FullText);

                for (int k = 0; k < ListFile.Count(); k++)
                {
                    API.Models.MsgSuccess FlagFileDownload = save_file_from_url(ListFile[k].Img, Domain + ListFile[k].Path, Cookie, UserAgent);
                }
            }

            data.ListItemsFile = ListFile;
            return Json(data);
        }


        public async Task<IActionResult> ImportVB([FromQuery] SearchImportVB dto, int SoTrang = 1)
        {
            Boolean flagLoadChild = false;
            ImportVBModel data = new ImportVBModel() { SearchData = dto };
            data.ListItems = new List<ImportVB>();
            
            dto.Link = "https://buonmathuot.daklak.gov.vn/van-ban-chi-ao-ieu-hanh?p_p_id=viewvbpqmanagement_WAR_unitechappportlet&p_p_lifecycle=0&p_p_state=normal&p_p_mode=view&p_p_col_id=column-3&p_p_col_count=1&_viewvbpqmanagement_WAR_unitechappportlet_javax.portlet.action=searchVanBan&_viewvbpqmanagement_WAR_unitechappportlet_loaivanbanid=0&_viewvbpqmanagement_WAR_unitechappportlet_trichyeu=&_viewvbpqmanagement_WAR_unitechappportlet_linhvucid=0&_viewvbpqmanagement_WAR_unitechappportlet_capbanhanhid=0&_viewvbpqmanagement_WAR_unitechappportlet_coquanbanhanhid=0&_viewvbpqmanagement_WAR_unitechappportlet_bdnbh=&_viewvbpqmanagement_WAR_unitechappportlet_ednbh=&_viewvbpqmanagement_WAR_unitechappportlet_bdnchl=&_viewvbpqmanagement_WAR_unitechappportlet_ednchl=&_viewvbpqmanagement_WAR_unitechappportlet_sokyhieu=&_viewvbpqmanagement_WAR_unitechappportlet_delta=10&_viewvbpqmanagement_WAR_unitechappportlet_keywords=&_viewvbpqmanagement_WAR_unitechappportlet_advancedSearch=false&_viewvbpqmanagement_WAR_unitechappportlet_andOperator=true&_viewvbpqmanagement_WAR_unitechappportlet_resetCur=false&_viewvbpqmanagement_WAR_unitechappportlet_cur=" + SoTrang.ToString();
            
            if (dto.Link != null && dto.Link!="")
            {
                //string UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/105.0.0.0 Safari/537.36";
                
                string Cookie = "";
               
                data.SearchData = dto;

                using var client = new HttpClient();
                client.DefaultRequestHeaders.Add("User-Agent", UserAgent);
                
                string content = await client.GetStringAsync(dto.Link);
                           

                HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();
                htmlDoc.LoadHtml(content);

                //Get Danh sách văn bản                     
                var ProductsNodes = htmlDoc.DocumentNode.SelectNodes("//table[@class='table table-bordered table-hover table-striped']//tr");
                var count = 0;
                if (ProductsNodes != null)
                {
                    foreach (HtmlNode node in ProductsNodes)
                    {
                        try
                        {

                            if (count< ProductsNodes.Count && node.SelectSingleNode(".//div[@class='vbpq-title']//a") != null)
                            {
                                try
                                {
                                    

                                    string childLink = node.SelectSingleNode(".//div[@class='vbpq-title']//a").GetAttributeValue("href", "");
                                    var item = new ImportVB()
                                    {
                                        IssuedDateShow = DateTime.Now.ToString("dd/MM/yyyy"),
                                        EffectiveDateShow = DateTime.Now.ToString("dd/MM/yyyy"),
                                        CatId = dto.CatId,
                                        CreatedBy = 1,
                                        ModifiedBy = 1,
                                        ListFile = new List<Areas.Admin.Models.Documents.FileDocuments>()
                                    };

                                    using var clientChild = new HttpClient();
                                    clientChild.DefaultRequestHeaders.Add("User-Agent", UserAgent);
                                    
                                    string contentChild = await client.GetStringAsync(childLink);

                                    HtmlDocument htmlDoc2 = new HtmlAgilityPack.HtmlDocument();
                                    htmlDoc2.LoadHtml(contentChild);


                                    var RowNodes = htmlDoc2.DocumentNode.SelectNodes("//table[@class='table table-bordered']//tr");
                                    item.Code = htmlDoc2.DocumentNode.SelectNodes("//table[@class='table table-bordered']//tr[1]/td[2]").FirstOrDefault().InnerText;
                                    item.IssuedDateShow = htmlDoc2.DocumentNode.SelectNodes("//table[@class='table table-bordered']//tr[2]/td[2]").FirstOrDefault().InnerText;
                                    item.EffectiveDateShow = htmlDoc2.DocumentNode.SelectNodes("//table[@class='table table-bordered']//tr[3]/td[2]").FirstOrDefault().InnerText;
                                    item.SignBy = htmlDoc2.DocumentNode.SelectNodes("//table[@class='table table-bordered']//tr[4]/td[2]").FirstOrDefault().InnerText;
                                    item.OrganizationName = htmlDoc2.DocumentNode.SelectNodes("//table[@class='table table-bordered']//tr[5]/td[2]").FirstOrDefault().InnerText;
                                    item.Introtext = htmlDoc2.DocumentNode.SelectNodes("//table[@class='table table-bordered']//tr[6]/td[2]").FirstOrDefault().InnerText;
                                    item.CatTitle = htmlDoc2.DocumentNode.SelectNodes("//table[@class='table table-bordered']//tr[8]/td[2]").FirstOrDefault().InnerText;
                                    item.LevelTitle = htmlDoc2.DocumentNode.SelectNodes("//table[@class='table table-bordered']//tr[9]/td[2]").FirstOrDefault().InnerText;
                                    item.FieldTitle = htmlDoc2.DocumentNode.SelectNodes("//table[@class='table table-bordered']//tr[10]/td[2]").FirstOrDefault().InnerText;
                                    
                                    //item.ExpriedStatus = htmlDoc2.DocumentNode.SelectNodes("//table[@class='table table-bordered']//tr[8]/td[2]").FirstOrDefault().InnerText;
                                    
                                    
                                    if(htmlDoc2.DocumentNode.SelectNodes("//table[@class='table table-bordered']//tr[11]/td[2]//a")!= null)
                                    {
                                        var myfile = new Areas.Admin.Models.Documents.FileDocuments();
                                        myfile.FilePath = htmlDoc2.DocumentNode.SelectNodes("//table[@class='table table-bordered']//tr[11]/td[2]//a").FirstOrDefault().GetAttributeValue("href", "");
                                        item.Link = htmlDoc2.DocumentNode.SelectNodes("//table[@class='table table-bordered']//tr[11]/td[2]//a").FirstOrDefault().GetAttributeValue("href", "");

                                        //save_file_from_url(myfile.FilePath, Domain + myfile.FilePath, Cookie, UserAgent);

                                        item.ListFile.Add(myfile);
                                    }
                                    if(htmlDoc2.DocumentNode.SelectNodes("//table[@class='table table-bordered']//tr[12]/td[1]") != null)
                                    {
                                        item.FullText = htmlDoc2.DocumentNode.SelectNodes("//table[@class='table table-bordered']//tr[12]/td[1]").FirstOrDefault().InnerText;
                                    }
                                  
                                    
                                    item.LinkRoot = childLink;

                                    

                                    DuLieuThongTin ItemDuLieuThongTin = new DuLieuThongTin()
                                    {
                                        CoQuanBanHanh = item.LevelTitle,
                                        LoaiVanBan = item.TypeTitle,
                                        LinhVuc = item.FieldTitle                                        
                                    };

                                    

                                    //Clone
                                    var vanban = new Documents()
                                    {
                                        IssuedDateShow = DateTime.Now.ToString("dd/MM/yyyy"),
                                        EffectiveDateShow = DateTime.Now.ToString("dd/MM/yyyy"),
                                        CatId = dto.CatId,
                                        LevelId = 2,
                                        TypeId = 4,
                                        Title = item.Introtext,
                                        Code = item.Code,
                                        Introtext = item.Introtext,
                                        ExpriedStatus = item.ExpriedStatus,
                                        OrganizationName = item.OrganizationName,
                                        SignBy = item.SignBy,
                                        Link = item.Link,
                                        ListFile = item.ListFile,
                                        Status = true,
                                        CreatedBy = 1,
                                        ModifiedBy = 1,
                                        FullText = item.FullText,
                                        LinkRoot = item.LinkRoot,
                                        DataContentHtml = contentChild,

                                    };
                                    vanban.DuLieuThongTin = JsonConvert.SerializeObject(ItemDuLieuThongTin);

                                    if (!string.IsNullOrEmpty(item.IssuedDateShow))
                                        vanban.IssuedDateShow = item.IssuedDateShow;
                                    if (!string.IsNullOrEmpty(item.EffectiveDateShow))
                                        vanban.EffectiveDateShow = item.EffectiveDateShow;


                                    /*
                                    //Loai vanban
                                    var loaivanban = DocumentsTypeService.GetItemByTitle(item.TypeTitle);
                                    if (loaivanban == null)
                                    {
                                        DocumentsType LoaiVanBanItem = new DocumentsType() { Title = item.TypeTitle, Alias = API.Models.MyHelper.StringHelper.UrlFriendly(item.TypeTitle), IdParent = 0, Status = true, CreatedBy = 1, ModifiedBy = 1 };
                                        var rsLoaiVanBan = DocumentsTypeService.SaveItem(LoaiVanBanItem);
                                        vanban.TypeId = rsLoaiVanBan.N;
                                    }
                                    else
                                    {
                                        vanban.TypeId = loaivanban.Id;
                                    }


                                    //Linh Vuc vanban
                                    var linhvucvanban = DocumentsFieldService.GetItemByTitle(item.TypeTitle);
                                    if (linhvucvanban == null)
                                    {
                                        DocumentsField LVVanBanItem = new DocumentsField() { Title = item.FieldTitle, Alias = API.Models.MyHelper.StringHelper.UrlFriendly(item.FieldTitle), CreatedBy = 1, ModifiedBy = 1 };
                                        var rslvVanBan = DocumentsFieldService.SaveItem(LVVanBanItem);
                                        vanban.FieldId = rslvVanBan.N;
                                    }
                                    else
                                    {
                                        vanban.FieldId = linhvucvanban.Id;
                                    }

                                    //Co quan ban hanh
                                    var coquanbanhanh = DocumentsLevelService.GetItemByTitle(item.TypeTitle);
                                    if (coquanbanhanh == null)
                                    {
                                        DocumentsLevel CQBHItem = new DocumentsLevel() { Title = item.LevelTitle, Alias = API.Models.MyHelper.StringHelper.UrlFriendly(item.LevelTitle) };
                                        var rscqVanBan = DocumentsLevelService.SaveItem(CQBHItem);
                                        vanban.LevelId = rscqVanBan.N;
                                    }
                                    else
                                    {
                                        vanban.LevelId = coquanbanhanh.Id;
                                    }*/


                                    try
                                    {
                                        DocumentsService.SaveItemTMP(vanban);

                                        data.ListItems.Add(item);
                                    }
                                    catch { }

                                }
                                catch(Exception e)
                                {

                                }
                            }

                            count = count + 1;
                        }
                        catch
                        {
                            throw;
                        }


                    }
                    //End 
                }// If null ProductsNodes

            }

            
            return View(data);
        }

        public IActionResult Index([FromQuery] SearchImport dto)
        {
            ImportModel data = new ImportModel() {  };
            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromBody]SearchImport dto, string btn = "search")
        {
            Boolean flagLoadChild = false;            
            ImportModel data = new ImportModel() { SearchData = dto };
            data.ListItems = new List<Import>();
           
            //;
            //dto.Link = "";
            if (dto.Link != null)
            {
                string Cookie = "";
                data.SearchData = dto;

                using var client = new HttpClient();
                client.DefaultRequestHeaders.Add("User-Agent", UserAgent);
                

                string content = await client.GetStringAsync(dto.Link);
               
                HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();
                
                htmlDoc.LoadHtml(content);

                //Get Danh sách bài viết                         
                var ProductsNodes = htmlDoc.DocumentNode.SelectNodes("//div[@class='nd_detail']");

                if (ProductsNodes == null)
                {

                }
                else
                {
                    foreach (HtmlNode node in ProductsNodes)
                    {
                        try
                        {
                            var item = new Import() { PublishUp = DateTime.Now, PublishUpShow = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), Status = true, LangId = "VI", CreatedBy = 1, IdCoQuan = 1, ModifiedBy = 1 };
                            item.Str_Link = node.SelectSingleNode("./div/a").GetAttributeValue("href", "");

                            item.Title = HttpUtility.HtmlDecode(node.SelectSingleNode("./div/a").InnerText);
                            var ngay = node.SelectSingleNode("./div/span").InnerText;
                            ngay = ngay.Replace("(", "");
                            ngay = ngay.Replace(")", "");
                            item.PublishUpShow = ngay;
                            item.IntroText = HttpUtility.HtmlDecode(node.SelectSingleNode("./p").InnerText);
                            if (node.SelectSingleNode("./a/img") != null)
                                item.Images = HttpUtility.HtmlDecode(node.SelectSingleNode("./a/img").GetAttributeValue("src", ""));



                            //DownloadFile
                            
                            if(!string.IsNullOrEmpty(item.Images))
                            {
                                InfoFile infoFile = ReadHtmlService.GetInfoFileByUrl(item.Images);
                                string url = Domain + item.Images;

                                if (infoFile.TypeFile==1)// Documents
                                {
                                   

                                    API.Models.MsgSuccess FlagFileDownload = save_file_from_url(infoFile.DirFilePath, url, Cookie, UserAgent);

                                    if (!FlagFileDownload.Success)
                                    {
                                        item.Images = url;
                                    }
                                }
                                else// journal
                                {
                                    API.Models.MsgSuccess FlagFileDownload = save_file_from_url(infoFile.DirFilePath, url, Cookie, UserAgent);

                                    if (FlagFileDownload.Success)
                                    {
                                        item.Images = infoFile.FilePath;
                                    }
                                    else
                                    {
                                        item.Images = url;
                                    }
                                } 

                            }


                            //Lấy chi tiết
                            try
                            {
                                using var clientChild = new HttpClient();
                                clientChild.DefaultRequestHeaders.Add("User-Agent", UserAgent);
                                if (Cookie != "NO_COOKIE")
                                {
                                    clientChild.DefaultRequestHeaders.Add("Cookie", Cookie);
                                }

                                Boolean flagLinkWebRoot = false;
                                if (item.Str_Link.Contains(Domain))
                                {
                                    flagLinkWebRoot = true;
                                }
                                else if (item.Str_Link.Contains(LinkNoSSL))
                                {
                                    flagLinkWebRoot = true;
                                }
                                else if (item.Str_Link.Contains(Link))
                                {
                                    flagLinkWebRoot = true;
                                }
                                var contentChild = "";

                                flagLinkWebRoot = true;

                                if (flagLinkWebRoot)
                                {
                                    contentChild = await clientChild.GetStringAsync(item.Str_Link);
                                }
                                else
                                {
                                    contentChild = await clientChild.GetStringAsync(Domain + item.Str_Link);
                                }
                                

                                HtmlDocument htmlDoc2 = new HtmlAgilityPack.HtmlDocument();
                                htmlDoc2.LoadHtml(contentChild);

                                HtmlNode journalContentNgay = htmlDoc2.DocumentNode.SelectSingleNode("//div[@class='nd_detail_chitiet']//div[@class='tieude-h2']/span");
                                if (journalContentNgay != null)
                                {
                                    if(item.PublishUpShow==null || item.PublishUpShow == "")
                                    {
                                        var ngayContent = journalContentNgay.InnerText;
                                        ngayContent = ngayContent.Replace("(", "");
                                        ngayContent = ngayContent.Replace(")", "");
                                        item.PublishUpShow = ngayContent;
                                    }
                                }
                                HtmlNode journalContentArticle = htmlDoc2.DocumentNode.SelectSingleNode("//div[@class='journal-content-article']");
                                if (journalContentArticle != null)
                                {
                                    flagLoadChild = true;
                                    item.FullText = HttpUtility.HtmlDecode(journalContentArticle.InnerHtml);

                                    List<TinymceFile> ListFile = ReadHtmlService.GetAllFileInContent(item.FullText);
                                    if (ListFile != null && ListFile.Count() > 0)
                                    {
                                        item.DataFile = JsonConvert.SerializeObject(ListFile);
                                        for(int k = 0; k < ListFile.Count(); k++)
                                        {
                                            API.Models.MsgSuccess FlagFileDownload = save_file_from_url(ListFile[k].Img, Domain+ListFile[k].Path, Cookie, UserAgent);
                                        }
                                    }
                                }
                                
                            }
                            catch
                            {

                            }

                            //Clone
                            var baiviet = new ArticlesModel() { Item = new Articles() { PublishUp = DateTime.Now, PublishUpShow = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), Status = true } };
                            baiviet.Item.Title = item.Title;
                            baiviet.Item.CatId = dto.CatId;
                            baiviet.Item.LinkRoot = item.Str_Link;
                            baiviet.Item.PublishUpShow = item.PublishUpShow;
                            baiviet.Item.IntroText = item.IntroText;
                            baiviet.Item.FullText = item.FullText;
                            baiviet.Item.Alias = API.Models.MyHelper.StringHelper.UrlFriendly(item.Title);                            
                            baiviet.Item.Status = flagLoadChild;
                            baiviet.Item.CreatedBy = 1;
                            baiviet.Item.ModifiedBy = 1;
                            baiviet.Item.IdCoQuan = 1;
                            baiviet.Item.Images = item.Images;
                            baiviet.Item.DataFile = item.DataFile;
                            ArticlesService.SaveItemTMP(baiviet.Item);
                            data.ListItems.Add(item);

                        }
                        catch
                        {
                            throw;
                        }


                    }
                    //End 
                }



            }



            return Json(new API.Models.MsgSuccess() { Data = data });
        }

        public API.Models.MsgSuccess save_file_from_url(string file_name, string url, string Cookie, string UserAgent)
        {
            if(UserAgent==null || UserAgent == "")
            {
                UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/105.0.0.0 Safari/537.36";
            }

            if (!System.IO.File.Exists(file_name))
            {
                byte[] content;
                try
                {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                    request.Headers.Add("User-Agent", UserAgent);
                    
                    WebResponse response = request.GetResponse();

                    Stream stream = response.GetResponseStream();

                    using (BinaryReader br = new BinaryReader(stream))
                    {
                        content = br.ReadBytes(70000000);
                        br.Close();
                    }
                    response.Close();

                    FileStream fs = new FileStream(file_name, FileMode.Create);
                    BinaryWriter bw = new BinaryWriter(fs);
                    try
                    {
                        bw.Write(content);
                    }
                    finally
                    {
                        fs.Close();
                        bw.Close();
                    }
                    return new API.Models.MsgSuccess() { };
                }
                catch (Exception e)
                {
                    return new API.Models.MsgSuccess() { Success = false };
                }
            }
            else
            {
                return new API.Models.MsgSuccess() { };
            }
                
            
        }

    }
}