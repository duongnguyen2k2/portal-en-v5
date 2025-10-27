using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using API.Areas.Admin.Models.Partial;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace API.Areas.Admin.Models.Contacts
{
    public class Contacts
    {
		public string? Ids { get; set; }
        public int TotalRows { get; set; }
        public int Id { get; set; }       
        [StringLength(1024, MinimumLength = 3, ErrorMessage = "Độ dài chuỗi phải lớn hơn {2} Không quá {1} ký tự")]
        [Required(ErrorMessage = "Tiêu đề không được để trống")]
        public string? Title { get; set; }
 		public Boolean Status { get; set; }
        [StringLength(1024, MinimumLength = 3, ErrorMessage = "Độ dài chuỗi phải lớn hơn {2} Không quá {1} ký tự")]
        [Required(ErrorMessage = "Tên không được để trống")]
        public string? Fullname { get; set; }
        [StringLength(2000, MinimumLength = 3, ErrorMessage= "Độ dài chuỗi phải lớn hơn {2} Không quá {1} ký tự")]
        [Required(ErrorMessage = "Nội dung không được để trống")]
        public string? Introtext { get; set; }
 		public string? Description { get; set; }
 		public string? Phone { get; set; }
 		public string? Email { get; set; }
 		public string? Address { get; set; }
 		public Boolean Deleted { get; set; }
 		public int CreatedBy { get; set; }
 		public int IdCoQuan { get; set; }
 		public DateTime CreatedDate { get; set; }
 		public DateTime NgayTraLoi { get; set; }
 		public string? CreatedDateShow { get; set; }
 		public string? Token { get; set; }
 		public string? LinhVuc { get; set; }
 		public string? NguoiTraLoi { get; set; }
 		public string? Link { get; set; }
 		public string? LinkRoot { get; set; }
 		public string? DataHtml { get; set; }
 		public int? ModifiedBy { get; set; }
 		public int Hits { get; set; }
 		public int IdRoot { get; set; }
        public int TypeId { get; set; } = 0; // 0: hỏi đáp, 1: Liên Hệ, 2: Góp ý ứng dụng
 		public DateTime? ModifiedDate { get; set; }
        public DateTime PublishUp { get; set; } = DateTime.Now;
        public string? PublishUpShow { get; set; } = DateTime.Now.ToString("dd/MM/yyyy");

    }
	
	public class ContactsModel {
        public List<Contacts> ListItems { get; set; } = new List<Contacts>() { };       
        public SearchContacts SearchData { get; set; } = new SearchContacts() { };
        public Contacts Item { get; set; } = new Contacts() { };
        public PartialPagination Pagination { get; set; } = new PartialPagination() { };
        public List<SelectListItem> ListItemsTypes { get; set; } = new List<SelectListItem>() { };
    }

    public class SearchContacts {
        public int CurrentPage { get; set; }
        public int ItemsPerPage { get; set; }
        public int IdCoQuan { get; set; }
        public int Status { get; set; } = -1;
        public string? Keyword { get; set; }
        public string? Cookie { get; set; }
        public int TypeId { get; set; } = -1;
    }
}
