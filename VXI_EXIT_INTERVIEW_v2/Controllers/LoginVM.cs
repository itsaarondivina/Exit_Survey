using System;

namespace VXI_EXIT_INTERVIEW_v2.Controllers
{
    public class LoginVM
    {
        public string Hrid { get; set; }
        public string BirthDate { get; set; }
    }
    public class InfoModel
    {
        public string id { get; set; }
        public string projectId { get; set; }
        public string firstName { get; set; }
        public string middleName { get; set; }
        public string lastName { get; set; }
        public string gender { get; set; }
        public DateTime dateBirth { get; set; }
        public string buildingAssignment { get; set; }
        public string lineOfBusiness { get; set; }
        public string position { get; set; }
        public string division { get; set; }
        public string team { get; set; }
        public string positionLevel { get; set; }
        public DateTime dateHired { get; set; }
        public string fileImage { get; set; }
        public string supervisorID { get; set; }
        public string employeeStatus { get; set; }
        public string windowsNT { get; set; }
        public string separatedDate { get; set; }
        public string Email { get; set; }
    }
}