using API.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace API.Areas.Admin.Models.HTX
{
    public class HTXService
    {
        public static List<HTX> GetListPagination(SearchHTX dto, string SecretId, string Culture = "all")
        {
            Culture = API.Models.MyHelper.StringHelper.ConverLan(Culture);

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
            var tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_HTX",
                new string[] { "@flag", "@CurrentPage", "@ItemsPerPage", "@Keyword", "@IdCoQuan" },
                new object[] { "GetListPagination", dto.CurrentPage, dto.ItemsPerPage, dto.Keyword, dto.IdCoQuan });
            if (tabl == null)
            {
                return new List<HTX>();
            }
            else
            {
                return (from r in tabl.AsEnumerable()
                        select new HTX
                        {
                            Id = (int)r["Id"],
                            Title = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
                            Description = (string)((r["Description"] == System.DBNull.Value) ? null : r["Description"]),
                            IdCoQuan = (int)((r["IdCoQuan"] == System.DBNull.Value) ? null : r["IdCoQuan"]),
                            Status = (Boolean)((r["Status"] == System.DBNull.Value) ? null : r["Status"]),                           
                            Ids = MyModels.Encode((int)r["Id"], SecretId),
                            TitleEn = (string)((r["TitleEn"] == System.DBNull.Value) ? null : r["TitleEn"]),
                            TotalRows = (int)r["TotalRows"],
                        }).ToList();
            }


        }


        public static List<HTX> GetListPaginationClient(SearchHTX dto, string SecretId, string Culture = "all")
        {
            Culture = API.Models.MyHelper.StringHelper.ConverLan(Culture);

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
            var tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_HTX",
                new string[] { "@flag", "@CurrentPage", "@ItemsPerPage", "@Keyword", "@IdCoQuan" },
                new object[] { "GetListPagination", dto.CurrentPage, dto.ItemsPerPage, dto.Keyword, dto.IdCoQuan });
            if (tabl == null)
            {
                return new List<HTX>();
            }
            else
            {
                if(Culture == "vi")
                {
                    return (from r in tabl.AsEnumerable()
                            select new HTX
                            {
                                Id = (int)r["Id"],
                                Title = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
                                Image = (string)((r["Image"] == System.DBNull.Value) ? null : r["Image"]),
                                Description = (string)((r["Description"] == System.DBNull.Value) ? null : r["Description"]),
                                Phone = (string)((r["Phone"] == System.DBNull.Value) ? null : r["Phone"]),
                                Address = (string)((r["Address"] == System.DBNull.Value) ? null : r["Address"]),
                                IdCoQuan = (int)((r["IdCoQuan"] == System.DBNull.Value) ? null : r["IdCoQuan"]),
                                Status = (Boolean)((r["Status"] == System.DBNull.Value) ? null : r["Status"]),
                                Ids = MyModels.Encode((int)r["Id"], SecretId),
                                TotalRows = (int)r["TotalRows"],
                            }).ToList();
                }
                else
                {
                    return (from r in tabl.AsEnumerable()
                            select new HTX
                            {
                                Id = (int)r["Id"],
                                Title = (string)((r["TitleEn"] == System.DBNull.Value) ? null : r["TitleEn"]),
                                Image = (string)((r["Image"] == System.DBNull.Value) ? null : r["Image"]),
                                Description = (string)((r["DescriptionEn"] == System.DBNull.Value) ? null : r["DescriptionEn"]),
                                Phone = (string)((r["Phone"] == System.DBNull.Value) ? null : r["Phone"]),
                                Address = (string)((r["AddressEn"] == System.DBNull.Value) ? null : r["AddressEn"]),
                                IdCoQuan = (int)((r["IdCoQuan"] == System.DBNull.Value) ? null : r["IdCoQuan"]),
                                Status = (Boolean)((r["Status"] == System.DBNull.Value) ? null : r["Status"]),
                                Ids = MyModels.Encode((int)r["Id"], SecretId),
                                TotalRows = (int)r["TotalRows"],
                            }).ToList();
                }
            }


        }

        public static List<HTX> GetList(int IdCoQuan = 0, string Culture = "vi")
        {
            Culture = API.Models.MyHelper.StringHelper.ConverLan(Culture);

            List<HTX> ListItems = new List<HTX>();

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_HTX",
                new string[] { "@flag", "@IdCoQuan" }, new object[] { "GetList", IdCoQuan });

            if (Culture == "en")
            {
                ListItems = (from r in tabl.AsEnumerable()
                             select new HTX
                             {
                                 Id = (int)r["Id"],
                                 Title = (string)((r["TitleEn"] == System.DBNull.Value) ? r["Title"] : r["TitleEn"]),
                                 Description = (string)((r["DescriptionEn"] == System.DBNull.Value) ? null : r["DescriptionEn"]),
                                 Address = (string)((r["AddressEn"] == System.DBNull.Value) ? null : r["AddressEn"]),
                                 IdCoQuan = (int)((r["IdCoQuan"] == System.DBNull.Value) ? null : r["IdCoQuan"]),
                                 Phone = (string)((r["Phone"] == System.DBNull.Value) ? null : r["Phone"]),
                                 Image = (string)((r["Image"] == System.DBNull.Value) ? null : r["Image"]),
                             }).ToList();
            }
            else
            {
                ListItems = (from r in tabl.AsEnumerable()
                             select new HTX
                             {
                                 Id = (int)r["Id"],
                                 Title = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
                                 Description = (string)((r["Description"] == System.DBNull.Value) ? null : r["Description"]),
                                 Address = (string)((r["Address"] == System.DBNull.Value) ? null : r["Address"]),
                                 Status = (Boolean)((r["Status"] == System.DBNull.Value) ? null : r["Status"]),
                                 IdCoQuan = (int)((r["IdCoQuan"] == System.DBNull.Value) ? null : r["IdCoQuan"]),
                                 Image = (string)((r["Image"] == System.DBNull.Value) ? null : r["Image"]),
                                 Phone = (string)((r["Phone"] == System.DBNull.Value) ? null : r["Phone"]),
                             }).ToList();
            }
            return ListItems;
        }

        public static HTX GetItem(decimal Id, string SecretId = null, string Culture = "vi")
        {
            Culture = API.Models.MyHelper.StringHelper.ConverLan(Culture);

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_HTX",
            new string[] { "@flag", "@Id" }, new object[] { "GetItem", Id });
            return (from r in tabl.AsEnumerable()
                    select new HTX
                    {
                        Id = (int)r["Id"],
                        Title = (string)((r["Title"] == System.DBNull.Value) ? null : r["Title"]),
                        TitleEn = (string)((r["TitleEn"] == System.DBNull.Value) ? null : r["TitleEn"]),
                        Description = (string)((r["Description"] == System.DBNull.Value) ? null : r["Description"]),
                        DescriptionEn = (string)((r["DescriptionEn"] == System.DBNull.Value) ? null : r["DescriptionEn"]),
                        Address = (string)((r["Address"] == System.DBNull.Value) ? null : r["Address"]),
                        AddressEn = (string)((r["AddressEn"] == System.DBNull.Value) ? null : r["AddressEn"]),
                        IdCoQuan = (int)((r["IdCoQuan"] == System.DBNull.Value) ? null : r["IdCoQuan"]),
                        Status = (Boolean)((r["Status"] == System.DBNull.Value) ? null : r["Status"]),
                        Ids = MyModels.Encode((int)r["Id"], SecretId),
                        Image = (string)((r["Image"] == System.DBNull.Value) ? null : r["Image"]),
                        Phone = (string)((r["Phone"] == System.DBNull.Value) ? null : r["Phone"]),
                    }).FirstOrDefault();
        }

        public static dynamic SaveItem(HTX dto, string Culture = "vi")
        {

            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_HTX",
            new string[] { "@flag", "@Id", "@Title", "@TitleEn", "@Address", "@AddressEn", "@Description", "@DescriptionEn", "@Status", "@CreatedBy", "@ModifiedBy", "@IdCoQuan", "@Phone", "@Image" },
            new object[] { "SaveItem", dto.Id, dto.Title, dto.TitleEn, dto.Address, dto.AddressEn, dto.Description, dto.DescriptionEn, dto.Status, dto.CreatedBy, dto.ModifiedBy, dto.IdCoQuan , dto.Phone, dto.Image});
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }
        public static dynamic DeleteItem(HTX dto)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_HTX",
            new string[] { "@flag", "@Id", "@ModifiedBy" },
            new object[] { "DeleteItem", dto.Id, dto.ModifiedBy });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }

        public static dynamic UpdateStatus(HTX dto)
        {
            DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_HTX",
            new string[] { "@flag", "@Id", "@Status", "@ModifiedBy" },
            new object[] { "UpdateStatus", dto.Id, dto.Status, dto.ModifiedBy });
            return (from r in tabl.AsEnumerable()
                    select new
                    {
                        N = (int)(r["N"]),
                    }).FirstOrDefault();

        }
    }
}
