using API.Areas.Admin.Models.CategoriesProducts;
using API.Areas.Admin.Models.DMCoQuan;
using API.Areas.Admin.Models.Manufacturer;
using API.Areas.Admin.Models.Products;
using API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace API.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class ProductsController : Controller
	{
		private string controllerName = "ProductsController";
		private string controllerSecret;
		public ProductsController(IConfiguration config)
		{
			controllerSecret = config["Security:SecretId"] + controllerName;
		}

		public IActionResult Index()
		{
			return View();
		}

		public IActionResult DongBoDuLieu()
		{
			List<Products> ListItems = ProductsService.GetList();
			if (ListItems != null && ListItems.Count() > 0)
			{
				for (int i = 0; i < ListItems.Count(); i++)
				{
					ListItems[i].PriceDealDisplay = API.Models.MyHelper.StringHelper.ConvertNumberToDisplay(ListItems[i].Price.ToString());
					ListItems[i].PriceDisplay = API.Models.MyHelper.StringHelper.ConvertNumberToDisplay(ListItems[i].Price.ToString());
					ListItems[i].Available = ListItems[i].Quantity - ListItems[i].Sold;
					var DataSave = ProductsService.SaveItemDongBo(ListItems[i]);
				}
			}
			return Json(new MsgSuccess());
		}

		[HttpPost]
		public IActionResult GetListJson([FromBody] SearchProducts dto)
		{
			var OCOP_Status = 0;
			dto.AuthorId = int.Parse(HttpContext.Request.Headers["Id"]);
			List<Products> ListItems = ProductsService.GetListPagination(dto, this.controllerSecret + HttpContext.Request.Headers["UserName"].ToString(), OCOP_Status);
			return Json(new MsgSuccess() { Data = ListItems });
		}

		public IActionResult SaveItem(string Id = null)
		{
			ProductsModel data = new ProductsModel();
			int IdUser = int.Parse(HttpContext.Request.Headers["Id"]);
			int IdCoQuan = Int32.Parse(HttpContext.Request.Headers["IdCoQuan"]);
			int IdDC = Int32.Parse(MyModels.Decode(Id, this.controllerSecret + HttpContext.Request.Headers["UserName"].ToString()).ToString());
			data.SearchData = new SearchProducts() { CurrentPage = 0, ItemsPerPage = 10, Keyword = "" };
			data.ListItemsCategories = CategoriesProductsService.GetSelectListItems();
			data.ListItemsManufacturer = ManufacturerService.GetListSelectItems();
			data.ListDMCoQuan = DMCoQuanService.GetListByParent(0, IdCoQuan);

			data.ListStar = ProductsService.GetListStar();
			if (IdDC == 0)
			{
				data.Item = new Products() { PublishUp = DateTime.Now, PublishUpShow = DateTime.Now.ToString("dd/MM/yyyy") };
			}
			else
			{
				data.Item = ProductsService.GetItem(IdDC, this.controllerSecret + HttpContext.Request.Headers["UserName"].ToString());
				data.Item.List_IdManufacturer = ManufacturerService.GetListIdByProduct(IdDC);
			}

			return View(data);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult SaveItem(Products model)
		{
			int OCOP_Status = 0;
			int IdUser = int.Parse(HttpContext.Request.Headers["Id"]);
			int IdDC = Int32.Parse(MyModels.Decode(model.Ids, this.controllerSecret + HttpContext.Request.Headers["UserName"].ToString()).ToString());
			ProductsModel data = new ProductsModel() { Item = model };

			if (ModelState.IsValid)
			{
				if (model.Id == IdDC)
				{
					model.CreatedBy = model.ModifiedBy = int.Parse(HttpContext.Request.Headers["Id"]);

					// Start Table Products_Manufacturers ----
					DataTable tbItem = new DataTable();
					tbItem.Columns.Add("IdProduct", typeof(int));
					tbItem.Columns.Add("IdManufacturer", typeof(int));


					if (model.Id != 0 && model.List_IdManufacturer != null && model.List_IdManufacturer.Count() > 0)
					{
						for (int i = 0; i < model.List_IdManufacturer.Count(); i++)
						{
							var row = tbItem.NewRow();
							row["IdProduct"] = model.Id;
							row["IdManufacturer"] = model.List_IdManufacturer[i];
							tbItem.Rows.Add(row);
						}
					}

					// End Table Category

					if (model.Alias == null || model.Alias == "")
					{
						model.Alias = API.Models.MyHelper.StringHelper.UrlFriendly(model.Title);
					}

					var DataSave = ProductsService.SaveItem(model, tbItem, OCOP_Status);

					if (model.FlagJson)
					{
						return Json(new MsgSuccess() { Data = MyModels.Encode(DataSave.N, controllerSecret + HttpContext.Request.Headers["UserName"]) });
					}
					else
					{
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
			}
			else
			{
				data.ListItemsCategories = CategoriesProductsService.GetListItemsByUser(true, IdUser);
				data.ListStar = ProductsService.GetListStar();
			}

			if (model.Id > 0)
			{

			}
			return View(data);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult SaveItemJson([FromBody] Products model)
		{
			int OCOP_Status = 0;
			int IdDC = 0;
			if (model != null)
			{
				IdDC = Int32.Parse(MyModels.Decode(model.Ids, controllerSecret + HttpContext.Request.Headers["UserName"]).ToString());
				ProductsModel data = new ProductsModel() { Item = model };
				if (ModelState.IsValid)
				{
					model.CreatedBy = model.ModifiedBy = int.Parse(HttpContext.Request.Headers["Id"]);
					model.Id = 0;
					model.Status = false;

					// Start Table Category ----
					DataTable tbItem = new DataTable();
					tbItem.Columns.Add("ProductId", typeof(int));
					tbItem.Columns.Add("CatId", typeof(int));
					/*
                    if (model.CatMultiId != null && model.CatMultiId.Count() > 0)
                    {
                        for (int k = 0; k < model.CatMultiId.Count(); k++)
                        {
                            var row = tbItem.NewRow();
                            row["ProductId"] = 0;
                            row["CatId"] = model.CatMultiId[k];
                            tbItem.Rows.Add(row);
                        }
                    }*/
					// End Table Category

					if (model.Alias == null || model.Alias == "")
					{
						model.Alias = API.Models.MyHelper.StringHelper.UrlFriendly(model.Title);
					}

					var DataSave = ProductsService.SaveItem(model, tbItem, OCOP_Status);

					return Json(new MsgSuccess() { Data = MyModels.Encode(DataSave.N, controllerSecret + HttpContext.Request.Headers["UserName"]) });
				}
				else
				{
					var DataError = ModelState.Values.SelectMany(v => v.Errors)
								   .Select(v => v.ErrorMessage + " " + v.Exception).ToList();

					return Json(new MsgError() { Data = DataError });
				}
			}
			else
			{
				return Json(new MsgError() { Data = null });
			}

		}

		[ValidateAntiForgeryToken]
		public ActionResult DeleteItem([FromBody] Products dto)
		{
			Products item = new Products() { Id = Int32.Parse(MyModels.Decode(dto.Ids, controllerSecret + HttpContext.Request.Headers["UserName"]).ToString()) };
			try
			{
				if (item.Id > 0)
				{
					item.CreatedBy = item.ModifiedBy = int.Parse(HttpContext.Request.Headers["Id"]);
					dynamic DataDelete = ProductsService.DeleteItem(item);
					return Json(new MsgSuccess() { Msg = "Xóa thành công" });
				}
				else
				{
					return Json(new MsgError() { Msg = "Xóa Không thành công" });
				}
			}
			catch
			{
				return Json(new MsgError() { Msg = "Xóa không thành công" });
			}
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult CopyItem([FromBody] Products dto)
		{
			int IdDC = Int32.Parse(MyModels.Decode(dto.Ids, controllerSecret + HttpContext.Request.Headers["UserName"]).ToString());
			try
			{
				if (IdDC > 0 && dto.Id == IdDC)
				{
					dto.CreatedBy = dto.ModifiedBy = int.Parse(HttpContext.Request.Headers["Id"]);
					dynamic DataDelete = ProductsService.CopyItem(dto);

					return Json(new MsgSuccess());
				}
				else
				{
					return Json(new MsgError() { Msg = "Copy Không thành công" });
				}
			}
			catch
			{
				return Json(new MsgError() { Msg = "Copy Không thành công" });
			}
		}

		[ValidateAntiForgeryToken]
		public ActionResult UpdateStatus([FromQuery] string Ids, Boolean Status)
		{
			Products item = new Products() { Id = Int32.Parse(MyModels.Decode(Ids, controllerSecret + HttpContext.Request.Headers["UserName"]).ToString()), Status = Status };
			try
			{
				if (item.Id > 0)
				{
					item.CreatedBy = int.Parse(HttpContext.Request.Headers["Id"]);
					item.ModifiedBy = int.Parse(HttpContext.Request.Headers["Id"]);
					dynamic UpdateStatus = ProductsService.UpdateStatus(item);
					return Json(new MsgSuccess() { Msg = "Cập nhật Trạng Thái thành công" });
				}
				else
				{
					return Json(new MsgError() { Msg = "Cập nhật Trạng Thái Không thành công" });
				}
			}
			catch
			{
				return Json(new MsgError() { Msg = "Cập nhật Trạng Thái không thành công" });
			}
		}

		[ValidateAntiForgeryToken]
		public ActionResult UpdateFeatured([FromQuery] string Ids, Boolean Featured)
		{
			Products item = new Products() { Id = Int32.Parse(MyModels.Decode(Ids, controllerSecret + HttpContext.Request.Headers["UserName"]).ToString()), Featured = Featured };
			try
			{
				if (item.Id > 0)
				{
					item.CreatedBy = int.Parse(HttpContext.Request.Headers["Id"]);
					item.ModifiedBy = int.Parse(HttpContext.Request.Headers["Id"]);
					dynamic UpdateStatus = ProductsService.UpdateFeatured(item);
					return Json(new MsgSuccess() { Msg = "Cập nhật Trạng Thái Nổi bật thành công" });
				}
				else
				{
					return Json(new MsgError() { Msg = "Cập nhật Trạng Thái Nổi bật Không thành công" });
				}
			}
			catch
			{
				return Json(new MsgError() { Msg = "Cập nhật Trạng Thái Nổi bật không thành công" });
			}
		}

	}
}