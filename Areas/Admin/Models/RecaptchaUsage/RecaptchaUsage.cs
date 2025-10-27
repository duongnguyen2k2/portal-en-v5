using System;

namespace API.Areas.Admin.Models.RecaptchaUsage
{
    public class RecaptchaUsage
    {
        public int Id { get; set; }
        public int Month { get; set; }  // Tháng (1-12)
        public int Year { get; set; }   // Năm
        public long Count { get; set; } = 0;  // Số assessments
        public int IdCoQuan { get; set; } = 0;  // Multi-tenant nếu cần
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
