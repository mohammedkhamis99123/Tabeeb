using System;
using System.Collections.Generic;
using System.Text;

namespace Tabeeb.Application.Common.Services
{
    public interface IPaymentGateway
    {
        Task<PaymentInitiationResult> CreatePaymentAsync(
            PaymentInitiationRequest request,
            CancellationToken cancellationToken);
    }
}
