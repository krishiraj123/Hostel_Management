using System.Text;
using Microsoft.AspNetCore.Mvc;
using myapp.Areas.Staff.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace myapp.Areas.Staff.Controllers
{
    [Area("Staff")]
    [AreaAuthorization("Staff")]
    public class HomeController : Controller
    {
        private readonly HttpClient _client;
        public HomeController()
        {
            _client = new HttpClient();
            _client.BaseAddress = Globals.baseAddress;
        }
        public async Task<IActionResult> Index()
        {
            DashboardModel dashboardModel = new DashboardModel
            {
                Counts = new List<DashboardCounts>(),
                NewestAdmission = new List<NewestAdmission>(),
                LatestComplaints = new List<LatestComplaints>(),
                ReservationStatistic = new List<ReservationStatistic>(),
            };
            try
            {
                var res = await _client.GetAsync($"{_client.BaseAddress}/Dashboard/GetDashboardData");

                if(res.IsSuccessStatusCode)
                {
                    var jsonData = await res.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<JObject>(jsonData);

                    if (data["status"].ToString() == "Success" && data["data"] != null)
                    {
                        dashboardModel = JsonConvert.DeserializeObject<DashboardModel>(data["data"].ToString());
                    }
                }
            }
            catch(Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred: {ex.Message}";
                Console.WriteLine($"Exception: {ex.Message}");
            }
            return View(dashboardModel);
        }

        public async Task<IActionResult> Profile()
        {
            HostelModel hm = new HostelModel();
            var id = Globals.GetHostelID().Value;

            if (id == null || id == 0)
            {                
                TempData["ErrorMessage"] = "Hostel ID not found in session.";
                return RedirectToAction("Error");
            }

            try
            {
                var res = await _client.GetAsync($"{_client.BaseAddress}/Hostels/HostelSelectByID/{id}");

                if(res.IsSuccessStatusCode)
                {
                    var data = await res.Content.ReadAsStringAsync();
                    var jsonData = JsonConvert.DeserializeObject<JObject>(data);

                    if (jsonData["status"].ToString() == "Success")
                    {
                       hm = JsonConvert.DeserializeObject<HostelModel>(JsonConvert.SerializeObject(jsonData["data"]));
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred: {ex.Message}";
                Console.WriteLine($"Exception: {ex.Message}");
            }
            return View(hm);
        }

        [HttpGet]
        public IActionResult UpdatePassword()
        {
            return View(new HostelUpdatePasswordModel());
        }
     
        public async Task<IActionResult> UpdatePassword(HostelUpdatePasswordModel hm)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData["ErrorMessage"] = $"Incorrect Data Format";
                    return View("UpdatePassword", hm);
                }

                var json = JsonConvert.SerializeObject(hm);
                var data = new StringContent(json, Encoding.UTF8, "application/json");

                var res = await _client.PutAsync($"{_client.BaseAddress}/Hostels/HostelUpdatePassword", data);

                if (res.IsSuccessStatusCode)
                {
                    TempData["Message"] = $"Password Updated Successfully";
                }
                else
                {
                    TempData["ErrorMessage"] = $"Incorrect Password";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred: {ex.Message}";
                Console.WriteLine($"Exception: {ex.Message}");
            }
            return View(hm);
        }
    }
}
