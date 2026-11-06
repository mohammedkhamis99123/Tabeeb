using Mapster;
using MediatR;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Text;
using Tabeeb.Application.Areas.SupscriptionPackage.Queries.GetSubscriptionPackages;
using Tabeeb.Application.Common.Exceptions;
using Tabeeb.Application.Common.Services;
using Tabeeb.Resources;

namespace Tabeeb.Application.Areas.SupscriptionPackage.Queries.GetSubscriptionPackage
{
    public record GetSubscriptionPackage(byte Id) : IRequest<SubscriptionPackageDto>;


    public class GetSubscriptionPackageHandler

        (IAppDbContext appDbContext,
        IStringLocalizer<SharedResource> localizer)
        : IRequestHandler<GetSubscriptionPackage, SubscriptionPackageDto>
    {
        public async  Task<SubscriptionPackageDto> Handle(GetSubscriptionPackage request, CancellationToken cancellationToken)
        {
            var entity = await appDbContext.SubscriptionPackages
                                              .FindAsync(request.Id);

            if (entity is null)
                throw new ItemNotFoundException(localizer["NotFound"]);


            return entity.Adapt<SubscriptionPackageDto>();

          
        }
    }


}
