using API.Areas.Admin.Models.DocumentRefers;
using API.Areas.Admin.Models.DocumentRefersCategories;
using API.Areas.Admin.Models.ManagerFile;
using API.Areas.Admin.Models.ManagerFiles;
using API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace API.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class DocumentRefersController : Controller
	{
		public IActionResult IndexTaiLieuHop([FromQuery] SearchDocumentRefers dto)
		{
			int TotalItems = 0;
			string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
			dto.TaiLieuHop = true;
			dto.IdCoQuan = int.Parse(HttpContext.Request.Headers["IdCoQuan"]);

			DocumentRefersModel data = new DocumentRefersModel() { SearchData = dto };
			data.ListItems = DocumentRefersService.GetListPagination(data.SearchData, API.Models.Settings.SecretId + ControllerName);
			if (data.ListItems != null && data.ListItems.Count() > 0)
			{
				TotalItems = data.ListItems[0].TotalRows;
			}
			data.Pagination = new Models.Partial.PartialPagination() { CurrentPage = data.SearchData.CurrentPage, ItemsPerPage = data.SearchData.ItemsPerPage, TotalItems = TotalItems, QueryString = Request.QueryString.ToString() };
			data.ListItemsStatus = DocumentRefersService.GetListItemsStatus();
			data.ListDocumentsCategories = DocumentRefersCategoriesService.GetListSelectItems(dto.IdCoQuan, dto.TaiLieuHop);
			return View(data);
		}

		public IActionResult Index([FromQuery] SearchDocumentRefers dto)
		{
			int TotalItems = 0;
			string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
			dto.TaiLieuHop = false;

			dto.IdCoQuan = int.Parse(HttpContext.Request.Headers["IdCoQuan"]);
			DocumentRefersModel data = new DocumentRefersModel() { SearchData = dto };
			data.ListItems = DocumentRefersService.GetListPagination(data.SearchData, API.Models.Settings.SecretId + ControllerName);
			if (data.ListItems != null && data.ListItems.Count() > 0)
			{
				TotalItems = data.ListItems[0].TotalRows;
			}
			data.Pagination = new Models.Partial.PartialPagination() { CurrentPage = data.SearchData.CurrentPage, ItemsPerPage = data.SearchData.ItemsPerPage, TotalItems = TotalItems, QueryString = Request.QueryString.ToString() };
			data.ListItemsStatus = DocumentRefersService.GetListItemsStatus();
			data.ListDocumentsCategories = DocumentRefersCategoriesService.GetListSelectItems(dto.IdCoQuan, dto.TaiLieuHop);
			return View(data);
		}

		public IActionResult SaveItemTaiLieuHop(string Id = null, int CatId = 0)
		{
			DocumentRefersModel data = new DocumentRefersModel();
			string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
			int IdDC = Int32.Parse(MyModels.Decode(Id, API.Models.Settings.SecretId + ControllerName).ToString());
			data.SearchData = new SearchDocumentRefers()
			{
				CurrentPage = 0,
				ItemsPerPage = 10,
				TaiLieuHop = true,
				CatId = CatId,
				Keyword = "",
				IdCoQuan = int.Parse(HttpContext.Request.Headers["IdCoQuan"])
			};
			data.ListDocumentsCategories = DocumentRefersCategoriesService.GetListSelectItems(data.SearchData.IdCoQuan, data.SearchData.TaiLieuHop);
			DocumentRefers Item = new DocumentRefers()
			{
				IssuedDateShow = DateTime.Now.ToString("dd/MM/yyyy"),
				Status = true,
				CatId = CatId,
				IdCoQuan = int.Parse(HttpContext.Request.Headers["IdCoQuan"])
			};
			if (IdDC > 0)
			{
				Item = DocumentRefersService.GetItem(IdDC, API.Models.Settings.SecretId + ControllerName);
			}

			data.Item = Item;

			return View(data);
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> SaveItemTaiLieuHop(DocumentRefers model)
		{
			List<String> listMIMETypes = MIMETypesService.GetListTinymce();
			List<ManagerFile.TinymceFile> ListFiles = new List<ManagerFile.TinymceFile>();
			string Size = "";
			string Extension = "";
			Boolean flagSave = false;

			string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
			int IdDC = Int32.Parse(MyModels.Decode(model.Ids, API.Models.Settings.SecretId + ControllerName).ToString());
			DocumentRefersModel data = new DocumentRefersModel() { Item = model };
			model.IdCoQuan = int.Parse(HttpContext.Request.Headers["IdCoQuan"]);
			data.ListDocumentsCategories = DocumentRefersCategoriesService.GetListSelectItems(model.IdCoQuan, true);

			if (ModelState.IsValid)
			{
				if (model.Id == IdDC)
				{
					model.CreatedBy = int.Parse(HttpContext.Request.Headers["Id"]);
					model.ModifiedBy = int.Parse(HttpContext.Request.Headers["Id"]);
					model.IssuedDateShow = DateTime.Now.ToString("dd/MM/yyyy");
					if (Request.Form.Files.Count() > 0)
					{
						Extension = Path.GetExtension(Request.Form.Files[0].FileName);
						Size = Request.Form.Files[0].Length.ToString();
						for (int j = 0; j < listMIMETypes.Count; j++)
						{
							if (Extension.ToLower() == listMIMETypes[j].ToLower())
							{
								flagSave = true;
							}
						}
						if (flagSave)
						{
							try
							{
								if (model.Id == 0)
								{
									model.KeyLink = API.Models.MyHelper.StringHelper.RandomString(10);
								}
								string NewFile = DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + API.Models.MyHelper.StringHelper.UrlFriendly(Request.Form.Files[0].FileName.Replace(Extension, ""));

								model.LinkPrivate = NewFile + Extension;

								string filePath = Path.Combine(Directory.GetCurrentDirectory(), "TaiLieuHop/") + model.LinkPrivate;

								using (var fileStream = new FileStream(filePath, FileMode.Create))
								{
									await Request.Form.Files[0].CopyToAsync(fileStream);
								}
								DocumentRefersService.SaveItem(model);
								if (model.Id > 0)
								{
									TempData["MessageSuccess"] = "Cập nhật thành công";
								}
								else
								{
									TempData["MessageSuccess"] = "Thêm mới thành công";
								}
								return RedirectToAction("IndexTaiLieuHop", new { CatId = model.CatId, IdCoQuan = int.Parse(HttpContext.Request.Headers["IdCoQuan"]) });
							}
							catch (Exception e)
							{
								TempData["MessageError"] = "Lỗi khi lưu dữ liệu " + e.ToString();
							}
						}
						else
						{
							TempData["MessageError"] = "Kiểu file không hợp lệ. Bạn chỉ gửi được file .Pdf, .doc,.docx";
						}

					}
					else
					{
						TempData["MessageError"] = "Bạn chưa chọn  file upload ";
					}


				}
			}
			return View(data);
		}


		public IActionResult SaveItem(string Id = null)
		{
			DocumentRefersModel data = new DocumentRefersModel();
			string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
			int IdDC = Int32.Parse(MyModels.Decode(Id, API.Models.Settings.SecretId + ControllerName).ToString());
			data.SearchData = new SearchDocumentRefers()
			{
				CurrentPage = 0,
				ItemsPerPage = 10,
				Keyword = "",
				IdCoQuan = int.Parse(HttpContext.Request.Headers["IdCoQuan"])
			};
			data.ListDocumentsCategories = DocumentRefersCategoriesService.GetListSelectItems(data.SearchData.IdCoQuan, data.SearchData.TaiLieuHop);
			DocumentRefers Item = new DocumentRefers()
			{
				IssuedDateShow = DateTime.Now.ToString("dd/MM/yyyy"),
				Status = true,
				IdCoQuan = int.Parse(HttpContext.Request.Headers["IdCoQuan"])
			};
			if (IdDC > 0)
			{
				Item = DocumentRefersService.GetItem(IdDC, API.Models.Settings.SecretId + ControllerName);
			}

			data.Item = Item;

			return View(data);
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult SaveItem(DocumentRefers model)
		{
			string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
			int IdDC = Int32.Parse(MyModels.Decode(model.Ids, API.Models.Settings.SecretId + ControllerName).ToString());
			DocumentRefersModel data = new DocumentRefersModel() { Item = model };
			model.IdCoQuan = int.Parse(HttpContext.Request.Headers["IdCoQuan"]);

			data.ListDocumentsCategories = DocumentRefersCategoriesService.GetListSelectItems(model.IdCoQuan, false);
			if (ModelState.IsValid)
			{
				if (model.Id == IdDC)
				{
					model.CreatedBy = int.Parse(HttpContext.Request.Headers["Id"]);
					model.ModifiedBy = int.Parse(HttpContext.Request.Headers["Id"]);

					try
					{
						if (model.Id == 0)
						{
							model.KeyLink = API.Models.MyHelper.StringHelper.RandomString(10);
						}
						DocumentRefersService.SaveItem(model);
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
					catch
					{
						TempData["MessageError"] = "Lỗi khi lưu dữ liệu";
					}

				}
			}
			return View(data);
		}

		[ValidateAntiForgeryToken]
		public ActionResult DeleteItem(string Id)
		{
			string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
			DocumentRefers model = new DocumentRefers() { Id = Int32.Parse(MyModels.Decode(Id, API.Models.Settings.SecretId + ControllerName).ToString()) };
			try
			{
				if (model.Id > 0)
				{
					model.CreatedBy = int.Parse(HttpContext.Request.Headers["Id"]);
					model.ModifiedBy = int.Parse(HttpContext.Request.Headers["Id"]);
					model.IdCoQuan = int.Parse(HttpContext.Request.Headers["IdCoQuan"]);
					DocumentRefersService.DeleteItem(model);
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
			DocumentRefers item = new DocumentRefers() { Id = Int32.Parse(MyModels.Decode(Ids, API.Models.Settings.SecretId + ControllerName).ToString()), Status = Status };
			try
			{
				if (item.Id > 0)
				{
					item.CreatedBy = int.Parse(HttpContext.Request.Headers["Id"]);
					item.ModifiedBy = int.Parse(HttpContext.Request.Headers["Id"]);
					item.IdCoQuan = int.Parse(HttpContext.Request.Headers["IdCoQuan"]);
					dynamic UpdateStatus = DocumentRefersService.UpdateStatus(item);
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
