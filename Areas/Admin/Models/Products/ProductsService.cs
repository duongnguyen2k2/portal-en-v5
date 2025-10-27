using API.Models;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;

namespace API.Areas.Admin.Models.Products
{
	public class ProductsService
	{
		public static List<Products> GetListPagination(SearchProducts dto, string SecretId, int OCOP)
		{
			dto.CurrentPage = dto.CurrentPage <= 0 ? 1 : dto.CurrentPage;
			dto.ItemsPerPage = dto.ItemsPerPage <= 0 ? 20 : dto.ItemsPerPage;
			dto.Keyword = dto.Keyword == null ? "" : dto.Keyword;

			string str_sql = "GetListPagination_Status";
			Boolean Status = true;
			if (dto.Status == -1)
			{
				str_sql = "GetListPagination";
			}
			else if (dto.Status == 0)
			{
				Status = false;
			}
			var tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Products",
				new string[] { "@flag", "@CurrentPage", "@ItemsPerPage", "@Keyword", "@CatId", "@IdManufacturer", "@Status", "@IdUser", "@IdCoQuan", "@OCOP" },
				new object[] { str_sql, dto.CurrentPage, dto.ItemsPerPage, dto.Keyword, dto.CatId, dto.IdManufacturer, Status, dto.AuthorId, dto.IdCoQuan, OCOP });
			if (tabl == null)
			{
				return new List<Products>();
			}
			else
			{
				return (from r in tabl.AsEnumerable()
						select new Products
						{
							Id = (int)r["Id"],
							Title = (string)r["Title"],
							Status = (bool)r["Status"],
							Featured = (bool)r["Featured"],
							Introtext = (string)((r["Introtext"] == System.DBNull.Value) ? "" : r["Introtext"]),
							CatTitle = (string)((r["CatTitle"] == System.DBNull.Value) ? "" : r["CatTitle"]),
							TitleManufacturer = (string)((r["TitleManufacturer"] == System.DBNull.Value) ? "" : r["TitleManufacturer"]),
							AddressManufacturer = (string)((r["AddressManufacturer"] == System.DBNull.Value) ? "" : r["AddressManufacturer"]),
							ContactManufacturer = (string)((r["ContactManufacturer"] == System.DBNull.Value) ? "" : r["ContactManufacturer"]),
							Alias = (string)((r["Alias"] == System.DBNull.Value) ? "" : r["Alias"]),
							Image = (string)((r["Image"] == System.DBNull.Value) ? "" : r["Image"]),
							Sku = (string)((r["Sku"] == System.DBNull.Value) ? "" : r["Sku"]),
							Price = (double)((r["Price"] == System.DBNull.Value) ? Double.Parse("0") : r["Price"]),
							PriceDisplay = (string)((r["PriceDisplay"] == System.DBNull.Value) ? "0" : r["PriceDisplay"]),
							PriceDealDisplay = (string)((r["PriceDealDisplay"] == System.DBNull.Value) ? "0" : r["PriceDealDisplay"]),
							TenCoQuan = (string)((r["TenCoQuan"] == System.DBNull.Value) ? "" : r["TenCoQuan"]),
							PriceDeal = (double)((r["PriceDeal"] == System.DBNull.Value) ? Double.Parse("0") : r["PriceDeal"]),
							Discount = (double)((r["Discount"] == System.DBNull.Value) ? Double.Parse("0") : r["Discount"]),
							Quantity = (int)((r["Quantity"] == System.DBNull.Value) ? 0 : r["Quantity"]),
							PerCat = (int)((r["PerCat"] == System.DBNull.Value) ? 0 : r["PerCat"]),
							Sold = (int)((r["Sold"] == System.DBNull.Value) ? 0 : r["Sold"]),
							IdCoQuan = (int)((r["IdCoQuan"] == System.DBNull.Value) ? 0 : r["IdCoQuan"]),
							Star = (int)((r["Star"] == System.DBNull.Value) ? 3 : r["Star"]),
							Available = (int)((r["Available"] == System.DBNull.Value) ? 0 : r["Available"]),
							Ids = MyModels.Encode((int)r["Id"], SecretId),
							TotalRows = (int)r["TotalRows"],
						}).ToList();
			}
		}

