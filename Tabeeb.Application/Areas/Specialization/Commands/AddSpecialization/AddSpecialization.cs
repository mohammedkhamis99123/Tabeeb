using Mapster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using Tabeeb.Application.Common.Services;

namespace Tabeeb.Application.Areas.Specialization.Commands.AddSpecialization
{
    public  class AddSpecialization:IRequest
    {
        public string ArName { get; set; } = null!;

        public string EnName { get; set; } = null!;

    }

    public class AddSpecializationCommandHandler 
        (IAppDbContext appDbContext)
        : IRequestHandler<AddSpecialization>
    {
        public async  Task Handle(AddSpecialization request, CancellationToken cancellationToken)
        {
           var data= request.Adapt<Domain.Entities.Specialization>();
            await appDbContext.Specializations.AddAsync(data);

            await appDbContext.SaveChangesAsync();
          
        }
    }
}
