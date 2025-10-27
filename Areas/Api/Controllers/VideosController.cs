using API.Areas.Admin.Models.CategoriesVideos;
using API.Areas.Admin.Models.Products;
using API.Areas.Admin.Models.Videos;
using API.Models;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Collections.Generic;

namespace API.Areas.Api.Controllers
{
	[Area("Api")]
	public class VideosController : Controller
	{
		// Lấy danh sách Loại videos
		public IActionResult GetListCat()
		{			
			var ListItem = CategoriesVideosService.GetList();
			return Json(new MsgSuccess() { Data = ListItem });
		}
		// Lấy danh sách Videos theo Loại Video
		public IActionResult GetListByCat(string alias, int Id, [FromQuery] SearchVideos dto)
        {

			CategoriesVideos categories = CategoriesVideosService.GetItem(Id, "");
			VideosModel data = new VideosModel()
			{
				SearchData = dto
			};
            if (categories!=null && categories.Id > 0)
            {
                int IdCoQuan = 1;
                
                if (HttpContext.Session.GetString("IdCoQuan") != null && HttpContext.Session.GetString("IdCoQuan") != "")
                {
                    IdCoQuan = int.Parse(HttpContext.Session.GetString("IdCoQuan"));
                }
                dto.Status = 1;
				dto.IdCoQuan = IdCoQuan;
				dto.CatId = categories.Id;
				data = new VideosModel() { SearchData = dto, Categories = categories };
				data.ListItems = VideosService.GetListPagination(data.SearchData, API.Models.Settings.SecretId);				
			}
			
			return Json(new MsgSuccess(){ Data = data.ListItems});
        }
		// Lấy chi tiết Video
		public IActionResult Detail([FromQuery] int Id)
		{
			Videos Item = VideosService.GetItem(Id);
			return Json(new MsgSuccess() { Data = Item });
		}
	}
}
