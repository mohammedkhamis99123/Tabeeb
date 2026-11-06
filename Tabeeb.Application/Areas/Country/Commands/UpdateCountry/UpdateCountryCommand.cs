using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Text;
using Tabeeb.Application.Areas.Country.Commands.AddCountry;
using Tabeeb.Application.Common.Exceptions;
using Tabeeb.Application.Common.Services;
using Tabeeb.Resources;

namespace Tabeeb.Application.Areas.Country.Commands.UpdateCountry
{
    public class UpdateCountryCommand : IRequest
    {
        public byte Id { get; set; }
        public string ArName { get; set; } = null!;

        public string EnName { get; set; } = null!;


        public string ISOCode { get; set; } = null!;
    }
    public class UpdateCountryCommandHandler
      (IAppDbContext appDbContext,
        IStringLocalizer<SharedResource> localizer)
      : IRequestHandler<UpdateCountryCommand>
    {
        public async Task Handle(UpdateCountryCommand request, CancellationToken cancellationToken)
        {
            var entity = await appDbContext.Countries
                                     .AsNoTracking()
                                    .FirstOrDefaultAsync(a=>a.Id==request.Id);

            if (entity is null)
                throw new ItemNotFoundException(localizer["NotFound"]);

            var data = request.Adapt<Domain.Entities.Country>();
            appDbContext.Countries.Update(data);
            await appDbContext.SaveChangesAsync();
        }

    }


}
