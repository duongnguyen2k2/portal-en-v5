using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using HtmlAgilityPack;
using HtmlAgilityPack.CssSelectors.NetCore;
using static API.Areas.Admin.Models.ManagerFile.ManagerFile;
using API.Models.ImportVB;
using API.Models.Import;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net;

namespace API.Models.ReadHtml
{
    public class ReadHtml
    {
        public string? Title { get; set; }
        public string? Link { get; set; }
        public string? Time { get; set; }
        public string? IntroText { get; set; }
        public string? FullText { get; set; }
        public string? Image { get; set; }
    }

    public class ReadHtmlService
    {
        public static string ListHuyen(int IdCoQuan)
        {
            string Link = "";
            if (IdCoQuan == 13)
            {
                Link = "https://krongnang.daklak.gov.vn/";
            }
            else if (IdCoQuan == 10)
            {
                Link = "https://eahleo.daklak.gov.vn/";
            }
            else if (IdCoQuan == 6)
            {
                Link = "https://cukuin.daklak.gov.vn/";
            }
            return Link;
        }

        public static async Task<Import.Import>   UpdateInfoArticleByLink(string Domain,string Link,string UserAgent,string Cookie)
        {
            Import.Import item = new Import.Import() { PublishUp = DateTime.Now, PublishUpShow = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), Status = true, LangId = "VI", CreatedBy = 1, IdCoQuan = 1, ModifiedBy = 1 };
            Boolean flagLoadChild = false;
            //Lấy chi tiết
            try
            {
                using var clientChild = new HttpClient();
                clientChild.DefaultRequestHeaders.Add("User-Agent", UserAgent);
                clientChild.DefaultRequestHeaders.Add("Cookie", Cookie);
                var contentChild = await clientChild.GetStringAsync(Domain+Link);

                HtmlDocument htmlDoc2 = new HtmlAgilityPack.HtmlDocument();
                htmlDoc2.LoadHtml(contentChild);

                HtmlNode journalContentNgay = htmlDoc2.DocumentNode.SelectSingleNode("//div[@class='nd_detail_chitiet asset-content-custom']/h2/span");
                if (journalContentNgay != null)
                {
                    if (item.PublishUpShow == null || item.PublishUpShow == "")
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
                        for (int k = 0; k < ListFile.Count(); k++)
                        {
                            API.Models.MsgSuccess FlagFileDownload = ReadHtmlService.save_file_from_url(ListFile[k].Img, Domain + ListFile[k].Path, Cookie, UserAgent);
                        }
                    }
                }

            }
            catch
            {

            }
            item.Status = flagLoadChild;
            return item;
        }

        public static string GetCookieVT(string contentSession )
        {
            

            HtmlDocument docSession = new HtmlAgilityPack.HtmlDocument();
            docSession.LoadHtml(contentSession);

            HtmlNode scriptHTML = docSession.DocumentNode.SelectNodes("//body/script").First();
            string innerScript = scriptHTML.InnerText.Replace("document.cookie=\"", "");
            innerScript = innerScript.Trim().Replace("window.location.reload(true);", "");
            innerScript = innerScript.Trim().Replace("window.location.reload(true);", "");
            innerScript = innerScript.Replace("\"+\"", "");
            innerScript = innerScript.Replace("\" + \"", "");
            innerScript = innerScript.Replace("path=/\";", "path=/;");

            return innerScript;
        }
        
        public static List<TinymceFile> GetAllFileInContent(string contentSession, Boolean flagDownloadFile=true )
        {

            List<TinymceFile> ListFile = new List<TinymceFile>();

            HtmlDocument htmlContent = new HtmlAgilityPack.HtmlDocument();
            htmlContent.LoadHtml(contentSession);

            IList<HtmlNode> nodesImg = htmlContent.QuerySelectorAll("img");
            IList<HtmlNode> nodesA = htmlContent.QuerySelectorAll("a");
            if (nodesImg.Count() > 0)
            {
                for (int i = 0; i < nodesImg.Count(); i++)
                {

                    HtmlNode nodeImg = nodesImg[i].QuerySelector("img");
                    if (nodeImg != null)
                    {
                        TinymceFile item = new TinymceFile()
                        {
                            Path = HttpUtility.UrlDecode(nodeImg.Attributes["src"].Value.ToString()),
                            Extension = Path.GetExtension(HttpUtility.UrlDecode(nodeImg.Attributes["src"].Value.ToString())),
                            IsImage = true
                        };

                        List<string> fooArray = item.Extension.Split('?').ToList();
                        if (fooArray != null && fooArray.Count > 0)
                        {
                            item.Extension = fooArray[0];
                        }
                        InfoFile InfoFile = ReadHtmlService.GetInfoFileByUrl(item.Path);
                        item.Img = InfoFile.DirFilePath;
                        item.Name = InfoFile.Name;
                        item.PathParent = InfoFile.Folder;
                        ListFile.Add(item);
                    }
                    
                }

            }

            if (nodesA.Count() > 0)
            {
                for (int i = 0; i < nodesA.Count(); i++)
                {

                    HtmlNode nodeA = nodesA[i].QuerySelector("a");
                    if (nodeA != null)
                    {
                        if (nodeA.Attributes["href"] != null)
                        {
                            TinymceFile item = new TinymceFile()
                            {
                                Path = HttpUtility.UrlDecode(nodeA.Attributes["href"].Value.ToString()),
                                Extension = Path.GetExtension(HttpUtility.UrlDecode(nodeA.Attributes["href"].Value.ToString())),
                                IsImage = false
                            };

                            List<string> fooArray = item.Extension.Split('?').ToList();
                            if (fooArray != null && fooArray.Count > 0)
                            {
                                item.Extension = fooArray[0];
                            }
                            InfoFile InfoFile = ReadHtmlService.GetInfoFileByUrl(item.Path);
                            item.Img = InfoFile.DirFilePath;
                            item.Name = InfoFile.Name;
                            item.PathParent = InfoFile.Folder;

                            ListFile.Add(item);
                        }
                        
                    }
                    
                }

            }

            return ListFile;
        }

        public static InfoFile GetInfoFileByUrl(string url,Boolean flagFolder=true, string Domain= "hhh")
        {
            InfoFile info = new InfoFile()
            {
                Path = url,
                TypeFile = 0
            };

            if (url!=null && url != "")
            {
                url = HttpUtility.UrlDecode(url);
                int TypeFile = 1; // 1=documents, 2=journal
                                  //Tìm loại file Documents hoặc journal
                string FindUrl = url.Replace(Domain, "");
                List<string> ArrFindUrl = FindUrl.Split(new char[] { '/' }).ToList();
                if (ArrFindUrl != null && ArrFindUrl.Count() > 1)
                {
                    if (ArrFindUrl[0] == "" && ArrFindUrl[1].ToLower() == "image" && ArrFindUrl[2].ToLower() == "journal")
                    {
                        TypeFile = 2;
                    }
                    else if (ArrFindUrl[0] == "" && ArrFindUrl[1].ToLower() == "documents")
                    {
                        TypeFile = 1;
                    }
                    else if (ArrFindUrl[0] == "" && ArrFindUrl[1].ToLower() == "attachment")
                    {
                        TypeFile = 3;
                    }
                }
                //End Tìm loại file Documents hoặc journal


                info = new InfoFile()
                {
                    Path = url,
                    TypeFile = TypeFile
                };

                if (TypeFile == 1)//doucments
                {
                    /*
                    string file_name_root = Path.GetFileName(url);// Tên FIle có ?
                    string FilePathRoot = url.Replace(file_name_root, "").Trim(); // Lấy đường dẫn folder
                    FilePathRoot = FilePathRoot.Replace(".JPG/", ".JPG").Trim();
                    FilePathRoot = FilePathRoot.Replace(".jpg/", ".jpg").Trim();
                    info.Folder = FilePathRoot;*/

                    string file_name_root_1 = Path.GetFileName(url);// Tên FIle có ?
                    string file_name_root = url.Replace(file_name_root_1, "").Trim(); // Lấy đường dẫn folder
                    file_name_root = file_name_root.Replace(".JPG/", ".JPG").Trim();
                    file_name_root = file_name_root.Replace(".jpg/", ".jpg").Trim();
                    file_name_root = file_name_root.Replace(".pdf/", ".pdf").Trim();
                    file_name_root = file_name_root.Replace(".PDF/", ".PDF").Trim();
                    file_name_root = file_name_root.Replace(".png/", ".png").Trim();
                    file_name_root = file_name_root.Replace(".PNG/", ".PNG").Trim();
                    file_name_root = file_name_root.Replace(".doc/", ".doc").Trim();
                    file_name_root = file_name_root.Replace(".DOC/", ".DOC").Trim();
                    file_name_root = file_name_root.Replace(".DOCX/", ".DOCX").Trim();
                    file_name_root = file_name_root.Replace(".docx/", ".docx").Trim();
                    file_name_root = file_name_root.Replace(".xlsx/", ".xlsx").Trim();
                    file_name_root = file_name_root.Replace(".XLSX/", ".XLSX").Trim();
                    file_name_root = file_name_root.Replace(".XLS/", ".XLS").Trim();
                    file_name_root = file_name_root.Replace(".xls/", ".xls").Trim();

                    info.Name = Path.GetFileName(file_name_root);
                    if (info.Name == "")
                    {
                        file_name_root = Path.GetFileName(url);// Tên FIle có ?
                        string FilePathRoot = url.Replace(file_name_root, "").Trim(); // Lấy đường dẫn folder
                        info.Folder = FilePathRoot;
                        List<string> resultFileName = file_name_root.Split(new char[] { '?' }).ToList();
                        if (resultFileName != null && resultFileName.Count > 0)
                        {
                            info.Name = resultFileName[0]; // Lấy tên file sạch tenfile.jpg
                        }
                        info.FilePath = info.Folder + info.Name;
                    }
                    else
                    {
                        if (file_name_root.Length > 0)
                        {
                            info.FilePath = file_name_root; // Đường dẫn file /documents/10181/587890/599.pdf
                            info.Folder = file_name_root.Replace(info.Name, "").Trim(); // Đường dẫn folder
                        }
                    }
                    
                   

                    string dirPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot") + info.Folder; //G:\vnpt-daklak\portal-v2\API\wwwroot/documents/21554/0/
                    info.DirFilePath = dirPath + info.Name;
                    if (flagFolder)
                    {
                        try
                        {
                            if (!Directory.Exists(dirPath))
                            {
                                Directory.CreateDirectory(dirPath);
                            }
                        }
                        catch
                        {

                        }

                    }
                }
                else if (TypeFile == 2)
                {
                    string file_name_root = Path.GetFileName(url);// Tên FIle có ?
                    string FilePathRoot = url.Replace(file_name_root, "").Trim(); // Lấy đường dẫn folder
                    info.Folder = FilePathRoot;
                    List<string> resultFileName = file_name_root.Split(new char[] { '?' }).ToList();
                    if (resultFileName != null && resultFileName.Count > 0)
                    {
                        List<string> ListIdArticle = resultFileName[1].Split(new char[] { '=' }).ToList();
                        List<string> ListIdArticleAND = resultFileName[1].Split(new char[] { '&' }).ToList();
                        if (ListIdArticleAND != null && ListIdArticleAND.Count > 0)
                        {
                            ListIdArticle = ListIdArticleAND[0].Split(new char[] { '=' }).ToList();
                        }


                        if (ListIdArticle != null && ListIdArticle.Count() > 0)
                        {
                            info.Name = ListIdArticle[1].Trim() + ".jpg";
                        }
                        else
                        {
                            info.Name = DateTime.Now.Ticks.ToString() + ".jpg";
                        }
                    }

                    string dirPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot") + "/thumbs/";
                    info.DirFilePath = dirPath + info.Name;
                    info.FilePath = "/thumbs/" + info.Name;
                    info.Folder = "/thumbs/";
                    info.Path = url;
                }
                else if (TypeFile == 3)
                {
                    string file_name_root = Path.GetFileName(url);// Tên FIle có ?
                    string FilePathRoot = url.Replace(file_name_root, "").Trim(); // Lấy đường dẫn folder
                    info.Folder = FilePathRoot;
                    List<string> resultFileName = file_name_root.Split(new char[] { '?' }).ToList();
                    if (resultFileName != null && resultFileName.Count > 0)
                    {
                        info.Name = resultFileName[0]; // Lấy tên file sạch tenfile.jpg
                    }
                    info.FilePath = info.Folder + info.Name; ///documents/21554/0/New_9.10.jpg

                    string dirPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot") + info.Folder; //G:\vnpt-daklak\portal-v2\API\wwwroot/documents/21554/0/
                    info.DirFilePath = dirPath + info.Name;
                    if (flagFolder)
                    {
                        try
                        {
                            if (!Directory.Exists(dirPath))
                            {
                                Directory.CreateDirectory(dirPath);
                            }
                        }
                        catch
                        {

                        }

                    }
                }

            }
            else
            {
                
            }
            
            


            return info;
        }

        public static API.Models.MsgSuccess save_file_from_url(string file_name, string url, string Cookie, string UserAgent)
        {
            if (UserAgent == null || UserAgent == "")
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
                    request.Headers.Add("Cookie", Cookie);

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
