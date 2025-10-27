using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using API.Areas.Admin.Models.CategoriesProducts;
using API.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace API.Areas.Admin.Models.CategoriesProducts
{
	public class CategoriesProductsService
	{
		public static List<CategoriesProducts> GetListPagination(SearchCategoriesProducts dto, string SecretId)
		{
			if (dto.CurrentPage <= 0)
			{
				dto.CurrentPage = 1;
			}
			if (dto.ItemsPerPage <= 0)
			{
				dto.ItemsPerPage = 10;
			}
			if (dto.Keyword == null)
			{
				dto.Keyword = "";
			}
			var tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_CategoriesProducts",
				new string[] { "@flag", "@CurrentPage", "@ItemsPerPage", "@Keyword" },
				new object[] { "GetListPagination", dto.CurrentPage, dto.ItemsPerPage, dto.Keyword });
			if (tabl == null)
			{
				return new List<CategoriesProducts>();
			}
			else
			{
				return (from r in tabl.AsEnumerable()
						select new CategoriesProducts
						{
							Id = (int)r["Id"],
							Title = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
							Alias = (string)((r["Alias"] == System.DBNull.Value) ? null : r["Alias"]),
							Code = (string)((r["Code"] == System.DBNull.Value) ? null : r["Code"]),
							Introtext = (string)((r["Introtext"] == System.DBNull.Value) ? null : r["Introtext"]),
							ParentId = (int?)((r["ParentId"] == System.DBNull.Value) ? null : r["ParentId"]),
							Featured = (Boolean)((r["Featured"] == System.DBNull.Value) ? false : r["Featured"]),
							QuantityProducts = (int)((r["QuantityProducts"] == System.DBNull.Value) ? 0 : r["QuantityProducts"]),
							Status = (Boolean)r["Status"],
							Hits = (int)((r["Hits"] == System.DBNull.Value) ? 0 : r["Hits"]),
							PerCat = (int)((r["PerCat"] == System.DBNull.Value) ? 0 : r["PerCat"]),
							Images = (string)((r["Images"] == System.DBNull.Value) ? null : r["Images"]),
							Params = (string)((r["Params"] == System.DBNull.Value) ? null : r["Params"]),
							Ordering = (int)((r["Ordering"] == System.DBNull.Value) ? 0 : r["Ordering"]),
							StrPermission = (string)((r["StrPermission"] == System.DBNull.Value) ? "" : r["StrPermission"]),
							Ids = MyModels.Encode((int)r["Id"], SecretId),
						}).ToList();
			}
		}

		public static List<CategoriesProducts> GetList(Boolean Selected = true)
		{

			DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_CategoriesProducts",
				new string[] { "@flag", "@Selected" }, new object[] { "GetList", Convert.ToDecimal(Selected) });
			if (tabl == null)
			{
				return new List<CategoriesProducts>();
			}
			else
			{
				return (from r in tabl.AsEnumerable()
						select new CategoriesProducts
						{
							Id = (int)r["Id"],
							Title = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
							Alias = (string)((r["Alias"] == System.DBNull.Value) ? null : r["Alias"]),
							Code = (string)((r["Code"] == System.DBNull.Value) ? null : r["Code"]),
							//Introtext = (string)((r["Introtext"] == System.DBNull.Value) ? null : r["Introtext"]),
							ParentId = (int?)((r["ParentId"] == System.DBNull.Value) ? null : r["ParentId"]),
							PerCat = (int)((r["PerCat"] == System.DBNull.Value) ? 0 : r["PerCat"]),
							Images = (string)((r["Images"] == System.DBNull.Value) ? null : r["Images"]),

						}).ToList();
			}
		}

		public static List<CatGroup> GetListGroupsByCat(int IdCat, string SecretId = null)
		{

			DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_CategoriesProducts",
			new string[] { "@flag", "@IdCat" }, new object[] { "GetListGroupsByCat", IdCat });
			return (from r in tabl.AsEnumerable()
					select new CatGroup
					{
						IdCat = (int)r["IdCat"],
						IdGroup = (int)r["IdGroup"],
						CatName = (string)((r["CatName"] == System.DBNull.Value) ? "" : r["CatName"]),
						GroupTitle = (string)((r["GroupTitle"] == System.DBNull.Value) ? "" : r["GroupTitle"]),
						IdsCat = MyModels.Encode((int)r["IdCat"], SecretId),
						IdsGroup = MyModels.Encode((int)r["IdGroup"], SecretId),
						Selected = (Boolean)((r["Selected"] == System.DBNull.Value) ? false : r["Selected"]),
					}).ToList();
		}

		public static dynamic SaveManagerCatGroup(List<CatGroup> ListFiles, int IdCat)
		{
			List<string> ListTitle = new List<string>();
			DataTable tbItem = new DataTable();
			tbItem.Columns.Add("IdCat", typeof(int));
			tbItem.Columns.Add("IdGroup", typeof(int));

			for (int k = 0; k < ListFiles.Count(); k++)
			{
				if (ListFiles[k].Selected)
				{
					var row = tbItem.NewRow();
					row["IdCat"] = IdCat;
					row["IdGroup"] = ListFiles[k].IdGroup;
					tbItem.Rows.Add(row);
					ListTitle.Add(ListFiles[k].GroupTitle);
				}
			}
			string StrPermission = string.Join(",", ListTitle);
			DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_CategoriesProducts",
			new string[] { "@flag", "@TBL_ManagerCatProductsGroup", "@IdCat", "@StrPermission" },
			new object[] { "SaveManagerCatGroup", tbItem, IdCat, StrPermission });
			return (from r in tabl.AsEnumerable()
					select new
					{
						N = (int)(r["N"]),
					}).FirstOrDefault();
		}

		public static List<CategoriesProducts> GetListFeaturedHome()
		{
			DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_CategoriesProducts",
				new string[] { "@flag" }, new object[] { "GetListFeaturedHome" });
			if (tabl == null)
			{
				return new List<CategoriesProducts>();
			}
			else
			{
				return (from r in tabl.AsEnumerable()
						select new CategoriesProducts
						{
							Id = (int)r["Id"],
							Title = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
							Images = (string)((r["Images"] == System.DBNull.Value) ? null : r["Images"]),
							Alias = (string)((r["Alias"] == System.DBNull.Value) ? null : r["Alias"]),
							ParentId = (int?)((r["ParentId"] == System.DBNull.Value) ? null : r["ParentId"]),
							QuantityProducts = (int)((r["QuantityProducts"] == System.DBNull.Value) ? 0 : r["QuantityProducts"]),
						}).ToList();
			}
		}

		public static List<SelectListItem> GetListItemsByUser(Boolean Selected = true, int IdUser = 0)
		{
			/*
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_CategoriesProducts",
                new string[] { "@flag", "@Selected" , "@IdUser" }, new object[] { "GetList", Convert.ToDecimal(Selected), IdUser });

            List<CategoriesProducts> ListItems =  (from r in tabl.AsEnumerable()
                    select new CategoriesProducts
                    {
                        Id = (int)r["Id"],
                        Title = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),                        
                        PerCat = (int)((r["PerCat"] == System.DBNull.Value) ? 0 : r["PerCat"]),                                             
                    }).ToList();

            List<SelectListItem> ListItemsSl = new List<SelectListItem>();
            ListItemsSl.Insert(0, (new SelectListItem { Text = "Chọn Danh mục", Value = "0" }));
            int k = 0;
            if (ListItems != null && ListItems.Count() > 0) {
                for (int i = 0; i < ListItems.Count(); i++) {
                    if (ListItems[i].PerCat > 0) {
                        k++;
                        ListItemsSl.Insert(k, (new SelectListItem { Text = ListItems[i].Title, Value = ListItems[i].Id.ToString() }));
                    }
                }
                

            }

            return ListItemsSl;
            */

			DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_CategoriesProducts",
				new string[] { "@flag", "@Selected" }, new object[] { "GetList", Convert.ToDecimal(Selected) });
			List<SelectListItem> ListItems = (from r in tabl.AsEnumerable()
											  select new SelectListItem
											  {
												  Value = (string)((r["Id"] == System.DBNull.Value) ? null : r["Id"].ToString()),
												  Text = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
											  }).ToList();

			ListItems.Insert(0, (new SelectListItem { Text = "Chọn Danh mục", Value = "0" }));
			return ListItems;
		}

		public static List<SelectListItem> GetListItems(Boolean Selected = true)
		{
			DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_CategoriesProducts",
				new string[] { "@flag", "@Selected" }, new object[] { "GetList", Convert.ToDecimal(Selected) });
			List<SelectListItem> ListItems = (from r in tabl.AsEnumerable()
											  select new SelectListItem
											  {
												  Value = (string)((r["Id"] == System.DBNull.Value) ? null : r["Id"].ToString()),
												  Text = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
											  }).ToList();

			ListItems.Insert(0, (new SelectListItem { Text = "Chọn Danh mục", Value = "0" }));
			return ListItems;
		}

		public static List<SelectListItem> GetSelectListItems(Boolean Selected = true)
		{
			DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_CategoriesProducts",
				new string[] { "@flag", "@Selected" },
				new object[] { "GetSelectListItems", Convert.ToDecimal(Selected) });
			List<SelectListItem> ListItems = (from r in tabl.AsEnumerable()
											  select new SelectListItem
											  {
												  Value = (string)((r["Id"] == System.DBNull.Value) ? null : r["Id"].ToString()),
												  Text = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
												  Disabled = false,
											  }).ToList();

			ListItems.Insert(0, (new SelectListItem { Text = "Chọn Danh mục Nông sản", Value = "-1", Disabled = true }));
			return ListItems;
		}

		public static List<SelectListItem> GetSelectListItemsNo(Boolean Selected = true)
		{
			DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_CategoriesProducts",
				new string[] { "@flag", "@Selected" },
				new object[] { "GetSelectListItems", Convert.ToDecimal(Selected) });
			List<SelectListItem> ListItems = (from r in tabl.AsEnumerable()
											  select new SelectListItem
											  {
												  Value = (string)((r["Id"] == System.DBNull.Value) ? null : r["Id"].ToString()),
												  Text = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
												  Disabled = false,
											  }).ToList();
			return ListItems;
		}

		public static CategoriesProducts GetItem(decimal Id, string SecretId = null)
		{

			DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_CategoriesProducts",
			new string[] { "@flag", "@Id" }, new object[] { "GetItem", Id });

			CategoriesProducts Item = (from r in tabl.AsEnumerable()
									   select new CategoriesProducts
									   {
										   Id = (int)r["Id"],
										   Title = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
										   Alias = (string)((r["Alias"] == System.DBNull.Value) ? null : r["Alias"]),
										   Code = (string)((r["Code"] == System.DBNull.Value) ? null : r["Code"]),
										   Introtext = (string)((r["Introtext"] == System.DBNull.Value) ? null : r["Introtext"]),
										   Description = (string)((r["Description"] == System.DBNull.Value) ? null : r["Description"]),
										   ParentId = (int?)((r["ParentId"] == System.DBNull.Value) ? null : r["ParentId"]),
										   Featured = (Boolean)((r["Featured"] == System.DBNull.Value) ? false : r["Featured"]),
										   Status = (Boolean)r["Status"],
										   Metadesc = (string)((r["Metadesc"] == System.DBNull.Value) ? null : r["Metadesc"]),
										   Metakey = (string)((r["Metakey"] == System.DBNull.Value) ? null : r["Metakey"]),
										   Metadata = (string)((r["Metadata"] == System.DBNull.Value) ? null : r["Metadata"]),
										   Images = (string)((r["Images"] == System.DBNull.Value) ? null : r["Images"]),
										   Params = (string)((r["Params"] == System.DBNull.Value) ? null : r["Params"]),
										   Ordering = (int)((r["Ordering"] == System.DBNull.Value) ? 0 : r["Ordering"]),
										   QuantityProducts = (int)((r["QuantityProducts"] == System.DBNull.Value) ? 0 : r["QuantityProducts"]),
										   Hits = (int)((r["Hits"] == System.DBNull.Value) ? 0 : r["Hits"]),
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
						Item.MetadataCV = new API.Models.MetaData() { MetaTitle = Item.Title, MetaH1 = Item.Title, MetaH3 = Item.Title };
					}
				}
				else
				{
					Item.MetadataCV = new API.Models.MetaData() { MetaTitle = Item.Title, MetaH1 = Item.Title, MetaH3 = Item.Title };
				}
			}
			else
			{
				Item = new CategoriesProducts();
			}
			return Item;
		}
		public static CategoriesProducts GetItemByAlias(string Alias, string SecretId = null)
		{

			DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_CategoriesProducts",
			new string[] { "@flag", "@Alias" }, new object[] { "GetItemByAlias", Alias });
			return (from r in tabl.AsEnumerable()
					select new CategoriesProducts
					{
						Id = (int)r["Id"],
						Title = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
						Alias = (string)((r["Alias"] == System.DBNull.Value) ? null : r["Alias"]),
						Code = (string)((r["Code"] == System.DBNull.Value) ? null : r["Code"]),
						Introtext = (string)((r["Introtext"] == System.DBNull.Value) ? null : r["Introtext"]),
						Description = (string)((r["Description"] == System.DBNull.Value) ? null : r["Description"]),
						ParentId = (int?)((r["ParentId"] == System.DBNull.Value) ? null : r["ParentId"]),
						Featured = (Boolean)((r["Featured"] == System.DBNull.Value) ? false : r["Featured"]),
						Status = (Boolean)r["Status"],
						//FeaturedHome = (Boolean)((r["FeaturedHome"] == System.DBNull.Value) ? false : r["FeaturedHome"]),
						Metadesc = (string)((r["Metadesc"] == System.DBNull.Value) ? null : r["Metadesc"]),
						Metakey = (string)((r["Metakey"] == System.DBNull.Value) ? null : r["Metakey"]),
						Metadata = (string)((r["Metadata"] == System.DBNull.Value) ? null : r["Metadata"]),
						Images = (string)((r["Images"] == System.DBNull.Value) ? null : r["Images"]),
						Params = (string)((r["Params"] == System.DBNull.Value) ? null : r["Params"]),
						Ordering = (int)((r["Ordering"] == System.DBNull.Value) ? 0 : r["Ordering"]),
						QuantityProducts = (int)((r["QuantityProducts"] == System.DBNull.Value) ? 0 : r["QuantityProducts"]),
						Hits = (int)((r["Hits"] == System.DBNull.Value) ? 0 : r["Hits"]),
						Ids = MyModels.Encode((int)r["Id"], SecretId),
					}).FirstOrDefault();
		}

		public static List<CategoriesProducts> GetListChild(int Id)
		{

			DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_CategoriesProducts",
			new string[] { "@flag", "@Id" }, new object[] { "GetListChild", Id });
			return (from r in tabl.AsEnumerable()
					select new CategoriesProducts
					{
						Id = (int)r["Id"],
						Title = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
						Alias = (string)((r["Alias"] == System.DBNull.Value) ? null : r["Alias"]),
						Code = (string)((r["Code"] == System.DBNull.Value) ? null : r["Code"]),
						Introtext = (string)((r["Introtext"] == System.DBNull.Value) ? null : r["Introtext"]),
						Description = (string)((r["Description"] == System.DBNull.Value) ? null : r["Description"]),
						QuantityProducts = (int)((r["QuantityProducts"] == System.DBNull.Value) ? 0 : r["QuantityProducts"]),
						ParentId = (int?)((r["ParentId"] == System.DBNull.Value) ? null : r["ParentId"]),
						Featured = (Boolean)((r["Featured"] == System.DBNull.Value) ? false : r["Featured"]),
						Status = (Boolean)r["Status"],
						// FeaturedHome = (Boolean)((r["FeaturedHome"] == System.DBNull.Value) ? false : r["FeaturedHome"]),
						Metadesc = (string)((r["Metadesc"] == System.DBNull.Value) ? null : r["Metadesc"]),
						Metakey = (string)((r["Metakey"] == System.DBNull.Value) ? null : r["Metakey"]),
						Metadata = (string)((r["Metadata"] == System.DBNull.Value) ? null : r["Metadata"]),
						Images = (string)((r["Images"] == System.DBNull.Value) ? null : r["Images"]),
						Params = (string)((r["Params"] == System.DBNull.Value) ? null : r["Params"]),
						Ordering = (int)((r["Ordering"] == System.DBNull.Value) ? 0 : r["Ordering"]),
						Hits = (int)((r["Hits"] == System.DBNull.Value) ? 0 : r["Hits"]),
					}).ToList();
		}

		public static dynamic SaveItem(CategoriesProducts dto)
		{
            dto.Metadesc = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.Metadesc);
            dto.Metakey = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.Metakey);
            dto.Metadata = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.Metadata);
            dto.Title = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.Title);
            dto.Images = API.Models.MyHelper.StringHelper.RemoveTagsLink(dto.Images);
            dto.Alias = API.Models.MyHelper.StringHelper.UrlFriendly(dto.Title);

            
			if (dto.MetadataCV != null)
			{
				if (dto.MetadataCV.MetaTitle == null || dto.MetadataCV.MetaTitle == "")
				{
					dto.MetadataCV.MetaTitle = dto.Title;
					dto.MetadataCV.MetaH1 = dto.Title;
					dto.MetadataCV.MetaH3 = dto.Title;
				}
				dto.Metadata = JsonConvert.SerializeObject(dto.MetadataCV);
			}


			DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_CategoriesProducts",
			new string[] { "@flag", "@Id", "@Title", "@Alias", "@Code", "@Featured", "@Introtext", "@Description", "@ParentId", "@Status", "@Deleted", "@CreatedBy", "@ModifiedBy", "@Metadesc", "@Metakey", "@Metadata", "@Images", "@Params", "@Ordering", "@Hits" },
			new object[] { "SaveItem", dto.Id, dto.Title, dto.Alias, dto.Code, dto.Featured, dto.Introtext, dto.Description, dto.ParentId, dto.Status, dto.Deleted, dto.CreatedBy, dto.ModifiedBy, dto.Metadesc, dto.Metakey, dto.Metadata, dto.Images, dto.Params, dto.Ordering, dto.Hits });
			return (from r in tabl.AsEnumerable()
					select new
					{
						N = (int)(r["N"]),
					}).FirstOrDefault();

		}
		public static dynamic DeleteItem(CategoriesProducts dto)
		{
			DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_CategoriesProducts",
			new string[] { "@flag", "@Id", "@ModifiedBy" },
			new object[] { "DeleteItem", dto.Id, dto.ModifiedBy });
			return (from r in tabl.AsEnumerable()
					select new
					{
						N = (int)(r["N"]),
					}).FirstOrDefault();

		}

		public static dynamic UpdateStatus(CategoriesProducts dto)
		{
			DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_CategoriesProducts",
			new string[] { "@flag", "@Id", "@Status", "@ModifiedBy" },
			new object[] { "UpdateStatus", dto.Id, dto.Status, dto.ModifiedBy });
			return (from r in tabl.AsEnumerable()
					select new
					{
						N = (int)(r["N"]),
					}).FirstOrDefault();

		}

		public static dynamic UpdateFeatured(CategoriesProducts dto)
		{
			DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_CategoriesProducts",
			new string[] { "@flag", "@Id", "@Featured", "@ModifiedBy" },
			new object[] { "UpdateFeatured", dto.Id, dto.Featured, dto.ModifiedBy });
			return (from r in tabl.AsEnumerable()
					select new
					{
						N = (int)(r["N"]),
					}).FirstOrDefault();

		}

		public static dynamic UpdateAlias(int Id, string Alias)
		{
			DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_CategoriesProducts",
			new string[] { "@flag", "@Id", "@Alias" },
			new object[] { "UpdateAlias", Id, Alias });
			return (from r in tabl.AsEnumerable()
					select new
					{
						N = (int)(r["N"]),
					}).FirstOrDefault();

		}



	}
}
