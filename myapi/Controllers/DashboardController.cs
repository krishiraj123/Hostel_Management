using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using myapi.Data;

namespace myapi.Controllers
{
    [Route("apiv1/[controller]/[action]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly DashboardRepository _dashboardRepository;

        public DashboardController(DashboardRepository dashboardRepository) {
            _dashboardRepository = dashboardRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetDashboardData()
        {
            try
            {
                var res = await _dashboardRepository.GetDashboardData();

                if (res == null)
                {
                    return NotFound(new
                    {
                        Status = "Failure",
                        Message = "No data found"
                    });
                }
                return Ok(new
                {
                    Status = "Success",
                    Data = res,
                    Message = "Data found"
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
