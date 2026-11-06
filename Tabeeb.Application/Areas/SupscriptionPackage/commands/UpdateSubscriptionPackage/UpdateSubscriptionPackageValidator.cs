using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Text;
using Tabeeb.Application.Common.Services;
using Tabeeb.Resources;

namespace Tabeeb.Application.Areas.SupscriptionPackage.commands.UpdateSubscriptionPackage
{
    public class UpdateSubscriptionPackageValidator : AbstractValidator<UpdateSubscriptionPackage>
    {
        private readonly IAppDbContext appDbContext;
        private readonly IStringLocalizer<SharedResource> localizer;

        public UpdateSubscriptionPackageValidator(IAppDbContext appDbContext,
            IStringLocalizer<SharedResource> localizer
            )
        {
            RuleFor(a => a.ArName)
                .NotEmpty().
                WithMessage(localizer["Required"])
                .MaximumLength(200)
                .WithMessage(localizer["maxLength", 200]);

            RuleFor(a => a.EnName)
               .NotEmpty().
               WithMessage(localizer["Required"])
               .MaximumLength(200)
               .WithMessage(localizer["maxLength", 200]);


            RuleFor(a => a.ArDescription)
            .NotEmpty().
            WithMessage(localizer["Required"])
            .MaximumLength(200)
            .WithMessage(localizer["maxLength", 500]);


            RuleFor(a => a.EnDescription)
                    .NotEmpty().
                    WithMessage(localizer["Required"])
                    .MaximumLength(200)
                    .WithMessage(localizer["maxLength", 500]);

            RuleFor(x => x.DurationInDays)
                  .NotEmpty().
                   WithMessage(localizer["Required"])
                   .GreaterThanOrEqualTo(1)
                   .WithMessage(localizer["InvalidDuration"]);

            RuleFor(x => x.Price)
                .NotEmpty()
                .WithMessage(localizer["Required"])
                .InclusiveBetween(1, 1000000)
                .WithMessage(localizer["InvalidPrice"]);


            RuleFor(x => x)
                .MustAsync((command, CancellationToken) => IsUniqueName(command, CancellationToken))
                .WithMessage(localizer["NameAlreadyExists"]);
            this.appDbContext = appDbContext;
            this.localizer = localizer;
        }

        private async Task<bool> IsUniqueName(UpdateSubscriptionPackage command, CancellationToken cancellationToken)
        {
            return !await appDbContext.SubscriptionPackages
                .AnyAsync(a =>( a.ArName == command.ArName || a.EnName == command.EnName) && a.Id!=command.Id);
        }

    }
}


    