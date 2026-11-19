using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Tabeeb.Application.Common;
using Tabeeb.Application.Common.Services;
using Tabeeb.Domain.Entities;
using Tabeeb.Domain.Enums;

namespace Tabeeb.Application.Areas.Reservation.Commands
{


    public class CreatePaymentCommand : IRequest<PaymentInitiationResult>
    {
        public PaymentPurpose Purpose { get; set; }

        public long? ReservationId { get; set; }

        public long? SubscriptionId { get; set; }
    }
    public class CreatePaymentCommandHandler
    : IRequestHandler<CreatePaymentCommand, PaymentInitiationResult>
    {
        private readonly IAppDbContext _context;
        private readonly IPaymentGateway _paymentGateway;

        public CreatePaymentCommandHandler(
            IAppDbContext context,
            IPaymentGateway paymentGateway)
        {
            _context = context;
            _paymentGateway = paymentGateway;
        }

        public async Task<PaymentInitiationResult> Handle(
            CreatePaymentCommand request,
            CancellationToken cancellationToken)
        {
            // 1. Validate request
            if (request.Purpose == PaymentPurpose.PatientReservation &&
                (!request.ReservationId.HasValue || request.SubscriptionId.HasValue))
            {
                throw new ArgumentException(
                    "A valid ReservationId is required for reservation payment.");
            }

            if (request.Purpose == PaymentPurpose.ClinicSubscription &&
                (!request.SubscriptionId.HasValue || request.ReservationId.HasValue))
            {
                throw new ArgumentException(
                    "A valid SubscriptionId is required for subscription payment.");
            }

            decimal amount;
            int clinicId;
            string itemName;
            string customerName;
            string customerEmail;
            string customerPhone;

            long? reservationId = null;
            long? subscriptionId = null;

            // 2. Determine payment data from database
            switch (request.Purpose)
            {
                case PaymentPurpose.PatientReservation:
                    {
                        var reservation = await _context.Reservations
                            .Include(r => r.Customer)
                            .FirstOrDefaultAsync(
                                r => r.Id == request.ReservationId,
                                cancellationToken);

                        if (reservation == null)
                            throw new KeyNotFoundException("Reservation not found.");

                        if (reservation.TotalAmount <= 0)
                            throw new InvalidOperationException(
                                "Reservation amount must be greater than zero.");

                        amount = reservation.TotalAmount;
                        clinicId = reservation.ClinicId;
                        reservationId = reservation.Id;

                        itemName = $"Reservation #{reservation.ReservationNo}";
                        customerName = reservation.Customer.ArName;
                        customerEmail = reservation.Customer.Email;
                        customerPhone = reservation.Customer.PhoneNumber;

                        break;
                    }

                case PaymentPurpose.ClinicSubscription:
                    {
                        var subscription = await _context.Subscriptions
                            .Include(s => s.Package)
                            .Include(s => s.Clinics)
                            .FirstOrDefaultAsync(
                                s => s.Id == request.SubscriptionId,
                                cancellationToken);

                        if (subscription == null)
                            throw new KeyNotFoundException("Subscription not found.");

                        if (subscription.Package.Price <= 0)
                            throw new InvalidOperationException(
                                "Subscription amount must be greater than zero.");

                        amount = subscription.Package.Price;
                        clinicId = subscription.ClinicId;
                        subscriptionId = subscription.Id;

                        itemName = subscription.Package.EnName;
                        customerName = subscription.Clinics.OwnerEnName;
                        customerEmail = subscription.Clinics.OwnerEmail;
                        customerPhone = subscription.Clinics.WhatsappNumber;

                        break;
                    }

                default:
                    throw new ArgumentOutOfRangeException(
                        nameof(request.Purpose),
                        "Unsupported payment purpose.");
            }

            // 3. Create local payment transaction
            var payment = new PaymentTransaction
            {
                Reference = Guid.NewGuid(),
                Purpose = request.Purpose,
                Status = PaymentStatus.Pending,
                Amount = amount,
                Currency = "EGP",
                ClinicId = clinicId,
                ReservationId = reservationId,
                SubscriptionId = subscriptionId,
                Provider = "Paymob"
            };

            _context.PaymentTransactions.Add(payment);

            await _context.SaveChangesAsync(cancellationToken);

            try
            {
                // 4. Prepare Paymob request
                var gatewayRequest = new PaymentInitiationRequest
                {
                    Reference = payment.Reference,
                    Amount = payment.Amount,
                    Currency = payment.Currency,
                    ItemName = itemName,
                    CustomerName = customerName,
                    CustomerEmail = customerEmail,
                    CustomerPhone = customerPhone
                };

                // 5. Create payment with Paymob
                var gatewayResult = await _paymentGateway.CreatePaymentAsync(
                    gatewayRequest,
                    cancellationToken);

                // 6. Save Paymob response
                payment.ProviderIntentionId = gatewayResult.IntentionId;
                payment.ProviderOrderId = gatewayResult.OrderId;
                payment.CheckoutUrl = gatewayResult.CheckoutUrl;

                await _context.SaveChangesAsync(cancellationToken);

                return gatewayResult;
            }
            catch
            {
                // 7. Mark payment as failed if initiation fails
                payment.Status = PaymentStatus.Failed;

                await _context.SaveChangesAsync(cancellationToken);

                throw;
            }
        }
    }
}
