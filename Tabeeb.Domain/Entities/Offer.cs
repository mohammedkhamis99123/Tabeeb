using Tabeeb.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Tabeeb.Domain.Entities
{
    public  class Offer:BaseAuditableEntity<int>
    {
        
        public DateTime StartAt { get; set; }

        public DateTime EndAt { get; set; }

        public decimal Percentage {  get; set; }


        public bool IsActive { get; set; }
        
        public int DoctorId {  get; set; }

        public Doctor Doctor { get; set; } = null!;

        public int ClinicId { get; set; }

        public Clinic Clinic { get; set; } = null!;


        public IEnumerable<Reservation> Reservations { get; set; }


    }
}
