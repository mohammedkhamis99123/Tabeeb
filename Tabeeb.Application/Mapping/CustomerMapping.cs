using Mapster;
using System;
using System.Collections.Generic;
using System.Text;
using Tabeeb.Application.Areas.Country.Queries;
using Tabeeb.Application.Areas.Customer.Queries.GetCustomers;
using Tabeeb.Domain.Entities;

namespace Tabeeb.Application.Mapping
{
    public  class CustomerMapping
    {
        public static void Run()
        {
            TypeAdapterConfig<Customer, CustomerDto>
                .NewConfig()
                .Map(dest => dest.Medicalfile, src => src.ClinicCustomer.FirstOrDefault().MedicalFile);

        }
    }
}
