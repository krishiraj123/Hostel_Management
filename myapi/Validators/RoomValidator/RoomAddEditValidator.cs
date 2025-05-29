using FluentValidation;
using myapi.Models;

namespace myapi.Validators.RoomValidator
{
	public class RoomAddEditValidator: AbstractValidator<RoomAddEditModel>
	{
		public RoomAddEditValidator()
		{
			RuleFor(x => x.RoomNumber)
				.NotEmpty().WithMessage("Room number is required.")
				.MaximumLength(10).WithMessage("Room number cannot exceed 10 characters.");

			RuleFor(x => x.RoomCapacity)
				.GreaterThan(0).WithMessage("Room capacity must be greater than 0.");			

			RuleFor(x => x.RoomFloor)
				.GreaterThanOrEqualTo(0).WithMessage("Room floor must be a non-negative number.");

			RuleFor(x => x.RoomRent)
				.GreaterThanOrEqualTo(0).WithMessage("Room rent must be a non-negative number.");

			RuleFor(x => x.RoomType)
				.NotEmpty().WithMessage("Room type is required.")
				.MaximumLength(20).WithMessage("Room type cannot exceed 20 characters.");

			RuleFor(x => x.RoomStatus)
				.NotEmpty().WithMessage("Room status is required.")
				.Must(x => x == "Vacant" || x == "Full").WithMessage("Room status must be either 'Vacant' or 'Full'.");

			RuleFor(x => x.HostelID)
				.GreaterThan(0).WithMessage("Hostel ID must be greater than 0.");
		}
	}
}
