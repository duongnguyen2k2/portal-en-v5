using System;
using System.Collections.Generic;
using System.Linq;
using API.Areas.Admin.Models.CategoriesVideos;
using API.Areas.Admin.Models.DMCoQuan;
using API.Areas.Admin.Models.DocumentRefers;
using API.Areas.Admin.Models.DocumentRefersCategories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace API.Controllers
{
    public class DocumentRefersController : Controller
    {
        public IActionResult Index(string alias, int id, [FromQuery] SearchDocumentRefers dto)
        {
            string ThongTinCoQuan = HttpContext.Session.GetString("ThongTinCoQuan");
            DMCoQuan ItemCoQuan = JsonConvert.DeserializeObject<DMCoQuan>(ThongTinCoQuan);
            

            DocumentRefersCategories categories = DocumentRefersCategoriesService.GetItem(id, "");
            if (categories == null)
            {
                categories = new DocumentRefersCategories() { Title = "Tài liệu", Id = 0, Alias = "tai-lieu" };

            }
            int TotalItems = 0;
            dto.ItemsPerPage = 30;
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            DocumentRefersModel data = new DocumentRefersModel() { SearchData = dto };
            data.SearchData.Status = 0;
            data.SearchData.CatId = categories.Id;
            data.SearchData.IdCoQuan = ItemCoQuan.Id;
            data.Categories = categories;
            
            if (categories != null && categories.Id > 0)
            {
                data.ListItems = DocumentRefersService.GetListPaginationFE(data.SearchData, API.Models.Settings.SecretId + ControllerName);
                data.ListDocumentsCategories = DocumentRefersCategoriesService.GetListSelectItems(ItemCoQuan.Id,false);
                if (data.ListItems != null && data.ListItems.Count() > 0)
                {
                    TotalItems = data.ListItems[0].TotalRows;
                }
                data.Pagination = new Areas.Admin.Models.Partial.PartialPagination() { CurrentPage = data.SearchData.CurrentPage, ItemsPerPage = data.SearchData.ItemsPerPage, TotalItems = TotalItems, QueryString = Request.QueryString.ToString() };
                data.PaginationMobile = new Areas.Admin.Models.Partial.PartialPagination() { CurrentPage = data.SearchData.CurrentPage, ItemsPerPage = data.SearchData.ItemsPerPage, TotalItems = TotalItems, QueryString = Request.QueryString.ToString(), Link = 5 };

            }
            else
            {
                data.ListItems = DocumentRefersService.GetListPaginationFE(data.SearchData, API.Models.Settings.SecretId + ControllerName);
                data.ListDocumentsCategories = DocumentRefersCategoriesService.GetListSelectItems(ItemCoQuan.Id,false);
                if (data.ListItems != null && data.ListItems.Count() > 0)
                {
                    TotalItems = data.ListItems[0].TotalRows;
                }
                data.Pagination = new Areas.Admin.Models.Partial.PartialPagination() { CurrentPage = data.SearchData.CurrentPage, ItemsPerPage = data.SearchData.ItemsPerPage, TotalItems = TotalItems, QueryString = Request.QueryString.ToString() };
                data.PaginationMobile = new Areas.Admin.Models.Partial.PartialPagination() { CurrentPage = data.SearchData.CurrentPage, ItemsPerPage = data.SearchData.ItemsPerPage, TotalItems = TotalItems, QueryString = Request.QueryString.ToString(), Link = 5 };
            }
            return View(data);
        }

        public IActionResult Detail(string alias, int id)
        {
            int IdCoQuan = 1;
            if (HttpContext.Session.GetString("IdCoQuan") != null && HttpContext.Session.GetString("IdCoQuan") != "")
            {
                IdCoQuan = int.Parse(HttpContext.Session.GetString("IdCoQuan"));
            }

            DocumentRefersModel data = new DocumentRefersModel();
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            data.SearchData = new SearchDocumentRefers() { CurrentPage = 0, ItemsPerPage = 10, Keyword = "" };            
            data.Item = DocumentRefersService.GetItemFE(id, IdCoQuan, API.Models.Settings.SecretId + ControllerName);
            if(data.Item != null )
            {
                data.Item.Metadesc = data.Item.Metadata = data.Item.Introtext;
            }
            
            return View(data);
        }

        public IActionResult DetailTLH(string alias, int id, [FromQuery] string MatKhau)
        {
            int IdCoQuan = 1;
            if (HttpContext.Session.GetString("IdCoQuan") != null && HttpContext.Session.GetString("IdCoQuan") != "")
            {
                IdCoQuan = int.Parse(HttpContext.Session.GetString("IdCoQuan"));
            }

            DocumentRefersCategoriesModel data = new DocumentRefersCategoriesModel();
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            data.SearchData = new SearchDocumentRefersCategories() { CurrentPage = 0, ItemsPerPage = 10, Keyword = "" };


            data.Item = DocumentRefersCategoriesService.GetItemTLH(id, MatKhau);
            if (data.Item == null)
            {

            }
            else
            {
                
                SearchDocumentRefers dataRe = new SearchDocumentRefers()
                {
                    CatId = data.Item.Id,
                    IdCoQuan = IdCoQuan,
                    IssuedDateStart = DateTime.Now,
                    TaiLieuHop = true
                };
                data.ListItemsFile = DocumentRefersService.GetListPagination(dataRe, API.Models.Settings.SecretId + ControllerName);

            }
            return View(data);
        }


    }
}