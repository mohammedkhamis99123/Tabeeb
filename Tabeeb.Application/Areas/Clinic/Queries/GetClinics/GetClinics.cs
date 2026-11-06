using AutoMapper.QueryableExtensions;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Tabeeb.Application.Common.Extensions;
using Tabeeb.Application.Common.Pagination;
using Tabeeb.Application.Common.Services;

namespace Tabeeb.Application.Areas.Clinic.Queries.GetClinics
{
    public  class GetClinics: GenericPaginationQurery<PaginatedList<ClinicDto>>
    {
        public bool? IsActive {  get; set; }
        public int? CityId {  get; set; }

        public byte? SubscriptionPackageId { get; set; }

        public DateOnly? From { get; set; }

        public DateOnly? To { get; set; }
    }


    public class GetClinicsHandler 
        (IAppDbContext appDbContext)
        : IRequestHandler<GetClinics, PaginatedList<ClinicDto>>
    {
        public async  Task<PaginatedList<ClinicDto>> Handle(GetClinics request, CancellationToken cancellationToken)
        {
            return await appDbContext.Clinics
                .IgnoreQueryFilters()
                .Include(a=>a.City)
                  .ThenInclude(a=>a.Country)
                .Include(A=>A.Doctors)
                .Include(a => a.Images)
                .Include(a => a.Subscriptions)
                  .ThenInclude(a => a.Package)
                .Where(a => request.SearchTerm.Trim() == ""
                || request.SearchTerm.Contains(a.ArName)
                || request.SearchTerm.Contains(a.EnName)
                ||request.SearchTerm.Contains(a.StreetArName)
                || request.SearchTerm.Contains(a.StreetEnName)
                )
                .Where(a=>!request.CityId.HasValue || a.CityId==request.CityId)
                .Where(A => !request.IsActive.HasValue || A.IsActive == request.IsActive)
                .Where(a=>!request.SubscriptionPackageId.HasValue 
                || a.Subscriptions.OrderByDescending(a=>a.StartAt).Select(a=>a.PackageID).FirstOrDefault()==request.SubscriptionPackageId)
                .Where(a=>!request.From.HasValue || a.Subscriptions.OrderByDescending(a => a.StartAt).Select(a => a.EndAt).FirstOrDefault() >= request.From)
                .Where(a => !request.To.HasValue || a.Subscriptions.OrderByDescending(a => a.StartAt).Select(a => a.EndAt).FirstOrDefault() <= request.To)
                .ProjectToType<ClinicDto>()
                .OrderData(request.SortBy, request.SortOrder)
                .PaginateAsync(request.Page, request.PageSize);
        }
    }
}
