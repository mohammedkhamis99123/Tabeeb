using Tabeeb.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;
using Tabeeb.Domain.Enums;

namespace Tabeeb.Domain.Entities
{
    public  class Clinic:BaseAuditableEntity<int>
    {
        public string ArName { get; set; } = null!;

        public string EnName { get; set; } = null!;


        public string StreetArName {  get; set; } = null!;

        public string StreetEnName { get; set; } = null!;

        public string Langitude { get; set; } = null!;


        public string Latitude {  get; set; } = null!;

        public int CityId { get; set; }


        public City City { get; set; } = null!;


        //public int CountryId { get; set; }
        //public Country Country { get; set; } = new();

        public string? ArDescription {  get; set; }

        public string? EnDescription { get; set; }


        public string? ProfileAr { get; set; }


        public string? ProfileEn { get; set; }

        public string OwnerArName { get; set; } = null!;

        public string OwnerEnName { get; set; } = null!;

        public string OwnerEmail { get; set; }

        public string SnapshatUser {  get; set; }

        public string InstgramUser {  get; set; } = null!;

        public string WhatsappNumber {  get; set; }

        public bool ActivateOnlineBooking { get; set; }


        public virtual ICollection<AppUser> Users { get; set; } = new List<AppUser>();


        public virtual ICollection<ClinicImage> Images { get; set; } = new List<ClinicImage>();

        public virtual ICollection<ClinicCustomer> Customers { get; set; } = new List<ClinicCustomer>();

        public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();

        public virtual ICollection<Subscriptions> Subscriptions { get; set; } = new List<Subscriptions>();

        public virtual ICollection<Offer> Offers { get; set; } = new List<Offer>();

        public virtual ICollection<DoctorClinic> Doctors { get; set; } = new List<DoctorClinic>();

        public virtual ICollection<DoctorWorkTime> WorkTimes { get; set; } = new List<DoctorWorkTime>();

        public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

        public virtual ICollection<RolePermissions> RolePermissions { get; set; } = new List<RolePermissions>();

        public virtual ICollection<SubscriptionInvoices> Invoices { get; set; } = new List<SubscriptionInvoices>();

        public bool IsActive {  get; set; }



    }

}
