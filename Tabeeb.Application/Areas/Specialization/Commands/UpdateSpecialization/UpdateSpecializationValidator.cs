using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Text;
using Tabeeb.Application.Common.Services;
using Tabeeb.Resources;

namespace Tabeeb.Application.Areas.Specialization.Commands.UpdateSpecialization
{
    public  class UpdateSpecializationValidator : AbstractValidator<UpdateSpecialization>
    {
        private readonly IAppDbContext appDbContext;
        private readonly IStringLocalizer<SharedResource> localizer;

        public UpdateSpecializationValidator(IAppDbContext appDbContext,IStringLocalizer<SharedResource> localizer)
        {
            this.appDbContext = appDbContext;
            this.localizer = localizer;

            RuleFor(A => A.ArName)
             .NotEmpty()
             .WithMessage(localizer["Required"])
             .MaximumLength(150)
             .WithMessage(localizer["MaxLength", 150]);

            RuleFor(A => A.EnName)
            .NotEmpty()
            .WithMessage(localizer["Required"])
            .MaximumLength(150)
            .WithMessage(localizer["MaxLength", 150]);

            RuleFor(x => x)
                .MustAsync((command, CancellationToken) => IsUniqueNames(command))
                .WithMessage(localizer["AlreadyExistName"]);
        }

        private async Task<bool> IsUniqueNames(UpdateSpecialization command)
        {
            return !await appDbContext.Specializations
                                         .AnyAsync(a => (a.ArName == command.ArName || a.EnName == command.EnName) 
                                         && a.Id!=command.Id);
        }
    }
  
}
