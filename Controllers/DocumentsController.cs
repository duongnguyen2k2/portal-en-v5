using System;
using System.Collections.Generic;
using System.Linq;
using API.Areas.Admin.Models.Documents;
using API.Areas.Admin.Models.DocumentsCategories;
using API.Areas.Admin.Models.DocumentsType;
using API.Areas.Admin.Models.DocumentsField;
using API.Areas.Admin.Models.DocumentsLevel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using API.Areas.Image.Models;
using API.Areas.Admin.Models.Liferay;
using System.IO;

namespace API.Controllers
{
    public class DocumentsController : Controller
    {

        public IActionResult Index([FromQuery] SearchDocuments dto)
        {
            int IdCoQuan = 1;
            if (HttpContext.Session.GetString("IdCoQuan") != null && HttpContext.Session.GetString("IdCoQuan") != "")
            {
                IdCoQuan = int.Parse(HttpContext.Session.GetString("IdCoQuan"));
            }

            int TotalItems = 0;
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            dto.IdCoQuan = IdCoQuan;
            dto.ShowStartDate = "01/01/1990";
            dto.Status = 1;
            DocumentsModel data = new DocumentsModel() { SearchData = dto };
            data.ListItems = DocumentsService.GetListPagination(data.SearchData, API.Models.Settings.SecretId + ControllerName);
            if (data.ListItems != null && data.ListItems.Count() > 0)
            {
                TotalItems = data.ListItems[0].TotalRows;
            }
            data.Pagination = new Areas.Admin.Models.Partial.PartialPagination() { CurrentPage = data.SearchData.CurrentPage, ItemsPerPage = data.SearchData.ItemsPerPage, TotalItems = TotalItems, QueryString = Request.QueryString.ToString() };
            data.ListDocumentsCategories = DocumentsCategoriesService.GetListSelectItems();
            data.ListDocumentsType = DocumentsTypeService.GetListSelectItems();
            data.ListDocumentsField = DocumentsFieldService.GetListSelectItems();
            data.ListDocumentsLevel = DocumentsLevelService.GetListSelectItems();
            return View(data);
        }

        public IActionResult Notification([FromQuery] SearchDocuments dto)
        {
            int IdCoQuan = 1;
            if (HttpContext.Session.GetString("IdCoQuan") != null && HttpContext.Session.GetString("IdCoQuan") != "")
            {
                IdCoQuan = int.Parse(HttpContext.Session.GetString("IdCoQuan"));
            }

            int TotalItems = 0;
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            dto.IdCoQuan = IdCoQuan;
            dto.ShowStartDate = "01/01/1990";
            dto.Status = 1;
            DocumentsModel data = new DocumentsModel() { SearchData = dto };
            data.ListItems = DocumentsService.GetListNotification(data.SearchData);
            if (data.ListItems != null && data.ListItems.Count() > 0)
            {
                TotalItems = data.ListItems[0].TotalRows;
            }
            data.Pagination = new Areas.Admin.Models.Partial.PartialPagination() { CurrentPage = data.SearchData.CurrentPage, ItemsPerPage = data.SearchData.ItemsPerPage, TotalItems = TotalItems, QueryString = Request.QueryString.ToString() };
            data.ListDocumentsCategories = DocumentsCategoriesService.GetListSelectItems();
            data.ListDocumentsType = DocumentsTypeService.GetListSelectItems();
            data.ListDocumentsField = DocumentsFieldService.GetListSelectItems();
            data.ListDocumentsLevel = DocumentsLevelService.GetListSelectItems();
            return View(data);
        }

        public IActionResult Detail(string alias, int id)
        {
            DocumentsModel data = new DocumentsModel();
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            data.SearchData = new SearchDocuments() { CurrentPage = 0, ItemsPerPage = 10, Keyword = "" };
            data.Item = DocumentsService.GetItem(id, API.Models.Settings.SecretId + ControllerName);
            return View(data);
        }


        public IActionResult DongBoCode(int folderid, int filenameid, string filetitle, string uid)
        {
            string filename = filetitle.Replace("+", " ");

            Z_Dlfileentry item = LiferayService.GetItemDlfileentry(filename);

            if (item != null)
            {
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "document_library") + "/"+item.companyid.ToString()+"/"+ item.folderid.ToString()+ "/" + item.name + "/1.0";
                if (!System.IO.File.Exists(filePath))
                {
                    filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot") + "/images/no-image.png";
                }
                Byte[] b = System.IO.File.ReadAllBytes(filePath);   // You can use your own method over here.         
                return File(b, item.mimetype);
            }
            else {

                return Json(new API.Models.MsgSuccess() { Data = item, Msg = filename });

            }

            
        }

    }
}