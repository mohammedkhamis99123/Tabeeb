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

namespace Tabeeb.Application.Areas.City.Commands.UpdateCity
{
    public class UpdateCityCommand:IRequest
    {
        public int Id {  get; set; }

        public string ArName { get; set; } = null!;

        public string EnName { get; set; } = null!;

        public bool IsActive {  get; set; }

        public byte CountryId {  get; set; }

    }

    public class UpdateCityCommandHandler
        (IAppDbContext appDbContext,
        IStringLocalizer<SharedResource> localizer)
        : IRequestHandler<UpdateCityCommand>
    {
        public async Task Handle(UpdateCityCommand request, CancellationToken cancellationToken)
        {
            var entity = await appDbContext.Cities
                                      .AsNoTracking()
                                     .FirstOrDefaultAsync(a=>a.Id==request.Id);

            if (entity is null)
                throw new ItemNotFoundException(localizer["NotFound"]);

            var data = request.Adapt<Domain.Entities.City>();
            appDbContext.Cities.Update(data);
            await appDbContext.SaveChangesAsync(cancellationToken);

        }
    }

}
