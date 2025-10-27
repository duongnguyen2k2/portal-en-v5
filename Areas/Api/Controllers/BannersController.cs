using API.Areas.Admin.Models.Ablums;
using API.Areas.Admin.Models.Banners;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace API.Areas.Api.Controllers
{
    [Area("Api")]
    public class BannersController : Controller
    {
        public IActionResult GetListBannerByCat([FromQuery] SearchBanners dto)
        {
            BannersModel data = new BannersModel() { SearchData = dto };
            data.ListItems = BannersService.GetListByCat(dto.CatId, dto.IdCoQuan);
            return Json(new MsgSuccess() { Data = data.ListItems });
        }
    }
}
