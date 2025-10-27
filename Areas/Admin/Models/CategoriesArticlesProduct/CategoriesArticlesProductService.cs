using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using API.Areas.Admin.Models.CategoriesArticlesProduct;
using API.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace API.Areas.Admin.Models.CategoriesArticlesProduct
{
	public class CategoriesArticlesProductService
	{
		public static List<CategoriesArticlesProduct> GetListPagination(SearchCategoriesArticlesProduct dto, string SecretId)
		{
			dto.CurrentPage = dto.CurrentPage <= 0 ? 1 : dto.CurrentPage;
			dto.ItemsPerPage = dto.ItemsPerPage <= 0 ? 10 : dto.ItemsPerPage;
			dto.Keyword = dto.Keyword == null ? "" : dto.Keyword;

			var tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_CategoriesArticlesProduct",
				new string[] { "@flag", "@CurrentPage", "@ItemsPerPage", "@Keyword", "@IdCoQuan" },
				new object[] { "GetListPagination", dto.CurrentPage, dto.ItemsPerPage, dto.Keyword, dto.IdCoQuan });

			if (tabl == null)
			{
				return new List<CategoriesArticlesProduct>();
			}
			else
			{
				return (from r in tabl.AsEnumerable()
						select new CategoriesArticlesProduct
						{
							Id = (int)r["Id"],
							Title = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
							TitleRoot = (string)((r["TitleRoot"] == System.DBNull.Value) ? null : r["TitleRoot"]),
							Alias = (string)((r["Alias"] == System.DBNull.Value) ? null : r["Alias"]),
							Icon = (string)((r["Icon"] == System.DBNull.Value) ? null : r["Icon"]),
							IdParent = (int?)((r["IdParent"] == System.DBNull.Value) ? null : r["IdParent"]),
							Status = (Boolean)r["Status"],
							//FeaturedHome = (Boolean)((r["FeaturedHome"] == System.DBNull.Value) ? false : r["FeaturedHome"]),
							Hits = (int?)((r["Hits"] == System.DBNull.Value) ? null : r["Hits"]),
							Images = (string)((r["Images"] == System.DBNull.Value) ? null : r["Images"]),
							Params = (string)((r["Params"] == System.DBNull.Value) ? null : r["Params"]),
							Ordering = (int)((r["Ordering"] == System.DBNull.Value) ? 0 : r["Ordering"]),
							TotalRows = (int)((r["TotalRows"] == System.DBNull.Value) ? 0 : r["TotalRows"]),
							Ids = MyModels.Encode((int)r["Id"], SecretId),
						}).ToList();
			}
		}

		public static List<CategoriesArticlesProductMobile> GetListMobile(Boolean Selected = true, int IdCoQuan = 2)
		{
			DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_CategoriesArticlesProduct",
				new string[] { "@flag", "@Selected", "@IdCoQuan" }, 
				new object[] { "GetListMobile", Convert.ToDecimal(Selected), IdCoQuan });
			if (tabl == null)
			{
				return new List<CategoriesArticlesProductMobile>();
			}
			else
			{
				return (from r in tabl.AsEnumerable()
						select new CategoriesArticlesProductMobile
						{
							Id = (int)r["Id"],
							Title = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
							Alias = (string)((r["Alias"] == System.DBNull.Value) ? null : r["Alias"]),
							Icon = (string)((r["Icon"] == System.DBNull.Value) ? null : r["Icon"]),
							Ordering = (int)((r["Ordering"] == System.DBNull.Value) ? 0 : r["Ordering"]),
							Description = (string)((r["Description"] == System.DBNull.Value) ? null : r["Description"]),
							TotalRows = (int)r["TotalRows"],
						}).ToList();
			}
		}

		public static List<CategoriesArticlesProduct> GetListFeaturedHome(int IdCoQuan = 1)
		{
			DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_CategoriesArticlesProduct",
				new string[] { "@flag", "@IdCoQuan" }, new object[] { "GetListFeaturedHome", IdCoQuan });
			if (tabl == null)
			{
				return new List<CategoriesArticlesProduct>();
			}
			else
			{
				return (from r in tabl.AsEnumerable()
						select new CategoriesArticlesProduct
						{
							Id = (int)r["Id"],
							Icon = (string)((r["Icon"] == System.DBNull.Value) ? "" : r["Icon"]),
							Description = (string)((r["Description"] == System.DBNull.Value) ? "" : r["Description"]),
							Title = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
							Alias = (string)((r["Alias"] == System.DBNull.Value) ? null : r["Alias"]),
							IdParent = (int?)((r["IdParent"] == System.DBNull.Value) ? null : r["IdParent"]),
						}).ToList();
			}
		}

		public static List<SelectListItem> GetListItems(Boolean Selected = true, int IdParent = 0)
		{
			string sql = "GetList";
			sql = IdParent > 0 ? "GetListCatMenuChild" : sql;

			DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_CategoriesArticlesProduct",
				new string[] { "@flag", "@Selected", "@IdParent" },
				new object[] { sql, Convert.ToDecimal(Selected), IdParent });
			List<SelectListItem> ListItems = (from r in tabl.AsEnumerable()
											  select new SelectListItem
											  {
												  Value = (string)((r["Id"] == System.DBNull.Value) ? null : r["Id"].ToString()),
												  Text = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
											  }).ToList();

			ListItems.Insert(0, (new SelectListItem { Text = "Chọn Danh mục", Value = "0" }));
			return ListItems;
		}

		public static List<SelectListItem> GetListItemsCAP(Boolean check)
		{
			DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_CategoriesArticlesProduct",
				new string[] { "@flag" },
				new object[] { "GetListItemsCAP" });
			List<SelectListItem> ListItems = (from r in tabl.AsEnumerable()
											  select new SelectListItem
											  {
												  Value = (string)((r["Id"] == System.DBNull.Value) ? null : r["Id"].ToString()),
												  Text = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
												  Disabled = false,
											  }).ToList();

			ListItems.Insert(0, (new SelectListItem { Text = "Chọn Loại Bài viết", Value = "0", Disabled = check }));
			return ListItems;
		}

		public static CategoriesArticlesProduct GetItem(decimal Id, string SecretId = null, int IdCoQuan = 2)
		{

			DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_CategoriesArticlesProduct",
			new string[] { "@flag", "@Id", "@IdCoQuan" },
			new object[] { "GetItem", Id, IdCoQuan });
			return (from r in tabl.AsEnumerable()
					select new CategoriesArticlesProduct
					{
						Id = (int)r["Id"],
						Title = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
						Alias = (string)((r["Alias"] == System.DBNull.Value) ? null : r["Alias"]),
						IdParent = (int?)((r["IdParent"] == System.DBNull.Value) ? null : r["IdParent"]),
						Images = (string)((r["Images"] == System.DBNull.Value) ? null : r["Images"]),
						Description = (string)((r["Description"] == System.DBNull.Value) ? null : r["Description"]),
						Icon = (string)((r["Icon"] == System.DBNull.Value) ? null : r["Icon"]),
						FeaturedHome = (Boolean)((r["FeaturedHome"] == System.DBNull.Value) ? false : r["FeaturedHome"]),
						Metadesc = (string)((r["Metadesc"] == System.DBNull.Value) ? null : r["Metadesc"]),
						Metakey = (string)((r["Metakey"] == System.DBNull.Value) ? null : r["Metakey"]),
						Metadata = (string)((r["Metadata"] == System.DBNull.Value) ? null : r["Metadata"]),
						Params = (string)((r["Params"] == System.DBNull.Value) ? null : r["Params"]),
						Ordering = (int)((r["Ordering"] == System.DBNull.Value) ? 0 : r["Ordering"]),
						OrderingHome = (int)((r["OrderingHome"] == System.DBNull.Value) ? 0 : r["OrderingHome"]),
						Status = (Boolean)r["Status"],
						Hits = (int?)((r["Hits"] == System.DBNull.Value) ? null : r["Hits"]),
						Ids = MyModels.Encode((int)r["Id"], SecretId),
					}).FirstOrDefault();
		}

		public static CategoriesArticlesProduct GetItemByAlias(string Alias, string SecretId = null)
		{

			DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_CategoriesArticlesProduct",
			new string[] { "@flag", "@Alias" }, new object[] { "GetItemByAlias", Alias });
			return (from r in tabl.AsEnumerable()
					select new CategoriesArticlesProduct
					{
						Id = (int)r["Id"],
						Title = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
						Alias = (string)((r["Alias"] == System.DBNull.Value) ? null : r["Alias"]),
						Icon = (string)((r["Icon"] == System.DBNull.Value) ? null : r["Icon"]),
						Description = (string)((r["Description"] == System.DBNull.Value) ? null : r["Description"]),
						IdParent = (int?)((r["IdParent"] == System.DBNull.Value) ? null : r["IdParent"]),
						Status = (Boolean)r["Status"],
						//FeaturedHome = (Boolean)((r["FeaturedHome"] == System.DBNull.Value) ? false : r["FeaturedHome"]),
						Metadesc = (string)((r["Metadesc"] == System.DBNull.Value) ? null : r["Metadesc"]),
						Metakey = (string)((r["Metakey"] == System.DBNull.Value) ? null : r["Metakey"]),
						Metadata = (string)((r["Metadata"] == System.DBNull.Value) ? null : r["Metadata"]),
						Images = (string)((r["Images"] == System.DBNull.Value) ? null : r["Images"]),
						Params = (string)((r["Params"] == System.DBNull.Value) ? null : r["Params"]),
						Ordering = (int)((r["Ordering"] == System.DBNull.Value) ? 0 : r["Ordering"]),
						Hits = (int?)((r["Hits"] == System.DBNull.Value) ? null : r["Hits"]),
						Ids = MyModels.Encode((int)r["Id"], SecretId),
					}).FirstOrDefault();
		}

		public static List<CategoriesArticlesProduct> GetListChild(int Id)
		{

			DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_CategoriesArticlesProduct",
			new string[] { "@flag", "@Id" }, new object[] { "GetListChild", Id });
			return (from r in tabl.AsEnumerable()
					select new CategoriesArticlesProduct
					{
						Id = (int)r["Id"],
						Title = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
						Alias = (string)((r["Alias"] == System.DBNull.Value) ? null : r["Alias"]),
						Icon = (string)((r["Icon"] == System.DBNull.Value) ? null : r["Icon"]),
						Description = (string)((r["Description"] == System.DBNull.Value) ? null : r["Description"]),
						IdParent = (int?)((r["IdParent"] == System.DBNull.Value) ? null : r["IdParent"]),
						Status = (Boolean)r["Status"],
						Metadesc = (string)((r["Metadesc"] == System.DBNull.Value) ? null : r["Metadesc"]),
						Metakey = (string)((r["Metakey"] == System.DBNull.Value) ? null : r["Metakey"]),
						Metadata = (string)((r["Metadata"] == System.DBNull.Value) ? null : r["Metadata"]),
						Images = (string)((r["Images"] == System.DBNull.Value) ? null : r["Images"]),
						Params = (string)((r["Params"] == System.DBNull.Value) ? null : r["Params"]),
						Ordering = (int)((r["Ordering"] == System.DBNull.Value) ? 0 : r["Ordering"]),
						Hits = (int?)((r["Hits"] == System.DBNull.Value) ? null : r["Hits"]),
					}).ToList();
		}

		public static dynamic SaveItem(CategoriesArticlesProduct dto)
		{
			dto.Metadesc = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.Metadesc);
			dto.Metakey = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.Metakey);
			dto.Metadata = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.Metadata);
			dto.Title = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.Title);
			dto.Description = API.Models.MyHelper.StringHelper.RemoveTagsIntroText(dto.Description);
			dto.Images = API.Models.MyHelper.StringHelper.RemoveTagsLink(dto.Images);
			dto.Alias = API.Models.MyHelper.StringHelper.UrlFriendly(dto.Title);

			if (dto.Alias == null || dto.Alias == "")
			{
				dto.Alias = API.Models.MyHelper.StringHelper.UrlFriendly(dto.Title);
			}

			DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_CategoriesArticlesProduct",
			new string[] { "@flag", "@Id", "@Title", "@Alias", "@Description", "@IdParent", "@Status", "@Deleted", "@CreatedBy", "@ModifiedBy", "@Metadesc", "@Metakey", "@Metadata", "@Images",
				"@Params", "@Ordering", "@Hits", "@IdCoQuan", "@Icon" },
			new object[] { "SaveItem", dto.Id, dto.Title, dto.Alias, dto.Description, dto.IdParent, dto.Status, dto.Deleted, dto.CreatedBy, dto.ModifiedBy, dto.Metadesc, dto.Metakey, dto.Metadata, dto.Images,
				dto.Params, dto.Ordering, dto.Hits, dto.IdCoQuan, dto.Icon });
			return (from r in tabl.AsEnumerable()
					select new
					{
						N = (int)(r["N"]),
					}).FirstOrDefault();
		}

		public static dynamic SaveItemInfo(CategoriesArticlesProduct dto)
		{
			dto.Metadesc = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.Metadesc);
			dto.Metakey = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.Metakey);
			dto.Metadata = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.Metadata);
			dto.Title = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.Title);
			dto.Description = API.Models.MyHelper.StringHelper.RemoveTagsIntroText(dto.Description);
			dto.Images = API.Models.MyHelper.StringHelper.RemoveTagsLink(dto.Images);
			dto.Alias = API.Models.MyHelper.StringHelper.UrlFriendly(dto.Title);

			if (dto.Alias == null || dto.Alias == "")
			{
				dto.Alias = API.Models.MyHelper.StringHelper.UrlFriendly(dto.Title);
			}
			DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_CategoriesArticlesProduct",
			new string[] { "@flag", "@Id", "@Description", "@Status", "@ModifiedBy", "@Metadesc", "@Metakey", "@Metadata", "@Images", "@Ordering", "@Hits", "@FeaturedHome", "@IdCoQuan", "@Icon" },
			new object[] { "SaveItemInfo", dto.Id, dto.Description, dto.Status, dto.ModifiedBy, dto.Metadesc, dto.Metakey, dto.Metadata, dto.Images, dto.Ordering, dto.Hits, dto.FeaturedHome, dto.IdCoQuan, dto.Icon });
			return (from r in tabl.AsEnumerable()
					select new
					{
						N = (int)(r["N"]),
					}).FirstOrDefault();

		}
		public static dynamic DeleteItem(CategoriesArticlesProduct dto)
		{
			DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_CategoriesArticlesProduct",
			new string[] { "@flag", "@Id", "@ModifiedBy" },
			new object[] { "DeleteItem", dto.Id, dto.ModifiedBy });
			return (from r in tabl.AsEnumerable()
					select new
					{
						N = (int)(r["N"]),
					}).FirstOrDefault();

		}

		public static dynamic UpdateFeaturedHome(CategoriesArticlesProduct dto)
		{
			DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_CategoriesArticlesProduct",
			new string[] { "@flag", "@Id", "@IdCoQuan", "@FeaturedHome", "@ModifiedBy" },
			new object[] { "UpdateFeaturedHome", dto.Id, dto.IdCoQuan, dto.FeaturedHome, dto.ModifiedBy });
			return (from r in tabl.AsEnumerable()
					select new
					{
						N = (int)(r["N"]),
					}).FirstOrDefault();

		}

		public static dynamic UpdateStatus(CategoriesArticlesProduct dto)
		{
			DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_CategoriesArticlesProduct",
			new string[] { "@flag", "@Id", "@IdCoQuan", "@Status", "@ModifiedBy" },
			new object[] { "UpdateStatus", dto.Id, dto.IdCoQuan, dto.Status, dto.ModifiedBy });
			return (from r in tabl.AsEnumerable()
					select new
					{
						N = (int)(r["N"]),
					}).FirstOrDefault();

		}

		public static dynamic UpdateAlias(int Id, string Alias)
		{
			DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_CategoriesArticlesProduct",
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
