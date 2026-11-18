using System;
using System.Collections.Generic;
using System.Text;

namespace Tabeeb.Application.Common
{
    public class PaymentInitiationRequest
    {
        public Guid Reference { get; set; }

        public decimal Amount { get; set; }

        public string Currency { get; set; } = "EGP";

        public string CustomerName { get; set; } = null!;

        public string CustomerEmail { get; set; } = null!;

        public string CustomerPhone { get; set; } = null!;
    }

    public class PaymentInitiationResult
    {
        public string IntentionId { get; set; } = null!;

        public string ClientSecret { get; set; } = null!;

        public string CheckoutUrl { get; set; } = null!;
    }
}
