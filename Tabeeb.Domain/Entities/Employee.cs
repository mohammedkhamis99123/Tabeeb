using Tabeeb.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tabeeb.Domain.Entities
{
    public  class Employee:BaseAuditableEntity<int>
    {
        public string ArName { get; set; } = null!;

        public string EnName { get; set; } = null!;



        public string Addrees { get; set; } = null!;


        public bool IsActive { get; set; }

        public int? DoctorId {  get; set; }

        public int ClinicId {  get; set; }

        public string? ImagePath { get; set; }

        public string IdentityNumber { get; set; } = null!;

        public string UserId { get; set; } = null!;
        public AppUser AppUser { get; set; } = null!;
        public Clinic Clinics { get; set; } = null!;

        public Doctor? Doctor { get; set; }





    }
}
