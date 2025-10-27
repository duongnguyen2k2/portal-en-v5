using API.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;

namespace API.Areas.Admin.Models.ArticlesFestival
{
	public class ArticlesFestivalService
	{
		public static List<ArticlesFestival> GetListPagination(SearchArticlesFestival dto, string SecretId, Boolean MobileCheck)
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

			if(MobileCheck)
			{
				str_sql = "GetListPagination_Mobile";
			}
			var tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_ArticlesFestival",
				new string[] { "@flag", "@CurrentPage", "@ItemsPerPage", "@Keyword", "@Status" },
				new object[] { str_sql, dto.CurrentPage, dto.ItemsPerPage, dto.Keyword, Status });
			if (tabl == null)
			{
				return new List<ArticlesFestival>();
			}
			else
			{
				return (from r in tabl.AsEnumerable()
						select new ArticlesFestival
						{
							Id = (int)r["Id"],
							Title = (string)r["Title"],
							Alias = (string)((r["Alias"] == System.DBNull.Value) ? "" : r["Alias"]),
							Introtext = (string)((r["Introtext"] == System.DBNull.Value) ? "" : r["Introtext"]),
							Description = (string)((r["Description"] == System.DBNull.Value) ? "" : r["Description"]),
							AuthorName = (string)((r["AuthorName"] == System.DBNull.Value) ? "" : r["AuthorName"]),
							PublishUp = (DateTime)((r["PublishUp"] == System.DBNull.Value) ? DateTime.Now : r["PublishUp"]),
							Image = (string)((r["Image"] == System.DBNull.Value) ? "" : r["Image"]),
							Str_ListImage = (string)((r["Str_ListImage"] == System.DBNull.Value) ? "" : r["Str_ListImage"]),
							IdCoQuan = (int)((r["IdCoQuan"] == System.DBNull.Value) ? 0 : r["IdCoQuan"]),
							Status = (bool)r["Status"],
							Ids = MyModels.Encode((int)r["Id"], SecretId),
							TotalRows = (int)r["TotalRows"],
						}).ToList();
			}
		}

		public static List<ArticlesFestival> GetList(Boolean Selected = true)
		{
			DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_ArticlesFestival",
				new string[] { "@flag", "@Selected" }, new object[] { "GetList", Selected });
			if (tabl == null)
			{
				return new List<ArticlesFestival>();
			}
			else
			{
				return (from r in tabl.AsEnumerable()
						select new ArticlesFestival
						{
							Id = (int)r["Id"],
							Title = (string)r["Title"],
							Image = (string)((r["Image"] == System.DBNull.Value) ? "" : r["Image"]),
						}).ToList();
			}
		}

		public static List<SelectListItem> GetListSelectItems(Boolean Selected = true)
		{
			DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_ArticlesFestival",
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

		public static ArticlesFestival GetItem(int Id, string SecretId = null, int OCOP = 0)
		{
			DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_ArticlesFestival",
			new string[] { "@flag", "@Id" }, new object[] { "GetItem", Id });
			ArticlesFestival Item = (from r in tabl.AsEnumerable()
									 select new ArticlesFestival
									 {
										 Id = (int)r["Id"],
										 Title = (string)r["Title"],
										 Alias = (string)((r["Alias"] == System.DBNull.Value) ? "" : r["Alias"]),
										 Introtext = (string)((r["Introtext"] == System.DBNull.Value) ? "" : r["Introtext"]),
										 Description = (string)((r["Description"] == System.DBNull.Value) ? "" : r["Description"]),
										 AuthorName = (string)((r["AuthorName"] == System.DBNull.Value) ? "" : r["AuthorName"]),
										 Image = (string)((r["Image"] == System.DBNull.Value) ? "" : r["Image"]),
										 Str_ListImage = (string)((r["ListImage"] == System.DBNull.Value) ? "" : r["ListImage"]),
										 PublishUp = (DateTime)((r["PublishUp"] == System.DBNull.Value) ? DateTime.Now : r["PublishUp"]),
										 PublishUpShow = (string)((r["PublishUp"] == System.DBNull.Value) ? DateTime.Now.ToString("dd/MM/yyyy") : (string)((DateTime)r["PublishUp"]).ToString("dd/MM/yyyy")),
										 Metadesc = (string)((r["Metadesc"] == System.DBNull.Value) ? "" : r["Metadesc"]),
										 Metakey = (string)((r["Metakey"] == System.DBNull.Value) ? "" : r["Metakey"]),
										 Metadata = (string)((r["Metadata"] == System.DBNull.Value) ? "" : r["Metadata"]),
										 IdCoQuan = (int)((r["IdCoQuan"] == System.DBNull.Value) ? 0 : r["IdCoQuan"]),
										 Status = (Boolean)((r["Status"] == System.DBNull.Value) ? false : r["Status"]),
										 Ids = MyModels.Encode((int)r["Id"], SecretId),
									 }).FirstOrDefault();

			if (Item != null)
			{
				if (Item.Str_ListImage != null && Item.Str_ListImage != "")
				{
					Item.ListImage = JsonConvert.DeserializeObject<List<ImageFile>>(Item.Str_ListImage);
					Item.ImageCount = Item.ListImage.Count();
				}
			}

			return Item;
		}

		public static dynamic SaveItem(ArticlesFestival dto)
		{
            System.Text.RegularExpressions.Regex rRemScript = new System.Text.RegularExpressions.Regex(@"<script[^>]*>[\s\S]*?</script>");
            dto.Title = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.Title);
            dto.Introtext = API.Models.MyHelper.StringHelper.RemoveTagsIntroText(dto.Introtext);
            dto.Description = API.Models.MyHelper.StringHelper.RemoveTagsFullText(dto.Description);
            dto.Alias = API.Models.MyHelper.StringHelper.UrlFriendly(dto.Title);

            dto.Metadesc = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.Metadesc);
            dto.Metakey = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.Metakey);
            dto.Metadata = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.Metadata);
            dto.AuthorName = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.AuthorName);

            dto.Image = API.Models.MyHelper.StringHelper.RemoveTagsLink(dto.Image);

            List<ImageFile> images = new List<ImageFile>();
			String ListImageString = "";

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
			DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_ArticlesFestival",
			new string[] { "@flag", "@Id", "@Title", "@Alias", "@IntroText", "@Description", "@AuthorName", "@Image", "@ListImage", "@PublishUp", "@Metadesc", "@Metakey", "@Metadata", "@CreatedBy",
				"@ModifiedBy", "@IdCoQuan", "@Status", },
			new object[] { "SaveItem", dto.Id, dto.Title, dto.Alias, dto.Introtext, dto.Description, dto.AuthorName, dto.Image, ListImageString, NgayDang, dto.Metadesc, dto.Metakey, dto.Metadata,  dto.CreatedBy,
				dto.ModifiedBy, dto.IdCoQuan, dto.Status });
			return (from r in tabl.AsEnumerable()
					select new
					{
						N = (int)(r["N"]),
					}).FirstOrDefault();
		}

		public static dynamic UpdateStatus(ArticlesFestival dto)
		{
			DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_ArticlesFestival",
			new string[] { "@flag", "@Id", "@Status", "@ModifiedBy" },
			new object[] { "UpdateStatus", dto.Id, dto.Status, dto.ModifiedBy });
			return (from r in tabl.AsEnumerable()
					select new
					{
						N = (int)(r["N"]),
					}).FirstOrDefault();

		}

		public static dynamic DeleteItem(ArticlesFestival dto)
		{
			DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_ArticlesFestival",
			new string[] { "@flag", "@Id", "@ModifiedBy" },
			new object[] { "DeleteItem", dto.Id, dto.ModifiedBy });
			return (from r in tabl.AsEnumerable()
					select new
					{
						N = (int)(r["N"]),
					}).FirstOrDefault();
		}
	}
}
