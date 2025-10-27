using API.Models;
using API.Areas.Admin.Models.SYSParams;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
namespace API.Areas.Admin.Models.SYSParams
{
    public class SYSParamsService
    {

        public static List<SYSParams> GetListBySTRId(string IdType, string str_Id)
        {
            var tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_SYS_Params",
               new string[] { "@flag", "@IdType", "@str_Id" },
               new object[] { "GetListBySTRId", IdType, str_Id });
            return (from r in tabl.AsEnumerable()
                    select new SYSParams
                    {
                        Id = (int)r["Id"],
                        Title = (string)((r["Title"] == System.DBNull.Value) ? "" : r["Title"]),
                        Icon = (string)((r["Icon"] == System.DBNull.Value) ? "" : r["Icon"]),
                        Introtext = (string)((r["Introtext"] == System.DBNull.Value) ? "" : r["Introtext"]),
                        Alias = (string)((r["Alias"] == System.DBNull.Value) ? "" : r["Alias"]),
                        Selected = (Boolean)((r["Selected"] == System.DBNull.Value) ? false : r["Selected"]),
                    }).ToList();
        }
        public static SYSConfig GetItemConfigByHome(API.Areas.Admin.Models.DMCoQuan.DMCoQuan ItemCoQuan, HttpContext context)
        {
            SYSConfig Item = new SYSConfig();
            string StrConfigHome = context.Session.GetString("ConfigHome");

            if (StrConfigHome != null && StrConfigHome != "" && ItemCoQuan.MetadataCV.FlagSession == 1)
            {
                Item = JsonConvert.DeserializeObject<SYSConfig>(StrConfigHome);
            }
            else
            {
                Item = SYSParamsService.GetItemConfig();
                context.Session.SetString("ConfigHome",JsonConvert.SerializeObject(Item));

            }
            if(ItemCoQuan.MetadataCV.FlagSession != 1)
            {
                context.Session.Remove("ConfigHome");
            }
            return Item;
        }

        public static SYSConfig GetItemConfig()
        {
            SearchSYSParams dto = new SearchSYSParams() { IdType = "IdConfig" };
            List<SYSParams> listCauHinh = SYSParamsService.GetList(dto);
            SYSConfig itemCauHinhHeThong = new SYSConfig();
            Dictionary<string, string> dAttributes = new Dictionary<string, string>();
            for (int k = 0; k < listCauHinh.Count(); k++)
            {
                string type = listCauHinh[k].Title;
                string value = listCauHinh[k].Introtext;
                dAttributes.Add(type, value);
            }

            itemCauHinhHeThong.Email = dAttributes["Email"];
            itemCauHinhHeThong.WebsiteName = dAttributes["WebsiteName"];
            itemCauHinhHeThong.CompanyName = dAttributes["CompanyName"];
            itemCauHinhHeThong.Slogan = dAttributes["Slogan"];
            itemCauHinhHeThong.Phone = dAttributes["Phone"];
            itemCauHinhHeThong.Facebook = dAttributes["Facebook"];
            itemCauHinhHeThong.Twitter = dAttributes["Twitter"];
            itemCauHinhHeThong.Footer = dAttributes["Footer"];
            itemCauHinhHeThong.SEODescription = dAttributes["SEODescription"];
            itemCauHinhHeThong.SEOKeyword = dAttributes["SEOKeyword"];
            itemCauHinhHeThong.Contact = dAttributes["Contact"];
            itemCauHinhHeThong.Map = dAttributes["Map"];
            itemCauHinhHeThong.Youtube = dAttributes["Youtube"];
            itemCauHinhHeThong.Address = dAttributes["Address"];
            itemCauHinhHeThong.CountYTST = dAttributes["CountYTST"];
            itemCauHinhHeThong.CopyrightTitle = dAttributes["CopyrightTitle"];
            itemCauHinhHeThong.CopyrightLink = dAttributes["CopyrightLink"];
            itemCauHinhHeThong.CopyrightPhone = dAttributes["CopyrightPhone"];
            itemCauHinhHeThong.EMC = dAttributes["EMC"];
            itemCauHinhHeThong.ImgDomain = dAttributes["ImgDomain"];

            return itemCauHinhHeThong;
        }


