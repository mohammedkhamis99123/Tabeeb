using Tabeeb.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tabeeb.Domain.Entities
{
    public  class Prescription:BaseAuditableEntity<long>
    {
        public DateTime Date{  get; set; }

        public long ReservationId {  get; set; }

        public Reservation Reservation { get; set; } = new();

        public string Diagnosis { get; set; } = null!;

        public string Notes {  get; set; }= null!;


        public virtual ICollection<Medicine> Medicines { get; set; } = new List<Medicine>();
    }
}
