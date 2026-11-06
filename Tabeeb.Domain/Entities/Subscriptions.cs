using System;
using System.Collections.Generic;
using System.Text;
using Tabeeb.Domain.Common;
using Tabeeb.Domain.Enums;

namespace Tabeeb.Domain.Entities
{
    public  class Subscriptions:BaseAuditableEntity<long>
    {
        public Clinic Clinics { get; set; } = null!;

        public SubscriptionPackage Package { get; set; } = null!;


        public int ClinicId {  get; set; }


        public byte PackageID { get; set; }


        public DateOnly? StartAt {  get; set; }


        public DateOnly? EndAt { get; set; }

        public SubscriptionStatus Status { get; set; }

        public long InvoiceId {  get; set; }

        public SubscriptionInvoices Invoice { get; set; }
    }
}
