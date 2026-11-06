using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Tabeeb.Application.Areas.City.Queries;
using Tabeeb.Application.Common.GenericReponse;
using Tabeeb.Application.Common.Services;

namespace Tabeeb.Application.Areas.Clinic.Queries.ClinicsDropDown
{
    public  class ClinicsDropDown: IRequest<List<GenericDropDownDto>>
    {
        public bool? IsActive {  get; set; }

    }
    public class ClinicsDropDownHandler
    (IAppDbContext appDbContext)
    : IRequestHandler<ClinicsDropDown, List<GenericDropDownDto>>
    {
        public async Task<List<GenericDropDownDto>> Handle(ClinicsDropDown request, CancellationToken cancellationToken)
        {
            return await appDbContext.Clinics
                 .Where(a=>!request.IsActive.HasValue||a.IsActive==request.IsActive)
                 .ProjectToType<GenericDropDownDto>()
                 .ToListAsync();
        }
    }
}
