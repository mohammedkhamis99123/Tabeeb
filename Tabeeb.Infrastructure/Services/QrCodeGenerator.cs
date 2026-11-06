using QRCoder;
using System;
using System.Collections.Generic;
using System.Text;
using Tabeeb.Application.Common.Services;

namespace Tabeeb.Infrastructure.Services
{
    public class QrCodeGenerator : IQrCodeGernerator
    {
        public async Task<byte[]> GetSubscriptionInvoiceQrCode(string InvoiceNumber)
        {

            // رابط الـ Web App الخاص بك (سواء كان على الـ Localhost أو السيرفر الحقيقي)
            // هذا الرابط سيفتح صفحة ويب بتصميم الفاتورة الاحترافي مباشرة للعميل
            string invoiceUrl = $"https://yourdomain.com/invoices/view/{InvoiceNumber}";

            using var qrGenerator = new QRCodeGenerator();
            var qrData = qrGenerator.CreateQrCode(invoiceUrl, QRCodeGenerator.ECCLevel.Q);
            var qrCode = new PngByteQRCode(qrData);

            return qrCode.GetGraphic(20); // إرجاع الصورة كـ byte[] جاهزة لـ QuestPDF

        }
    }
}
