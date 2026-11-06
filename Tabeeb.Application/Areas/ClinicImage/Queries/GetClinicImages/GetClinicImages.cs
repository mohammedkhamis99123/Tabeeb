using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Tabeeb.Application.Common.Services;

namespace Tabeeb.Application.Areas.ClinicImage.Queries.GetClinicImages
{
    public record GetClinicImages(int ClinicId) : IRequest<List<ClinicImageDto>>;


    public class GetClinicImagesHandler
        (IAppDbContext appDbContext)
        : IRequestHandler<GetClinicImages, List<ClinicImageDto>>
    {
        public async  Task<List<ClinicImageDto>> Handle(GetClinicImages request, CancellationToken cancellationToken)
        {
           return   await appDbContext.ClinicImages
                                .Where(a => a.ClinicId == request.ClinicId)
                                .ProjectToType<ClinicImageDto>()
                                .ToListAsync();

        }
    }
}
