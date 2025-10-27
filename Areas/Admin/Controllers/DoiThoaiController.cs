using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using API.Areas.Admin.Models.DoiThoai;
using API.Areas.Admin.Models.CategoriesDoiThoai;
using API.Models;
using Newtonsoft.Json;
using API.Areas.Admin.Models.DMCoQuan;
using API.Areas.Admin.Models.USUsers;

namespace API.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class DoiThoaiController : Controller
	{
		public IActionResult Index([FromQuery] SearchDoiThoai dto)
		{
			var Login = HttpContext.Session.GetString("Login");
			USUsers MyInfo = JsonConvert.DeserializeObject<USUsers>(Login);
			int TotalItems = 0;
			if (dto.IdCoQuan == 0) { dto.IdCoQuan = MyInfo.IdCoQuan; }
			string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
			DoiThoaiModel data = new DoiThoaiModel() { SearchData = dto };
			data.ListItems = DoiThoaiService.GetListPagination(data.SearchData, API.Models.Settings.SecretId + ControllerName);
			if (data.ListItems != null && data.ListItems.Count() > 0)
			{
				TotalItems = data.ListItems[0].TotalRows;
			}

			data.ListDMCoQuan = DMCoQuanService.GetListByLoaiCoQuan(0, 0, MyInfo.IdCoQuan, false);
			data.Pagination = new Models.Partial.PartialPagination() { CurrentPage = data.SearchData.CurrentPage, ItemsPerPage = data.SearchData.ItemsPerPage, TotalItems = TotalItems, QueryString = Request.QueryString.ToString() };
			data.ListItemsCategories = CategoriesDoiThoaiService.GetListSelectItems();
			return View(data);
		}

		public IActionResult SaveItem(string Id = null, int CatId = 0, int IdCoQuan = 1)
		{

			DoiThoaiModel data = new DoiThoaiModel();
			string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
			int IdDC = Int32.Parse(MyModels.Decode(Id, API.Models.Settings.SecretId + ControllerName).ToString());
			data.SearchData = new SearchDoiThoai() { CurrentPage = 0, ItemsPerPage = 10, Keyword = "", CatId = CatId, IdCoQuan = IdCoQuan };
			data.ListItemsCategories = CategoriesDoiThoaiService.GetListSelectItems();
			data.ListDMCoQuan = DMCoQuanService.GetListByParent(1, int.Parse(HttpContext.Request.Headers["IdCoQuan"]));
			if (IdDC == 0)
			{
				data.Item = new DoiThoai() { CatId = CatId };
			}
			else
			{
				data.Item = DoiThoaiService.GetItem(IdDC, API.Models.Settings.SecretId + ControllerName);
			}


			return View(data);
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult SaveItem(DoiThoai model)
		{
			string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
			int IdDC = Int32.Parse(MyModels.Decode(model.Ids, API.Models.Settings.SecretId + ControllerName).ToString());
			DoiThoaiModel data = new DoiThoaiModel() { Item = model };

			if (ModelState.IsValid)
			{
				if (model.Id == IdDC)
				{
					model.CreatedBy = int.Parse(HttpContext.Request.Headers["Id"]);
					model.ModifiedBy = int.Parse(HttpContext.Request.Headers["Id"]);
					DoiThoaiService.SaveItem(model);
					if (model.Id > 0)
					{
						TempData["MessageSuccess"] = "Cập nhật thành công";
					}
					else
					{
						TempData["MessageSuccess"] = "Thêm mới thành công";
					}
					return RedirectToAction("Index", new { CatId = model.CatId, IdCoQuan = int.Parse(HttpContext.Request.Headers["IdCoQuan"]) });
				}
			}
			else
			{
				data.ListItemsCategories = CategoriesDoiThoaiService.GetListSelectItems();
				data.ListDMCoQuan = DMCoQuanService.GetListByParent(1, int.Parse(HttpContext.Request.Headers["IdCoQuan"]));
			}
			return View(data);
		}

		[ValidateAntiForgeryToken]
		public ActionResult DeleteItem(string Id)
		{
			string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
			DoiThoai model = new DoiThoai() { Id = Int32.Parse(MyModels.Decode(Id, API.Models.Settings.SecretId + ControllerName).ToString()) };
			try
			{
				if (model.Id > 0)
				{
					model.CreatedBy = int.Parse(HttpContext.Request.Headers["Id"]);
					model.ModifiedBy = int.Parse(HttpContext.Request.Headers["Id"]);
					dynamic result = DoiThoaiService.DeleteItem(model);
					if (result.N == 0)
					{
						TempData["MessageError"] = "Xóa Không thành công. Không cho phép xóa.";
					}
					else
					{
						TempData["MessageSuccess"] = "Xóa thành công";
					}
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
			DoiThoai item = new DoiThoai() { Id = Int32.Parse(MyModels.Decode(Ids, API.Models.Settings.SecretId + ControllerName).ToString()), Status = Status };
			try
			{
				if (item.Id > 0)
				{
					item.CreatedBy = int.Parse(HttpContext.Request.Headers["Id"]);
					item.ModifiedBy = int.Parse(HttpContext.Request.Headers["Id"]);
					dynamic UpdateStatus = DoiThoaiService.UpdateStatus(item);
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
