using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using myapp.Areas.Student.Models;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;

namespace myapp.Areas.Student.Controllers
{
    [Area("Student")]
    [AreaAuthorization("Student")]
    public class HomeController : Controller
    {
        private readonly HttpClient _client;
        private Uri baseAddress = Globals.baseAddress;

        public HomeController()
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
        }

        public async  Task<IActionResult> Index()
        {
            StudentModel sm = new StudentModel();
            int id = Globals.GetStudentID()!.Value;

            try
            {
                if (id != 0)
                {
                    var response = await _client.GetAsync($"{_client.BaseAddress}/Student/GetStudentID/{id}");

                    if (response.IsSuccessStatusCode)
                    {
                        var jsonData = await response.Content.ReadAsStringAsync();
                        var apiResponse = JsonConvert.DeserializeObject<JObject>(jsonData);

                        if (apiResponse["status"].ToString() == "Success")
                        {
                            sm = JsonConvert.DeserializeObject<StudentModel>(JsonConvert.SerializeObject(apiResponse["data"]));
                        }                       
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Error occurred while fetching student details.";
                    }
                }
                else
                {
                    sm = new StudentModel();
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred: {ex.Message}";
                Console.WriteLine($"Exception: {ex.Message}");
            }
            await NotificationList();
            return View(sm);
        }

        [HttpGet]
        public async Task<IActionResult> NotificationList()
        {
            List<NotificationModel> nList = new List<NotificationModel>();
            try
            {
                var response = await _client.GetAsync($"{_client.BaseAddress}/Notification/GetAllNotifications?id={Globals.GetHostelID().Value}");

                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var jsonData = JsonConvert.DeserializeObject<JObject>(data);

                    if (jsonData["status"]?.ToString().ToLower() == "success")
                    {
                        nList = JsonConvert.DeserializeObject<List<NotificationModel>>(jsonData["data"]?.ToString());
                    }
                }               
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred: {ex.Message}";
                Console.WriteLine($"Exception: {ex}");
            }

            ViewBag.Notification = nList;
            return PartialView("_Header_Student");
        }

        [HttpGet]
        public IActionResult UpdatePassword()
        {
            return View(new StudentUpdatePasswordModel());
        }

        public async Task<IActionResult> UpdatePassword(StudentUpdatePasswordModel sm)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData["ErrorMessage"] = "Incorrect Data Format";
                    return View(sm);
                }

                var json = JsonConvert.SerializeObject(sm);
                var data = new StringContent(json, Encoding.UTF8, "application/json");

                var res = await _client.PutAsync($"{_client.BaseAddress}/Student/StudentUpdatePassword", data);

                if (res.IsSuccessStatusCode)
                {
                    TempData["Message"] = "Password Updated Successfully";
                }
                else
                {
                    TempData["ErrorMessage"] = "Incorrect Password";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred: {ex.Message}";
                Console.WriteLine($"Exception: {ex.Message}");
            }
            return View(sm);
        }
    }
}
