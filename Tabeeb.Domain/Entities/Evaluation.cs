using System;
using System.Collections.Generic;
using System.Text;
using Tabeeb.Domain.Common;

namespace Tabeeb.Domain.Entities
{
    public  class Evaluation:BaseAuditableEntity<long>
    {
        public long ReservationId {  get; set; }


        public Reservation Reservation { get; set; } = null!;


        public int Rate {  get; set; }
    }
    
}
