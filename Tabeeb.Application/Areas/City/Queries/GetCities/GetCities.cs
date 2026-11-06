using Mapster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using Tabeeb.Application.Common.Pagination;
using Tabeeb.Application.Common.Services;
using Tabeeb.Application.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Tabeeb.Application.Areas.City.Queries.GetCities
{
    public  class GetCities: GenericPaginationQurery<PaginatedList<CityDto>>
    {
        public bool? IsActive {  get; set; }
        public int? CountryId { get; set; }
    }

    public class GetCitiesHandler(IAppDbContext appDbContext) : IRequestHandler<GetCities, PaginatedList<CityDto>>
    {
        public Task<PaginatedList<CityDto>> Handle(GetCities request, CancellationToken cancellationToken)
        {
            return appDbContext.Cities
                   
                   .Where(a => request.SearchTerm.Trim() == ""
                   || a.ArName.Contains(request.SearchTerm)
                   || a.EnName.Contains(request.SearchTerm))
                   .Where(A => !request.IsActive.HasValue || A.IsActive == request.IsActive)
                   .Where(a=> !request.CountryId.HasValue|| a.CountryId==request.CountryId)
                   .ProjectToType<CityDto>()
                   .OrderData(request.SortBy, request.SortOrder)
                   .PaginateAsync(request.Page, request.PageSize)
                   
                   ;

                   

        }
    }


}
