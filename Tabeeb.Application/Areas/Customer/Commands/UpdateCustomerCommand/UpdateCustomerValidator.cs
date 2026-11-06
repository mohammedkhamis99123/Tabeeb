using FluentValidation;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Text;
using Tabeeb.Resources;

namespace Tabeeb.Application.Areas.Customer.Commands.UpdateCustomerCommand
{
    public  class UpdateCustomerValidator:AbstractValidator<UpdateCustomerCommand>
    {
        public UpdateCustomerValidator(IStringLocalizer<SharedResource> localizer)
        {

            RuleFor(A => A.CustomerId)
                .NotEmpty()
                .WithMessage(localizer["Required"]);


            RuleFor(a=>a.MedicalFile)
                .NotEmpty()
                .WithMessage(localizer["Required"]);

        }
    }
}
