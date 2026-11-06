using Tabeeb.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tabeeb.Domain.Entities
{
    public  class Medicine:BaseAuditableEntity<long>
    {
        public string Name { get; set; } = null!;

        public string Dose {  get; set; }= null!;

        public string Notes {  get; set; }= null!;

        public long PrescriptionId {  get; set; }

        public Prescription Prescription { get; set; } = null!;



    }
}
