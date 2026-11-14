using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Tabeeb.Application.Common.Exceptions;
using Tabeeb.Application.Common.Services;
using Tabeeb.Domain.Entities;
using Tabeeb.Resources;

namespace Tabeeb.Application.Areas.ClinicDoctor.Command.AddClinicDoctor
{
    public  class AddClinicDoctorCommand:IRequest
    {
       

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
      IStringLocalizer<SharedResource> localizer,
      IEmailSender emailSender)
      : IRequestHandler<AddClinicDoctorCommand>
    {
        public async Task Handle(AddClinicDoctorCommand request, CancellationToken cancellationToken)
        {
           
                await ProcessNewDoctorAsync(request, cancellationToken);

           
        }

        private async Task ProcessNewDoctorAsync(AddClinicDoctorCommand doctorDto, CancellationToken cancellationToken)
        {
            var clinic = appDbContext.Clinics.FirstOrDefault(a => a.Id == a.Id);

            if (clinic is null)
                throw new ItemNotFoundException(localizer["NotFound"]);

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

                var password = GenerateSecurePassword();
                var userResult = await userManager.CreateAsync(user,password);
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



                await SendDoctorWelcomeEmail(DoctorData.Email, doctorData.ArName, clinic.ArName, password, "");

            }
            else
            {
                throw new Exception(localizer["AlreadyUsedEmail"]);  
              //await  AddDoctorClinicRelation(doctorDto, DoctorData.Doctor.Id);

            }
      
        }

     

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

        public static string GenerateSecurePassword(int length = 12)
        {
            const string validChars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*_-+=?";
            var chars = new char[length];

            using (var rng = RandomNumberGenerator.Create())
            {
                byte[] data = new byte[length];
                rng.GetBytes(data);

                for (int i = 0; i < length; i++)
                {
                    // Map the random byte to an index within our valid characters string
                    chars[i] = validChars[data[i] % validChars.Length];
                }
            }

            return new string(chars);
        }

        private async Task SendDoctorWelcomeEmail(string doctorEmail, string doctorName, string clinicName, string tempPassword, string loginUrl)
        {
            var body = $@"
    <!DOCTYPE html>
    <html lang='ar' dir='rtl'>
    <head>
        <meta charset='UTF-8'>
        <style>
            body {{
                font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
                background-color: #f4f6f9;
                margin: 0;
                padding: 0;
                color: #333333;
            }}
            .email-container {{
                max-width: 600px;
                margin: 40px auto;
                background: #ffffff;
                border-radius: 12px;
                overflow: hidden;
                box-shadow: 0 4px 15px rgba(0, 0, 0, 0.05);
            }}
            .email-header {{
                background: linear-gradient(135deg, #4361ee, #3a0ca3);
                color: #ffffff;
                text-align: center;
                padding: 30px 20px;
            }}
            .logo-box {{
                font-size: 24px;
                font-weight: bold;
                background: rgba(255, 255, 255, 0.15);
                display: inline-block;
                padding: 8px 20px;
                border-radius: 8px;
                letter-spacing: 1px;
            }}
            .email-header h1 {{
                margin: 15px 0 0 0;
                font-size: 22px;
                font-weight: 700;
            }}
            .email-body {{
                padding: 35px 30px;
            }}
            .greeting {{
                font-size: 18px;
                font-weight: 600;
                color: #1e293b;
                margin-bottom: 15px;
            }}
            .message-text {{
                font-size: 15px;
                line-height: 1.6;
                color: #475569;
                margin-bottom: 25px;
            }}
            .credentials-card {{
                background: #f8fafc;
                border: 1px solid #e2e8f0;
                border-radius: 8px;
                padding: 20px;
                margin-bottom: 25px;
            }}
            .credential-label {{
                font-weight: 600;
                color: #64748b;
                padding: 8px 0;
                font-size: 14px;
                width: 35%;
            }}
            .credential-value {{
                font-weight: 700;
                color: #1e293b;
                padding: 8px 0;
                font-size: 14px;
                text-align: left;
                direction: ltr;
            }}
            .btn-container {{
                text-align: center;
                margin: 30px 0 15px 0;
            }}
            .btn {{
                background: linear-gradient(135deg, #4361ee, #3a0ca3);
                color: #ffffff !important;
                padding: 12px 30px;
                border-radius: 8px;
                text-decoration: none;
                font-size: 15px;
                font-weight: bold;
                display: inline-block;
                box-shadow: 0 4px 10px rgba(67, 97, 238, 0.3);
            }}
            .warning-text {{
                font-size: 13px;
                color: #dc3545;
                margin-top: 15px;
                text-align: center;
            }}
            .email-footer {{
                background: #f1f5f9;
                text-align: center;
                padding: 20px;
                font-size: 13px;
                color: #94a3b8;
            }}
        </style>
    </head>
    <body>
        <div class='email-container'>
            <div class='email-header'>
                <div class='logo-box'>+ Tabeeb | طبيب</div>
                <h1>مرحباً بك في فريق عمل العيادة</h1>
            </div>
            <div class='email-body'>
                <div class='greeting'>دكتور {doctorName}، أهلاً بك معنا!</div>
                <div class='message-text'>
                    يسعدنا إبلاغك أنه تم إضافتك بنجاح إلى فريق عمل عيادة <strong>{clinicName}</strong> على منصة <strong>طبيب</strong>. أصبح بإمكانك الآن البدء في متابعة جدول الحجوزات والمرضى بكل سهولة عبر لوحة التحكم الخاصة بك.
                </div>
                
                <div class='credentials-card'>
                    <table width='100%' cellpadding='0' cellspacing='0' style='border-collapse: collapse;'>
                        <tr>
                            <td class='credential-label'>البريد الإلكتروني:</td>
                            <td class='credential-value'>{doctorEmail}</td>
                        </tr>
                        <tr>
                            <td class='credential-label' style='border-top: 1px solid #e2e8f0;'>كلمة المرور :</td>
                            <td class='credential-value' style='border-top: 1px solid #e2e8f0;'>{tempPassword}</td>
                        </tr>
                    </table>
                </div>

                <div class='btn-container'>
                    <a href='{loginUrl}' target='_blank' class='btn'>تسجيل الدخول إلى اللوحة</a>
                </div>

               
            </div>
            <div class='email-footer'>
                &copy; {DateTime.Now.Year} منصة طبيب (Tabeeb). جميع الحقوق محفوظة.
            </div>
        </div>
    </body>
    </html>";

            await emailSender.SendEmail(doctorEmail, body,null);
        }

    }
}
