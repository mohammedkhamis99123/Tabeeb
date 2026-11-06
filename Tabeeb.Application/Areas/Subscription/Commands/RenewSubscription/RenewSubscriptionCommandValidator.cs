using FluentValidation;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Text;
using Tabeeb.Application.Common.Services;
using Tabeeb.Domain.Entities;
using Tabeeb.Resources;

namespace Tabeeb.Application.Areas.Subscription.Commands.RenewSubscription
{
    public  class RenewSubscriptionCommandValidator:AbstractValidator<RenewSubscriptionCommand>
    {
        private readonly IAppDbContext appDbContext;
        private readonly IStringLocalizer<SharedResource> localizer;

        public RenewSubscriptionCommandValidator(IAppDbContext appDbContext,IStringLocalizer<SharedResource> localizer)
        {
            this.appDbContext = appDbContext;
            this.localizer = localizer;

            RuleFor(x => x.SubscriptionPackageId)
                   .NotEmpty()
                   .WithMessage(localizer["Required"])
                   .Must((SubscriptionPackageId) => IsValidPackage(SubscriptionPackageId))
                   .WithMessage(localizer["InvalidPackage"]);

            RuleFor(x => x.ClinicId)
                  .NotEmpty()
                  .WithMessage(localizer["Required"])
                  .Must((ClinicId) => IsValidClinic(ClinicId))
                  .WithMessage(localizer["InvalidClinic"]);
        }

        private bool IsValidClinic(int clinicId)
        {
            return appDbContext.Clinics.Any(a => a.Id == clinicId);
        }

        private bool IsValidPackage(byte subscriptionPackageId)
        {
           return  appDbContext.SubscriptionPackages.Any(a=>a.Id== subscriptionPackageId);
        }
    }
}
