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

namespace Tabeeb.Application.Areas.Specialization.Commands.UpdateSpecialization
{
    public  class UpdateSpecialization:IRequest
    {
        public byte Id { get; set; }
        public string ArName { get; set; } = null!;

        public string EnName { get; set; } = null!;
    }
    public class UpdateSpecializationCommandHandler
       (IAppDbContext appDbContext,
        IStringLocalizer<SharedResource> localizer)
       : IRequestHandler<UpdateSpecialization>
    {
        public async Task Handle(UpdateSpecialization request, CancellationToken cancellationToken)
        {
            var entity = await appDbContext.Specializations
                                    .AsNoTracking()
                                   .FirstOrDefaultAsync(a=>a.Id==request.Id);
            if (entity == null)
                throw new ItemNotFoundException(localizer["NotFound"]);
                             
            var data = request.Adapt<Domain.Entities.Specialization>();
             appDbContext.Specializations.Update(data);

            await appDbContext.SaveChangesAsync();

        }
    }
}
