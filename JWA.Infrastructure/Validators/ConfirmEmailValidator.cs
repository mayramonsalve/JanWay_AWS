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
                .Length(10, 100).WithMessage("{PropertyName} must have at least 10 characters ");
            
            RuleFor(x=>x.ConfirmPassword)
                .NotEmpty()
                .Length(5, 100).WithMessage("{PropertyName} must have at least 5 characters ");

            RuleFor(x => x.token)
                .NotEmpty();
        }
    }
}
