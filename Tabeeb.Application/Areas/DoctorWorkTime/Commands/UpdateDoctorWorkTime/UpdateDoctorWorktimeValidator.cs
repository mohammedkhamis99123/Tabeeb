using FluentValidation;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Text;
using Tabeeb.Application.Common.Services;
using Tabeeb.Resources;

namespace Tabeeb.Application.Areas.DoctorWorkTime.Commands.UpdateDoctorWorkTime
{
    public  class UpdateDoctorWorktimeValidator:AbstractValidator<UpdateDoctorWorkTime>
    {
        private readonly IAppDbContext appDbContext;
        private readonly IStringLocalizer<SharedResource> localizer;

        public UpdateDoctorWorktimeValidator(IAppDbContext appDbContext,IStringLocalizer<SharedResource> localizer)
        {
            this.appDbContext = appDbContext;
            this.localizer = localizer;

            // 6. التحقق من مواعيد العمل الخاصة بالطبيب (Child Collection)
            RuleForEach(d => d.WorkTimes).ChildRules(workTime =>
             {
                 workTime.RuleFor(w => w.StartAt)
                     .LessThan(w => w.EndAt)
                     .WithMessage(localizer["StartTimeMustBeBeforeEndTime"])
                     .NotEmpty()
                     .WithMessage(localizer["Required"]);

                 workTime.RuleFor(w => w.DayOfWeek)
                     .IsInEnum().WithMessage(localizer["InvalidDayOfWeek"])
                      .NotEmpty()
                     .WithMessage(localizer["Required"]); ;
             });

            RuleFor(a=>a.DoctorID)
                .NotEmpty()
                .WithMessage(localizer["Required"]);



            RuleFor(a => a.ClinicId)
                .NotEmpty()
                .WithMessage(localizer["Required"]);
        }
    }
}
