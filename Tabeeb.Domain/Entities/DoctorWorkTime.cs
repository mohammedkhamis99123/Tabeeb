using Tabeeb.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tabeeb.Domain.Entities
{
    public  class DoctorWorkTime:BaseAuditableEntity<int>
    {
        public DayOfWeek DayOfWeek { get; set; }

        public TimeOnly? StartAt { get; set; }


        public TimeOnly? EndAt { get; set; }

        public bool IsActive {  get; set; }

        public int DoctorID {  get; set; }

        public int ClinicId {  get; set; }

        public Doctor Doctor { get; set; } = null!;

        public Clinic clinic {  get; set; } = null!;



    }
}
