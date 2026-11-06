using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Tabeeb.Application.Areas.City.Queries.GetCities;
using Tabeeb.Application.Common.Extensions;
using Tabeeb.Application.Common.Pagination;
using Tabeeb.Application.Common.Services;

namespace Tabeeb.Application.Areas.Country.Queries.GetCountries
{
    public class GetCountries : GenericPaginationQurery<PaginatedList<CountryDto>>
    {

    }
    public class GetCountriesHandler(IAppDbContext appDbContext) : IRequestHandler<GetCountries, PaginatedList<CountryDto>>
    {
        public Task<PaginatedList<CountryDto>> Handle(GetCountries request, CancellationToken cancellationToken)
        {
            return appDbContext.Countries
                    .Include(a => a.Cities)
                   .Where(a => request.SearchTerm.Trim() == ""
                   || a.ArName.Contains(request.SearchTerm)
                   || a.EnName.Contains(request.SearchTerm))
                   .ProjectToType<CountryDto>()
                   .OrderData(request.SortBy, request.SortOrder)
                   .PaginateAsync(request.Page, request.PageSize)

                   ;



        }
    }

}