        public static void SaveConfig(SYSConfig dto)
        {
            Dictionary<string, string> dAttributes = new Dictionary<string, string>() {
                { "Email", dto.Email},
                { "WebsiteName",dto.WebsiteName},
                { "CompanyName",dto.CompanyName},
                { "Slogan",dto.Slogan},
                { "Phone",dto.Phone},
                { "Facebook",dto.Facebook},
                { "Youtube",dto.Youtube},
                { "Twitter",dto.Twitter},
                { "SEODescription",dto.SEODescription},
                { "SEOKeyword",dto.SEOKeyword},
                { "Map",dto.Map},
                { "Contact",dto.Contact},
                { "Address",dto.Address},
                { "CopyrightPhone",dto.CopyrightPhone},
                { "CopyrightTitle",dto.CopyrightTitle},
                { "CopyrightLink",dto.CopyrightLink},
                { "Footer",dto.Footer},
                { "EMC",dto.EMC},
                { "ImgDomain",dto.ImgDomain},
            };


            foreach (var item in dAttributes)
            {
                SYSParams param = new SYSParams() { Title = item.Key, Introtext = item.Value };
                var tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_SYS_Params",
                    new string[] { "@flag", "@Title", "@Introtext" },
                    new object[] { "SaveConfig", param.Title, param.Introtext });
            }
        }

        public static dynamic SaveItemCountYTST(string Introtext)
        {
            return ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_SYS_Params",
                    new string[] { "@flag", "@Title", "@Introtext" },
                    new object[] { "SaveConfig", "CountYTST", Introtext });
        }

        public static SYSParams GetItem(string IdType, int Id)

        {

            var tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_SYS_Params",
                new string[] { "@flag", "@IdType", "@Id" },
                new object[] { "GetItem", IdType, Id });
            return (from r in tabl.AsEnumerable()
                    select new SYSParams
                    {
                        Id = (int)r["Id"],
                        Title = (string)((r["Title"] == System.DBNull.Value) ? "" : r["Title"]),
                        Icon = (string)((r["Icon"] == System.DBNull.Value) ? "" : r["Icon"]),
                        Introtext = (string)((r["Introtext"] == System.DBNull.Value) ? "" : r["Introtext"]),
                        Alias = (string)((r["Alias"] == System.DBNull.Value) ? "" : r["Alias"]),
                    }).FirstOrDefault();
        }

        public static SYSParams GetItemByAlias(SearchSYSParams dto)
        {

            var tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_SYS_Params",
                new string[] { "@flag", "@IdType", "@Alias" },
                new object[] { "GetItemByAlias", dto.IdType, dto.Alias });
            return (from r in tabl.AsEnumerable()
                    select new SYSParams
                    {
                        Id = (int)r["Id"],
                        Title = (string)((r["Title"] == System.DBNull.Value) ? "" : r["Title"]),
                        Icon = (string)((r["Icon"] == System.DBNull.Value) ? "" : r["Icon"]),
                        Introtext = (string)((r["Introtext"] == System.DBNull.Value) ? "" : r["Introtext"]),
                        Alias = (string)((r["Alias"] == System.DBNull.Value) ? "" : r["Alias"]),
                    }).FirstOrDefault();
        }

        public static List<SYSParams> GetListByHome(SearchSYSParams dto)
        {
            int status = 0;
            if (dto.Selected)
            {
                status = 1;
            }
            var tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_SYS_Params",
                new string[] { "@flag", "@IdType", "@Selected" },
                new object[] { "GetList", dto.IdType, status });
            return (from r in tabl.AsEnumerable()
                    select new SYSParams
                    {
                        Id = (int)r["Id"],
                        Title = (string)((r["Title"] == System.DBNull.Value) ? "" : r["Title"]),
                        Icon = (string)((r["Icon"] == System.DBNull.Value) ? "" : r["Icon"]),
                        Introtext = (string)((r["Introtext"] == System.DBNull.Value) ? "" : r["Introtext"]),
                        Alias = (string)((r["Alias"] == System.DBNull.Value) ? "" : r["Alias"])
                    }).ToList();
        }


        public static List<SYSParams> GetList(SearchSYSParams dto)
        {
            int status = 0;
            if (dto.Selected)
            {
                status = 1;
            }
            var tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_SYS_Params",
                new string[] { "@flag", "@IdType", "@Selected" },
                new object[] { "GetList", dto.IdType, status });
            return (from r in tabl.AsEnumerable()
                    select new SYSParams
                    {
                        Id = (int)r["Id"],
                        Title = (string)((r["Title"] == System.DBNull.Value) ? "" : r["Title"]),
                        Icon = (string)((r["Icon"] == System.DBNull.Value) ? "" : r["Icon"]),
                        Introtext = (string)((r["Introtext"] == System.DBNull.Value) ? "" : r["Introtext"]),
                        Alias = (string)((r["Alias"] == System.DBNull.Value) ? "" : r["Alias"])
                    }).ToList();
        }

        public static List<SYSParams> GetListDestinationCat(SearchSYSParams dto)
        {
            int status = 0;
            if (dto.Selected)
            {
                status = 1;
            }
            var tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_SYS_Params",
                new string[] { "@flag", "@IdType", "@Selected" },
                new object[] { "GetList", dto.IdType, status });
            return (from r in tabl.AsEnumerable()
                    select new SYSParams
                    {
                        Id = (int)r["Id"],
                        Title = (string)((r["Title"] == System.DBNull.Value) ? "" : r["Title"]),
                        Icon = (string)((r["Icon"] == System.DBNull.Value) ? "" : r["Icon"]),
                        Introtext = (string)((r["Introtext"] == System.DBNull.Value) ? "" : r["Introtext"]),
                        Alias = (string)((r["Alias"] == System.DBNull.Value) ? "" : r["Alias"])
                    }).ToList();
        }

    }
}