using API.Areas.Admin.Models.ArticleComment;
using API.Areas.Admin.Models.ArticlesProduct;
using API.Areas.Admin.Models.CategoriesArticles;
using API.Areas.Admin.Models.CategoriesArticlesProduct;
using API.Areas.Admin.Models.CategoriesProducts;
using API.Areas.Admin.Models.DMCoQuan;
using API.Areas.Admin.Models.Manufacturer;
using API.Areas.Admin.Models.Products;
using API.Areas.Admin.Models.USUsers;
using API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace API.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class ArticlesProductController : Controller
	{
		private string controllerName = "ArticlesProductController";
		private string controllerSecret;
		private IConfiguration Configuration;
		public ArticlesProductController(IConfiguration config)
		{
			controllerSecret = config["Security:SecretId"] + controllerName;
			Configuration = config;
		}

		public IActionResult Index([FromQuery] SearchArticlesProduct dto)
		{
			// Trường hợp tìm kiếm đi theo cơ sở sản xuất?????????
			int TotalItems = 0;
			int IdProduct = Int32.Parse(MyModels.Decode(dto.Ids, controllerSecret + HttpContext.Request.Headers["UserName"]).ToString());
			dto.IdCoQuan = int.Parse(HttpContext.Request.Headers["IdCoQuan"]);

			ArticlesProductModel data = new ArticlesProductModel() { SearchData = dto };

			data.ListItems = ArticlesProductService.GetListPagination(data.SearchData, IdProduct, dto.IdManufacturer, controllerSecret + HttpContext.Request.Headers["UserName"]);
			data.ListItemsStatus = ArticlesProductService.GetListItemsStatus();
			data.ListItemCategoriesAP = CategoriesArticlesProductService.GetListItemsCAP(false);

			if (data.ListItems != null && data.ListItems.Count() > 0)
			{
				TotalItems = data.ListItems[0].TotalRows;
			}

			HttpContext.Session.SetString("STR_Action_Link_" + controllerName, Request.QueryString.ToString());
			data.Pagination = new Models.Partial.PartialPagination() { CurrentPage = data.SearchData.CurrentPage, ItemsPerPage = data.SearchData.ItemsPerPage, TotalItems = TotalItems, QueryString = Request.QueryString.ToString() };

			return View(data);
		}

		public IActionResult SaveItem(string Id = null, string IdsProduct = null)
		{
			int IdProduct = Int32.Parse(MyModels.Decode(IdsProduct, controllerSecret + HttpContext.Request.Headers["UserName"]).ToString());
			int IdDC = Int32.Parse(MyModels.Decode(Id, controllerSecret + HttpContext.Request.Headers["UserName"]).ToString());

			ArticlesProductModel data = new ArticlesProductModel();
			ProductsModel productData = new ProductsModel();
			ArticlesProduct Item = new ArticlesProduct() { PublishUp = DateTime.Now, PublishUpShow = DateTime.Now.ToString("dd/MM/yyyy") };

			data.SearchData = new SearchArticlesProduct() { CurrentPage = 0, ItemsPerPage = 10, Keyword = "" };
			data.ListItemCategoriesAP = CategoriesArticlesProductService.GetListItemsCAP(true);
			data.ListItemsManufacturer = ManufacturerService.GetListSelectItem();
			data.ListItemCategoriesProduct = CategoriesProductsService.GetSelectListItems(true);

			// Get Data Item Product
			productData.Item = ProductsService.GetItem(IdProduct, this.controllerSecret + HttpContext.Request.Headers["UserName"].ToString());

			// IdProduct không tồn tại quay về trang Danh sách Sản phẩm
			if (IdProduct <= 0 && IdDC <= 0)
			{
				data.Item = new ArticlesProduct();
				return View(data);
				//return RedirectToAction("Index", "Products");
			}
			else if (IdDC > 0) // Get Detail Article Product
			{
				Item = ArticlesProductService.GetItem(IdDC, controllerSecret + HttpContext.Request.Headers["UserName"]);
			}

			if (productData.Item != null)
			{
				Item.ManufacturerName = productData.Item.TitleManufacturer;
				Item.CatProductName = productData.Item.CatTitle;
				Item.IdManufacturer = productData.Item.IdManufacturer;
				Item.IdCatProduct = productData.Item.CatId;
			}

			if (IdProduct > 0)
			{
				Item.IdProduct = IdProduct;
				Item.IdCatAP = 1;
			}

			data.Item = Item;
			return View(data);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> SaveItem(ArticlesProduct data)
		{
			var Login = HttpContext.Session.GetString("Login");
			USUsers MyInfo = JsonConvert.DeserializeObject<USUsers>(Login);
			int IdGroup = Int32.Parse(HttpContext.Session.GetString("IdGroup"));
			int ParentIdCat = 0;
			if (IdGroup == 6)
			{
				ParentIdCat = Int32.Parse(Configuration["IdCatCompat"]);
			}
			ArticlesProductModel model = new ArticlesProductModel() { Item = data };

			int IdDC = Int32.Parse(MyModels.Decode(model.Item.Ids, controllerSecret + HttpContext.Request.Headers["UserName"]).ToString());

			if (ModelState.IsValid)
			{
				if (data.Alias == null || data.Alias == "")
				{
					model.Item.Alias = API.Models.MyHelper.StringHelper.UrlFriendly(data.Title);
				}

				if (model.Item.Id == IdDC)
				{
					model.Item.CreatedBy = int.Parse(HttpContext.Request.Headers["Id"]);
					model.Item.ModifiedBy = int.Parse(HttpContext.Request.Headers["Id"]);
					model.Item.IdCoQuan = int.Parse(HttpContext.Request.Headers["IdCoQuan"]);

					if (MyInfo.IdGroup == 3)
					{
						ArticlesProduct ItemRoot = ArticlesProductService.GetItem(IdDC, controllerSecret + HttpContext.Request.Headers["UserName"]);
						if (ItemRoot != null)
						{
							if (ItemRoot.Status == false)
							{
								model.Item.Status = false;
							}
						}
						else
						{
							model.Item.Status = false;
						}

					}
					try
					{
						ArticlesProductService.SaveItem(model.Item);
						TempData["MessageSuccess"] = "Cập nhật thành công";
						string Str_Url = HttpContext.Session.GetString("STR_Action_Link_" + controllerName);
						if (Str_Url != null && Str_Url != "")
						{
							return Redirect("/Admin/ArticlesProduct/Index" + Str_Url);
						}
						else
						{
							return RedirectToAction("Index");
						}
					}
					catch
					{

					}
				}
			}

			model.SearchData = new SearchArticlesProduct() { CurrentPage = 0, ItemsPerPage = 10, Keyword = "" };
			model.ListItemsDanhMuc = CategoriesArticlesService.GetListItems(true, ParentIdCat);
			model.ListItemsAuthors = API.Areas.Admin.Models.USUsers.USUsersService.GetListItemsAuthor(4, MyInfo.IdCoQuan);
			model.ListCategoryType = ArticlesProductService.GetListCategoryType();
			model.ListLevelArticlesProduct = ArticlesProductService.GetListLevelArticle();
			model.ListItemCategoriesAP = CategoriesArticlesProductService.GetListItemsCAP(true);
			model.ListItemsManufacturer = ManufacturerService.GetListSelectItems();
			model.ListItemCategoriesProduct = CategoriesProductsService.GetSelectListItems(true);

			return View(model);
		}

		public IActionResult ViewLog(string Id = null)
		{
			ArticlesProductModel data = new ArticlesProductModel() { };
			int IdDC = Int32.Parse(MyModels.Decode(Id, controllerSecret + HttpContext.Request.Headers["UserName"]).ToString());
			data.ListItems = ArticlesProductService.GetListLogArticlesProduct(IdDC, controllerSecret + HttpContext.Request.Headers["UserName"]);
			return View(data);
		}

		[HttpPost]
		public async Task<dynamic> TaoFileBaoCao([FromBody] SearchArticlesProduct dto)
		{
			try
			{
				dto.ItemsPerPage = 1000;
				string ThongTinCoQuan = HttpContext.Session.GetString("ThongTinCoQuan");
				DMCoQuan ItemCoQuan = JsonConvert.DeserializeObject<DMCoQuan>(ThongTinCoQuan);
				dto.IdCoQuan = int.Parse(HttpContext.Request.Headers["IdCoQuan"]);
				ArticlesProductModel data = new ArticlesProductModel() { SearchData = dto };
				return new API.Models.MsgSuccess() { Data = await ArticlesProductService.TaoFileBaoCao(data.SearchData, false, ItemCoQuan) };
			}
			catch (Exception e)
			{
				return new API.Models.MsgError() { Msg = e.Message };
			}
		}

		public IActionResult GetItemLogArticle(string Id = null)
		{
			ArticlesProduct Item = new ArticlesProduct();

			int IdDC = Int32.Parse(MyModels.Decode(Id, controllerSecret + HttpContext.Request.Headers["UserName"]).ToString());
			if (IdDC > 0)
			{
				Item = ArticlesProductService.GetItemLogArticle(IdDC, controllerSecret + HttpContext.Request.Headers["UserName"]);
			}
			return Json(Item);
		}

		public IActionResult GetItem(string Id = null)
		{
			ArticlesProduct Item = new ArticlesProduct();

			int IdDC = Int32.Parse(MyModels.Decode(Id, controllerSecret + HttpContext.Request.Headers["UserName"]).ToString());
			if (IdDC > 0)
			{
				Item = ArticlesProductService.GetItem(IdDC, controllerSecret + HttpContext.Request.Headers["UserName"]);
			}
			return Json(Item);
		}

		[ValidateAntiForgeryToken]
		public ActionResult DeleteItem(string Id)
		{
			ArticlesProduct model = new ArticlesProduct() { Id = Int32.Parse(MyModels.Decode(Id, controllerSecret + HttpContext.Request.Headers["UserName"]).ToString()) };
			try
			{
				if (model.Id > 0)
				{
					model.CreatedBy = int.Parse(HttpContext.Request.Headers["Id"]);
					model.ModifiedBy = int.Parse(HttpContext.Request.Headers["Id"]);
					ArticlesProductService.DeleteItem(model);
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
			ArticlesProduct item = new ArticlesProduct() { Id = Int32.Parse(MyModels.Decode(Ids, controllerSecret + HttpContext.Request.Headers["UserName"]).ToString()), Status = Status };
			try
			{
				if (item.Id > 0)
				{
					item.CreatedBy = int.Parse(HttpContext.Request.Headers["Id"]);
					item.ModifiedBy = int.Parse(HttpContext.Request.Headers["Id"]);

					var Login = HttpContext.Session.GetString("Login");
					USUsers MyInfo = JsonConvert.DeserializeObject<USUsers>(Login);

					if (MyInfo.IdGroup == 3)
					{
						ArticlesProduct ItemRoot = ArticlesProductService.GetItem(item.Id, controllerSecret + HttpContext.Request.Headers["UserName"]);
						if (ItemRoot.Status == false)
						{
							item.Status = false;
						}
					}

					dynamic UpdateStatus = ArticlesProductService.UpdateStatus(item);
					if (item.Status == true)
					{
						TempData["MessageSuccess"] = "Cập nhật trạng thái hiển thị thành công";
					}
					else
					{
						TempData["MessageSuccess"] = "Cập nhật trạng thái ẩn thành công";
					}

					return Json(new MsgSuccess());
				}
				else
				{
					TempData["MessageError"] = "Cập nhật trạng thái hiển thị không thành công";
					return Json(new MsgError());
				}
			}
			catch
			{
				TempData["MessageSuccess"] = "Cập nhật trạng thái ẩn không thành công";
				return Json(new MsgError());
			}
		}

		[ValidateAntiForgeryToken]
		public ActionResult CreateAudio([FromQuery] string Ids)
		{
			int IdDC = Int32.Parse(MyModels.Decode(Ids, controllerSecret + HttpContext.Request.Headers["UserName"]).ToString());
			string ThongTinCoQuan = HttpContext.Session.GetString("ThongTinCoQuan");
			DMCoQuan ItemCoQuan = JsonConvert.DeserializeObject<DMCoQuan>(ThongTinCoQuan);
			ArticlesProduct Item = ArticlesProductService.GetItem(IdDC);
			try
			{
				if (Item.Id > 0)
				{
					string Fu = API.Models.MyHelper.StringHelper.RemoveHtmlTags(Item.FullText).Replace("\r", "").Trim();
					Item.FullText = HttpUtility.HtmlDecode(Fu);

					//Item.FileItem  = API.Models.MyHelper.SmartVoiceService.GetFileVoice(ItemCoQuan, Item);

					dynamic UpdateFeatured = ArticlesProductService.UpdateFileAudio(Item.Id, Item.FileItem);
					TempData["MessageSuccess"] = "Tạo file âm thanh thành công";
					return Json(new MsgSuccess());
				}
				else
				{
					TempData["MessageError"] = "Cập nhật file âm thanh thất bại. Xin vui lòng tạo lại";
					return Json(new MsgError());
				}
			}
			catch
			{
				TempData["MessageSuccess"] = "Cập nhật Featured không thành công";
				return Json(new MsgError());
			}
		}

		public IActionResult ViewComment(string Id = null)
		{
			ArticleCommentModel data = new ArticleCommentModel() { };
			int IdDC = Int32.Parse(MyModels.Decode(Id, controllerSecret + HttpContext.Request.Headers["UserName"]).ToString());

			if (IdDC > 0)
			{
				data.ArticlesProductItem = ArticlesProductService.GetItem(IdDC, controllerSecret + HttpContext.Request.Headers["UserName"]);

			}
			else
			{
				data.ArticlesProductItem = new ArticlesProduct() { Title = "Quản lý bình luận", Id = 0 };
			}

			//data.ListItems = ArticlesProductService.GetListLogArticlesProduct(IdDC, controllerSecret + HttpContext.Request.Headers["UserName"]);
			return View(data);
		}

		[HttpPost]
		public IActionResult GetListComment([FromBody] SearchArticleComment dto)
		{
			ArticleCommentModel data = new ArticleCommentModel() { };
			int IdDC = Int32.Parse(MyModels.Decode(dto.ArticleIds, controllerSecret + HttpContext.Request.Headers["UserName"]).ToString());
			List<ArticleComment> ListItems = ArticleCommentService.GetListPagination(dto, controllerSecret + HttpContext.Request.Headers["UserName"]);
			return Json(new MsgSuccess() { Data = ListItems });
		}

		[HttpPost]
		public IActionResult EditComment([FromBody] ArticleComment dto)
		{
			ArticleCommentModel data = new ArticleCommentModel() { };
			int IdDC = Int32.Parse(MyModels.Decode(dto.Ids, controllerSecret + HttpContext.Request.Headers["UserName"]).ToString());
			ArticleComment Item = new ArticleComment();
			if (IdDC > 0)
			{
				Item = ArticleCommentService.GetItem(IdDC, controllerSecret + HttpContext.Request.Headers["UserName"]);

				if (Item != null)
				{
					ArticleCommentService.SaveItem(dto);
					return Json(new MsgSuccess() { Data = Item });
				}
			}

			return Json(new MsgError() { Data = Item });
		}

		[HttpPost]
		public IActionResult DeletedComment([FromBody] ArticleComment dto)
		{
			ArticleCommentModel data = new ArticleCommentModel() { };
			int IdDC = Int32.Parse(MyModels.Decode(dto.Ids, controllerSecret + HttpContext.Request.Headers["UserName"]).ToString());
			ArticleComment Item = new ArticleComment();

			if (IdDC > 0)
			{
				ArticleCommentService.DeleteItem(dto);
				return Json(new MsgSuccess() { Data = Item });
			}

			return Json(new MsgError() { Data = Item });
		}

		[HttpPost]
		public IActionResult UpdateStatusComment([FromBody] ArticleComment dto)
		{
			ArticleCommentModel data = new ArticleCommentModel() { };
			int IdDC = Int32.Parse(MyModels.Decode(dto.Ids, controllerSecret + HttpContext.Request.Headers["UserName"]).ToString());
			ArticleComment Item = new ArticleComment();
			if (IdDC > 0)
			{
				Item = ArticleCommentService.GetItem(IdDC, controllerSecret + HttpContext.Request.Headers["UserName"]);
				if (Item != null)
				{
					Item.Status = !Item.Status;
					ArticleCommentService.UpdateStatus(Item);
					return Json(new MsgSuccess() { Data = Item, Msg = "Cập nhật trạng thái Bình luận thành công" });
				}
			}

			return Json(new MsgError() { Data = Item, Msg = "Cập nhật trạng thái Bình luận Thất bại. Xin vui lòng cập nhật lại" });
		}
	}
}
