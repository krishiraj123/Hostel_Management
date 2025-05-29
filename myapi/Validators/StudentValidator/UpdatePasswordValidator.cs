using FluentValidation;
using myapi.Models;

namespace myapi.Validators.StudentValidator
{
    public class UpdatePasswordValidator: AbstractValidator<StudentUpdatePasswordModel>
    {
        public UpdatePasswordValidator()
        {
            RuleFor(h => h.StudentID)
                .NotNull()
                .NotEmpty()
                .WithMessage("Student ID is required.");

            RuleFor(h => h.CurrentPassword)
                .NotNull()
                .NotEmpty()
                .WithMessage("Current password is required.");

            RuleFor(h => h.NewPassword)
                .NotNull()
                .NotEmpty()
                .WithMessage("New password is required.");

            RuleFor(h => h.ConfirmPassword)
                .NotNull()
                .NotEmpty()
                .WithMessage("Confirm password is required.")
                .Equal(h => h.NewPassword)
                .WithMessage("Confirm password must match the new password.");
        }
    }
}
