using Mapster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using Tabeeb.Application.Areas.Employee.Queries;
using Tabeeb.Application.Areas.Employee.Queries.GetEmployees;
using Tabeeb.Application.Common.Extensions;
using Tabeeb.Application.Common.Pagination;
using Tabeeb.Application.Common.Services;

namespace Tabeeb.Application.Areas.Specialization.Queries.GetSpecializations
{
    public class GetSpecializations : GenericPaginationQurery<PaginatedList<SpecializationDto>>;

    public class GetSpecializationsHandler
    (IAppDbContext appDbContext)
    : IRequestHandler<GetSpecializations, PaginatedList<SpecializationDto>>
    {
        public async Task<PaginatedList<SpecializationDto>> Handle(GetSpecializations request, CancellationToken cancellationToken)
        {
            var searchTerm = request.SearchTerm?.Trim();
            bool hasSearchTerm = !string.IsNullOrWhiteSpace(searchTerm);

            return await appDbContext.Specializations
                .Where(a => !hasSearchTerm
                    || a.ArName.Contains(searchTerm)
                    || a.EnName.Contains(searchTerm)
                   )
                .ProjectToType<SpecializationDto>()
                .OrderData(request.SortBy, request.SortOrder)
                .PaginateAsync(request.Page, request.PageSize);

        }
    }

}
