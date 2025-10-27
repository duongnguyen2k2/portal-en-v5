using API.Areas.Admin.Models.ArticlesProduct;
using API.Areas.Admin.Models.CategoriesProducts;
using API.Areas.Admin.Models.Manufacturer;
using API.Areas.Admin.Models.Products;
using API.Areas.Admin.Models.SYSParams;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace API.Areas.Api.Controllers
{
	[Area("Api")]
	public class ProductsController : Controller
	{
		// Lấy toàn bộ Danh sách sản phẩm
		public IActionResult GetList([FromQuery] SearchProducts dto)
		{
			dto.Status = 1;
			if (dto.CatId == 0)
			{
				dto.CatId = -1;
			}

			List<Products> ListItems = ProductsService.GetListPagination(dto, API.Models.Settings.SecretId, 0);

			return Json(new MsgSuccess() { Data = ListItems, Msg = "Lấy sản phẩm người dân đăng thành công" });
		}

		public IActionResult GetListCat([FromQuery] SearchCategoriesProducts dto)
		{

			var ListItems = CategoriesProductsService.GetList();

			return Json(new MsgSuccess() { Data = ListItems, Msg = "Lấy sản phẩm người dân đăng thành công" });
		}

		// GET LIST_PRODUCTS bởi Mã cở sở sản xuất
		public IActionResult GetListByManufacturer([FromQuery] int IdManufacturer)
		{
			int OCOP_Product = 0;
			Boolean MobileCheck = true;
			ProductsModel model = new ProductsModel();
			SearchProducts searchP = new SearchProducts() { Status = 1, CatId = -1, IdCoQuan = 2, IdManufacturer = IdManufacturer };
			model.ListItems = ProductsService.GetListByManufacturer(searchP, API.Models.Settings.SecretId, OCOP_Product, MobileCheck);

			return Json(new MsgSuccess() { Data = model.ListItems, Msg = "Lấy sản phẩm thành công" });
		}

		// Lấy toàn bộ Danh sách sản phẩm OCOP
		public IActionResult GetListOCOP()
		{
			List<Products> ListItems = ProductsService.GetListOCOP();
			return Json(new MsgSuccess() { Data = ListItems, Msg = "Lấy sản phẩm OCOP thành công" });
		}

		public IActionResult DetailOCOP([FromQuery] int Id)
		{
			string domain = HttpContext.Request.Host.Value.ToLower();
			SYSConfig sysConfig = SYSParamsService.GetItemConfig();
			string imgDomain = sysConfig.ImgDomain;

			Products Item = ProductsService.GetItem(Id, "", 1);

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

			return Json(new MsgSuccess() { Data = Item });
		}

		public IActionResult Detail([FromQuery] int Id)
		{
			Products Item = ProductsService.GetItem(Id);
			// Get List IdManufacturer by Product
			Item.List_IdManufacturer = ManufacturerService.GetListIdByProduct(Id);
			return Json(new MsgSuccess() { Data = Item });
		}
	}
}
