using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Text;
using Tabeeb.Application.Common.Services;
using Tabeeb.Resources;

namespace Tabeeb.Application.Areas.Country.Commands.AddCountry
{
    public  class AddCountryCommandValidator:AbstractValidator<AddCountryCommand>
    {
        private readonly IAppDbContext appDbContext;
        private readonly IStringLocalizer<SharedResource> localizer;

        public AddCountryCommandValidator(IAppDbContext appDbContext,IStringLocalizer<SharedResource> localizer)
        {

            RuleFor(x => x)
                .MustAsync((command, CancellationToken) => IsUniqueNames(command))
                .WithMessage(localizer["AlreadyExists"]);

            RuleFor(x => x.ArName)
              .NotNull()
              .WithMessage(localizer["Required"])
              .MaximumLength(200)
              .WithMessage(localizer["MaxLength", 200]);
         

            RuleFor(x => x.EnName)
             .NotNull()
             .WithMessage(localizer["Required"])
             .MaximumLength(200)
             .WithMessage(localizer["MaxLength", 200]);


            RuleFor(x => x.ISOCode)
            .NotNull()
            .WithMessage(localizer["Required"])
            .MaximumLength(5)
            .WithMessage(localizer["MaxLength", 5]);
            this.appDbContext = appDbContext;
            this.localizer = localizer;
        }

        private async Task<bool> IsUniqueNames(AddCountryCommand command)
        {
            return !await appDbContext.Countries
                .AnyAsync(a => a.ArName == command.ArName || a.EnName == command.EnName);
        }
    }
}
