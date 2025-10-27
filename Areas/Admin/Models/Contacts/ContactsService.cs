using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using API.Areas.Admin.Models.Contacts;
using API.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace API.Areas.Admin.Models.Contacts
{
    public class ContactsService
    {
        public static List<Contacts> GetListPagination(SearchContacts dto, string SecretId)
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

            var tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Contacts",
                new string[] { "@flag", "@CurrentPage", "@ItemsPerPage", "@Keyword", "@IdCoQuan", "@Status", "@TypeId" },
                new object[] { str_sql, dto.CurrentPage, dto.ItemsPerPage, dto.Keyword,dto.IdCoQuan , Status ,dto.TypeId });
            if (tabl == null)
            {
                return new List<Contacts>();
            }
            else
            {
                return (from r in tabl.AsEnumerable()
					select new Contacts
					{
						Id = (int)r["Id"],
 						Title = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
                        Status = (Boolean)r["Status"],
 						Fullname = (string)((r["Fullname"] == System.DBNull.Value) ? null : r["Fullname"]),
 						Phone = (string)((r["Phone"] == System.DBNull.Value) ? null : r["Phone"]),
 						Email = (string)((r["Email"] == System.DBNull.Value) ? null : r["Email"]),
 						Introtext = (string)((r["Introtext"] == System.DBNull.Value) ? null : r["Introtext"]),
 						Description = (string)((r["Description"] == System.DBNull.Value) ? null : r["Description"]),
 						Address = (string)((r["Address"] == System.DBNull.Value) ? null : r["Address"]),
                        TypeId = (int)((r["TypeId"] == System.DBNull.Value) ? 0 : r["TypeId"]),
                        Ids = MyModels.Encode((int)r["Id"], SecretId),
						TotalRows = (int)r["TotalRows"],
                        PublishUp = (DateTime)((r["PublishUp"] == System.DBNull.Value) ? null : r["PublishUp"]),
                        PublishUpShow = (string)((r["PublishUp"] == System.DBNull.Value) ? DateTime.Now.ToString("dd/MM/yyyy") : (string)((DateTime)r["PublishUp"]).ToString("dd/MM/yyyy")),
                        IdCoQuan = (int)r["IdCoQuan"],
					}).ToList();
            }


        }

        public static List<SelectListItem> GetListItemsType()
        {
            List<SelectListItem> ListItems = new List<SelectListItem>();
            ListItems.Insert(0, (new SelectListItem { Text = "--- Loại ---", Value = "-1" }));
            ListItems.Insert(1, (new SelectListItem { Text = "Hỏi Đáp", Value = "0" }));
            ListItems.Insert(2, (new SelectListItem { Text = "Liên Hệ", Value = "1" }));
            return ListItems;
        }
        public static List<Contacts> GetList(Boolean Selected = true)
        {
            DateTime a =DateTime.Now;
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Contacts",
                new string[] { "@flag", "@Selected" }, new object[] { "GetList", Convert.ToDecimal(Selected) });
            if (tabl == null)
            {
                return new List<Contacts>();
            }
            else
            {
                return (from r in tabl.AsEnumerable()
					select new Contacts
					{
						Id = (int)r["Id"],
                        Title = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
                        Status = (Boolean)r["Status"],
 						Fullname = (string)((r["Fullname"] == System.DBNull.Value) ? null : r["Fullname"]),
 						Phone = (string)((r["Phone"] == System.DBNull.Value) ? null : r["Phone"]),
 						Email = (string)((r["Email"] == System.DBNull.Value) ? null : r["Email"]),
 						Address = (string)((r["Address"] == System.DBNull.Value) ? null : r["Address"]),
                        TypeId = (int)((r["TypeId"] == System.DBNull.Value) ? 0 : r["TypeId"]),
                        PublishUp = (DateTime)((r["PublishUp"] == System.DBNull.Value) ? null : r["PublishUp"]),
                        PublishUpShow = (string)((r["PublishUp"] == System.DBNull.Value) ? DateTime.Now.ToString("dd/MM/yyyy") : (string)((DateTime)r["PublishUp"]).ToString("dd/MM/yyyy")),
                        IdCoQuan = (int)r["IdCoQuan"],
                    }).ToList();
            }

        }

        public static Contacts GetItem(decimal Id, int IdCoQuan, string SecretId = null)
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Contacts",
            new string[] { "@flag", "@Id","@IdCoQuan" }, new object[] { "GetItem", Id, IdCoQuan });
            return (from r in tabl.AsEnumerable()
                    select new Contacts
                    {
                        Id = (int)r["Id"],
                        Title = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
                        Status = (Boolean)r["Status"],
 						Fullname = (string)((r["Fullname"] == System.DBNull.Value) ? null : r["Fullname"]),
 						Phone = (string)((r["Phone"] == System.DBNull.Value) ? null : r["Phone"]),
 						Email = (string)((r["Email"] == System.DBNull.Value) ? null : r["Email"]),
 						Address = (string)((r["Address"] == System.DBNull.Value) ? null : r["Address"]),
 						Introtext = (string)((r["Introtext"] == System.DBNull.Value) ? null : r["Introtext"]),
 						Description = (string)((r["Description"] == System.DBNull.Value) ? null : r["Description"]),
                        TypeId = (int)((r["TypeId"] == System.DBNull.Value) ? 0 : r["TypeId"]),
                        Deleted = (Boolean)r["Deleted"],
 						CreatedBy = (int)r["CreatedBy"],
 						CreatedDate = (DateTime)r["CreatedDate"],
 						ModifiedBy = (int?)((r["ModifiedBy"] == System.DBNull.Value) ? null : r["ModifiedBy"]),
 						ModifiedDate = (DateTime?)((r["ModifiedDate"] == System.DBNull.Value) ? null : r["ModifiedDate"]),
                        PublishUp = (DateTime)((r["PublishUp"] == System.DBNull.Value) ? null : r["PublishUp"]),
                        PublishUpShow = (string)((r["PublishUp"] == System.DBNull.Value) ? DateTime.Now.ToString("dd/MM/yyyy") : (string)((DateTime)r["PublishUp"]).ToString("dd/MM/yyyy")),
                        Ids = MyModels.Encode((int)r["Id"], SecretId),
                        IdCoQuan = (int)r["IdCoQuan"],
                    }).FirstOrDefault();
        }

        public static string ConvertBodyEmail(Contacts dto,string  Body)
        {
            
            Body = Body.Replace("{{Title}}", dto.Title);
            Body = Body.Replace("{{Fullname}}", dto.Fullname);
            Body = Body.Replace("{{Phone}}", dto.Phone);
            Body = Body.Replace("{{Address}}", dto.Address);
            Body = Body.Replace("{{Email}}", dto.Email);
            Body = Body.Replace("{{Introtext}}", dto.Introtext);
            Body = Body.Replace("{{Description}}", dto.Description);            
            Body = Body.Replace("{{CreatedDate}}", DateTime.Now.Hour.ToString()+":"+ DateTime.Now.Minute+ " &nbsp; " + API.Models.MyHelper.StringHelper.DayweekVN(DateTime.Now) + ", Ngày " + DateTime.Now.ToString("dd") + ", Tháng " + DateTime.Now.ToString("MM") + " Năm " + DateTime.Now.ToString("yyyy"));
            
            return Body;
            

        } 
        
        public static dynamic SaveItem(Contacts dto)
        {
            dto.Title = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.Title);
            dto.Fullname = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.Fullname);
            dto.Phone = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.Phone);
            dto.Email = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.Email);
            dto.Address = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.Address);
            dto.Description = API.Models.MyHelper.StringHelper.RemoveTagsFullText(dto.Description);
            dto.Introtext = API.Models.MyHelper.StringHelper.RemoveTagsIntroText(dto.Introtext);

            DateTime NgayDang = DateTime.Now;
            if (dto.PublishUpShow != null && dto.PublishUpShow != "")
            {
                NgayDang = DateTime.ParseExact(dto.PublishUpShow, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Contacts",
            new string[] { "@flag","@Id","@Title","@Introtext","@Description","@Status","@Fullname","@Phone","@Email","@Address","@CreatedBy","@ModifiedBy", "@IdCoQuan", "@TypeId", "@PublishUp" },
            new object[] { "SaveItem",dto.Id,dto.Title,dto.Introtext,dto.Description,dto.Status,dto.Fullname,dto.Phone,dto.Email,dto.Address,dto.CreatedBy,dto.ModifiedBy,dto.IdCoQuan,dto.TypeId,NgayDang});
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();
        }
        public static dynamic DeleteItem(Contacts dto)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Contacts",
            new string[] { "@flag", "@Id", "@ModifiedBy" },
            new object[] { "DeleteItem", dto.Id, dto.ModifiedBy});
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }

        public static dynamic UpdateStatus(Contacts dto)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Contacts",
            new string[] { "@flag", "@Id","@Status", "@ModifiedBy" },
            new object[] { "UpdateStatus", dto.Id,dto.Status, dto.ModifiedBy });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }

        // TMP
        public static dynamic SaveItemTMP(Contacts dto)
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Contacts",
            new string[] { "@flag", "@Id", "@Title", "@Introtext", "@Description", "@Status", "@Fullname", "@Phone", "@Email", "@Address", "@CreatedBy", "@CreatedDate", "@ModifiedBy", "@IdCoQuan", "@NguoiTraLoi", "@NgayTraLoi", "@Link", "@LinkRoot", "@DataHtml", "@LinhVuc", "@Hits" , "@IdRoot" },
            new object[] { "SaveItemTMP", dto.Id, dto.Title, dto.Introtext, dto.Description, dto.Status, dto.Fullname, dto.Phone, dto.Email, dto.Address, dto.CreatedBy,dto.CreatedDate, dto.ModifiedBy, dto.IdCoQuan,dto.NguoiTraLoi,dto.NgayTraLoi,dto.Link,dto.LinkRoot,dto.DataHtml,dto.LinhVuc ,dto.Hits,dto.IdRoot });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();
        }


        public static Contacts GetItemTMP(int IdRoot)
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Contacts",
            new string[] { "@flag", "@IdRoot" }, new object[] { "GetItemTMP", IdRoot });
            return (from r in tabl.AsEnumerable()
                    select new Contacts
                    {
                        Id = (int)r["Id"],
                        Title = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
                        Status = (Boolean)r["Status"],
                        Fullname = (string)((r["Fullname"] == System.DBNull.Value) ? null : r["Fullname"]),
                        Phone = (string)((r["Phone"] == System.DBNull.Value) ? null : r["Phone"]),
                        Email = (string)((r["Email"] == System.DBNull.Value) ? null : r["Email"]),
                        Address = (string)((r["Address"] == System.DBNull.Value) ? null : r["Address"]),
                        //Introtext = (string)((r["Introtext"] == System.DBNull.Value) ? null : r["Introtext"]),
                        /*Description = (string)((r["Description"] == System.DBNull.Value) ? null : r["Description"]),
                        Deleted = (Boolean)r["Deleted"],
                        CreatedBy = (int)r["CreatedBy"],
                        CreatedDate = (DateTime)r["CreatedDate"],
                        ModifiedBy = (int?)((r["ModifiedBy"] == System.DBNull.Value) ? null : r["ModifiedBy"]),
                        ModifiedDate = (DateTime?)((r["ModifiedDate"] == System.DBNull.Value) ? null : r["ModifiedDate"]),                        
                        IdCoQuan = (int)r["IdCoQuan"],*/
                    }).FirstOrDefault();
        }
    }
}
