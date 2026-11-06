using Mapster;
using MediatR;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Text;
using Tabeeb.Application.Common.Services;
using Tabeeb.Domain.Entities;
using Tabeeb.Resources;

namespace Tabeeb.Application.Areas.Offer.Commands.AddOffer
{
    public  class AddOffer:IRequest
    {
        public DateTime StartAt { get; set; }

        public DateTime EndAt { get; set; }

        public decimal Percentage { get; set; }


        public bool IsActive { get; set; }

        public int DoctorId { get; set; }


    }

    public class AddOfferCommandHandler
        (IAppDbContext appDbContext,
        IStringLocalizer<SharedResource> localizer,
        ICurrentUserService currentUserService)
        : IRequestHandler<AddOffer>
    {
        public async Task Handle(AddOffer request, CancellationToken cancellationToken)
        {
            var data = request.Adapt<Domain.Entities.Offer>();
            data.ClinicId = currentUserService.ClinicId.Value;

            await appDbContext.Offers.AddAsync(data);

            await appDbContext.SaveChangesAsync(cancellationToken);
        }
    }



}
