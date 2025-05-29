using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using myapp.Areas.Student.Models;
using Newtonsoft.Json;
using System.Text.Json.Nodes;
using Newtonsoft.Json.Linq;

namespace myapp.Areas.Student.Controllers
{
    [Area("Student")]
    [AreaAuthorization("Student")]
    public class PaymentController : Controller
    {
        private readonly HttpClient _httpClient;

        public PaymentController()
        {
            _httpClient = new HttpClient { BaseAddress = Globals.baseAddress };
        }
        public IActionResult Index()
        {
            var model = new PaymentModel
            {
                StudentID = Convert.ToInt32(HttpContext.Session.GetString("StudentID") ?? "0"),
                RoomID = Convert.ToInt32(HttpContext.Session.GetString("RoomID") ?? "0"),
                HostelID = Convert.ToInt32(HttpContext.Session.GetString("HostelID") ?? "0"),
                HostelName = HttpContext.Session.GetString("HostelName") ?? "N/A",
                RoomNumber = HttpContext.Session.GetString("RoomNumber") ?? "N/A",
                RoomRent = Convert.ToInt32(HttpContext.Session.GetString("RoomRent") ?? "0")
            };
            return View(model);
        }
        
        public async Task<IActionResult> PayNow()
        {
            try
            {
                int studentId = Convert.ToInt32(HttpContext.Session.GetString("StudentID") ?? "0");
                int roomId = Convert.ToInt32(HttpContext.Session.GetString("RoomID") ?? "0");
                int hostelId = Convert.ToInt32(HttpContext.Session.GetString("HostelID") ?? "0");

                if (studentId == 0 || roomId == 0 || hostelId == 0)
                {
                    TempData["Error"] = "Invalid session data. Please try again.";
                    return RedirectToAction("Index");
                }
                
                var apiResponse = await _httpClient.PostAsync($"{_httpClient.BaseAddress}/Payment/PayFee?roomId={roomId}&studentId={studentId}&hostelId={hostelId}", null);

                if (!apiResponse.IsSuccessStatusCode)
                {
                    TempData["Error"] = "Payment API request failed. Please try again later.";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["Message"] = "Payment Done Successfully";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Payment failed: {ex.Message}";
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> PaymentHistory()
        {
             List<PaymentModel> pm = new List<PaymentModel>();

            try
            {
                int? studentId = Globals.GetStudentID()!.Value ;
                if (!studentId.HasValue)
                {
                    TempData["ErrorMessage"] = "Student ID is missing.";
                    return View(pm);
                }

                var response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}/Payment/GetPaymentHistory/{studentId.Value}");

                if (response.IsSuccessStatusCode)
                {
                    var jsonData = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<JObject>(jsonData);

                    if (data != null && data["status"]?.ToString() == "Success" && data["data"] != null)
                    {
                        pm = JsonConvert.DeserializeObject<List<PaymentModel>>(data["data"].ToString()) ?? new List<PaymentModel>();
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "No payment history found.";
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to fetch payment history. Server returned an error.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred: " + ex.Message;
            }

            return View(pm);
        }
        public async Task<IActionResult> DownloadReceipt(int paymentId)
        {
            try
            {                
                var response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}/Payment/DownloadReceipt/{paymentId}");

                if (response.IsSuccessStatusCode)
                {
                    var fileBytes = await response.Content.ReadAsByteArrayAsync();                   
                    string filename = $"Receipt_{paymentId}.html";
                    if (response.Content.Headers.ContentDisposition != null &&
                        !string.IsNullOrEmpty(response.Content.Headers.ContentDisposition.FileName))
                    {
                        filename = response.Content.Headers.ContentDisposition.FileName.Trim('"');
                    }

                    return File(fileBytes, response.Content.Headers.ContentType.ToString(), filename);
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to download receipt. Server returned an error.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while downloading receipt: " + ex.Message;
            }
            return RedirectToAction("PaymentHistory");
        }
    }
}
