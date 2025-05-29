namespace myapi.Models
{
    public class DashboardCounts
    {
        public string Metrics { get; set; }
        public int Value { get; set; }
    }

    public class NewestAdmission
    {
        public string StudentImage { get; set; }
        public string StudentName { get; set; }
        public DateTime AdmissionDate { get; set; }
        public string RoomNumber { get; set; }
        public int RoomCapacity { get; set; }
    }

    public class LatestComplaints
    {
        public string StudentName { get; set; }
        public DateTime PostedAt { get; set; }
        public string ComplaintBody { get; set; }
        public string ComplaintStatus { get; set; }
        public string RoomNumber { get; set; }
        public string ComplainSubject { get; set; }
        public string ProfileImage { get; set; }
    }

    public class ReservationStatistic
    {
        public string CountOfStudent { get; set; }
    }

    public class DashboardModel
    {        
        public List<DashboardCounts> Counts { get; set; }
        public List<NewestAdmission> NewestAdmission { get; set; }
        public List<LatestComplaints> LatestComplaints { get; set; }
        public List<ReservationStatistic> ReservationStatistic { get; set; }
    }
}
