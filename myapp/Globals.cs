using Microsoft.AspNetCore.Http;

namespace myapp
{
    public static class Globals
    {
        private static readonly IHttpContextAccessor _HttpContextAccessor = new HttpContextAccessor();

        public static readonly Uri baseAddress = new Uri("https://localhost:44350/apiv1");

        public static int? GetHostelID()
        {
            var sessionValue = _HttpContextAccessor.HttpContext?.Session.GetString("HostelID");
            return string.IsNullOrEmpty(sessionValue) ? null : (int?)Convert.ToInt32(sessionValue);
        }

        public static string GetHostelName()
        {
            return _HttpContextAccessor.HttpContext?.Session.GetString("HostelName");
        }

        public static int? GetStudentID()
        {
            var sessionValue = _HttpContextAccessor.HttpContext?.Session.GetString("StudentID");
            return string.IsNullOrEmpty(sessionValue) ? null : (int?)Convert.ToInt32(sessionValue);
        }

        public static string GetStudentName()
        {
            return _HttpContextAccessor.HttpContext?.Session.GetString("StudentName");
        }

        public static string GetStudentImage()
        {
            return _HttpContextAccessor.HttpContext?.Session.GetString("ProfileImage");
        }

        public static int? GetRoomID()
        {
            var sessionValue = _HttpContextAccessor.HttpContext?.Session.GetString("RoomID");
            return string.IsNullOrEmpty(sessionValue) ? null : (int?)Convert.ToInt32(sessionValue);
        }

        public static string GetRoomNumber()
        {
            return _HttpContextAccessor.HttpContext?.Session.GetString("RoomNumber");
        }

        public static int? GetRoomRent()
        {
            var sessionValue = _HttpContextAccessor.HttpContext?.Session.GetString("RoomRent");
            return string.IsNullOrEmpty(sessionValue) ? null : (int?)Convert.ToInt32(sessionValue);
        }
    }
}