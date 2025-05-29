using Microsoft.Build.ObjectModelRemoting;

namespace myapp.Areas.Student.Models
{
    public class RoomMateModel
    {
        public string StudentName { get; set; }
        public string StudentEmail { get; set; }
        public string StudentPhoneNumber { get; set; }
        public string StudentCity { get; set; }
        public string StudentState { get; set; }
        public string StudentCountry { get; set; }
        public DateTime StudentDOB { get; set; }
        public string EmergencyContactNumber { get; set; }
        public string GuardianName { get; set; }
        public string GuardianPhoneNumber { get; set; }
        public string StudentEducationStatus { get; set; }
        public string StudentInstituteName {get;set;}
        public string HostelName { get; set; }
        public string ProfileImage { get; set; }
    }
}
