using FluentValidation;
using JWA.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace JWA.Infrastructure.Validators
{
    public class InviteValidator : AbstractValidator<InviteDtos>
    {
        public InviteValidator()
        {
            RuleFor(invite => invite.Email)
                .NotEmpty().WithMessage("{PropertyName} must not be empty.")
                .EmailAddress().WithMessage("{PropertyName} not valid.")
                .Length(5,100).WithMessage("{PropertyName} must have at least 5 characters and at most 100 characters.");

            RuleFor(invite => invite.RoleId)
                .NotNull().WithMessage("Role must not be empty.");
        }
    }
}
