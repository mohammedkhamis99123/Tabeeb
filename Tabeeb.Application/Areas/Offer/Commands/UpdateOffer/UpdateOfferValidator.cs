using FluentValidation;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Text;
using Tabeeb.Application.Common.Services;
using Tabeeb.Resources;

namespace Tabeeb.Application.Areas.Offer.Commands.UpdateOffer
{
    public class UpdateOfferValidator : AbstractValidator<UpdateOffer>
    {
        private readonly IAppDbContext appDbContext;
        private readonly IStringLocalizer<SharedResource> localizer;

        public UpdateOfferValidator(IAppDbContext appDbContext
            , IStringLocalizer<SharedResource> localizer)
        {
            this.appDbContext = appDbContext;
            this.localizer = localizer;


            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage(localizer["Required"]);

            RuleFor(X => X.StartAt)
                .NotEmpty()
                .WithMessage(localizer["Required"]);


            RuleFor(X => X.EndAt)
              .NotEmpty()
              .WithMessage(localizer["Required"]);

            RuleFor(X => X.Percentage)
             .NotEmpty()
             .WithMessage(localizer["Required"])
             .InclusiveBetween(0, 1)
             .WithMessage(localizer["rangebetween", 0, 1]);

            RuleFor(x => x.DoctorId)
                .NotEmpty()
                .WithMessage(localizer["required"]);







        }

    }

}