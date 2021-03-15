using FluentValidation;
using JWA.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace JWA.Infrastructure.Validators
{
    public class SetupOrganizationDtoValidator : AbstractValidator<OrganizationInfoDto>
    {
        public SetupOrganizationDtoValidator()
        {
            RuleFor(organization => organization.Name)
                .NotEmpty().WithMessage("{PropertyName} must not be empty.")
                .Length(5, 50).WithMessage("{PropertyName} must have at least 5 characters and at most 50 characters.");

            RuleFor(organization => organization.PhoneNumber)
                .NotEmpty().WithMessage("{PropertyName} must not be empty.")
                .Must(CommonValidators.ValidPhoneNumber).WithMessage("{PropertyName} not valid.");

        }
    }

    public class DeleteOrganizationDtoValidator : AbstractValidator<DeleteOrganizationDto>
    {
        public DeleteOrganizationDtoValidator()
        {
            RuleFor(organization => organization.Id)
                .NotNull().WithMessage("{PropertyName} must not be null.")
                .GreaterThan(0).WithMessage("{PropertyName} must greater than 0.");
        }
    }

    public class ActivateOrganizationDtoValidator : AbstractValidator<ActivateOrganizationDto>
    {
        public ActivateOrganizationDtoValidator()
        {
            RuleFor(organization => organization.Id)
                .NotNull().WithMessage("{PropertyName} must not be null.")
                .GreaterThan(0).WithMessage("{PropertyName} must greater than 0.");
        }
    }

    public class DeactivateOrganizationDtoValidator : AbstractValidator<DeactivateOrganizationDto>
    {
        public DeactivateOrganizationDtoValidator()
        {
            RuleFor(organization => organization.Id)
                .NotNull().WithMessage("{PropertyName} must not be null.")
                .GreaterThan(0).WithMessage("{PropertyName} must greater than 0.");
        }
    }
}
