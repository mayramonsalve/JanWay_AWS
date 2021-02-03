using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using JWA.Core.DTOs;

namespace JWA.Infrastructure.Validators
{
    public class SignInValidator : AbstractValidator<SignInDto>
    {
        public SignInValidator()
        {
            RuleFor(x => x.Email)
                .EmailAddress().WithMessage("Invalid Email")
                .NotEmpty();
            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(10).WithMessage("Passqord must be at least 10 charaters long");
            RuleFor(x => x.RememberMe)
                .NotEmpty();
        }
    }
}
