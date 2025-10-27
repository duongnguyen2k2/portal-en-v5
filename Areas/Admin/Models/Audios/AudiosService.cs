using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using API.Areas.Admin.Models.Audios;
using API.Areas.Admin.Models.DMCoQuan;
using API.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace API.Areas.Admin.Models.Audios
{
    public class AudiosService
    {
        public static List<Audios> GetListPagination(SearchAudios dto, string SecretId)
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

            string str_sql = "GetListPagination_Status";
            Boolean Status = true;
            if (dto.Status == -1)
            {
                str_sql = "GetListPagination";
            }
            else if (dto.Status == 0)
            {
                Status = false;
            }

            var tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Audios",
                new string[] { "@flag", "@CurrentPage", "@ItemsPerPage", "@Keyword", "@Status", "@IdCoQuan", "@CatId" },
                new object[] { str_sql, dto.CurrentPage, dto.ItemsPerPage, dto.Keyword, Status,dto.IdCoQuan,dto.CatId });
            if (tabl == null)
            {
                return new List<Audios>();
            }
            else
            {
                return (from r in tabl.AsEnumerable()
					select new Audios
					{
						Id = (int)r["Id"],
 						Title = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
                        Alias = (string)((r["Alias"] == System.DBNull.Value) ? null : r["Alias"]),
                        Description = (string)((r["Description"] == System.DBNull.Value) ? null : r["Description"]),
						PublishUp = (DateTime)((r["PublishUp"] == System.DBNull.Value) ? DateTime.Now : r["PublishUp"]),
                        PublishUpShow = (string)((r["PublishUp"] == System.DBNull.Value) ? DateTime.Now.ToString("dd/MM/yyyy") : (string)((DateTime)r["PublishUp"]).ToString("dd/MM/yyyy")),
                        CatTitle = (string)((r["CatTitle"] == System.DBNull.Value) ? "" : r["CatTitle"]),
 						Link = (string)((r["Link"] == System.DBNull.Value) ? null : r["Link"]),
 						LinkSv = (string)((r["LinkSv"] == System.DBNull.Value) ? null : r["LinkSv"]),
 						CatId = (int)((r["CatId"] == System.DBNull.Value) ? 0 : r["CatId"]),
                        Images = (string)((r["Images"] == System.DBNull.Value) ? null : r["Images"]),
 						Status = (Boolean)r["Status"],
 						Featured = (Boolean)r["Featured"], 						
						Ids = MyModels.Encode((int)r["Id"], SecretId),
						TotalRows = (int)r["TotalRows"],
					}).ToList();
            }


        }

        public static List<Audios> GetListNew(int IdCoQuan)
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Audios",
                new string[] { "@flag", "@IdCoQuan" }, new object[] { "GetListNew",IdCoQuan });
            if (tabl == null)
            {
                return new List<Audios>();
            }
            else
            {
                return (from r in tabl.AsEnumerable()
                        select new Audios
                        {
                            Id = (int)r["Id"],
                            Title = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
                            Alias = (string)((r["Alias"] == System.DBNull.Value) ? null : r["Alias"]),
                            CatTitle = (string)((r["CatTitle"] == System.DBNull.Value) ? "" : r["CatTitle"]),
                            PublishUp = (DateTime)((r["PublishUp"] == System.DBNull.Value) ? DateTime.Now : r["PublishUp"]),
                            PublishUpShow = (string)((r["PublishUp"] == System.DBNull.Value) ? DateTime.Now.ToString("dd/MM/yyyy") : (string)((DateTime)r["PublishUp"]).ToString("dd/MM/yyyy")),
                            Description = (string)((r["Description"] == System.DBNull.Value) ? null : r["Description"]),
                            Link = (string)((r["Link"] == System.DBNull.Value) ? null : r["Link"]),
                            LinkSv = (string)((r["LinkSv"] == System.DBNull.Value) ? null : r["LinkSv"]),
                            CatId = (int)((r["CatId"] == System.DBNull.Value) ? 0 : r["CatId"]),
                            Images = (string)((r["Images"] == System.DBNull.Value) ? null : r["Images"]),
                        }).ToList();
            }

        }

		public static List<Audios> GetListByCatFeaturedHome(int IdCoQuan,int CatId = 0)
		{

			DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Audios",
				new string[] { "@flag", "@CatId", "@IdCoQuan" }, new object[] { "GetListByCatFeaturedHome", CatId, IdCoQuan });
			if (tabl == null)
			{
				return new List<Audios>();
			}
			else
			{
				return (from r in tabl.AsEnumerable()
						select new Audios
						{
							Id = (int)r["Id"],
							Title = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
							Alias = (string)((r["Alias"] == System.DBNull.Value) ? null : r["Alias"]),
							CatTitle = (string)((r["CatTitle"] == System.DBNull.Value) ? "" : r["CatTitle"]),
							Description = (string)((r["Description"] == System.DBNull.Value) ? null : r["Description"]),
                            PublishUp = (DateTime)((r["PublishUp"] == System.DBNull.Value) ? DateTime.Now : r["PublishUp"]),
                            PublishUpShow = (string)((r["PublishUp"] == System.DBNull.Value) ? DateTime.Now.ToString("dd/MM/yyyy") : (string)((DateTime)r["PublishUp"]).ToString("dd/MM/yyyy")),
                            Link = (string)((r["Link"] == System.DBNull.Value) ? null : r["Link"]),
							LinkSv = (string)((r["LinkSv"] == System.DBNull.Value) ? null : r["LinkSv"]),
							CatId = (int)((r["CatId"] == System.DBNull.Value) ? 0 : r["CatId"]),
							Images = (string)((r["Images"] == System.DBNull.Value) ? null : r["Images"]),
						}).ToList();
			}

		}

		public static List<Audios> GetListRelativeNews(string Alias,int IdCoQuan, int CatId = 0)
        {
            if (Alias == null || Alias == "" || CatId == 0)
            {
                return new List<Audios>();
            }
            else
            {
                DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Audios",
                new string[] { "@flag", "@Alias", "@CatId", "@IdCoQuan" }, new object[] { "GetListRelativeNews", Alias, CatId, IdCoQuan });
                if (tabl == null)
                {
                    return new List<Audios>();
                }
                else
                {
                    return (from r in tabl.AsEnumerable()
                            select new Audios
                            {
                                Id = (int)r["Id"],
                                Title = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
                                Alias = (string)((r["Alias"] == System.DBNull.Value) ? null : r["Alias"]),
                                Description = (string)((r["Description"] == System.DBNull.Value) ? null : r["Description"]),
                                PublishUp = (DateTime)((r["PublishUp"] == System.DBNull.Value) ? DateTime.Now : r["PublishUp"]),
                                PublishUpShow = (string)((r["PublishUp"] == System.DBNull.Value) ? DateTime.Now.ToString("dd/MM/yyyy") : (string)((DateTime)r["PublishUp"]).ToString("dd/MM/yyyy")),
                                CatTitle = (string)((r["CatTitle"] == System.DBNull.Value) ? "" : r["CatTitle"]),
                                Link = (string)((r["Link"] == System.DBNull.Value) ? null : r["Link"]),
                                LinkSv = (string)((r["LinkSv"] == System.DBNull.Value) ? null : r["LinkSv"]),
                                CatId = (int)((r["CatId"] == System.DBNull.Value) ? 0 : r["CatId"]),
                                Images = (string)((r["Images"] == System.DBNull.Value) ? null : r["Images"]),
                                Status = (Boolean)r["Status"],
                                Featured = (Boolean)r["Featured"],                                
                                TotalRows = (int)r["TotalRows"],
                            }).ToList();
                }
            }


        }

        public static List<Audios> GetListFeatured(int IdCoQuan)
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Audios",
                new string[] { "@flag", "@IdCoQuan" }, new object[] { "GetListFeatured", IdCoQuan });
            if (tabl == null)
            {
                return new List<Audios>();
            }
            else
            {
                return (from r in tabl.AsEnumerable()
                        select new Audios
                        {
                            Id = (int)r["Id"],
                            Title = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
                            Alias = (string)((r["Alias"] == System.DBNull.Value) ? null : r["Alias"]),
                            CatTitle = (string)((r["CatTitle"] == System.DBNull.Value) ? "" : r["CatTitle"]),                            
                            Description = (string)((r["Description"] == System.DBNull.Value) ? null : r["Description"]),
                            Link = (string)((r["Link"] == System.DBNull.Value) ? null : r["Link"]),
                            LinkSv = (string)((r["LinkSv"] == System.DBNull.Value) ? null : r["LinkSv"]),
                            CatId = (int)((r["CatId"] == System.DBNull.Value) ? 0 : r["CatId"]),
                            Images = (string)((r["Images"] == System.DBNull.Value) ? null : r["Images"]),
                            PublishUp = (DateTime)((r["PublishUp"] == System.DBNull.Value) ? DateTime.Now : r["PublishUp"]),
                            PublishUpShow = (string)((r["PublishUp"] == System.DBNull.Value) ? DateTime.Now.ToString("dd/MM/yyyy") : (string)((DateTime)r["PublishUp"]).ToString("dd/MM/yyyy")),
                        }).ToList();
            }

        }

        public static List<Audios> GetList(int IdCoQuan, Boolean Selected = true)
        {
            
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Audios",
                new string[] { "@flag", "@Selected", "@IdCoQuan" }, new object[] { "GetList", Convert.ToDecimal(Selected), IdCoQuan });
            if (tabl == null)
            {
                return new List<Audios>();
            }
            else
            {
                return (from r in tabl.AsEnumerable()
					select new Audios
					{
						Id = (int)r["Id"],
 						Title = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
                        Alias = (string)((r["Alias"] == System.DBNull.Value) ? null : r["Alias"]),
                        Description = (string)((r["Description"] == System.DBNull.Value) ? null : r["Description"]),
 						Link = (string)((r["Link"] == System.DBNull.Value) ? null : r["Link"]),
                        LinkSv = (string)((r["LinkSv"] == System.DBNull.Value) ? null : r["LinkSv"]),
                        CatId = (int)((r["CatId"] == System.DBNull.Value) ? 0 : r["CatId"]),
                        Images = (string)((r["Images"] == System.DBNull.Value) ? null : r["Images"]),
                        PublishUp = (DateTime)((r["PublishUp"] == System.DBNull.Value) ? DateTime.Now : r["PublishUp"]),
                        PublishUpShow = (string)((r["PublishUp"] == System.DBNull.Value) ? DateTime.Now.ToString("dd/MM/yyyy") : (string)((DateTime)r["PublishUp"]).ToString("dd/MM/yyyy")),
                    }).ToList();
            }

        }

        public static Audios GetItem(int Id, string SecretId = null)
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Audios",
            new string[] { "@flag", "@Id" }, new object[] { "GetItem", Id });
            return (from r in tabl.AsEnumerable()
                    select new Audios
                    {
                        Id = (int)r["Id"],
 						Title = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
                        Alias = (string)((r["Alias"] == System.DBNull.Value) ? null : r["Alias"]),
                        Description = (string)((r["Description"] == System.DBNull.Value) ? null : r["Description"]),
                        PublishUp = (DateTime)((r["PublishUp"] == System.DBNull.Value) ? DateTime.Now : r["PublishUp"]),
                        PublishUpShow = (string)((r["PublishUp"] == System.DBNull.Value) ? DateTime.Now.ToString("dd/MM/yyyy") : (string)((DateTime)r["PublishUp"]).ToString("dd/MM/yyyy")),
						Link = (string)((r["Link"] == System.DBNull.Value) ? null : r["Link"]),
                        LinkSv = (string)((r["LinkSv"] == System.DBNull.Value) ? null : r["LinkSv"]),
                        CatId = (int)((r["CatId"] == System.DBNull.Value) ? 0 : r["CatId"]),
                        Images = (string)((r["Images"] == System.DBNull.Value) ? null : r["Images"]),
 						Status = (Boolean)r["Status"],
                        Featured = (Boolean)r["Featured"],
 						Deleted = (Boolean)r["Deleted"],
 						CreatedBy = (int)r["CreatedBy"],
 						CreatedDate = (DateTime?)((r["CreatedDate"] == System.DBNull.Value) ? null : r["CreatedDate"]),
 						ModifiedBy = (int?)((r["ModifiedBy"] == System.DBNull.Value) ? null : r["ModifiedBy"]),
 						ModifiedDate = (DateTime?)((r["ModifiedDate"] == System.DBNull.Value) ? null : r["ModifiedDate"]),
                        Ids = MyModels.Encode((int)r["Id"], SecretId),
                    }).FirstOrDefault();
        }

        public static dynamic SaveItem(Audios dto)
        {
            dto.Title = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.Title);
            dto.Description = API.Models.MyHelper.StringHelper.RemoveTagsIntroText(dto.Description);
            dto.Images = API.Models.MyHelper.StringHelper.RemoveTagsLink(dto.Images);
            dto.Link = API.Models.MyHelper.StringHelper.RemoveTagsLink(dto.Link);
            dto.LinkSv = API.Models.MyHelper.StringHelper.RemoveTagsLink(dto.LinkSv);
            dto.Alias = API.Models.MyHelper.StringHelper.UrlFriendly(dto.Title);
			DateTime NgayDang = DateTime.ParseExact(dto.PublishUpShow, "dd/MM/yyyy", CultureInfo.InvariantCulture);
			dto.CreatedDate = DateTime.Now;

			DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Audios",
            new string[] { "@flag","@Id","@Title", "@Alias", "@IdCoQuan", "@Featured", "@PublishUp", "@Description","@Link", "@LinkSv", "@CatId", "@Images", "@Status","@Deleted","@CreatedBy","@CreatedDate","@ModifiedBy","@ModifiedDate" },
            new object[] { "SaveItem",dto.Id,dto.Title,dto.Alias,dto.IdCoQuan, dto.Featured,NgayDang, dto.Description,dto.Link,dto.LinkSv, dto.CatId,dto.Images, dto.Status,dto.Deleted,dto.CreatedBy,dto.CreatedDate,dto.ModifiedBy,dto.ModifiedDate });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }
        public static dynamic DeleteItem(Audios dto)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Audios",
            new string[] { "@flag", "@Id", "@ModifiedBy" },
            new object[] { "DeleteItem", dto.Id, dto.ModifiedBy});
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }
		
		public static dynamic UpdateStatus(Audios dto)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Audios",
            new string[] { "@flag", "@Id","@Status", "@ModifiedBy" },
            new object[] { "UpdateStatus", dto.Id,dto.Status, dto.ModifiedBy });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }

        public static dynamic UpdateFeatured(Audios dto)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Audios",
            new string[] { "@flag", "@Id", "@Featured", "@ModifiedBy" },
            new object[] { "UpdateFeatured", dto.Id, dto.Featured, dto.ModifiedBy });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }


    }
}
