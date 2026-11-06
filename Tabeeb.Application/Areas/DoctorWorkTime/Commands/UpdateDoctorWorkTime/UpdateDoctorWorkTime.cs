using MediatR;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;
using Tabeeb.Application.Common.Services;
using Tabeeb.Domain.Entities;

namespace Tabeeb.Application.Areas.DoctorWorkTime.Commands.UpdateDoctorWorkTime
{
    public  class UpdateDoctorWorkTime:IRequest
    {

        public List<DoctorWorkTimeInfo> WorkTimes {  get; set; }
      

        public int DoctorID { get; set; }

        public int ClinicId { get; set; }


    }

    public class UpdateDoctorWorkTimeHandler 
        (IAppDbContext appDbContext)
        : IRequestHandler<UpdateDoctorWorkTime>
    {
        public async Task Handle(UpdateDoctorWorkTime request, CancellationToken cancellationToken)
        {
           var entities= appDbContext.DoctorWorkTimes
                .Where(a => a.DoctorID == request.DoctorID && a.ClinicId == request.ClinicId);

            var data = request.WorkTimes
                .Select(a=>a.DayOfWeek)
                .ToHashSet();

            var toDelete = entities.Where(a =>! data.Contains(a.DayOfWeek));



            List<Domain.Entities.DoctorWorkTime> Times = new List<Domain.Entities.DoctorWorkTime>();
            foreach (var item in request.WorkTimes)
            {
                var record = entities.FirstOrDefault(a => a.DayOfWeek == item.DayOfWeek);
                if (record != null)
                {
                    record.StartAt = item.StartAt;
                    record.EndAt = item.EndAt;
                    record.IsActive = item.IsActive;
                    record.DayOfWeek = item.DayOfWeek;

                }
                else
                {
                    var Time = new Domain.Entities.DoctorWorkTime
                    {
                        StartAt = item.StartAt,
                        EndAt = item.EndAt,
                        IsActive = item.IsActive,
                        DoctorID = request.DoctorID,
                        ClinicId = request.ClinicId,
                        DayOfWeek = item.DayOfWeek,
                    };
                    Times.Add(Time);

                }
                    

            }

            await appDbContext.DoctorWorkTimes.AddRangeAsync(Times);
            appDbContext.DoctorWorkTimes.RemoveRange(toDelete);

            await appDbContext.SaveChangesAsync();


           

         
                
        }
    }
}
