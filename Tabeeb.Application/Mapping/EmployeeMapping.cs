using Mapster;
using System;
using System.Collections.Generic;
using System.Text;
using Tabeeb.Application.Areas.Clinic.Queries;
using Tabeeb.Application.Areas.Employee.Queries;
using Tabeeb.Domain.Entities;

namespace Tabeeb.Application.Mapping
{
    public  class EmployeeMapping
    {
        public static void Run()
        {
            TypeAdapterConfig<Employee, EmployeeDto>
                .NewConfig()
                .Map(dest => dest.Email, src => src.AppUser.Email);
             

        }
    }
}
