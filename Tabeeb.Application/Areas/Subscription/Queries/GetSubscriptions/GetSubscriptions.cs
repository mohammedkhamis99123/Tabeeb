using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Tabeeb.Application.Areas.Employee.Queries;
using Tabeeb.Application.Common.Extensions;
using Tabeeb.Application.Common.Pagination;
using Tabeeb.Application.Common.Services;
using Tabeeb.Domain.Enums;

namespace Tabeeb.Application.Areas.Subscription.Queries.GetSubscriptions
{
    public class GetSubscriptions : GenericPaginationQurery<PaginatedList<SubscriptionDto>>
    {
        public int? ClinicId { get; set; }

        public byte? PackageId { get; set; }

        public SubscriptionStatus? Status { get; set; }
    }

    public class GetSubscriptionsQueryHandler
        (IAppDbContext appDbContext)
        : IRequestHandler<GetSubscriptions, PaginatedList<SubscriptionDto>>
    {
        public async Task<PaginatedList<SubscriptionDto>> Handle(GetSubscriptions request, CancellationToken cancellationToken)
        {
            // البدء من جدول العيادات لضمان الترجمة السليمة لـ SQL وتجنب مشاكل الـ GroupBy المعقدة
            var query = appDbContext.Clinics
                .Where(c => c.DeletedAt == null)
                .Where(c => !request.ClinicId.HasValue || c.Id == request.ClinicId)
                .Select(c => new
                {
                    Clinic = c,
                    // استخراج أحدث اشتراك لكل عيادة بناءً على الـ ID الأكبر
                    LatestSubscription = c.Subscriptions
                        .OrderByDescending(s => s.Id)
                        .FirstOrDefault()
                })
                .Where(x => x.LatestSubscription != null) // التأكد أن العيادة تمتلك اشتراكاً
                .Where(x => !request.PackageId.HasValue || x.LatestSubscription.PackageID == request.PackageId)
                .Where(x => !request.Status.HasValue || x.LatestSubscription.Status == request.Status)
                .Select(x => new SubscriptionDto
                {
                    ClinicId = x.Clinic.Id,
                    ClinicArName = x.Clinic.ArName,
                    ClinicEnName = x.Clinic.EnName,
                    PackageID = x.LatestSubscription.PackageID,
                    PackageArName = x.LatestSubscription.Package.ArName,
                    PackageEnName = x.LatestSubscription.Package.EnName,
                    Status = x.LatestSubscription.Status,
                    StartAt = x.LatestSubscription.StartAt,
                    EndAt = x.LatestSubscription.EndAt,
                    Id=x.LatestSubscription.Id
                });

            // تطبيق الترتيب (Sorting) والـ Pagination بأمان وسلاسة
            return await query
                .OrderData(request.SortBy, request.SortOrder)
                .PaginateAsync(request.Page, request.PageSize);
        }
    }
}