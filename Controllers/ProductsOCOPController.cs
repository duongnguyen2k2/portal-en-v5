using API.Areas.Admin.Models.CategoriesProducts;
using API.Areas.Admin.Models.DMCoQuan;
using API.Areas.Admin.Models.Orders;
using API.Areas.Admin.Models.Products;
using API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace API.Controllers
{
	public class ProductsOCOPController : Controller
	{
		public IActionResult Index(string alias, int id, [FromQuery] SearchProducts dto)
		{
			int TotalItems = 0;
			int OCOP_Status = 1;

			int IdCoQuan = 1;
			if (HttpContext.Session.GetString("IdCoQuan") != null && HttpContext.Session.GetString("IdCoQuan") != "")
			{
				IdCoQuan = int.Parse(HttpContext.Session.GetString("IdCoQuan"));
			}

			DMCoQuan ItemCoQuan = DMCoQuanService.GetItem(IdCoQuan);
			if (ItemCoQuan.CategoryId == 3)
			{
				IdCoQuan = ItemCoQuan.ParentId;
			}

			string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
			ProductsModel data = new ProductsModel() { SearchData = dto };
			data.SearchData.Status = 1;
			if (id == 0)
			{
				data.SearchData.CatId = -1;
			}
			else
			{
				data.SearchData.CatId = id;
			}

			data.CategoriesItem = CategoriesProductsService.GetItem(id);
			data.ListItems = ProductsService.GetListPagination(data.SearchData, API.Models.Settings.SecretId + ControllerName, OCOP_Status);
			data.ListItemsCategories = CategoriesProductsService.GetListItems();
			data.ListDMCoQuan = DMCoQuanService.GetListByLoaiCoQuan(0, 0, IdCoQuan, false);

			if (data.ListItems != null && data.ListItems.Count() > 0)
			{
				TotalItems = data.ListItems[0].TotalRows;
			}
			data.Pagination = new Areas.Admin.Models.Partial.PartialPagination() { CurrentPage = data.SearchData.CurrentPage, ItemsPerPage = data.SearchData.ItemsPerPage, TotalItems = TotalItems, QueryString = Request.QueryString.ToString() };

			return View(data);
		}

		public IActionResult CartList()
		{
			ProductsModel data = new ProductsModel() { SearchData = new SearchProducts(), Cart = new Cart() { Total = 0, Amount = 0 } };
			string str = HttpContext.Session.GetString("ListCart");
			List<Products> ListCart = new List<Products>();
			if (str != null && str != "")
			{
				ListCart = JsonConvert.DeserializeObject<List<Products>>(str);
			}
			data.ListItems = ListCart;

			return View(data);
		}

		public IActionResult UpdateCart([FromQuery] string Ids = null, int Quantity = 0, int Flag = 1)
		{
			int TotalProduct = 0;
			string str = HttpContext.Session.GetString("ListCart");
			List<Products> ListCart = new List<Products>();
			List<Products> ListCartNew = new List<Products>();
			if (str != null && str != "")
			{
				ListCart = JsonConvert.DeserializeObject<List<Products>>(str);
			}
			string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
			int IdDC = Int32.Parse(MyModels.Decode(Ids, API.Models.Settings.SecretId + ControllerName).ToString());
			if (IdDC > 0)
			{
				for (int i = 0; i < ListCart.Count(); i++)
				{
					if (ListCart[i].Id == IdDC)
					{
						ListCart[i].Amounts = Quantity;
					}
					if (ListCart[i].Amounts > 0)
					{
						ListCartNew.Add(ListCart[i]);
					}
					TotalProduct = TotalProduct + ListCart[i].Amounts;
				}
				HttpContext.Session.SetString("ListCart", JsonConvert.SerializeObject(ListCartNew));
				HttpContext.Session.SetString("TotalCart", TotalProduct.ToString());

				if (Flag == 1)
				{
					return Json(new MsgSuccess() { Msg = "Cập nhật sản phẩm thành công", Data = TotalProduct });
				}
				else
				{
					return Redirect("/gio-hang.html");
				}
			}
			else
			{
				if (Flag == 1)
				{
					return Json(new MsgError() { Msg = "Sản phẩm đã bị khóa hoặc không tồn tại" });
				}
				else
				{
					return Redirect("/gio-hang.html");
				}
			}

		}

		public IActionResult AddCart([FromQuery] string Ids = null, int Quantity = 0, int Flag = 1)
		{
			// Flag = 1; Adcart ajax; Flag=2; 
			int TotalProduct = 0;
			string str = HttpContext.Session.GetString("ListCart");
			List<Products> ListCart = new List<Products>();
			string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
			int IdDC = Int32.Parse(MyModels.Decode(Ids, API.Models.Settings.SecretId + ControllerName).ToString());

			if (str != null && str != "")
			{
				ListCart = JsonConvert.DeserializeObject<List<Products>>(str);
			}

			if (IdDC > 0)
			{
				Products Item = ProductsService.GetItem(IdDC, API.Models.Settings.SecretId + ControllerName);
				if (Item != null && Item.Id > 0)
				{
					Boolean flagIsset = false;
					for (int i = 0; i < ListCart.Count(); i++)
					{
						if (ListCart[i].Id == Item.Id)
						{
							flagIsset = true;
							ListCart[i].Amounts = ListCart[i].Amounts + Quantity;
						}
						TotalProduct = TotalProduct + ListCart[i].Amounts;
					}
					Item.Amounts = Item.Amounts + Quantity;
					if (!flagIsset)
					{
						ListCart.Add(Item);
						TotalProduct = TotalProduct + Item.Amounts;
					}
				}

				HttpContext.Session.SetString("ListCart", JsonConvert.SerializeObject(ListCart));
				HttpContext.Session.SetString("TotalCart", TotalProduct.ToString());
				if (Flag == 1)
				{
					return Json(new MsgSuccess() { Msg = "Thêm sản phẩm thành công", Data = TotalProduct });
				}
				else
				{
					return Redirect("/gio-hang.html");
				}
			}

			if (Flag == 1)
			{
				return Json(new MsgError() { Msg = "Sản phẩm đã bị khóa hoặc không tồn tại" });
			}
			else
			{
				return Redirect("/gio-hang.html");
			}
		}

		[HttpPost]
		public IActionResult AddCart([FromForm] Products dto)
		{
			// Flag = 1; Adcart ajax; Flag=2; 
			int TotalProduct = 0;
			string str = HttpContext.Session.GetString("ListCart");
			List<Products> ListCart = new List<Products>();
			string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
			int IdDC = Int32.Parse(MyModels.Decode(dto.Ids, API.Models.Settings.SecretId + ControllerName).ToString());

			if (str != null && str != "")
			{
				ListCart = JsonConvert.DeserializeObject<List<Products>>(str);
			}

			if (IdDC > 0)
			{
				Products Item = ProductsService.GetItem(IdDC, API.Models.Settings.SecretId + ControllerName);
				if (Item != null && Item.Id > 0)
				{
					Boolean flagIsset = false;
					for (int i = 0; i < ListCart.Count(); i++)
					{
						if (ListCart[i].Id == Item.Id)
						{
							flagIsset = true;
							ListCart[i].Amounts = ListCart[i].Amounts + dto.Quantity;
						}
						TotalProduct = TotalProduct + ListCart[i].Amounts;
					}
					Item.Amounts = Item.Amounts + dto.Quantity;
					if (!flagIsset)
					{
						ListCart.Add(Item);
						TotalProduct = TotalProduct + Item.Amounts;
					}
				}

				HttpContext.Session.SetString("ListCart", JsonConvert.SerializeObject(ListCart));
				HttpContext.Session.SetString("TotalCart", TotalProduct.ToString());

				return Redirect("/gio-hang.html");
			}

			return Redirect("/gio-hang.html");
		}

		public IActionResult CheckOut(string alias, int id)
		{
			OrdersModel data = new OrdersModel() { Item = new Orders() { } };
			string str = HttpContext.Session.GetString("ListCart");
			List<Products> ListCart = new List<Products>();
			if (str != null && str != "")
			{
				ListCart = JsonConvert.DeserializeObject<List<Products>>(str);
			}
			data.ListCart = ListCart;
			if (data.ListCart == null || data.ListCart.Count() == 0)
			{
				return Redirect("/gio-hang.html");
			}

			return View(data);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult CheckOut(Orders dto)
		{
			OrdersModel data = new OrdersModel() { Item = dto };
			string str = HttpContext.Session.GetString("ListCart");
			List<Products> ListCart = new List<Products>();
			if (str != null && str != "")
			{
				ListCart = JsonConvert.DeserializeObject<List<Products>>(str);
			}

			data.ListCart = ListCart;

			if (ModelState.IsValid)
			{
				if (data.ListCart != null || data.ListCart.Count() > 0)
				{
					DataTable tbItem = new DataTable();
					tbItem.Columns.Add("OrderId", typeof(int));
					tbItem.Columns.Add("ProductId", typeof(int));
					tbItem.Columns.Add("Price", typeof(float));
					tbItem.Columns.Add("Quantity", typeof(int));
					tbItem.Columns.Add("Total", typeof(float));
					tbItem.Columns.Add("TotalDisplay", typeof(string));

					for (int i = 0; i < data.ListCart.Count(); i++)
					{
						var row = tbItem.NewRow();
						row["OrderId"] = 0;
						row["ProductId"] = data.ListCart[i].Id;
						row["Price"] = data.ListCart[i].Price;
						row["Quantity"] = data.ListCart[i].Amounts;
						row["Total"] = data.ListCart[i].Amounts * data.ListCart[i].Price;
						row["TotalDisplay"] = API.Models.MyHelper.StringHelper.ConvertNumberToDisplay((data.ListCart[i].Amounts * data.ListCart[i].Price).ToString());
						tbItem.Rows.Add(row);
					}
					dynamic DataSave = OrdersService.SaveItem(data.Item, tbItem);

					return Redirect("/");
				}
			}
			else
			{

			}

			return View(data);
		}
		public IActionResult Detail(string alias, int id)
		{
			ProductsModel data = new ProductsModel();
			string ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
			data.SearchData = new SearchProducts() { CurrentPage = 0, ItemsPerPage = 10, Keyword = "" };
			data.Item = ProductsService.GetItem(id, API.Models.Settings.SecretId + ControllerName);
			data.Item.Amounts = 1;
			data.ListItemsCategories = CategoriesProductsService.GetListItems();

			return View(data);
		}
	}
}