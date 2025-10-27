using API.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace API.Areas.Admin.Models.ImportDBOld
{
    public class ImportDBOldService
    {
        public static List<ImportDBOld> GetList()
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Articles",
                new string[] { "@flag" }, new object[] { "GetList" });
            if (tabl == null)
            {
                return new List<ImportDBOld>();
            }
            else {
                return (from r in tabl.AsEnumerable()
                        select new ImportDBOld
                        {
                            Id = (int)r["Id"],
                            Title = (string)r["Title"],                                                    
                            Introtext = (string)((r["Introtext"] == System.DBNull.Value) ? "" : r["Introtext"]),

                        }).ToList();
            }
            
        }
    }
}
