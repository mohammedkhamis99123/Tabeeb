using System;
using System.Collections.Generic;
using System.Text;
using Tabeeb.Domain.Common;

namespace Tabeeb.Domain.Entities
{
    public  class SubscriptionPackage:BaseAuditableEntity<byte>
    {

        public string ArName { get; set; } = null!;

        public string EnName { get; set; } = null!;

        public string ArDescription {  get; set; } = null!;

        public string EnDescription {  get; set; } = null!;
        public int DurationInDays {  get; set; }

        public decimal Price {  get; set; }
        public bool IsActive {  get; set; }
        public virtual ICollection<Subscriptions> Subscriptions { get; set; }=new List<Subscriptions>();

        public virtual ICollection<SubscriptionInvoices> Invoices { get; set; } = new List<SubscriptionInvoices>();
    }
}
