using Mapster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using Tabeeb.Application.Common.Services;
using Tabeeb.Domain.Entities;

namespace Tabeeb.Application.Areas.City.Commands.CreateCity
{
    public  class CreateCityCommand:IRequest
    {
        public string ArName { get; set; } = null!;

        public string EnName { get; set; } = null!;

        public byte CountryId { get; set; }

        public bool IsActive { get; set; }
    }

    public class CreateCityCommandHandler(IAppDbContext appDbContext) : IRequestHandler<CreateCityCommand>
    {
        public async Task Handle(CreateCityCommand request, CancellationToken cancellationToken)
        {
            var data=request.Adapt<Domain.Entities.City>();
            await appDbContext.Cities.AddAsync(data);
            await appDbContext.SaveChangesAsync();

        }
    }
}
