using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using API.Areas.Admin.Models.LogsRequest;
using API.Models;
namespace API.Areas.Admin.Models.LogsRequest
{
    public class LogsRequestService
    {
        public static List<LogsRequest> GetItemNew()
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_LogsRequest",
            new string[] { "@flag" }, new object[] { "GetItemNew" });
            return (from r in tabl.AsEnumerable()
                    select new LogsRequest
                    {
                        Id = (int)r["Id"],
                        Title = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
                        Introtext = (string)((r["Introtext"] == System.DBNull.Value) ? null : r["Introtext"]),
                        Description = (string)((r["Description"] == System.DBNull.Value) ? null : r["Description"]),
                        CreatedBy = (int)r["CreatedBy"],
                        CreatedDate = (DateTime)r["CreatedDate"],                        
                        IdCoQuan = (int)r["IdCoQuan"],
                    }).ToList();
        }

        public static LogsRequest GetItem(decimal Id, int IdCoQuan, string SecretId = null)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_LogsRequest",
            new string[] { "@flag", "@Id", "@IdCoQuan" }, new object[] { "GetItem", Id, IdCoQuan });
            return (from r in tabl.AsEnumerable()
                    select new LogsRequest
                    {
                        Id = (int)r["Id"],
                        Title = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),                       
                        Introtext = (string)((r["Introtext"] == System.DBNull.Value) ? null : r["Introtext"]),
                        Description = (string)((r["Description"] == System.DBNull.Value) ? null : r["Description"]),                        
                        CreatedBy = (int)r["CreatedBy"],
                        CreatedDate = (DateTime)r["CreatedDate"],                       
                        Ids = MyModels.Encode((int)r["Id"], SecretId),
                        IdCoQuan = (int)r["IdCoQuan"],
                    }).FirstOrDefault();
        }
        public static dynamic SaveItem(LogsRequest dto)
        {
            
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_LogsRequest",
            new string[] { "@flag","@Id","@Title","@Introtext","@Description","@CreatedBy", "@IdCoQuan" },
            new object[] { "SaveItem",dto.Id,dto.Title,dto.Introtext,dto.Description,dto.CreatedBy,dto.IdCoQuan});

            try
            {

                return (from r in tabl.AsEnumerable()
                        select new
                        {
                            N = (int)(r["N"]),
                        }).FirstOrDefault();
            }
            catch(Exception e)
            {
                return null;
            }
        }
        public static dynamic DeleteItem(LogsRequest dto)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_LogsRequest",
            new string[] { "@flag", "@Id", "@ModifiedBy" },
            new object[] { "DeleteItem", dto.Id, dto.ModifiedBy});
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }

        
    }
}
