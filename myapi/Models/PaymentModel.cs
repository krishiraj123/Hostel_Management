namespace myapi.Models
{
    public class PaymentModel
    {
        public int PaymentID { get; set; }
        public string TransactionID { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; } = DateTime.Now;
        public string PaymentStatus { get; set; }      
        public int StudentID { get; set; }
        public string? StudentName { get; set; }
        public string? StudentEmail { get; set; }      
        public string? StudentEducationStatus { get; set; }
        public string? StudentInstituteName { get; set; }
        public string? StudentPhoneNumber { get; set; }

        public int RoomID { get; set; }
        public string? RoomNumber { get; set; }
        public int? RoomCapacity { get; set; }
        public int? CurrentVacancy { get; set; }
        public int? RoomRent { get; set; }
        public string? RoomType { get; set; }

        public int HostelID { get; set; }
        public string? HostelName { get; set; }
        public string? HostelContactNumber { get; set; }
        public string? HostelEmail { get; set; }

        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class PaymentCreateModel
    {
        public int StudentID { get; set; }
        public int RoomID { get; set; }
        public int HostelID { get; set; }
        public decimal Amount { get; set; }
    }  
}
