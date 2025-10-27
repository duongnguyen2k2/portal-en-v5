using API.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace API.Areas.Admin.Models.EmailTemplate
{
    public class EmailTemplateService
    {
        public static EmailTemplate GetItem(int Id)
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_EmailTemplate",
            new string[] { "@flag", "@Id" }, new object[] { "GetItem", Id });
            return (from r in tabl.AsEnumerable()
                    select new EmailTemplate
                    {
                        Id = (int)((r["Id"] == System.DBNull.Value) ? 0 : r["Id"]),
                        Title = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
                        Description = (string)((r["Description"] == System.DBNull.Value) ? null : r["Description"]),                        
                    }).FirstOrDefault();
        }
    }
}
