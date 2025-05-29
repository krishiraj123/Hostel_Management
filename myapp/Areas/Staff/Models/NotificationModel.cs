using System.ComponentModel.DataAnnotations;

namespace myapp.Areas.Staff.Models
{  
    public class NotificationModel
    {
        public int? NotificationID { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Message { get; set; }
        public DateTime? SentAt { get; set; } = DateTime.Now;
        [Required(ErrorMessage="No. of days to show notification is required")]
        [Range(1,100,ErrorMessage="No. of Days to show must be between 1-100 days")]
        public int? NoOfDays { get; set; } = 0;
        public int? HostelID { get; set; }
        public string? HostelName { get; set; } = Globals.GetHostelName();
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; } = DateTime.Now;
    }

    public class NotificationAddEditModel()
    {
        public int? NotificationID { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public int? NoOfDays { get; set; } = 1;
        public int? HostelID { get; set; }
    }
}

