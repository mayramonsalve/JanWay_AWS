using FluentValidation;
using JWA.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace JWA.Infrastructure.Validators
{
    public class InviteValidators : AbstractValidator<InviteDto>
    {
        public InviteValidators()
        {
            RuleFor(invite => invite.Email)
                .NotEmpty().WithMessage("{PropertyName} must not be empty.")
                .EmailAddress().WithMessage("{PropertyName} not valid.")
                .Length(5,100).WithMessage("{PropertyName} must have at least 5 characters and at most 100 characters.");

            RuleFor(invite => invite.RoleId)
                .NotNull().WithMessage("Role must not be empty.");

            RuleFor(invite => invite.FacilityId)
                .NotNull().WithMessage("Role must not be empty.")
                .GreaterThan(0).WithMessage("{PropertyName} must greater than 0.");

            RuleFor(invite => invite.OrganizationId)
                .NotNull().WithMessage("Role must not be empty.")
                .GreaterThan(0).WithMessage("{PropertyName} must greater than 0.");
        }
    }
}
