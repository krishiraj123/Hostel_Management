using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using myapp.Areas.Student.Models;

namespace myapp.Areas.Student.Controllers
{
    [Area("Student")]
    [AreaAuthorization("Student")]
    public class FoodTimeTableController : Controller
    {
        private readonly HttpClient _httpClient;
        private Uri baseAddress = Globals.baseAddress;

        public FoodTimeTableController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = baseAddress;
        }

        [HttpGet]
        public async Task<IActionResult> FoodTimeTableList()
        {      
            int hostelId = Globals.GetHostelID().Value;
            List<FoodTimeTableModel> timetable = new List<FoodTimeTableModel>();
            try
            {
                var response = await _httpClient.GetAsync(
                    $"{_httpClient.BaseAddress}/FoodTimeTable/GetTimetable/{hostelId}");

                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var jsonData = JsonConvert.DeserializeObject<JObject>(data);

                    if (jsonData["status"]?.ToString().ToLower() == "success")
                    {
                        timetable = JsonConvert.DeserializeObject<List<FoodTimeTableModel>>(
                            jsonData["data"]?.ToString());
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to fetch timetable.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred: {ex.Message}";
                Console.WriteLine($"Exception: {ex}");
            }

            return View(timetable);
        }
    }
}
