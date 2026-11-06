using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient.Diagnostics;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;
using Tabeeb.Application.Common.Services;
using Tabeeb.Domain.Entities;

namespace Tabeeb.Application.Areas.Customer.Commands.AddCustomerCommand
{
    public  class AddCustomerCommand:IRequest
    {
        public string ArName { get; set; } = null!;

        public string EnName { get; set; } = null!;


        public string Address { get; set; } = null!;

        public string Email { get; set; } = null!;


        public string PhoneNumber { get; set; } = null!;

        public string IdentityNumber { get; set; } = null!;

        public long MedicalFile {  get; set; }
    }
    public class AddCustomerCommandHandler
        (IAppDbContext appDbContext,
        UserManager<AppUser> userManager,
        ICurrentUserService currentUserService)
        : IRequestHandler<AddCustomerCommand>
    {
        public async Task Handle(AddCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = await appDbContext.Customers
                                                  .Where(a => a.PhoneNumber == request.PhoneNumber && !a.DeletedAt.HasValue)
                                                  .FirstOrDefaultAsync();
            var customerId = 0l;
            if(customer is null)
            {
                var userID=await AddCustomerUser(request);
                var Element = new Domain.Entities.Customer
                {
                    EnName = request.EnName,
                    Address = request.Address,
                    Email = request.Email,
                    ArName = request.ArName,
                    UserId = userID,
                    PhoneNumber = request.PhoneNumber,
                    IdentityNumber = request.IdentityNumber,


                };
               await  appDbContext.Customers.AddAsync(Element);

                await appDbContext.SaveChangesAsync();
                var customerClinic = new ClinicCustomer
                {
                    ClinicId = currentUserService.ClinicId.Value,
                    CustomerId = Element.Id,
                    MedicalFile = request.MedicalFile,
                };
                await appDbContext.ClinicCustomers.AddAsync(customerClinic);

                customerId = Element.Id;

            }
            else
            {
                var customerClinic = new ClinicCustomer
                {
                    ClinicId = currentUserService.ClinicId.Value,
                    CustomerId = customerId,
                    MedicalFile=request.MedicalFile
                };
                await appDbContext.ClinicCustomers.AddAsync(customerClinic);
            }


             
          
               await  appDbContext.SaveChangesAsync();
           
         
        }

      
        private async Task<string> AddCustomerUser(AddCustomerCommand request)
        {
            var user = new AppUser
            {
                ArName = request.ArName,
                EnName = request.EnName,
                Email = request.Email,
                UserName = string.IsNullOrWhiteSpace(request.EnName)
               ? request.Email
               : request.EnName.Trim().Replace(" ", "_"),
                ClinicId=currentUserService.ClinicId.Value
               
            };
            var password = GenerateRandomPassword(8);


            var userResult = await userManager.CreateAsync(user,password);
            if (!userResult.Succeeded)
            {
                await userManager.AddToRoleAsync(user,"Customer");
                throw new Exception($"Failed to create user for doctor {request.EnName}: " +
                                    string.Join(", ", userResult.Errors.Select(e => e.Description)));
            }

            return user.Id;

        }

        private static string GenerateRandomPassword(int length)
        {
            const string upper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string lower = "abcdefghijklmnopqrstuvwxyz";
            const string numbers = "0123456789";
            const string special = "!@#$%^&*";

            const string all = upper + lower + numbers + special;

            var password = new char[length];

            // نضمن وجود كل نوع مطلوب في الـ password
            password[0] = upper[RandomNumberGenerator.GetInt32(upper.Length)];
            password[1] = lower[RandomNumberGenerator.GetInt32(lower.Length)];
            password[2] = numbers[RandomNumberGenerator.GetInt32(numbers.Length)];
            password[3] = special[RandomNumberGenerator.GetInt32(special.Length)];

            for (int i = 4; i < length; i++)
            {
                password[i] = all[RandomNumberGenerator.GetInt32(all.Length)];
            }

            // Shuffle
            for (int i = password.Length - 1; i > 0; i--)
            {
                int j = RandomNumberGenerator.GetInt32(i + 1);
                (password[i], password[j]) = (password[j], password[i]);
            }

            return new string(password);
        }
    }
}
