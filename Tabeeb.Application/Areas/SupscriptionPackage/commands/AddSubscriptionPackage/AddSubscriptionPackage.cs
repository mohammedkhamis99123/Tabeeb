using Mapster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using Tabeeb.Application.Common.Services;

namespace Tabeeb.Application.Areas.SupscriptionPackage.commands.AddSubscriptionPackage
{
    public class AddSubscriptionPackage:IRequest
    {
        public string ArName { get; set; } = null!;

        public string EnName { get; set; } = null!;

        public string ArDescription { get; set; } = null!;

        public string EnDescription { get; set; } = null!;
        public int DurationInDays { get; set; }

        public bool IsActive { get; set; }

        public decimal Price {  get; set; }
    }

    public class AddSubscriptionPackageCommandHandler
        (IAppDbContext appDbContext)
        : IRequestHandler<AddSubscriptionPackage>
    {
        public async Task Handle(AddSubscriptionPackage request, CancellationToken cancellationToken)
        {

            var data = request.Adapt<Domain.Entities.SubscriptionPackage>();

            appDbContext.SubscriptionPackages.Add(data);

            await appDbContext.SaveChangesAsync();
        }
    }
}
