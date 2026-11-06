using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Text;
using Tabeeb.Application.Common.Services;
using Tabeeb.Resources;

namespace Tabeeb.Application.Areas.Specialization.Commands.AddSpecialization
{
    public  class AddSpecializationValidator:AbstractValidator<AddSpecialization>
    {
        private readonly IAppDbContext appDbContext;
        private readonly IStringLocalizer<SharedResource> stringLocalizer;

        public AddSpecializationValidator(IAppDbContext appDbContext,IStringLocalizer<SharedResource> stringLocalizer)
        {
            RuleFor(A => A.ArName)
             .NotEmpty()
             .WithMessage(stringLocalizer["Required"])
             .MaximumLength(150)
             .WithMessage(stringLocalizer["MaxLength", 150]);

            RuleFor(A => A.EnName)
            .NotEmpty()
            .WithMessage(stringLocalizer["Required"])
            .MaximumLength(150)
            .WithMessage(stringLocalizer["MaxLength", 150]);

            RuleFor(x => x)
                .MustAsync((command, CancellationToken) => IsUniqueNames(command))
                .WithMessage(stringLocalizer["AlreadyExistName"]);
            this.appDbContext = appDbContext;
            this.stringLocalizer = stringLocalizer;
        }

        private async Task<bool> IsUniqueNames(AddSpecialization command)
        {
            return !await appDbContext.Specializations
                                         .AnyAsync(a => a.ArName == command.ArName || a.EnName == command.EnName);
        }
    }
}
