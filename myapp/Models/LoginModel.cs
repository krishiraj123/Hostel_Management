using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace myapp.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Username is required")]
        [EmailAddress]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }        
        public string? UserType { get; set; } //Staff, Student
    }

    public class ApiResponse
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public Dictionary<string,object> Data { get; set; }
    }
}
