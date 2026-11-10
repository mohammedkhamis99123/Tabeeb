using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Tabeeb.Application.Areas.Subscription.Queries.GetSubscriptions;
using Tabeeb.Application.Common.Services;

namespace Tabeeb.Application.Areas.Subscription.Queries.GetSubscriptionHistory
{
    public record GetSubscriptionHistory(int ClinicId) : IRequest<List<SubscriptionDto>>;

    public class GetSubscriptionHistoryHandler
        (IAppDbContext appDbContext)
        : IRequestHandler<GetSubscriptionHistory, List<SubscriptionDto>>
    {
        public async Task<List<SubscriptionDto>> Handle(GetSubscriptionHistory request, CancellationToken cancellationToken)
        {
            return await appDbContext.Subscriptions
                                     .Where(a => a.ClinicId == request.ClinicId)
                                     .OrderBy(a => a.Id)
                                     .Select(a => new SubscriptionDto
                                     {
                                         ClinicArName = a.Clinics.ArName,
                                         ClinicEnName = a.Clinics.EnName,
                                         PackageArName = a.Package.ArName,
                                         PackageEnName = a.Package.EnName,
                                         Status = a.Status,
                                         StartAt = a.StartAt.Value,
                                         EndAt = a.EndAt.Value,
                                     }).ToListAsync(cancellationToken);
        }
    }


}
