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
                .Matches(@"^(\+\d{1,2}\s)?\(?\d{3}\)?[\s.-]\d{3}[\s.-]\d{4}$").WithMessage("Please Enter valid mobile number. For Example 123-456-7890, (123) 456 - 7890, 123 456 7890, 123.456.7890, + 91(123) 456 - 7890")
                .NotEmpty();
        }
    }
}
