using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Tabeeb.Application.Areas.Clinic.Queries.ClinicsDropDown;
using Tabeeb.Application.Common.GenericReponse;
using Tabeeb.Application.Common.Services;

namespace Tabeeb.Application.Areas.Employee.Queries.EmployeeDropDown
{
    public  class EmployeeDropDown: IRequest<List<GenericDropDownDto>>
    {
        public bool? IsActive { get; set; }

    }

    public class EmployeeDropDownHandler
  (IAppDbContext appDbContext)
  : IRequestHandler<EmployeeDropDown, List<GenericDropDownDto>>
    {
        public async Task<List<GenericDropDownDto>> Handle(EmployeeDropDown request, CancellationToken cancellationToken)
        {
            return await appDbContext.Employees
                 .Where(a => !request.IsActive.HasValue || a.IsActive == request.IsActive)
                 .ProjectToType<GenericDropDownDto>()
                 .ToListAsync();
        }
    }
}
