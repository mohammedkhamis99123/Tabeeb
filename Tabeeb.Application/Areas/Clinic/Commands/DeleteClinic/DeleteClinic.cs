using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Text;
using Tabeeb.Application.Areas.City.Commands.DeleteCity;
using Tabeeb.Application.Common.Services;
using Tabeeb.Resources;

namespace Tabeeb.Application.Areas.Clinic.Commands.DeleteClinic
{
    public record DeleteClinic(int id):IRequest;
    public class DeleteClinicHandler(IAppDbContext db
       , IStringLocalizer<SharedResource> localizer,
       ICurrentUserService currentUser) : IRequestHandler<DeleteClinic>
    {
        public async Task Handle(DeleteClinic request, CancellationToken cancellationToken)
        {
            if (await IsRelatedToElement(request.id))
                throw new Exception(localizer["DeleteError"]);


            await db.Clinics.Where(a => a.Id == request.id)
                .ExecuteUpdateAsync(opt => opt.SetProperty(x => x.DeletedAt, DateTime.UtcNow)
                .SetProperty(x => x.DeletedBy, currentUser.UserId)
                );


        }



        private async Task<bool> IsRelatedToElement(int ClinicId)
        {
            string query = @$"
            SELECT CASE
            WHEN EXISTS (

                SELECT 1 FROM DoctorClinic WHERE Clinicid  = @ClinicId 
                union
               SELECT 1 FROM ClinicCustomer WHERE Clinicid  = @ClinicId  
                 union
               SELECT 1 FROM Reservation WHERE Clinicid  = @ClinicId  and DeletedAt is null
               union
               SELECT 1 FROM Subscription WHERE Clinicid  = @ClinicId 
                 union
               SELECT 1 FROM Offers WHERE Clinicid  = @ClinicId  and DeletedAt is null
                union
               SELECT 1 FROM DoctorWorkTimes WHERE Clinicid  = @ClinicId  and DeletedAt is null
                union
               SELECT 1 FROM Employee WHERE Clinicid  = @ClinicId  and DeletedAt is null
               
               
            )
                THEN 1
                ELSE 0
            End AS IsRelatedToElement
        ";

            await using var command = db.Database.GetDbConnection().CreateCommand();
            command.CommandText = query;
            command.CommandType = System.Data.CommandType.Text;

            var parameter = new SqlParameter("@ClinicId", ClinicId);
            command.Parameters.Add(parameter);

            await db.Database.OpenConnectionAsync();
            var result = await command.ExecuteScalarAsync();

            int isRelated = (result == DBNull.Value) ? 0 : Convert.ToInt32(result);

            return isRelated == 1;
        }

    }
}

