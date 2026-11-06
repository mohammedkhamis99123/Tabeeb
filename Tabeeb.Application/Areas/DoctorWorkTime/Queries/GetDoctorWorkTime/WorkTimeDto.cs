namespace Tabeeb.Application.Areas.DoctorWorkTime.Queries.GetDoctorWorkTime
{
    public class WorkTimeDto
    {
        public DayOfWeek DayOfWeek { get; set; }

        public TimeOnly StartAt { get; set; }


        public TimeOnly EndAt { get; set; }

        public bool IsActive { get; set; }

        public int DoctorID { get; set; }

        public int ClinicId { get; set; }
    }
}