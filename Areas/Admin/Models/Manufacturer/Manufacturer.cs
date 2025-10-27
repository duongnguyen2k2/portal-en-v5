using API.Areas.Admin.Models.Partial;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
/*https://docs.microsoft.com/en-us/aspnet/core/tutorials/razor-pages/validation?view=aspnetcore-2.2*/
namespace API.Areas.Admin.Models.Manufacturer
{
	public class Manufacturer
	{
		public int Id { get; set; }

		[Display(Name = "Tên")]
		[StringLength(60, MinimumLength = 3, ErrorMessage = "Độ dài chuỗi phải lớn hơn {2} Không quá {1} ký tự")]
		[Required(ErrorMessage = "Tên chức vụ không được để trống")]
		public string? Title { get; set; }
		public string? Address { get; set; }
		public string? Description { get; set; }
		public string? Phone { get; set; }
		public string? Contact { get; set; }
		public string? FullName { get; set; }
		public string? Image { get; set; }
		public List<ImageFile> ListImage { get; set; }
		public string? Str_ListImage { get; set; }
		public int ImageCount { get; set; }
		public Boolean IsMaVungTrong { get; set; }
		public string? Certificates { get; set; }
		public int IdCoQuan { get; set; } = 0;
		public Boolean Status { get; set; }
		public int CreatedBy { get; set; } = 0;
		public int ModifiedBy { get; set; } = 0;
		public int TotalRows { get; set; } = 0;
		public string? Ids { get; set; }
		public List<int> List_IdProduct { get; set; }
		public List<Products.Products> ListProduct { get; set; }

	}

	public class ImageFile
	{
		public string? Title { get; set; }
		public string? ImagePath { get; set; }
		public int Ordering { get; set; }
	}

	public class ManufacturerModel
	{
		public List<Manufacturer> ListItems { get; set; } = new List<Manufacturer>();
		public List<SelectListItem> ListCatProduct { get; set; } = new List<SelectListItem>();
		public List<SelectListItem> ListProduct { get; set; } = new List<SelectListItem>();
		public SearchManufacturer SearchData { get; set; } = new SearchManufacturer() { };
        public Manufacturer Item { get; set; } = new Manufacturer() { };
		public PartialPagination Pagination { get; set; } = new PartialPagination() { };
	}
	public class SearchManufacturer
	{
		public int CurrentPage { get; set; }
		public int ItemsPerPage { get; set; }
		public int IdCoQuan { get; set; }
		public int IdCatProduct { get; set; }
		public int Status { get; set; } = -1;
		public string? Keyword { get; set; }
	}
}
