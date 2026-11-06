using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Text;
using Tabeeb.Application.Common.Services;
using Tabeeb.Domain.Entities;
using Tabeeb.Resources;

namespace Tabeeb.Application.Areas.DoctorWorkTime.Queries.GetDoctorWorkTime
{
    public record GetDoctorWorkTime(int DoctorID, int ClinicID) : IRequest<List<WorkTimeDto>>;


    public class GetDoctorWorkTimeHandler
        (IAppDbContext appDbContext,
        IStringLocalizer<SharedResource> localizer)
        : IRequestHandler<GetDoctorWorkTime, List<WorkTimeDto>>
    {
        public async Task<List<WorkTimeDto>> Handle(GetDoctorWorkTime request, CancellationToken cancellationToken)
        {
            return await appDbContext.DoctorWorkTimes
                  .Where(a => a.ClinicId == request.ClinicID && a.DoctorID == request.DoctorID)
                  .ProjectToType<WorkTimeDto>()
                  .ToListAsync();
        }
    }
}
