using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using Tabeeb.Application.Common.Services;
using Tabeeb.Domain.Entities;

namespace Tabeeb.Application.Areas.Employee.Commands.AddEmployee
{
    public  class AddEmployeeCommand:IRequest
    {
        public string ArName { get; set; } = null!;

        public string EnName { get; set; } = null!;



        public string Addrees { get; set; } = null!;


        public bool IsActive { get; set; }

        public int? DoctorId { get; set; }


        public IFormFile Image {  get; set; }
        public string IdentityNumber { get; set; } = null!;

        public string Email {  get; set; } = null!;

        public string Role { get; set; }


        public string Password { get; set; }


     
    }

    public class AddEmployeeCommandHandler
        (IFileService fileService,
        IAppDbContext appDbContext,
        UserManager<AppUser> userManager,
        ICurrentUserService currentUser,
        RoleManager<AspNetRole> roleManager)
        : IRequestHandler<AddEmployeeCommand>
    {
        public async Task Handle(AddEmployeeCommand request, CancellationToken cancellationToken)
        {
            var userID=await AddEmployeeUser(request);

            var data = request.Adapt<Domain.Entities.Employee>();
            data.ImagePath = fileService.UploadFile(request.Image, "Employees");
            data.UserId = userID;
            data.ClinicId = currentUser.ClinicId.Value;

           appDbContext.Employees.Add(data);
           await appDbContext.SaveChangesAsync();

                
        }

        private async Task<string> AddEmployeeUser(AddEmployeeCommand request)
        {
            var user = new AppUser
            {
                ArName = request.ArName,
                EnName = request.EnName,
                Email = request.Email,
                UserName = string.IsNullOrWhiteSpace(request.EnName)
                   ? request.Email
                   : request.EnName.Trim().Replace(" ", "_"),
                ClinicId=currentUser.ClinicId.Value
            };

            var userResult = await userManager.CreateAsync(user,request.Password);

            var role = await roleManager.FindByIdAsync(request.Role);
            await userManager.AddToRoleAsync(user, role.Name);
            if (!userResult.Succeeded)
            {

                throw new Exception($"Failed to create user for doctor {request.EnName}: " +
                                    string.Join(", ", userResult.Errors.Select(e => e.Description)));
            }

            return user.Id;

        }
    }
}
