using FluentValidation;
using JWA.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace JWA.Infrastructure.Validators
{
    public class ChangeUserPasswordValidadtor: AbstractValidator<ChangeUserPasswordDto>
    {
        public ChangeUserPasswordValidadtor()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress().WithMessage("Invalid Email");

            RuleFor(x => x.OldPassword)
                .NotEmpty()
                .MinimumLength(10).WithMessage("Password length must be at least 10 characters long");

            RuleFor(x => x.NewPassword)
                .NotEmpty()
                .MinimumLength(10).WithMessage("Password length must be at least 10 characters long");
        }
    }
}
