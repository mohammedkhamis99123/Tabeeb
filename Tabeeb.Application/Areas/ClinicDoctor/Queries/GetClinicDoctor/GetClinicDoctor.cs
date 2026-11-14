using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Text;
using Tabeeb.Application.Areas.ClinicDoctor.Command;
using Tabeeb.Application.Common.Exceptions;
using Tabeeb.Application.Common.Services;
using Tabeeb.Resources;

namespace Tabeeb.Application.Areas.ClinicDoctor.Queries.GetClinicDoctor
{
    public record GetClinicDoctor(int DoctorId,int ClinicId) : IRequest<ClinicDoctorDto>;

    public class GetDoctorQueryQueryHandler 
        (IAppDbContext appDbContext,
        IStringLocalizer<SharedResource> localizer)
        : IRequestHandler<GetClinicDoctor, ClinicDoctorDto>
    {
        public async Task<ClinicDoctorDto> Handle(GetClinicDoctor request, CancellationToken cancellationToken)
        {
            var doctor = await appDbContext.doctorClinics
                .Include(A=>A.Doctor)
                  .ThenInclude(a=>a.AppUser)
                .Include(A => A.Doctor)
                  .ThenInclude(a => a.Specialization)
                .FirstOrDefaultAsync(a => a.DoctorId == request.DoctorId && a.ClinicId==request.ClinicId);

            if (doctor == null)
                throw new ItemNotFoundException(localizer["NotFound"]);


            return new ClinicDoctorDto
            {
                ArName = doctor.Doctor.ArName,
                ClinicId = request.ClinicId,
                DoctorId = request.DoctorId,
                DoctorStamp = doctor.DoctorStamp,
                Email = doctor.Doctor.AppUser.Email,
                EnName = doctor.Doctor.EnName,
                IdentityNumber = doctor.Doctor.IdentityNumber,
                ImagePath = doctor.ImagePath,
                IsActive = doctor.IsActive,
                JoinDate = doctor.JoinDate,
                SessionDuration = doctor.SessionDuration,
                SessionPrice = doctor.SessionPrice,
                SpecializationArName = doctor.Doctor.Specialization.ArName,
                SpecializationEnName = doctor.Doctor.Specialization.EnName,
                SpecializationId = doctor.Doctor.Specialization.Id,

            };
        }
    }

}
