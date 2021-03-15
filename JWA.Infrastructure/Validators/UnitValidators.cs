using FluentValidation;
using JWA.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace JWA.Infrastructure.Validators
{
    public class RegisterDtoValidator : AbstractValidator<RegisterUnitDto>
    {
        public RegisterDtoValidator()
        {
            RuleFor(unit => unit.MacAddress)
                .NotEmpty().WithMessage("{PropertyName} must not be empty.")
                .Length(29).WithMessage("{PropertyName} must have 29 characters.");
                //.CustomExtension(x => x.B).WithMessage("Error via custom extension");
        }
    }

    public class AssignUnitDtoValidator : AbstractValidator<AssignUnitDto>
    {
        public AssignUnitDtoValidator()
        {
            RuleFor(unit => unit.Name)
                .NotEmpty().WithMessage("{PropertyName} must not be empty.")
                .Length(5, 50).WithMessage("{PropertyName} must have at least 5 characters and at most 50 characters.");

            RuleFor(unit => unit.SuinNumber)
                .NotNull().WithMessage("{PropertyName} must not be null.")
                .GreaterThan(100).WithMessage("{PropertyName} must greater than 100.");

            RuleFor(unit => unit.FacilityId)
                .GreaterThan(0).WithMessage("{PropertyName} must greater than 0.");
        }
    }

    public class EditUnitDtoValidator : AbstractValidator<EditUnitDto>
    {
        public EditUnitDtoValidator()
        {
            RuleFor(unit => unit.Name)
                .NotEmpty().WithMessage("{PropertyName} must not be empty.")
                .Length(5, 50).WithMessage("{PropertyName} must have at least 5 characters and at most 50 characters.");

            RuleFor(unit => unit.FacilityId)
                .GreaterThan(0).WithMessage("{PropertyName} must greater than 0.");
        }
    }

    public class DetachUnitDtoValidator : AbstractValidator<DetachUnitDto>
    {
        public DetachUnitDtoValidator()
        {
            RuleFor(unit => unit.Id)
                .NotNull().WithMessage("{PropertyName} must not be null.")
                .GreaterThan(0).WithMessage("{PropertyName} must greater than 0.");
        }
    }

    public class RebootUnitDtoValidator : AbstractValidator<RebootUnitDto>
    {
        public RebootUnitDtoValidator()
        {
            RuleFor(unit => unit.Id)
                .NotNull().WithMessage("{PropertyName} must not be null.")
                .GreaterThan(0).WithMessage("{PropertyName} must greater than 0.");
        }
    }
}
