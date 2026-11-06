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

namespace Tabeeb.Application.Areas.SupscriptionPackage.commands.UpdateSubscriptionPackage
{
    public  class UpdateSubscriptionPackage:IRequest
    {
        public byte Id { get; set; }
        public string ArName { get; set; } = null!;

        public string EnName { get; set; } = null!;

        public string ArDescription { get; set; } = null!;

        public string EnDescription { get; set; } = null!;
        public int DurationInDays { get; set; }

        public bool IsActive { get; set; }

        public decimal Price { get; set; }
    }


    public class UpdateSubscriptionPackageCommandHandler
        (IAppDbContext appDbContext,
        IStringLocalizer<SharedResource> localizer)
        : IRequestHandler<UpdateSubscriptionPackage>
    {
        public async Task Handle(UpdateSubscriptionPackage request, CancellationToken cancellationToken)
        {
            var entity = await appDbContext.SubscriptionPackages
                                     .AsNoTracking()
                                     .FirstOrDefaultAsync(a=>a.Id==request.Id);

            if (entity is null)
                throw new ItemNotFoundException(localizer["NotFound"]);

            var data=request.Adapt<Domain.Entities.SubscriptionPackage>();

            appDbContext.SubscriptionPackages.Update(data);
            await appDbContext.SaveChangesAsync();




                
        }
    }
}
