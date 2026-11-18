using System;
using System.Collections.Generic;
using System.Text;

namespace Tabeeb.Domain.Common
{
    public class PaymobOptions
    {
        public string BaseUrl { get; set; } = "https://accept.paymob.com";

        public string SecretKey { get; set; } = null!;

        public string PublicKey { get; set; } = null!;

        public string HmacSecret { get; set; } = null!;

        public int IntegrationId { get; set; }

        public string NotificationUrl { get; set; } = null!;

        public string RedirectUrl { get; set; } = null!;
    }
}
