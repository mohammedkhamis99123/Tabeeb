using Mapster;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Text;
using Tabeeb.Application.Common.Exceptions;
using Tabeeb.Application.Common.Services;
using Tabeeb.Domain.Entities;
using Tabeeb.Domain.Enums;
using Tabeeb.Resources;

namespace Tabeeb.Application.Areas.Employee.Queries.GetEmployee
{
    public record GetEmployee(int Id) : IRequest<EmployeeDto>;

    public class GetEmployeeQueryHandler
        (IAppDbContext appDbContext,
        IStringLocalizer<SharedResource> localizer,
        UserManager<AppUser> userManager,
        RoleManager<AspNetRole> roleManager)
        : IRequestHandler<GetEmployee, EmployeeDto>
    {
        public async  Task<EmployeeDto> Handle(GetEmployee request, CancellationToken cancellationToken)
        {
            var item = appDbContext.Employees
                .Include(a=>a.AppUser)
                .FirstOrDefault(a => a.Id == request.Id);
            if (item is null)
                throw new ItemNotFoundException(localizer["NotFound"]);


            var user = await userManager.FindByIdAsync(item.UserId);
            var roleName =  await userManager.GetRolesAsync(user);

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
                PhoneNumber = item.PhoneNumber,
                Role = roleName[0],
                Salary = item.Salary,
                WorkHours = item.WorkHours
            };

            return data;


        }
    }
}
