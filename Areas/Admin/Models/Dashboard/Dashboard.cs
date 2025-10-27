using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Areas.Admin.Models.Dashboard
{
    public class Dashboard
    {

    }

    public class ReportArticle
    {
        public string? ShowStartDate { get; set; }
        public string? ShowEndDate { get; set; }
        public int IdCoQuan { get; set; }
        public int Nam { get; set; }
    }

    public class CountArticle
    {
        public string? Title { get; set; }
        public int SoBaiViet { get; set; }
        public int SoHinhAnh { get; set; }
        public int IdCoQuan { get; set; }
    }
    public class ReportMonth
    {
        public int ThangDonVi { get; set; }
        public int ThangHuyen { get; set; }
        public int ThangTinh { get; set; }
        public int ThangTatCa { get; set; }
    }
}
