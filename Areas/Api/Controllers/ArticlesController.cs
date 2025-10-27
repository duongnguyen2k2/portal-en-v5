using API.Areas.Admin.Models.Articles;
using API.Areas.Admin.Models.CategoriesArticles;
using API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;

namespace API.Areas.Api.Controllers
{
	[Area("Api")]
	public class ArticlesController : Controller
	{
		private IConfiguration Configuration;
		public ArticlesController(IConfiguration config)
		{
			Configuration = config;
		}

		// Lấy danh sách Loại Bài Viết
		public IActionResult GetListCat([FromQuery] string Lang = "vi")
		{
			var ListItem = CategoriesArticlesService.GetList(true, 1, Lang);
			return Json(new MsgSuccess() { Data = ListItem });
		}

		// Lấy danh sách Bài viết theo loại
		public IActionResult GetListByCat(string alias, int Id, [FromQuery] SearchArticles dto, string Lang = "vi")
		{			
			CategoriesArticles categories = CategoriesArticlesService.GetItem(Id, "ds", dto.IdCoQuan, Lang);

			dto.CatId = Id;
			if (dto.IdCoQuan == 0)
			{
				dto.IdCoQuan = Int32.Parse(Configuration["IdCoQuanParent"].ToString());
			}
			
			dto.ShowStartDate = "01/01/2010";
			dto.Status = 1;
			ArticlesModel data = new ArticlesModel() { SearchData = dto, Categories = categories };
			data.ListItems = ArticlesService.GetListPagination(data.SearchData,"ds", Lang);
			
			return Json(new MsgSuccess() { Data = data.ListItems });
		}

		public IActionResult Detail([FromQuery] int Id = 0, int IdCoQuan=0, string Lang = "vi" )
		{
			if (IdCoQuan == 0)
			{
				IdCoQuan = Int32.Parse(Configuration["IdCoQuanParent"].ToString());
			}

			string domain = HttpContext.Request.Host.Value.ToLower();

			Articles Item = ArticlesService.GetItem(Id, "ds", Lang);
			if (Item != null)
			{
				if (Item.Status == true)
				{
					Item.FullText = Item.FullText.Replace("src='/'", "src='http://" + domain + "/");
					Item.FullText = Item.FullText.Replace("src=\"/", "src=\"http://" + domain + "/");
					return Json(new MsgSuccess() { Data = Item });
				}
				else
				{
					return Json(new MsgSuccess() { Data = null });
				}
			}

			return Json(new MsgError() { Data = null, Msg = "Bài viết không tồn tại" });
		}

		public IActionResult DetailView([FromQuery] int Id = 0, int IdCoQuan = 0, string Lang = "vi")
		{
			if (IdCoQuan == 0)
			{
				IdCoQuan = Int32.Parse(Configuration["IdCoQuanParent"].ToString());
			}

			string domain = HttpContext.Request.Host.Value.ToLower();

			Articles Item = ArticlesService.GetItem(Id, "ds", Lang);
			if (Item != null)
			{
				if (Item.Status == true)
				{
					Item.FullText = Item.FullText.Replace("src='/'", "src='http://" + domain+"/");
					Item.FullText = Item.FullText.Replace("src=\"/", "src=\"http://" + domain+"/");
					return View(Item);
				}
				else
				{
					return View(Item);
				}
			}

			return View(Item);
		}
	}
}
