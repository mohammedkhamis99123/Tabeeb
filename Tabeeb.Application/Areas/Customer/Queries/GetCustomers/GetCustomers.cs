using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Tabeeb.Application.Common.Extensions;
using Tabeeb.Application.Common.Pagination;
using Tabeeb.Application.Common.Services;

namespace Tabeeb.Application.Areas.Customer.Queries.GetCustomers
{
    public  class GetCustomers:GenericPaginationQurery<PaginatedList<CustomerDto>>
    {
    }

    public class GetCustomersHandler
        (IAppDbContext appDbContext,
        ICurrentUserService currentUserService)
        : IRequestHandler<GetCustomers, PaginatedList<CustomerDto>>
    {
        public async Task<PaginatedList<CustomerDto>> Handle(GetCustomers request, CancellationToken cancellationToken)
        {
            return await appDbContext.Customers
       .Where(a =>
           (request.SearchTerm.Trim() == "" ||
            a.ArName.Contains(request.SearchTerm) ||
            a.EnName.Contains(request.SearchTerm) ||
            a.IdentityNumber.Contains(request.SearchTerm) ||
            a.Email.Contains(request.SearchTerm)))
 
       .Select(a => new CustomerDto
       {
           Id = a.Id,
           ArName = a.ArName,
           EnName = a.EnName,
           Address = a.Address,
           Email = a.Email,
           PhoneNumber = a.PhoneNumber,
           IdentityNumber = a.IdentityNumber,
           UserId = a.UserId,

           Medicalfile = a.ClinicCustomer
               .Where(c => c.ClinicId == currentUserService.ClinicId)
               .Select(c => c.MedicalFile)
               .FirstOrDefault()
       })
       .OrderData(request.SortBy, request.SortOrder)
       .PaginateAsync(request.Page, request.PageSize);

        }
    }
}
