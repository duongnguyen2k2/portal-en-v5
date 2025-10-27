using API.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace API.Areas.Admin.Models.CategoriesLanguage
{
    public class CategoriesLanguageService
    {
        public static CategoriesLanguage GetItem(int IdRoot,int TypeId,string LanguageCode, string SecretId = null)
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_CategoriesLanguage",
            new string[] { "@flag", "@IdRoot", "@TypeId" }, new object[] { "GetItem", IdRoot, TypeId , LanguageCode });
            CategoriesLanguage Item = (from r in tabl.AsEnumerable()
                                       select new CategoriesLanguage
                                       {
                                           Id = (int)r["Id"],
                                           TypeId = (int)r["TypeId"],
                                           IdRoot = (int)r["IdRoot"],
                                           Title = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
                                           Alias = (string)((r["Alias"] == System.DBNull.Value) ? null : r["Alias"]),
                                           Description = (string)((r["Description"] == System.DBNull.Value) ? null : r["Description"]),
                                           Introtext = (string)((r["Introtext"] == System.DBNull.Value) ? null : r["Introtext"]),                                          
                                           Metadesc = (string)((r["Metadesc"] == System.DBNull.Value) ? null : r["Metadesc"]),
                                           Metakey = (string)((r["Metakey"] == System.DBNull.Value) ? null : r["Metakey"]),
                                           Metadata = (string)((r["Metadata"] == System.DBNull.Value) ? null : r["Metadata"]),                                         
                                           LanguageCode = (string)((r["LanguageCode"] == System.DBNull.Value) ? "en" : r["LanguageCode"]),                                         
                                           Ids = MyModels.Encode((int)r["Id"], SecretId),
                                       }).FirstOrDefault();

            if (Item != null)
            {
                if (Item.Metadata != null)
                {
                    Item.MetadataCV = JsonConvert.DeserializeObject<API.Models.MetaData>(Item.Metadata);
                }
            }
            return Item;
        }

        public static dynamic SaveItem(CategoriesLanguage dto)
        {
            if (dto.Alias == null || dto.Alias == "")
            {
                dto.Alias = API.Models.MyHelper.StringHelper.UrlFriendly(dto.Title);
            }
            if (dto.MetadataCV != null)
            {
                if (dto.MetadataCV.MetaTitle == null || dto.MetadataCV.MetaTitle == "")
                {
                    dto.MetadataCV.MetaTitle = dto.Title;
                    dto.MetadataCV.MetaH1 = dto.Title;
                    dto.MetadataCV.MetaH3 = dto.Title;
                }
                dto.Metadata = JsonConvert.SerializeObject(dto.MetadataCV);
            }
            else {
                dto.Metadata = null;
            }
            

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_CategoriesLanguage",
            new string[] { "@flag", "@Id", "@Title", "@Alias", "@Description", "@LanguageCode", "@IdRoot", "@TypeId", "@Metadesc", "@Metakey", "@Metadata", "@Introtext" },
            new object[] { "SaveItem", dto.Id, dto.Title, dto.Alias, dto.Description, dto.LanguageCode, dto.IdRoot, dto.TypeId,dto.Metadesc, dto.Metakey, dto.Metadata,dto.Introtext });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }
    }
}
