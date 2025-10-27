using API.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API.Areas.Admin.Models.Manufacturer;
using API.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace API.Areas.Admin.Models.Manufacturer
{
    public class ManufacturerService
    {
        public static List<Manufacturer> GetListByCatProduct(SearchManufacturer dto, string SecretId)
        {
            dto.CurrentPage = dto.CurrentPage <= 0 ? 1 : dto.CurrentPage;
            dto.ItemsPerPage = dto.ItemsPerPage <= 0 ? 10 : dto.ItemsPerPage;
            dto.Keyword = dto.Keyword == null ? "" : dto.Keyword;

            var tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Manufacturer",
                new string[] { "@flag", "@IdCatProduct", "@CurrentPage", "@ItemsPerPage", "@Keyword" },
                new object[] { "GetListByCatProduct", dto.IdCatProduct, dto.CurrentPage, dto.ItemsPerPage, dto.Keyword });
            if (tabl == null)
            {
                return new List<Manufacturer>();
            }
            else
            {
                List<Manufacturer> list = (from r in tabl.AsEnumerable()
                                           select new Manufacturer
                                           {
                                               Id = (int)r["Id"],
                                               Title = (string)r["Title"],
                                               Address = (string)((r["Address"] == System.DBNull.Value) ? "" : r["Address"]),
                                               Description = (string)((r["Description"] == System.DBNull.Value) ? "" : r["Description"]),
                                               Phone = (string)((r["Phone"] == System.DBNull.Value) ? "" : r["Phone"]),
                                               Contact = (string)((r["Contact"] == System.DBNull.Value) ? "" : r["Contact"]),
                                               FullName = (string)((r["FullName"] == System.DBNull.Value) ? "" : r["FullName"]),
                                               Image = (string)((r["Image"] == System.DBNull.Value) ? "" : r["Image"]),
                                               Str_ListImage = (string)((r["ListImage"] == System.DBNull.Value) ? "" : r["ListImage"]),
                                               Certificates = (string)((r["Certificates"] == System.DBNull.Value) ? "" : r["Certificates"]),
                                               IsMaVungTrong = (bool)((r["IsMaVungTrong"] == System.DBNull.Value) ? 0 : r["IsMaVungTrong"]),
                                               Status = (bool)r["Status"],
                                               Ids = MyModels.Encode((int)r["Id"], SecretId),
                                               TotalRows = (int)r["TotalRows"],
                                           }).ToList();

                foreach (Manufacturer item in list)
                {
                    if (item != null)
                    {
                        if (item.Str_ListImage != null && item.Str_ListImage != "")
                        {
                            item.ListImage = JsonConvert.DeserializeObject<List<ImageFile>>(item.Str_ListImage);
                        }
                    }
                }

                return list;
            }
        }

        public static List<int> GetListIdByProduct(int IdProduct)
        {
            List<int> result = new List<int>();

            var tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Manufacturer",
                new string[] { "@flag", "@IdProduct" },
                new object[] { "GetListIdByProduct", IdProduct });
            if (tabl == null)
            {
                //return new List<int>();
                return null;
            }
            else
            {
                //return (from r in tabl.AsEnumerable()
                //		select new Manufacturer
                //		{
                //			Id = (int)r["IdManufacturer"],
                //		}).ToList();
                foreach (DataRow item in tabl.Rows)
                {
                    result.Add((int)item["IdManufacturer"]);
                }

                return result;
            }
        }

		public static List<Manufacturer> GetListPagination(SearchManufacturer dto, string SecretId)
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
			var tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Manufacturer",
				new string[] { "@flag", "@CurrentPage", "@ItemsPerPage", "@Keyword" },
				new object[] { "GetListPagination", dto.CurrentPage, dto.ItemsPerPage, dto.Keyword });
			if (tabl == null)
			{
				return new List<Manufacturer>();
			}
			else
			{
				return (from r in tabl.AsEnumerable()
						select new Manufacturer
						{
							Id = (int)r["Id"],
							Title = (string)r["Title"],
							Status = (bool)r["Status"],
							Address = (string)((r["Address"] == System.DBNull.Value) ? "" : r["Address"]),
							Description = (string)((r["Description"] == System.DBNull.Value) ? "" : r["Description"]),
							Contact = (string)((r["Contact"] == System.DBNull.Value) ? "" : r["Contact"]),
							Image = (string)((r["Image"] == System.DBNull.Value) ? "" : r["Image"]),
							Phone = (string)((r["Phone"] == System.DBNull.Value) ? "" : r["Phone"]),
							FullName = (string)((r["FullName"] == System.DBNull.Value) ? "" : r["FullName"]),
							Ids = MyModels.Encode((int)r["Id"], SecretId),
							TotalRows = (int)r["TotalRows"],
						}).ToList();
			}
		}

				

		public static List<Manufacturer> GetList(Boolean Selected = true)
		{

			DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Manufacturer",
				new string[] { "@flag", "@Selected" }, new object[] { "GetList", Selected });
			return (from r in tabl.AsEnumerable()
					select new Manufacturer
					{
						Id = (int)r["Id"],
						Title = (string)r["Title"],
						Description = (string)((r["Description"] == System.DBNull.Value) ? "" : r["Description"])
					}).ToList();

		}

		public static List<SelectListItem> GetListSelectItem(Boolean Selected = true)
		{

			DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Manufacturer",
				new string[] { "@flag", "@Selected" },
				new object[] { "GetListSelectItem", Convert.ToDecimal(Selected) });
			List<SelectListItem> ListItems = (from r in tabl.AsEnumerable()
											  select new SelectListItem
											  {
												  Value = (string)((r["Id"] == System.DBNull.Value) ? null : r["Id"].ToString()),
												  Text = (string)((r["Title"] == System.DBNull.Value) ? "" : r["Title"]),
												  Disabled = false,
											  }).ToList();
			ListItems.Insert(0, (new SelectListItem { Text = "Chọn Danh mục Cơ sở SX", Value = "0", Disabled = true }));
			return ListItems;
		}

		public static List<SelectListItem> GetListSelectItems(Boolean Selected = true)
		{

			DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Manufacturer",
				new string[] { "@flag", "@Selected" }, new object[] { "GetList", Convert.ToDecimal(Selected) });
			List<SelectListItem> ListItems = (from r in tabl.AsEnumerable()
											  select new SelectListItem
											  {
												  Value = (string)((r["Id"] == System.DBNull.Value) ? null : r["Id"].ToString()),
												  Text = (string)((r["Title"] == System.DBNull.Value) ? "" : r["Title"]),
												  Disabled = false,
											  }).ToList();
			ListItems.Insert(0, (new SelectListItem { Text = "Chọn Danh mục Cơ sở SX", Value = "0", Disabled = true }));
			return ListItems;
		}

		public static Manufacturer GetItem(int Id, string SecretId = null)
		{

			DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Manufacturer",
			new string[] { "@flag", "@Id" }, new object[] { "GetItem", Id });
			Manufacturer Item = (from r in tabl.AsEnumerable()
								 select new Manufacturer
								 {
									 Id = (int)r["Id"],
									 Title = (string)r["Title"],
									 Address = (string)((r["Address"] == System.DBNull.Value) ? "" : r["Address"]),
									 Description = (string)((r["Description"] == System.DBNull.Value) ? "" : r["Description"]),
									 Phone = (string)((r["Phone"] == System.DBNull.Value) ? "" : r["Phone"]),
									 Contact = (string)((r["Contact"] == System.DBNull.Value) ? "" : r["Contact"]),
									 Image = (string)((r["Image"] == System.DBNull.Value) ? "" : r["Image"]),
									 Str_ListImage = (string)((r["ListImage"] == System.DBNull.Value) ? "" : r["ListImage"]),
									 FullName = (string)((r["FullName"] == System.DBNull.Value) ? "" : r["FullName"]),
									 IsMaVungTrong = (bool)((r["IsMaVungTrong"] == System.DBNull.Value) ? Byte.Parse("0") : r["IsMaVungTrong"]),
									 Certificates = (string)((r["Certificates"] == System.DBNull.Value) ? "" : r["Certificates"]),
									 Status = (bool)r["Status"],
									 Ids = MyModels.Encode((int)r["Id"], SecretId),
								 }).FirstOrDefault();

			if (Item != null)
			{
				if (Item.Str_ListImage != null && Item.Str_ListImage != "")
				{
					Item.ListImage = JsonConvert.DeserializeObject<List<ImageFile>>(Item.Str_ListImage);
					Item.ImageCount = Item.ListImage.Count();
				}
			}

			return Item;
		}

		public static dynamic SaveItem(Manufacturer dto, DataTable tbItem)
		{
            dto.Title = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.Title);
            dto.Contact = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.Contact);
            dto.Address = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.Address);
            dto.FullName = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.FullName);
            dto.Description = API.Models.MyHelper.StringHelper.RemoveTagsIntroText(dto.Description);                       
            dto.Phone = API.Models.MyHelper.StringHelper.RemoveTagsTitle(dto.Phone);
            dto.Image = API.Models.MyHelper.StringHelper.RemoveTagsLink(dto.Image);
           
          
           

            List<ImageFile> images = new List<ImageFile>();
			String ListImageString = "";

			// Convert ImageFile from List -> StringObject
			if (dto.ListImage != null && dto.ListImage.Count() > 0)
			{
				for (int i = 0; i < dto.ListImage.Count(); i++)
				{
					if (dto.ListImage[i].ImagePath != null)
					{
						images.Add(dto.ListImage[i]);
					}
				}
				if (images != null && images.Count() > 0)
				{
					ListImageString = JsonConvert.SerializeObject(images);
				}
			}

			DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Manufacturer",
			new string[] { "@flag", "@Id", "@Title", "@Status", "@Description", "@Contact", "@Address", "@Image", "@Phone", "@FullName", "@CreatedBy",
				"@ModifiedBy", "@ListImage", "@TBL_Products_Manufacturers" },
			new object[] { "SaveItem", dto.Id, dto.Title, dto.Status, dto.Description, dto.Contact, dto.Address, dto.Image, dto.Phone, dto.FullName, dto.CreatedBy,
				dto.ModifiedBy, ListImageString, tbItem });
			return (from r in tabl.AsEnumerable()
					select new
					{
						N = (int)(r["N"]),
					}).FirstOrDefault();
		}

		public static dynamic UpdateStatus(Manufacturer dto)
		{
			DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Manufacturer",
			new string[] { "@flag", "@Id", "@Status", "@ModifiedBy" },
			new object[] { "UpdateStatus", dto.Id, dto.Status, dto.ModifiedBy });
			return (from r in tabl.AsEnumerable()
					select new
					{
						N = (int)(r["N"]),
					}).FirstOrDefault();

		}

		public static dynamic DeleteItem(Manufacturer dto)
		{
			DataTable tabl = ConnectDb.ExecuteDataTableTask(Startup.ConnectionString, "SP_Manufacturer",
			new string[] { "@flag", "@Id", "@ModifiedBy" },
			new object[] { "DeleteItem", dto.Id, dto.ModifiedBy });
			return (from r in tabl.AsEnumerable()
					select new
					{
						N = (int)(r["N"]),
					}).FirstOrDefault();

		}


	}
}
