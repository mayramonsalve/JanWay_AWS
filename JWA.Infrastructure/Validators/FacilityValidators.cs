using FluentValidation;
using JWA.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace JWA.Infrastructure.Validators
{
    public class AddFacilityDtoValidator : AbstractValidator<FacilityInfoDto>
    {
        public AddFacilityDtoValidator()
        {
            RuleFor(facility => facility.Name)
                .NotEmpty().WithMessage("{PropertyName} must not be empty.")
                .Length(5, 50).WithMessage("{PropertyName} must have at least 5 characters and at most 50 characters.");

            RuleFor(facility => facility.PhoneNumber)
                .NotEmpty().WithMessage("{PropertyName} must not be empty.")
                .Must(CommonValidators.ValidPhoneNumber).WithMessage("{PropertyName} not valid.");

            RuleFor(facility => facility.OrganizationId)
                .NotNull().WithMessage("{PropertyName} must not be null.")
                .GreaterThan(0).WithMessage("{PropertyName} must greater than 0.");
        }
    }

    public class DeleteFacilityDtoValidator : AbstractValidator<DeleteFacilityDto>
    {
        public DeleteFacilityDtoValidator()
        {
            RuleFor(facility => facility.Id)
                .NotNull().WithMessage("{PropertyName} must not be null.")
                .GreaterThan(0).WithMessage("{PropertyName} must greater than 0.");
        }
    }

    public class ActivateFacilityDtoValidator : AbstractValidator<ActivateFacilityDto>
    {
        public ActivateFacilityDtoValidator()
        {
            RuleFor(facility => facility.Id)
                .NotNull().WithMessage("{PropertyName} must not be null.")
                .GreaterThan(0).WithMessage("{PropertyName} must greater than 0.");
        }
    }

    public class DeactivateFacilityDtoValidator : AbstractValidator<DeactivateFacilityDto>
    {
        public DeactivateFacilityDtoValidator()
        {
            RuleFor(facility => facility.Id)
                .NotNull().WithMessage("{PropertyName} must not be null.")
                .GreaterThan(0).WithMessage("{PropertyName} must greater than 0.");
        }
    }
}
