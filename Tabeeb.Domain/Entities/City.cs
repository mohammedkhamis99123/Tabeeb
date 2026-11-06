using Tabeeb.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tabeeb.Domain.Entities
{
    public  class City:BaseAuditableEntity<int>
    {
        public string ArName { get; set; } = null!;

        public string EnName {  get; set; } = null!;

        public byte CountryId {  get; set; }


        public Country Country { get; set; } = null!;

        public bool IsActive {  get; set; }
        public virtual ICollection<Clinic> Clinics { get; set; } = new List<Clinic>();
    }
}
