using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Text;
using Tabeeb.Application.Common.Services;
using Tabeeb.Domain.Entities;
using Tabeeb.Resources;

namespace Tabeeb.Application.Areas.ClinicDoctor.Command.AddClinicDoctor
{
    public  class AddClinicDoctorCommand:IRequest
    {
        //dpublic int? DoctorId { get; set; }

        public string ArName { get; set; } = null!;

        public string EnName { get; set; } = null!;

        public byte SpecializationId { get; set; }


        public string IdentityNumber { get; set; } = null!;
        public IFormFile Image { get; set; }

        public IFormFile Stamp { get; set; }
        

        public int SessionDuration { get; set; }

        public decimal SessionPrice { get; set; }


        public bool IsActive { get; set; }

        public string Email { get; set; }

        public int ClinicId { get; set; }
    }

    public class AddClinicDoctorCommandHandler(
      IAppDbContext appDbContext,
      UserManager<AppUser> userManager,
      IFileService fileService,
      IStringLocalizer<SharedResource> localizer)
      : IRequestHandler<AddClinicDoctorCommand>
    {
        public async Task Handle(AddClinicDoctorCommand request, CancellationToken cancellationToken)
        {
           
                await ProcessNewDoctorAsync(request, cancellationToken);

           
        }

        private async Task ProcessNewDoctorAsync(AddClinicDoctorCommand doctorDto, CancellationToken cancellationToken)
        {
            var DoctorData = appDbContext.Users
                             .Include(a=>a.Doctor)
                .Where(a=>a.Email==doctorDto.Email)
                .FirstOrDefault();

            if(DoctorData is null)
            {
                // أ) إنشاء حساب المستخدم في Identity
                var user = new AppUser
                {
                    ArName = doctorDto.ArName,
                    EnName = doctorDto.EnName,
                    Email = doctorDto.Email,
                    UserName = string.IsNullOrWhiteSpace(doctorDto.EnName)
                        ? doctorDto.Email
                        : doctorDto.EnName.Trim().Replace(" ", "_"),
                    ClinicId = doctorDto.ClinicId,

                };

                var userResult = await userManager.CreateAsync(user);
                if (!userResult.Succeeded)
                {
                    throw new Exception($"Failed to create user for doctor {doctorDto.EnName}: " +
                                        string.Join(", ", userResult.Errors.Select(e => e.Description)));
                }

                // ب) إنشاء بيانات الطبيب
                var doctorData = new Domain.Entities.Doctor
                {
                    ArName = doctorDto.ArName,
                    EnName = doctorDto.EnName,
                    IdentityNumber = doctorDto.IdentityNumber,
                    IsActive = doctorDto.IsActive,
                    SpecializationId = doctorDto.SpecializationId,
                    UserId = user.Id,

                };

                appDbContext.Doctors.Add(doctorData);
                await appDbContext.SaveChangesAsync(cancellationToken);

               await AddDoctorClinicRelation(doctorDto, doctorData.Id);

            }
            else
            {
                throw new Exception(localizer["AlreadyUsedEmail"]);  
              //await  AddDoctorClinicRelation(doctorDto, DoctorData.Doctor.Id);

            }
            // حفظ هنا لتوليد doctorData.Id

            // ج) إضافة مواعيد العمل
            //AddDoctorWorkTimes(doctorDto, doctorData.Id);

            // د) ربط الطبيب بالعيادة (DoctorClinic)
        }

       /* private void AddDoctorWorkTimes(AddClinicDoctorCommand doctorDto, int doctorId)
        {
            if (doctorDto.worktimeDtos == null || !doctorDto.worktimeDtos.Any())
                return;

            var workTimes = doctorDto.worktimeDtos.Select(item => new DoctorWorkTime
            {
                ClinicId = doctorDto.ClinicId,
                DoctorID = doctorId,
                DayOfWeek = item.DayOfWeek,
                StartAt = item.StartAt,
                EndAt = item.EndAt,
            }).ToList();

            appDbContext.DoctorWorkTimes.AddRange(workTimes);
        }
       */

        /// <summary>
        /// إنشاء رابط العيادة والطبيب DoctorClinic
        /// </summary>
        private async Task AddDoctorClinicRelation(AddClinicDoctorCommand doctorDto, int doctorId)
        {
            var doctorClinic = new DoctorClinic
            {
                ClinicId = doctorDto.ClinicId,
                DoctorId = doctorId,
                DoctorStamp = fileService.UploadFile(doctorDto.Stamp, "Stamps"),
                JoinDate = DateTime.UtcNow,
                SessionDuration = doctorDto.SessionDuration,
                SessionPrice = doctorDto.SessionPrice,
                ImagePath = fileService.UploadFile(doctorDto.Image, "Doctors"),
                IsActive = doctorDto.IsActive,

            };

            appDbContext.doctorClinics.Add(doctorClinic);
           await  appDbContext.SaveChangesAsync();
        }

    }
}
