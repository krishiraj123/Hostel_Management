using Microsoft.AspNetCore.Mvc;
using myapp.Areas.Staff.Models;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using myapp.Areas.Student.Models;

namespace myapp.Areas.Student.Controllers
{
    [Area("Student")]
    [AreaAuthorization("Student")]
    public class RoomController : Controller
    {
        private readonly HttpClient _httpClient;

        public RoomController()
        {
            _httpClient = new HttpClient { BaseAddress = Globals.baseAddress };
        }

        [HttpGet]
        public async Task<IActionResult> RoomMatesList()
        {
            List<RoomMateModel> roomMates = new List<RoomMateModel>();

            try
            {
                int hostelId = Globals.GetHostelID().Value;
                int roomId = Globals.GetRoomID().Value;

                var response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}/Room/GetRoomMates?roomId={roomId}&hostelId={hostelId}");

                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var jsonData = JsonConvert.DeserializeObject<JObject>(data);

                    if (jsonData?["status"]?.ToString() == "Success")
                    {
                        roomMates = JsonConvert.DeserializeObject<List<RoomMateModel>>(jsonData["data"].ToString());
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = "Unable to fetch roommates information";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred: {ex.Message}";
            }

            return View(roomMates);
        }
    }
}