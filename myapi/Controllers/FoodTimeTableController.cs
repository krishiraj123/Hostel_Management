using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using myapi.Data;
using myapi.Models;

namespace myapi.Controllers
{
    [Route("apiv1/[controller]/[action]")]
    [ApiController]
    public class FoodTimeTableController : ControllerBase
    {
        private readonly FoodTimeTableRepository _repository;

        public FoodTimeTableController(FoodTimeTableRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("{hostelId}")]
        public IActionResult GetTimetable(int hostelId)
        {
            try
            {
                var timetable = _repository.GetTimetable(hostelId);

                if (timetable == null || timetable.Count == 0)
                {
                    return NotFound(new
                    {
                        Status = "Failure",
                        Message = "No timetable found for this hostel"
                    });
                }

                return Ok(new
                {
                    Status = "Success",
                    Data = timetable,
                    Message = "Timetable retrieved successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Status = "Failure",
                    Message = "Internal server error",
                    Error = ex.Message
                });
            }
        }

        [HttpPost("{hostelId}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadTimetable(IFormFile file, int hostelId)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return BadRequest(new
                    {
                        Status = "Failure",
                        Message = "No file uploaded"
                    });
                }

                var result = await _repository.UploadTimetable(file, hostelId);

                if (!result)
                {
                    return BadRequest(new
                    {
                        Status = "Failure",
                        Message = "Failed to process the timetable file"
                    });
                }

                return Ok(new
                {
                    Status = "Success",
                    Message = "Timetable uploaded successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Status = "Failure",
                    Message = "Internal server error",
                    Error = ex.Message
                });
            }
        }
    }
}