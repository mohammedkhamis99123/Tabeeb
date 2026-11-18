using FluentValidation;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Text;
using Tabeeb.Application.Common.Services;
using Tabeeb.Resources;

namespace Tabeeb.Application.Areas.Customer.Commands.UpdateCustomerCommand
{
    public  class UpdateCustomerValidator:AbstractValidator<UpdateCustomerCommand>
    {
        private readonly IAppDbContext appDbContext;
        private readonly IStringLocalizer<SharedResource> localizer;
        private readonly ICurrentUserService currentUserService;
        public UpdateCustomerValidator(IAppDbContext appDbContext
       , IStringLocalizer<SharedResource> localizer
       , ICurrentUserService currentUserService)
        {

            RuleFor(A => A.CustomerId)
                .NotEmpty()
                .WithMessage(localizer["Required"]);


            RuleFor(x => x.MedicalFile)
           .NotEmpty()
           .WithMessage(localizer["Required"]);

            RuleFor(x=>x)
                    .Must(x => IsValidMedicalFile(x))
           .WithMessage(localizer["AlreadyUsedMedicalFile"]);

            this.appDbContext = appDbContext;
            this.localizer = localizer;
            this.currentUserService = currentUserService;
        }

        private bool IsValidMedicalFile(UpdateCustomerCommand request)
        {
      
            return !appDbContext.ClinicCustomers.Any(a =>
            a.MedicalFile == request.MedicalFile
            && a.ClinicId == currentUserService.ClinicId
            && a.CustomerId !=request.CustomerId);
        
    }
    }
}
