using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API.Areas.Admin.Models.Question;
using API.Areas.Admin.Models.Survey;
using API.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace API.Areas.Admin.Models.Survey
{
    public class SurveyService
    {
        

        public static List<Survey> GetListPagination(SearchSurvey dto, string SecretId)
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
            var tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Survey",
                new string[] { "@flag", "@CurrentPage", "@ItemsPerPage", "@Keyword","@IdCoQuan" },
                new object[] { "GetListPagination", dto.CurrentPage, dto.ItemsPerPage, dto.Keyword,dto.IdCoQuan });
            if (tabl == null)
            {
                return new List<Survey>();
            }
            else
            {
                return (from r in tabl.AsEnumerable()
                        select new Survey
                        {
                            Id = (int)r["Id"],
                            Title = (string)r["Title"],
                            Status = (bool)r["Status"],
                            Description = (string)((r["Description"] == System.DBNull.Value) ? "" : r["Description"]),
                            Ordering = (int)((r["Ordering"] == System.DBNull.Value) ? 0 : r["Ordering"]),
                            Ids = MyModels.Encode((int)r["Id"], SecretId),
                            TotalRows = (int)r["TotalRows"],
                        }).ToList();
            }


        }

        public static List<Survey> GetList(string SecretId)
        {
            
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Survey",
                new string[] { "@flag" }, new object[] { "GetList" });
            return (from r in tabl.AsEnumerable()
                    select new Survey
                    {
                        Id = (int)r["Id"],
                        Title = (string)r["Title"],                        
                        Description = (string)((r["Description"] == System.DBNull.Value) ? "" : r["Description"])   ,
                        Ids = MyModels.Encode((int)r["Id"], SecretId),
                    }).ToList();

        }

        public static Survey GetSurveyHome(string SecretId, int IdCoQuan)
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Survey",
                new string[] { "@flag", "@IdCoQuan" }, new object[] { "GetList", IdCoQuan });

            Survey DetailItem = (from r in tabl.AsEnumerable()
                    select new Survey
                    {
                        Id = (int)r["Id"],
                        Title = (string)r["Title"],
                        
                        Ids = MyModels.Encode((int)r["Id"], SecretId),
                    }).FirstOrDefault();

            List<Question.Question> ListItemsQuestion = QuestionService.GetList();
            
            if (ListItemsQuestion != null && ListItemsQuestion.Count() > 0)
            {
                if (DetailItem != null && DetailItem.Id > 0)
                {
                    for (int j = 0; j < ListItemsQuestion.Count(); j++)
                    {
                        if (DetailItem.Id == ListItemsQuestion[j].SurveyId)
                        {
                            if (DetailItem.ListQuestion == null)
                            {
                                DetailItem.ListQuestion = new List<Question.Question>();
                            }
                            DetailItem.ListQuestion.Add(ListItemsQuestion[j]);

                        }
                    }
                }
                
            }
            return DetailItem;

        }

        public static List<SelectListItem> GetListSelectItems(Boolean Selected = true)
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Survey",
                new string[] { "@flag", "@Selected" }, new object[] { "GetList", Convert.ToDecimal(Selected) });
            List<SelectListItem> ListItems = (from r in tabl.AsEnumerable()
                                              select new SelectListItem
                                              {
                                                  Value = (string)((r["Id"] == System.DBNull.Value) ? null : r["Id"].ToString()),
                                                  Text = (string)((r["Title"] == System.DBNull.Value) ? "" : r["Title"]),
                                              }).ToList();
            ListItems.Insert(0, (new SelectListItem { Text = "--- Chọn Khảo sát---", Value = "0" }));
            return ListItems;

        }

        public static Survey GetItem(int Id, string SecretId = null)
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Survey",
            new string[] { "@flag", "@Id" }, new object[] { "GetItem", Id });
            return (from r in tabl.AsEnumerable()
                    select new Survey
                    {
                        Id = (int)r["Id"],
                        Title = (string)r["Title"],
                        Status = (bool)r["Status"],
                        Description = (string)((r["Description"] == System.DBNull.Value) ? "" : r["Description"]),                        
                        IdCoQuan = (int)((r["IdCoQuan"] == System.DBNull.Value) ? 0: r["IdCoQuan"]),
                        Ordering = (int)((r["Ordering"] == System.DBNull.Value) ? 0 : r["Ordering"]),
                        Ids = MyModels.Encode((int)r["Id"], SecretId),
                    }).FirstOrDefault();
        }

        public static dynamic SaveItem(Survey dto)
        {
            
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Survey",
            new string[] { "@flag","@Id", "@Title", "@Status", "@Description", "@IdCoQuan", "@CreatedBy", "@ModifiedBy", "@Ordering" },
            new object[] { "SaveItem", dto.Id, dto.Title, dto.Status, dto.Description, dto.IdCoQuan,dto.CreatedBy,dto.ModifiedBy,dto.Ordering });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }

        public static dynamic UpdateStatus(Survey dto)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Survey",
            new string[] { "@flag", "@Id","@Status", "@ModifiedBy" },
            new object[] { "UpdateStatus", dto.Id,dto.Status, dto.ModifiedBy });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }

        public static dynamic DeleteItem(Survey dto)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Survey",
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
