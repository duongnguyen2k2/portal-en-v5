using API.Areas.Admin.Models.ArticlesProduct;
using API.Areas.Admin.Models.CategoriesArticlesProduct;
using API.Areas.Admin.Models.SYSParams;
using API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;

namespace API.Areas.Api.Controllers
{
	[Area("Api")]
	public class ArticlesProductController : Controller
	{
		private string controllerName = "ArticlesProductController";
		private string controllerSecret;
		private IConfiguration Configuration;

		public ArticlesProductController(IConfiguration config)
		{
			Configuration = config;
		}

		//Lấy danh sách Loại Bài Viết Sản phẩm
		public IActionResult GetListCatAP()
		{
			var ListItem = CategoriesArticlesProductService.GetListMobile();
			return Json(new MsgSuccess() { Data = ListItem });
		}

		// Lấy danh sách Bài viết theo loại 
		public IActionResult GetListByCat([FromQuery] SearchArticlesProduct dto)
		{
			Boolean MobileCheck = true;
			string domain = HttpContext.Request.Host.Value.ToLower();
			Boolean isHttps = HttpContext.Request.IsHttps;
			ArticlesProductModel data = new ArticlesProductModel() { SearchData = dto };
			data.ListItems = ArticlesProductService.GetListPaginationMobile(dto, MobileCheck, controllerSecret + HttpContext.Request.Headers["UserName"]);

			foreach (ArticlesProduct article in data.ListItems)
			{
				if (article != null)
				{
					if (article.Status == true)
					{
						if (isHttps == true)
						{
							article.FullText = article.FullText.Replace("src='/'", "src='https://" + domain + "/");
							article.FullText = article.FullText.Replace("src=\"/", "src=\"https://" + domain + "/");
						}
						else
						{
							article.FullText = article.FullText.Replace("src='/'", "src='http://" + domain + "/");
							article.FullText = article.FullText.Replace("src=\"/", "src=\"http://" + domain + "/");
						}
					}
				}
			}

			return Json(new MsgSuccess() { Data = data.ListItems });
		}

		// GET Articles_Product bởi Mã CSSX + Loại CatAP = "Bài Đăng"
		public IActionResult GetListByManufacturer([FromQuery] int IdManufacturer, int id)
		{
			Boolean MobileCheck = true;
			int IdCatAP = 1; // Set Loại bài viết = Bài đăng
			ArticlesProductModel data = new ArticlesProductModel();
			SearchArticlesProduct searchAP = new SearchArticlesProduct() { Status = 1, IdCatAP = IdCatAP, IdManufacturer = IdManufacturer };
			data.ListItems = ArticlesProductService.GetListPaginationMobile(searchAP, MobileCheck, controllerSecret + HttpContext.Request.Headers["UserName"]);

			return Json(new MsgSuccess() { Data = data.ListItems, Msg = "Lấy sản phẩm thành công" });
		}

		// Get chi tiết bài viết sản phẩm bởi ID
		public IActionResult Detail([FromQuery] string Ids = null)
		{
			int Id = Int32.Parse(MyModels.Decode(Ids, controllerSecret + HttpContext.Request.Headers["UserName"]).ToString());
			SYSConfig sysConfig = SYSParamsService.GetItemConfig();
			string domain = HttpContext.Request.Host.Value.ToLower();
			Boolean isHttps = HttpContext.Request.IsHttps;
			string imgDomain = sysConfig.ImgDomain;

			ArticlesProduct Item = ArticlesProductService.GetItem(Id, "ds");
			if (Item != null)
			{
				if (Item.Status == true)
				{
					if (!string.IsNullOrEmpty(imgDomain))
					{
						Item.FullText = Item.FullText.Replace("src='/'", "src='" + imgDomain + "/");
						Item.FullText = Item.FullText.Replace("src=\"/", "src=\"" + imgDomain + "/");
					}
					else if (isHttps == true)
					{
						Item.FullText = Item.FullText.Replace("src='/'", "src='https://" + domain + "/");
						Item.FullText = Item.FullText.Replace("src=\"/", "src=\"https://" + domain + "/");
					}
					else
					{
						Item.FullText = Item.FullText.Replace("src='/'", "src='http://" + domain + "/");
						Item.FullText = Item.FullText.Replace("src=\"/", "src=\"http://" + domain + "/");
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
