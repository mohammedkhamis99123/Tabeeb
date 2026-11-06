using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Localization;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Text;
using Tabeeb.Application.Common.Exceptions;
using Tabeeb.Application.Common.Services;
using Tabeeb.Resources;

namespace Tabeeb.Application.Areas.Employee.Commands.DeleteEmployee
{
    public record DeleteEmployee(int Id) : IRequest;

    public class DeleteEmployeeCommandHandler
        (IAppDbContext appDbContext,
        IStringLocalizer<SharedResource> localizer)
        : IRequestHandler<DeleteEmployee>
    {
        public async Task Handle(DeleteEmployee request, CancellationToken cancellationToken)
        {
            var entity = await appDbContext.Employees
                 .FindAsync(request.Id);

            if (entity is null)
                throw new ItemNotFoundException(localizer["NotFound"]);



            appDbContext.Employees.Remove(entity);
            await appDbContext.SaveChangesAsync();
                                
        }


    }

}
