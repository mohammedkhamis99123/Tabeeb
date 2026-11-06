using FluentValidation;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Text;
using Tabeeb.Application.Common.Services;
using Tabeeb.Resources;

namespace Tabeeb.Application.Areas.Subscription.Commands.CreateSubscriptionCommand
{
    public  class CreateSubscriptionCommandValidator:AbstractValidator<CreateSubscriptionCommand>
    {
        private readonly IAppDbContext appDbContext;
        private readonly IStringLocalizer<SharedResource> localizer;

        public CreateSubscriptionCommandValidator(IAppDbContext appDbContext,IStringLocalizer<SharedResource> localizer)
        {
            this.appDbContext = appDbContext;
            this.localizer = localizer;

            RuleFor(x => x.PackageId)
                 .NotEmpty()
                 .WithMessage(localizer["Required"])
                 .Must((SubscriptionPackageId) => IsValidPackage(SubscriptionPackageId))
                 .WithMessage(localizer["InvalidPackage"]);

            RuleFor(x => x.ClinicId)
                  .NotEmpty()
                  .WithMessage(localizer["Required"])
                  .Must((ClinicId) => IsValidClinic(ClinicId))
                  .WithMessage(localizer["InvalidClinic"]);

            RuleFor(x => x.StartAt)
                .NotEmpty()
                .WithMessage(localizer["Required"])
                .Must((StartAt)=>IsValidDate(StartAt))
                .WithMessage(localizer["InvalidDate"]);
        }

        private bool IsValidDate(DateOnly startAt)
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            return startAt >= today;
        }

        private bool IsValidClinic(int clinicId)
        {
            return appDbContext.Clinics.Any(a => a.Id == clinicId);
        }

        private bool IsValidPackage(byte subscriptionPackageId)
        {
            return appDbContext.SubscriptionPackages.Any(a => a.Id == subscriptionPackageId);
        }
    }
    
}
