using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using JWA.Core.DTOs;

namespace JWA.Infrastructure.Validators
{
    public class RecoverPasswordValidator:AbstractValidator<RecoverPasswordDto>
    {
        public RecoverPasswordValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress().WithMessage("Invalid Email");

            RuleFor(x => x.NewPassword)
                .NotEmpty()
                .MinimumLength(10).WithMessage("Password length must be at least 10 characters long");

            RuleFor(x => x.Token)
                .NotEmpty();
        }
    }
}
