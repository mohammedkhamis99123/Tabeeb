using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tabeeb.Domain.Entities
{
    public  class AppUser:IdentityUser
    {
        public string ArName { get; set; } = null!;

        public string EnName {  get; set; } = null!;

        public string? RefreshToken {  get; set; }

        public DateTime? RefreshTokenExpiredAt { get; set; }
    

        public int? ClinicId {  get; set; }

        public Clinic Clinic { get; set; }

       
        public virtual Doctor? Doctor { get; set; }
        public virtual Employee? Employee { get; set; }

        public virtual Customer? Customer { get; set; }


        //City
        public virtual ICollection<City> CityCreatedBy { get; set; }=new HashSet<City>();
        public virtual ICollection<City> CityUpdatedBy { get; set; } = new HashSet<City>();
        public virtual ICollection<City> CityDeletedBy { get; set; } = new HashSet<City>();


        //Country

        public virtual ICollection<Country> CountryCreatedBy { get; set; } = new HashSet<Country>();
        public virtual ICollection<Country> CountryUpdatedBy { get; set; } = new HashSet<Country>();
        public virtual ICollection<Country> CountryDeletedBy { get; set; } = new HashSet<Country>();



        //ClinicImage

        public virtual ICollection<ClinicImage> ClinicImageCreatedBy { get; set; } = new HashSet<ClinicImage>();
        public virtual ICollection<ClinicImage> ClinicImageUpdatedBy { get; set; } = new HashSet<ClinicImage>();
        public virtual ICollection<ClinicImage> ClinicImageDeletedBy { get; set; } = new HashSet<ClinicImage>();



        //Clinic

        public virtual ICollection<Clinic> ClinicCreatedBy { get; set; } = new HashSet<Clinic>();
        public virtual ICollection<Clinic> ClinicUpdatedBy { get; set; } = new HashSet<Clinic>();
        public virtual ICollection<Clinic> ClinicDeletedBy { get; set; } = new HashSet<Clinic>();



        //Customer

        public virtual ICollection<Customer> CustomerCreatedBy { get; set; } = new HashSet<Customer>();
        public virtual ICollection<Customer> CustomerUpdatedBy { get; set; } = new HashSet<Customer>();
        public virtual ICollection<Customer> CustomerDeletedBy { get; set; } = new HashSet<Customer>();



        //Doctor

        public virtual ICollection<Doctor> DoctorCreatedBy { get; set; } = new HashSet<Doctor>();
        public virtual ICollection<Doctor> DoctorUpdatedBy { get; set; } = new HashSet<Doctor>();
        public virtual ICollection<Doctor> DoctorDeletedBy { get; set; } = new HashSet<Doctor>();




        //DoctorWorkTime

        public virtual ICollection<DoctorWorkTime> DoctorWorkTimeCreatedBy { get; set; } = new HashSet<DoctorWorkTime>();
        public virtual ICollection<DoctorWorkTime> DoctorWorkTimeUpdatedBy { get; set; } = new HashSet<DoctorWorkTime>();
        public virtual ICollection<DoctorWorkTime> DoctorWorkTimeDeletedBy { get; set; } = new HashSet<DoctorWorkTime>();



        //Employee

        public virtual ICollection<Employee> EmployeeCreatedBy { get; set; } = new HashSet<Employee>();
        public virtual ICollection<Employee> EmployeeUpdatedBy { get; set; } = new HashSet<Employee>();
        public virtual ICollection<Employee> EmployeeDeletedBy { get; set; } = new HashSet<Employee>();



        //Medicine

        public virtual ICollection<Medicine> MedicineCreatedBy { get; set; } = new HashSet<Medicine>();
        public virtual ICollection<Medicine> MedicineUpdatedBy { get; set; } = new HashSet<Medicine>();
        public virtual ICollection<Medicine> MedicineDeletedBy { get; set; } = new HashSet<Medicine>();



        //Offer

        public virtual ICollection<Offer> OfferCreatedBy { get; set; } = new HashSet<Offer>();
        public virtual ICollection<Offer> OfferUpdatedBy { get; set; } = new HashSet<Offer>();
        public virtual ICollection<Offer> OfferDeletedBy { get; set; } = new HashSet<Offer>();



        //PatientVital

        public virtual ICollection<PatientVital> PatientVitalCreatedBy { get; set; } = new HashSet<PatientVital>();
        public virtual ICollection<PatientVital> PatientVitalUpdatedBy { get; set; } = new HashSet<PatientVital>();
        public virtual ICollection<PatientVital> PatientVitalDeletedBy { get; set; } = new HashSet<PatientVital>();



        //Prescription

        public virtual ICollection<Prescription> PrescriptionCreatedBy { get; set; } = new HashSet<Prescription>();
        public virtual ICollection<Prescription> PrescriptionUpdatedBy { get; set; } = new HashSet<Prescription>();
        public virtual ICollection<Prescription> PrescriptionDeletedBy { get; set; } = new HashSet<Prescription>();


        //Prescription

        public virtual ICollection<Reservation> ReservationCreatedBy { get; set; } = new HashSet<Reservation>();
        public virtual ICollection<Reservation> ReservationUpdatedBy { get; set; } = new HashSet<Reservation>();
        public virtual ICollection<Reservation> ReservationDeletedBy { get; set; } = new HashSet<Reservation>();


        //Prescription

        public virtual ICollection<Specialization> SpecializationCreatedBy { get; set; } = new HashSet<Specialization>();
        public virtual ICollection<Specialization> SpecializationUpdatedBy { get; set; } = new HashSet<Specialization>();
        public virtual ICollection<Specialization> SpecializationDeletedBy { get; set; } = new HashSet<Specialization>();


        //SubscriptionPackage

        public virtual ICollection<SubscriptionPackage> SubscriptionPackageCreatedBy { get; set; } = new HashSet<SubscriptionPackage>();
        public virtual ICollection<SubscriptionPackage> SubscriptionPackageUpdatedBy { get; set; } = new HashSet<SubscriptionPackage>();
        public virtual ICollection<SubscriptionPackage> SubscriptionPackageDeletedBy { get; set; } = new HashSet<SubscriptionPackage>();



        //Subscriptions

        public virtual ICollection<Subscriptions> SubscriptionsCreatedBy { get; set; } = new HashSet<Subscriptions>();
        public virtual ICollection<Subscriptions> SubscriptionsUpdatedBy { get; set; } = new HashSet<Subscriptions>();
        public virtual ICollection<Subscriptions> SubscriptionsDeletedBy { get; set; } = new HashSet<Subscriptions>();


        //SubscriptionInvoice
        public virtual ICollection<SubscriptionInvoices> SubscriptionInvoiceCreatedBy { get; set; } = new HashSet<SubscriptionInvoices>();
        public virtual ICollection<SubscriptionInvoices> SubscriptionInvoiceUpdatedBy { get; set; } = new HashSet<SubscriptionInvoices>();
        public virtual ICollection<SubscriptionInvoices> SubscriptionInvoiceDeletedBy { get; set; } = new HashSet<SubscriptionInvoices>();

        //Evaluation

        public virtual ICollection<Evaluation> EvaluationCreatedBy { get; set; } = new HashSet<Evaluation>();
        public virtual ICollection<Evaluation> EvaluationUpdatedBy { get; set; } = new HashSet<Evaluation>();
        public virtual ICollection<Evaluation> EvaluationDeletedBy { get; set; } = new HashSet<Evaluation>();

    }
}
