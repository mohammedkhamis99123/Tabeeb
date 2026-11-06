using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Text;
using Tabeeb.Application.Areas.Customer.Commands.AddCustomerCommand;
using Tabeeb.Application.Common.Exceptions;
using Tabeeb.Application.Common.Services;
using Tabeeb.Domain.Entities;
using Tabeeb.Resources;

namespace Tabeeb.Application.Areas.Customer.Commands.UpdateCustomerCommand
{
    public class UpdateCustomerCommand : IRequest
    {
        public long CustomerId {  get; set; }
     public long MedicalFile {  get; set; }
    }
    public class UpdateCustomerCommandHandler
        (IAppDbContext appDbContext,
        UserManager<AppUser> userManager,
        ICurrentUserService currentUserService,
        IStringLocalizer<SharedResource> localizer)
        : IRequestHandler<UpdateCustomerCommand>
    {
        public async Task Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = await appDbContext.Customers  
                                                  .FindAsync(request.CustomerId);

            if (customer == null)
                throw new ItemNotFoundException(localizer["NotFound"]);

            var data = appDbContext.ClinicCustomers
                              .Where(a => a.ClinicId == currentUserService.ClinicId && a.CustomerId==customer.Id)
                               .FirstOrDefault();

            if(data is null)
                throw new ItemNotFoundException(localizer["NotFound"]);


            data.MedicalFile = request.MedicalFile;


            await appDbContext.SaveChangesAsync();


        }


    }
}