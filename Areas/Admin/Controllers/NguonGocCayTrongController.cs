using API.Areas.Admin.Models.DMPhuongXa;
using API.Areas.Admin.Models.DMQuanHuyen;
using API.Areas.Admin.Models.NguonGocCayTrong;
using API.Areas.Admin.Models.USUsers;
using API.Models;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace API.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class NguonGocCayTrongController : Controller
	{
		private string controllerName = "NguonGocCayTrongController";
		private string controllerSecret;
		public NguonGocCayTrongController(IConfiguration config)
		{

			controllerSecret = config["Security:SecretId"] + controllerName;
		}

		public IActionResult Index([FromQuery] SearchNguonGocCayTrong dto)
		{
			int TotalItems = 0;

			NguonGocCayTrongModel data = new NguonGocCayTrongModel() { SearchData = dto };


			data.ListItems = NguonGocCayTrongService.GetListPagination(data.SearchData, controllerSecret + HttpContext.Request.Headers["UserName"]);
			if (data.ListItems != null && data.ListItems.Count() > 0)
			{
				TotalItems = data.ListItems[0].TotalRows;
			}
			data.Pagination = new Models.Partial.PartialPagination() { CurrentPage = data.SearchData.CurrentPage, ItemsPerPage = data.SearchData.ItemsPerPage, TotalItems = TotalItems, QueryString = Request.QueryString.ToString() };

			return View(data);
		}

		public IActionResult ImportExcel()
		{
			try
			{
				string pass = USUsersService.GetMD5("Abc@123456");

				List<NguonGocCayTrong> listItems = new List<NguonGocCayTrong>();

				string filePath = Path.Combine("wwwroot", "temp/FileUpload");

				XLWorkbook workBook = new XLWorkbook(filePath + "/" + "ma-vung-trong.xlsx");
				IXLWorksheet workSheet = workBook.Worksheet(1);
				bool firstRow = true;
				foreach (IXLRow row in workSheet.Rows())
				{
					if (firstRow)
					{
						firstRow = false;
					}
					else
					{
						int i = -1;
						NguonGocCayTrong item = new NguonGocCayTrong() { Id = 0, Status = true };
						foreach (IXLCell cell in row.Cells())
						{
							i++;
							if (i == 0)
							{
								item.UserName = cell.Value.ToString();
								item.Password = pass;
								item.Status = true;
								item.CreatedBy = 1;
							}
							if (i == 1) { item.IdPhuongXa = Int32.Parse(cell.Value.ToString()); }
							if (i == 2) { item.Title = cell.Value.ToString(); }
							if (i == 3) { item.Description = cell.Value.ToString(); }
							if (i == 4) { item.Latitude = cell.Value.ToString(); }
							if (i == 5) { item.Longitude = cell.Value.ToString(); }
							if (i == 6) { item.Address = cell.Value.ToString(); }


						}
						if (item.UserName != null && item.UserName.Trim() != "")
						{
							listItems.Add(item);
							dynamic DataSave = NguonGocCayTrongService.SaveItem(item);

						}

					}
				}

				return Json(new API.Models.MsgSuccess() { Msg = "Thanh Cong", Data = listItems });
			}
			catch (Exception e)
			{
				return Json(new API.Models.MsgError() { Msg = e.Message });
			}
		}

		public IActionResult SaveItem(string Id = null)
		{
			NguonGocCayTrongModel data = new NguonGocCayTrongModel();

			int IdDC = Int32.Parse(MyModels.Decode(Id, controllerSecret + HttpContext.Request.Headers["UserName"]).ToString());
			data.SearchData = new SearchNguonGocCayTrong() { CurrentPage = 0, ItemsPerPage = 10, Keyword = "" };

			if (IdDC == 0)
			{
				data.Item = new NguonGocCayTrong();
			}
			else
			{
				data.Item = NguonGocCayTrongService.GetItem(IdDC, controllerSecret + HttpContext.Request.Headers["UserName"]);
				if (data.Item != null && data.Item.IdQuanHuyen > 0)
				{
					data.ListItemsDMPhuongXa = DMPhuongXaService.GetListSelectItems(data.Item.IdQuanHuyen);
				}
			}

			data.ListItemsDMQuanHuyen = DMQuanHuyenService.GetListSelectItems(42);

			return View(data);
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult SaveItem(NguonGocCayTrong model)
		{
			string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
			int IdDC = Int32.Parse(MyModels.Decode(model.Ids, controllerSecret + HttpContext.Request.Headers["UserName"]).ToString());
			NguonGocCayTrongModel data = new NguonGocCayTrongModel() { Item = model };
			if (ModelState.IsValid)
			{
				if (model.Id == IdDC)
				{
					model.CreatedBy = model.ModifiedBy = int.Parse(HttpContext.Request.Headers["Id"]);

					if (model.Id == 0)
					{
						if (model.Password == null || model.Password == null)
						{
							model.Password = "Abc@123";
						}
						model.Password = USUsersService.GetMD5(model.Password);
					}

					dynamic DataSave = NguonGocCayTrongService.SaveItem(model);
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

			NguonGocCayTrong item = new NguonGocCayTrong() { Id = Int32.Parse(MyModels.Decode(Id, controllerSecret + HttpContext.Request.Headers["UserName"]).ToString()) };
			try
			{
				if (item.Id > 0)
				{
					item.CreatedBy = item.ModifiedBy = int.Parse(HttpContext.Request.Headers["Id"]);

					dynamic DataDelete = NguonGocCayTrongService.DeleteItem(item);
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
			NguonGocCayTrong item = new NguonGocCayTrong() { Id = Int32.Parse(MyModels.Decode(Ids, controllerSecret + HttpContext.Request.Headers["UserName"]).ToString()), Status = Status };
			try
			{
				if (item.Id > 0)
				{
					item.CreatedBy = int.Parse(HttpContext.Request.Headers["Id"]);
					item.ModifiedBy = int.Parse(HttpContext.Request.Headers["Id"]);
					dynamic UpdateStatus = NguonGocCayTrongService.UpdateStatus(item);
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
		public ActionResult RessetPassword(string Id)
		{

			NguonGocCayTrong item = new NguonGocCayTrong() { Id = Int32.Parse(MyModels.Decode(Id, controllerSecret + HttpContext.Request.Headers["UserName"]).ToString()) };
			try
			{
				if (item.Id > 0)
				{
					item.CreatedBy = int.Parse(HttpContext.Request.Headers["Id"]);
					item.ModifiedBy = int.Parse(HttpContext.Request.Headers["Id"]);
					item.Password = USUsersService.GetMD5("Abc@123");
					dynamic DataDelete = NguonGocCayTrongService.ChangePassword(item.Id, item.Password);
					string Msg = "Cập nhật mật khẩu thành công. Mật khẩu mặc định là <strong>" + "Abc@123" + "<strong>";
					return Json(new MsgSuccess() { Msg = Msg });
				}
				else
				{
					string Msg = "Cập nhật mật khẩu Không thành công. Xin vui lòng làm lại";
					return Json(new MsgError() { Msg = Msg });
				}
			}
			catch
			{
				string Msg = "Cập nhật mật khẩu Không thành công. Xin vui lòng làm lại.";
				return Json(new MsgError() { Msg = Msg });
			}


		}
		[HttpPost]
		public IActionResult ImportListNguonGocCayTrong([FromBody] List<NguonGocCayTrong> dto)
		{

			if (dto != null && dto.Count() > 0)
			{
				for (int i = 0; i < dto.Count(); i++)
				{

				}
			}
			return View();
		}
		[HttpPost]
		public async Task<dynamic> UploadExcelNguonGocCayTrong()
		{
			try
			{
				List<NguonGocCayTrong> listItems = new List<NguonGocCayTrong>();
				var file = Request.Form.Files[0];
				string filePath = Path.Combine("temp", "FileUpload");
				if (!Directory.Exists(filePath))
				{
					Directory.CreateDirectory(filePath);
				}
				if (file.Length > 0)
				{
					using (var fileStream = new FileStream(Path.Combine(filePath, file.FileName), FileMode.Create))
					{
						await file.CopyToAsync(fileStream);

					}
					XLWorkbook workBook = new XLWorkbook(filePath + "/" + file.FileName);
					IXLWorksheet workSheet = workBook.Worksheet(1);
					bool firstRow = true;
					foreach (IXLRow row in workSheet.Rows())
					{
						if (firstRow)
						{
							firstRow = false;
						}
						else
						{
							int i = -1;
							NguonGocCayTrong item = new NguonGocCayTrong() { Id = 0, Status = true };
							foreach (IXLCell cell in row.Cells())
							{
								i++;
								if (i == 0) { item.UserName = cell.Value.ToString(); }
								if (i == 1) { item.IdPhuongXa = Int32.Parse(cell.Value.ToString()); }
								if (i == 2) { item.FullName = cell.Value.ToString(); }
								if (i == 3) { item.Description = cell.Value.ToString(); }
								if (i == 4) { item.Latitude = cell.Value.ToString(); }
								if (i == 5) { item.Longitude = cell.Value.ToString(); }
								if (i == 6) { item.Address = cell.Value.ToString(); }
							}
							if (item.Title != null && item.Title.Trim() != "")
							{
								listItems.Add(item);
							}

						}
					}
				}

				return new API.Models.MsgSuccess() { Msg = "Đỗ Dữ Liệu Thành Công", Data = listItems };
			}
			catch (Exception e)
			{
				return new API.Models.MsgError() { Msg = e.Message };
			}
		}
	}
}