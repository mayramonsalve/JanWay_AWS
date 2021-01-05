using FluentValidation;
using JWA.Core.DTOs;

namespace JWA.Infrastructure.Validators
{
    public class UserValidator : AbstractValidator<UserDto>//, AbstractValidator<ConfirmAccountRequest>
    {
        public UserValidator()
        {
            RuleFor(user => user.Name)
                .NotNull().WithMessage("{PropertyName} must not be null.")
                .Length(5,50).WithMessage("{PropertyName} must have at least 5 characters and at most 50 characters.");
        }
    }
}
