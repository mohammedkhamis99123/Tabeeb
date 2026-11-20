using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Text;
using Tabeeb.Application.Common;
using Tabeeb.Application.Common.Exceptions;
using Tabeeb.Application.Common.Services;
using Tabeeb.Domain.Entities;
using Tabeeb.Domain.Enums;
using Tabeeb.Resources;

namespace Tabeeb.Application.Areas.Reservation.Commands
{


    public class CreateReservationCommand : IRequest<PaymentInitiationResult>
    {
        public PaymentPurpose Purpose { get; set; }

        public DateTime Date { get; set; }



        public long CustomerId { get; set; }



        public int DoctorID { get; set; }




  
    }
    public class CreateReservationCommandHandler
    : IRequestHandler<CreateReservationCommand, PaymentInitiationResult>
    {
        private readonly IAppDbContext _context;
        private readonly IPaymentGateway _paymentGateway;
        private readonly ICurrentUserService currentUserService;
        private readonly IStringLocalizer<SharedResource> localizer;

        public CreateReservationCommandHandler(
            IAppDbContext context,
            IPaymentGateway paymentGateway,
            ICurrentUserService currentUserService,
            IStringLocalizer<SharedResource> localizer)
        {
            _context = context;
            _paymentGateway = paymentGateway;
           this.currentUserService = currentUserService;
            this.localizer = localizer;
        }

        public async Task<PaymentInitiationResult> Handle(
            CreateReservationCommand request,
            CancellationToken cancellationToken)
        {


            var DoctorClinic = await _context.doctorClinics
                .Include(a=>a.Offer)
                .Where(a => a.DoctorId == request.DoctorID 
                && a.ClinicId == currentUserService.ClinicId)
                .FirstOrDefaultAsync();


            var offerPercent = DoctorClinic.Offer != null
               ? (request.Date >= DoctorClinic.Offer.StartAt 
               && request.Date <= DoctorClinic.Offer.EndAt
               && DoctorClinic.Offer.IsActive)
               ? DoctorClinic.Offer.Percentage
               : 0
               : 0;


            var Customer = _context.Customers.Where(a => a.Id == request.CustomerId ).FirstOrDefault();

            if (Customer is null)
                throw new ItemNotFoundException(localizer["NotFound"]);

            var Amount = DoctorClinic.SessionPrice - (DoctorClinic.SessionPrice * offerPercent) / 100;

 

            var reservation = new Domain.Entities.Reservation
            {
                ClinicId = currentUserService.ClinicId.Value,
                CustomerId = request.CustomerId,
                DoctorID = request.DoctorID,
                OfferId = offerPercent >0 ?null :DoctorClinic.OfferId,
                Status = ReservationStatus.PendingPayment,
                TotalBeforeDiscount=DoctorClinic.SessionPrice,
                TotalAfterDiscount=Amount,
                Type=ReservationType.New,
                

            };

            await _context.Reservations.AddAsync(reservation);

            await _context.SaveChangesAsync();


     

            // 3. Create local payment transaction
            var payment = new PaymentTransaction
            {
                Reference = Guid.NewGuid(),
                Purpose = request.Purpose,
                Status = PaymentStatus.Pending,
                Amount = Amount,
                Currency = "EGP",
                ClinicId = currentUserService.ClinicId.Value,
                ReservationId = reservation.Id,
                SubscriptionId = null,
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
                    CustomerName = Customer.EnName,
                    CustomerEmail = Customer.Email,
                    CustomerPhone = Customer.PhoneNumber
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
