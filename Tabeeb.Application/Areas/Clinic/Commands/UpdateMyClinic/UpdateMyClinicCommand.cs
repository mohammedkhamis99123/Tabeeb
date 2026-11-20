using Mapster;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Tabeeb.Application.Common.Exceptions;
using Tabeeb.Application.Common.Services;
using Tabeeb.Domain.Entities;
using Tabeeb.Resources;

namespace Tabeeb.Application.Areas.Clinic.Commands.UpdateMyClinic
{
    public class UpdateMyClinicCommand : IRequest
    {
        public string ArName { get; set; } = null!;
        public string EnName { get; set; } = null!;
        public string StreetArName { get; set; } = null!;
        public string StreetEnName { get; set; } = null!;
        public string Langitude { get; set; } = null!;
        public string Latitude { get; set; } = null!;
        public int CityId { get; set; }
        public string? ArDescription { get; set; }
        public string? EnDescription { get; set; }
        public string? ProfileAr { get; set; }
        public string? ProfileEn { get; set; }
        public string OwnerArName { get; set; } = null!;
        public string OwnerEnName { get; set; } = null!;
        public string OwnerEmail { get; set; } = null!;
        public string? SnapshatUser { get; set; }
        public string? InstgramUser { get; set; }
        public string? WhatsappNumber { get; set; }
    }

    public class UpdateMyClinicCommandHandler(
        IAppDbContext appDbContext,
        UserManager<AppUser> userManager,
        ICurrentUserService currentUserService,
        IStringLocalizer<SharedResource> localizer)
        : IRequestHandler<UpdateMyClinicCommand>
    {
        public async Task Handle(UpdateMyClinicCommand request, CancellationToken cancellationToken)
        {
            var clinicId = currentUserService.ClinicId;
            if (!clinicId.HasValue)
                throw new ItemNotFoundException(localizer["NotFound"]);

            // 1. جلب العيادة وهي متعقبة (Tracked) للحفاظ على باقي الأعمدة غير الموجودة في الـ Command
            var entity = await appDbContext.Clinics
                .FirstOrDefaultAsync(a => a.Id == clinicId.Value, cancellationToken);

            if (entity is null)
                throw new ItemNotFoundException(localizer["NotFound"]);

            var previousOwnerEmail = entity.OwnerEmail;

            // 2. تحديث بيانات الكيان الحالي مباشرة دون تصفير الحقول الأخرى
            request.Adapt(entity);

            // 3. تحديث بيانات مالك العيادة بأمان
            if (!string.IsNullOrWhiteSpace(previousOwnerEmail))
            {
                var owner = await userManager.FindByEmailAsync(previousOwnerEmail);
                if (owner is not null)
                {
                    await UpdateOwner(owner, request);
                }
            }

            await appDbContext.SaveChangesAsync(cancellationToken);
        }

        private async Task UpdateOwner(AppUser owner, UpdateMyClinicCommand command)
        {
            owner.ArName = command.OwnerArName;
            owner.EnName = command.OwnerEnName;

            if (!string.Equals(owner.Email, command.OwnerEmail, StringComparison.OrdinalIgnoreCase))
            {
                var setEmailResult = await userManager.SetEmailAsync(owner, command.OwnerEmail);
                if (!setEmailResult.Succeeded)
                {
                    throw new Exception(string.Join(", ", setEmailResult.Errors.Select(e => e.Description)));
                }
            }

            var updateResult = await userManager.UpdateAsync(owner);
            if (!updateResult.Succeeded)
            {
                throw new Exception(string.Join(", ", updateResult.Errors.Select(e => e.Description)));
            }
        }
    }
}