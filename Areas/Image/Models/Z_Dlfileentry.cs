using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Areas.Image.Models
{
    public class Z_Dlfileentry
    {
        public int companyid { get; set; }
        public int folderid { get; set; }
        public string? name { get; set; }
        public string? mimetype { get; set; }
        public string? version { get; set; }
    }
}
