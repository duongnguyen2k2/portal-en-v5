using API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Areas.Admin.Models.Articles;
using API.Areas.Admin.Models.Search;
using System.Data;

namespace API.Areas.Admin.Models.Search
{
    public class SearchService
    {
        public static List<Search> GetListPagination(SearchAll dto)
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
            var tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Search",
                new string[] { "@flag", "@CurrentPage", "@ItemsPerPage", "@Keyword", "@CatId", "@IdCoQuan" },
                new object[] { "GetListPagination", dto.CurrentPage, dto.ItemsPerPage, dto.Keyword, dto.CatId,dto.IdCoQuan});
            if (tabl == null)
            {
                return new List<Search>();
            }
            else
            {
                return (from r in tabl.AsEnumerable()
                        select new Search
                        {
                            Id = (int)r["Id"],
                            Title = (string)r["Title"],
                            Alias = (string)((r["Alias"] == System.DBNull.Value) ? null : r["Alias"]),
                            CatId = (int)r["CatId"],                           
                            IntroText = (string)((r["IntroText"] == System.DBNull.Value) ? null : r["IntroText"]),
                            FullText = (string)((r["FullText"] == System.DBNull.Value) ? null : r["FullText"]),                           
                            Images = (string)((r["Images"] == System.DBNull.Value) ? null : r["Images"]),
                            CreatedDate = (DateTime?)((r["CreatedDate"] == System.DBNull.Value) ? null : r["CreatedDate"]),
                            TotalRows = (int)r["TotalRows"],
                        }).ToList();
            }
        }
    }
}
