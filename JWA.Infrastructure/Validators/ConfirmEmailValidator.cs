using FluentValidation;
using JWA.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace JWA.Infrastructure.Validators
{
    public class ConfirmEmailValidator : AbstractValidator<ConfirmEmailDto>
    {
        public ConfirmEmailValidator()
        {
            RuleFor(x=>x.Email)
                .NotEmpty()
                .EmailAddress().WithMessage("{PropertyName} not valid.")
                .Length(5, 100).WithMessage("{PropertyName} must have at least 5 characters and at most 100 characters.");
            
            RuleFor(x=>x.Password)
                .NotEmpty()
                .MinimumLength(10).WithMessage("{PropertyName} must be 10 character long")
                .Matches("[A-Z]").WithMessage("'{PropertyName}' must contain one or more capital letters.")
                .Matches("[a-z]").WithMessage("'{PropertyName}' must contain one or more lowercase letters.")
                .Matches(@"\d").WithMessage("'{PropertyName}' must contain one or more digits.")
                .Matches(@"[][""!@$%^&*(){}:;<>,.?/+_=|'~\\-]").WithMessage("'{ PropertyName}' must contain one or more special characters.")
                .Length(10, 100).WithMessage("{PropertyName} must be 10 character long,one upper case, one lower case , must have unique charaters and must also have numeric digits ");
            
            RuleFor(x=>x.ConfirmPassword)
                .NotEmpty()
                .Matches(x=>x.Password).WithMessage("confirm password and password should match")
                .Length(5, 100).WithMessage("{PropertyName} must have at least 5 characters ");

            RuleFor(x => x.Token)
                .NotEmpty();
        }
    }
}
