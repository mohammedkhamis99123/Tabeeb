using Tabeeb.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tabeeb.Domain.Entities
{
    public  class Country:BaseAuditableEntity<byte>
    {
        public string ArName { get; set; } = null!;

        public string EnName { get; set; }= null!;


        public string ISOCode {  get; set; }= null!;

        public virtual ICollection<City> Cities { get; set; } = new List<City>();
        //public virtual ICollection<Clinics> Clinics { get; set; } = new List<Clinics>();

    }
}
