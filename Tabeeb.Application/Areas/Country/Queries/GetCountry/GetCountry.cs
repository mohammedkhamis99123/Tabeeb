using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Tabeeb.Application.Common.Pagination;
using Tabeeb.Application.Common.Services;

namespace Tabeeb.Application.Areas.Country.Queries.GetCountry
{
    public record GetCountry(int Id):IRequest<CountryDto>;


    public class GetCountriesHandler(IAppDbContext appDbContext) : IRequestHandler<GetCountry,CountryDto>
    {
        public async Task<CountryDto> Handle(GetCountry request, CancellationToken cancellationToken)
        {
           var data=  await appDbContext.Countries
                      .Include(A=>A.Cities)
                      .FirstOrDefaultAsync(a=>a.Id==request.Id);

            var result = data.Adapt<CountryDto>();

            return result;



                
                   ;



        }
    }


}
