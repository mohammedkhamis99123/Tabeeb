using FluentValidation;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Text;
using Tabeeb.Application.Areas.Employee.Commands.AddEmployee;
using Tabeeb.Application.Common.Services;
using Tabeeb.Resources;

namespace Tabeeb.Application.Areas.Customer.Commands.AddCustomerCommand
{
    public  class AddCustomerValidator:AbstractValidator<AddCustomerCommand>
    {
        public AddCustomerValidator(IAppDbContext appDbContext,IStringLocalizer<SharedResource> localizer)
        {
            RuleFor(a => a.ArName)
                .NotEmpty()
                .WithMessage(localizer["Required"])
                .MaximumLength(150)
                .WithMessage(localizer["maxLength", 150])
                 ;

            RuleFor(a => a.EnName)
                .NotEmpty()
                .WithMessage(localizer["Required"])
                .MaximumLength(150)
                .WithMessage(localizer["maxLength", 150])
                 ;

            RuleFor(a => a.Address)
            .NotEmpty()
            .WithMessage(localizer["Required"])
            .MaximumLength(150)
            .WithMessage(localizer["maxLength", 250])
             ;

            RuleFor(a => a.PhoneNumber)
           .NotEmpty()
           .WithMessage(localizer["Required"])
           .MaximumLength(150)
           .WithMessage(localizer["maxLength", 250])
           .Matches(@"^(?:\+966|00966|0)?5\d{8}$")
            .WithMessage(localizer["InvalidSaudiPhoneNumber"]);



            RuleFor(a => a.Email)
         .NotEmpty()
         .WithMessage(localizer["Required"])
         .EmailAddress()
         .WithMessage(localizer["InvalidEmail"]);


            string saudiIdentityPattern = @"^[12]\d{9}$";

            RuleFor(a => a.IdentityNumber)
                .NotEmpty()
                .WithMessage(localizer["Required"])
                .Matches(saudiIdentityPattern)
                .WithMessage(localizer["InvalidIdentityNumber"]); // رسالة: رقم الهوية أو الإقامة غير صحيح


        }
    }
}
