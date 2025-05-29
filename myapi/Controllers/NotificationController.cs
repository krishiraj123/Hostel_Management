using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using myapi.Data;
using myapi.Models;
using myapi.Validators.NotificationValidator;
using Newtonsoft.Json;

namespace myapi.Controllers
{
	[Route("apiv1/[controller]/[action]")]
	[ApiController]
	public class NotificationController : ControllerBase
	{
		private readonly NotificationRepository _notificationRepository;
		//private readonly IValidator<NotificationAddEditModel> _notificationAddEditValidator;

		public NotificationController(NotificationRepository notificationRepository
			//IValidator<NotificationAddEditModel> notificationAddEditValidator
			)
		{
			_notificationRepository = notificationRepository;
			//_notificationAddEditValidator = notificationAddEditValidator;
		}

		[HttpGet]
		public IActionResult GetAllNotifications(int id)
		{
			try
			{
				var res = _notificationRepository.GetAllNotifications(id);

				if (res == null || !res.Any())
				{
					return NotFound(new
					{
						Status = "Failure",
						Message = "No notifications found"
					});
				}

				var updateRes = res.Where(n => Convert.ToDateTime(n.SentAt).AddDays(Convert.ToDouble(n.NoOfDays)) >= DateTime.Now).ToList();
				return Ok(new
				{
					Status = "Success",
					Data = updateRes,
					Message = "Notifications retrieved successfully"
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
		public IActionResult GetNotificationById(int id)
		{
			try
			{
				var res = _notificationRepository.GetNotificationById(id);

				if (res == null)
				{
					return NotFound(new
					{
						Status = "Failure",
						Message = "Notification not found"
					});
				}

				return Ok(new
				{
					Status = "Success",
					Data = res,
					Message = "Notification retrieved successfully"
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

		[HttpPost]
		public IActionResult AddNotification([FromBody] NotificationModel nm)
		{
			try
			{
				//var validationResult = _notificationAddEditValidator.Validate(new ValidationContext<NotificationAddEditModel>(nm));

				//if (!validationResult.IsValid)
				//{
				//	return BadRequest(new
				//	{
				//		Status = "Failure",
				//		Message = "Validation failed",
				//		Errors = validationResult.Errors.Select(e => e.ErrorMessage)
				//	});
				//}

				var res = _notificationRepository.InsertNotification(nm);

				if (!res)
				{
					return BadRequest(new
					{
						Status = "Failure",
						Message = "Failed to insert notification"
					});
				}

				return Ok(new
				{
					Status = "Success",
					Message = "Notification added successfully"
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

		[HttpPut("{id}")]
		public IActionResult UpdateNotification(int id,[FromBody] NotificationModel nm)
		{
			try
			{
				//var validationResult = _notificationAddEditValidator.Validate(new ValidationContext<NotificationAddEditModel>(nm));

				//if (!validationResult.IsValid)
				//{
				//	return BadRequest(new
				//	{
				//		Status = "Failure",
				//		Message = "Validation failed",
				//		Errors = validationResult.Errors.Select(e => e.ErrorMessage)
				//	});
				//}

				var res = _notificationRepository.UpdateNotification(id,nm);

				if (!res)
				{
					return BadRequest(new
					{
						Status = "Failure",
						Message = "Failed to update notification"
					});
				}

				return Ok(new
				{
					Status = "Success",
					Message = "Notification updated successfully"
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
		public IActionResult DeleteNotification(int id)
		{
			try
			{
				var isDeleted = _notificationRepository.DeleteNotification(id);

				if (!isDeleted)
				{
					return NotFound(new
					{
						Status = "Failure",
						Message = $"Notification with ID {id} not found or already deleted."
					});
				}

				return Ok(new
				{
					Status = "Success",
					Message = "Notification deleted successfully."
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
