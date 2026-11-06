using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Tabeeb.Application.Areas.Country.Queries.CountryDropDown;
using Tabeeb.Application.Common.GenericReponse;
using Tabeeb.Application.Common.Services;

namespace Tabeeb.Application.Areas.SupscriptionPackage.Queries.DropdownSubscriptionPackages
{
    public class DropdownSubscriptionPackages : IRequest<List<GenericDropDownDto>>
    {

    }


    public class DropdownSubscriptionPackagesHandler
        (IAppDbContext appDbContext)
        : IRequestHandler<DropdownSubscriptionPackages, List<GenericDropDownDto>>
    {
        public async Task<List<GenericDropDownDto>> Handle(DropdownSubscriptionPackages request, CancellationToken cancellationToken)
        {
            return await appDbContext.SubscriptionPackages
                 .ProjectToType<GenericDropDownDto>()
                 .ToListAsync();
        }
    }
}