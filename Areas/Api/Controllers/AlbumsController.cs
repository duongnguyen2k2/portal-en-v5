
using API.Areas.Admin.Models.Ablums;
using API.Areas.Admin.Models.CategoriesAblums;
using API.Areas.Admin.Models.Ablums;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace API.Areas.Api.Controllers
{
    [Area("Api")]
    public class AlbumsController : Controller
    {
        public IActionResult GetAlbum([FromQuery] int Id)
        {                        
            CategoriesAblums  Item = CategoriesAblumsService.GetItem(Id,"12");
            List<Ablums> ListItemsAlbums = AblumsService.GetList(Id, "12");
            return Json(new MsgSuccess() { Data = new { Cat = Item, ListItemsAlbums = ListItemsAlbums } });
        }
        // Lấy danh sách Albums 
        public IActionResult GetListAll([FromQuery] SearchCategoriesAblums dto)
        {
            CategoriesAblumsModel data = new CategoriesAblumsModel() { SearchData = dto };
            data.ListItemsAlbums = AblumsService.GetListPagination(new SearchAblums() { IdCoQuan = dto.IdCoQuan, Status = 1 });

            List<CategoriesAblums> ListCatAblum = new List<CategoriesAblums>();
            ListCatAblum = CategoriesAblumsService.GetList(true, dto.IdCoQuan);

            for (int i = 0; i < ListCatAblum.Count(); i++)
            {
                List<Ablums> tmp = new List<Ablums>();
                for (int j = 0; j < data.ListItemsAlbums.Count(); j++)
                {
                    if (ListCatAblum[i].Id == data.ListItemsAlbums[j].CatId)
                    {
                        tmp.Add(data.ListItemsAlbums[j]);
                    }
                }
                ListCatAblum[i].ListItemsAlbums = tmp;
            }
            data.ListItems = ListCatAblum;
            return Json(new MsgSuccess() { Data = data.ListItems });
        }
        // Lấy chi tiết Albums

        public IActionResult Detail([FromQuery] int Id)
        {
            Ablums Item = AblumsService.GetItem(Id);
            return Json(new MsgSuccess() { Data = Item });
        }

        // Lấy danh sách Albums  là banner
        public IActionResult GetListAlbumBanner([FromQuery] SearchCategoriesAblums dto)
        {
            CategoriesAblumsModel data = new CategoriesAblumsModel() { SearchData = dto };
            data.ListItemsAlbums = AblumsService.GetListPagination(new SearchAblums() { IdCoQuan = dto.IdCoQuan, Status = 1 });

            List<CategoriesAblums> ListCatAblum = new List<CategoriesAblums>();
            List<CategoriesAblums> ListCatAblumterm = CategoriesAblumsService.GetList(true, dto.IdCoQuan);

            foreach( var cat in ListCatAblumterm )
            {
                if(cat.Title.ToLower() == "banners" && cat.Alias.ToLower() == "banners")
                {
                    ListCatAblum.Add(cat);
                }
            }

            for (int i = 0; i < ListCatAblum.Count(); i++)
            {
                List<Ablums> tmp = new List<Ablums>();
                for (int j = 0; j < data.ListItemsAlbums.Count(); j++)
                {
                    if (ListCatAblum[i].Id == data.ListItemsAlbums[j].CatId)
                    {
                        tmp.Add(data.ListItemsAlbums[j]);
                    }
                }
                ListCatAblum[i].ListItemsAlbums = tmp;
            }

            data.ListItems = ListCatAblum;
            return Json(new MsgSuccess() { Data = data.ListItems });
        }
    }
}
