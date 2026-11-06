using System;
using System.Collections.Generic;
using System.Text;
using Tabeeb.Domain.Enums;

namespace Tabeeb.Domain.Common
{
    public  class SubscriptionInvoiceDto
    {
        public string InvoiceNumber { get; set; } = string.Empty;
        public DateTime IssueDate { get; set; }
        public string ClinicName { get; set; } = string.Empty;
        public string PackageName { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal RemainingAmount => TotalAmount - PaidAmount;

        public int DaysCount {  get; set; }
        public SubscriptionInvoiceType Type { get; set; }

        // مسارات أصلية أو بيانات مصفوفة بايتبايت (Byte Array) للصور والـ QR

        public DateOnly StartAt { get; set; }

        public DateOnly EndAt { get; set; }
        public byte[]? QrCodeBytes { get; set; }
    }
}
