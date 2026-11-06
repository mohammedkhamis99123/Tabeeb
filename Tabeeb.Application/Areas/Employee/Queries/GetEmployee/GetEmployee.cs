using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Text;
using Tabeeb.Application.Common.Exceptions;
using Tabeeb.Application.Common.Services;
using Tabeeb.Resources;

namespace Tabeeb.Application.Areas.Employee.Queries.GetEmployee
{
    public record GetEmployee(int Id) : IRequest<EmployeeDto>;

    public class GetEmployeeQueryHandler
        (IAppDbContext appDbContext,
        IStringLocalizer<SharedResource> localizer)
        : IRequestHandler<GetEmployee, EmployeeDto>
    {
        public async  Task<EmployeeDto> Handle(GetEmployee request, CancellationToken cancellationToken)
        {
            var item = appDbContext.Employees
                .Include(a=>a.AppUser)
                .FirstOrDefault(a => a.Id == request.Id);
            if (item is null)
                throw new ItemNotFoundException(localizer["NotFound"]);

            var data = new EmployeeDto
            {
                Addrees = item.Addrees,
                ArName = item.ArName,
                DoctorId = item.DoctorId,
                Email = item.AppUser.Email,
                EnName = item.EnName,
                IdentityNumber = item.IdentityNumber,
                ImagePath = item.ImagePath,
                IsActive = item.IsActive,
            };

            return data;


        }
    }
}
