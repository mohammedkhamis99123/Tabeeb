using System;
using System.Collections.Generic;
using System.Text;

namespace Tabeeb.Application.Areas.ClinicDoctor.Command
{
    public  class ClinicDoctorWorktimeDto
    {
        public DayOfWeek DayOfWeek { get; set; }

        public TimeOnly StartAt { get; set; }


        public TimeOnly EndAt { get; set; }
    }

  
}
