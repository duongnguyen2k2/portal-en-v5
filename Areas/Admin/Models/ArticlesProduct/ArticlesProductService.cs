using API.Models;
using ClosedXML.Report;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace API.Areas.Admin.Models.ArticlesProduct
{
	public class ArticlesProductService
	{
		public static List<ArticlesProduct> GetListPagination(SearchArticlesProduct dto, int IdProduct, int IdManufacturer, string SecretId)
		{
			Boolean Status = true;
			string str_sql = "GetListPagination_Status";
			string StartDate = DateTime.ParseExact(dto.ShowStartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyyMMdd");
			string EndDate = DateTime.ParseExact(dto.ShowEndDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyyMMdd");
			dto.CurrentPage = dto.CurrentPage <= 0 ? 1 : dto.CurrentPage;
			dto.ItemsPerPage = dto.ItemsPerPage <= 0 ? 15 : dto.ItemsPerPage;
			dto.Keyword = dto.Keyword == null ? "" : dto.Keyword;

			if (dto.Status == -1)
			{
				str_sql = "GetListPagination";
			}
			else if (dto.Status == 0)
			{
				Status = false;
			}

			if (dto.Keyword != null && dto.Keyword != "")
			{
				string[] arrKey = dto.Keyword.Split(':');
				if (arrKey.Count() == 2)
				{
					if (arrKey[0].ToLower() == "id")
					{
						dto.Id = int.Parse(arrKey[1]);
						if (dto.Id > 0)
						{
							str_sql = "GetListPaginationKeyWord";
						}
					}
				}
			}

			var tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_ArticlesProduct",
				new string[] { "@flag", "@IdProduct", "@IdManufacturer", "@Id", "@CurrentPage", "@ItemsPerPage", "@Keyword", "@IdCatAP", "@IdCoQuan", "@AuthorId", "@StartDate", "@EndDate", "@CreatedBy", "@Status" },
				new object[] { str_sql, IdProduct, IdManufacturer, dto.Id, dto.CurrentPage, dto.ItemsPerPage, dto.Keyword, dto.IdCatAP, dto.IdCoQuan, dto.AuthorId, StartDate, EndDate, dto.CreatedBy, Status });
			if (tabl == null)
			{
				return new List<ArticlesProduct>();
			}
			else
			{
				try
				{
					return (from r in tabl.AsEnumerable()
							select new ArticlesProduct
							{
								Id = (int)r["Id"],
								Title = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
								Alias = (string)((r["Alias"] == System.DBNull.Value) ? null : r["Alias"]),
								IdCatAP = (int)((r["IdCatAP"] == System.DBNull.Value) ? 0 : r["IdCatAP"]),
								IdManufacturer = (int)((r["IdManufacturer"] == System.DBNull.Value) ? 0 : r["IdManufacturer"]),
								IntroText = (string)((r["IntroText"] == System.DBNull.Value) ? null : r["IntroText"]),
								FullText = (string)((r["FullText"] == System.DBNull.Value) ? null : r["FullText"]),
								Images = (string)((r["Images"] == System.DBNull.Value) ? null : r["Images"]),
								PublishUp = (DateTime)((r["PublishUp"] == System.DBNull.Value) ? null : r["PublishUp"]),
								PublishUpShow = (string)((r["PublishUp"] == System.DBNull.Value) ? DateTime.Now.ToString("dd/MM/yyyy") : (string)((DateTime)r["PublishUp"]).ToString("dd/MM/yyyy")),
								Author = (string)((r["Author"] == System.DBNull.Value) ? null : r["Author"]),
								AuthorId = (int)((r["AuthorId"] == System.DBNull.Value) ? 0 : r["AuthorId"]),
								AuthorName = (string)((r["AuthorName"] == System.DBNull.Value) ? null : r["AuthorName"]),
								TenCoQuan = (string)((r["TenCoQuan"] == System.DBNull.Value) ? null : r["TenCoQuan"]),
								IdCoQuan = (int)((r["IdCoQuan"] == System.DBNull.Value) ? 0 : r["IdCoQuan"]),
								Status = (Boolean)((r["Status"] == System.DBNull.Value) ? false : r["Status"]),
								CreatedBy = (int?)((r["CreatedBy"] == System.DBNull.Value) ? 0 : r["CreatedBy"]),
								CatAPName = (string)((r["CatAPName"] == System.DBNull.Value) ? "" : r["CatAPName"]),
								CatProductName = (string)((r["CatProductName"] == System.DBNull.Value) ? "" : r["CatProductName"]),
								ManufacturerName = (string)((r["ManufacturerName"] == System.DBNull.Value) ? "" : r["ManufacturerName"]),
								TotalRows = (int)r["TotalRows"],

								Ids = MyModels.Encode((int)r["Id"], SecretId),
							}).ToList();
				}
				catch (Exception e)
				{
					return new List<ArticlesProduct>();
				}
			}
		}

		// Get List ArticlesProduct by CategoriesProduct and CategoriesArticlesProduct
		public static List<ArticlesProduct> GetListPaginationMobile(SearchArticlesProduct dto, Boolean mobileCheck, string SecretId)
		{
			if (!mobileCheck)
			{
				dto.CurrentPage = dto.CurrentPage <= 0 ? 1 : dto.CurrentPage;
				dto.ItemsPerPage = dto.ItemsPerPage <= 0 ? 10 : dto.ItemsPerPage;
				dto.Keyword = dto.Keyword == null ? "" : dto.Keyword;
			}

			var tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_ArticlesProduct",
				new string[] { "@flag", "@IdCatAP", "@IdCatProduct", "@IdManufacturer", "@CurrentPage", "@ItemsPerPage", "@Keyword", "@MobileCheck" },
				new object[] { "GetListPaginationMobile", dto.IdCatAP, dto.IdCatProduct, dto.IdManufacturer, dto.CurrentPage, dto.ItemsPerPage, dto.Keyword, mobileCheck });
			if (tabl == null)
			{
				return new List<ArticlesProduct>();
			}
			else
			{
				List<ArticlesProduct> list = (from r in tabl.AsEnumerable()
											  select new ArticlesProduct
											  {
												  Id = (int)r["Id"],
												  Title = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
												  Alias = (string)((r["Alias"] == System.DBNull.Value) ? null : r["Alias"]),
												  IdCatAP = (int)((r["IdCatAP"] == System.DBNull.Value) ? 0 : r["IdCatAP"]),
												  IdManufacturer = (int)((r["IdManufacturer"] == System.DBNull.Value) ? 0 : r["IdManufacturer"]),
												  IntroText = (string)((r["IntroText"] == System.DBNull.Value) ? null : r["IntroText"]),
												  FullText = (string)((r["FullText"] == System.DBNull.Value) ? null : r["FullText"]),
												  Images = (string)((r["Images"] == System.DBNull.Value) ? null : r["Images"]),
												  PublishUp = (DateTime)((r["PublishUp"] == System.DBNull.Value) ? null : r["PublishUp"]),
												  PublishUpShow = (string)((r["PublishUp"] == System.DBNull.Value) ? DateTime.Now.ToString("dd/MM/yyyy") : (string)((DateTime)r["PublishUp"]).ToString("dd/MM/yyyy")),
												  Author = (string)((r["Author"] == System.DBNull.Value) ? null : r["Author"]),
												  AuthorId = (int)((r["AuthorId"] == System.DBNull.Value) ? 0 : r["AuthorId"]),
												  IdCoQuan = (int)((r["IdCoQuan"] == System.DBNull.Value) ? 0 : r["IdCoQuan"]),
												  Status = (Boolean)((r["Status"] == System.DBNull.Value) ? false : r["Status"]),
												  CatProductName = (string)((r["CatProductName"] == System.DBNull.Value) ? "" : r["CatProductName"]),
												  ManufacturerName = (string)((r["ManufacturerName"] == System.DBNull.Value) ? "" : r["ManufacturerName"]),
												  TotalRows = (int)r["TotalRows"],

												  Ids = MyModels.Encode((int)r["Id"], SecretId),
											  }).ToList();

				foreach (ArticlesProduct item in list)
				{

				}

				return list;
			}
		}

		public static ArticlesProduct GetItem(decimal Id, string SecretId = null)
		{

			DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_ArticlesProduct",
			new string[] { "@flag", "@Id" },
			new object[] { "GetItem", Id });
			ArticlesProduct Item = (from r in tabl.AsEnumerable()
									select new ArticlesProduct
									{
										Id = (int)r["Id"],
										Title = (string)r["Title"],
										Alias = (string)((r["Alias"] == System.DBNull.Value) ? null : r["Alias"]),
										IdCatAP = (int)r["IdCatAP"],
										IdCatProduct = (int)((r["IdCatProduct"] == System.DBNull.Value) ? 0 : r["IdCatProduct"]),
										IdProduct = (int)((r["IdProduct"] == System.DBNull.Value) ? 0 : r["IdProduct"]),
										IdManufacturer = (int)((r["IdManufacturer"] == System.DBNull.Value) ? 0 : r["IdManufacturer"]),
										IntroText = (string)((r["IntroText"] == System.DBNull.Value) ? null : r["IntroText"]),
										FullText = (string)((r["FullText"] == System.DBNull.Value) ? null : r["FullText"]),
										Images = (string)((r["Images"] == System.DBNull.Value) ? null : r["Images"]),
										PublishUp = (DateTime)((r["PublishUp"] == System.DBNull.Value) ? null : r["PublishUp"]),
										PublishUpShow = (string)((r["PublishUp"] == System.DBNull.Value) ? DateTime.Now.ToString("dd/MM/yyyy") : (string)((DateTime)r["PublishUp"]).ToString("dd/MM/yyyy")),
										AuthorId = (int)((r["AuthorId"] == System.DBNull.Value) ? 0 : r["AuthorId"]),
										Author = (string)((r["Author"] == System.DBNull.Value) ? "" : r["Author"]),
										Language = (string)((r["Language"] == System.DBNull.Value) ? null : r["Language"]),
										Ordering = (int?)((r["Ordering"] == System.DBNull.Value) ? null : r["Ordering"]),
										Metadesc = (string)((r["Metadesc"] == System.DBNull.Value) ? null : r["Metadesc"]),
										Metakey = (string)((r["Metakey"] == System.DBNull.Value) ? null : r["Metakey"]),
										Metadata = (string)((r["Metadata"] == System.DBNull.Value) ? null : r["Metadata"]),
										Str_ListFile = (string)((r["Str_ListFile"] == System.DBNull.Value) ? null : r["Str_ListFile"]),
										Str_Link = (string)((r["Str_Link"] == System.DBNull.Value) ? null : r["Str_Link"]),
										IdCoQuan = (int)((r["IdCoQuan"] == System.DBNull.Value) ? 0 : r["IdCoQuan"]),
										Status = (Boolean)r["Status"],
										CreatedBy = (int?)((r["CreatedBy"] == System.DBNull.Value) ? 0 : r["CreatedBy"]),
										ModifiedBy = (int?)((r["ModifiedBy"] == System.DBNull.Value) ? null : r["ModifiedBy"]),
										Deleted = (Boolean)((r["Deleted"] == System.DBNull.Value) ? null : r["Deleted"]),

										CatAPName = (string)((r["CatAPName"] == System.DBNull.Value) ? "" : r["CatAPName"]),
										CatProductName = (string)((r["CatProductName"] == System.DBNull.Value) ? "" : r["CatProductName"]),
										ManufacturerName = (string)((r["ManufacturerName"] == System.DBNull.Value) ? "" : r["ManufacturerName"]),
										Ids = MyModels.Encode((int)r["Id"], SecretId),
									}).FirstOrDefault();
			if (Item != null)
			{
				if (Item.Str_ListFile != null && Item.Str_ListFile != "")
				{
					Item.ListFile = JsonConvert.DeserializeObject<List<FileArticle>>(Item.Str_ListFile);
				}
				if (Item.Str_Link != null && Item.Str_Link != "")
				{
					Item.ListLinkArticle = JsonConvert.DeserializeObject<List<LinkArticle>>(Item.Str_Link);
				}
			}
			return Item;
		}

		public static List<SelectListItem> GetListItemsStatus()
		{
			List<SelectListItem> ListItems = new List<SelectListItem>();
			ListItems.Insert(0, (new SelectListItem { Text = "--- Trạng Thái ---", Value = "-1" }));
			ListItems.Insert(1, (new SelectListItem { Text = "Tắt", Value = "0" }));
			ListItems.Insert(2, (new SelectListItem { Text = "Bật", Value = "1" }));
			return ListItems;
		}

		public static List<SelectListItem> GetListCategoryType()
		{
			List<SelectListItem> ListItems = new List<SelectListItem>();
			ListItems.Insert(0, (new SelectListItem { Text = "Chọn Loại Tin", Value = "0" }));
			ListItems.Insert(0, (new SelectListItem { Text = "Tin", Value = "1" }));
			ListItems.Insert(0, (new SelectListItem { Text = "Bài", Value = "2" }));
			return ListItems;
		}

		// ------------------------------------------------------------
		public static List<SelectListItem> GetListLevelArticle()
		{
			List<SelectListItem> ListItems = new List<SelectListItem>();
			ListItems.Insert(0, (new SelectListItem { Text = "Xếp loại bài viết", Value = "0" }));
			ListItems.Insert(0, (new SelectListItem { Text = "A", Value = "1" }));
			ListItems.Insert(0, (new SelectListItem { Text = "B", Value = "2" }));
			ListItems.Insert(0, (new SelectListItem { Text = "C", Value = "3" }));
			ListItems.Insert(0, (new SelectListItem { Text = "D", Value = "4" }));
			return ListItems;
		}

		public static List<SelectListItem> GetListItems(Boolean Selected = true)
		{

			DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_ArticlesProduct",
				new string[] { "@flag", "@Selected" }, new object[] { "GetList", Convert.ToDecimal(Selected) });
			List<SelectListItem> ListItems = (from r in tabl.AsEnumerable()
											  select new SelectListItem
											  {
												  Value = (string)((r["Id"] == System.DBNull.Value) ? null : r["Id"].ToString()),
												  Text = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
											  }).ToList();

			ListItems.Insert(0, (new SelectListItem { Text = "Chọn bài đăng", Value = "0" }));
			return ListItems;
		}

		public static List<ArticlesProduct> GetListLogArticlesProduct(int Id, string SecretId)
		{
			DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_LogArticlesProduct",
				new string[] { "@flag", "@Id" }, new object[] { "GetList", Id });
			if (tabl == null)
			{
				return new List<ArticlesProduct>();
			}
			else
			{
				return (from r in tabl.AsEnumerable()
						select new ArticlesProduct
						{
							Id = (int)r["Id"],
							Title = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
							Alias = (string)((r["Alias"] == System.DBNull.Value) ? null : r["Alias"]),
							IdCatAP = (int)((r["IdCatAP"] == System.DBNull.Value) ? 0 : r["IdCatAP"]),
							IdManufacturer = (int)((r["IdManufacturer"] == System.DBNull.Value) ? 0 : r["IdManufacturer"]),
							IntroText = (string)((r["IntroText"] == System.DBNull.Value) ? null : r["IntroText"]),
							FullText = (string)((r["FullText"] == System.DBNull.Value) ? null : r["FullText"]),
							Images = (string)((r["Images"] == System.DBNull.Value) ? null : r["Images"]),
							PublishUp = (DateTime)((r["PublishUp"] == System.DBNull.Value) ? null : r["PublishUp"]),
							PublishUpShow = (string)((r["PublishUp"] == System.DBNull.Value) ? DateTime.Now.ToString("dd/MM/yyyy") : (string)((DateTime)r["PublishUp"]).ToString("dd/MM/yyyy")),
							Author = (string)((r["Author"] == System.DBNull.Value) ? null : r["Author"]),
							AuthorId = (int)((r["AuthorId"] == System.DBNull.Value) ? 0 : r["AuthorId"]),
							AuthorName = (string)((r["AuthorName"] == System.DBNull.Value) ? null : r["AuthorName"]),
							Language = (string)((r["Language"] == System.DBNull.Value) ? null : r["Language"]),
							ModifiedDate = (DateTime?)((r["ModifiedDate"] == System.DBNull.Value) ? null : r["ModifiedDate"]),
							Metadesc = (string)((r["Metadesc"] == System.DBNull.Value) ? null : r["Metadesc"]),
							Metakey = (string)((r["Metakey"] == System.DBNull.Value) ? null : r["Metakey"]),
							Metadata = (string)((r["Metadata"] == System.DBNull.Value) ? null : r["Metadata"]),
							IdCoQuan = (int)((r["IdCoQuan"] == System.DBNull.Value) ? 0 : r["IdCoQuan"]),
							Status = (Boolean)((r["Status"] == System.DBNull.Value) ? false : r["Status"]),
							CreatedBy = (int?)((r["CreatedBy"] == System.DBNull.Value) ? null : r["CreatedBy"]),
							ModifiedBy = (int?)((r["ModifiedBy"] == System.DBNull.Value) ? null : r["ModifiedBy"]),
							CreatedDate = (DateTime)((r["CreatedDate"] == System.DBNull.Value) ? null : r["CreatedDate"]),
							CatProductName = (string)((r["CatProductName"] == System.DBNull.Value) ? "" : r["CatProductName"]),
							ManufacturerName = (string)((r["ManufacturerName"] == System.DBNull.Value) ? "" : r["ManufacturerName"]),
							TotalRows = (int)r["TotalRows"],

							Ids = MyModels.Encode((int)r["Id"], SecretId),
						}).ToList();
			}

		}
		public static List<SelectListItem> GetListStaticArticle(Boolean Selected = true)
		{

			DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_ArticlesProduct",
				new string[] { "@flag", "@Selected" }, new object[] { "GetListStaticArticle", Convert.ToDecimal(Selected) });
			List<SelectListItem> ListItems = (from r in tabl.AsEnumerable()
											  select new SelectListItem
											  {
												  Value = (string)((r["Id"] == System.DBNull.Value) ? null : r["Id"].ToString()),
												  Text = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
											  }).ToList();

			ListItems.Insert(0, (new SelectListItem { Text = "Chọn bài đăng", Value = "0" }));
			return ListItems;

		}

		public static List<ArticlesProduct> GetList(Boolean Selected = true)
		{

			DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_ArticlesProduct",
				new string[] { "@flag", "@Selected" }, new object[] { "GetList", Convert.ToDecimal(Selected) });
			if (tabl == null)
			{
				return new List<ArticlesProduct>();
			}
			else
			{
				return (from r in tabl.AsEnumerable()
						select new ArticlesProduct
						{
							Id = (int)r["Id"],
							Title = (string)r["Title"],
							Alias = (string)((r["Alias"] == System.DBNull.Value) ? null : r["Alias"]),
							IdCatAP = (int)r["IdCatAP"],
							IntroText = (string)((r["IntroText"] == System.DBNull.Value) ? null : r["IntroText"]),
							FullText = (string)((r["FullText"] == System.DBNull.Value) ? null : r["FullText"]),
							Status = (Boolean)r["Status"],
							CreatedBy = (int?)((r["CreatedBy"] == System.DBNull.Value) ? null : r["CreatedBy"]),
							ModifiedBy = (int?)((r["ModifiedBy"] == System.DBNull.Value) ? null : r["ModifiedBy"]),
							CreatedDate = (DateTime)((r["CreatedDate"] == System.DBNull.Value) ? null : r["CreatedDate"]),
							PublishUp = (DateTime)((r["PublishUp"] == System.DBNull.Value) ? DateTime.Now : r["PublishUp"]),
							PublishUpShow = (string)((r["PublishUp"] == System.DBNull.Value) ? DateTime.Now.ToString("dd/MM/yyyy") : r["PublishUp"]),
							ModifiedDate = (DateTime?)((r["ModifiedDate"] == System.DBNull.Value) ? null : r["ModifiedDate"]),
							Metadesc = (string)((r["Metadesc"] == System.DBNull.Value) ? null : r["Metadesc"]),
							Metakey = (string)((r["Metakey"] == System.DBNull.Value) ? null : r["Metakey"]),
							Metadata = (string)((r["Metadata"] == System.DBNull.Value) ? null : r["Metadata"]),
							Language = (string)((r["Language"] == System.DBNull.Value) ? null : r["Language"]),
							Images = (string)((r["Images"] == System.DBNull.Value) ? null : r["Images"]),
							Ordering = (int?)((r["Ordering"] == System.DBNull.Value) ? null : r["Ordering"]),
							Deleted = (Boolean)((r["Deleted"] == System.DBNull.Value) ? null : r["Deleted"]),
							TotalRows = (int)r["TotalRows"],
						}).ToList();
			}

		}

		public static List<ArticlesProduct> GetListNew(int IdCatAP = 0, int Limit = 5, int IdCoQuan = 1)
		{

			DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_ArticlesProduct",
				new string[] { "@flag", "@IdCatAP", "@IdCoQuan" }, new object[] { "GetListNew", IdCatAP, IdCoQuan });
			if (tabl == null)
			{
				return new List<ArticlesProduct>();
			}
			else
			{
				return (from r in tabl.AsEnumerable()
						select new ArticlesProduct
						{
							Id = (int)r["Id"],
							Title = (string)r["Title"],
							Alias = (string)((r["Alias"] == System.DBNull.Value) ? null : r["Alias"]),
							IdCatAP = (int)r["IdCatAP"],
							IntroText = (string)((r["IntroText"] == System.DBNull.Value) ? null : r["IntroText"]),
							FullText = (string)((r["FullText"] == System.DBNull.Value) ? null : r["FullText"]),
							Status = (Boolean)r["Status"],
							CreatedBy = (int?)((r["CreatedBy"] == System.DBNull.Value) ? null : r["CreatedBy"]),
							ModifiedBy = (int?)((r["ModifiedBy"] == System.DBNull.Value) ? null : r["ModifiedBy"]),
							CreatedDate = (DateTime)((r["CreatedDate"] == System.DBNull.Value) ? DateTime.Now : r["CreatedDate"]),
							PublishUp = (DateTime)((r["PublishUp"] == System.DBNull.Value) ? DateTime.Now : r["PublishUp"]),
							PublishUpShow = (string)((r["PublishUp"] == System.DBNull.Value) ? DateTime.Now.ToString("dd/MM/yyyy") : (string)((DateTime)r["PublishUp"]).ToString("dd/MM/yyyy")),
							ModifiedDate = (DateTime?)((r["ModifiedDate"] == System.DBNull.Value) ? null : r["ModifiedDate"]),
							Metadesc = (string)((r["Metadesc"] == System.DBNull.Value) ? null : r["Metadesc"]),
							Metakey = (string)((r["Metakey"] == System.DBNull.Value) ? null : r["Metakey"]),
							Metadata = (string)((r["Metadata"] == System.DBNull.Value) ? null : r["Metadata"]),
							Language = (string)((r["Language"] == System.DBNull.Value) ? null : r["Language"]),
							Images = (string)((r["Images"] == System.DBNull.Value) ? null : r["Images"]),
							Ordering = (int?)((r["Ordering"] == System.DBNull.Value) ? null : r["Ordering"]),
							Author = (string)((r["Author"] == System.DBNull.Value) ? "" : r["Author"]),
							Deleted = (Boolean)((r["Deleted"] == System.DBNull.Value) ? null : r["Deleted"])
						}).ToList();
			}

		}

		public static ArticlesProduct GetItemByAlias(string Alias, string SecretId = null)
		{

			DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_ArticlesProduct",
			new string[] { "@flag", "@Alias" }, new object[] { "GetItemByAlias", Alias });
			ArticlesProduct Item = (from r in tabl.AsEnumerable()
									select new ArticlesProduct
									{
										Id = (int)r["Id"],
										Title = (string)r["Title"],
										Alias = (string)((r["Alias"] == System.DBNull.Value) ? null : r["Alias"]),
										Str_ListFile = (string)((r["Str_ListFile"] == System.DBNull.Value) ? null : r["Str_ListFile"]),
										Str_Link = (string)((r["Str_Link"] == System.DBNull.Value) ? null : r["Str_Link"]),
										IdCatAP = (int)r["IdCatAP"],
										IntroText = (string)((r["IntroText"] == System.DBNull.Value) ? null : r["IntroText"]),
										FullText = (string)((r["FullText"] == System.DBNull.Value) ? null : r["FullText"]),
										Status = (Boolean)r["Status"],
										CreatedBy = (int?)((r["CreatedBy"] == System.DBNull.Value) ? null : r["CreatedBy"]),
										ModifiedBy = (int?)((r["ModifiedBy"] == System.DBNull.Value) ? null : r["ModifiedBy"]),
										CreatedDate = (DateTime)((r["CreatedDate"] == System.DBNull.Value) ? null : r["CreatedDate"]),
										PublishUp = (DateTime)((r["PublishUp"] == System.DBNull.Value) ? null : r["PublishUp"]),
										PublishUpShow = (string)((r["PublishUp"] == System.DBNull.Value) ? DateTime.Now.ToString("dd/MM/yyyy") : (string)((DateTime)r["PublishUp"]).ToString("dd/MM/yyyy")),
										ModifiedDate = (DateTime?)((r["ModifiedDate"] == System.DBNull.Value) ? null : r["ModifiedDate"]),
										Metadesc = (string)((r["Metadesc"] == System.DBNull.Value) ? null : r["Metadesc"]),
										Metakey = (string)((r["Metakey"] == System.DBNull.Value) ? null : r["Metakey"]),
										Metadata = (string)((r["Metadata"] == System.DBNull.Value) ? null : r["Metadata"]),
										Language = (string)((r["Language"] == System.DBNull.Value) ? null : r["Language"]),
										Images = (string)((r["Images"] == System.DBNull.Value) ? null : r["Images"]),
										Ordering = (int?)((r["Ordering"] == System.DBNull.Value) ? null : r["Ordering"]),
										Deleted = (Boolean)((r["Deleted"] == System.DBNull.Value) ? null : r["Deleted"]),
										AuthorId = (int)((r["AuthorId"] == System.DBNull.Value) ? 0 : r["AuthorId"]),
										Ids = MyModels.Encode((int)r["Id"], SecretId),
									}).FirstOrDefault();

			if (Item.Str_ListFile != null && Item.Str_ListFile != "")
			{
				Item.ListFile = JsonConvert.DeserializeObject<List<FileArticle>>(Item.Str_ListFile);
			}

			if (Item.Str_Link != null && Item.Str_Link != "")
			{
				Item.ListLinkArticle = JsonConvert.DeserializeObject<List<LinkArticle>>(Item.Str_Link);
			}
			return Item;
		}


		public static ArticlesProduct GetItemLogArticle(decimal Id, string SecretId = null)
		{
			DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_LogArticlesProduct",
			new string[] { "@flag", "@Id" }, new object[] { "GetItem", Id });
			ArticlesProduct Item = (from r in tabl.AsEnumerable()
									select new ArticlesProduct
									{
										Id = (int)r["Id"],
										Title = (string)r["Title"],
										Str_ListFile = (string)((r["Str_ListFile"] == System.DBNull.Value) ? null : r["Str_ListFile"]),
										Str_Link = (string)((r["Str_Link"] == System.DBNull.Value) ? null : r["Str_Link"]),
										Alias = (string)((r["Alias"] == System.DBNull.Value) ? null : r["Alias"]),
										IdCatAP = (int)r["IdCatAP"],
										IntroText = (string)((r["IntroText"] == System.DBNull.Value) ? null : r["IntroText"]),
										FullText = (string)((r["FullText"] == System.DBNull.Value) ? null : r["FullText"]),
										Status = (Boolean)r["Status"],
										CreatedBy = (int?)((r["CreatedBy"] == System.DBNull.Value) ? null : r["CreatedBy"]),
										ModifiedBy = (int?)((r["ModifiedBy"] == System.DBNull.Value) ? null : r["ModifiedBy"]),
										CreatedDate = (DateTime)((r["CreatedDate"] == System.DBNull.Value) ? null : r["CreatedDate"]),
										PublishUp = (DateTime)((r["PublishUp"] == System.DBNull.Value) ? null : r["PublishUp"]),
										PublishUpShow = (string)((r["PublishUp"] == System.DBNull.Value) ? DateTime.Now.ToString("dd/MM/yyyy") : (string)((DateTime)r["PublishUp"]).ToString("dd/MM/yyyy")),
										ModifiedDate = (DateTime?)((r["ModifiedDate"] == System.DBNull.Value) ? null : r["ModifiedDate"]),
										Metadesc = (string)((r["Metadesc"] == System.DBNull.Value) ? null : r["Metadesc"]),
										Metakey = (string)((r["Metakey"] == System.DBNull.Value) ? null : r["Metakey"]),
										Metadata = (string)((r["Metadata"] == System.DBNull.Value) ? null : r["Metadata"]),
										Language = (string)((r["Language"] == System.DBNull.Value) ? null : r["Language"]),
										Images = (string)((r["Images"] == System.DBNull.Value) ? null : r["Images"]),
										Ordering = (int?)((r["Ordering"] == System.DBNull.Value) ? null : r["Ordering"]),
										IdCoQuan = (int)((r["IdCoQuan"] == System.DBNull.Value) ? 0 : r["IdCoQuan"]),
										AuthorId = (int)((r["AuthorId"] == System.DBNull.Value) ? 0 : r["AuthorId"]),
										Ids = MyModels.Encode((int)r["Id"], SecretId),
									}).FirstOrDefault();
			if (Item.Str_ListFile != null && Item.Str_ListFile != "")
			{
				Item.ListFile = JsonConvert.DeserializeObject<List<FileArticle>>(Item.Str_ListFile);
			}
			if (Item.Str_Link != null && Item.Str_Link != "")
			{
				Item.ListLinkArticle = JsonConvert.DeserializeObject<List<LinkArticle>>(Item.Str_Link);
			}
			return Item;
		}



		public static dynamic SaveItem(ArticlesProduct dto)
		{
            dto.Metadesc = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.Metadesc);
            dto.Metakey = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.Metakey);
            dto.Metadata = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.Metadata);
            dto.AuthorName = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.AuthorName);
            dto.Author = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.Author);
            dto.Title = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.Title);
            dto.IntroText = API.Models.MyHelper.StringHelper.RemoveTagsIntroText(dto.IntroText);
            dto.FullText = API.Models.MyHelper.StringHelper.RemoveTagsFullText(dto.FullText);
            dto.Images = API.Models.MyHelper.StringHelper.RemoveTagsLink(dto.Images);
            dto.Alias = API.Models.MyHelper.StringHelper.UrlFriendly(dto.Title);
            
            string Str_ListFile = null;
			string Str_Link = null;
			List<FileArticle> ListFileArticle = new List<FileArticle>();
			List<LinkArticle> ListLinkArticle = new List<LinkArticle>();

			if (dto.IdCatAP == 0)
			{
				dto.IdCatAP = 1;
			}

			if (dto.ListFile != null && dto.ListFile.Count() > 0)
			{
				for (int i = 0; i < dto.ListFile.Count(); i++)
				{
					if (dto.ListFile[i].FilePath != null && dto.ListFile[i].FilePath.Trim() != "")
					{
						ListFileArticle.Add(dto.ListFile[i]);
					}
				}
				if (ListFileArticle != null && ListFileArticle.Count() > 0)
				{
					Str_ListFile = JsonConvert.SerializeObject(ListFileArticle);
				}
			}

			if (dto.ListLinkArticle != null && dto.ListLinkArticle.Count() > 0)
			{
				for (int i = 0; i < dto.ListLinkArticle.Count(); i++)
				{
					if (dto.ListLinkArticle[i].Title != null && dto.ListLinkArticle[i].Title.Trim() != "" && dto.ListLinkArticle[i].Status == true)
					{
						ListLinkArticle.Add(dto.ListLinkArticle[i]);
					}
				}
				if (ListLinkArticle != null && ListLinkArticle.Count() > 0)
				{
					Str_Link = JsonConvert.SerializeObject(ListLinkArticle);
				}
			}

			DateTime NgayDang = DateTime.ParseExact(dto.PublishUpShow, "dd/MM/yyyy", CultureInfo.InvariantCulture);
			dto.CreatedDate = DateTime.Now;
			DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_ArticlesProduct",
			new string[] { "@flag", "@Id", "@Title", "@Alias", "@IntroText", "@FullText", "@Status", "@CreatedBy", "@ModifiedBy", "@CreatedDate", "@ModifiedDate", "@Metadesc", "@Metakey", "@Metadata",
				"@Language", "@Images", "@Ordering", "@Deleted", "@IdCoQuan", "@PublishUp", "@Str_ListFile", "@Str_Link", "@AuthorId",
				"@Author", "@IdManufacturer", "@IdCatProduct", "@IdCatAP", "@IdProduct" },
			new object[] { "SaveItem", dto.Id, dto.Title, dto.Alias, dto.IntroText, dto.FullText, dto.Status, dto.CreatedBy, dto.ModifiedBy, dto.CreatedDate, dto.ModifiedDate, dto.Metadesc, dto.Metakey, dto.Metadata,
				dto.Language, dto.Images, dto.Ordering, dto.Deleted, dto.IdCoQuan, NgayDang, Str_ListFile, Str_Link, dto.AuthorId,
				 dto.Author, dto.IdManufacturer, dto.IdCatProduct, dto.IdCatAP, dto.IdProduct });
			return (from r in tabl.AsEnumerable()
					select new
					{
						N = (int)(r["N"]),
					}).FirstOrDefault();

		}
		public static dynamic DeleteItem(ArticlesProduct dto)
		{
			DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_ArticlesProduct",
			new string[] { "@flag", "@Id", "@ModifiedBy" },
			new object[] { "DeleteItem", dto.Id, dto.ModifiedBy });
			return (from r in tabl.AsEnumerable()
					select new
					{
						N = (int)(r["N"]),
					}).FirstOrDefault();

		}

		public static dynamic UpdateStatus(ArticlesProduct dto)
		{
			DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_ArticlesProduct",
			new string[] { "@flag", "@Id", "@Status", "@ModifiedBy" },
			new object[] { "UpdateStatus", dto.Id, dto.Status, dto.ModifiedBy });
			return (from r in tabl.AsEnumerable()
					select new
					{
						N = (int)(r["N"]),
					}).FirstOrDefault();

		}


		public static dynamic UpdateAlias(int Id, string Alias, string Introtext)
		{
			DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_ArticlesProduct",
			new string[] { "@flag", "@Id", "@Alias", "@Introtext" },
			new object[] { "UpdateAlias", Id, Alias, Introtext });
			return (from r in tabl.AsEnumerable()
					select new
					{
						N = (int)(r["N"]),
					}).FirstOrDefault();

		}

		public static dynamic UpdateLike(int Id)
		{
			DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_ArticlesProduct",
			new string[] { "@flag", "@Id" },
			new object[] { "UpdateLike", Id });
			return (from r in tabl.AsEnumerable()
					select new
					{
						N = (int)(r["N"]),
					}).FirstOrDefault();

		}

		public static dynamic UpdateFileAudio(int Id, string FileItem)
		{
			DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_ArticlesProduct",
			new string[] { "@flag", "@Id", "@FileItem" },
			new object[] { "UpdateFileAudio", Id, FileItem });
			return (from r in tabl.AsEnumerable()
					select new
					{
						N = (int)(r["N"]),
					}).FirstOrDefault();

		}

		public static async Task<string> TaoFileBaoCao(SearchArticlesProduct dto, Boolean flag = false, DMCoQuan.DMCoQuan ItemCoQuan = null)
		{
			string ShowCurrentDate = "Đắk Lắk, ngày " + DateTime.Now.ToString("dd") + " tháng " + DateTime.Now.ToString("MM") + " năm " + DateTime.Now.ToString("yyyy");
			string CreateDate = DateTime.Now.ToString("dd/MM/yyyy");
			dto.Status = 3;
			double Total = 0;
			List<ArticlesProduct> ListItems = ArticlesProductService.GetListPagination(dto, 1, 1, "Hello");
			if (ListItems != null && ListItems.Count() > 0)
			{
				for (int i = 0; i < ListItems.Count(); i++)
				{
					Total = Total;
					//ListItems[i].PublishUpShow = ListItems[i].PublishUp.ToString("dd/MM/yyyy HH:mm:ss");
				}
			}
			string TenFileLuu = "DanhSachBaiViet_" + string.Format("{0:ddMMyyHHmmss}" + ".xlsx", DateTime.Now);
			string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "temp");
			string outputFile = System.IO.Path.Combine(path, TenFileLuu);
			string ReportTemplate = System.IO.Path.Combine(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ReportTemplate"), "DanhSachBaiViet.xlsx");
			if (ItemCoQuan.Id == 7)
			{
				ReportTemplate = System.IO.Path.Combine(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ReportTemplate"), "DanhSachBaiVietEakar.xlsx");
			}
			var template = new XLTemplate(ReportTemplate);
			template.AddVariable("ShowStartDate", dto.ShowStartDate);
			template.AddVariable("ShowEndDate", dto.ShowEndDate);
			template.AddVariable("ListItems", ListItems);
			template.AddVariable("ShowCurrentDate", ShowCurrentDate);
			template.AddVariable("Total", Total);
			template.AddVariable("CompanyName", ItemCoQuan.CompanyName);
			template.AddVariable("ShowTotal", API.Models.MyHelper.StringHelper.NumberToTextVN(Decimal.Parse(Total.ToString())));
			template.Generate();
			template.SaveAs(outputFile);
			return TenFileLuu;
		}
	}
}
