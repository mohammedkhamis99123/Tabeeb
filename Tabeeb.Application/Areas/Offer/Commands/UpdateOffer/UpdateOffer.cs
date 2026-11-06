using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Text;
using Tabeeb.Application.Common.Exceptions;
using Tabeeb.Application.Common.Services;
using Tabeeb.Resources;

namespace Tabeeb.Application.Areas.Offer.Commands.UpdateOffer
{
    public  class UpdateOffer:IRequest
    {
        public int Id { get; set; }

        public DateTime StartAt { get; set; }

        public DateTime EndAt { get; set; }

        public decimal Percentage { get; set; }


        public bool IsActive { get; set; }

        public int DoctorId { get; set; }
    }

    public class UpdateOfferCommandHandler
      (IAppDbContext appDbContext,
      IStringLocalizer<SharedResource> localizer,
      ICurrentUserService currentUserService)
      : IRequestHandler<UpdateOffer>
    {
        public async Task Handle(UpdateOffer request, CancellationToken cancellationToken)
        {
            var entity = await appDbContext.Offers
                                    .AsNoTracking()
                                    .FirstOrDefaultAsync(a=>a.Id==request.Id);

            if (entity is null)
                throw new ItemNotFoundException(localizer["NotFound"]);

            var data = request.Adapt<Domain.Entities.Offer>();
            data.ClinicId = currentUserService.ClinicId.Value;

            appDbContext.Offers.Update(data);

            await appDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
