using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Text;
using Tabeeb.Application.Common.Services;
using Tabeeb.Domain.Entities;
using Tabeeb.Resources;

namespace Tabeeb.Application.Areas.City.Commands.CreateCity
{
    public  class CreateCityValidator:AbstractValidator<CreateCityCommand>
    {
        private readonly IStringLocalizer<SharedResource> localizer;
        private readonly IAppDbContext appDb;

        public CreateCityValidator(IStringLocalizer<SharedResource> localizer,IAppDbContext appDb)
        {
            this.localizer = localizer;
            this.appDb = appDb;
            RuleFor(a => a.ArName)
            .NotEmpty()
            .WithMessage(localizer["Required"])
            .MaximumLength(200)
            .WithMessage(localizer["MaxLength", 200]);

            RuleFor(a => a.EnName)
           .NotEmpty()
           .WithMessage(localizer["Required"])
           .MaximumLength(200)
           .WithMessage(localizer["MaxLength", 200]);

            RuleFor(a => a.CountryId)
          .NotEmpty()
          .WithMessage(localizer["Required"])
          .MustAsync((countryId, CancellationToken) => IsValidCountry(countryId))
          .WithMessage(localizer["InvalidCountry"]);

            RuleFor(x => x)
                    .MustAsync((command, CancellationToken) => IsUniqueName(command))
                    .WithMessage(localizer["AlreadyExists"]);
          
        }

        private async Task<bool> IsUniqueName(CreateCityCommand command)
        {
            return !await appDb.Cities
                .AnyAsync(c => c.ArName==command.ArName || c.EnName==command.EnName);

        }

        private async Task<bool> IsValidCountry(byte countryId)
        {
            return  await appDb.Countries.AnyAsync(c => c.Id == countryId);
        }


    }
}
