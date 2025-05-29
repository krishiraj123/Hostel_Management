using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using myapp.Areas.Student.Models;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text;

namespace myapp.Areas.Student.Controllers
{
    [Area("Student")]
    [AreaAuthorization("Student")]
    public class ComplaintController : Controller
    {
        private readonly HttpClient _httpClient;

        public ComplaintController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = Globals.baseAddress;
        }

        [HttpGet]
        public async Task<IActionResult> ComplaintList()
        {
            List<ComplaintModel> cm = new List<ComplaintModel>();
            try
            {
                var res = await _httpClient.GetAsync($"{_httpClient.BaseAddress}/Complaints/GetAllComplaintsRoomWise?hostelID={Globals.GetHostelID()!.Value}&roomID={Globals.GetRoomID()!.Value}");

                if (res.IsSuccessStatusCode)
                {
                    var data = await res.Content.ReadAsStringAsync();
                    var jsonData = JsonConvert.DeserializeObject<JObject>(data);

                    if (jsonData["status"].ToString() == "Success")
                    {
                        cm = JsonConvert.DeserializeObject<List<ComplaintModel>>(jsonData["data"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex;
            }

            return View(cm);
        }

        public async Task<IActionResult> ComplainAddEdit(int? id)
        {
            ComplainAddEditModel cm = new ComplainAddEditModel();
            try
            {
                var res = await _httpClient.GetAsync($"{_httpClient.BaseAddress}/Complaints/GetComplaintsByID/{id}");

                if (res.IsSuccessStatusCode)
                {
                    var data = await res.Content.ReadAsStringAsync();
                    var jsonData = JsonConvert.DeserializeObject<JObject>(data);

                    if (jsonData["status"].ToString() == "Success")
                    {
                        cm = JsonConvert.DeserializeObject<ComplainAddEditModel>(jsonData["data"].ToString());
                    }
                }

            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex;
            }

            return View(cm);
        }

        [HttpPost]
        public async Task<IActionResult> SaveComplaint(ComplainAddEditModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData["ErrorMessage"] = "Please correct the form errors";
                    return View("ComplaintAddEdit", model);
                }
               
                HttpResponseMessage response;
                bool isEdit = model.ComplainID.HasValue && model.ComplainID > 0;

                if (isEdit)
                {
                    var jsonData = JsonConvert.SerializeObject(model);
                    var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                    response = await _httpClient.PostAsync(
                        $"{_httpClient.BaseAddress}/Complaints/StudentComplainAddEdit/{model.ComplainID}", content);
                }
                else
                {
                    model.HostelID = Globals.GetHostelID()!.Value;
                    model.StudentID = Globals.GetStudentID()!.Value;
                    model.RoomID = Globals.GetRoomID()!.Value;

                    var jsonData = JsonConvert.SerializeObject(model);
                    var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                    response = await _httpClient.PostAsync(
                    $"{_httpClient.BaseAddress}/Complaints/StudentComplainAdd", content);
                }

                if (response.IsSuccessStatusCode)
                {
                    TempData["Message"] = $"Complaint {(isEdit ? "updated" : "submitted")} successfully";
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    TempData["ErrorMessage"] = $"Failed to {(isEdit ? "update" : "submit")} complaint. as it already completed";
                    Console.WriteLine($"API Error: {errorContent}");
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error: {ex.Message}";
                Console.WriteLine($"Exception: {ex}");
            }

            return RedirectToAction("ComplaintList");
        }

        public async Task<IActionResult> DeleteComplaint(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync(
                    $"{_httpClient.BaseAddress}/Complaints/StudentDeleteComplaintPermanently/{id}");

                if (response.IsSuccessStatusCode)
                {
                    TempData["Message"] = "Complaint deleted successfully";
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    var jsonResponse = JObject.Parse(errorContent);

                    TempData["ErrorMessage"] = jsonResponse?["message"]?.ToString()
                        ?? $"Failed to delete complaint. Status: {response.StatusCode}";
                }
            }
            catch (HttpRequestException httpEx)
            {
                TempData["ErrorMessage"] = $"Connection error: {httpEx.Message}";
                Console.WriteLine($"HTTP Error: {httpEx}");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Unexpected error: {ex.Message}";
                Console.WriteLine($"Exception: {ex}");
            }

            return RedirectToAction("ComplaintList");
        }
    }
}
