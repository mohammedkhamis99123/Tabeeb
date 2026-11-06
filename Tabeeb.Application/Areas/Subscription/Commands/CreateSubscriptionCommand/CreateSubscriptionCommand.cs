using MediatR;
using Microsoft.Extensions.Localization;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Text;
using Tabeeb.Application.Areas.Subscription.Commands.RenewSubscription;
using Tabeeb.Application.Common.Exceptions;
using Tabeeb.Application.Common.Services;
using Tabeeb.Domain.Common;
using Tabeeb.Domain.Entities;
using Tabeeb.Domain.Enums;
using Tabeeb.Resources;

namespace Tabeeb.Application.Areas.Subscription.Commands.CreateSubscriptionCommand
{
    public class CreateSubscriptionCommand : IRequest<byte[]>
    {
        public int ClinicId { get; set; }

        public byte PackageId { get; set; }

        public DateOnly StartAt { get; set; }
    }

    public class CreateSubscriptionCommandHandler
      (IAppDbContext appDbContext,
      IStringLocalizer<SharedResource> localizer,
      IInvoiceGenerator invoiceGenerator,
      IEmailSender emailSender)
      : IRequestHandler<CreateSubscriptionCommand, byte[]>
    {
        public async Task<byte[]> Handle(CreateSubscriptionCommand request, CancellationToken cancellationToken)
        {

            var package = await appDbContext.SubscriptionPackages.FindAsync(request.PackageId);
            if (package is null)
                throw new ItemNotFoundException(localizer["NotFound"]);

            var Clinic = await appDbContext.Clinics.FindAsync(request.ClinicId);
            if (Clinic is null)
                throw new ItemNotFoundException(localizer["NotFound"]);


            var Invoice = new SubscriptionInvoices
            {
                ClinicId = Clinic.Id,
                PackageId = request.PackageId,
                TotalAmount = package.Price,
                PaidAmount = package.Price,
                Type = SubscriptionInvoiceType.New,

            };
            await appDbContext.SubscriptionInvoices.AddAsync(Invoice);
            await appDbContext.SaveChangesAsync();


            var subscription = new Domain.Entities.Subscriptions
            {
                ClinicId = request.ClinicId,
                PackageID = request.PackageId,
                StartAt = request.StartAt,
                EndAt = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(package.DurationInDays)),
                Status = request.StartAt > DateOnly.FromDateTime(DateTime.Now) ? Domain.Enums.SubscriptionStatus.New : Domain.Enums.SubscriptionStatus.Current,
                InvoiceId=Invoice.Id

            };

            appDbContext.Subscriptions.Add(subscription);
            await appDbContext.SaveChangesAsync();
            var invoiceModel = new SubscriptionInvoiceDto
            {
                ClinicName = Clinic.ArName,
                InvoiceNumber = Invoice.Id.ToString(),
                IssueDate = DateTime.UtcNow.AddHours(3),
                PackageName = package.ArName,
                PaidAmount = package.Price,
                TotalAmount = package.Price,
                QrCodeBytes = GenerateInvoiceQrCode(Invoice.Id.ToString()),
                Type = Domain.Enums.SubscriptionInvoiceType.New

            };
            var InvoiceBytes = invoiceGenerator.GenerateSubscriptionInvoice(invoiceModel);

            await SendEmailToOwner(Clinic.OwnerEmail, Clinic.OwnerArName, subscription.StartAt.Value, subscription.EndAt.Value, InvoiceBytes);

            return InvoiceBytes;

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
                        <h1>تم تجديد اشتراك عيادتك بنجاح</h1>
                    </div>
                    <div class='email-body'>
                        <div class='greeting'>مرحباً د. {ownerName}،</div>
                        <div class='message-text'>
                            يسعدنا انضمامك إلى منصة <strong>طبيب</strong> الإدارية. تم باقة الاشتراك الخاصة بك بنجاح، وأصبح بإمكانك الآن البدء في إدارة عيادتك ومواعيدك بكل سهولة.
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

            await emailSender.SendEmail(ownerEmail, body, Attachemnt);

        }
        public byte[] GenerateInvoiceQrCode(string invoiceNumber)
        {
            // رابط الـ Web App الخاص بك (سواء كان على الـ Localhost أو السيرفر الحقيقي)
            // هذا الرابط سيفتح صفحة ويب بتصميم الفاتورة الاحترافي مباشرة للعميل
            string invoiceUrl = $"https://yourdomain.com/invoices/view/{invoiceNumber}";

            using var qrGenerator = new QRCodeGenerator();
            var qrData = qrGenerator.CreateQrCode(invoiceUrl, QRCodeGenerator.ECCLevel.Q);
            var qrCode = new PngByteQRCode(qrData);

            return qrCode.GetGraphic(20); // إرجاع الصورة كـ byte[] جاهزة لـ QuestPDF
        }
    }
}
