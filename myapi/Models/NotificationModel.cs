using System.ComponentModel.DataAnnotations;

namespace myapi.Models
{
	public class NotificationModel
	{
		public int? NotificationID { get; set; }		
		public string Title { get; set; }		
		public string Message { get; set; }
		public DateTime? SentAt { get; set; } = DateTime.Now;
		public int? NoOfDays { get; set; } = 1;
		public int? HostelID { get; set; }
		public string? HostelName { get; set; }
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
