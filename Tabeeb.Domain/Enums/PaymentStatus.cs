using System;
using System.Collections.Generic;
using System.Text;

namespace Tabeeb.Domain.Enums
{
    public enum PaymentStatus : byte
    {
        Pending = 1,
        Paid = 2,
        Failed = 3,
        Cancelled = 4,
        Refunded = 5,
        PartiallyRefunded = 6
    }
}
