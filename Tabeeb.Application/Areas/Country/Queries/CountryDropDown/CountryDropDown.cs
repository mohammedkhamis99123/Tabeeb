using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Tabeeb.Application.Common.GenericReponse;
using Tabeeb.Application.Common.Services;

namespace Tabeeb.Application.Areas.Country.Queries.CountryDropDown
{
    public class CountryDropDown : IRequest<List<GenericDropDownDto>>
    {

    }


    public class CountryDropDownHandler
        (IAppDbContext appDbContext)
        : IRequestHandler<CountryDropDown, List<GenericDropDownDto>>
    {
        public async Task<List<GenericDropDownDto>> Handle(CountryDropDown request, CancellationToken cancellationToken)
        {
            return await appDbContext.Countries
                 .Include(a => a.Cities)
                 .ProjectToType<GenericDropDownDto>()
                 .ToListAsync();
        }
    }



}
