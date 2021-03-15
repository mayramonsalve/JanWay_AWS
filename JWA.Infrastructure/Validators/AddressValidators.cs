using FluentValidation;
using JWA.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace JWA.Infrastructure.Validators
{
    public class AddressInfoDtoValidator : AbstractValidator<AddressInfoDto>
    {
        public AddressInfoDtoValidator()
        {
            RuleFor(address => address.Address)
                .NotEmpty().WithMessage("{PropertyName} must not be empty.")
                .Length(5, 50).WithMessage("{PropertyName} must have at least 5 characters and at most 50 characters.");

            RuleFor(address => address.City)
                .NotEmpty().WithMessage("{PropertyName} must not be empty.")
                .Length(5, 100).WithMessage("{PropertyName} must have at least 5 characters and at most 100 characters.");

            RuleFor(address => address.StateId)
                .NotNull().WithMessage("{PropertyName} must not be null.")
                .GreaterThan(0).WithMessage("{PropertyName} must greater than 0.");
        }
    }
}
