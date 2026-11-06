using Mapster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using Tabeeb.Application.Areas.City.Queries.GetCities;
using Tabeeb.Application.Areas.Country.Queries;
using Tabeeb.Application.Areas.Country.Queries.GetCountry;
using Tabeeb.Application.Common.Services;

namespace Tabeeb.Application.Areas.City.Queries.GetCity
{
    public record GetCity(int Id) : IRequest<CityDto>;
    public class GetCityHandler(IAppDbContext appDbContext) : IRequestHandler<GetCity, CityDto>
    {
        public async Task<CityDto> Handle(GetCity request, CancellationToken cancellationToken)
        {
            var data = await appDbContext.Cities
                       .FindAsync(request.Id);

            var result = data.Adapt<CityDto>();

            return result;



        }
    }

}