using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using JWA.Core.DTOs;

namespace JWA.Infrastructure.Validators
{
    public class EditProfileValidator:AbstractValidator<EditProfileDto>
    {
        public EditProfileValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress().WithMessage("Invalid Email");

            RuleFor(x => x.Phone)
                .NotEmpty();
        }
    }
}
