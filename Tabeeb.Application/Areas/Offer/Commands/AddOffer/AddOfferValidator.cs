using FluentValidation;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Text;
using Tabeeb.Application.Common.Services;
using Tabeeb.Resources;

namespace Tabeeb.Application.Areas.Offer.Commands.AddOffer
{
    public class AddOfferValidator:AbstractValidator<AddOffer>
    {
        private readonly IAppDbContext appDbContext;
        private readonly IStringLocalizer<SharedResource> localizer;
        private readonly ICurrentUserService currentUserService;

        public AddOfferValidator(IAppDbContext appDbContext
            ,IStringLocalizer<SharedResource> localizer
            ,ICurrentUserService currentUserService)
        {
            this.appDbContext = appDbContext;
            this.localizer = localizer;
            this.currentUserService = currentUserService;
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
             .WithMessage(localizer["rangebetween",0,1]);

            RuleFor(x => x.DoctorId)
                .NotEmpty()
                .WithMessage(localizer["required"]);

            RuleFor(x => x)
                .Must((x) => ValidateCycle(x))
                .WithMessage(localizer["AlreadyDefindOfferAtThatTime"]);


            


        }

        private bool ValidateCycle(AddOffer x)
        {
            return !appDbContext
                .Offers.Any(a => a.DoctorId == x.DoctorId
                && a.ClinicId == currentUserService.ClinicId
                && a.IsActive
                &&(( x.EndAt <= a.EndAt && x.StartAt>=a.StartAt)
                ||(a.StartAt >= x.StartAt && a.EndAt<=x.EndAt ) 
                || (x.StartAt >=a.StartAt && x.EndAt>=a.EndAt)
                || (x.StartAt <= a.StartAt && x.EndAt <= a.EndAt))
               );
        }
    }
}
