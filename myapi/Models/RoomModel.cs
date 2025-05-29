using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace myapi.Models
{
    public class RoomModel
    {       
        public int? RoomID { get; set; }
                
        public string RoomNumber { get; set; }
        
        public int RoomCapacity { get; set; }
        
        public int? CurrentVacancy { get; set; }
        
        public int RoomFloor { get; set; }
        
        public int RoomRent { get; set; }        
        
        public string RoomType { get; set; }

        public string? RoomStatus { get; set; } = "Vacant";
       
        public int HostelID { get; set; }

        public DateTime? CreatedAt { get; set; } = DateTime.Now;

        public DateTime? UpdatedAt { get; set; } = DateTime.Now;
        
        public string? HostelName { get; set; }
    }

    public class RoomAddEditModel
    {
		public int? RoomID { get; set; }

		public string RoomNumber { get; set; }

		public int RoomCapacity { get; set; }

		public int? CurrentVacancy { get; set; }

		public int RoomFloor { get; set; }

		public int RoomRent { get; set; }

		public string RoomType { get; set; }

		public string RoomStatus { get; set; }

		public int HostelID { get; set; }
	}

    public class AvaiableRoomModel
    {
        public int RoomID { get; set; }
        public string RoomNumber { get; set; }
    }

    public class RoomMatesModel
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
        public string StudentInstituteName { get; set; }
        public string HostelName { get; set; }
        public string ProfileImage { get; set; }
    }
}

