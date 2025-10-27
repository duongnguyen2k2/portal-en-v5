
using System.Data;
using System.Linq;
using API.Models;
namespace API.Areas.Admin.Models.SystemLog
{
    public class SystemLogService
    {        
        public static dynamic SaveItem(SystemLog dto)
        {
           
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_System_Log",
            new string[] { "@flag","@Id", "@Title","@Description",  "@CreatedBy", "@IdCoQuan" },
            new object[] { "SaveItem", dto.Id, dto.Title, dto.Description, dto.CreatedBy,dto.IdCoQuan });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }


    }
}
