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

namespace Tabeeb.Application.Areas.Country.Commands.DeleteCountry
{
    public record DeleteCountry(int Id) : IRequest;

    public class DeleteCountryCommandHandler(IAppDbContext db
      , IStringLocalizer<SharedResource> localizer,
      ICurrentUserService currentUser) : IRequestHandler<DeleteCountry>
    {
        public async Task Handle(DeleteCountry request, CancellationToken cancellationToken)
        {
            if (await IsRelatedToElement(request.Id))
                throw new Exception(localizer["DeleteError"]);


            await db.Countries.Where(a => a.Id == request.Id)
                .ExecuteUpdateAsync(opt => opt.SetProperty(x => x.DeletedAt, DateTime.UtcNow)
                .SetProperty(x => x.DeletedBy, currentUser.UserId)
                );


        }



        private async Task<bool> IsRelatedToElement(int countryId)
        {
            string query = @$"
            SELECT CASE
            WHEN EXISTS (

                SELECT 1 FROM City WHERE countryId  = @countryId  and DeletedAt is null
               
               
            )
                THEN 1
                ELSE 0
            End AS IsRelatedToElement
        ";

            await using var command = db.Database.GetDbConnection().CreateCommand();
            command.CommandText = query;
            command.CommandType = System.Data.CommandType.Text;

            var parameter = new SqlParameter("@countryId", countryId);
            command.Parameters.Add(parameter);

            await db.Database.OpenConnectionAsync();
            var result = await command.ExecuteScalarAsync();

            int isRelated = (result == DBNull.Value) ? 0 : Convert.ToInt32(result);

            return isRelated == 1;
        }
    }

}
