using API.Models;
using System.Data;
using System.Linq;
using Newtonsoft.Json;
namespace API.Areas.Admin.Models.DMCoQuanLanguage
{
    public class DMCoQuanLanguageService
    {
        public static dynamic SaveItem(DMCoQuan.DMCoQuan dto)
        {
            

            if (dto.MetadataCV.Placename == null || dto.MetadataCV.Placename.Trim() == "")
            {
                dto.MetadataCV.Placename = dto.Title;              
            }

            if (dto.MetadataCV != null)
            {
                dto.Metadata = JsonConvert.SerializeObject(dto.MetadataCV);
            }

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "DanhMuc_CoQuan",
            new string[] { "@flag", "@Title",  "@Id", "@LanguageCode", "@Description", "@Icon", "@Address", "@Telephone", "@CreatedBy", "@ModifiedBy", "@CategoryId", "ParentId", "@Metadesc", "@Metakey", "@Fax", "@Images", "@Email", "@CompanyName", "@Slogan", "@Facebook", "@Twitter", "@Youtube", "@CssName" , "@PositionBannerHome" , "@DataHeader", "@CodeMotCua", "@CodeVanBan", "@CodeLCT", "@CTNC", "@Metadata"},
            new object[] { "SaveItemLanguage", dto.Title,dto.Id,dto.Culture, dto.Description, dto.Icon, dto.Address, dto.Telephone, dto.CreatedBy, dto.ModifiedBy, dto.CategoryId, dto.ParentId, dto.Metadesc, dto.Metadesc, dto.Fax, dto.Images,dto.Email,dto.CompanyName,dto.Slogan,dto.Facebook,dto.Twitter,dto.Youtube,dto.CssName,dto.PositionBannerHome,dto.DataHeader, dto.CodeMotCua, dto.CodeVanBan,dto.CodeLCT, dto.CTNC,dto.Metadata});
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }

       

    }
}
