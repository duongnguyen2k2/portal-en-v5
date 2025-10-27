using System;
using System.Collections.Generic;
using System.Linq;
using API.Areas.Admin.Models.DocumentForms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace API.Controllers
{
    public class DocumentFormsController : Controller
    {
        public IActionResult Index([FromQuery] SearchDocumentForms dto)
        {
            int TotalItems = 0;
            dto.ItemsPerPage = 30;
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            DocumentFormsModel data = new DocumentFormsModel() { SearchData = dto };
            data.SearchData.Status = 0;
            data.ListItems = DocumentFormsService.GetListPagination(data.SearchData, API.Models.Settings.SecretId + ControllerName);
            if (data.ListItems != null && data.ListItems.Count() > 0)
            {
                TotalItems = data.ListItems[0].TotalRows;
            }
            data.Pagination = new Areas.Admin.Models.Partial.PartialPagination() { CurrentPage = data.SearchData.CurrentPage, ItemsPerPage = data.SearchData.ItemsPerPage, TotalItems = TotalItems, QueryString = Request.QueryString.ToString() };
            data.PaginationMobile = new Areas.Admin.Models.Partial.PartialPagination() { CurrentPage = data.SearchData.CurrentPage, ItemsPerPage = data.SearchData.ItemsPerPage, TotalItems = TotalItems, QueryString = Request.QueryString.ToString(),Link=5 };
           
            return View(data);
        }

        public IActionResult Detail(string alias, int id)
        {
            int IdCoQuan = 1;
            if (HttpContext.Session.GetString("IdCoQuan") != null && HttpContext.Session.GetString("IdCoQuan") != "")
            {
                IdCoQuan = int.Parse(HttpContext.Session.GetString("IdCoQuan"));
            }

            DocumentFormsModel data = new DocumentFormsModel();
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            data.SearchData = new SearchDocumentForms() { CurrentPage = 0, ItemsPerPage = 10, Keyword = "" };
            data.Item = DocumentFormsService.GetItem(id, API.Models.Settings.SecretId + ControllerName);
            data.Item.Metadesc = data.Item.Metadata = data.Item.Introtext;
            return View(data);

        }
    }
}