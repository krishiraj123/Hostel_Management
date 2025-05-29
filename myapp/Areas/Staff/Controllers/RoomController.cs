using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using myapp.Areas.Staff.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace myapp.Areas.Staff.Controllers
{
    [Area("Staff")]
    [AreaAuthorization("Staff")]
    public class RoomController : Controller
    {
        private readonly HttpClient _httpClient;

        public RoomController()
        {
            _httpClient = new HttpClient { BaseAddress = Globals.baseAddress };
        }

        [HttpGet]
        public async Task<IActionResult> RoomList()
        {
            List<RoomModel> rooms = new List<RoomModel>();

            try
            {
                var response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}/Room/GetAllRooms?id={Globals.GetHostelID().Value}");
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var jsonData = JsonConvert.DeserializeObject<JObject>(data);

                    if (jsonData?["status"]?.ToString() == "Success")
                    {
                        rooms = JsonConvert.DeserializeObject<List<RoomModel>>(jsonData["data"].ToString());
                    }
                    //else
                    //{
                    //    TempData["ErrorMessage"] = "Failed to retrieve data.";
                    //}
                }
                else
                {
                    TempData["ErrorMessage"] = "No data found!";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred: {ex.Message}";
            }

            return View(rooms);
        }

        [HttpGet]
        public async Task<IActionResult> RoomAddEdit(int? id)
        {
            RoomModel room = new RoomModel();

            try
            {
                if (id.HasValue)
                {
                    var response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}/Room/GetRoomsByID/{id}");
                    if (response.IsSuccessStatusCode)
                    {
                        var data = await response.Content.ReadAsStringAsync();
                        var jsonData = JsonConvert.DeserializeObject<JObject>(data);

                        if (jsonData?["status"]?.ToString() == "Success")
                        {
                            room = JsonConvert.DeserializeObject<RoomModel>(jsonData["data"].ToString());
                        }
                        else
                        {
                            TempData["ErrorMessage"] = "Failed to retrieve room details.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred: {ex.Message}";
            }

            return View(room);
        }

        [HttpPost]
        public async Task<IActionResult> RoomSave(RoomModel room)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData["ErrorMessage"] = "Invalid form submission.";
                    foreach (var entry in ModelState)
                    {
                        if (entry.Value.Errors.Count > 0)
                        {
                            Console.WriteLine($"Key: {entry.Key}, Errors: {string.Join(", ", entry.Value.Errors.Select(e => e.ErrorMessage))}");
                        }
                    }
                    return RedirectToAction("RoomAddEdit", room);
                }

                string jsonData = JsonConvert.SerializeObject(room);
                Console.WriteLine($"Sending Data: {jsonData}");

                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                HttpResponseMessage response;
                bool isUpdate = room.RoomID.HasValue && room.RoomID > 0;

                if (isUpdate)
                {
                    response = await _httpClient.PutAsync($"{_httpClient.BaseAddress}/Room/UpdateRoom/{room.RoomID}", content);
                }
                else
                {
                    response = await _httpClient.PostAsync($"{_httpClient.BaseAddress}/Room/InsertRoom", content);
                }

                string responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Response: {response.StatusCode}, {responseContent}"); // Log response for debugging

                if (response.IsSuccessStatusCode)
                {
                    TempData["Message"] = isUpdate ? "Room updated successfully." : "Room added successfully.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to save room.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred: {ex.Message}";
            }

            return RedirectToAction("RoomList");    
        }


        public async Task<IActionResult> DeleteRoom(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{_httpClient.BaseAddress}/Room/DeleteRoom/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var jsonData = JsonConvert.DeserializeObject<JObject>(data);

                    if (jsonData?["status"]?.ToString() == "Success")
                    {
                        TempData["Message"] = "Room deleted successfully.";
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Failed to delete room.";
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to delete room from the server.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred: {ex.Message}";
            }

            return RedirectToAction("RoomList");
        }
    }
}
