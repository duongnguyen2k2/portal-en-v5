using API.Areas.Admin.Models.ArticlesFestival;
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
	public class ArticlesFestivalController : Controller
	{
		private string controllerName = "ArticlesFestivalController";
		private string controllerSecret;
		public ArticlesFestivalController(IConfiguration config)
		{
			controllerSecret = config["Security:SecretId"] + controllerName;
		}

		public IActionResult Index()
		{
			return View();
		}

		[HttpPost]
		public IActionResult GetListJson([FromBody] SearchArticlesFestival dto)
		{
			string secret = this.controllerSecret + HttpContext.Request.Headers["UserName"].ToString();
			List<ArticlesFestival> ListItems = ArticlesFestivalService.GetListPagination(dto, secret, false);
			return Json(new MsgSuccess() { Data = ListItems });
		}

		public IActionResult SaveItem(string Id = null)
		{
			ArticlesFestivalModel data = new ArticlesFestivalModel();
			int IdUser = int.Parse(HttpContext.Request.Headers["Id"]);
			int IdCoQuan = Int32.Parse(HttpContext.Request.Headers["IdCoQuan"]);
			int IdDC = Int32.Parse(MyModels.Decode(Id, this.controllerSecret + HttpContext.Request.Headers["UserName"].ToString()).ToString());
			data.SearchData = new SearchArticlesFestival() { CurrentPage = 0, ItemsPerPage = 10, Keyword = "" };

			if (IdDC == 0)
			{
				data.Item = new ArticlesFestival() { PublishUp = DateTime.Now, PublishUpShow = DateTime.Now.ToString("dd/MM/yyyy") };
			}
			else
			{
				data.Item = ArticlesFestivalService.GetItem(IdDC, this.controllerSecret + HttpContext.Request.Headers["UserName"].ToString());
			}

			return View(data);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult SaveItem(ArticlesFestival model)
		{
			model.IdCoQuan = Int32.Parse(HttpContext.Request.Headers["IdCoQuan"]);
			int IdDC = Int32.Parse(MyModels.Decode(model.Ids, this.controllerSecret + HttpContext.Request.Headers["UserName"].ToString()).ToString());
			ArticlesFestivalModel data = new ArticlesFestivalModel() { Item = model };

			if (ModelState.IsValid)
			{
				if (model.Id == IdDC)
				{
					model.CreatedBy = model.ModifiedBy = int.Parse(HttpContext.Request.Headers["Id"]);

					if (model.Alias == null || model.Alias == "")
					{
						model.Alias = API.Models.MyHelper.StringHelper.UrlFriendly(model.Title);
					}

					var DataSave = ArticlesFestivalService.SaveItem(model);

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

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult SaveItemJson([FromBody] ArticlesFestival model)
		{
			int IdDC = 0;
			if (model != null)
			{
				IdDC = Int32.Parse(MyModels.Decode(model.Ids, controllerSecret + HttpContext.Request.Headers["UserName"]).ToString());
				ArticlesFestivalModel data = new ArticlesFestivalModel() { Item = model };
				if (ModelState.IsValid)
				{
					model.CreatedBy = model.ModifiedBy = int.Parse(HttpContext.Request.Headers["Id"]);
					model.Id = 0;
					model.Status = false;

					if (model.Alias == null || model.Alias == "")
					{
						model.Alias = API.Models.MyHelper.StringHelper.UrlFriendly(model.Title);
					}

					var DataSave = ArticlesFestivalService.SaveItem(model);

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
		public ActionResult DeleteItem([FromBody] ArticlesFestival dto)
		{
			ArticlesFestival item = new ArticlesFestival() { Id = Int32.Parse(MyModels.Decode(dto.Ids, controllerSecret + HttpContext.Request.Headers["UserName"]).ToString()) };
			try
			{
				if (item.Id > 0)
				{
					item.CreatedBy = item.ModifiedBy = int.Parse(HttpContext.Request.Headers["Id"]);
					dynamic DataDelete = ArticlesFestivalService.DeleteItem(item);
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

		[ValidateAntiForgeryToken]
		public ActionResult UpdateStatus([FromQuery] string Ids, Boolean Status)
		{
			ArticlesFestival item = new ArticlesFestival() { Id = Int32.Parse(MyModels.Decode(Ids, controllerSecret + HttpContext.Request.Headers["UserName"]).ToString()), Status = Status };
			try
			{
				if (item.Id > 0)
				{
					item.CreatedBy = int.Parse(HttpContext.Request.Headers["Id"]);
					item.ModifiedBy = int.Parse(HttpContext.Request.Headers["Id"]);
					dynamic UpdateStatus = ArticlesFestivalService.UpdateStatus(item);
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

	}
}