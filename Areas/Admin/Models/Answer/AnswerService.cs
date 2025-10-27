using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API.Areas.Admin.Models.Answer;
using API.Areas.Admin.Models.DMCoQuan;
using API.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace API.Areas.Admin.Models.Answer
{
    public class AnswerService
    {
        

        public static List<Answer> GetListPagination(SearchAnswer dto, string SecretId)
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
            var tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Answer",
                new string[] { "@flag", "@CurrentPage", "@ItemsPerPage", "@Keyword", "@IdCoQuan" },
                new object[] { "GetListPagination", dto.CurrentPage, dto.ItemsPerPage, dto.Keyword,dto.IdCoQuan });
            if (tabl == null)
            {
                return new List<Answer>();
            }
            else
            {
                return (from r in tabl.AsEnumerable()
                        select new Answer
                        {
                            Id = (int)r["Id"],
                            CustomerId = (string)r["CustomerId"],
                            SurveyId = (int)r["SurveyId"],
                            QuestionId = (int)((r["QuestionId"] == System.DBNull.Value) ? 0 : r["QuestionId"]),
                            QuestionTitle = (string)((r["QuestionTitle"] == System.DBNull.Value) ? "" : r["QuestionTitle"]),
                            SurveyTitle = (string)((r["SurveyTitle"] == System.DBNull.Value) ? "" : r["SurveyTitle"]),
                            CreatedDate = (DateTime)((r["CreatedDate"] == System.DBNull.Value) ? DateTime.Now : r["CreatedDate"]),
                            Ids = MyModels.Encode((int)r["Id"], SecretId),
                            TotalRows = (int)r["TotalRows"],
                        }).ToList();
            }


        }

        public static List<Answer> GetListChart(int SurveyId,int IdCoQuan)
        {
           
            var tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Answer",
                new string[] { "@flag", "@SurveyId", "@IdCoQuan" },
                new object[] { "Chart", SurveyId, IdCoQuan });
            if (tabl == null)
            {
                return new List<Answer>();
            }
            else
            {
                return (from r in tabl.AsEnumerable()
                        select new Answer
                        {                           
                            QuestionTitle = (string)((r["QuestionTitle"] == System.DBNull.Value) ? "" : r["QuestionTitle"]),
                            SurveyTitle = (string)((r["SurveyTitle"] == System.DBNull.Value) ? "" : r["SurveyTitle"]),                            
                            TotalRows = (int)r["TotalRows"],
                        }).ToList();
            }


        }

        public static dynamic SaveItem(Answer dto)
        {
            
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Answer",
            new string[] { "@flag","@Id", "@CustomerId", "@SurveyId", "@QuestionId", "@IdCoQuan", "@CreatedBy", "@ModifiedBy" },
            new object[] { "SaveItem", dto.Id, dto.CustomerId, dto.SurveyId, dto.QuestionId,dto.IdCoQuan, dto.CreatedBy,dto.ModifiedBy });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }


        public static dynamic DeleteItem(Answer dto)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Answer",
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
