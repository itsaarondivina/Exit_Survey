using System;

namespace VXI_EXIT_INTERVIEW_v2.Controllers
{
    public class OtherDetails
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<System.DateTime> CreatedAt { get; set; }
        public string Hrid { get; set; }
    }
}