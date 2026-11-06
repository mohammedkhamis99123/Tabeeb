using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Text;
using Tabeeb.Application.Areas.Country.Commands.DeleteCountry;
using Tabeeb.Application.Common.Services;
using Tabeeb.Resources;

namespace Tabeeb.Application.Areas.Specialization.Commands.DeleteSpecialization
{
    public record DeleteSpecialization(int Id):IRequest;

    public class DeleteSpecializationCommandHandler(IAppDbContext db
      , IStringLocalizer<SharedResource> localizer,
      ICurrentUserService currentUser) : IRequestHandler<DeleteSpecialization>
    {
        public async Task Handle(DeleteSpecialization request, CancellationToken cancellationToken)
        {
            if (await IsRelatedToElement(request.Id))
                throw new Exception(localizer["DeleteError"]);


            await db.Specializations.Where(a => a.Id == request.Id)
                .ExecuteUpdateAsync(opt => opt.SetProperty(x => x.DeletedAt, DateTime.UtcNow)
                .SetProperty(x => x.DeletedBy, currentUser.UserId)
                );


        }



        private async Task<bool> IsRelatedToElement(int SpecializationId)
        {
            string query = @$"
            SELECT CASE
            WHEN EXISTS (

                SELECT 1 FROM Doctor WHERE SpecializationId  = @SpecializationId  and DeletedAt is null
               
               
            )
                THEN 1
                ELSE 0
            End AS IsRelatedToElement
        ";

            await using var command = db.Database.GetDbConnection().CreateCommand();
            command.CommandText = query;
            command.CommandType = System.Data.CommandType.Text;

            var parameter = new SqlParameter("@SpecializationId", SpecializationId);
            command.Parameters.Add(parameter);

            await db.Database.OpenConnectionAsync();
            var result = await command.ExecuteScalarAsync();

            int isRelated = (result == DBNull.Value) ? 0 : Convert.ToInt32(result);

            return isRelated == 1;
        }

    }
    }
