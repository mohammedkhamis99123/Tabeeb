using Tabeeb.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tabeeb.Domain.Entities
{
    public  class Specialization:BaseAuditableEntity<byte>
    {
        public string ArName { get; set; } = null!;

        public string EnName { get; set; } = null!;


        public virtual ICollection<Doctor> Doctors { get; set; } = new List<Doctor>();

    }
}
