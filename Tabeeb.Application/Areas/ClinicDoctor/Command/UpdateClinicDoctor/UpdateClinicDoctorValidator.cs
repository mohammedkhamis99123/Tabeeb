using FluentValidation;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Text;
using Tabeeb.Application.Common.Services;
using Tabeeb.Resources;

namespace Tabeeb.Application.Areas.ClinicDoctor.Command.UpdateClinicDoctor
{
    public  class UpdateClinicDoctorValidator:AbstractValidator<UpdateDoctorClinic>
    {
        private readonly IAppDbContext appDbContext;
        private readonly IStringLocalizer<SharedResource> localizer;

        public UpdateClinicDoctorValidator(IAppDbContext appDbContext, IStringLocalizer<SharedResource> localizer)
        {
            this.appDbContext = appDbContext;
            this.localizer = localizer;

            // 1. الاسم بالعربية
            //RuleFor(d => d.ArName)
            //     .NotEmpty().WithMessage(localizer["Required"])
            //     .MaximumLength(150).WithMessage(localizer["MaxLength", 150]);

            //RuleFor(d => d.EnName)
            //            .NotEmpty().WithMessage(localizer["Required"])
            //            .MaximumLength(150).WithMessage(localizer["MaxLength", 150]);

            //// 3. البريد الإلكتروني (مطلوب ولابد أن يكون صيغة بريد صحيحة)
            //RuleFor(d => d.Email)
            //     .NotEmpty().WithMessage(localizer["EmailRequired"])
            //     .EmailAddress().WithMessage(localizer["InvalidEmailFormat"]);

            //// 4. التخصص
            //RuleFor(d => d.SpecializationId)
            //     .NotEmpty()
            //     .WithMessage(localizer["Required"])
            //     .MustAsync((id, CancellationToken) => IsValidSpecialization(id))
            //     .WithMessage(localizer["InvalidSpecialization"]);

            //RuleFor(a => a.IdentityNumber)
            //   .NotEmpty()
            //   .WithMessage(localizer["Required"]);


            RuleFor(a => a.SessionDuration)
                .NotEmpty()
                .WithMessage(localizer["Required"])
                .GreaterThan(0)
                .WithMessage(localizer["InvalidSessionDuration"]);

            RuleFor(a => a.SessionPrice)
             .NotEmpty()
             .WithMessage(localizer["Required"])
             .GreaterThanOrEqualTo(1)
             .WithMessage(localizer["InvalidSessionPrice"])
             ;

            //RuleFor(A => A.Image)
            //   .NotEmpty()
            //   .WithMessage(localizer["Required"]);



            // 5. العيادة
            RuleFor(d => d.ClinicId)
                 .GreaterThan(0).WithMessage(localizer["ClinicRequired"]);

            // 6. التحقق من مواعيد العمل الخاصة بالطبيب (Child Collection)
            //RuleForEach(d => d.worktimeDtos).ChildRules(workTime =>
            //{
            //    workTime.RuleFor(w => w.StartAt)
            //        .LessThan(w => w.EndAt)
            //        .WithMessage(localizer["StartTimeMustBeBeforeEndTime"]);

            //    workTime.RuleFor(w => w.DayOfWeek)
            //        .IsInEnum().WithMessage(localizer["InvalidDayOfWeek"]);
            //});


        }

        private async Task<bool> IsValidSpecialization(byte id)
        {
            return appDbContext.Specializations.Any(x => x.Id == id);
        }

    }
}
