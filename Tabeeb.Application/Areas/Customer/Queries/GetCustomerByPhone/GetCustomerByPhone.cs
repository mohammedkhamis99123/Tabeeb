using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Text;
using Tabeeb.Application.Areas.Customer.Queries.GetCustomers;
using Tabeeb.Application.Common.Exceptions;
using Tabeeb.Application.Common.Services;
using Tabeeb.Resources;

namespace Tabeeb.Application.Areas.Customer.Queries.GetCustomerByPhone
{
    public record GetCustomerByPhone(string Phone) : IRequest<CustomerDto>;

    public class GetCustomerByPhoneHandler
      (IAppDbContext appDbContext,
      IStringLocalizer<SharedResource> localizer,
      ICurrentUserService currentUserService)
      : IRequestHandler<GetCustomerByPhone, CustomerDto>
    {
        public async Task<CustomerDto> Handle(GetCustomerByPhone request, CancellationToken cancellationToken)
        {
            var customer = await appDbContext.Customers
                                     .Include(a => a.AppUser)
                                     .FirstOrDefaultAsync(a => a.PhoneNumber == request.Phone);

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
