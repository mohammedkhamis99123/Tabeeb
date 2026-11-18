using System;
using System.Collections.Generic;
using System.Text;
using Tabeeb.Domain.Common;
using Tabeeb.Domain.Enums;

namespace Tabeeb.Domain.Entities
{
    public class PaymentTransaction : BaseAuditableEntity<long>
    {
        public Guid Reference { get; set; }

        public PaymentPurpose Purpose { get; set; }

        public PaymentStatus Status { get; set; }

        public decimal Amount { get; set; }

        public string Currency { get; set; } = "EGP";

        // Clinic
        public int ClinicId { get; set; }
        public Clinic Clinic { get; set; } = null!;

        // Subscription
        public long? SubscriptionId { get; set; }
        public Subscriptions? Subscription { get; set; }

        // Reservation
        public long? ReservationId { get; set; }
        public Reservation? Reservation { get; set; }

        // Paymob
        public string Provider { get; set; } = "Paymob";

        public string? ProviderOrderId { get; set; }

        public string? ProviderIntentionId { get; set; }

        public string? ProviderTransactionId { get; set; }

        public string? CheckoutUrl { get; set; }

        public string? FailureReason { get; set; }

        public DateTime? PaidAt { get; set; }
    }
}
