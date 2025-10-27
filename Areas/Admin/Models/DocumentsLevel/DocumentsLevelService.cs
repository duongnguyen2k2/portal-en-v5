using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using API.Areas.Admin.Models.DocumentsLevel;
using API.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace API.Areas.Admin.Models.DocumentsLevel
{
    public class DocumentsLevelService
    {

        public static List<DocumentsLevel> GetList()
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_DocumentsLevel",
                new string[] { "@flag", "@Selected" }, new object[] { "GetList", true });
            List<DocumentsLevel> ListItems = (from r in tabl.AsEnumerable()
                                              select new DocumentsLevel
                                              {
                                                  Id = (int)r["Id"],
                                                  Alias = (string)((r["Alias"] == System.DBNull.Value) ? null : r["Alias"]),
                                                  Title = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
                                              }).ToList();

            return ListItems;
        }


        public static List<SelectListItem> GetListSelectItems()
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_DocumentsLevel",
                new string[] { "@flag", "@Selected" }, new object[] { "GetList", true });
            List<SelectListItem> ListItems = (from r in tabl.AsEnumerable()
                                              select new SelectListItem
                                              {
                                                  Value = (string)((r["Id"] == System.DBNull.Value) ? null : r["Id"].ToString()),
                                                  Text = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
                                              }).ToList();

            ListItems.Insert(0, (new SelectListItem { Text = "--- Cấp Ban Hành ---", Value = "0" }));
            return ListItems;

        }

        public static DocumentsLevel GetItemByTitle(string Title = null)
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_DocumentsLevel",
            new string[] { "@flag", "@Title" }, new object[] { "GetItemByTitle", Title });
            return (from r in tabl.AsEnumerable()
                    select new DocumentsLevel
                    {
                        Id = (int)r["Id"],
                        Alias = (string)((r["Alias"] == System.DBNull.Value) ? null : r["Alias"]),
                        Title = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
                        //Description = (string)((r["Description"] == System.DBNull.Value) ? null : r["Description"]),
                        //Status = (Boolean)r["Status"],
                    }).FirstOrDefault();
        }

        public static dynamic SaveItem(DocumentsLevel dto)
        {
            dto.Title = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.Title);
            dto.Description = API.Models.MyHelper.StringHelper.RemoveTagsIntroText(dto.Description);
            dto.Alias = API.Models.MyHelper.StringHelper.UrlFriendly(dto.Title);

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_DocumentsLevel",
            new string[] { "@flag", "@Id", "@Alias", "@Title", "@Description", "@Status", "@CreatedBy", "@ModifiedBy" },
            new object[] { "SaveItem", dto.Id, dto.Alias, dto.Title, dto.Description, dto.Status, dto.CreatedBy, dto.ModifiedBy });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }




    }
}
