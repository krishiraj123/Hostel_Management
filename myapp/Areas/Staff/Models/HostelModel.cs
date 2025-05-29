using System.ComponentModel.DataAnnotations;

namespace myapp.Areas.Staff.Models
{
    public class HostelModel
    {
        public int? HostelID { get; set; } = 0;

        public string HostelName { get; set; }

        public string HostelAddress { get; set; }

        public string HostelContactNumber { get; set; }

        public string HostelEmail { get; set; }

        public string HostelAdminPassword { get; set; }

        public string HostelPincode { get; set; }

        public string HostelCity { get; set; }

        public string HostelState { get; set; }

        public string HostelCountry { get; set; }

        public int HostelCapacity { get; set; }

        public string Amenities { get; set; }

        public string HostelPolicies { get; set; }

        public string HostelType { get; set; }

        public DateTime? CreatedAt { get; set; } = DateTime.Now;

        public DateTime? UpdatedAt { get; set; } = DateTime.Now;
        public int TotalRooms { get; set; }
        public int VacantRooms { get; set; }
        public int FullRooms { get; set; }
    }

    public class HostelUpdatePasswordModel
    {
        public int? HostelID = Globals.GetHostelID().Value;
        [Required]
        public string CurrentPassword { get; set; }
        [Required]
        [Compare("ConfirmPassword", ErrorMessage = "New Password and Confirm Password Must be Equal")]
        public string NewPassword { get; set; }
        [Required]
        [Compare("NewPassword",ErrorMessage ="New Password and Confirm Password Must be Equal")]
        public string ConfirmPassword { get; set; }
    }
}

