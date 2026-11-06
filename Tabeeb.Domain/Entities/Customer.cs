using Tabeeb.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tabeeb.Domain.Entities
{
    public  class Customer:BaseAuditableEntity<long>
    {
        public string ArName { get; set; } = null!;

        public string EnName { get; set; } = null!;


        public string Address { get; set; } = null!;

        public string Email {  get; set; } = null!;


        public string PhoneNumber {  get; set; } = null!;

        public string IdentityNumber { get; set; } = null!;

        public string UserId { get; set; } = null!;
        public AppUser AppUser { get; set; } = null!;
        public virtual ICollection<ClinicCustomer> ClinicCustomer { get; set; }= new List<ClinicCustomer>();

        public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();



    }
}
