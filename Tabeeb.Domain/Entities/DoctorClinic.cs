using System;
using System.Collections.Generic;
using System.Text;

namespace Tabeeb.Domain.Entities
{
    public  class DoctorClinic
    {
        public int ClinicId {  get; set; }

        public int DoctorId {  get; set; }

        public string? ImagePath { get; set; }

        public string? DoctorStamp { get; set; }

        public int SessionDuration { get; set; }

        public decimal SessionPrice {  get; set; }
        public Clinic Clinic { get; set; } = null!;

        public Doctor Doctor { get; set; } = null!;

        public bool IsActive { get; set; }
        public DateTime JoinDate { get; set; }
    }
}
