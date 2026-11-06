using Mapster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using Tabeeb.Application.Common.Extensions;
using Tabeeb.Application.Common.Pagination;
using Tabeeb.Application.Common.Services;

namespace Tabeeb.Application.Areas.SupscriptionPackage.Queries.GetSubscriptionPackages
{
    public  class GetSubscriptionPackages : GenericPaginationQurery<PaginatedList<SubscriptionPackageDto>>
    {
     public bool? IsActive {  get; set; }


    }



    public class GetSubscriptionPackagesCommandHandler
        (IAppDbContext appDbContext)
        : IRequestHandler<GetSubscriptionPackages, PaginatedList<SubscriptionPackageDto>>
    {
        public async Task<PaginatedList<SubscriptionPackageDto>> Handle(GetSubscriptionPackages request, CancellationToken cancellationToken)
        {
            return await appDbContext.SubscriptionPackages
                  .Where(a => request.SearchTerm.Trim() == ""
                  || a.ArName.Contains(request.SearchTerm)
                  || a.EnName.Contains(request.SearchTerm))
                  .Where(a => !request.IsActive.HasValue || a.IsActive == request.IsActive)
                  .OrderData(request.SortBy, request.SortOrder)
                  .ProjectToType<SubscriptionPackageDto>()
                  .PaginateAsync(request.Page, request.PageSize);
        }
    }


}
