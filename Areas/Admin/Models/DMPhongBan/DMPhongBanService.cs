using API.Models;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;

namespace API.Areas.Admin.Models.DMPhongBan
{
	public class DMPhongBanService
	{


		public static List<DMPhongBan> GetListPagination(SearchDMPhongBan dto, string SecretId)
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
			var tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "DanhMuc_PhongBan",
				new string[] { "@flag", "@CurrentPage", "@ItemsPerPage", "@Keyword", "@IdType", "@IdCoQuan" },
				new object[] { "GetListPagination", dto.CurrentPage, dto.ItemsPerPage, dto.Keyword, dto.IdType, dto.IdCoQuan });
			if (tabl == null)
			{
				return new List<DMPhongBan>();
			}
			else
			{
				return (from r in tabl.AsEnumerable()
						select new DMPhongBan
						{
							Id = (int)r["Id"],
							Title = (string)r["Title"],
							Alias = (string)r["Alias"],
							Status = (bool)r["Status"],
							Description = (string)((r["Description"] == System.DBNull.Value) ? "" : r["Description"]),
							Images = (string)((r["Images"] == System.DBNull.Value) ? "" : r["Images"]),
							Ids = MyModels.Encode((int)r["Id"], SecretId),
							IdType = (int)((r["IdType"] == System.DBNull.Value) ? 0 : r["IdType"]),
                            Ordering = (int)((r["Ordering"] == System.DBNull.Value) ? 0 : r["Ordering"]),
							TypeTitle = (string)((r["TypeTitle"] == System.DBNull.Value) ? null : r["TypeTitle"]),
							Email = (string)((r["Email"] == System.DBNull.Value) ? null : r["Email"]),
							Address = (string)((r["Address"] == System.DBNull.Value) ? "" : r["Address"]),
							Telephone = (string)((r["Telephone"] == System.DBNull.Value) ? "" : r["Telephone"]),
							Fax = (string)((r["Fax"] == System.DBNull.Value) ? "" : r["Fax"]),
							Link = (string)((r["Link"] == System.DBNull.Value) ? "" : r["Link"]),
							Youtube = (string)((r["Youtube"] == System.DBNull.Value) ? "" : r["Youtube"]),
							TotalRows = (int)r["TotalRows"],
						}).ToList();
			}


		}

		public static DataTable GetList(Boolean Selected = true, int IdType = 0, int IdCoQuan = 0)
		{

			DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "DanhMuc_PhongBan",
				new string[] { "@flag", "@Selected", "@IdType", "@IdCoQuan" },
				new object[] { "GetList", Selected, IdType, IdCoQuan });
			return tabl;

		}

		public static DataTable GetListAPI(Boolean Selected = true, int IdType = 0, int IdCoQuan = 0)
		{

			DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "DanhMuc_PhongBan",
				new string[] { "@flag", "@Selected", "@IdType", "@IdCoQuan" },
				new object[] { "GetListAPI", Selected, IdType, IdCoQuan });
			return tabl;

		}

		public static List<SelectListItem> GetListSelectItems(Boolean Selected = true)
		{

			DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "DanhMuc_PhongBan",
				new string[] { "@flag", "@Selected" }, new object[] { "GetList", Convert.ToDecimal(Selected) });
			List<SelectListItem> ListItems = (from r in tabl.AsEnumerable()
											  select new SelectListItem
											  {
												  Value = (string)((r["Id"] == System.DBNull.Value) ? null : r["Id"].ToString()),
												  Text = (string)((r["Title"] == System.DBNull.Value) ? "" : r["Title"]),
											  }).ToList();
			ListItems.Insert(0, (new SelectListItem { Text = "--- Chọn Danh mục ---", Value = "0" }));
			return ListItems;

		}

		public static List<SelectListItem> GetListSelectItemsByType(int IdType = 0, int IdCoQuan = 0)
		{

			DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "DanhMuc_PhongBan",
				new string[] { "@flag", "@IdType", "@IdCoQuan" }, new object[] { "GetListAPI", IdType, IdCoQuan });
			List<SelectListItem> ListItems = (from r in tabl.AsEnumerable()
											  select new SelectListItem
											  {
												  Value = (string)((r["Id"] == System.DBNull.Value) ? null : r["Id"].ToString()),
												  Text = (string)((r["Title"] == System.DBNull.Value) ? "" : r["Title"]),
											  }).ToList();
			ListItems.Insert(0, (new SelectListItem { Text = "--- Chọn Danh mục ---", Value = "0" }));
			return ListItems;

		}

		public static DMPhongBan GetItem(int Id, string SecretId = null)
		{

			DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "DanhMuc_PhongBan",
			new string[] { "@flag", "@Id" }, new object[] { "GetItem", Id });
			return (from r in tabl.AsEnumerable()
					select new DMPhongBan
					{
						Id = (int)r["Id"],
						Title = (string)r["Title"],
						Alias = (string)r["Alias"],
						Status = (bool)r["Status"],
						IdType = (int)((r["IdType"] == System.DBNull.Value) ? 0 : r["IdType"]),
						TypeTitle = (string)((r["TypeTitle"] == System.DBNull.Value) ? null : r["TypeTitle"]),
                        Ordering = (int)((r["Ordering"] == System.DBNull.Value) ? 0 : r["Ordering"]),
                        Description = (string)((r["Description"] == System.DBNull.Value) ? "" : r["Description"]),
						Images = (string)((r["Images"] == System.DBNull.Value) ? "" : r["Images"]),
						Email = (string)((r["Email"] == System.DBNull.Value) ? null : r["Email"]),
						Address = (string)((r["Address"] == System.DBNull.Value) ? "" : r["Address"]),
						Telephone = (string)((r["Telephone"] == System.DBNull.Value) ? "" : r["Telephone"]),
						Fax = (string)((r["Fax"] == System.DBNull.Value) ? "" : r["Fax"]),
						Link = (string)((r["Link"] == System.DBNull.Value) ? "" : r["Link"]),
						Youtube = (string)((r["Youtube"] == System.DBNull.Value) ? "" : r["Youtube"]),
						Ids = MyModels.Encode((int)r["Id"], SecretId),
					}).FirstOrDefault();
		}

		public static dynamic SaveItem(DMPhongBan dto)
		{
            dto.Title = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.Title);
            dto.Description = API.Models.MyHelper.StringHelper.RemoveTagsIntroText(dto.Description);
            dto.Address = API.Models.MyHelper.StringHelper.RemoveTagsIntroText(dto.Address);


            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "DanhMuc_PhongBan",
			new string[] { "@flag", "@Id", "@Title", "@Alias", "@Status", "@Description", "@Fax", "@Images", "@Email", "@Address", "@Telephone", "@Youtube", "@Link", "@IdType", "@CreatedBy", "@ModifiedBy", "@IdCoQuan", "@Ordering" },
			new object[] { "SaveItem", dto.Id, dto.Title, dto.Alias, dto.Status, dto.Description, dto.Fax, dto.Images, dto.Email, dto.Address, dto.Telephone, dto.Youtube, dto.Link, dto.IdType, dto.CreatedBy, dto.ModifiedBy, dto.IdCoQuan,dto.Ordering });
			return (from r in tabl.AsEnumerable()
					select new
					{
						N = (int)(r["N"]),
					}).FirstOrDefault();

		}

		public static dynamic UpdateStatus(DMPhongBan dto)
		{
			DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "DanhMuc_PhongBan",
			new string[] { "@flag", "@Id", "@Status", "@ModifiedBy" },
			new object[] { "UpdateStatus", dto.Id, dto.Status, dto.ModifiedBy });
			return (from r in tabl.AsEnumerable()
					select new
					{
						N = (int)(r["N"]),
					}).FirstOrDefault();

		}

		public static dynamic DeleteItem(DMPhongBan dto)
		{
			DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "DanhMuc_PhongBan",
			new string[] { "@flag", "@Id", "@ModifiedBy" },
			new object[] { "DeleteItem", dto.Id, dto.ModifiedBy });
			return (from r in tabl.AsEnumerable()
					select new
					{
						N = (int)(r["N"]),
					}).FirstOrDefault();

		}

		public static List<SelectListItem> GetListTypes()
		{

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "DanhMuc_LoaiPhongBan",
                   new string[] { "@flag" }, new object[] { "GetList" });
            List<SelectListItem> ListItems = (from r in tabl.AsEnumerable()
                                              select new SelectListItem
                                              {
                                                  Value = (string)((r["Id"] == System.DBNull.Value) ? null : r["Id"].ToString()),
                                                  Text = (string)((r["Title"] == System.DBNull.Value) ? "" : r["Title"]),
                                              }).ToList();
            ListItems.Insert(0, (new SelectListItem { Text = "--- Chọn Loại Danh mục ---", Value = "0" }));
            return ListItems;          

		}


	}
}
