using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using API.Areas.Admin.Models.USGroups;
using API.Areas.Admin.Models.USUsers;
using API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;


namespace API.Areas.Admin.Models.USGroups
{
    public class USGroupsService
    {
        public static List<USGroups> GetListPagination(SearchUSGroups dto, string SecretId)
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
            var tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "User_Groups",
                new string[] { "@flag", "@Keyword" },
                new object[] { "GetList", dto.Keyword });
            if (tabl == null)
            {
                return new List<USGroups>();
            }
            else
            {
                return (from r in tabl.AsEnumerable()
                        select new USGroups
                        {
                            Id = (int)r["Id"],
                            Title = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
                            ListMenuId = (string)((r["ListMenuId"] == System.DBNull.Value) ? null : r["ListMenuId"]),
                            Status = (Boolean)((r["Status"] == System.DBNull.Value) ? null : r["Status"]),
                            Ids = MyModels.Encode((int)r["Id"], SecretId),
                        }).ToList();
            }
        }

        public static List<USGroups> GetList(Boolean Selected = true)
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "User_Groups",
                new string[] { "@flag", "@Selected" }, new object[] { "GetList", Convert.ToDecimal(Selected) });
            if (tabl == null)
            {
                return new List<USGroups>();
            }
            else
            {
                return (from r in tabl.AsEnumerable()
                        select new USGroups
                        {
                            Id = (int)r["Id"],
                            Title = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
                            ListMenuId = (string)((r["ListMenuId"] == System.DBNull.Value) ? null : r["ListMenuId"]),
                            Status = (Boolean)((r["Status"] == System.DBNull.Value) ? null : r["Status"]),
                        }).ToList();
            }

        }
        public static DataTable GetListChucNang(int Id = 0)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "user_groups",
                new string[] { "@flag", "@Id" }, new object[] { "GetListGroupMenu", Id });
            return tabl;
        }
        public static List<SelectListItem> GetListSelectItems(int IdTinhThanh = 0, USUsers.USUsers MyInfo = null)
        {
            List<USGroups> ListItemsGroups = new List<USGroups>();
            List<SelectListItem> ListItems = new List<SelectListItem>();

            int IdGroup = 0;
            if (MyInfo != null)
            {
                IdGroup = MyInfo.IdGroup;
            }
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "User_Groups",
                 new string[] { "@flag", "@Selected" }, new object[] { "GetList", true });
            ListItemsGroups = (from r in tabl.AsEnumerable()
                     select new USGroups
                     {
                         Id = (int)r["Id"],
                         Title = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),                        
                     }).ToList();
            ListItems.Insert(0, (new SelectListItem { Text = "--- Chọn Nhóm Quyền ---", Value = "0" }));

            foreach (USGroups item in ListItemsGroups)
            {
                if (IdGroup == 1)
                {
                    ListItems.Add(new SelectListItem { Text = item.Title, Value = item.Id.ToString() });
                }
                else
                {
                    if(item.Id != 1)
                    {
                        ListItems.Add(new SelectListItem { Text = item.Title, Value = item.Id.ToString() });
                    }
                }
            }
            
            return ListItems;

        }

        public static USGroups GetItem(decimal Id, string SecretId = null)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "User_Groups",
            new string[] { "@flag", "@Id" }, new object[] { "GetItem", Id });
            return (from r in tabl.AsEnumerable()
                    select new USGroups
                    {
                        Id = (int)r["Id"],
                        Title = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
                        ListMenuId = (string)((r["ListMenuId"] == System.DBNull.Value) ? null : r["ListMenuId"]),
                        ListCatId = (string)((r["ListCatId"] == System.DBNull.Value) ? null : r["ListCatId"]),
                        Status = (Boolean)((r["Status"] == System.DBNull.Value) ? null : r["Status"]),
                        Ids = MyModels.Encode((int)r["Id"], SecretId),
                    }).FirstOrDefault();
        }

        public static dynamic SaveItem(USGroups dto)
        {
            dto.Title = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.Title);

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "User_Groups",
            new string[] { "@flag", "@Id", "@Title", "@ListMenuId", "@Status", "@CreatedBy", "@ModifiedBy" },
            new object[] { "SaveItem", dto.Id, dto.Title, dto.ListMenuId, dto.Status, dto.CreatedBy, dto.ModifiedBy });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }
        public static dynamic DeleteItem(USGroups dto)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "User_Groups",
            new string[] { "@flag", "@Id", "@ModifiedBy" },
            new object[] { "DeleteItem", dto.Id, dto.ModifiedBy });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }
        public static dynamic UpdateMenu(USGroups dto)
        {
            return ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "user_groups",
                new string[] { "@flag", "@Id", "@ListMenuId" },
                new object[] { "UpdateMenu", dto.Id, dto.ListMenuId });
        }

        public static dynamic UpdateCat(USGroups dto)
        {
            return ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "user_groups",
                new string[] { "@flag", "@Id", "@ListCatId", "@ListMenuId" },
                new object[] { "UpdateCat", dto.Id, dto.ListCatId, dto.ListMenuId });
        }
        public static dynamic UpdateStatus(USGroups dto)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "User_Groups",
            new string[] { "@flag", "@Id", "@Status", "@ModifiedBy" },
            new object[] { "UpdateStatus", dto.Id, dto.Status, dto.ModifiedBy });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }
    }
}
