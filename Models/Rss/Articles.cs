using System.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace API.Models.Rss
{
    public class Articles
    {        
        public int Id { get; set; }       
        public string? Title { get; set; }        
        public string? Alias { get; set; }
        public int CatId { get; set; }
        public int TotalRows { get; set; } = 0;
        public string? IntroText { get; set; }
        public string? FullText { get; set; }               
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }        
        public string? Images { get; set; }       
        public string? Link { get; set; }       

    }
}
