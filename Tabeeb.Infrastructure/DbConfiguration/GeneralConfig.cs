using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Tabeeb.Domain.Entities;

namespace Tabeeb.Infrastructure.DbConfiguration
{
    public  class GeneralConfig
    {
        public static void RunConfig(ModelBuilder modelBuilder)
        {
            CityConfig.Runconfig(modelBuilder);
            CountryConfig.RunConfig(modelBuilder);
            ClinicConfig.RunConfig (modelBuilder);
            ClinicCustomerConfig.RunConfig(modelBuilder);
            ClinicImageConfig.RunConfig(modelBuilder);
            CustomerConfig.RunConfig (modelBuilder);
            DoctorConfig.RunConfig(modelBuilder);
            DoctorWorkTimeConfig.RunConfig(modelBuilder);
            EmployeeConfig.RunConfig(modelBuilder);
            MedicineConfig.RunConfig (modelBuilder);
            OfferConfig.RunConfig(modelBuilder);
            PatientVitalConfig.RunConfig(modelBuilder);
            PermissionConfig.RunConfig(modelBuilder);
            PrescriptionConfig.RunConfig(modelBuilder);
            ReservationConfig.RunConfig(modelBuilder);
            RolePermissionsConfig.RunConfig(modelBuilder);
            SpecializationConfig.RunConfig(modelBuilder);
            SubscriptionPackageConfig.RunConfig(modelBuilder);
            SubscriptionsConfig.RunConfig(modelBuilder);
            EvaluationConfig.RunConfig(modelBuilder);
            ClinicDoctorConfig.RunConfig(modelBuilder);
            SubscriptionInvoiceConfig.RunConfig(modelBuilder);

        }
    }
}
