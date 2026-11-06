using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Tabeeb.Application.Areas.ClinicDoctor.Command;
using Tabeeb.Application.Common.Services;
using Tabeeb.Domain.Entities;

namespace Tabeeb.Application.Areas.ClinicDoctor.Queries.GetClinicDoctors
{
    public record GetClinicDoctors(int ClinicId) : IRequest<List<ClinicDoctorDto>>;

    public class GetClinicDoctorsQueryHandler(IAppDbContext appDbContext)
            : IRequestHandler<GetClinicDoctors, List<ClinicDoctorDto>>
    {
        public async Task<List<ClinicDoctorDto>> Handle(GetClinicDoctors request, CancellationToken cancellationToken)
        {
            // جلب أطباء العيادة المحددة مع بيانات الـ AppUser والـ WorkTimes بطلب SQL واحد عالي الأداء (AsNoTracking)
            var doctors = await appDbContext.doctorClinics
                .IgnoreQueryFilters()
                .AsNoTracking()
                .Where(dc => dc.ClinicId == request.ClinicId)
                .Select(dc => new ClinicDoctorDto
                {
                    DoctorId = dc.DoctorId,
                    ClinicId = dc.ClinicId,
                    ArName = dc.Doctor.ArName,
                    EnName = dc.Doctor.EnName,
                    IdentityNumber = dc.Doctor.IdentityNumber,
                    IsActive = dc.IsActive,
                    SpecializationId = dc.Doctor.SpecializationId,
                    SessionPrice = dc.SessionPrice,
                    SessionDuration = dc.SessionDuration,
                    DoctorStamp = dc.DoctorStamp,
                    ImagePath = dc.ImagePath,
                    JoinDate = dc.JoinDate,
                    // جلب أوقات عمل الطبيب لهذه العيادة تحديداً
                    worktimeDtos = appDbContext.DoctorWorkTimes
                        .AsNoTracking()
                        .Where(wt => wt.DoctorID == dc.DoctorId && wt.ClinicId == dc.ClinicId)
                        .Select(wt => new ClinicDoctorWorktimeDto
                        {
                            DayOfWeek = wt.DayOfWeek,
                            StartAt = wt.StartAt,
                            EndAt = wt.EndAt
                        }).ToList()
                })
                .ToListAsync(cancellationToken);

            // جلب الإيميلات بشكل دفعة واحدة لتقليل استعلامات الـ DB (إذا كان الـ AppUser منفصلاً عبر UserId)
            var doctorUserIds = await appDbContext.Doctors
               .IgnoreQueryFilters()
                .AsNoTracking()
                .Where(d => doctors.Select(x => x.DoctorId).Contains(d.Id))
                .Select(d => new { d.Id, d.UserId })
                .ToListAsync(cancellationToken);

            var userIds = doctorUserIds.Select(x => x.UserId).ToList();
            var users = await appDbContext.Users
                .AsNoTracking()
                .Where(u => userIds.Contains(u.Id))
                .ToDictionaryAsync(u => u.Id, u => u.Email, cancellationToken);

            foreach (var doc in doctors)
            {
                var userId = doctorUserIds.FirstOrDefault(x => x.Id == doc.DoctorId)?.UserId;
                if (!string.IsNullOrEmpty(userId) && users.TryGetValue(userId, out var email))
                {
                    doc.Email = email ?? string.Empty;
                }
            }

            return doctors;
        }
    }
}