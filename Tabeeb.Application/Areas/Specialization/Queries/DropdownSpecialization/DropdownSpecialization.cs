using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Tabeeb.Application.Areas.Employee.Queries.EmployeeDropDown;
using Tabeeb.Application.Common.GenericReponse;
using Tabeeb.Application.Common.Services;

namespace Tabeeb.Application.Areas.Specialization.Queries.DropdownSpecialization
{
    public  class DropdownSpecialization: IRequest<List<GenericDropDownDto>>
    {
    }

public class DropdownSpecializationHandler
(IAppDbContext appDbContext)
: IRequestHandler<DropdownSpecialization, List<GenericDropDownDto>>
{
    public async Task<List<GenericDropDownDto>> Handle(DropdownSpecialization request, CancellationToken cancellationToken)
    {
        return await appDbContext.Specializations
             .ProjectToType<GenericDropDownDto>()
             .ToListAsync();
    }
}
}
