using System.Text;
using Microsoft.AspNetCore.Mvc;
using myapp.Areas.Staff.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace myapp.Areas.Staff.Controllers
{
    [Area("Staff")]
    [AreaAuthorization("Staff")]
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
            List<ComplaintsModel> cm = new List<ComplaintsModel>();

            try
            {
                var res = await _httpClient.GetAsync($"{_httpClient.BaseAddress}/Complaints/GetAllComplaints?id={Globals.GetHostelID().Value}");

                if (res.IsSuccessStatusCode)
                {
                    var data = await res.Content.ReadAsStringAsync();
                    var jsonData = JsonConvert.DeserializeObject<JObject>(data);

                    if (jsonData["status"].ToString() == "Success")
                    {
                        cm = JsonConvert.DeserializeObject<List<ComplaintsModel>>(jsonData["data"].ToString());
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
            ComplaintsModel cm = new ComplaintsModel();
            try
            {
                var res = await _httpClient.GetAsync($"{_httpClient.BaseAddress}/Complaints/GetByID/{id}");

                if (res.IsSuccessStatusCode)
                {
                    var data = await res.Content.ReadAsStringAsync();
                    var jsonData = JsonConvert.DeserializeObject<JObject>(data);

                    if (jsonData["status"].ToString() == "Success")
                    {
                        cm = JsonConvert.DeserializeObject<ComplaintsModel>(jsonData["data"].ToString());
                    }
                }

            }
            catch(Exception ex)
            {
                TempData["ErrorMessage"] = ex;
            }

            return View(cm);
        }

		public async Task<IActionResult> UpdateComplaintsStatus(int id, string status)
		{
			try
			{				
				var allowedStatuses = new[] { "Accepted", "Rejected", "Completed", "Pending" };
				if (!allowedStatuses.Contains(status))
				{
					TempData["ErrorMessage"] = "Invalid status value.";
					return BadRequest(new { message = "Invalid status value." });
				}

				ComplainUpdateStatusModel cm = new ComplainUpdateStatusModel
				{
					ComplainID = id,
					ComplainStatus = status
				};

				var jsonData = JsonConvert.SerializeObject(cm);
				var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
				
				var response = await _httpClient.PutAsync($"{_httpClient.BaseAddress}/Complaints/UpdateComplaintsStatus/{id}", content);

				if (!response.IsSuccessStatusCode)
				{
					TempData["ErrorMessage"] = "Failed to update the complaint status.";
					return BadRequest(new { message = "Failed to update the complaint status." });
				}

				return Ok(new { message = "Complaint status updated successfully." });
			}
			catch (Exception ex)
			{
				TempData["ErrorMessage"] = $"An error occurred: {ex.Message}";
				return StatusCode(500, new { message = "Internal server error." });
			}
		}

	}
}
