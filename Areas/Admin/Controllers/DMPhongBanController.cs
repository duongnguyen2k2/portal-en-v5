using API.Areas.Admin.Models.DMPhongBan;
using API.Areas.Admin.Models.USUsers;
using API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace API.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class DMPhongBanController : Controller
	{
		private string controllerName = "DMPhongBanController";
		private string controllerSecret;
		public DMPhongBanController(IConfiguration config)
		{

			controllerSecret = config["Security:SecretId"] + controllerName;
		}

		public IActionResult Index([FromQuery] SearchDMPhongBan dto)
		{
			int TotalItems = 0;

			var Login = HttpContext.Session.GetString("Login");
			USUsers MyInfo = JsonConvert.DeserializeObject<USUsers>(Login);

			if (dto.IdCoQuan == 0) { dto.IdCoQuan = MyInfo.IdCoQuan; }

			DMPhongBanModel data = new DMPhongBanModel() { SearchData = dto };

			data.ListItems = DMPhongBanService.GetListPagination(data.SearchData, controllerSecret + HttpContext.Request.Headers["UserName"]);
			if (data.ListItems != null && data.ListItems.Count() > 0)
			{
				TotalItems = data.ListItems[0].TotalRows;
			}
			data.Pagination = new Models.Partial.PartialPagination() { CurrentPage = data.SearchData.CurrentPage, ItemsPerPage = data.SearchData.ItemsPerPage, TotalItems = TotalItems, QueryString = Request.QueryString.ToString() };
			data.ListItemsDanhMuc = DMPhongBanService.GetListTypes();
			return View(data);
		}

		public IActionResult SaveItem(string Id = null, int IdType = 0)
		{
			DMPhongBanModel data = new DMPhongBanModel();

			int IdDC = Int32.Parse(MyModels.Decode(Id, controllerSecret + HttpContext.Request.Headers["UserName"]).ToString());

			data.SearchData = new SearchDMPhongBan() { CurrentPage = 0, ItemsPerPage = 10, Keyword = "",IdType= IdType };

			if (IdDC == 0)
			{
				data.Item = new DMPhongBan() { IdType = IdType };
			}
			else
			{
				data.Item = DMPhongBanService.GetItem(IdDC, controllerSecret);
			}

			data.ListItemsDanhMuc = DMPhongBanService.GetListTypes();
			return View(data);
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult SaveItem(DMPhongBan model)
		{

			int IdDC = Int32.Parse(MyModels.Decode(model.Ids, controllerSecret + HttpContext.Request.Headers["UserName"]).ToString());
			DMPhongBanModel data = new DMPhongBanModel() { Item = model };
			if (ModelState.IsValid)
			{
				if (model.Alias == null || model.Alias == "")
				{
					model.Alias = API.Models.MyHelper.StringHelper.UrlFriendly(model.Title);
				}

				if (model.Id == IdDC)
				{
					model.IdCoQuan = int.Parse(HttpContext.Request.Headers["IdCoQuan"]);
					model.CreatedBy = model.ModifiedBy = int.Parse(HttpContext.Request.Headers["Id"]);
					dynamic DataSave = DMPhongBanService.SaveItem(model);
					if (model.Id > 0)
					{
						TempData["MessageSuccess"] = "Cập nhật thành công";
					}
					else
					{
						TempData["MessageSuccess"] = "Thêm mới thành công";
					}
                    return RedirectToAction("Index", new { IdType = model.IdType, IdCoQuan = int.Parse(HttpContext.Request.Headers["IdCoQuan"]) });
                }
			}
			data.ListItemsDanhMuc = DMPhongBanService.GetListTypes();
			return View(data);
		}

		[ValidateAntiForgeryToken]
		public ActionResult DeleteItem(string Id)
		{

			DMPhongBan item = new DMPhongBan() { Id = Int32.Parse(MyModels.Decode(Id, controllerSecret + HttpContext.Request.Headers["UserName"]).ToString()) };
			try
			{
				if (item.Id > 0)
				{
					item.CreatedBy = item.ModifiedBy = int.Parse(HttpContext.Request.Headers["Id"]);

					dynamic DataDelete = DMPhongBanService.DeleteItem(item);
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

			DMPhongBan item = new DMPhongBan() { Id = Int32.Parse(MyModels.Decode(Ids, controllerSecret + HttpContext.Request.Headers["UserName"]).ToString()), Status = Status };
			try
			{
				if (item.Id > 0)
				{
					item.CreatedBy = int.Parse(HttpContext.Request.Headers["Id"]);
					item.ModifiedBy = int.Parse(HttpContext.Request.Headers["Id"]);
					dynamic UpdateStatus = DMPhongBanService.UpdateStatus(item);
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