using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using API.Areas.Admin.Models.Partial;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace API.Areas.Admin.Models.Audios
{
    public class Audios
    {
		public string? Ids { get; set; }
        public int TotalRows { get; set; }
        public int Id { get; set; }
 		public string? Title { get; set; } 		
 		public string? Alias { get; set; } 		
 		public string? Description { get; set; }
 		public string? CatTitle { get; set; }
 		public string? Link { get; set; }
 		public string? LinkSv { get; set; }
        public int CatId { get; set; } = 1;
        public int IdCoQuan { get; set; } = 1;
 		public string? Images { get; set; }
 		public Boolean Status { get; set; }
 		public Boolean Featured { get; set; }
 		public Boolean Deleted { get; set; }
 		public int CreatedBy { get; set; }
 		public DateTime? CreatedDate { get; set; }
 		public int? ModifiedBy { get; set; }
 		public DateTime? ModifiedDate { get; set; }
		[Required(ErrorMessage = "Ngày đăng không được để trống")]
		public DateTime PublishUp { get; set; } = DateTime.Now;
		public string? PublishUpShow { get; set; } = DateTime.Now.ToString("dd/MM/yyyy");

	}
	
	public class AudiosModel {
        public List<Audios> ListItems { get; set; } = new List<Audios>();       
        
        public List<SelectListItem> ListItemsCat { get; set; } = new List<SelectListItem>();       
        public SearchAudios SearchData { get; set; } = new SearchAudios() { };
        public Audios Item { get; set; } = new Audios() { };
        public PartialPagination Pagination { get; set; } = new PartialPagination() { };
        public CategoriesAudios.CategoriesAudios Categories { get; set; } = null;

    }

    public class SearchAudios {
        public int CurrentPage { get; set; }
        public int ItemsPerPage { get; set; }
        public int CatId { get; set; }
        public int IdCoQuan { get; set; }
        public string? Keyword { get; set; }
        public int Status { get; set; } = -1;
    }
}
