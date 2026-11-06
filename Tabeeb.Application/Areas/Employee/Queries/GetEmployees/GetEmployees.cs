using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Tabeeb.Application.Common.Extensions;
using Tabeeb.Application.Common.Pagination;
using Tabeeb.Application.Common.Services;

namespace Tabeeb.Application.Areas.Employee.Queries.GetEmployees
{
    public  class GetEmployees:GenericPaginationQurery<PaginatedList<EmployeeDto>>
    {
        public bool? IsActive {  get; set; }
    }

    public class GetEmployeesQueryHandler
        (IAppDbContext appDbContext)
        : IRequestHandler<GetEmployees, PaginatedList<EmployeeDto>>
    {
        public async Task<PaginatedList<EmployeeDto>> Handle(GetEmployees request, CancellationToken cancellationToken)
        {
            var searchTerm = request.SearchTerm?.Trim();
            bool hasSearchTerm = !string.IsNullOrWhiteSpace(searchTerm);

            return await appDbContext.Employees
                .Include(a=>a.AppUser)
                .Where(a => !hasSearchTerm
                    || a.ArName.Contains(searchTerm)
                    || a.EnName.Contains(searchTerm)
                    || a.IdentityNumber.Contains(searchTerm)
                    || (a.AppUser != null && a.AppUser.Email.Contains(searchTerm)))
                .Where(a => !request.IsActive.HasValue || a.IsActive == request.IsActive)
                .ProjectToType<EmployeeDto>()
                .OrderData(request.SortBy, request.SortOrder)
                .PaginateAsync(request.Page, request.PageSize);

        }
    }
}
