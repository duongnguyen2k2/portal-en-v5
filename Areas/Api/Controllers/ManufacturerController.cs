using API.Areas.Admin.Models.Manufacturer;
using API.Areas.Admin.Models.Products;
using API.Areas.Admin.Models.SYSParams;
using API.Models;
using DocumentFormat.OpenXml.EMMA;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace API.Areas.Api.Controllers
{
	[Area("Api")]
	public class ManufacturerController : Controller
	{
		// Lấy danh sách Nhà Vườn
		public IActionResult GetListByCatProduct([FromQuery] SearchManufacturer dto)
		{
			dto.Status = 1;
			dto.IdCoQuan = 2;
			var ListItem = ManufacturerService.GetListByCatProduct(dto, "ManufacturerController");

			return Json(new MsgSuccess() { Data = ListItem });
		}

		// Lấy chi tiết Nhà vườn
		public IActionResult Detail([FromQuery] int Id, string Key = "APP_CM", int IdCoQuan = 0)
		{
			string domain = HttpContext.Request.Host.Value.ToLower();
			SYSConfig sysConfig = SYSParamsService.GetItemConfig();
			string imgDomain = sysConfig.ImgDomain;
			Manufacturer Item = ManufacturerService.GetItem(Id);
			// Get List IdProduct by IdManufacturer
			Item.List_IdProduct = ProductsService.GetListIdByManufacturer(Id);
			// Convert IdProduct in List_Product into List_IdProduct
			SearchProducts searchP = new SearchProducts() { Status = 1, CatId = -1, IdCoQuan = 2, IdManufacturer = Id };
			Item.ListProduct = ProductsService.GetListByManufacturer(searchP, API.Models.Settings.SecretId, 0, true);

			if (Item != null)
			{
				if (!string.IsNullOrEmpty(imgDomain))
				{
					Item.Description = Item.Description.Replace("src='/'", "src='" + imgDomain + "/");
					Item.Description = Item.Description.Replace("src=\"/", "src=\"" + imgDomain + "/");
					return Json(new MsgSuccess() { Data = Item });
				}
				else if (Item.Status == true)
				{
					Item.Description = Item.Description.Replace("src='/'", "src='http://" + domain + "/");
					Item.Description = Item.Description.Replace("src=\"/", "src=\"http://" + domain + "/");
					return Json(new MsgSuccess() { Data = Item });
				}
				else
				{
					return Json(new MsgSuccess() { Data = null });
				}
			}

			return Json(new MsgError() { Data = null });
		}
	}
}
