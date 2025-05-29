using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using myapi.Data;
using myapi.Models;

namespace myapi.Controllers
{
    [Route("apiv1/[controller]/[action]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly IValidator<RoomAddEditModel> _validator;
        private readonly RoomRepository _roomRepository;
        public RoomController(IValidator<RoomAddEditModel> validator, RoomRepository roomRepository) 
        { 
            _validator = validator;
            _roomRepository = roomRepository;
        }

        [HttpGet]
        public IActionResult GetAllRooms(int id)
        {
			try
			{
				var res = _roomRepository.GetAllRooms(id);

				if (res == null || !res.Any())
				{
					return NotFound(new
					{
						Status = "Failure",
						Message = "No Rooms found"
					});
				}
				return Ok(new
				{
					Status = "Success",
					Data = res,
					Message = "Rooms found"
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

        [HttpGet]
        public IActionResult GetRoomMates(int roomId, int hostelId)
        {
            try
            {
                var res = _roomRepository.GetRoomMates(roomId, hostelId);

                if (res == null || !res.Any())
                {
                    return NotFound(new
                    {
                        Status = "Failure",
                        Message = "No roommates found in this room"
                    });
                }
                return Ok(new
                {
                    Status = "Success",
                    Data = res,
                    Message = $"Found {res.Count()} roommates"
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

        [HttpGet("{id}")]		
		public IActionResult GetRoomsByID(int id)
		{
			try
			{
				var res = _roomRepository.GetRoomByID(id);

				if (res == null)
				{
					return NotFound(new
					{
						Status = "Failure",
						Message = "No Room found"
					});
				}
				return Ok(new
				{
					Status = "Success",
					Data = res,
					Message = "Room found"
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

		[HttpDelete("{id}")]
		public IActionResult DeleteRoom(int id)
		{
			try
			{
				var res = _roomRepository.DeleteRoom(id);

				if(res)
				{
					return Ok(new
					{
						Status = "Success",
						Message = "Room Deleted Successfully"
					});
				}
				return BadRequest(new
				{
					Status = "Failure",
					Message = "Failed to Delete Room"
				});
			}
			catch(Exception ex)
			{
				return StatusCode(500, new
				{
					Status = "Failure",
					Message = "Internal server error",
					Error = ex.Message
				});
			}
		}

		[HttpPost]
		public IActionResult InsertRoom([FromBody] RoomAddEditModel rm)
		{
			try
			{
				var validateRes = _validator.Validate(rm);

				if(!validateRes.IsValid)
				{
					return BadRequest(new
					{
						Status = "Failure",
						Errors = validateRes.Errors.Select(e => e.ErrorMessage)
					});
				}

				var res = _roomRepository.InsertRoom(rm);

				if (res)
				{
					return Ok(new
					{
						Status = "Success",
						Message = "Room Inserted Successfully"
					});
				}
				return BadRequest(new
				{
					Status = "Failure",
					Message = "Failed to Insert Room"
				});
			}
			catch(Exception ex)
			{
				return StatusCode(500, new
				{
					Status = "Failure",
					Message = "Internal server error",
					Error = ex.Message
				});
			}
		}

		[HttpPut("{id}")]
		public IActionResult UpdateRoom(int id,[FromBody] RoomAddEditModel rm)
		{
			try
			{
				var validateRes = _validator.Validate(rm);

				if (!validateRes.IsValid)
				{
					return BadRequest(new
					{
						Status = "Failure",
						Errors = validateRes.Errors.Select(e => e.ErrorMessage)
					});
				}

				var res = _roomRepository.UpdateRoom(id,rm);

				if (res)
				{
					return Ok(new
					{
						Status = "Success",
						Message = "Room Updated Successfully"
					});
				}
				return BadRequest(new
				{
					Status = "Failure",
					Message = "Failed to Update Room"
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

		[HttpGet("{id}")]
		public IActionResult AvailableRoomList(int id)
		{
			try
			{
				var res = _roomRepository.AvailableRoomList(id);

				if (res.IsNullOrEmpty())
				{
					return NotFound(new
					{
						Status = "Failure",
						Message = "No Rooms found"
					});
				}
				return Ok(new
				{
					Status = "Success",
					Data = res,
					Message = "Rooms found"
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

		[HttpGet("{HostelID}/{RoomID}")]
		public IActionResult AllAvailableRoomList(int HostelID,int RoomID)
		{
			try
			{
				var res = _roomRepository.AllAvailableRoomList(HostelID,RoomID);

				if (res.IsNullOrEmpty())
				{
					return NotFound(new
					{
						Status = "Failure",
						Message = "No Rooms found"
					});
				}
				return Ok(new
				{
					Status = "Success",
					Data = res,
					Message = "Rooms found"
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
