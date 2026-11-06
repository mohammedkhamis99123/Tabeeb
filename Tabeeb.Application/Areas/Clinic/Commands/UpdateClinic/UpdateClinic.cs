using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Text;
using Tabeeb.Application.Areas.Clinic.Commands.AddClinic;
using Tabeeb.Application.Common.Exceptions;
using Tabeeb.Application.Common.Services;
using Tabeeb.Resources;

namespace Tabeeb.Application.Areas.Clinic.Commands.UpdateClinic
{
    public class UpdateClinic:IRequest

    {
        public int Id { get; set; }
        public string ArName { get; set; } = null!;

        public string EnName { get; set; } = null!;


        public string StreetArName { get; set; } = null!;

        public string StreetEnName { get; set; } = null!;

        public string Langitude { get; set; } = null!;


        public string Latitude { get; set; } = null!;

        public int CityId { get; set; }


        public string? ArDescription { get; set; }

        public string? EnDescription { get; set; }

        public string OwnerArName { get; set; } = null!;

        public string OwnerEnName { get; set; } = null!;

        public string OwnerEmail { get; set; }

        public string OwnerPassword { get; set; }
        public string? ProfileAr { get; set; }

        public string SnapshatUser { get; set; }

        public string InstgramUser { get; set; } = null!;

        public string WhatsappNumber { get; set; }
        public string? ProfileEn { get; set; }

        public bool IsActive { get; set; }

        public bool ActivateOnlineBooking { get; set; }

    }

    public class AUpdateClinicHandler(IAppDbContext appDbContext,
        IStringLocalizer<SharedResource> localizer
        ) : IRequestHandler<UpdateClinic>
    {
        public async Task Handle(UpdateClinic request, CancellationToken cancellationToken)
        {

            var entity = await appDbContext.Clinics
                                     .AsNoTracking()
                                    .FirstOrDefaultAsync(a=>a.Id==request.Id);

            if (entity is null)
                throw new ItemNotFoundException(localizer["NotFound"]);

            var data = request.Adapt<Domain.Entities.Clinic>();
            appDbContext.Clinics.Update(data);
            await  appDbContext.SaveChangesAsync();
        } 
    }
}
