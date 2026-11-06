using Tabeeb.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tabeeb.Domain.Entities
{
    public  class Doctor:BaseAuditableEntity<int>
    {
        public string ArName { get; set; } = null!;

        public string EnName { get; set; } = null!;

        public byte SpecializationId {  get; set; }

  
        public string IdentityNumber { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public AppUser AppUser { get; set; } = null!;
        public Specialization Specialization { get; set; } = null!;

        public virtual ICollection<DoctorWorkTime> DoctorWorkTimes { get; set; } = new List<DoctorWorkTime>();

        public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();

        public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

        public virtual ICollection<Offer> Offers { get; set; } = new List<Offer>();

       

        public virtual ICollection<ClinicCustomer> Customers { get; set; } = new List<ClinicCustomer>();

        public virtual ICollection<DoctorClinic> DoctorClinic { get; set; } = new List<DoctorClinic>();

       

        public bool IsActive { get; set; }


    }
}