		public static List<Products> GetListAllProductByCat(string SecretId = "", Boolean Featured = false)
		{
			string sql = "GetListAllProductByCat";
			if (Featured)
			{
				sql = "GetListAllProductByCatFeature";
			}
			DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Products",
			new string[] { "@flag" }, new object[] { sql });
			if (tabl == null)
			{
				return new List<Products>();
			}
			else
			{
				return (from r in tabl.AsEnumerable()
						select new Products
						{
							Id = (int)r["Id"],
							Title = (string)r["Title"],
							Status = (bool)r["Status"],
							Introtext = (string)((r["Introtext"] == System.DBNull.Value) ? "" : r["Introtext"]),
							Alias = (string)((r["Alias"] == System.DBNull.Value) ? "" : r["Alias"]),
							Image = (string)((r["Image"] == System.DBNull.Value) ? "" : r["Image"]),
							Sku = (string)((r["Sku"] == System.DBNull.Value) ? "" : r["Sku"]),
							Price = (double)((r["Price"] == System.DBNull.Value) ? Double.Parse("0") : r["Price"]),
							PriceDisplay = (string)((r["PriceDisplay"] == System.DBNull.Value) ? "0" : r["PriceDisplay"]),
							PriceDealDisplay = (string)((r["PriceDealDisplay"] == System.DBNull.Value) ? "0" : r["PriceDealDisplay"]),
							PriceDeal = (double)((r["PriceDeal"] == System.DBNull.Value) ? Double.Parse("0") : r["PriceDeal"]),
							Discount = (double)((r["Discount"] == System.DBNull.Value) ? Double.Parse("0") : r["Discount"]),
							Quantity = (int)((r["Quantity"] == System.DBNull.Value) ? 0 : r["Quantity"]),
							CatId = (int)((r["CatId"] == System.DBNull.Value) ? 0 : r["CatId"]),
							Sold = (int)((r["Sold"] == System.DBNull.Value) ? 0 : r["Sold"]),
							Star = (int)((r["Star"] == System.DBNull.Value) ? 3 : r["Star"]),
							Available = (int)((r["Available"] == System.DBNull.Value) ? 0 : r["Available"]),
							Ids = MyModels.Encode((int)r["Id"], SecretId),
						}).ToList();
			}
		}

		public static List<Products> GetListByManufacturer(SearchProducts dto, string SecretId, int OCOP_Product, Boolean MobileCheck)
		{
			string sql = "GetListByManufacturer";
			if (MobileCheck)
			{
				sql = "GetListByManufacturerMobile";
			}
			DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Products",
			new string[] { "@flag", "@CatId", "@IdManufacturer", "@Status", "@IdCoQuan", "@OCOP" },
			new object[] { sql, dto.CatId, dto.IdManufacturer, dto.Status, dto.IdCoQuan, OCOP_Product });
			if (tabl == null)
			{
				return new List<Products>();
			}
			else
			{
				List<Products> list = (from r in tabl.AsEnumerable()
									   select new Products
									   {
										   Id = (int)r["Id"],
										   Title = (string)r["Title"],
										   Status = (bool)r["Status"],
										   Introtext = (string)((r["Introtext"] == System.DBNull.Value) ? "" : r["Introtext"]),
										   Description = (string)((r["Description"] == System.DBNull.Value) ? "" : r["Description"]),
										   Str_ListImage = (string)((r["Images"] == System.DBNull.Value) ? "" : r["Images"]),
										   Alias = (string)((r["Alias"] == System.DBNull.Value) ? "" : r["Alias"]),
										   Image = (string)((r["Image"] == System.DBNull.Value) ? "" : r["Image"]),
										   Sku = (string)((r["Sku"] == System.DBNull.Value) ? "" : r["Sku"]),
										   Price = (double)((r["Price"] == System.DBNull.Value) ? Double.Parse("0") : r["Price"]),
										   PriceDisplay = (string)((r["PriceDisplay"] == System.DBNull.Value) ? "0" : r["PriceDisplay"]),
										   PriceDealDisplay = (string)((r["PriceDealDisplay"] == System.DBNull.Value) ? "0" : r["PriceDealDisplay"]),
										   PriceDeal = (double)((r["PriceDeal"] == System.DBNull.Value) ? Double.Parse("0") : r["PriceDeal"]),
										   Discount = (double)((r["Discount"] == System.DBNull.Value) ? Double.Parse("0") : r["Discount"]),
										   Quantity = (int)((r["Quantity"] == System.DBNull.Value) ? 0 : r["Quantity"]),
										   CatId = (int)((r["CatId"] == System.DBNull.Value) ? 0 : r["CatId"]),
										   Ids = MyModels.Encode((int)r["Id"], SecretId),
									   }).ToList();


				foreach (Products item in list)
				{
					if (item != null)
					{
						if (item.Str_ListImage != null && item.Str_ListImage != "")
						{
							item.ListImage = JsonConvert.DeserializeObject<List<ImageFile>>(item.Str_ListImage);
							item.ImageCount = item.ListImage.Count;
						}
					}
				}

				return list;
			}


		}

		public static List<int> GetListIdByManufacturer(int IdManufacturer)
		{
			List<int> result = new List<int>();

			var tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Products",
				new string[] { "@flag", "@IdManufacturer" },
				new object[] { "GetListIdByManufacturer", IdManufacturer });

			if (tabl != null)
			{
				foreach (DataRow item in tabl.Rows)
				{
					result.Add((int)item["IdProduct"]);
				}
			}
			return result;
		}

        public static List<Products> GetListFeature(int Limit = 5, string SecretId = "")
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Products",
            new string[] { "@flag", "@ItemsPerPage" }, new object[] { "GetListFeatured", Limit });
            if (tabl == null)
            {
                return new List<Products>();
            }
            else
            {
                return (from r in tabl.AsEnumerable()
                        select new Products
                        {
                            Id = (int)r["Id"],
                            Title = (string)r["Title"],
                            Status = (bool)r["Status"],
                            Introtext = (string)((r["Introtext"] == System.DBNull.Value) ? "" : r["Introtext"]),
                            TitleManufacturer = (string)((r["TitleManufacturer"] == System.DBNull.Value) ? "" : r["TitleManufacturer"]),
                            AddressManufacturer = (string)((r["AddressManufacturer"] == System.DBNull.Value) ? "" : r["AddressManufacturer"]),
                            ContactManufacturer = (string)((r["ContactManufacturer"] == System.DBNull.Value) ? "" : r["ContactManufacturer"]),
                            Alias = (string)((r["Alias"] == System.DBNull.Value) ? "" : r["Alias"]),
                            Image = (string)((r["Image"] == System.DBNull.Value) ? "" : r["Image"]),
                            Sku = (string)((r["Sku"] == System.DBNull.Value) ? "" : r["Sku"]),
                            Price = (double)((r["Price"] == System.DBNull.Value) ? Double.Parse("0") : r["Price"]),
                            PriceDisplay = (string)((r["PriceDisplay"] == System.DBNull.Value) ? "0" : r["PriceDisplay"]),
                            PriceDealDisplay = (string)((r["PriceDealDisplay"] == System.DBNull.Value) ? "0" : r["PriceDealDisplay"]),
                            PriceDeal = (double)((r["PriceDeal"] == System.DBNull.Value) ? Double.Parse("0") : r["PriceDeal"]),
                            Discount = (double)((r["Discount"] == System.DBNull.Value) ? Double.Parse("0") : r["Discount"]),
                            Quantity = (int)((r["Quantity"] == System.DBNull.Value) ? 0 : r["Quantity"]),
                            Sold = (int)((r["Sold"] == System.DBNull.Value) ? 0 : r["Sold"]),
                            Available = (int)((r["Available"] == System.DBNull.Value) ? 0 : r["Available"]),
                            Star = (int)((r["Star"] == System.DBNull.Value) ? 3 : r["Star"]),
                            Ids = MyModels.Encode((int)r["Id"], SecretId),
                        }).ToList();
            }
        }

        public static List<Products> GetListNew(int CatId = 0, int Limit = 5, string SecretId = "")
		{
			DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Products",
			new string[] { "@flag", "@ItemsPerPage" }, new object[] { "GetListNew", CatId });
			if (tabl == null)
			{
				return new List<Products>();
			}
			else
			{
				return (from r in tabl.AsEnumerable()
						select new Products
						{
							Id = (int)r["Id"],
							Title = (string)r["Title"],
							Status = (bool)r["Status"],
							Introtext = (string)((r["Introtext"] == System.DBNull.Value) ? "" : r["Introtext"]),
							TitleManufacturer = (string)((r["TitleManufacturer"] == System.DBNull.Value) ? "" : r["TitleManufacturer"]),
							AddressManufacturer = (string)((r["AddressManufacturer"] == System.DBNull.Value) ? "" : r["AddressManufacturer"]),
							ContactManufacturer = (string)((r["ContactManufacturer"] == System.DBNull.Value) ? "" : r["ContactManufacturer"]),
							Alias = (string)((r["Alias"] == System.DBNull.Value) ? "" : r["Alias"]),
							Image = (string)((r["Image"] == System.DBNull.Value) ? "" : r["Image"]),
							Sku = (string)((r["Sku"] == System.DBNull.Value) ? "" : r["Sku"]),
							Price = (double)((r["Price"] == System.DBNull.Value) ? Double.Parse("0") : r["Price"]),
							PriceDisplay = (string)((r["PriceDisplay"] == System.DBNull.Value) ? "0" : r["PriceDisplay"]),
							PriceDealDisplay = (string)((r["PriceDealDisplay"] == System.DBNull.Value) ? "0" : r["PriceDealDisplay"]),
							PriceDeal = (double)((r["PriceDeal"] == System.DBNull.Value) ? Double.Parse("0") : r["PriceDeal"]),
							Discount = (double)((r["Discount"] == System.DBNull.Value) ? Double.Parse("0") : r["Discount"]),
							Quantity = (int)((r["Quantity"] == System.DBNull.Value) ? 0 : r["Quantity"]),
							Sold = (int)((r["Sold"] == System.DBNull.Value) ? 0 : r["Sold"]),
							Available = (int)((r["Available"] == System.DBNull.Value) ? 0 : r["Available"]),
							Star = (int)((r["Star"] == System.DBNull.Value) ? 3 : r["Star"]),
							Ids = MyModels.Encode((int)r["Id"], SecretId),
						}).ToList();
			}
		}

		public static List<SelectListItem> GetListStar(Boolean Selected = true)
		{
			List<SelectListItem> ListItems = new List<SelectListItem>();
			ListItems.Insert(0, (new SelectListItem { Text = "Đánh giá Số Sao", Value = "0" }));
			for (int i = 1; i < 6; i++)
			{
				ListItems.Insert(i, (new SelectListItem { Text = i + " Sao", Value = i.ToString() }));
			}

			return ListItems;
		}

		public static List<Products> GetListOCOP(Boolean Selected = true)
		{
			DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Products",
				new string[] { "@flag", "@Selected" }, new object[] { "GetListOCOP", Selected });
			if (tabl == null)
			{
				return new List<Products>();
			}
			else
			{
				return (from r in tabl.AsEnumerable()
						select new Products
						{
							Id = (int)r["Id"],
							Title = (string)r["Title"],
							Image = (string)((r["Image"] == System.DBNull.Value) ? "" : r["Image"]),
							Sku = (string)((r["Sku"] == System.DBNull.Value) ? "" : r["Sku"]),
							Price = (double)((r["Price"] == System.DBNull.Value) ? Double.Parse("0") : r["Price"]),
							PriceDisplay = (string)((r["PriceDisplay"] == System.DBNull.Value) ? "0" : r["PriceDisplay"]),
							PriceDealDisplay = (string)((r["PriceDealDisplay"] == System.DBNull.Value) ? "0" : r["PriceDealDisplay"]),
							PriceDeal = (double)((r["PriceDeal"] == System.DBNull.Value) ? Double.Parse("0") : r["PriceDeal"]),
							Discount = (double)((r["Discount"] == System.DBNull.Value) ? Double.Parse("0") : r["Discount"]),
							Quantity = (int)((r["Quantity"] == System.DBNull.Value) ? 0 : r["Quantity"]),
							Sold = (int)((r["Sold"] == System.DBNull.Value) ? 0 : r["Sold"]),
							Available = (int)((r["Available"] == System.DBNull.Value) ? 0 : r["Available"]),
							Star = (int)((r["Star"] == System.DBNull.Value) ? 3 : r["Star"]),
						}).ToList();
			}
		}


		public static List<Products> GetList(Boolean Selected = true)
		{
			DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Products",
				new string[] { "@flag", "@Selected" }, new object[] { "GetList", Selected });
			if (tabl == null)
			{
				return new List<Products>();
			}
			else
			{
				return (from r in tabl.AsEnumerable()
						select new Products
						{
							Id = (int)r["Id"],
							Title = (string)r["Title"],
							Image = (string)((r["Image"] == System.DBNull.Value) ? "" : r["Image"]),
							Sku = (string)((r["Sku"] == System.DBNull.Value) ? "" : r["Sku"]),
							Price = (double)((r["Price"] == System.DBNull.Value) ? Double.Parse("0") : r["Price"]),
							PriceDisplay = (string)((r["PriceDisplay"] == System.DBNull.Value) ? "0" : r["PriceDisplay"]),
							PriceDealDisplay = (string)((r["PriceDealDisplay"] == System.DBNull.Value) ? "0" : r["PriceDealDisplay"]),
							PriceDeal = (double)((r["PriceDeal"] == System.DBNull.Value) ? Double.Parse("0") : r["PriceDeal"]),
							Discount = (double)((r["Discount"] == System.DBNull.Value) ? Double.Parse("0") : r["Discount"]),
							Quantity = (int)((r["Quantity"] == System.DBNull.Value) ? 0 : r["Quantity"]),
							Sold = (int)((r["Sold"] == System.DBNull.Value) ? 0 : r["Sold"]),
							Available = (int)((r["Available"] == System.DBNull.Value) ? 0 : r["Available"]),
							Star = (int)((r["Star"] == System.DBNull.Value) ? 3 : r["Star"]),
						}).ToList();
			}
		}

		public static List<int> GetListCatIdByProductId(int ProductsId)
		{

			DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Products",
				new string[] { "@flag", "@Id" }, new object[] { "GetListCatIdByProductId", ProductsId });
			if (tabl == null)
			{
				return new List<int>();
			}
			else
			{
				return tabl.AsEnumerable().Select(r => (int)r["CatId"]).ToList();
			}
		}

		public static List<SelectListItem> GetListSelectItems(Boolean Selected = true)
		{
			DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Products",
				new string[] { "@flag", "@Selected" }, new object[] { "GetList", Convert.ToDecimal(Selected) });
			List<SelectListItem> ListItems = (from r in tabl.AsEnumerable()
											  select new SelectListItem
											  {
												  Value = (string)((r["Id"] == System.DBNull.Value) ? null : r["Id"].ToString()),
												  Text = (string)((r["Title"] == System.DBNull.Value) ? "" : r["Title"]),
											  }).ToList();
			ListItems.Insert(0, (new SelectListItem { Text = "--- Chọn Sản Phẩm ---", Value = "0" }));
			return ListItems;
		}

		public static List<SelectListItem> GetListItemsNoOCOP(Boolean Selected = true)
		{
			DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Products",
				new string[] { "@flag", "@Selected" }, new object[] { "GetListItemsNoOCOP", Convert.ToDecimal(Selected) });
			List<SelectListItem> ListItems = (from r in tabl.AsEnumerable()
											  select new SelectListItem
											  {
												  Value = (string)((r["Id"] == System.DBNull.Value) ? null : r["Id"].ToString()),
												  Text = (string)((r["Title"] == System.DBNull.Value) ? "" : r["Title"]),
											  }).ToList();
			ListItems.Insert(0, (new SelectListItem { Text = "--- Chọn Sản Phẩm ---", Value = "0", Disabled = false }));
			return ListItems;
		}

		public static Products GetItem(int Id, string SecretId = null, int OCOP = 0)
		{
			string sql = "GetItem";
			if (OCOP == 1)
			{
				sql = "GetItemOCOP";
			}

			DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Products",
			new string[] { "@flag", "@Id" }, new object[] { sql, Id });
			Products Item = (from r in tabl.AsEnumerable()
							 select new Products
							 {
								 Id = (int)r["Id"],
								 Title = (string)r["Title"],
								 Alias = (string)((r["Alias"] == System.DBNull.Value) ? "" : r["Alias"]),
								 Status = (Boolean)((r["Status"] == System.DBNull.Value) ? false : r["Status"]),
								 Featured = (Boolean)((r["Featured"] == System.DBNull.Value) ? false : r["Featured"]),
								 Image = (string)((r["Image"] == System.DBNull.Value) ? "" : r["Image"]),
								 Str_ListImage = (string)((r["Images"] == System.DBNull.Value) ? "" : r["Images"]),
								 PublishUp = (DateTime)((r["PublishUp"] == System.DBNull.Value) ? DateTime.Now : r["PublishUp"]),
								 PublishUpShow = (string)((r["PublishUp"] == System.DBNull.Value) ? DateTime.Now.ToString("dd/MM/yyyy") : (string)((DateTime)r["PublishUp"]).ToString("dd/MM/yyyy")),
								 Sku = (string)((r["Sku"] == System.DBNull.Value) ? "" : r["Sku"]),
								 Price = (double)((r["Price"] == System.DBNull.Value) ? 0 : r["Price"]),
								 PriceDisplay = (string)((r["PriceDisplay"] == System.DBNull.Value) ? "0" : r["PriceDisplay"]),
								 PriceDealDisplay = (string)((r["PriceDealDisplay"] == System.DBNull.Value) ? "0" : r["PriceDealDisplay"]),
								 PriceDeal = (double)((r["PriceDeal"] == System.DBNull.Value) ? 0 : r["PriceDeal"]),
								 Discount = (double)((r["Discount"] == System.DBNull.Value) ? 0 : r["Discount"]),
								 Quantity = (int)((r["Quantity"] == System.DBNull.Value) ? 0 : r["Quantity"]),
								 //IdManufacturer = (int)((r["IdManufacturer"] == System.DBNull.Value) ? 0 : r["IdManufacturer"]),
								 Sold = (int)((r["Sold"] == System.DBNull.Value) ? 0 : r["Sold"]),
								 CatId = (int)((r["CatId"] == System.DBNull.Value) ? 0 : r["CatId"]),
								 //CatTitle = (string)((r["CatTitle"] == System.DBNull.Value) ? "" : r["CatTitle"]),
								 Available = (int)((r["Available"] == System.DBNull.Value) ? 0 : r["Available"]),
								 IdCoQuan = (int)((r["IdCoQuan"] == System.DBNull.Value) ? 0 : r["IdCoQuan"]),
								 Description = (string)((r["Description"] == System.DBNull.Value) ? "" : r["Description"]),
								 Introtext = (string)((r["Introtext"] == System.DBNull.Value) ? "" : r["Introtext"]),
								 Metadesc = (string)((r["Metadesc"] == System.DBNull.Value) ? "" : r["Metadesc"]),
								 Metakey = (string)((r["Metakey"] == System.DBNull.Value) ? "" : r["Metakey"]),
								 Metadata = (string)((r["Metadata"] == System.DBNull.Value) ? "" : r["Metadata"]),
								 TitleManufacturer = (string)((r["TitleManufacturer"] == System.DBNull.Value) ? "" : r["TitleManufacturer"]),
								 AddressManufacturer = (string)((r["AddressManufacturer"] == System.DBNull.Value) ? "" : r["AddressManufacturer"]),
								 ContactManufacturer = (string)((r["ContactManufacturer"] == System.DBNull.Value) ? "" : r["ContactManufacturer"]),
								 Star = (int)((r["Star"] == System.DBNull.Value) ? 3 : r["Star"]),
								 OCOP = (Boolean)((r["OCOP"] == System.DBNull.Value) ? false : r["OCOP"]),
								 Ids = MyModels.Encode((int)r["Id"], SecretId),
							 }).FirstOrDefault();

			if (Item != null)
			{
				if (Item.Metadata != null)
				{
					try
					{
						Item.MetadataCV = JsonConvert.DeserializeObject<API.Models.MetaData>(Item.Metadata);
					}
					catch
					{
						Item.MetadataCV = new API.Models.MetaData() { MetaTitle = Item.Title };
					}
				}
				else
				{
					Item.MetadataCV = new API.Models.MetaData() { MetaTitle = Item.Title, MetaH1 = Item.Title, MetaH3 = Item.Title };
				}

				if (Item.Str_ListImage != null && Item.Str_ListImage != "")
				{
					Item.ListImage = JsonConvert.DeserializeObject<List<ImageFile>>(Item.Str_ListImage);
					Item.ImageCount = Item.ListImage.Count();
				}
			}

			return Item;
		}

		public static dynamic SaveItem(Products dto, DataTable tbItem, int OCOP)
		{
            dto.Title = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.Title);
            dto.Description = API.Models.MyHelper.StringHelper.RemoveTagsIntroText(dto.Description);
            dto.Introtext = API.Models.MyHelper.StringHelper.RemoveTagsIntroText(dto.Introtext);
            dto.Metadesc = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.Metadesc);
            dto.Metakey = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.Metakey);
            dto.Metadata = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.Metadata);
            dto.Title = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.Title);
           
            dto.Image = API.Models.MyHelper.StringHelper.RemoveTagsLink(dto.Image);

            dto.Alias = API.Models.MyHelper.StringHelper.UrlFriendly(dto.Title);

            List<ImageFile> images = new List<ImageFile>();
			String ListImageString = "";

			if (dto.MetadataCV != null)
			{
				if (dto.MetadataCV.MetaTitle == null || dto.MetadataCV.MetaTitle.Trim() == "")
				{
					dto.MetadataCV.MetaTitle = dto.Title;
					dto.MetadataCV.MetaH1 = dto.Title;
					dto.MetadataCV.MetaH3 = dto.Title;
				}

				dto.Metadata = JsonConvert.SerializeObject(dto.MetadataCV);
			}

			dto.PriceDisplay = API.Models.MyHelper.StringHelper.ConvertNumberToDisplay(dto.Price.ToString());
			dto.PriceDealDisplay = API.Models.MyHelper.StringHelper.ConvertNumberToDisplay(dto.PriceDeal.ToString());

			if (dto.PriceDeal > 0)
			{
				dto.Discount = Math.Round((dto.PriceDeal / dto.Price * 100) - 100);
			}
			else
			{
				dto.Discount = 0;
			}

			dto.Available = dto.Quantity - dto.Sold;

			if (dto.Sold < 0)
			{
				dto.Sold = 0;
			}

			// Convert ImageFile from List -> StringObject
			if (dto.ListImage != null && dto.ListImage.Count() > 0)
			{
				for (int i = 0; i < dto.ListImage.Count(); i++)
				{
					if (dto.ListImage[i].ImagePath != null)
					{
						images.Add(dto.ListImage[i]);
					}
				}
				if (images != null && images.Count() > 0)
				{
					ListImageString = JsonConvert.SerializeObject(images);
				}
			}

			DateTime NgayDang = DateTime.ParseExact(dto.PublishUpShow, "dd/MM/yyyy", CultureInfo.InvariantCulture);
			DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Products",
			new string[] { "@flag", "@Id", "@Title", "@Alias", "@Status", "@Featured", "@Metadesc", "@Metakey", "@Metadata", "@Image", "@Images", "@Description", "@CreatedBy",
				"@ModifiedBy", "@Sku", "@Price", "@PriceDeal", "@Discount", "@Quantity", "@PublishUp", "@TBL_Products_Manufacturers", "@PriceDisplay", "@PriceDealDisplay", "@IntroText",
				"@Available", "@Sold", "@CatId", "@IdCoQuan", "@Star", "@OCOP" },
			new object[] { "SaveItem", dto.Id, dto.Title, dto.Alias, dto.Status, dto.Featured, dto.Metadesc, dto.Metakey, dto.Metadata, dto.Image, ListImageString, dto.Description, dto.CreatedBy,
				dto.ModifiedBy, dto.Sku, dto.Price, dto.PriceDeal, dto.Discount, dto.Quantity, NgayDang, tbItem, dto.PriceDisplay, dto.PriceDealDisplay, dto.Introtext,
				dto.Available, dto.Sold, dto.CatId, dto.IdCoQuan, dto.Star, OCOP });
			return (from r in tabl.AsEnumerable()
					select new
					{
						N = (int)(r["N"]),
					}).FirstOrDefault();
		}

		public static dynamic UpdateFeatured(Products dto)
		{
			DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Products",
			new string[] { "@flag", "@Id", "@Featured", "@ModifiedBy" },
			new object[] { "UpdateFeatured", dto.Id, dto.Featured, dto.ModifiedBy });
			return (from r in tabl.AsEnumerable()
					select new
					{
						N = (int)(r["N"]),
					}).FirstOrDefault();

		}

		public static dynamic UpdateStatus(Products dto)
		{
			DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Products",
			new string[] { "@flag", "@Id", "@Status", "@ModifiedBy" },
			new object[] { "UpdateStatus", dto.Id, dto.Status, dto.ModifiedBy });
			return (from r in tabl.AsEnumerable()
					select new
					{
						N = (int)(r["N"]),
					}).FirstOrDefault();

		}

		public static dynamic SaveItemDongBo(Products dto)
		{
			DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Products",
			new string[] { "@flag", "@Id", "@PriceDisplay", "@PriceDealDisplay", "@Available" },
			new object[] { "UpdateFeatured", dto.Id, dto.PriceDisplay, dto.PriceDealDisplay, dto.Available });
			return (from r in tabl.AsEnumerable()
					select new
					{
						N = (int)(r["N"]),
					}).FirstOrDefault();

		}

		public static dynamic DeleteItem(Products dto)
		{
			DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Products",
			new string[] { "@flag", "@Id", "@ModifiedBy" },
			new object[] { "DeleteItem", dto.Id, dto.ModifiedBy });
			return (from r in tabl.AsEnumerable()
					select new
					{
						N = (int)(r["N"]),
					}).FirstOrDefault();

		}

		public static dynamic CopyItem(Products dto)
		{
			DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Products",
			new string[] { "@flag", "@Id", "@ModifiedBy" },
			new object[] { "CopyItem", dto.Id, dto.ModifiedBy });
			return (from r in tabl.AsEnumerable()
					select new
					{
						N = (int)(r["N"]),
					}).FirstOrDefault();
		}

	}
}
