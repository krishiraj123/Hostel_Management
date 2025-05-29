using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;

namespace myapp.Areas.Staff.Models
{
    public class StudentModel
    {
        public int? StudentID { get; set; }

        [Required(ErrorMessage = "Student name is required.")]
        public string StudentName { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Enter a valid 10-digit phone number.")]
        public string StudentPhoneNumber { get; set; }

        [Required(ErrorMessage = "Email address is required.")]
        [EmailAddress(ErrorMessage = "Enter a valid email address.")]
        public string StudentEmail { get; set; }

        public string StudentAddress { get; set; }

        [Required(ErrorMessage = "Gender is required.")]
        public string StudentGender { get; set; }

        [Required(ErrorMessage = "Date of Birth is required.")]
        [DataType(DataType.Date, ErrorMessage = "Enter a valid date.")]
        public DateTime? StudentDOB { get; set; }

        [Required(ErrorMessage = "Education status is required.")]
        public string StudentEducationStatus { get; set; } // E.g., School/College/Working

        public string StudentInstituteName { get; set; }

        [Required(ErrorMessage = "Emergency contact number is required.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Enter a valid 10-digit phone number.")]
        public string EmergencyContactNumber { get; set; }

        public string StudentCity { get; set; }

        public string StudentState { get; set; }

        public string StudentCountry { get; set; }

        [RegularExpression(@"^\d{6}$", ErrorMessage = "Enter a valid 6-digit pincode.")]
        public string StudentPincode { get; set; }

        public string GuardianName { get; set; }

        [RegularExpression(@"^\d{10}$", ErrorMessage = "Enter a valid 10-digit phone number.")]
        public string GuardianPhoneNumber { get; set; }

        [DataType(DataType.Date, ErrorMessage = "Enter a valid date.")]
        public DateTime? AdmissionDate { get; set; } = DateTime.Now;

        [BindProperty]
        public string? ProfileImage { get; set; }

        [NotMapped] 
        public IFormFile? ProfileImageFile { get; set; }

        public string? StudentPassword { get; set; } = "demopassword";

        public int? RoomID { get; set; }

        public int? HostelID { get; set; }

        public bool IsDeleted { get; set; } = false;

        public DateTime? CreatedAt { get; set; } = DateTime.Now;

        public DateTime? UpdatedAt { get; set; } = DateTime.Now;
        
        public string? HostelName { get; set; }
        public string? RoomNumber { get; set; }
    }
}
