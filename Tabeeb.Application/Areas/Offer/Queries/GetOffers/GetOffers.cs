using Mapster;
using MediatR;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Text;
using Tabeeb.Application.Common.Extensions;
using Tabeeb.Application.Common.Pagination;
using Tabeeb.Application.Common.Services;
using Tabeeb.Domain.Entities;
using Tabeeb.Resources;

namespace Tabeeb.Application.Areas.Offer.Queries.GetOffers
{
    public  class GetOffers : GenericPaginationQurery<PaginatedList<OfferDto>>
    {
       public DateTime? From { get; set; }
        public bool? IsActive {  get; set; }

        public DateTime? To {  get; set; }

    }

    public class GetOffersQueryHandler
        (IAppDbContext appDbContext,
        IStringLocalizer<SharedResource> localizer)
        : IRequestHandler<GetOffers, PaginatedList<OfferDto>>
    {
        public async Task<PaginatedList<OfferDto>> Handle(GetOffers request, CancellationToken cancellationToken)
        {
            return await appDbContext.Offers
                         .Where(a => !request.From.HasValue || a.StartAt >= request.From)
                         .Where(a => !request.To.HasValue || a.EndAt <= request.To)
                         .Where(A => !request.IsActive.HasValue || A.IsActive == request.IsActive)
                         .OrderData(request.SortBy, request.SortOrder)
                         .ProjectToType<OfferDto>()
                         .PaginateAsync(request.Page, request.PageSize);




        }
    }



}
