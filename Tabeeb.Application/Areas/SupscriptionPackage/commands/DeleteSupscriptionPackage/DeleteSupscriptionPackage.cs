using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Text;
using Tabeeb.Application.Common.Exceptions;
using Tabeeb.Application.Common.Services;
using Tabeeb.Resources;

namespace Tabeeb.Application.Areas.SupscriptionPackage.commands.DeleteSupscriptionPackage
{
    public record DeleteSupscriptionPackage(byte Id) : IRequest;


    public class DeleteSupscriptionPackageHandler
        (IAppDbContext appDbContext,
        IStringLocalizer<SharedResource> localizer,
        ICurrentUserService currentUser)
        : IRequestHandler<DeleteSupscriptionPackage>
    {
        public async Task Handle(DeleteSupscriptionPackage request, CancellationToken cancellationToken)
        {
            var entity = await appDbContext.SubscriptionPackages
                                            .FindAsync(request.Id);

            if (entity is null)
                throw new ItemNotFoundException(localizer["NotFound"]);

            if(await IsRelatedToElement(request.Id))
                throw new Exception(localizer["DeleteError"]);




            await appDbContext.SubscriptionPackages.Where(a => a.Id == request.Id)
                .ExecuteUpdateAsync(opt => opt.SetProperty(x => x.DeletedAt, DateTime.UtcNow)
                .SetProperty(x => x.DeletedBy, currentUser.UserId)
                );
            



        }


        private async Task<bool> IsRelatedToElement(int PackageId)
        {
            string query = @$"
            SELECT CASE
            WHEN EXISTS (

                SELECT 1 FROM Subscription WHERE PackageId  = @PackageId 
              
               
               
            )
                THEN 1
                ELSE 0
            End AS IsRelatedToElement
        ";

            await using var command = appDbContext.Database.GetDbConnection().CreateCommand();
            command.CommandText = query;
            command.CommandType = System.Data.CommandType.Text;

            var parameter = new SqlParameter("@PackageId", PackageId);
            command.Parameters.Add(parameter);

            await appDbContext.Database.OpenConnectionAsync();
            var result = await command.ExecuteScalarAsync();

            int isRelated = (result == DBNull.Value) ? 0 : Convert.ToInt32(result);

            return isRelated == 1;
        }
    }


}
