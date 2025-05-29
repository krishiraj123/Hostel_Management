using System.Diagnostics;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using myapp.Models;
using Newtonsoft.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace myapp.Controllers
{
    public class HomeController : Controller
    {
        private readonly JwtService _jwtService;
        private readonly HttpClient _client;

        private Uri baseAddress = Globals.baseAddress;

        public HomeController(JwtService jwtService)
        {
            _jwtService = jwtService;
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View(new LoginModel());
        }

        [HttpPost]
        public async Task<IActionResult> LoginUser(LoginModel lm)
        {
            if (!ModelState.IsValid) 
            {
                return RedirectToAction("Login");
            }
            
            if (lm.UserType == "Staff")
            {
                var staffModel = new LoginModel
                {
                    Username = lm.Username,
                    Password = lm.Password
                };

                var jsonData = JsonConvert.SerializeObject(staffModel);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                var res = await _client.PostAsync($"{_client.BaseAddress}/Hostels/HostelLogin", content);

                if (res.IsSuccessStatusCode)
                {
                    var resBody = await res.Content.ReadAsStringAsync();
                    var resData = JsonConvert.DeserializeObject<ApiResponse>(resBody);

                    if (resData.Status == "Success")
                    {
                        var data = JsonConvert.DeserializeObject<Dictionary<string,dynamic>>(JsonConvert.SerializeObject(resData.Data));
                        string hostelID = data["HostelID"].ToString();
                        string hostelName = data["HostelName"].ToString();

                        HttpContext.Session.SetString("HostelID", hostelID);
                        HttpContext.Session.SetString("HostelName", hostelName);

                        var token = _jwtService.GenerateToken(lm.Username, "Staff");
                        HttpContext.Response.Cookies.Append("AuthToken", token);
                        HttpContext.Response.Cookies.Append("UserType", "Staff");

                        return RedirectToActionPermanent("Index", "Home", new { area = "Staff" });
                    }
                    else
                    {
                        TempData["Error"] = "Invalid email or password.";
                    }
                }
                else
                {
                    TempData["Error"] = "Invalid email or password.";
                }
                return RedirectToAction("Login");
            }
            else if (lm.UserType == "Student")
            {
                var studentModel = new LoginModel
                {
                    Username = lm.Username,
                    Password = lm.Password
                };
                var jsonData = JsonConvert.SerializeObject(studentModel);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                var res = await _client.PostAsync($"{_client.BaseAddress}/Student/StudentLogin",content);

                if (res.IsSuccessStatusCode)
                {
                    var responseBody = await res.Content.ReadAsStringAsync();
                    var responseData = JsonConvert.DeserializeObject<ApiResponse>(responseBody);
                    
                    if (responseData.Status == "Success")
                    {
                        var dataDict = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(JsonConvert.SerializeObject(responseData.Data));

                        string hostelID = dataDict["HostelID"].ToString();
                        string studentID = dataDict["StudentID"].ToString();
                        string roomID = dataDict["RoomID"].ToString();
                        string studentName = dataDict["StudentName"].ToString();
                        string roomNumber = dataDict["RoomNumber"].ToString();
                        string roomRent = dataDict["RoomRent"].ToString();
                        string profileImage = dataDict["ProfileImage"].ToString();

                        HttpContext.Session.SetString("HostelID", hostelID.ToString());
                        HttpContext.Session.SetString("StudentID", studentID.ToString());
                        HttpContext.Session.SetString("RoomID", roomID.ToString());
                        HttpContext.Session.SetString("StudentName", studentName.ToString());
                        HttpContext.Session.SetString("RoomNumber", roomNumber.ToString());
                        HttpContext.Session.SetString("RoomRent", roomRent.ToString());
                        HttpContext.Session.SetString("ProfileImage", profileImage.ToString());

                        var token = _jwtService.GenerateToken(lm.Username, "Student");
                        HttpContext.Response.Cookies.Append("AuthToken", token);
                        HttpContext.Response.Cookies.Append("UserType", "Student");

                        return RedirectToActionPermanent("Index", "Home", new { area = "Student" });
                    }
                    else
                    {
                        TempData["Error"] = "Invalid email or password.";
                    }
                }
                else
                {
                    TempData["Error"] = "Invalid email or password.";
                }
                return RedirectToAction("Login");
            }

            ViewBag.Error = "Invalid email or password.";            
            return RedirectToAction("Login",lm);
        }

        public IActionResult Logout()
        {
            HttpContext.Response.Cookies.Delete("AuthToken");
            HttpContext.Response.Cookies.Delete("UserType");
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Home");
        }
    }
}
