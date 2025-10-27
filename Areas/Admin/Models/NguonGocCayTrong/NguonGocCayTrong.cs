using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using API.Areas.Admin.Models.Partial;
using Microsoft.AspNetCore.Mvc.Rendering;
/*https://docs.microsoft.com/en-us/aspnet/core/tutorials/razor-pages/validation?view=aspnetcore-2.2*/
namespace API.Areas.Admin.Models.NguonGocCayTrong
{
    public class NguonGocCayTrong
    {
        public int Id { get; set; }       
        public string? Title { get; set; }
        public string? Description { get; set; }        
        public string? UserName { get; set; }        
        public string? Password { get; set; }        
        public string? FullName { get; set; }        
        public string? LinkFile { get; set; }        
        public Boolean Status { get; set; }
        public string? Address { get; set; }
        public string? Latitude { get; set; }
        public string? Longitude { get; set; }
        public string? Ids { get; set; }
        public int TotalRows { get; set; } = 0;
        public int CreatedBy { get; set; } = 0;
        public int ModifiedBy { get; set; } = 0;
        public int IdPhuongXa { get; set; } = 0;
        public int IdQuanHuyen { get; set; } = 0;
        public string? TenTinhThanh { get; set; }
        public string? TenQuanHuyen { get; set; }
        public string? TenPhuongXa { get; set; }
    }

    public class NguonGocCayTrongModel
    {
        public List<NguonGocCayTrong> ListItems { get; set; } = new List<NguonGocCayTrong>();       
        public SearchNguonGocCayTrong SearchData { get; set; } = new SearchNguonGocCayTrong() { };
        public NguonGocCayTrong Item { get; set; } = new NguonGocCayTrong() { };

        public PartialPagination Pagination { get; set; } = new PartialPagination() { };
        public List<SelectListItem> ListItemsTinhThanh { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> ListItemsDMQuanHuyen { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> ListItemsDMPhuongXa { get; set; } = new List<SelectListItem>();
    }
    public class SearchNguonGocCayTrong
    {
        public int CurrentPage { get; set; }
        public int ItemsPerPage { get; set; }
        public string? Keyword { get; set; }
        public string? Key { get; set; }
        public int IdCoQuan { get; set; }
        public int IdTinhThanh { get; set; }
        public int IdQuanHuyen { get; set; }
        public int IdPhuongXa { get; set; }
        public int Status { get; set; }
    }
}
