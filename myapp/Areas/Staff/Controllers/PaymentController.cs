using System.Text;
using Microsoft.AspNetCore.Mvc;
using myapp.Areas.Staff.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace myapp.Areas.Staff.Controllers
{
    [Area("Staff")]
    [AreaAuthorization("Staff")]
    public class PaymentController : Controller
    {
        private readonly HttpClient _httpClient;

        public PaymentController()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = Globals.baseAddress
            };
        }

        [HttpGet]
        public async Task<IActionResult> PaymentList()
        {
            List<PaymentModel> paymentList = new List<PaymentModel>();
            try
            {
                var response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}/Payment/GetPaymentDetails?hostelId={Globals.GetHostelID().Value}");

                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var jsonData = JsonConvert.DeserializeObject<JObject>(data);

                    if (jsonData["status"]?.ToString().ToLower() == "success")
                    {
                        paymentList = JsonConvert.DeserializeObject<List<PaymentModel>>(jsonData["data"]?.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred: {ex.Message}";
                Console.WriteLine($"Exception: {ex}");
            }

            return View(paymentList);
        }        
    }
}
