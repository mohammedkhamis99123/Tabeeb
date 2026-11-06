using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Text;
using Tabeeb.Application.Common.Exceptions;
using Tabeeb.Application.Common.Services;
using Tabeeb.Domain.Common;
using Tabeeb.Domain.Entities;
using Tabeeb.Domain.Enums;
using Tabeeb.Resources;

namespace Tabeeb.Application.Areas.Subscription.Queries.GetInvoice
{
    public record GetInvoiceQuery(long id) : IRequest<byte[]>;

    public class GetInvoiceQueryHandler
        (IAppDbContext appDbContext,
        IStringLocalizer<SharedResource> localizer,
        IInvoiceGenerator invoiceGenerator)
        : IRequestHandler<GetInvoiceQuery, byte[]>
    {

        

        public async Task<byte[]> Handle(GetInvoiceQuery request, CancellationToken cancellationToken)
        {
            var Subscription = await appDbContext.Subscriptions
                                 .Include(a=>a.Clinics)
                                 .Include(A=>A.Package)
                                .FirstOrDefaultAsync(a => a.Id == request.id);

            if (Subscription is null)
                throw new ItemNotFoundException(localizer["NotFound"]);

            var invoice = await appDbContext.SubscriptionInvoices
                .FirstOrDefaultAsync(a => a.Id == Subscription.InvoiceId);

            if (invoice is null)
                throw new ItemNotFoundException(localizer["NotFound"]);

            int days = DateOnly.FromDateTime(DateTime.UtcNow).DayNumber - Subscription.StartAt.Value.DayNumber;

            var dayPrice = (decimal)Subscription.Package.Price / Subscription.Package.DurationInDays;
            var daysprice = (dayPrice * days);

            var invoiceModel = new SubscriptionInvoiceDto
            {
                ClinicName = Subscription.Clinics.ArName,
                InvoiceNumber = invoice.Id.ToString(),
                IssueDate = DateTime.UtcNow.AddHours(3),
                PackageName =Subscription.Package.ArName,
                TotalAmount =Subscription.Package.Price,
                QrCodeBytes = GenerateInvoiceQrCode(invoice.Id.ToString()),
                Type = (Subscription.Status==SubscriptionStatus.New|| Subscription.Status == SubscriptionStatus.Current)
                ? SubscriptionInvoiceType.New :Subscription.Status==SubscriptionStatus.Canceled ? SubscriptionInvoiceType.Cancel :SubscriptionInvoiceType.Renew,
                DaysCount = days,
                StartAt = Subscription.StartAt.Value

            };
            var InvoiceBytes = invoiceGenerator.GenerateSubscriptionInvoice(invoiceModel);

            return InvoiceBytes;
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
