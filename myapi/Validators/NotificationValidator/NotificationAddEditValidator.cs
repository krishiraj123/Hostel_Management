using FluentValidation;
using myapi.Models;

namespace myapi.Validators.NotificationValidator
{
	public class NotificationAddEditValidator: AbstractValidator<NotificationAddEditModel>
	{
		public NotificationAddEditValidator() 
		{
			RuleFor(n => n.Title).NotEmpty()
				.NotNull().WithMessage("Title is required")
				.WithMessage("Title is required");
			RuleFor(n => n.Message)
				.NotNull().WithMessage("Message is required")
				.NotEmpty().WithMessage("Message is required");
			RuleFor(n => n.NoOfDays)
				.NotNull().WithMessage("Number of days after which the notification should deleted are not defined")
				.NotEmpty().WithMessage("Number of days after which the notification should deleted are not defined");
			RuleFor(n => n.HostelID)
				.NotNull().WithMessage("HostelID is required")
				.NotEmpty().WithMessage("HostelID is required");
		}
	}
}
