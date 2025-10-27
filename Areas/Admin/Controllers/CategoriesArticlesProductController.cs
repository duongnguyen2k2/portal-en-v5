using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using API.Areas.Admin.Models.CategoriesArticlesProduct;
using API.Models;
using API.Models.Utilities;
using Microsoft.Extensions.Configuration;

namespace API.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class CategoriesArticlesProductController : Controller
	{
		private string controllerName = "CategoriesArticlesProductController";
		private string controllerSecret;
		public CategoriesArticlesProductController(IConfiguration config)
		{
			controllerSecret = config["Security:SecretId"] + controllerName;
		}

		public IActionResult Index([FromQuery] SearchCategoriesArticlesProduct dto)
		{
			int TotalItems = 0;
			dto.IdCoQuan = int.Parse(HttpContext.Request.Headers["IdCoQuan"]);
			CategoriesArticlesProductModel data = new CategoriesArticlesProductModel() { SearchData = dto };

			data.ListItems = CategoriesArticlesProductService.GetListPagination(data.SearchData, controllerSecret + HttpContext.Request.Headers["UserName"]);

			if (data.ListItems != null && data.ListItems.Count() > 0)
			{
				TotalItems = data.ListItems[0].TotalRows;
			}

			data.Pagination = new Models.Partial.PartialPagination() { CurrentPage = data.SearchData.CurrentPage, ItemsPerPage = data.SearchData.ItemsPerPage, TotalItems = TotalItems, QueryString = Request.QueryString.ToString() };

			return View(data);
		}

		public IActionResult SaveItem(string Id = null)
		{
			int IdCoQuan = int.Parse(HttpContext.Request.Headers["IdCoQuan"]);
			CategoriesArticlesProductModel data = new CategoriesArticlesProductModel();
			int IdDC = Int32.Parse(MyModels.Decode(Id, controllerSecret + HttpContext.Request.Headers["UserName"]).ToString());
			data.SearchData = new SearchCategoriesArticlesProduct() { CurrentPage = 0, ItemsPerPage = 10, Keyword = "" };

			if (IdDC == 0)
			{
				data.Item = new CategoriesArticlesProduct();
			}
			else
			{
				data.Item = CategoriesArticlesProductService.GetItem(IdDC, controllerSecret + HttpContext.Request.Headers["UserName"], IdCoQuan);
			}

			return View(data);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> SaveItem(CategoriesArticlesProductModel model)
		{
			model.Item.IdCoQuan = int.Parse(HttpContext.Request.Headers["IdCoQuan"]);
			int IdDC = Int32.Parse(MyModels.Decode(model.Item.Ids, controllerSecret + HttpContext.Request.Headers["UserName"]).ToString());
			CategoriesArticlesProductModel data = model;

			if (ModelState.IsValid)
			{
				if (model.Item.Id == IdDC)
				{
					model.Item.CreatedBy = int.Parse(HttpContext.Request.Headers["Id"]);
					model.Item.ModifiedBy = int.Parse(HttpContext.Request.Headers["Id"]);
					var resultObj = CategoriesArticlesProductService.SaveItem(model.Item);

					if (resultObj.N == -2)
					{
						TempData["MessageError"] = "Chọn danh mục cha không hợp lệ";
						data.ListItemsDanhMuc = CategoriesArticlesProductService.GetListItems();
						return View(data);
					}

					TempData["MessageSuccess"] = "Cập nhật thành công";
					return RedirectToAction("Index");
				}
			}

			data.ListItemsDanhMuc = CategoriesArticlesProductService.GetListItems();
			return View(data);
		}

		public IActionResult SaveItemInfo(string Id = null)
		{
			int IdCoQuan = int.Parse(HttpContext.Request.Headers["IdCoQuan"]);
			CategoriesArticlesProductModel data = new CategoriesArticlesProductModel();

			int IdDC = Int32.Parse(MyModels.Decode(Id, controllerSecret + HttpContext.Request.Headers["UserName"]).ToString());
			data.ListItemsDanhMuc = CategoriesArticlesProductService.GetListItems();
			data.SearchData = new SearchCategoriesArticlesProduct() { CurrentPage = 0, ItemsPerPage = 10, Keyword = "" };

			if (IdDC == 0)
			{
				data.Item = new CategoriesArticlesProduct();
			}
			else
			{
				data.Item = CategoriesArticlesProductService.GetItem(IdDC, controllerSecret + HttpContext.Request.Headers["UserName"], IdCoQuan);
			}

			data.Item.Ordering = data.Item.OrderingHome;

			return View(data);
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> SaveItemInfo(CategoriesArticlesProductModel model)
		{

			model.Item.IdCoQuan = int.Parse(HttpContext.Request.Headers["IdCoQuan"]);

			int IdDC = Int32.Parse(MyModels.Decode(model.Item.Ids, controllerSecret + HttpContext.Request.Headers["UserName"]).ToString());
			CategoriesArticlesProductModel data = model;
			if (ModelState.IsValid)
			{

				if (model.Item.Id == IdDC)
				{
					model.Item.CreatedBy = int.Parse(HttpContext.Request.Headers["Id"]);
					model.Item.ModifiedBy = int.Parse(HttpContext.Request.Headers["Id"]);
					var Obj = CategoriesArticlesProductService.SaveItemInfo(model.Item);
					if (Obj.N == -2)
					{
						TempData["MessageError"] = "Chọn danh mục cha không hợp lệ";
						data.ListItemsDanhMuc = CategoriesArticlesProductService.GetListItems();
						return View(data);
					}
					TempData["MessageSuccess"] = "Cập nhật thành công";
					return RedirectToAction("Index");
				}
			}
			data.ListItemsDanhMuc = CategoriesArticlesProductService.GetListItems();
			return View(data);
		}
		[ValidateAntiForgeryToken]
		public ActionResult DeleteItem(string Id)
		{
			CategoriesArticlesProduct model = new CategoriesArticlesProduct() { Id = Int32.Parse(MyModels.Decode(Id, controllerSecret + HttpContext.Request.Headers["UserName"]).ToString()) };
			try
			{
				if (model.Id > 0)
				{
					model.CreatedBy = int.Parse(HttpContext.Request.Headers["Id"]);
					model.ModifiedBy = int.Parse(HttpContext.Request.Headers["Id"]);
					CategoriesArticlesProductService.DeleteItem(model);
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
		public ActionResult UpdateFeaturedHome([FromQuery] string Ids, Boolean FeaturedHome)
		{
			CategoriesArticlesProduct item = new CategoriesArticlesProduct() { Id = Int32.Parse(MyModels.Decode(Ids, controllerSecret + HttpContext.Request.Headers["UserName"]).ToString()), FeaturedHome = FeaturedHome, IdCoQuan = int.Parse(HttpContext.Request.Headers["IdCoQuan"]) };
			try
			{
				if (item.Id > 0)
				{
					item.CreatedBy = int.Parse(HttpContext.Request.Headers["Id"]);
					item.ModifiedBy = int.Parse(HttpContext.Request.Headers["Id"]);
					dynamic UpdateStatus = CategoriesArticlesProductService.UpdateFeaturedHome(item);
					TempData["MessageSuccess"] = "Cập nhật Hiện trang chủ thành công";
					return Json(new MsgSuccess());
				}
				else
				{
					TempData["MessageError"] = "Cập nhật Hiện trang chủ Không thành công";
					return Json(new MsgError());
				}
			}
			catch
			{
				TempData["MessageSuccess"] = "Cập nhật Hiện trang chủ không thành công";
				return Json(new MsgError());
			}
		}

		[ValidateAntiForgeryToken]
		public ActionResult UpdateStatus([FromQuery] string Ids, Boolean Status)
		{
			CategoriesArticlesProduct item = new CategoriesArticlesProduct() { Id = Int32.Parse(MyModels.Decode(Ids, controllerSecret + HttpContext.Request.Headers["UserName"]).ToString()), Status = Status };
			try
			{
				if (item.Id > 0)
				{
					item.CreatedBy = int.Parse(HttpContext.Request.Headers["Id"]);
					item.ModifiedBy = int.Parse(HttpContext.Request.Headers["Id"]);
					dynamic UpdateStatus = CategoriesArticlesProductService.UpdateStatus(item);
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
	}
}
