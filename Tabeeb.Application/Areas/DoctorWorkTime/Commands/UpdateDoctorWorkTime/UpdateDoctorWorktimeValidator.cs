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
                 workTime.RuleFor(w => w)
                      .Must((w) => ValidateDates(w))
                     .WithMessage(localizer["InvalidDates"]);

                 workTime.RuleFor(w => w.DayOfWeek)
                     .IsInEnum().WithMessage(localizer["InvalidDayOfWeek"])
                      .NotNull()
                     .WithMessage(localizer["Required"]); ;
             });

            RuleFor(a=>a.DoctorID)
                .NotEmpty()
                .WithMessage(localizer["Required"]);



            RuleFor(a => a.ClinicId)
                .NotEmpty()
                .WithMessage(localizer["Required"]);
        }

        private bool ValidateDates(DoctorWorkTimeInfo w)
        {
            if (w.StartAt == null && w.EndAt == null && !w.IsActive) return true;
            if(w.IsActive &&(w.StartAt==null || w.EndAt==null)) return false;

            if(w.IsActive && w.StartAt.HasValue && w.EndAt.HasValue)
            {
                if (w.StartAt.Value < w.EndAt.Value) return true;

                return false;
            }
            return true;
        }
    }
}
