using API.Areas.Admin.Models.Audios;
using API.Areas.Admin.Models.CategoriesAudios;
using API.Areas.Admin.Models.DMCoQuan;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class AudiosController : Controller
    {
        private string controllerName = "AudiosController";
        private string controllerSecret;
        public AudiosController(IConfiguration config)
        {

            controllerSecret = config["Security:SecretId"] + controllerName;
        }
        public IActionResult Index(string alias, int id, [FromQuery] SearchAudios dto)
        {
            int TotalItems = 0;
                       
            CategoriesAudios categories = CategoriesAudiosService.GetItem(id, controllerSecret);
            if (categories == null)
            {
				categories = new CategoriesAudios() { Title="Truyền Thanh",Id=0,Alias="truyen-thanh"};

			}

            int IdCoQuan = 0;
			if (HttpContext.Session.GetString("IdCoQuan") != null && HttpContext.Session.GetString("IdCoQuan") != "")
			{
				IdCoQuan = int.Parse(HttpContext.Session.GetString("IdCoQuan"));
			}
			dto.IdCoQuan = IdCoQuan;
			dto.Status = 1;
			dto.CatId = id;
			
			AudiosModel data = new AudiosModel() { SearchData = dto, Categories = categories };
            data.ListItems = AudiosService.GetListPagination(data.SearchData, controllerSecret);
            if (data.ListItems != null && data.ListItems.Count() > 0)
            {
                TotalItems = data.ListItems[0].TotalRows;
            }

            data.Pagination = new Areas.Admin.Models.Partial.PartialPagination() { CurrentPage = data.SearchData.CurrentPage, ItemsPerPage = data.SearchData.ItemsPerPage, TotalItems = TotalItems, QueryString = Request.QueryString.ToString() };

            return View(data);
        }

        public IActionResult Detail(string alias, int id)
        {
            AudiosModel data = new AudiosModel();
            data.SearchData = new SearchAudios() { CurrentPage = 0, ItemsPerPage = 10, Keyword = "" };
            data.ListItemsCat = CategoriesAudiosService.GetListItems();
            data.Item = AudiosService.GetItem(id, controllerSecret);
            CategoriesAudios categories = CategoriesAudiosService.GetItem(data.Item.CatId, controllerSecret);
            data.Categories = categories;
            if (categories.Id != 0)
            {
                data.ListItems = AudiosService.GetListRelativeNews(alias, categories.Id);
            }
            return View(data);

        }
    }

   
}
