using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Tabeeb.Application.Areas.Country.Queries.CountryDropDown;
using Tabeeb.Application.Common.GenericReponse;
using Tabeeb.Application.Common.Services;

namespace Tabeeb.Application.Areas.City.Queries.CityDropDpwn
{
    public  class CityDropDown:IRequest<List<GenericDropDownDto>>
    {

    }

    public class CityDropDownHandler
       (IAppDbContext appDbContext)
       : IRequestHandler<CityDropDown, List<GenericDropDownDto>>
    {
        public async Task<List<GenericDropDownDto>> Handle(CityDropDown request, CancellationToken cancellationToken)
        {
            return await appDbContext.Cities
                 .ProjectToType<GenericDropDownDto>()
                 .ToListAsync();
        }
    }

}
