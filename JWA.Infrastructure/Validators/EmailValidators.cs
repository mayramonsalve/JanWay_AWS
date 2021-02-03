using FluentValidation;
using JWA.Core.DTOs;

namespace JWA.Infrastructure.Validators
{
    public class EmailValidators : AbstractValidator<EmailDto>
    {
        public EmailValidators()
        {
            RuleFor(Email => Email.Email)
                .NotEmpty().WithMessage("{PropertyName} must not be empty.")
                .EmailAddress().WithMessage("{PropertyName} not valid.")
                .Length(5, 100).WithMessage("{PropertyName} must have at least 5 characters and at most 100 characters.");
        }
    }
}
