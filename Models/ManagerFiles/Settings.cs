using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models.ManagerFiles
{
    public class Settings
    {
        public string? Email { get; set; }
        public string? EmailPass { get; set; }
        public string? Secret { get; set; }
        public string? SecretId { get; set; }
    }
}