using System.Collections.Generic;
using System;
using System.Data;
using System.Linq;
using API.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace API.Areas.Admin.Models.USMenu
{
	public class USMenuService
	{
		public static List<USMenu> GetListPagination(SearchUSMenu dto, string SecretId)
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
			var tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "user_Menu",
				new string[] { "@flag", "@Keyword" },
				new object[] { "GetListPagination", dto.Keyword });
			if (tabl == null)
			{
				return new List<USMenu>();
			}
			else
			{
				return (from r in tabl.AsEnumerable()
						select new USMenu
						{
							Id = (int)r["Id"],
							Title = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
							PathName = (string)((r["PathName"] == System.DBNull.Value) ? null : r["PathName"]),
							Styles = (string)((r["Styles"] == System.DBNull.Value) ? "" : r["Styles"]),
							Icon = (string)((r["Icon"] == System.DBNull.Value) ? "" : r["Icon"]),
							TreeMenu = (string)((r["TreeMenu"] == System.DBNull.Value) ? null : r["TreeMenu"]),
							TreePath = (string)((r["TreePath"] == System.DBNull.Value) ? null : r["TreePath"]),
							Areas = (string)((r["Areas"] == System.DBNull.Value) ? null : r["Areas"]),
							Controller = (string)((r["Controller"] == System.DBNull.Value) ? null : r["Controller"]),
							SortOrder = (int)r["SortOrder"],
							IdParent = (int)r["IdParent"],
							Status = (Boolean)r["Status"],
							ShowMenu = (Boolean)r["ShowMenu"],
							Ids = MyModels.Encode((int)r["Id"], SecretId),
						}).ToList();
			}


		}


		public static List<USMenu> GetUSMenuByGroups(int GroupId)
		{
			DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "user_menu",
				new string[] { "@flag", "@GroupId" },
				new object[] { "GetUSMenuByGroups", GroupId });
			List<USMenu> ListItems = (from r in tabl.AsEnumerable()
									  select new USMenu
									  {
										  Id = (int)r["Id"],
										  Title = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
										  PathName = (string)((r["PathName"] == System.DBNull.Value) ? null : r["PathName"]),
										  Controller = (string)((r["Controller"] == System.DBNull.Value) ? null : r["Controller"]),
										  Areas = (string)((r["Areas"] == System.DBNull.Value) ? null : r["Areas"]),
										  IdParent = (int)r["IdParent"],
										  ChildCount = (int)r["ChildCount"],
										  Styles = (string)((r["Styles"] == System.DBNull.Value) ? null : r["Styles"]),
										  SortOrder = (int)r["SortOrder"],
										  Status = (Boolean)r["Status"],
										  ShowMenu = (Boolean)((r["ShowMenu"] == System.DBNull.Value) ? false : r["ShowMenu"]),
										  Icon = (string)((r["Icon"] == System.DBNull.Value) ? null : r["Icon"]),
									  }).ToList();


			return ListItems;
		}

		public static List<USMenuPermission> GetListPermission(string strMenu, int IdUser = 0)
		{
			List<int> ListItems = new List<int>();
			if (!string.IsNullOrEmpty(strMenu))
			{
				ListItems = strMenu.Split(',').Select(int.Parse).ToList();
			}

			DataTable tbItem = new DataTable();
			tbItem.Columns.Add("Id", typeof(int));

			if (ListItems != null && ListItems.Count() > 0)
			{
				foreach (int i in ListItems)
				{
					var row = tbItem.NewRow();
					row["Id"] = i;
					tbItem.Rows.Add(row);
				}


				DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "User_Menu",
					new string[] { "@flag", "@TBL_ListId" }, new object[] { "GetListPermission", tbItem });
				if (tabl == null)
				{
					return new List<USMenuPermission>();
				}
				else
				{
					List<USMenuPermission> ListItemsMenus = (from r in tabl.AsEnumerable()
															 select new USMenuPermission
															 {
																 Id = (int)r["Id"],
																 Title = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
																 PathName = (string)((r["PathName"] == System.DBNull.Value) ? null : r["PathName"]),
																 Controller = (string)((r["Controller"] == System.DBNull.Value) ? null : r["Controller"]),
																 Areas = (string)((r["Areas"] == System.DBNull.Value) ? null : r["Areas"]),
																 IdParent = (int)r["IdParent"],
																 Styles = (string)((r["Styles"] == System.DBNull.Value) ? null : r["Styles"]),
																 Icon = (string)((r["Icon"] == System.DBNull.Value) ? null : r["Icon"]),
																 ShowMenu = (Boolean)r["ShowMenu"],
															 }).ToList();
					if (IdUser == 1)
					{
						ListItemsMenus.Add(new USMenuPermission()
						{
							Id = 1999999,
							Title = "Nhóm quyền",
							PathName = "/Admin/USGroups",
							Controller = "USGroups",
							Areas = "Admin",
							IdParent = 0,
							ShowMenu = true,
							Styles = "",
							Icon = "fas fa-users-cog",
						});
						ListItemsMenus.Add(new USMenuPermission()
						{
							Id = 19999991,
							Title = "Chức năng",
							PathName = "/Admin/USMenu",
							Controller = "USMenu",
							Areas = "Admin",
							IdParent = 0,
							ShowMenu = true,
							Styles = "",
							Icon = "fas fa-wrench",
						});
					}
					return ListItemsMenus;
				}
			}
			else
			{
				return new List<USMenuPermission>(); ;
			}

		}

		public static List<USMenu> GetList(Boolean Selected = true)
		{

			DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "User_Menu",
				new string[] { "@flag", "@Selected" }, new object[] { "GetList", Convert.ToDecimal(Selected) });
			if (tabl == null)
			{
				return new List<USMenu>();
			}
			else
			{
				return (from r in tabl.AsEnumerable()
						select new USMenu
						{
							Id = (int)r["Id"],
							Title = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
							PathName = (string)((r["PathName"] == System.DBNull.Value) ? null : r["PathName"]),
							Controller = (string)((r["Controller"] == System.DBNull.Value) ? null : r["Controller"]),
							Areas = (string)((r["Areas"] == System.DBNull.Value) ? null : r["Areas"]),
							IdParent = (int)r["IdParent"],
							Styles = (string)((r["Styles"] == System.DBNull.Value) ? null : r["Styles"]),
							Icon = (string)((r["Icon"] == System.DBNull.Value) ? null : r["Icon"]),
							SortOrder = (int)r["SortOrder"],
							Status = (Boolean)r["Status"],
							ShowMenu = (Boolean)r["ShowMenu"],
						}).ToList();
			}

		}


		public static List<SelectListItem> GetListItems(Boolean Selected = true, int IdParent = 0)
		{

			DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "User_Menu",
				new string[] { "@flag", "@Selected", "@IdParent" }, new object[] { "GetList", Convert.ToDecimal(Selected), IdParent });
			List<SelectListItem> ListItems = (from r in tabl.AsEnumerable()
											  select new SelectListItem
											  {
												  Value = (string)((r["Id"] == System.DBNull.Value) ? null : r["Id"].ToString()),
												  Text = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
											  }).ToList();

			ListItems.Insert(0, (new SelectListItem { Text = "Root", Value = "0" }));
			return ListItems;

		}

		public static USMenu GetItem(decimal Id, string SecretId = null)
		{

			DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "User_Menu",
			new string[] { "@flag", "@Id" }, new object[] { "GetItem", Id });
			return (from r in tabl.AsEnumerable()
					select new USMenu
					{
						Id = (int)r["Id"],
						Title = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
						PathName = (string)((r["PathName"] == System.DBNull.Value) ? null : r["PathName"]),
						IdParent = (int)r["IdParent"],
						Styles = (string)((r["Styles"] == System.DBNull.Value) ? null : r["Styles"]),
						Icon = (string)((r["Icon"] == System.DBNull.Value) ? null : r["Icon"]),
						Description = (string)((r["Description"] == System.DBNull.Value) ? null : r["Description"]),
						Areas = (string)((r["Areas"] == System.DBNull.Value) ? null : r["Areas"]),
						Controller = (string)((r["Controller"] == System.DBNull.Value) ? null : r["Controller"]),
						SortOrder = (int)r["SortOrder"],
						Status = (Boolean)r["Status"],
						ShowMenu = (Boolean)r["ShowMenu"],
						CreatedBy = (int?)((r["CreatedBy"] == System.DBNull.Value) ? null : r["CreatedBy"]),
						CreatedDate = (DateTime?)((r["CreatedDate"] == System.DBNull.Value) ? null : r["CreatedDate"]),
						ModifiedBy = (int?)((r["ModifiedBy"] == System.DBNull.Value) ? null : r["ModifiedBy"]),
						ModifiedDate = (DateTime?)((r["ModifiedDate"] == System.DBNull.Value) ? null : r["ModifiedDate"]),
						Deleted = (Boolean)r["Deleted"],
						Ids = MyModels.Encode((int)r["Id"], SecretId),
					}).FirstOrDefault();
		}

		public static dynamic SaveItem(USMenu dto)
		{
			dto.Title = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.Title);
			dto.Icon = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.Icon);
			dto.Areas = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.Areas);
			dto.Controller = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.Controller);
			dto.PathName = API.Models.MyHelper.StringHelper.RemoveTagsLink(dto.PathName);
			dto.Description = API.Models.MyHelper.StringHelper.RemoveTagsIntroText(dto.Description);

			DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "user_menu",
			new string[] { "@flag", "@Id", "@Title", "@PathName", "@IdParent", "@Styles", "@Icon", "@Description", "@Areas", "@Controller", "@SortOrder", "@Status", "@ShowMenu", "@CreatedBy", "@CreatedDate", "@ModifiedBy", "@ModifiedDate", "@Deleted" },
			new object[] { "SaveItem", dto.Id, dto.Title, dto.PathName, dto.IdParent, dto.Styles, dto.Icon, dto.Description, dto.Areas, dto.Controller, dto.SortOrder, dto.Status, dto.ShowMenu, dto.CreatedBy, dto.CreatedDate, dto.ModifiedBy, dto.ModifiedDate, dto.Deleted });
			return (from r in tabl.AsEnumerable()
					select new
					{
						N = (int)(r["N"]),
					}).FirstOrDefault();

		}
		public static dynamic UpdateStatus(USMenu dto)
		{
			DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "User_Menu",
			new string[] { "@flag", "@Id", "@Status", "@ModifiedBy" },
			new object[] { "UpdateStatus", dto.Id, dto.Status, dto.ModifiedBy });
			return (from r in tabl.AsEnumerable()
					select new
					{
						N = (int)(r["N"]),
					}).FirstOrDefault();
		}
		public static dynamic DeleteItem(USMenu dto)
		{
			DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "User_Menu",
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
