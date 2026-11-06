using FluentValidation;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Text;
using Tabeeb.Application.Common.Services;
using Tabeeb.Resources;

namespace Tabeeb.Application.Areas.Employee.Commands.UpdateEmployee
{
    public class UpdateEmployeeValidator : AbstractValidator<UpdateEmployeeCommand>
    {
        private readonly IAppDbContext appDbContext;
        private readonly IStringLocalizer<SharedResource> localizer;

        public UpdateEmployeeValidator(IAppDbContext appDbContext, IStringLocalizer<SharedResource> localizer)
        {
            this.appDbContext = appDbContext;
            this.localizer = localizer;

            // 1. الاسم بالعربية
            RuleFor(d => d.ArName)
                 .NotEmpty().WithMessage(localizer["Required"])
                 .MaximumLength(150).WithMessage(localizer["MaxLength", 150]);

            RuleFor(d => d.EnName)
                        .NotEmpty().WithMessage(localizer["Required"])
                        .MaximumLength(150).WithMessage(localizer["MaxLength", 150]);

            RuleFor(d => d.Addrees)
               .NotEmpty().WithMessage(localizer["Required"])
               .MaximumLength(250).WithMessage(localizer["MaxLength", 250]);

            // 3. البريد الإلكتروني (مطلوب ولابد أن يكون صيغة بريد صحيحة)
            RuleFor(d => d.Email)
                 .NotEmpty()
                 .WithMessage(localizer["EmailRequired"])
                 .MaximumLength(250)
                 .WithMessage(localizer["MaxLength", 250]);


            RuleFor(a => a.IdentityNumber)
               .NotEmpty()
               .WithMessage(localizer["Required"]);





        }

    }
}