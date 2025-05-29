using System;
using System.ComponentModel.DataAnnotations;

namespace myapp.Areas.Staff.Models
{
    public class RoomModel
    {
        public int? RoomID { get; set; } = 0;

        [Required(ErrorMessage = "Room number is required.")]
        public string RoomNumber { get; set; }

        [Required(ErrorMessage = "Room capacity is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Room capacity must be at least 1.")]
        public int RoomCapacity { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true)]
        public int? CurrentVacancy { get; set; } = 0;

        [Required(ErrorMessage = "Room floor is required.")]
        [Range(0, 100, ErrorMessage = "Room floor must be between 0 and 100.")]
        public int RoomFloor { get; set; }

        [Required(ErrorMessage = "Room rent is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Room rent must be a positive value.")]
        public int RoomRent { get; set; }

        [Required(ErrorMessage = "Room type is required.")]
        public string RoomType { get; set; }

        public string? RoomStatus { get; set; } = "Vacant";

        [Required(ErrorMessage = "Hostel ID is required.")]
        public int HostelID { get; set; } = Globals.GetHostelID()!.Value;

        [DataType(DataType.DateTime)]   
        public DateTime? CreatedAt { get; set; } = DateTime.Now;

        [DataType(DataType.DateTime)]
        public DateTime? UpdatedAt { get; set; } = DateTime.Now;

        public string? HostelName { get; set; }
    }

    public class AvailableRoomModel
    {
        [Required]
        public int RoomID { get; set; }

        [Required]
        public string RoomNumber { get; set; }
    }
}
