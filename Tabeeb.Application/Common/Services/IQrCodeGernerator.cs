using QRCoder;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tabeeb.Application.Common.Services
{
    public interface IQrCodeGernerator
    {
       Task<byte[]> GetSubscriptionInvoiceQrCode(string InvoiceNumber);
    
    }
}
