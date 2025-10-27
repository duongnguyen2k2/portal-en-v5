using API.Areas.Admin.Models.Partial;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
/*https://docs.microsoft.com/en-us/aspnet/core/tutorials/razor-pages/validation?view=aspnetcore-2.2*/
namespace API.Areas.Admin.Models.Products
{
	public class Products
	{
		public int Id { get; set; }
		[Display(Name = "Tên sản phẩm")]
		[StringLength(60, MinimumLength = 3, ErrorMessage = "Độ dài chuỗi phải lớn hơn {2} Không quá {1} ký tự")]
		[Required(ErrorMessage = "Tên sản phẩm không được để trống")]
		public string? Title { get; set; }
		public string? Alias { get; set; }
		public string? Image { get; set; }
		public List<ImageFile> ListImage { get; set; }
		public string? Str_ListImage { get; set; }
		public int ImageCount { get; set; }
		public string? Sku { get; set; }
		public double Price { get; set; }
		public double PriceDeal { get; set; }
		public double Discount { get; set; }
		public int Quantity { get; set; }
		public int IdManufacturer { get; set; }
		public List<int> List_IdManufacturer { get; set; } = new List<int>() { };
		public string? Str_IdManufacturer { get; set; }
		public string? ContactManufacturer { get; set; }
		public string? TitleManufacturer { get; set; }
		public string? AddressManufacturer { get; set; }
		public string? Description { get; set; }
		public string? Introtext { get; set; }
		public string? PriceDisplay { get; set; }
		public string? PriceDealDisplay { get; set; }
		public Boolean Status { get; set; } = true;
		public Boolean Featured { get; set; } = true;
        public Boolean FlagJson { get; set; }
		public DateTime PublishUp { get; set; } = DateTime.Now;
		public string? PublishUpShow { get; set; } = DateTime.Now.ToString("dd/MM/yyyy");
		public string? Ids { get; set; }
		public int TotalRows { get; set; } = 0;
		public int CreatedBy { get; set; } = 0;
		public int ModifiedBy { get; set; } = 0;
		public string? Metadesc { get; set; }
		public string? Metakey { get; set; }
		public string? Metadata { get; set; }
		public int CatId { get; set; }
		public string? CatTitle { get; set; }
		public List<int> CatMultiId { get; set; } = new List<int>() { };
		public int Amounts { get; set; } = 0;
		public int Available { get; set; } = 0;
		public int Sold { get; set; } = 0;
		public int Star { get; set; } = 5;
		public Boolean OCOP { get; set; }
		public int PerCat { get; set; } = 0;
		public int IdCoQuan { get; set; } = 0;
		public string? TenCoQuan { get; set; }
		public API.Models.MetaData MetadataCV { get; set; }= new API.Models.MetaData() { };
	}

	public class ImageFile
	{
		public string? Title { get; set; }
		public int Ordering { get; set; }
		public string? ImagePath { get; set; }
	}

	public class ProductsModel
	{
		public List<Products> ListItems { get; set; } = new List<Products>() { };
        public List<CategoriesProducts.CategoriesProducts> ListCat { get; set; } = new List<CategoriesProducts.CategoriesProducts>() { };
        public List<SelectListItem> ListItemsCategories { get; set; } = new List<SelectListItem>() { };
		public List<SelectListItem> ListItemsManufacturer { get; set; } = new List<SelectListItem>() { };
		public List<SelectListItem> ListStar { get; set; } = new List<SelectListItem>() { };
		public List<SelectListItem> ListDMCoQuan { get; set; } = new List<SelectListItem>() { };
        public SearchProducts SearchData { get; set; } = new SearchProducts() { };
        public Products Item { get; set; } = new Products() { };
		public CategoriesProducts.CategoriesProducts CategoriesItem { get; set; } = new CategoriesProducts.CategoriesProducts() { };
        public PartialPagination Pagination { get; set; } = new PartialPagination() { };
		public Cart Cart { get; set; } = new Cart() { };
	}

	public class Cart
	{
		public Double Total { get; set; } = 0;
		public int Amount { get; set; } = 0;
	}

	public class AddCart
	{
		public Double Total { get; set; } = 0;
		public int Amount { get; set; } = 0;
	}

	public class SearchProducts
	{
		public int CurrentPage { get; set; }
		public int ItemsPerPage { get; set; }
		public int CatId { get; set; } = -1;
		public int IdManufacturer { get; set; } = 0;
		public int IdCoQuan { get; set; }
		public int AuthorId { get; set; }
		public int CreatedBy { get; set; }
		public int OCOP { get; set; }=1;
		public string? Keyword { get; set; }
		public string? KeywordMobile { get; set; }
		public int Status { get; set; } = -1;
	}


}
