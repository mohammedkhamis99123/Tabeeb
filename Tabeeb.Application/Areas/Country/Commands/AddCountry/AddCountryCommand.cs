using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Text;
using Tabeeb.Application.Common.Services;
using Tabeeb.Domain.Entities;

namespace Tabeeb.Application.Areas.Country.Commands.AddCountry
{
    public  class AddCountryCommand:IRequest
    {
        public string ArName { get; set; } = null!;

        public string EnName { get; set; } = null!;


        public string ISOCode { get; set; } = null!;
    }

    public class AddCountryCommandHandler
        (IAppDbContext appDbContext)
        : IRequestHandler<AddCountryCommand>
    {
        public async Task Handle(AddCountryCommand request, CancellationToken cancellationToken)
        {
            var data = request.Adapt<Domain.Entities.Country>();
            await  appDbContext.Countries.AddAsync(data);
            await appDbContext.SaveChangesAsync();
        }
    }
}
