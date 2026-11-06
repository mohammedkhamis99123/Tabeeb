using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Tabeeb.Application.Areas.ClinicDoctor.Command.AddClinicDoctor;
using Tabeeb.Application.Common.Services;
using Tabeeb.Domain.Entities;

namespace Tabeeb.Application.Areas.ClinicDoctor.Command.UpdateClinicDoctor
{
    public  class UpdateDoctorClinic:IRequest
    {
        public int? DoctorId { get; set; }

        //public string ArName { get; set; } = null!;

        //public string EnName { get; set; } = null!;

        //public byte SpecializationId { get; set; }


        //public string IdentityNumber { get; set; } = null!;

   
        public IFormFile? Image { get; set; }

        public IFormFile? Stamp { get; set; }
       

        public int SessionDuration { get; set; }

        public decimal SessionPrice { get; set; }


        public bool IsActive { get; set; }

        //public string Email { get; set; }
        public DateTime JoinDate { get; set; }

        public int ClinicId { get; set; }
    }

    public class UpdateDoctorClinicHandler(
  IAppDbContext appDbContext,
  UserManager<AppUser> userManager,
  IFileService fileService)
  : IRequestHandler<UpdateDoctorClinic>
    {
        public async Task Handle(UpdateDoctorClinic request, CancellationToken cancellationToken)
        {
            await ProcessExistingDoctorAsync(request, cancellationToken);
        }

        private async Task ProcessExistingDoctorAsync(UpdateDoctorClinic doctorDto, CancellationToken cancellationToken)
        {
            var existingDoctor = await appDbContext.Doctors
                .FirstOrDefaultAsync(d => d.Id == doctorDto.DoctorId, cancellationToken);

            if (existingDoctor == null)
                return;

            // أ) تحديث بيانات الـ Entity الخاصة بالطبيب
            //existingDoctor.ArName = doctorDto.ArName;
            //existingDoctor.EnName = doctorDto.EnName;
            //existingDoctor.IdentityNumber = doctorDto.IdentityNumber;
            existingDoctor.IsActive = doctorDto.IsActive;
            //existingDoctor.SpecializationId = doctorDto.SpecializationId;

            // ب) تحديث بيانات الـ AppUser الخاص به
            //await UpdateDoctorUserAsync(doctorDto);

            // ج) تحديث مواعيد العمل (حذف القديم وإضافة الجديد)
            //await UpdateDoctorWorkTimesAsync(doctorDto, cancellationToken);


            // د) ملاحظة: إن كان هناك إمكانية لتحديث DoctorClinic أيضاً للقدامى يمكن إضافتها هنا
            await UpdateDoctorClinicRelationAsync(doctorDto, cancellationToken);
            await appDbContext.SaveChangesAsync();
        }

        //private async Task UpdateDoctorUserAsync(UpdateDoctorClinic doctorDto)
        //{
        //    if (string.IsNullOrEmpty(doctorDto.Email))
        //        return;

        //    var user = await userManager.FindByEmailAsync(doctorDto.Email);
        //    if (user != null)
        //    {
        //        user.ArName = doctorDto.ArName;
        //        user.EnName = doctorDto.EnName;
        //        user.UserName = doctorDto.EnName.Trim().Replace(" ", "_");
        //        user.Email = doctorDto.Email;
        //        user.ClinicId = doctorDto.ClinicId;
        //        await userManager.UpdateAsync(user);
        //    }
        //}

     
     
        private async Task UpdateDoctorClinicRelationAsync(UpdateDoctorClinic doctorDto, CancellationToken cancellationToken)
        {
            var existingDoctorClinic = await appDbContext.doctorClinics
                .FirstOrDefaultAsync(dc => dc.DoctorId == doctorDto.DoctorId && dc.ClinicId == doctorDto.ClinicId, cancellationToken);

            if (existingDoctorClinic != null)
            {
                // تحديث البيانات الحالية
                existingDoctorClinic.SessionPrice = doctorDto.SessionPrice;
                existingDoctorClinic.SessionDuration = doctorDto.SessionDuration;

                // تحديث الختم فقط إذا تم إرسال قيمة جديدة
                if (doctorDto.Stamp!=null)
                {
                    fileService.DeleteFile(existingDoctorClinic.DoctorStamp);
                    existingDoctorClinic.DoctorStamp = fileService.UploadFile(doctorDto.Stamp,"Stamps");
                }
              
              
            
                // تحديث الختم فقط إذا تم إرسال قيمة جديدة
                if (doctorDto.Image!=null)
                {
                    fileService.DeleteFile(existingDoctorClinic.ImagePath);

                    existingDoctorClinic.DoctorStamp = fileService.UploadFile(doctorDto.Stamp, "Doctors");
                }
                appDbContext.doctorClinics.Update(existingDoctorClinic);
            }
            else
            {
                // في حال لم يكن مرتبطاً بهذه العيادة من قبل، ننشئ سجلاً جديداً
                var newDoctorClinic = new DoctorClinic
                {
                    ClinicId = doctorDto.ClinicId,
                    DoctorId = doctorDto.DoctorId!.Value,
                    DoctorStamp = fileService.UploadFile(doctorDto.Stamp, "Stamps"),
                    JoinDate = DateTime.UtcNow,
                    SessionDuration = doctorDto.SessionDuration,
                    SessionPrice = doctorDto.SessionPrice,
                    ImagePath = fileService.UploadFile(doctorDto.Image, "Doctors")

                };

                await appDbContext.doctorClinics.AddAsync(newDoctorClinic, cancellationToken);
            }

            await appDbContext.SaveChangesAsync();
        }
      

    }
}
