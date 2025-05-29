using System.Text;
using Microsoft.AspNetCore.Mvc;
using myapp.Areas.Staff.Models;
using myapp.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace myapp.Areas.Staff.Controllers
{
    [Area("Staff")]
    [AreaAuthorization("Staff")]
    public class StudentController : Controller
    {
        private readonly HttpClient _client;
        private Uri baseAddress = Globals.baseAddress;

        public StudentController()
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
        }

        [HttpGet]
        public async Task<IActionResult> StudentList()
        {
            try
            {
                var studentList = new List<StudentModel>();
                
                var response = await _client.GetAsync($"{_client.BaseAddress}/Student/SelectAllStudent?id={Globals.GetHostelID().Value}");

                if (response.IsSuccessStatusCode)
                {
                    var jsonData = await response.Content.ReadAsStringAsync();
                    
                    var responseObject = JsonConvert.DeserializeObject<JObject>(jsonData);
                    
                    if (responseObject["status"]?.ToString() == "Success")
                    {                       
                        var dataArray = responseObject["data"];
                        if (dataArray != null)
                        {
                            studentList = JsonConvert.DeserializeObject<List<StudentModel>>(dataArray.ToString());
                        }
                    }
                    //else
                    //{
                    //    TempData["ErrorMessage"] = responseObject["message"]?.ToString() ?? "Failed to fetch student data.";
                    //}
                }
                else
                {
                    TempData["ErrorMessage"] = "No student data found!";
                    Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                }

                return View(studentList);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred while fetching the student list: {ex.Message}";
                Console.WriteLine($"Exception: {ex.Message}");

                return View(new List<StudentModel>());
            }
        }

        public async Task<IActionResult> StudentAddEdit(int? id)
        {
            StudentModel sm = new StudentModel();

            try
            {
                if (id.HasValue && id != 0)
                {
                    var response = await _client.GetAsync($"{_client.BaseAddress}/Student/GetStudentID/{id}");

                    if (response.IsSuccessStatusCode)
                    {
                        var jsonData = await response.Content.ReadAsStringAsync();
                        var apiResponse = JsonConvert.DeserializeObject<JObject>(jsonData);

                        if (apiResponse["status"].ToString() == "Success")
                        {
                            sm = JsonConvert.DeserializeObject<StudentModel>(JsonConvert.SerializeObject(apiResponse["data"]));
                        }
                        else
                        {
                            TempData["ErrorMessage"] = "Failed to retrieve student details.";
                        }
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Error occurred while fetching student details.";
                    }
                }
                else
                {                    
                    sm = new StudentModel();
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred: {ex.Message}";
                Console.WriteLine($"Exception: {ex.Message}");
            }

            await AvailableRoomList(sm.StudentID,sm.RoomID);
            return View(sm);
        }



        [HttpPost]
        public async Task<IActionResult> StudentSave(StudentModel sm)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData["ErrorMessage"] = "Please correct the errors.";
                    await AvailableRoomList(sm.StudentID, sm.RoomID);
                    return View("StudentAddEdit", sm);
                }

                if (sm.ProfileImageFile != null && sm.ProfileImageFile.Length > 0)
                {                   
                    var uploadsDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                    if (!Directory.Exists(uploadsDir))
                    {
                        Directory.CreateDirectory(uploadsDir);
                    }
                   
                    var fileName = Guid.NewGuid() + Path.GetExtension(sm.ProfileImageFile.FileName);
                    var filePath = Path.Combine(uploadsDir, fileName);
                  
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await sm.ProfileImageFile.CopyToAsync(stream);
                    }

                    sm.ProfileImage = $"/uploads/{fileName}";                    
                }               

                string jsonData = JsonConvert.SerializeObject(sm);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                HttpResponseMessage res;
                bool hasStudentID = sm.StudentID.HasValue && sm.StudentID > 0;

                if (!hasStudentID)
                {
                    await AvailableRoomList(sm.StudentID, sm.RoomID);
                    res = await _client.PostAsync($"{_client.BaseAddress}/Student/InsertStudent", content);
                    Console.WriteLine("Received Student Data: " + JsonConvert.SerializeObject(sm));
                }
                else
                {
                    await AvailableRoomList(sm.StudentID, sm.RoomID);
                    res = await _client.PutAsync($"{_client.BaseAddress}/Student/UpdateStudent/{sm.StudentID}", content);
                    Console.WriteLine("Received Student Data: " + JsonConvert.SerializeObject(sm));
                }

                if (res.IsSuccessStatusCode)
                {
                    TempData["Message"] = $"Student {(hasStudentID ? "Updated" : "Inserted")} Successfully";
                }
                else
                {
                    TempData["ErrorMessage"] = $"Failed To {(hasStudentID ? "Update" : "Insert")} Student";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred: {ex.Message}";
                Console.WriteLine($"Exception: {ex.Message}");
            }
            return RedirectToAction("StudentList");
        }

        public async Task<IActionResult> DeleteStudent(int id)
        {
            try
            {               
                var response = await _client.DeleteAsync($"{_client.BaseAddress}/Student/DeleteStudent/{id}");

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
            catch (HttpRequestException httpEx)
            {
                TempData["ErrorMessage"] = $"HTTP Request Error: {httpEx.Message}";
                Console.WriteLine($"HttpRequestException: {httpEx}");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An unexpected error occurred: {ex.Message}";
                Console.WriteLine($"Exception: {ex}");
            }
            
            return RedirectToAction("StudentList", "Student", new { area = "Staff" });
        }

        public async Task AvailableRoomList(int? StudentID,int? RoomID)
        {
            try
            {
                ViewBag.IsRoomSelectionDisabled = StudentID.HasValue && StudentID != 0;

                var HostelID = Convert.ToInt32(HttpContext.Session.GetString("HostelID"));
                HttpResponseMessage res;

                if((!StudentID.HasValue || StudentID == 0 || StudentID == null) && (RoomID == 0 || RoomID == null))
                {
					res = await _client.GetAsync($"{_client.BaseAddress}/Room/AvailableRoomList/{HostelID}");
				}
                else
                {                    
                    res = await _client.GetAsync($"{_client.BaseAddress}/Room/AllAvailableRoomList/{HostelID}/{RoomID}");
				}				

                if(res.IsSuccessStatusCode)
                {
                    var data = await res.Content.ReadAsStringAsync();                    
                    var jsonData = JsonConvert.DeserializeObject<JObject>(data);

					if (jsonData["status"].ToString() != null && jsonData["status"].ToString() == "Success")
					{
						var roomList = JsonConvert.DeserializeObject<List<AvailableRoomModel>>(jsonData["data"].ToString());
						ViewBag.AvailableRoomList = roomList;
                        //return;
					}
					else
					{
						TempData["ErrorMessage"] = "Failed to retrieve student details.";
					}
				}
                else
                {
					TempData["ErrorMessage"] = "Failed to retrieve student details.";
				}
            }
			catch (Exception ex)
			{
				TempData["ErrorMessage"] = $"An error occurred: {ex.Message}";
				Console.WriteLine($"Exception: {ex.Message}");
			}
		}
    }
}
