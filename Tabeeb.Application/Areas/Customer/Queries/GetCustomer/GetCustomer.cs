using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using Tabeeb.Application.Areas.Customer.Queries.GetCustomers;
using Tabeeb.Application.Common.Exceptions;
using Tabeeb.Application.Common.Services;
using Tabeeb.Resources;

namespace Tabeeb.Application.Areas.Customer.Queries.GetCustomer
{
    public record GetCustomer(long Id) : IRequest<CustomerDto>;

    public class GetCustomerHandler
        (IAppDbContext appDbContext,
        IStringLocalizer<SharedResource> localizer,
        ICurrentUserService currentUserService)
        : IRequestHandler<GetCustomer, CustomerDto>
    {
        public async Task<CustomerDto> Handle(GetCustomer request, CancellationToken cancellationToken)
        {
            var customer = await appDbContext.Customers
                                     .Include(a=>a.ClinicCustomer)
                                     .FirstOrDefaultAsync(a=>a.Id==request.Id);

            if (customer is null)
                throw new ItemNotFoundException(localizer["ItemNotFound"]);

            return new CustomerDto
            {
                Id = customer.Id,
                ArName = customer.ArName,
                EnName = customer.EnName,
                Address = customer.Address,
                Email = customer.Email,
                PhoneNumber = customer.PhoneNumber,
                IdentityNumber = customer.IdentityNumber,
                UserId = customer.UserId,

                Medicalfile = customer.ClinicCustomer
               .Where(c => c.ClinicId == currentUserService.ClinicId)
               .Select(c => c.MedicalFile)
               .FirstOrDefault()
            };
        }
    }

}
