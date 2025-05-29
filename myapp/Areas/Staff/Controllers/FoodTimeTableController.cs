using System.Text;
using Microsoft.AspNetCore.Mvc;
using myapp.Areas.Staff.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OfficeOpenXml;

namespace myapp.Areas.Staff.Controllers
{
    [Area("Staff")]
    [AreaAuthorization("Staff")]
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

        [HttpPost("upload/")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadTimetable(IFormFile timetableFile)
        {
            int? hostelId = Globals.GetHostelID();
            if (!hostelId.HasValue)
            {
                TempData["ErrorMessage"] = "Hostel ID not found";
                return RedirectToAction("FoodTimeTableList");
            }

            try
            {
                if (timetableFile == null || timetableFile.Length == 0)
                {
                    TempData["ErrorMessage"] = "Please select a file to upload";
                    return RedirectToAction("FoodTimeTableList");
                }

                using var content = new MultipartFormDataContent();
                var fileContent = new StreamContent(timetableFile.OpenReadStream())
                {
                    Headers =
                    {
                        ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("form-data")
                        {
                            Name = "file",
                            FileName = timetableFile.FileName
                        }
                    }
                };
                content.Add(fileContent, "file", timetableFile.FileName);
                
                var response = await _httpClient.PostAsync(
                    $"{_httpClient.BaseAddress}/FoodTimeTable/UploadTimetable/{hostelId.Value}",
                    content);

                var responseContent = await response.Content.ReadAsStringAsync();
                var jsonResponse = JObject.Parse(responseContent);

                if (response.IsSuccessStatusCode)
                {
                    TempData["Message"] = jsonResponse?["message"]?.ToString() ?? "Timetable uploaded successfully!";
                }
                else
                {
                    TempData["ErrorMessage"] = jsonResponse?["message"]?.ToString()
                        ?? $"Upload failed. Status code: {response.StatusCode}";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred: {ex.Message}";
                Console.WriteLine($"Exception: {ex}");
            }

            return RedirectToAction("FoodTimeTableList");
            }


        public IActionResult FoodTimetableUpload(int hostelId)
        {   
            ViewBag.HostelId = hostelId;
            return View();
        }
    }
}