using API.Areas.Admin.Models.CategoriesProducts;
using API.Areas.Admin.Models.Manufacturer;
using API.Areas.Admin.Models.Products;
using API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace API.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class ManufacturerController : Controller
	{
		private string controllerName = "ManufacturerController";
		private string controllerSecret;

		public ManufacturerController(IConfiguration config)
		{
			controllerSecret = config["Security:SecretId"] + controllerName;
		}

		public IActionResult Index([FromQuery] SearchManufacturer dto)
		{
			int TotalItems = 0;
			ManufacturerModel data = new ManufacturerModel() { SearchData = dto };

			data.ListItems = ManufacturerService.GetListPagination(data.SearchData, controllerSecret + HttpContext.Request.Headers["UserName"]);
			if (data.ListItems != null && data.ListItems.Count() > 0)
			{
				TotalItems = data.ListItems[0].TotalRows;
			}
			data.Pagination = new Models.Partial.PartialPagination() { CurrentPage = data.SearchData.CurrentPage, ItemsPerPage = data.SearchData.ItemsPerPage, TotalItems = TotalItems, QueryString = Request.QueryString.ToString() };

			return View(data);
		}

		public IActionResult SaveItem(string Id = null)
		{
			ManufacturerModel data = new ManufacturerModel();

			int IdDC = Int32.Parse(MyModels.Decode(Id, controllerSecret + HttpContext.Request.Headers["UserName"]).ToString());
			data.SearchData = new SearchManufacturer() { CurrentPage = 0, ItemsPerPage = 10, Keyword = "" };
			//data.ListCatProduct = CategoriesProductsService.GetSelectListItems();
			data.ListProduct = ProductsService.GetListItemsNoOCOP();

			if (IdDC == 0)
			{
				data.Item = new Manufacturer();
			}
			else
			{
				data.Item = ManufacturerService.GetItem(IdDC, controllerSecret + HttpContext.Request.Headers["UserName"]);
				data.Item.List_IdProduct = ProductsService.GetListIdByManufacturer(IdDC);
			}

			return View(data);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult SaveItem(Manufacturer model)
		{
			string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
			int IdDC = Int32.Parse(MyModels.Decode(model.Ids, controllerSecret + HttpContext.Request.Headers["UserName"]).ToString());
			ManufacturerModel data = new ManufacturerModel() { Item = model };
			if (ModelState.IsValid)
			{
				if (model.Id == IdDC)
				{
					model.CreatedBy = model.ModifiedBy = int.Parse(HttpContext.Request.Headers["Id"]);
					// Start Table Products_Manufacturers ----
					DataTable tbItem = new DataTable();
					tbItem.Columns.Add("IdProduct", typeof(int));
					tbItem.Columns.Add("IdManufacturer", typeof(int));

					if (model.Id != 0 && model.List_IdProduct != null && model.List_IdProduct.Count() > 0)
					{
						for (int i = 0; i < model.List_IdProduct.Count(); i++)
						{
							var row = tbItem.NewRow();
							row["IdManufacturer"] = model.Id;
							row["IdProduct"] = model.List_IdProduct[i];
							tbItem.Rows.Add(row);
						}
					}

					dynamic DataSave = ManufacturerService.SaveItem(model, tbItem);

					if (model.Id > 0)
					{
						TempData["MessageSuccess"] = "Cập nhật thành công";
					}
					else
					{
						TempData["MessageSuccess"] = "Thêm mới thành công";
					}
					return RedirectToAction("Index");
				}
			}
			return View(data);
		}

		[ValidateAntiForgeryToken]
		public ActionResult DeleteItem(string Id)
		{
			string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
			Manufacturer item = new Manufacturer() { Id = Int32.Parse(MyModels.Decode(Id, controllerSecret + HttpContext.Request.Headers["UserName"]).ToString()) };
			try
			{
				if (item.Id > 0)
				{
					item.CreatedBy = item.ModifiedBy = int.Parse(HttpContext.Request.Headers["Id"]);

					dynamic DataDelete = ManufacturerService.DeleteItem(item);
					TempData["MessageSuccess"] = "Xóa thành công";
					return Json(new MsgSuccess());
				}
				else
				{
					TempData["MessageError"] = "Xóa Không thành công";
					return Json(new MsgError());
				}
			}
			catch
			{
				TempData["MessageSuccess"] = "Xóa không thành công";
				return Json(new MsgError());
			}
		}

		[ValidateAntiForgeryToken]
		public ActionResult UpdateStatus([FromQuery] string Ids, Boolean Status)
		{
			string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
			Manufacturer item = new Manufacturer() { Id = Int32.Parse(MyModels.Decode(Ids, controllerSecret + HttpContext.Request.Headers["UserName"]).ToString()), Status = Status };
			try
			{
				if (item.Id > 0)
				{
					item.CreatedBy = int.Parse(HttpContext.Request.Headers["Id"]);
					item.ModifiedBy = int.Parse(HttpContext.Request.Headers["Id"]);
					dynamic UpdateStatus = ManufacturerService.UpdateStatus(item);
					TempData["MessageSuccess"] = "Cập nhật Trạng Thái thành công";
					return Json(new MsgSuccess());
				}
				else
				{
					TempData["MessageError"] = "Cập nhật Trạng Thái Không thành công";
					return Json(new MsgError());
				}
			}
			catch
			{
				TempData["MessageSuccess"] = "Cập nhật Trạng Thái không thành công";
				return Json(new MsgError());
			}
		}

		[ValidateAntiForgeryToken]
		public IActionResult GetListSelectItem()
		{
			List<SelectListItem> ListItems = ManufacturerService.GetListSelectItems();
			return Json(new MsgSuccess() { Data = ListItems });
		}
	}
}