using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Text;
using Tabeeb.Application.Areas.Clinic.Commands.AddClinic;
using Tabeeb.Application.Common.Services;
using Tabeeb.Resources;

namespace Tabeeb.Application.Areas.Clinic.Commands.UpdateClinic
{
    public  class UpdateClinicValidator : AbstractValidator<UpdateClinic>
    {
        private readonly IAppDbContext appDbContext;
        private readonly IStringLocalizer<SharedResource> localizer;

        public UpdateClinicValidator(IAppDbContext appDbContext, IStringLocalizer<SharedResource> localizer)
        {
            this.appDbContext = appDbContext;
            this.localizer = localizer;

            RuleFor(x => x.ArName)
                .NotEmpty()
                .WithMessage(localizer["Required"])
                .MaximumLength(200)
                .WithMessage(localizer["MaxLength", 200]);

            RuleFor(x => x.EnName)
                .NotEmpty()
                .WithMessage(localizer["Required"])
                .MaximumLength(200)
                .WithMessage(localizer["MaxLength", 200]);

            RuleFor(a => a.ArDescription)
                  .NotEmpty()
                  .WithMessage(localizer["Required"])
                  .MaximumLength(200)
                  .WithMessage(localizer["MaxLength", 200]);


            RuleFor(a => a.EnDescription)
                .NotEmpty()
                .WithMessage(localizer["Required"])
                .MaximumLength(200)
                .WithMessage(localizer["MaxLength", 200]);


            RuleFor(a => a.ProfileAr)
                .NotEmpty()
                .WithMessage(localizer["Required"])
                .MaximumLength(100)
                .WithMessage(localizer["MaxLength", 100]);


            RuleFor(a => a.ProfileEn)
                .NotEmpty()
                .WithMessage(localizer["Required"])
                .MaximumLength(100)
                .WithMessage(localizer["MaxLength", 100]);

            RuleFor(a => a.StreetArName)
                        .NotEmpty()
                        .WithMessage(localizer["Required"])
                        .MaximumLength(100)
                        .WithMessage(localizer["MaxLength", 100]);


            RuleFor(a => a.StreetEnName)
                        .NotEmpty()
                        .WithMessage(localizer["Required"])
                        .MaximumLength(100)
                        .WithMessage(localizer["MaxLength", 100]);


            RuleFor(a => a.Langitude)
                        .NotEmpty()
                        .WithMessage(localizer["Required"])
                        .MaximumLength(150)
                        .WithMessage(localizer["MaxLength", 150]);

            RuleFor(a => a.Latitude)
                        .NotEmpty()
                        .WithMessage(localizer["Required"])
                        .MaximumLength(150)
                        .WithMessage(localizer["MaxLength", 150]);


            RuleFor(a => a.CityId)
                .NotEmpty()
                .WithMessage(localizer["Required"])
                .MustAsync((cityId, CancellationToken) => IsValidCity(cityId))
                .WithMessage(localizer["InvalidCity"]);
        }

        private async Task<bool> IsValidCity(int cityId)
        {
            return await appDbContext.Cities
                               .AnyAsync(a => a.Id == cityId && a.IsActive);
        }
    }
}
