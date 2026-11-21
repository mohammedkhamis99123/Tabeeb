using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Tabeeb.Application.Common.Services;
using Tabeeb.Domain.Entities;

namespace Tabeeb.Application.Areas.Employee.Commands.AddEmployee
{
    public  class AddEmployeeCommand:IRequest
    {
        public string ArName { get; set; } = null!;

        public string EnName { get; set; } = null!;



        public string Addrees { get; set; } = null!;

        public string PhoneNumber { get; set; } = null!;

        public bool IsActive { get; set; }

        public int? DoctorId { get; set; }


        public IFormFile Image {  get; set; }
        public string IdentityNumber { get; set; } = null!;

        public string Email {  get; set; } = null!;

        public string Role { get; set; }


        public decimal Salary {  get; set; }

        public int WorkHours { get; set; }




     
    }

    public class AddEmployeeCommandHandler
        (IFileService fileService,
        IAppDbContext appDbContext,
        UserManager<AppUser> userManager,
        ICurrentUserService currentUser,
        RoleManager<AspNetRole> roleManager,
        IEmailSender emailSender)
        : IRequestHandler<AddEmployeeCommand>
    {
        public async Task Handle(AddEmployeeCommand request, CancellationToken cancellationToken)
        {
            var userID=await AddEmployeeUser(request);

            var data = request.Adapt<Domain.Entities.Employee>();
            data.ImagePath = fileService.UploadFile(request.Image, "Employees");
            data.UserId = userID;
            data.ClinicId = currentUser.ClinicId.Value;

           appDbContext.Employees.Add(data);
           await appDbContext.SaveChangesAsync();

          



        }

        private async Task<string> AddEmployeeUser(AddEmployeeCommand request)
        {
            var user = new AppUser
            {
                ArName = request.ArName,
                EnName = request.EnName,
                Email = request.Email,
                UserName = string.IsNullOrWhiteSpace(request.EnName)
                   ? request.Email
                   : request.EnName.Trim().Replace(" ", "_"),
                ClinicId=currentUser.ClinicId.Value
            };

            var password = GenerateIdentityCompatiblePassword();
            var userResult = await userManager.CreateAsync(user,password);

            var role = await roleManager.FindByNameAsync(request.Role);
            await userManager.AddToRoleAsync(user, role.Name);
            if (!userResult.Succeeded)
            {

                throw new Exception($"Failed to create user for doctor {request.EnName}: " +
                                    string.Join(", ", userResult.Errors.Select(e => e.Description)));
            }
            var clinicName = appDbContext.Clinics.FirstOrDefault(a => a.Id == currentUser.ClinicId.Value).ArName;
            await SendEmployeeWelcomeEmail(request.Email, request.ArName, clinicName, password, "..");
            return user.Id;

        }

        public static string GenerateIdentityCompatiblePassword(int length = 10)
        {
            // التأكد من أن الطول لا يقل عن 8 خانات حسب إعداداتك
            if (length < 8) length = 8;

            const string uppercaseChars = "ABCDEFGHJKLMNOPQRSTUVWXYZ";
            const string lowercaseChars = "abcdefghijkmnopqrstuvwxyz";
            const string digitChars = "0123456789";
            const string allChars = uppercaseChars + lowercaseChars + digitChars;

            var passwordChars = new char[length];

            using (var rng = RandomNumberGenerator.Create())
            {
                byte[] data = new byte[length];
                rng.GetBytes(data);

                // 1. ضمان وجود حرف كبير واحد على الأقل (استجابة لـ RequireUppercase)
                passwordChars[0] = uppercaseChars[data[0] % uppercaseChars.Length];

                // 2. ضمان وجود حرف صغير واحد على الأقل (استجابة لـ RequireLowercase)
                passwordChars[1] = lowercaseChars[data[1] % lowercaseChars.Length];

                // 3. ضمان وجود رقم واحد على الأقل (استجابة لـ RequireDigit)
                passwordChars[2] = digitChars[data[2] % digitChars.Length];

                // 4. ملء باقي الحروف عشوائياً من المجموعات الكلية
                for (int i = 3; i < length; i++)
                {
                    passwordChars[i] = allChars[data[i] % allChars.Length];
                }

                // 5. خلط عناصر مصفوفة الحروف عشوائياً حتى لا تكون الشروط ثابتة في البداية
                for (int i = length - 1; i > 0; i--)
                {
                    int j = data[i] % (i + 1);
                    char temp = passwordChars[i];
                    passwordChars[i] = passwordChars[j];
                    passwordChars[j] = temp;
                }
            }

            return new string(passwordChars);
        }

        private async Task SendEmployeeWelcomeEmail(string employeeEmail, string employeeName, string clinicName, string tempPassword, string loginUrl)
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
                <div class='greeting'>موظف  {employeeName}، أهلاً بك معنا!</div>
                <div class='message-text'>
                    يسعدنا إبلاغك أنه تم إضافتك بنجاح إلى فريق عمل عيادة <strong>{clinicName}</strong> على منصة <strong>طبيب</strong>. أصبح بإمكانك الآن البدء في متابعة جدول الحجوزات والمرضى بكل سهولة عبر لوحة التحكم الخاصة بك.
                </div>
                
                <div class='credentials-card'>
                    <table width='100%' cellpadding='0' cellspacing='0' style='border-collapse: collapse;'>
                        <tr>
                            <td class='credential-label'>البريد الإلكتروني:</td>
                            <td class='credential-value'>{employeeEmail}</td>
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

            await emailSender.SendEmail(employeeEmail, body, null);
        }

    }
}
