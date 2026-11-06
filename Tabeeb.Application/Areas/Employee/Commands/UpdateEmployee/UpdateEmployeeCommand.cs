using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Text;
using Tabeeb.Application.Common.Exceptions;
using Tabeeb.Application.Common.Services;
using Tabeeb.Domain.Entities;
using Tabeeb.Resources;

namespace Tabeeb.Application.Areas.Employee.Commands.UpdateEmployee
{
    public  class UpdateEmployeeCommand:IRequest
    {
        public int Id {  get; set; }
        public string ArName { get; set; } = null!;

        public string EnName { get; set; } = null!;



        public string Addrees { get; set; } = null!;


        public bool IsActive { get; set; }

        public int? DoctorId { get; set; }

       
        public IFormFile? Image { get; set; }
        public string IdentityNumber { get; set; } = null!;

        public string Email { get; set; } = null!;


        public string Role { get; set; }
    }
    public class UpdateEmployeeCommandHandler
        (IAppDbContext appDbContext,
        IFileService fileService,
        UserManager<AppUser> userManager,
        IStringLocalizer<SharedResource> localizer,
        ICurrentUserService currentUser,
        RoleManager<AspNetRole> roleManager)
        : IRequestHandler<UpdateEmployeeCommand>
    {
        public async Task Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
        {
            var item = appDbContext.Employees
                .AsNoTracking()
                .FirstOrDefault(a => a.Id == request.Id);

            if (item is null)
                throw new ItemNotFoundException(localizer["NotFound"]);

             await UpdateEmployeeUser(request);


            var data = request.Adapt<Domain.Entities.Employee>();
            if(request.Image is not null)
            {
                fileService.DeleteFile(item.ImagePath);
                data.ImagePath = fileService.UploadFile(request.Image, "Employees");

            }
            data.ClinicId = currentUser.ClinicId.Value;
            data.UserId = item.UserId;
            appDbContext.Employees.Update(data);
            await appDbContext.SaveChangesAsync();
        }

        private async Task UpdateEmployeeUser(UpdateEmployeeCommand request)
        {
            if (string.IsNullOrEmpty(request.Email))
                return;

            var user = await userManager.FindByEmailAsync(request.Email);

            var role = await roleManager.Roles.Where(a => a.Id == request.Role).FirstOrDefaultAsync();

            var currentRoles = await userManager.GetRolesAsync(user);
            if (currentRoles.Count > 0)
                await userManager.RemoveFromRolesAsync(user, currentRoles);

            await userManager.AddToRoleAsync(user, role.Name);


           
            if (user != null)
            {
                user.ArName = request.ArName;
                user.EnName = request.EnName;
                user.UserName = request.EnName.Trim().Replace(" ", "_");
                user.Email = request.Email;
                user.ClinicId = currentUser.ClinicId;
                await userManager.UpdateAsync(user);
            }
        }
    }
}
