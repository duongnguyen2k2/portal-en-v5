using API.Areas.Image.Models;
using API.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace API.Areas.Admin.Models.Liferay
{
    public class LiferayService
    {
        public static Z_Image GetItemZImage(int imageid)
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Liferay",
            new string[] { "@flag", "@imageid" }, new object[] { "GetZImage", imageid });
            return (from r in tabl.AsEnumerable()
                    select new Z_Image
                    {
                        imageid = (int)r["imageid"],
                        type_ = (string)r["type_"],                       
                    }).FirstOrDefault();
        }

        public static Z_Dlfileentry GetItemDlfileentry(string Title)
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Liferay",
            new string[] { "@flag", "@Title" }, new object[] { "GetItemDlfileentry", Title });
            return (from r in tabl.AsEnumerable()
                    select new Z_Dlfileentry
                    {
                        folderid = (int)r["folderid"],
                        companyid = (int)r["companyid"],
                        mimetype = (string)r["mimetype"],
                        name = (string)r["name"],
                        version = (string)r["version"],
                    }).FirstOrDefault();
        }

    }
}
