using API.Areas.Admin.Models.ArticlesFestival;
using API.Areas.Admin.Models.SYSParams;
using API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;

namespace API.Areas.Api.Controllers
{
	[Area("Api")]
	public class ArticlesFestivalController : Controller
	{
		private string controllerName = "ArticlesFestivalController";
		private string controllerSecret;
		private IConfiguration Configuration;

		public ArticlesFestivalController(IConfiguration config)
		{
			Configuration = config;
		}

		// Lấy danh sách Bài viết theo loại 
		public IActionResult GetListAll([FromQuery] SearchArticlesFestival dto)
		{
			Boolean MobileCheck = true;
			string domain = HttpContext.Request.Host.Value.ToLower();
			Boolean isHttps = HttpContext.Request.IsHttps;
			ArticlesFestivalModel data = new ArticlesFestivalModel() { SearchData = dto };
			data.ListItems = ArticlesFestivalService.GetListPagination(dto, controllerSecret + HttpContext.Request.Headers["UserName"], true);

			foreach (ArticlesFestival article in data.ListItems)
			{
				if (article != null)
				{
					if (article.Status == true)
					{
						if (isHttps == true)
						{
							article.Description = article.Description.Replace("src='/'", "src='https://" + domain + "/");
							article.Description = article.Description.Replace("src=\"/", "src=\"https://" + domain + "/");
						}
						else
						{
							article.Description = article.Description.Replace("src='/'", "src='http://" + domain + "/");
							article.Description = article.Description.Replace("src=\"/", "src=\"http://" + domain + "/");
						}
					}
				}
			}

			return Json(new MsgSuccess() { Data = data.ListItems });
		}

		// Get chi tiết bài viết sản phẩm bởi ID
		public IActionResult Detail([FromQuery] int Id = 0)
		{
			SYSConfig sysConfig = SYSParamsService.GetItemConfig();
			string domain = HttpContext.Request.Host.Value.ToLower();
			Boolean isHttps = HttpContext.Request.IsHttps;
			string imgDomain = sysConfig.ImgDomain;

			ArticlesFestival Item = ArticlesFestivalService.GetItem(Id, "ds");
			if (Item != null)
			{
				if (Item.Status == true)
				{
					if (!string.IsNullOrEmpty(imgDomain))
					{
						Item.Description = Item.Description.Replace("src='/'", "src='" + imgDomain + "/");
						Item.Description = Item.Description.Replace("src=\"/", "src=\"" + imgDomain + "/");
					}
					else if (isHttps == true)
					{
						Item.Description = Item.Description.Replace("src='/'", "src='https://" + domain + "/");
						Item.Description = Item.Description.Replace("src=\"/", "src=\"https://" + domain + "/");
					}
					else
					{
						Item.Description = Item.Description.Replace("src='/'", "src='http://" + domain + "/");
						Item.Description = Item.Description.Replace("src=\"/", "src=\"http://" + domain + "/");
					}
					return Json(new MsgSuccess() { Data = Item });
				}
				else
				{
					return Json(new MsgSuccess() { Data = null });
				}
			}

			return Json(new MsgError() { Data = null, Msg = "Bài viết không tồn tại" });
		}
	}
}
