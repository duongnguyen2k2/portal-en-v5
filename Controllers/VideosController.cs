using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using API.Areas.Admin.Models.Videos;
using API.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using Microsoft.Extensions.Configuration;
using API.Areas.Admin.Models.CategoriesVideos;
using Microsoft.AspNetCore.Http;
using API.Areas.Admin.Models.CategoriesAudios;
using Microsoft.Extensions.Hosting;
using System.IO;
using DocumentFormat.OpenXml.Drawing.Charts;
using API.Areas.Admin.Models.DMCoQuan;
namespace API.Controllers
{
    public class VideosController : Controller
    {
        private string controllerName = "AudiosController";
        private string controllerSecret;
        public VideosController(IConfiguration config)
        {

            controllerSecret = config["Security:SecretId"] + controllerName;
        }
        public IActionResult Index(string alias, int id, [FromQuery] SearchVideos dto)
        {
            CategoriesVideos categories = CategoriesVideosService.GetItem(id, "", CultureInfo.CurrentCulture.Name.ToString());
			if (categories == null)
			{
				categories = new CategoriesVideos() { Title = "Truyền Thanh", Id = 0, Alias = "truyen-thanh" };

			}
            VideosModel data = new VideosModel()
            {
                SearchData = dto
            };
            int IdCoQuan = 1;
            if (HttpContext.Session.GetString("IdCoQuan") != null && HttpContext.Session.GetString("IdCoQuan") != "")
            {
                IdCoQuan = int.Parse(HttpContext.Session.GetString("IdCoQuan"));
            }

            int TotalItems = 0;
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            dto.Status = 1;
            dto.IdCoQuan = IdCoQuan;

            if (categories!=null && categories.Id > 0)
            {

                
                dto.CatId = categories.Id;

                data = new VideosModel() { SearchData = dto, Categories = categories };

                data.ListItems = VideosService.GetListPagination(data.SearchData, API.Models.Settings.SecretId + ControllerName,0, CultureInfo.CurrentCulture.Name.ToString());
                if (data.ListItems != null && data.ListItems.Count() > 0)
                {
                    TotalItems = data.ListItems[0].TotalRows;
                }
                data.Pagination = new API.Areas.Admin.Models.Partial.PartialPagination() { CurrentPage = data.SearchData.CurrentPage, ItemsPerPage = data.SearchData.ItemsPerPage, TotalItems = TotalItems, QueryString = Request.QueryString.ToString() };
            }
            else
            {
                categories = new CategoriesVideos()
                {
                    Title = "Video",
                    Id=0
                };
                dto.CatId= 0;
                data = new VideosModel() { SearchData = dto, Categories = categories };

                data.ListItems = VideosService.GetListPagination(data.SearchData, API.Models.Settings.SecretId + ControllerName, 0, CultureInfo.CurrentCulture.Name.ToString());
                if (data.ListItems != null && data.ListItems.Count() > 0)
                {
                    TotalItems = data.ListItems[0].TotalRows;
                }
                data.Pagination = new API.Areas.Admin.Models.Partial.PartialPagination() { CurrentPage = data.SearchData.CurrentPage, ItemsPerPage = data.SearchData.ItemsPerPage, TotalItems = TotalItems, QueryString = Request.QueryString.ToString() };

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
            VideosModel data = new VideosModel();
            data.SearchData = new SearchVideos() { CurrentPage = 0, ItemsPerPage = 10, Keyword = "" };
            data.ListItemsCat = CategoriesVideosService.GetListItems();
            data.Item = VideosService.GetItem(id, controllerSecret, CultureInfo.CurrentCulture.Name.ToString());
            if (data.Item.CatId == 0)
            {
                data.Item.CatId = 1;
            }
            CategoriesVideos categories = CategoriesVideosService.GetItem(data.Item.CatId, controllerSecret, CultureInfo.CurrentCulture.Name.ToString());
            data.Categories = categories;
            if (categories.Id != 0)
            {
                data.ListItems = VideosService.GetListRelative(IdCoQuan, categories.Id, CultureInfo.CurrentCulture.Name.ToString());
            }
            return View(data);

        }

        public IActionResult DetailStream(string alias, int id)
        {
            string filename = "nhac.mp4";
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "/uploads/" + filename);
            
            //Build the File Path.
            if (System.IO.File.Exists(path))
            {
                var filestream = System.IO.File.OpenRead(path);
                return File(filestream, contentType: "video/mp4", fileDownloadName: filename, enableRangeProcessing: true);
            }
            else
            {
                return Json(new MsgError() { Msg = "File Khong ton tai" });
            }
                

        }

        public IActionResult Test()
        {
            return View();
        }
    }
}