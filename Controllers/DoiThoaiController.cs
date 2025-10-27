using API.Areas.Admin.Models.DoiThoai;
using API.Areas.Admin.Models.CategoriesDoiThoai;
using API.Areas.Admin.Models.DMCoQuan;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Areas.Admin.Models.DoiThoai;
using API.Areas.Admin.Models.CategoriesDoiThoai;

namespace API.Controllers
{
    public class DoiThoaiController : Controller
    {
        private string controllerName = "DoiThoaiController";
        private string controllerSecret;
        public DoiThoaiController(IConfiguration config)
        {

            controllerSecret = config["Security:SecretId"] + controllerName;
        }

        public IActionResult GetListCat(string alias, int id, [FromQuery] SearchCategoriesDoiThoai dto)
        {
            int TotalItems = 0;

           

            int IdCoQuan = 0;
            if (HttpContext.Session.GetString("IdCoQuan") != null && HttpContext.Session.GetString("IdCoQuan") != "")
            {
                IdCoQuan = int.Parse(HttpContext.Session.GetString("IdCoQuan"));
            }
            dto.IdCoQuan = IdCoQuan;
            dto.Status = 1;
            
            CategoriesDoiThoaiModel data = new CategoriesDoiThoaiModel() { SearchData = dto };
            data.ListItems = CategoriesDoiThoaiService.GetListPagination(data.SearchData, controllerSecret);
            if (data.ListItems != null && data.ListItems.Count() > 0)
            {
                TotalItems = data.ListItems[0].TotalRows;
            }

            data.Pagination = new Areas.Admin.Models.Partial.PartialPagination() { CurrentPage = data.SearchData.CurrentPage, ItemsPerPage = data.SearchData.ItemsPerPage, TotalItems = TotalItems, QueryString = Request.QueryString.ToString() };

            return View(data);
        }

        public IActionResult Index(string alias, int id, [FromQuery] SearchDoiThoai dto)
        {
            int TotalItems = 0;
                       
            CategoriesDoiThoai categories = CategoriesDoiThoaiService.GetItem(id, controllerSecret);
            if (categories == null)
            {
				categories = new CategoriesDoiThoai() { Title="Chủ đề đối thoại",Id=0,Alias="chu-de-doi-thoai"};

			}

            int IdCoQuan = 0;
			if (HttpContext.Session.GetString("IdCoQuan") != null && HttpContext.Session.GetString("IdCoQuan") != "")
			{
				IdCoQuan = int.Parse(HttpContext.Session.GetString("IdCoQuan"));
			}
			dto.IdCoQuan = IdCoQuan;
			dto.Status = 1;
			dto.CatId = id;
			
			DoiThoaiModel data = new DoiThoaiModel() { SearchData = dto, Categories = categories };
            data.ListItems = DoiThoaiService.GetListPagination(data.SearchData, controllerSecret);
            if (data.ListItems != null && data.ListItems.Count() > 0)
            {
                TotalItems = data.ListItems[0].TotalRows;
            }

            data.Pagination = new Areas.Admin.Models.Partial.PartialPagination() { CurrentPage = data.SearchData.CurrentPage, ItemsPerPage = data.SearchData.ItemsPerPage, TotalItems = TotalItems, QueryString = Request.QueryString.ToString() };

            return View(data);
        }

        public IActionResult Detail(string alias, int id)
        {
            DoiThoaiModel data = new DoiThoaiModel();
            data.SearchData = new SearchDoiThoai() { CurrentPage = 0, ItemsPerPage = 10, Keyword = "" };
            
            data.Item = DoiThoaiService.GetItem(id, controllerSecret);
            CategoriesDoiThoai categories = CategoriesDoiThoaiService.GetItem(data.Item.CatId, controllerSecret);
            data.Categories = categories;
            
            return View(data);

        }
    }

   
}
