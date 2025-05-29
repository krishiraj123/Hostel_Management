using System.Text;
using Microsoft.AspNetCore.Mvc;
using myapp.Areas.Staff.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace myapp.Areas.Staff.Controllers
{
    [Area("Staff")]
    [AreaAuthorization("Staff")]
    public class NotificationController : Controller
    {
        private readonly HttpClient _httpClient;

        public NotificationController()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = Globals.baseAddress
            };
        }

        [HttpGet]
        public async Task<IActionResult> NotificationList()
        {
            List<NotificationModel> nList = new List<NotificationModel>();
            try
            {
                var response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}/Notification/GetAllNotifications?id={Globals.GetHostelID().Value}");

                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var jsonData = JsonConvert.DeserializeObject<JObject>(data);

                    if (jsonData["status"]?.ToString().ToLower() == "success")
                    {
                        nList = JsonConvert.DeserializeObject<List<NotificationModel>>(jsonData["data"]?.ToString());
                    }
                }
                //else
                //{
                //    TempData["ErrorMessage"] = "Failed to fetch notifications.";
                //}
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred: {ex.Message}";
                Console.WriteLine($"Exception: {ex}");
            }

            return View(nList);
        }
        
        public async Task<IActionResult> NotificationAddEdit(int? id)
        {
            NotificationModel nm = new NotificationModel();
            try
            {
                var response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}/Notification/GetNotificationById/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var jsonData = JsonConvert.DeserializeObject<JObject>(data);

                    if (jsonData["status"]?.ToString().ToLower() == "success")
                    {
                        nm = JsonConvert.DeserializeObject<NotificationModel>(jsonData["data"]?.ToString());
                    }
                }                
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred: {ex.Message}";
                Console.WriteLine($"Exception: {ex}");
            }

            return View(nm);
        }

        [HttpPost]
        public async Task<IActionResult> NotificationSave(NotificationModel nm)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData["ErrorMessage"] = "Please correct the errors.";
                    return View("NotificationAddEdit", nm);
                }

                string jsonData = JsonConvert.SerializeObject(nm);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                HttpResponseMessage response;
                bool isUpdate = nm.NotificationID.HasValue && nm.NotificationID > 0;

                if (isUpdate)
                {
                    response = await _httpClient.PutAsync($"{_httpClient.BaseAddress}/Notification/UpdateNotification/{nm.NotificationID}", content);
                }
                else
                {
                    response = await _httpClient.PostAsync($"{_httpClient.BaseAddress}/Notification/AddNotification", content);
                }

                if (response.IsSuccessStatusCode)
                {
                    TempData["Message"] = $"Notification {(isUpdate ? "Updated" : "Inserted")} Successfully.";
                }
                else
                {
                    TempData["ErrorMessage"] = $"Failed to {(isUpdate ? "Update" : "Insert")} Notification.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred: {ex.Message}";
                Console.WriteLine($"Exception: {ex}");
            }

            return RedirectToAction("NotificationList");
        }

        public async Task<IActionResult> DeleteNotification(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{_httpClient.BaseAddress}/Notification/DeleteNotification/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var jsonData = await response.Content.ReadAsStringAsync();

                    var jsonResponse = JsonConvert.DeserializeObject<JObject>(jsonData);

                    if (jsonResponse != null && jsonResponse["status"]?.ToString() == "Failure")
                    {
                        TempData["ErrorMessage"] = jsonResponse["message"]?.ToString() ?? "Failed to delete the student.";
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = $"Failed to delete the student. HTTP Status: {response.StatusCode}";
                }
            }          
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An unexpected error occurred: {ex.Message}";
                Console.WriteLine($"Exception: {ex}");
            }

            return RedirectToAction("NotificationList", "Notification", new { area = "Staff" });
        }
    }
}
