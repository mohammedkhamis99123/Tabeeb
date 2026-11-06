using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Text;
using Tabeeb.Domain.Common;
using Tabeeb.Domain.Enums;

namespace Tabeeb.Domain.Entities
{
    public  class SubscriptionInvoices:BaseAuditableEntity<long>
    {
        public int ClinicId {  get; set; }

        public byte PackageId {  get; set; }

        public SubscriptionInvoiceType Type {  get; set; }

        public decimal TotalAmount {  get; set; }

        public decimal PaidAmount {  get; set; }


        public SubscriptionPackage Package { get; set; }

        public Clinic Clinic { get; set; }

        public Subscriptions Subscriptions { get; set; }

    }
}
