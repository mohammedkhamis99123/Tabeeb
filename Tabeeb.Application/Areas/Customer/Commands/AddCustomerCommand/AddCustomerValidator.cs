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
        private readonly IAppDbContext appDbContext;
        private readonly IStringLocalizer<SharedResource> localizer;
        private readonly ICurrentUserService currentUserService;

        public AddCustomerValidator(IAppDbContext appDbContext
            ,IStringLocalizer<SharedResource> localizer
            ,ICurrentUserService currentUserService)
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
           .MaximumLength(20)
           .WithMessage(localizer["maxLength", 20]);
        



            RuleFor(a => a.Email)
         .NotEmpty()
         .WithMessage(localizer["Required"])
         .EmailAddress()
         .WithMessage(localizer["InvalidEmail"]);




            RuleFor(a => a.IdentityNumber)
                .NotEmpty()
                .WithMessage(localizer["Required"]);


            RuleFor(x => x.MedicalFile)
                .NotEmpty()
                .WithMessage(localizer["Required"])
                .Must((medicalfile)=>IsValidMedicalFile(medicalfile))
                .WithMessage(localizer["AlreadyUsedMedicalFile"]);
            this.appDbContext = appDbContext;
            this.localizer = localizer;
            this.currentUserService = currentUserService;
        }

        private bool IsValidMedicalFile(long medicalfile)
        {
            return !appDbContext.ClinicCustomers.Any(a=>a.MedicalFile== medicalfile && a.ClinicId==currentUserService.ClinicId);
        }
    }
}
