using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Tabeeb.Application.Areas.City.Queries.GetCities;
using Tabeeb.Application.Areas.City.Queries.GetCity;
using Tabeeb.Application.Common.Services;

namespace Tabeeb.Application.Areas.Clinic.Queries.GetClinic
{
    public record GetClinic(int Id) : IRequest<ClinicDto>;

    public class GetClinicHandler(IAppDbContext appDbContext) : IRequestHandler<GetClinic, ClinicDto>
    {
        public async Task<ClinicDto> Handle(GetClinic request, CancellationToken cancellationToken)
        {
            var data = await appDbContext.Clinics
                                        .Include(a=>a.Images)
                                        .Include(a=>a.Subscriptions)
                                        .ThenInclude(a=>a.Package)
                       .FirstOrDefaultAsync(a=>a.Id==request.Id);

            var result = data.Adapt<ClinicDto>();

            return result;



        }
    }

 }
