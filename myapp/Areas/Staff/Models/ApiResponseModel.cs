namespace myapp.Areas.Staff.Models
{
    public class ApiResponseModel
    {
        public string Status { get; set; }
        public object Data { get; set; }
        public string Message { get; set; }
    }
}
