using System;

namespace VXI_EXIT_INTERVIEW_v2.Controllers
{
    public class SurveyAnswers
    {
        public int Id { get; set; }
        //public System.Guid UserAnswerID { get; set; }
        public Nullable<System.Guid> AccountId { get; set; }
        public Nullable<System.Guid> AnswerID { get; set; }
        public Nullable<System.Guid> QuestionID { get; set; }
        public Nullable<System.Guid> SurveyID { get; set; }
        public Nullable<System.DateTime> CreatedAt { get; set; }
        public Nullable<System.Guid> Createdby { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public string OtherResponse { get; set; }
    }
}