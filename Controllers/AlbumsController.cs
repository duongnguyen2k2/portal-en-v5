using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Areas.Admin.Models.Ablums;
using API.Areas.Admin.Models.CategoriesAblums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class AlbumsController : Controller
    {
        public IActionResult Index([FromQuery] SearchCategoriesAblums dto)
        {
            int TotalItems = 0;
            string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

            dto.IdCoQuan = 1;
            dto.Status = 1;
            if (HttpContext.Session.GetString("IdCoQuan") != null && HttpContext.Session.GetString("IdCoQuan") != "")
            {
                dto.IdCoQuan = int.Parse(HttpContext.Session.GetString("IdCoQuan"));
            }

            CategoriesAblumsModel data = new CategoriesAblumsModel() { SearchData = dto };
            data.ListItemsAlbums = AblumsService.GetListPagination(new SearchAblums() { IdCoQuan = dto.IdCoQuan, Status=1});

            List<CategoriesAblums> ListCatAblum = new List<CategoriesAblums>();
            ListCatAblum = CategoriesAblumsService.GetList(true, dto.IdCoQuan);

            for (int i = 0; i < ListCatAblum.Count(); i++) {
                List<Ablums> tmp = new List<Ablums>();
                for (int j = 0; j < data.ListItemsAlbums.Count(); j++) {
                    if (ListCatAblum[i].Id == data.ListItemsAlbums[j].CatId) {
                        tmp.Add(data.ListItemsAlbums[j]);
                    }
                }
                ListCatAblum[i].ListItemsAlbums = tmp;
            }

            data.ListItems = ListCatAblum;
            if (data.ListItems != null && data.ListItems.Count() > 0)
            {
                TotalItems = data.ListItems[0].TotalRows;
            }
            data.Pagination = new Areas.Admin.Models.Partial.PartialPagination() { CurrentPage = data.SearchData.CurrentPage, ItemsPerPage = data.SearchData.ItemsPerPage, TotalItems = TotalItems, QueryString = Request.QueryString.ToString() };

            return View(data);
        }
    }
}