using Mapster;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Text;
using Tabeeb.Application.Common.Services;
using Tabeeb.Domain.Common;
using Tabeeb.Domain.Entities;
using Tabeeb.Domain.Enums;

namespace Tabeeb.Application.Areas.Clinic.Commands.AddClinic
{
    public  class AddClinicCommand:IRequest<int>
    {
        public string ArName { get; set; } = null!;

        public string EnName { get; set; } = null!;


        public string StreetArName { get; set; } = null!;

        public string StreetEnName { get; set; } = null!;

        public string Langitude { get; set; } = null!;


        public string Latitude { get; set; } = null!;

        public int CityId { get; set; }

        public byte SubscriptionPackageId {  get; set; }
        public string? ArDescription { get; set; }

        public string? EnDescription { get; set; }


        public string? ProfileAr { get; set; }


        public string? ProfileEn { get; set; }

        public string OwnerArName {  get; set; } = null!;

        public string OwnerEnName { get; set; } = null!;

        public string OwnerEmail {  get; set; }

        public string OwnerPassword { get; set; }

        public string SnapshatUser { get; set; }

        public string InstgramUser { get; set; } = null!;

        public string WhatsappNumber { get; set; }

        public bool IsActive {  get; set; }

        public bool ActivateOnlineBooking {  get; set; }
        public DateOnly SubscriptionStartAt { get; set; }


    }

    public class AddClinicCommandHandler(IAppDbContext appDbContext,
        UserManager<AppUser> userManager,
        IEmailSender emailSender,
        IInvoiceGenerator invoiceGenerator,
        IQrCodeGernerator qrCodeGernerator
        ) : IRequestHandler<AddClinicCommand, int>
    {
        public async Task<int> Handle(AddClinicCommand request, CancellationToken cancellationToken)
        {
            var data = request.Adapt<Domain.Entities.Clinic>();
            await appDbContext.Clinics.AddAsync(data);
            await appDbContext.SaveChangesAsync();

            var user = new AppUser
            {
                ArName = request.OwnerArName,
                EnName = request.OwnerEnName,
                Email = request.OwnerEmail,
                UserName = request.OwnerEmail,
                ClinicId=data.Id


            };

            var result=await userManager.CreateAsync(user,request.OwnerPassword);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, "Admin");
            }

            var package = await appDbContext.SubscriptionPackages.FirstOrDefaultAsync(a => a.Id == request.SubscriptionPackageId);

            var Invoice = new SubscriptionInvoices
            {
                ClinicId = data.Id,
                PackageId = request.SubscriptionPackageId,
                TotalAmount = package.Price,
                PaidAmount = package.Price,
                Type = SubscriptionInvoiceType.New,


            };
            await appDbContext.SubscriptionInvoices.AddAsync(Invoice);
            await appDbContext.SaveChangesAsync();

            var Subscription = new Subscriptions
            {
                ClinicId = data.Id,
                PackageID = request.SubscriptionPackageId,
                EndAt = request.SubscriptionStartAt.AddDays(package.DurationInDays),
                StartAt=request.SubscriptionStartAt,
                InvoiceId=Invoice.Id
            };
            appDbContext.Subscriptions.Add(Subscription);
            await appDbContext.SaveChangesAsync();

            var invoiceModel = new SubscriptionInvoiceDto
            {
                ClinicName = request.ArName,
                InvoiceNumber = Invoice.Id.ToString(),
                IssueDate = DateTime.UtcNow.AddHours(3),
                PackageName = package.ArName,
                PaidAmount = package.Price,
                TotalAmount = package.Price,
                QrCodeBytes = await qrCodeGernerator.GetSubscriptionInvoiceQrCode(Invoice.Id.ToString()),

            };
           var InvoiceBytes= invoiceGenerator.GenerateSubscriptionInvoice(invoiceModel);

            await SendEmailToOwner(request.OwnerEmail,request.OwnerArName,Subscription.StartAt.Value,Subscription.EndAt.Value,InvoiceBytes);
            return data.Id;



        }

      
        private async Task SendEmailToOwner(string ownerEmail, string ownerName, DateOnly startDate, DateOnly endDate, byte[] Attachemnt)
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
                    .details-card {{
                        background: #f8fafc;
                        border: 1px solid #e2e8f0;
                        border-radius: 8px;
                        padding: 20px;
                        margin-bottom: 25px;
                    }}
                    .detail-row {{
                        display: flex;
                        justify-content: space-between;
                        padding: 10px 0;
                        border-bottom: 1px solid #e2e8f0;
                        font-size: 14px;
                    }}
                    .detail-row:last-child {{
                        border-bottom: none;
                    }}
                    .detail-label {{
                        font-weight: 600;
                        color: #64748b;
                    }}
                    .detail-value {{
                        font-weight: 700;
                        color: #1e293b;
                    }}
                    .badge-active {{
                        background-color: #dcfce7;
                        color: #16a34a;
                        padding: 4px 10px;
                        border-radius: 6px;
                        font-size: 12px;
                        font-weight: 600;
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
                        <!-- مكان اللوجو أو النص التعريفي -->
                        <div class='logo-box'>+ Tabeeb | طبيب</div>
                        <h1>تم تفعيل اشتراك عيادتك بنجاح</h1>
                    </div>
                    <div class='email-body'>
                        <div class='greeting'>مرحباً د. {ownerName}،</div>
                        <div class='message-text'>
                            يسعدنا انضمامك إلى منصة <strong>طبيب</strong> الإدارية. تم إنشاء عيادتك وتفعيل باقة الاشتراك الخاصة بك بنجاح، وأصبح بإمكانك الآن البدء في إدارة عيادتك ومواعيدك بكل سهولة.
                        </div>
                        
                        <div class='details-card'>
                            <div class='detail-row'>
                                <span class='detail-label'>حالة الاشتراك:</span>
                                <span class='detail-value'><span class='badge-active'>نشط (Active)</span></span>
                            </div>
                            <div class='detail-row'>
                                <span class='detail-label'>تاريخ بداية الاشتراك:</span>
                                <span class='detail-value'>{startDate:yyyy-MM-dd}</span>
                            </div>
                            <div class='detail-row'>
                                <span class='detail-label'>تاريخ نهاية الاشتراك:</span>
                                <span class='detail-value'>{endDate:yyyy-MM-dd}</span>
                            </div>
                        </div>

                        <div class='message-text' style='margin-bottom: 0;'>
                            إذا واجهتك أي مشكلة أو كان لديك استفسار، فريق الدعم الفني يسعده دائماً مساعدتك.
                        </div>
                    </div>
                    <div class='email-footer'>
                        &copy; {DateTime.Now.Year} منصة طبيب (Tabeeb). جميع الحقوق محفوظة.
                    </div>
                </div>
            </body>
            </html>";

            await emailSender.SendEmail(ownerEmail,  body,Attachemnt);
        
    }
  
    }
}
