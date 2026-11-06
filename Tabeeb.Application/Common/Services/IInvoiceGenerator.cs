using System;
using System.Collections.Generic;
using System.Text;
using Tabeeb.Domain.Common;

namespace Tabeeb.Application.Common.Services
{
    public  interface IInvoiceGenerator
    {
        public byte[] GenerateSubscriptionInvoice(SubscriptionInvoiceDto subscriptionInvoice);
    }
}
