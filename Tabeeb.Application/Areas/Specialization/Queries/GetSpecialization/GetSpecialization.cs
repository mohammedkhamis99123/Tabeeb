using MediatR;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Text;
using Tabeeb.Application.Areas.Employee.Queries;
using Tabeeb.Application.Areas.Employee.Queries.GetEmployee;
using Tabeeb.Application.Areas.Specialization.Queries.GetSpecializations;
using Tabeeb.Application.Common.Exceptions;
using Tabeeb.Application.Common.Services;
using Tabeeb.Resources;

namespace Tabeeb.Application.Areas.Specialization.Queries.GetSpecialization
{
    public record GetSpecialization(int Id) : IRequest<SpecializationDto>;

    public class GetSpecializationQueryHandler
       (IAppDbContext appDbContext,
       IStringLocalizer<SharedResource> localizer)
       : IRequestHandler<GetSpecialization, SpecializationDto>
    {
        public async Task<SpecializationDto> Handle(GetSpecialization request, CancellationToken cancellationToken)
        {
            var item = appDbContext.Specializations
                .FirstOrDefault(a => a.Id == request.Id);
            if (item is null)
                throw new ItemNotFoundException(localizer["NotFound"]);

            var data = new SpecializationDto
            {
                Id = request.Id,
                ArName = item.ArName,
                EnName = item.EnName,

            };

            return data;

        }

    }
}

